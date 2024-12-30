import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmailTemplatesComponent } from './email-templates.component';
import { TemplateEmailServiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { HostEmailTemplatesComponent } from './host-template-component';




const routes: Routes = [
    {
        path: 'email-Template',
        component: EmailTemplatesComponent,
        pathMatch: 'full',
        data: { breadcrumb: 'EmailTemplates' }
    },
    {
        path: 'host-Template',
        component: HostEmailTemplatesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
    providers: [TemplateEmailServiceServiceProxy]
})
export class emailTemplateRoutingModule {}
