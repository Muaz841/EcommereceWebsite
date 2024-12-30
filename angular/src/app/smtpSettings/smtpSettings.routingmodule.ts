import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SmtpSettingsComponent } from './smtpSettings.component';




const routes: Routes = [
    {
        path: '',
        component: SmtpSettingsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
    
})
export class SmtpSettingsRoutingModule {}
