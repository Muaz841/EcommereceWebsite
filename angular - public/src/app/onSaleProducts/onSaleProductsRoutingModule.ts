import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { onSaleProductsComponent } from './onSaleProductsComponent';

const routes: Routes = [
    {
        path: '',
        component: onSaleProductsComponent,
        pathMatch: 'full',
    },
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class onSaleProductsRoutingModule {}
