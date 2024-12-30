import { NgModule } from '@angular/core';
import {queuedEmailComponent} from './queued-emailcomponent';
import { queuedEmailRoutingModule } from './queued-email-routingModule';
import { SharedModule } from '../../shared/shared.module';
import { QueueEmailServiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { RatingModule } from 'primeng/rating';
import { CommonModule } from '@angular/common';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { FieldsetModule } from 'primeng/fieldset';




@NgModule({
    declarations: [queuedEmailComponent],
    imports: [ButtonModule,DialogModule,queuedEmailRoutingModule,
        SharedModule,CommonModule,RatingModule,TagModule,TableModule,FieldsetModule],
    providers: [QueueEmailServiceServiceProxy]
})
export class queuedEmailModule {}