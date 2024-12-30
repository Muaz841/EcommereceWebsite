import { NgModule } from '@angular/core';
import {SharedModule} from '../../shared/shared.module';
import {CategoriesRoutingModule} from './categories.routingmodule'
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { PageModule,PageParts,PageRenderStrategy } from '@abp/ng.components/page';
import { FileUploadModule } from 'primeng/fileupload';
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
import { CategoryServiceServiceProxy } from '../../shared/service-proxies/service-proxies';
import { CategoryComponent } from './categories.component';
import {CreateCategoryComponent} from './create-category/createCategory.component'




@NgModule({
    declarations: [CategoryComponent,CreateCategoryComponent],
    imports: [CalendarModule,SharedModule,InputTextModule,FormsModule,IconFieldModule,DropdownModule,SelectButtonModule,InputIconModule,
        FormsModule,  PaginatorModule,ReactiveFormsModule, CategoriesRoutingModule,InputTextareaModule,CommonModule,TableModule,FileUploadModule, PageModule,ButtonModule,ToastModule,CardModule],
    providers: [ CategoryServiceServiceProxy]
})
export class CategoryModule {}