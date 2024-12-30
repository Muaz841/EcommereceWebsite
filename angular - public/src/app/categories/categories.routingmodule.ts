import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoryComponent } from './categories.component';
import { CreateCategoryComponent } from './create-category/createCategory.component';

const routes: Routes = [
    {
        path: '',
        component: CategoryComponent,
        pathMatch: 'full',
         data: { breadcrumb: 'Categories' }         
    },
    {
        path: 'create-category',
        component: CreateCategoryComponent,
        pathMatch: 'full',
         data: { breadcrumb: 'AddCategories' }         
    },
    {
            path: 'edit-category/:id',
            component: CreateCategoryComponent,
            pathMatch: 'full',
             data: { breadcrumb: 'EditCategory' }
        }, 
        {
            path: 'view-category/:id/:check',
            component: CreateCategoryComponent,
            pathMatch: 'full',
             data: { breadcrumb: 'ViewCategory' }
        }, 
  
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CategoriesRoutingModule {}
