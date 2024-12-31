import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReviewScreenComponent } from './reviewScreenComponent';
import { ThanksScreenComponent } from './thanksScreen/thanksScreenComponent';


const routes: Routes = [
    {
        path: 'reviewScreen/:id',
        component: ReviewScreenComponent,
        pathMatch: 'full',
    },
    {
        path: 'thank-you',
        component: ThanksScreenComponent,
        pathMatch: 'full',
    },
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class reviewScreenRoutingModule {}
