import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { AboutRoutingModule } from './about-routing.module';
import { AboutComponent } from './about.component';
import { CommonModule } from '@angular/common';
import { OrderServicesServiceProxy, PublicSiteServiceProxy, StripePaymentServiceProxy } from '@shared/service-proxies/service-proxies';
import { viewProductComponent } from './viewProduct/viewProductComponent';
import {CartComponent} from './cart/cartComponent'
import { ContactDetailsComponent } from './contactDetails/contact.component';
import { RatingModule } from 'primeng/rating';
import { ModalModule } from 'ngx-bootstrap/modal'; 
import {ProductReviewDialogComponent} from './productReview/productReviewComponent'

@NgModule({
    declarations: [AboutComponent,ProductReviewDialogComponent,viewProductComponent,CartComponent,ContactDetailsComponent],
    imports: [SharedModule,  ModalModule.forRoot(),AboutRoutingModule,RatingModule,CommonModule],
    providers: [PublicSiteServiceProxy,StripePaymentServiceProxy,OrderServicesServiceProxy]
})
export class AboutModule {}
