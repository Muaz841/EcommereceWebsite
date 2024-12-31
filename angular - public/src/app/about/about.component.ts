import { ChangeDetectorRef, Component, Injector, OnInit } from "@angular/core";
import { Router } from "@node_modules/@angular/router";

import { AppComponentBase } from "@shared/app-component-base";
import { EventTriggerServiceComponent } from "@shared/eventTriggerServiceComponent";
import {
  PublicSiteServiceProxy,
  PublicProductDto,
  ProductReviewsDto,  CartDto,
} from "@shared/service-proxies/service-proxies";
import { AppSessionService } from "@shared/session/app-session.service";
@Component({
  templateUrl: "./about.component.html",
  styleUrls: ["./about-style.css"],
})
export class AboutComponent extends AppComponentBase {
  lastestproduct: PublicProductDto[] = [];
  mainproduct: PublicProductDto = new PublicProductDto();
  customerReviews: ProductReviewsDto[] = [];

  constructor(
    private _cartRefreshService: EventTriggerServiceComponent,
    injector: Injector,
    private _sessionService: AppSessionService,
    private router: Router,
    private _productServie: PublicSiteServiceProxy,
    private cd: ChangeDetectorRef
  ) {
    super(injector);
  }
  ngOnInit(): void {
    this._productServie.getLatest().subscribe(
      (result) => {
        this.lastestproduct = result;
        this.cd.detectChanges();
      },
      () => abp.notify.error("Error fetching latest products")
    );
    this._productServie.getMainProduct().subscribe(
      (result) => {
        this.mainproduct = result;
        console.log(this.mainproduct);

        this.cd.detectChanges();
      },
      () => abp.notify.error("Error fetching main products")
    );

    this._productServie.getReviews().subscribe(
      (result) => {
        this.customerReviews = result;
        this.cd.detectChanges();
      },
      () => abp.notify.error("Error fetching main products")
    );
  }

  protected viewProduct(entity: number): void {
    this.router.navigate(["app/about/viewProduct", entity]);
  }

  addToCart(pID: number): void {
      if (!this._sessionService.user) {
        this.router.navigate(["/account/login"]);
      } else {
        const cart = new CartDto();
        {
          cart.productID = pID;
          cart.quantity = 1;
          cart.userID = this._sessionService.userId;
        }
        this._productServie.addToCart(cart).subscribe(
          (response) => {
            abp.notify.success(
              `Successfully added  product to the cart.`
            );
            this.router.navigate(["./app/about/cart"]);
            this._cartRefreshService.notifyCartUpdate();
          },
          (error) => {
            abp.notify.error("Error adding product to the cart.");
          }
        );
      }
    }
}
