import { ChangeDetectorRef, Component, Injector, OnInit } from "@angular/core";
import { Router } from "@node_modules/@angular/router";

import { AppComponentBase } from "@shared/app-component-base";
import { PublicSiteServiceProxy, PublicProductDto, ProductReviewsDto } from "@shared/service-proxies/service-proxies";
@Component({
  templateUrl: "./about.component.html",
  styleUrls: ['./about-style.css'],
})
export class AboutComponent extends AppComponentBase {
lastestproduct: PublicProductDto[] = [];
mainproduct: PublicProductDto = new PublicProductDto();
customerReviews:  ProductReviewsDto[] = [];

  constructor(
    injector: Injector,
    private router: Router,
    private _productServie: PublicSiteServiceProxy, 
    private cd: ChangeDetectorRef 
  ) {
    super(injector);
  }
  ngOnInit(): void {
    this._productServie.getlatest().subscribe((result) => {this.lastestproduct = result;
      this.cd.detectChanges();            
     },
   () => abp.notify.error('Error fetching latest products')
  )       
  this._productServie.getMainProduct().subscribe((result) => {this.mainproduct = result;
    this.cd.detectChanges();            
   },
 () => abp.notify.error('Error fetching main products')
 
) 

        this._productServie.getReviews().subscribe((result) => {this.customerReviews = result;
          this.cd.detectChanges();            
        },
        () => abp.notify.error('Error fetching main products')

        ) 
   }
  

   
    protected viewProduct(entity: number): void {      
       this.router.navigate(['app/about/viewProduct', entity]);
     }
}
