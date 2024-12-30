import { ChangeDetectorRef, Component, Injector, OnInit } from '@angular/core';
import { CategoryServiceServiceProxy, CategoryDto, CategoryDtoPagedResultDto } from '../../shared/service-proxies/service-proxies';
import { finalize } from 'rxjs';
import { PagedListingComponentBase, PagedRequestDto } from '../../shared/paged-listing-component-base';
import { Router } from '@node_modules/@angular/router';

class PagedCetegoiresRequestDto extends PagedRequestDto {
  keyword: string;
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
    this._categoryService.getAll(
      request.keyword,
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



    protected editcategory(entity: CategoryDto): void {
      this.router.navigate(["app/categories/edit-category", entity.id]);
    }
  
    protected viewCategory(entity: CategoryDto): void {
      this.router.navigate(["app/categories/view-category", entity.id, "1"]);
    }
  
  }

  
 
 
 


