import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import {
  ProductServiceProxy,
  CreateUpdateProductDto,
  ProductMediaDTO,
  DiscountType,
  CategoryListDto,
  CategoryServiceServiceProxy,
} from "@shared/service-proxies/service-proxies";
import { Router } from "@angular/router";

@Component({
  selector: "product-create",
  templateUrl: "./create.product.html",
  styleUrls: ["./create.product.css"],
})
export class CreateProductComponent implements OnInit {
  productDto: CreateUpdateProductDto = new CreateUpdateProductDto();
  categories: CategoryListDto[] = [];
  thumbnail: File | null = null;
  previewThumbnail: string | ArrayBuffer | null = null;
  images: File[] = [];
  previewImages: string[] = [];
  productId: number;
  isViewMode: boolean = false;
  flag: number;
  discounts: DiscountType[] = [];
  availableTags = [
    { id: 1, name: "Watch" },
    { id: 2, name: "Gadget" },
    { id: 3, name: "Electronics" },
    { id: 4, name: "Mobile" },
    { id: 5, name: "Accessories" },
  ];

  selectedTags = [];
  constructor(
    public _productService: ProductServiceProxy,
    public _categoryService: CategoryServiceServiceProxy,
    private route: ActivatedRoute,
    private router: Router,
    public cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.productId = Number(this.route.snapshot.paramMap.get("id"));
    this.productDto.id = this.productId || 0;
    this.flag = Number(this.route.snapshot.paramMap.get("check"));
    this.isViewMode = this.flag ? this.flag === 1 : false;

    this._productService.getDiscountTypes().subscribe((result) => {
      this.discounts = result;
    });

    this._categoryService.getList().subscribe((result) => {
      this.categories = result;

      if (this.productDto.id) {
        this._productService.getById(this.productDto.id).subscribe(
          (product) => {
            Object.assign(this.productDto, product);
            this.productDto.images = product.images || [];
            this.previewThumbnail = `data:image/jpeg;base64,${product.thumbnail}`;
            this.previewImages =
              product.images?.map(
                (media) => `data:image/jpeg;base64,${media.image}`
              ) || [];
            this.onDiscountTypeChange(product.discountId);
            this.cdr.detectChanges();
          },
          () => {
            abp.notify.error("Error fetching product details");
          }
        );
      }
    });
  }

  async submit(): Promise<void> {
    if (this.isViewMode) {
      return;
    }
    this.productDto.id = this.productId;
    if (this.thumbnail) {
      this.productDto.thumbnail = await this.convertFileToBase64(
        this.thumbnail
      );
    }

    // this.productDto.images = this.productDto.images.filter((image) =>
    //   this.previewImages.includes(`data:image/jpeg;base64,${image.image}`)
    // );

    this.productDto.images = this.productDto.images || [];
    // Convert new images to Base64 and create ProductMediaDTOs
    const newImages = await this.convertFilesToProductMediaDTO(this.images);
  
    // Combine existing and new images
    this.productDto.images = [...this.productDto.images, ...newImages];
  

    const request = this.productDto.id
      ? this._productService.update(this.productDto)
      : this._productService.create(this.productDto);

    request.subscribe(
      () => {
        abp.notify.success(
          this.productDto.id
            ? "Product updated successfully"
            : "Product created successfully"
        );
        this.router.navigate(["/app/products"]);
      },
      () => {
        abp.notify.error("Error saving product");
      }
    );
  }

  async convertFileToBase64(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = () => {
        const result = reader.result as string;

        const base64String = result.split(",")[1];
        resolve(base64String);
      };
      reader.onerror = (error) => reject(error);
      reader.readAsDataURL(file);
    });
  }

  async processImages() {
    const imagesBase64 = [];
    for (let item of this.images) {
      const base64 = await this.convertFileToBase64(item);
      imagesBase64.push(base64);
    }
    return imagesBase64;
  }

  async convertFilesToProductMediaDTO(
    files: File[]
  ): Promise<ProductMediaDTO[]> {
    return Promise.all(
      files.map(async (file) => {
        const productMedia = new ProductMediaDTO();
        productMedia.image = await this.convertFileToBase64(file);
        productMedia.imagename = file.name;
        productMedia.productId = this.productDto.id;
        productMedia.tenantId = 1;
        return productMedia;
      })
    );
  }

  onThumbnailChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      this.thumbnail = input.files[0];
      this.previewThumbnail = URL.createObjectURL(this.thumbnail);
    }
  }
  onPriceInput(event: Event) {
    const inputElement = event.target as HTMLInputElement;      
    const value = Number(inputElement.value);      
    if (value < 1) {      
      abp.message.warn("price value cannot less than 1");
      this.productDto.basePrice = 1;  
      inputElement.value = "1";    
      this.cdr.detectChanges();
      console.log(  this.productDto.basePrice);
      
    }
  }

  onImageUpload(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      Array.from(input.files).forEach((file) => {
        this.images.push(file);
        this.previewImages.push(URL.createObjectURL(file));
      });
    }
  }
  onDiscountTypeChange(discountId: number): void {    
    const selectedDiscount = this.discounts.find((d) => d.id === discountId);
    if (selectedDiscount) {
      this.productDto.discountPercentage = selectedDiscount.discountPercent;
    } else {
      this.productDto.discountPercentage = null;
    }
  }

  removeImage(index: number): void {
    // Remove from previewImages
    const removedPreviewImage = this.previewImages.splice(index, 1)[0];
  
    // Remove from images (newly uploaded files)
    this.images.splice(index, 1);
  
    // Remove from productDto.images if it exists
    const removedBase64 = removedPreviewImage.split(",")[1];
    this.productDto.images = this.productDto.images.filter(
      (image) => image.image !== removedBase64
    );
  }
  
}
