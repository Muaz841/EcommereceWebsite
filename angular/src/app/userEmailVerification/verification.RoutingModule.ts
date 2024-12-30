import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { verificationComponent } from './verification.Component';
import {ApiServiceProxy} from '@shared/service-proxies/service-proxies';


const routes: Routes = [
    {
        path: 'verify/:id/:token',
        component: verificationComponent,
        pathMatch: 'full',        
        
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
    providers: [ApiServiceProxy]
})
export class verificatioRoutingModulemodule {}
