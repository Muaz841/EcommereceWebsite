import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AboutComponent } from './about.component';
import { viewProductComponent} from './viewProduct/viewProductComponent';
import { CartComponent } from './cart/cartComponent';
import {ContactDetailsComponent} from './contactDetails/contact.component'

const routes: Routes = [
    {
        path: '',
        component: AboutComponent,
        pathMatch: 'full',
    },
    {
        path: 'viewProduct/:id',
        component: viewProductComponent,
        pathMatch: 'full',
    },
    {
        path: 'cart',
        component: CartComponent,
        pathMatch: 'full',
    },
    {
        path: 'contactdetails',
        component: ContactDetailsComponent,
        pathMatch: 'full',
    }
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AboutRoutingModule {}
