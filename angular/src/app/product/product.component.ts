import { ChangeDetectorRef, Component, Injector, OnInit } from "@angular/core";
import * as moment from "moment";
import {
  PagedListingComponentBase,
  PagedRequestDto,
} from "shared/paged-listing-component-base";
import {
  ProductServiceProxy,
  ProductDto,
  ProductDtoPagedResultDto,
} from "@shared/service-proxies/service-proxies";
import { finalize } from "rxjs";
import { log } from "node:console";
import { Router } from "@node_modules/@angular/router";

class PagedproductRequestDto extends PagedRequestDto {
  keyword: string;
  startDate: moment.Moment;
  endDate: moment.Moment;
}

@Component({
  templateUrl: "./product.component.html",
  styleUrls: ["./product.component.css"],
})
export class ProductComponent extends PagedListingComponentBase<ProductDto> {
  selectedButton: string = "all";
  selectedProductToDelete: ProductDto[] = [];
  allSelected: boolean = false;
  sortby = " ";
  keyword = " ";
  allProducts: ProductDto[] = [];
  publishedProducts: ProductDto[] = [];
  draftProducts: ProductDto[] = [];
  lowstockProducts: ProductDto[] = [];
  previewimage: string | ArrayBuffer | null;
  selectedFile: File | null;
  dateRange: Date[] = [];
  filteredProducts: ProductDto[] = [...this.allProducts];

  constructor(
    injector: Injector,
    private router: Router,
    private _productService: ProductServiceProxy,
    cd: ChangeDetectorRef
  ) {
    super(injector, cd);
  }

  protected list(
    request: PagedproductRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    if (this.dateRange && this.dateRange.length === 2) {
      request.startDate = moment(this.dateRange[0]);
      request.endDate = moment(this.dateRange[1]);
    } else {
      request.startDate = undefined;
      request.endDate = undefined;
    }

    request.keyword = this.keyword;
    this.publishedProducts = [];
    this.draftProducts = [];
    this.lowstockProducts = [];
    this._productService
      .getAll(
        request.keyword,
        request.startDate,
        request.endDate,
        this.sortby,
        request.skipCount,
        request.maxResultCount
      )
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: ProductDtoPagedResultDto) => {
        this.filteredProducts = result.items;
        this.allProducts = result.items;
        this.allProducts.forEach((product) => {
          if (!product.isActive) {
            this.publishedProducts.push(product);
          }
          if (product.lowStock && !product.isActive) {
            this.lowstockProducts.push(product);
          } else if (product.isActive) {
            this.draftProducts.push(product);
          }
        });
        this.showPaging(result, pageNumber);
      });
    this.dateRange = [];
  }

  onProductSelectChange(product: ProductDto, isChecked: boolean): void {
    if (isChecked) {
      this.selectedProductToDelete.push(product);
    } else {
      this.selectedProductToDelete = this.selectedProductToDelete.filter(p => p.id !== product.id);
    }
  }

  toggleAllCheckboxes(isChecked: boolean): void {
    this.allSelected = isChecked;
  
    if (isChecked) {      
      this.selectedProductToDelete = [...this.allProducts];
    } else {     
      this.selectedProductToDelete = [];
    }
  
    console.log(this.selectedProductToDelete);
  }
  protected delete(entity: ProductDto): void {
    const idsToDelete: number[] = Array.isArray(entity.id)
      ? entity.id
      : [entity.id];
    abp.message.confirm(
      "Are you sure you want to delete",
      undefined,
      (result: boolean) => {
        if (result) {
          this._productService.delete(idsToDelete).subscribe((response) => {
            abp.notify.success("Successfully deleted ");
            this.refresh();
          });
        }
      }
    );
  }

  protected deleteList(): void {    
    const idsToDelete: number[] = this.selectedProductToDelete.map(product => product.id);
      
    if (idsToDelete.length === 0) {
      abp.notify.error("No products selected for deletion.");
      return;
    }
  
    abp.message.confirm(
      "Are you sure you want to delete the selected products?",
      undefined,
      (result: boolean) => {
        if (result) {
          this._productService.delete(idsToDelete).subscribe((response) => {
            abp.notify.success("Successfully deleted selected products.");
          this.selectedProductToDelete = []; 
            this.refresh();
          });
        }
      }
    );
  }
  

  protected ExportFile(): void {
    this._productService.getExcelFile().subscribe((response) => {
      abp.notify.success("File Saved ");
      this.refresh();
    });
  }

  protected editProduct(entity: ProductDto): void {
    this.router.navigate(["app/products/edit-product", entity.id]);
  }

  protected viewProduct(entity: ProductDto): void {
    this.router.navigate(["app/products/view-product", entity.id, "1"]);
  }

  getStatusLabel(product: any): string {
    if(product.isActive) {
      return "DRAFT"}
    else if (product.lowStock) {
      return "LOW STOCK";
    } else if (product.isActive === false) {
      return "PUBLISHED";
    } 
    
  }

  getStatusColor(product: any): string {
    if(product.isActive)
    {
      return "#313131";
    }
   else if (product.lowStock) {
      return "#ED4765";
    } else if (product.isActive === false) {
      return "#EEA520";
    } 
  }

  getStatusClass(product: any): string {
    if (product.lowStock) {
      return "status-lowstock";
    } else if (product.isActive) {
      return "status-draft";
    }else if (product.isActive === false) {
      return "status-published";
    } 
  }

  selectButton(button: string): void {
    this.selectedButton = button;
    switch (button) {
      case "all":
        this.filteredProducts = [...this.allProducts];
        break;
      case "published":
        this.filteredProducts = [...this.publishedProducts];
        break;
      case "lowStock":
        this.filteredProducts = [...this.lowstockProducts];
        break;
      case "draft":
        this.filteredProducts = [...this.draftProducts];
        break;
      default:
        this.filteredProducts = [...this.allProducts];
        break;
    }
  }

  onDateSearch(): void {
    if (this.dateRange && this.dateRange.length === 2) {
      this.getDataPage(1);
    }
  }
  onsearch() {
    this.getDataPage(1);
  }
}
