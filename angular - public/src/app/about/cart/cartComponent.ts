import { ChangeDetectorRef, Component, Injector, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { PublicSiteServiceProxy , CartQuantityDto, CartDto} from "../../../shared/service-proxies/service-proxies";
import { AppComponentBase } from "../../../shared/app-component-base";
import { AppSessionService } from "../../../shared/session/app-session.service"; 
@Component({
  templateUrl: './cartView.html',
  styleUrls: ['./cartStyle.css'],
})
export class CartComponent implements OnInit {
    products: CartDto[] = [];
    selectedQuantity: number = 1;
    userId: number;
  
    constructor(
      injector: Injector,
      private router: Router,
      private route: ActivatedRoute,
      private _productServie: PublicSiteServiceProxy,
      private cd: ChangeDetectorRef,
      private _sessionService: AppSessionService
    ) {}
  
    ngOnInit(): void {      
      this._productServie.getCart(this._sessionService.userId).subscribe(
        (cartItems: CartDto[]) => {
          this.products = cartItems; 
          this.cd.detectChanges();          
        },
        () => {
          abp.notify.error("Error fetching product details");
        }
      );
    }
  
    increaseQuantity(item: CartDto): void {      
       var data = new CartQuantityDto();
       data.cartId = item.cartID;
       data.quantitychange = 1;
       this._productServie.productQuantityControl(data).subscribe(() => {
        item.quantity++;
        this.cd.detectChanges(); 
      });
    
    }
  
    decreaseQuantity(item: CartDto): void {
      if (item.quantity > 1) {
        var data = new CartQuantityDto();
        data.cartId = item.cartID;
        data.quantitychange = -1;
            this._productServie.productQuantityControl(data).subscribe(() => {
        item.quantity--;
        this.cd.detectChanges(); 
      });
    }
    }

    calculateSubtotal(): number {
        return this.products.reduce((sum, item) => sum + item.products.basePrice * item.quantity, 0) ;
      }
      
      // calculateDiscount(): number {
      //   return this.calculateSubtotal() * 0.2; 
      // }
      
      calculateTotal(): number {
        return this.calculateSubtotal()  +15 ;
      }

      removeProductFromCart(pID: number): void {
       this._productServie.removeFromCart(this._sessionService.userId, pID).subscribe(
        (response) => 
        {
          abp.notify.success(`Item Removed From Cart`);
          window.location.reload();  
        },
        (error) => {         
          abp.notify.error('Error Removing product From  cart.');
        }
       )
        }
      
  }
  
