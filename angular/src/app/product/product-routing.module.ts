import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductComponent } from './product.component';
import { CreateProductComponent } from './create_product/create.product.component';

const routes: Routes = [
    {
        path: '',
        component: ProductComponent,
        pathMatch: 'full',
         data: { breadcrumb: 'Products' }
         
    },
    {
        path: 'create-product',
        component: CreateProductComponent,
        pathMatch: 'full',
         data: { breadcrumb: 'CreateProduct' }
    },
    {
        path: 'edit-product/:id',
        component: CreateProductComponent,
        pathMatch: 'full',
         data: { breadcrumb: 'EditProduct' }
    }, 
    {
        path: 'view-product/:id/:check',
        component: CreateProductComponent,
        pathMatch: 'full',
         data: { breadcrumb: 'View Product' }
    }, 
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductsRoutingModule {}
