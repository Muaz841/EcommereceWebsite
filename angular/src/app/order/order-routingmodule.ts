import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrderComponent } from './order.component';
import {OrderDetailsComponent} from './orderDetails/orderDetailsComponent'

const routes: Routes = [
    {
        path: '',
        component: OrderComponent,
        pathMatch: 'full',
        data: { breadcrumb: 'Orders' }
    }, 
    {
        path: 'orderdDetails/:id',
        component: OrderDetailsComponent,
        pathMatch: 'full',
        data: { breadcrumb: 'OrdersDetails' }
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class OrdersRoutingModule {}
