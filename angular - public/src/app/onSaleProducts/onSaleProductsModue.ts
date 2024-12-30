import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { CommonModule } from '@angular/common';
import { PublicSiteServiceProxy } from '../../shared/service-proxies/service-proxies';
import { RatingModule } from 'primeng/rating';
import { ModalModule } from 'ngx-bootstrap/modal'; 
import { onSaleProductsRoutingModule } from './onSaleProductsRoutingModule';
import { onSaleProductsComponent } from './onSaleProductsComponent';


@NgModule({
    declarations: [onSaleProductsComponent],
    imports: [SharedModule,  ModalModule.forRoot(),onSaleProductsRoutingModule,RatingModule,CommonModule],
    providers: [PublicSiteServiceProxy]
})
export class onSaleProductsModule {}
