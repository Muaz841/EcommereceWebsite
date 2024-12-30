import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { CommonModule } from '@angular/common';
import { PublicSiteServiceProxy } from '../../shared/service-proxies/service-proxies';
import { RatingModule } from 'primeng/rating';
import { ModalModule } from 'ngx-bootstrap/modal'; 
import { newArrivalsRoutingModule } from './newArrivalsRoutingModule';
import { newArrivalsComponent } from './newArrivalsComponent';


@NgModule({
    declarations: [newArrivalsComponent],
    imports: [SharedModule,  ModalModule.forRoot(),newArrivalsRoutingModule,RatingModule,CommonModule],
    providers: [PublicSiteServiceProxy]
})
export class newArrivalssModule {}
