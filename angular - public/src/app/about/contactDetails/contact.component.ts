import { ChangeDetectorRef, Component, Injector, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import {
  PublicSiteServiceProxy,
  ContactDetailsDto,
  CartDto,
  OrderDto,
  StripePaymentServiceProxy,
  OrderProductDto,
  OrderServicesServiceProxy,
} from "../../../shared/service-proxies/service-proxies";
import { AppSessionService } from "../../../shared/session/app-session.service";
import {
  loadStripe,
  Stripe,
  StripeCardElement,
  StripeElements,
} from "@stripe/stripe-js";
import { loadavg } from "node:os";

@Component({
  templateUrl: "./contact.html",
  styleUrls: ["./contact-style.css"],
})
export class ContactDetailsComponent implements OnInit {
  contactDetails: ContactDetailsDto;
  shippingAddress: string;
  cartProducts: CartDto[] = [];
  itemsCount: number;
  isOver13: boolean = false;
  isCardReady: boolean = false;
  isSameBillingAndDelivery: boolean = false;
  loading: boolean = false;
  stripePublicKey: string =
    "pk_test_51QXJQGA1edLUrgbPtq2qiI2QkjFXa3O1ONHjBXiRTEXZHhFG5NNgTsIvoRLJVh1DiT9IJVfU9R5zz70NB5hPTcc600aIUiYes7";
  private stripe: Stripe | null = null;
  clientSecret: string | undefined;
  private elements: StripeElements | null = null;
  private card: StripeCardElement | null = null;

  constructor(
    injector: Injector,
    private router: Router,
    private route: ActivatedRoute,
    private cd: ChangeDetectorRef,
    private _productServices: PublicSiteServiceProxy,
    private _sessionService: AppSessionService,
    private _stripeService: StripePaymentServiceProxy,
    private _orderService: OrderServicesServiceProxy
  ) {}

  async ngOnInit(): Promise<void> {
    this._productServices
      .contactDetails(this._sessionService.userId)
      .subscribe((result) => {
        this.contactDetails = result;
        this.cd.detectChanges();
      });

    this._productServices.getCart(this._sessionService.userId).subscribe(
      (cartItems: CartDto[]) => {
        this.cartProducts = cartItems;
        this.itemsCount = cartItems.length;
        this.cd.detectChanges();
      },
      () => {
        abp.notify.error("Error fetching product details");
      }
    );

    const stripeInstance = await loadStripe(this.stripePublicKey);
    if (stripeInstance) {
      this.stripe = stripeInstance;
      this.elements = stripeInstance.elements();
      this.card = this.elements.create("card");
      this.card.mount("#card-element");
      this.card.on("change", (event) => {
        this.isCardReady = !event.empty && !event.error;
        this.cd.detectChanges();
      });
    }
    this.cd.detectChanges();
  }

  calculateSubtotal(): number {
    return this.cartProducts.reduce(
      (sum, item) => sum + item.products.basePrice * item.quantity,
      0
    );
  }

  calculateTotal(): number {
    return this.calculateSubtotal() + 6 + 71;
  }
  allCheckboxesChecked(): boolean {
    return this.isSameBillingAndDelivery && this.isOver13 && this.isCardReady;
  }

  submitPayment(): void {
    this.loading = true;
    if(!this.shippingAddress || this.shippingAddress.trim() === "" )
    {
      abp.notify.error("Shipping Address is missing")
      this.loading = false;
      return;
    }
    else if (!this.contactDetails.phoneNumber || this.contactDetails.phoneNumber.trim() === "")
      {
        abp.notify.error("Phone Number is missing")
        this.loading = false;
        return;
      }    
    this._stripeService
      .createPaymentIntent(
        this.calculateTotal(),
        this._sessionService.user.emailAddress
      )
      .subscribe(
        (result) => {
          if (!result) {
            this.loading = false;
            abp.notify.error("Stripe Is Not Working. Please try again.");
            return;
          }
          this.clientSecret = result;
          if (!this.stripe || !this.clientSecret || !this.card) {
            this.loading = false;
            abp.notify.error("Card is missing");
            return;
          }
          this.stripe
            .confirmCardPayment(this.clientSecret, {
              payment_method: {
                card: this.card,
                billing_details: {
                  email: this._sessionService.user.emailAddress,
                },
              },
            })
            .then(({ paymentIntent, error }) => {
              if (error) {
                this.loading = true;
                abp.notify.error("Payment failed", error.message);
              } else if (paymentIntent?.status === "succeeded") {
                abp.notify.success("Payment succeeded Thanks For Purchasing");
                this.loading = false;
                this.router.navigate(["./app/about"]);
                const order = new OrderDto();
                order.customerId = this._sessionService.userId;
                order.totalPrice = this.calculateTotal();
                order.shippingAddress = this.shippingAddress;
                order.phoneNumber = this.contactDetails.phoneNumber;
                order.orderProducts = this.cartProducts.map((cart) => {
                  const orderproduct = new OrderProductDto();
                  orderproduct.productId = cart.productID;
                  orderproduct.quantity = cart.quantity;
                  return orderproduct;
                });
                this._orderService.placeOrder(order).subscribe();
              }
            })
            .catch((error) => {
              this.loading = false;
              abp.notify.error("An error occurred", error.message);
            });
        },
        (error) => {
          this.loading = false;
          abp.notify.error("Failed to create payment intent", error.message);
        }
      );
      this.loading = false;
  }

  getStripe(): Stripe | null {
    return this.stripe;
  }
}
