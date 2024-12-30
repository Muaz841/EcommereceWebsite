import { NgModule } from '@angular/core';
import {emailTemplateRoutingModule} from './email-template-routingmodule'
import { SharedModule } from '@shared/shared.module';
import { EmailTemplatesComponent } from './email-templates.component';
import { FieldsetModule } from 'primeng/fieldset';
import { EditorModule } from 'primeng/editor';
import { TabViewModule } from 'primeng/tabview';
import { AccordionModule } from 'primeng/accordion';
import { TableModule } from 'primeng/table';
import { DropdownModule } from 'primeng/dropdown';
import { FormsModule } from '@angular/forms';
import { DialogModule } from 'primeng/dialog';
import { CommonModule } from '@angular/common';
import { FloatLabelModule } from 'primeng/floatlabel';
import { ButtonModule } from 'primeng/button';
import {CreateTemplatesComponent} from './createTemplate/createTemplateComponent.'
import {HostEmailTemplatesComponent} from './host-template-component'

@NgModule({
    declarations: [EmailTemplatesComponent, CreateTemplatesComponent, HostEmailTemplatesComponent],
    imports: [DialogModule,FormsModule,DropdownModule,TableModule,AccordionModule,CommonModule,ButtonModule,
        EditorModule,emailTemplateRoutingModule,SharedModule,FieldsetModule,TabViewModule,FloatLabelModule],

})
export class emailtemplatemodule {}