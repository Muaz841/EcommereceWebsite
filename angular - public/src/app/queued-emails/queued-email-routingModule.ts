import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { queuedEmailComponent as QueuedEmailComponent } from './queued-emailcomponent';


const routes: Routes = [
    {
        path: '',
        component: QueuedEmailComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class queuedEmailRoutingModule {}
