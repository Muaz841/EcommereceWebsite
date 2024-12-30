import { ChangeDetectorRef, Component, Injector, OnInit } from '@angular/core';
import { CategoryServiceServiceProxy, CategoryDto, CategoryDtoPagedResultDto } from '../../shared/service-proxies/service-proxies';
import { finalize } from 'rxjs';
import { PagedListingComponentBase, PagedRequestDto } from '../../shared/paged-listing-component-base';
import { Router } from '@node_modules/@angular/router';
import * as moment from 'moment';

class PagedCetegoiresRequestDto extends PagedRequestDto {
  keyword: string;
    startDate: moment.Moment;
    endDate: moment.Moment;
}

@Component({
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css'],
})
export class CategoryComponent extends PagedListingComponentBase<CategoryDto> {
  selectedButton: string = 'all';  
  sortby = " ";
  keyword = " ";
  allCategories: CategoryDto[] = []; 
  previewimage: string | ArrayBuffer | null;
  selectedFile: File | null;
  dateRange: Date[] = [];

  constructor(
    injector: Injector,
        private router: Router,
    private _categoryService: CategoryServiceServiceProxy,
    cd: ChangeDetectorRef
  ) {
    super(injector, cd);
  }


  protected list(request: PagedCetegoiresRequestDto, pageNumber: number, finishedCallback: Function): void {
    request.keyword = this.keyword;
      if (this.dateRange && this.dateRange.length === 2) {
          request.startDate = moment(this.dateRange[0]); 
          request.endDate = moment(this.dateRange[1]);   
        } else {
          request.startDate = undefined;
          request.endDate = undefined;
        }    
    this._categoryService.getAll(
      request.keyword,
      request.startDate,
      request.endDate,
      this.sortby,
      request.skipCount,
      request.maxResultCount
    ).pipe(
      finalize(() => {
        finishedCallback();
      })
    )
    .subscribe((result: CategoryDtoPagedResultDto) => {

      this.allCategories = result.items;      
      this.allCategories.forEach(product => {
      
      });            
      this.showPaging(result, pageNumber);   
      this.dateRange = [];
    });
  }  

  protected delete(entity: CategoryDto): void {
   
   }
  
  protected editProduct(entity: CategoryDto): void {
    throw new Error('Method not implemented.');
  }
  
  protected viewProduct(entity: CategoryDto): void {
    throw new Error('Method not implemented.');
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      const reader = new FileReader();
 
      reader.onload = (e: any) => {
        this.previewimage = e.target.result;
      };
 
      reader.readAsDataURL(file);
    }
  }
  onsearch()
  {
    debugger;
      this.getDataPage(1);
  }
  protected ExportFile(): void {
    this._categoryService.getExcelFile().subscribe((response) => {
      abp.notify.success("File Saved ");
      this.refresh();
    });
  }


  onDateSearch(): void {  
    if (this.dateRange && this.dateRange.length === 2) {this.getDataPage(1);} }  
    
    

    protected editcategory(entity: CategoryDto): void {
      this.router.navigate(["app/categories/edit-category", entity.id]);
    }
  
    protected viewCategory(entity: CategoryDto): void {
      this.router.navigate(["app/categories/view-category", entity.id, "1"]);
    }
  
  }

  
 
 
 


