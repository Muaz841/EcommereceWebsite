import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReviewScreenComponent } from './reviewScreenComponent';


const routes: Routes = [
    {
        path: 'reviewScreen/:id',
        component: ReviewScreenComponent,
        pathMatch: 'full',
    },
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class reviewScreenRoutingModule {}
