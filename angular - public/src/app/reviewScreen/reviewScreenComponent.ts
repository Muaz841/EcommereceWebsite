import { ChangeDetectorRef, Component, Injector, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { PublicSiteServiceProxy , PublicProductDto,CreateUpdateProductDto, CartDto} from "../../shared/service-proxies/service-proxies";
import { AppSessionService } from "../../shared/session/app-session.service"; 
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal'; 
@Component({
  templateUrl: './reviewScreenView.html',
  styleUrls: ['./reviewScreenStyle.css'],
})
export class reviewScreenComponent implements OnInit {
  allProducts: PublicProductDto[] = [];
  constructor(
    injector: Injector,
    private router: Router,
    private route: ActivatedRoute,
    private _productServie: PublicSiteServiceProxy, 
    private cd: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this._productServie.newArrivals().subscribe((result) => {this.allProducts = result;
      this.cd.detectChanges();            
     },
   () => abp.notify.error('Error products')
  )    
   };
   protected viewProduct(entity: number): void {      
    this.router.navigate(['app/about/viewProduct', entity]);
  }
}
