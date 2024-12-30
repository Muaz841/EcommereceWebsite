import { NgModule } from '@angular/core';
import {SharedModule} from '../../shared/shared.module';
import {ProductComponent} from './product.component'
import {ProductsRoutingModule} from './product-routing.module'
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { PageModule,PageParts,PageRenderStrategy } from '@abp/ng.components/page';
import { FileUploadModule } from 'primeng/fileupload';
import {CreateProductComponent} from './create_product/create.product.component';
import {  ProductServiceProxy, CategoryServiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';
import { SelectButtonModule } from 'primeng/selectbutton';
import { InputIconModule } from 'primeng/inputicon';
import { IconFieldModule } from 'primeng/iconfield';
import { CalendarModule } from 'primeng/calendar';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { ToastModule } from 'primeng/toast';
import { PaginatorModule } from 'primeng/paginator';
import { ReactiveFormsModule } from '@angular/forms';
import { TagModule } from 'primeng/tag';
import { MessageService } from 'primeng/api';
import { MultiSelectModule } from 'primeng/multiselect';
import { ToggleButtonModule } from 'primeng/togglebutton';






@NgModule({
    declarations: [ProductComponent,CreateProductComponent],
    imports: [CalendarModule,SharedModule,InputTextModule,FormsModule,IconFieldModule,DropdownModule,SelectButtonModule,
        InputIconModule,FormsModule,TagModule, PaginatorModule,ReactiveFormsModule, ProductsRoutingModule,InputTextareaModule,
        MultiSelectModule, CommonModule,TableModule,FileUploadModule, ToggleButtonModule,PageModule,ButtonModule,ToastModule,CardModule],
    providers: [ProductServiceProxy, CategoryServiceServiceProxy,MessageService]
})
export class ProductModule {}