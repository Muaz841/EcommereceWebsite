import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { newArrivalsComponent } from './newArrivalsComponent';


const routes: Routes = [
    {
        path: '',
        component: newArrivalsComponent,
        pathMatch: 'full',
    },
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class newArrivalsRoutingModule {}
