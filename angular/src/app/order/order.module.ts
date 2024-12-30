import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { OrdersRoutingModule } from './order-routingmodule';
import { OrderComponent } from './order.component';
import { CardModule } from 'primeng/card';
import { FormsModule } from '@angular/forms';
import { SelectButtonModule } from 'primeng/selectbutton';
import { TableModule } from 'primeng/table';
import { CommonModule } from '@angular/common';
import { InputIconModule } from 'primeng/inputicon';
import { IconFieldModule } from 'primeng/iconfield';
import { InputTextModule } from 'primeng/inputtext';
import { CalendarModule } from 'primeng/calendar';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { FileUploadModule } from 'primeng/fileupload';
import { ToastModule } from 'primeng/toast';
import { PaginatorModule } from 'primeng/paginator';
import { DropdownModule } from 'primeng/dropdown';
import { OrderServicesServiceProxy } from '@shared/service-proxies/service-proxies';
import {OrderDetailsComponent} from './orderDetails/orderDetailsComponent';
import {orderStatusModalComponent} from './orderStatusModal/orderStatusModalComponent'
@NgModule({
    declarations: [
        OrderComponent, OrderDetailsComponent ,orderStatusModalComponent
    ],
    imports: [
        SharedModule, 
        OrdersRoutingModule,
        CardModule,
        FormsModule,
        TableModule,        
        DropdownModule,
        CommonModule,
        InputIconModule,
        IconFieldModule,
        InputTextModule,
        CalendarModule,
        SelectButtonModule,
        InputTextareaModule,
        FileUploadModule,
        ToastModule,
        PaginatorModule
    ],
    providers: [OrderServicesServiceProxy]
})
export class OrdersModule {}