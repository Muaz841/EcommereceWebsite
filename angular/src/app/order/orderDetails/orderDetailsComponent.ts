import {
  ChangeDetectorRef,
  Component,
  Injector,
  OnInit,
  Renderer2,
} from "@angular/core";
import { AppComponentBase } from "../../../shared/app-component-base";
import {
  OrderServicesServiceProxy,
  OrderDetailsDto,
  OrderProductDto,
} from "@shared/service-proxies/service-proxies";
import { ActivatedRoute } from "@node_modules/@angular/router";

@Component({
  templateUrl: "./orderdDetailsView.html",
  styleUrls: ["./orderdDetailsStyle.css"],
})
export class OrderDetailsComponent extends AppComponentBase implements OnInit {
  orderDetails: OrderDetailsDto = new OrderDetailsDto();
  subtotal: number;
  total: number;
  totalproducts: number;

  constructor(
    injector: Injector,
    private _orderservice: OrderServicesServiceProxy,
    private cdr: ChangeDetectorRef,
    private route: ActivatedRoute
  ) {
    super(injector);
  }

  ngOnInit(): void {
    const orderId = Number(this.route.snapshot.paramMap.get("id"));
    this._orderservice
      .orderByID(orderId)
      .subscribe((result: OrderDetailsDto) => {
        this.orderDetails = result;
        this.cdr.detectChanges();
        this.totalproducts = this.orderDetails.orderProducts.length;
        this.calculateTotals();
        this.cdr.detectChanges();
      });
  }

  calculateproductstotal(product: OrderProductDto) {
    return product.quantity * product.productPrice;
  }

  calculateTotals() {
    this.subtotal = this.orderDetails.orderProducts.reduce(
      (acc, item) => acc + item.productPrice * item.quantity,
      0
    );
    this.total = this.subtotal + 20;
  }
}
