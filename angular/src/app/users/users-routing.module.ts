import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsersComponent } from './users.component';
import { UserDetailsComponent } from './userDetails/userDetailsComponent';

const routes: Routes = [
    {
        path: '',
        component: UsersComponent,
        pathMatch: 'full',        
        data: { breadcrumb: 'Users' },
    },
    {
        path: 'userdetails/:id',
        component: UserDetailsComponent,
        pathMatch: 'full',        
        data: { breadcrumb: 'UserDetails' },
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class UsersRoutingModule {}
