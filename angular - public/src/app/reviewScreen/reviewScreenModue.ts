import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { CommonModule } from '@angular/common';
import { PublicSiteServiceProxy } from '../../shared/service-proxies/service-proxies';
import { RatingModule } from 'primeng/rating';
import { ModalModule } from 'ngx-bootstrap/modal'; 
import { reviewScreenRoutingModule } from './reviewScreenRoutingModule';
import { ReviewScreenComponent } from './reviewScreenComponent';


@NgModule({
    declarations: [ReviewScreenComponent],
    imports: [SharedModule,  ModalModule.forRoot(),reviewScreenRoutingModule,RatingModule,CommonModule],
    providers: [PublicSiteServiceProxy]
})
export class reviewScreenModule {}
