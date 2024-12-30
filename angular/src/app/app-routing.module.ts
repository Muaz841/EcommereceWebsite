import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { AppComponent } from './app.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AppComponent,
                data: { breadcrumb: 'App' },
                children: [
                    {
                        path: 'dashboard',
                        loadChildren: () => import('./home/home.module').then((m) => m.HomeModule),
                        canActivate: [AppRouteGuard]
                    },
                    {
                        path: 'verification',
                        loadChildren: () => import('./userEmailVerification/verification.component.Module').then((m) => m.VerificationModule),                      
                    },
                    {
                        path: 'order',
                        loadChildren: () => import('./order/order.module').then((m) => m.OrdersModule),
                        canActivate: [AppRouteGuard]
                    },
                    {
                        path: 'about',
                        loadChildren: () => import('./about/about.module').then((m) => m.AboutModule),
                        canActivate: [AppRouteGuard]
                    },
                    {
                        path: 'users',
                        loadChildren: () => import('./users/users.module').then((m) => m.UsersModule),
                        data: { permission: 'Pages.Users' },
                        canActivate: [AppRouteGuard]
                    },
                    {
                        path: 'roles',
                        loadChildren: () => import('./roles/roles.module').then((m) => m.RolesModule),
                        data: { permission: 'Pages.Roles' },
                        canActivate: [AppRouteGuard]
                    },
                    {
                        path: 'products',
                        loadChildren: () => import('./product/product.module').then((m) => m.ProductModule),
                        data: { permission: 'Pages.Roles'  },
                        canActivate: [AppRouteGuard],
                        
                    },
                    {
                        path: 'categories',
                        loadChildren: () => import('./categories/categories.module').then((m) => m.CategoryModule),
                        data: { permission: 'Pages.Roles'  },
                        canActivate: [AppRouteGuard],
                        
                    },
                    {
                        path: 'tenants',
                        loadChildren: () => import('./tenants/tenants.module').then((m) => m.TenantsModule),
                        data: { permission: 'Pages.Tenants' },
                        canActivate: [AppRouteGuard]
                    },
                    {
                        path: 'update-password',
                        loadChildren: () => import('./users/users.module').then((m) => m.UsersModule),
                        canActivate: [AppRouteGuard]
                    },
                    {
                        path: 'email-Template',
                        loadChildren: () => import('./email-templates/email-template-module').then((m) => m.emailtemplatemodule),
                        canActivate: [AppRouteGuard]
                    },
                    {
                        path: 'queued-email',
                        loadChildren: () => import('./queued-emails/queued-email-module').then((m) => m.queuedEmailModule),
                        canActivate: [AppRouteGuard]
                    },
                    {
                        path: 'smtpSettings',
                        loadChildren: () => import('./smtpSettings/smtpSettings.module').then((m) => m.SmtpSettingmodule),
                        canActivate: [AppRouteGuard]
                    },
                ]
            }
        ])
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
