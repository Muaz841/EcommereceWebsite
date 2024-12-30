 import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router'; 
import {  CategoryDto, CategoryServiceServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: './createCategory.component.html',
  styleUrls: ['./createCategory.css'],
})
export class CreateCategoryComponent implements OnInit {
  CategoryDto: CategoryDto = new CategoryDto(); 
  thumbnail: File | null = null;
  previewThumbnail: string | ArrayBuffer | null = null;
  categoryId: number;
  isViewMode: boolean = false;
  flag: number;

  constructor(   
    public _categoryService: CategoryServiceServiceProxy,
    private route: ActivatedRoute,
    public cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {  
     this.categoryId = Number(this.route.snapshot.paramMap.get('id'));
    this.CategoryDto.id = this.categoryId || 0;
    this.flag = Number(this.route.snapshot.paramMap.get('check'));
    this.isViewMode = this.flag ? this.flag === 1 : false;
    
      if (this.CategoryDto.id) {
        this._categoryService.getById(this.CategoryDto.id).subscribe(
          (result) => {
            Object.assign(this.CategoryDto, result);
            this.previewThumbnail = `data:image/jpeg;base64,${result.thumbnail}`;            
          this.cdr.detectChanges();
          },
          () => {
            abp.notify.error('Error fetching product details');
          }
        );
      }
    };

  

  async submit(): Promise<void> {
    if (this.isViewMode) {
      return; 
    }
    this.CategoryDto.id = this.categoryId;
  
    if (this.thumbnail) {
      this.CategoryDto.thumbnail = await this.convertFileToBase64(this.thumbnail);
    }
this.CategoryDto.creationtime = null;
       
    const request = this.CategoryDto.id
      ? this._categoryService.update(this.CategoryDto)
      : this._categoryService.create(this.CategoryDto);

    request.subscribe(
      () => {
  
        abp.notify.success(this.CategoryDto.id ? 'Product updated successfully' : 'Product created successfully');
  
        window.location.reload();  
      },
      () => {

        abp.notify.error('Error saving product');
      }
    );   
  }
  

  async convertFileToBase64(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = () => {
        const result = reader.result as string;
        
        const base64String = result.split(',')[1]; 
        resolve(base64String); 
      };
      reader.onerror = (error) => reject(error);
      reader.readAsDataURL(file);
    });
  }
  
  
  onDragOver(event: DragEvent): void {
    event.preventDefault();
  }
  
  onDrop(event: DragEvent): void {
    event.preventDefault();
    if (event.dataTransfer?.files && event.dataTransfer.files.length > 0) {
      this.thumbnail = event.dataTransfer.files[0];
      this.previewThumbnail = URL.createObjectURL(this.thumbnail);
    }
  }
  
  onThumbnailChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      this.thumbnail = input.files[0];
      this.previewThumbnail = URL.createObjectURL(this.thumbnail);
    }
  }
  
  removeThumbnail(): void {
    this.thumbnail = null;
    this.previewThumbnail = null;
  }
  
}
