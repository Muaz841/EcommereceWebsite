import { ChangeDetectorRef, Component, Injector, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { PublicSiteServiceProxy , CreateUpdateProductDto, CartDto} from "../../../shared/service-proxies/service-proxies";
import { AppSessionService } from "../../../shared/session/app-session.service"; 
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal'; 
import { ProductReviewDialogComponent } from "../productReview/productReviewComponent";
@Component({
  templateUrl: './viewProduct.html',
  styleUrls: ['./viewProductstyle.css'],
})
export class viewProductComponent implements OnInit {
product: CreateUpdateProductDto = new CreateUpdateProductDto();
Images: string[] = [];
selectedQuantity: number = 1;
productId: number;
  constructor(
    injector: Injector,
    private router: Router,
    private route: ActivatedRoute,
    private _productServie: PublicSiteServiceProxy, 
    private cd: ChangeDetectorRef ,
    private _modalService: BsModalService,
    private _sessionService: AppSessionService,
  ) {}

  ngOnInit(): void {
    this.productId = Number(this.route.snapshot.paramMap.get('id'));
    this._productServie.getById(this.productId).subscribe(
      (product) => {
        Object.assign(this.product, product); 
        this.Images = product.images?.map((media) => `data:image/jpeg;base64,${media.image}`) || [];  
        console.log(product.reviews);                                        
      this.cd.detectChanges();      
                
      },
      () => {
        abp.notify.error('Error fetching product details');
      }
    );          
   () => abp.notify.error('Error fetching latest products')     
   };
   

   addToCart(pID: number): void {      
    if (!this._sessionService.user) {      
      this.router.navigate(['/account/login']);
    } else { 

      const cart = new CartDto();
      {
        cart.productID = pID;
        cart.quantity = this.selectedQuantity;
        cart.userID = this._sessionService.userId;
      }     
      this._productServie.addToCart(cart).subscribe(
        (response) => {
          
          abp.notify.success(`Successfully added ${this.selectedQuantity} of the product to the cart.`);
          this.router.navigate(['./app/about/cart' ]);
        },
        (error) => {         
          abp.notify.error('Error adding product to the cart.');
        }
      );  
    }
  }

  increaseQuantity(): void {
    if (this.selectedQuantity < this.product.quantity) {
      this.selectedQuantity++;
    }
  }


 productReviewModal(product:CreateUpdateProductDto): void {
  const initialState = {
    productId: product.id
  };
  const createOrEditUserDialog: BsModalRef = this._modalService.show(ProductReviewDialogComponent, { initialState });
  this.cd.detectChanges();  
  }

  decreaseQuantity(): void {
    if (this.selectedQuantity > 1) {
      this.selectedQuantity--;
    }
  }
  
}
