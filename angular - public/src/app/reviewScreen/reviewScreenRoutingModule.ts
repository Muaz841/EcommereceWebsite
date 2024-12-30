import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { reviewScreenComponent } from './reviewScreenComponent';


const routes: Routes = [
    {
        path: '',
        component: reviewScreenComponent,
        pathMatch: 'full',
    },
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class reviewScreenRoutingModule {}
