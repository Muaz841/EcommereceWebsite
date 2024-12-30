import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';
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
import { ChartModule } from 'primeng/chart';
import { DashboardServiceServiceProxy, DashboardRevenueDto, OrderServicesServiceProxy } from '@shared/service-proxies/service-proxies';

@NgModule({
    declarations: [HomeComponent],
    imports: [SharedModule, InputTextareaModule, CardModule,ChartModule,PaginatorModule,CommonModule,TableModule,SelectButtonModule,
        FormsModule,  ToastModule,FileUploadModule, HomeRoutingModule,CalendarModule,InputTextModule,IconFieldModule,InputIconModule],
        providers: [DashboardServiceServiceProxy,OrderServicesServiceProxy]
})
export class HomeModule {}
