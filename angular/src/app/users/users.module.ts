import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { CreateUserDialogComponent } from './create-user/create-user-dialog.component';
import { EditUserDialogComponent } from './edit-user/edit-user-dialog.component';
import { ResetPasswordDialogComponent } from './reset-password/reset-password.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { UsersRoutingModule } from './users-routing.module';
import { UsersComponent } from './users.component';
import { CardModule } from 'primeng/card';
import { FormsModule } from '@angular/forms';
import { SelectButtonModule } from 'primeng/selectbutton';
import { TableModule } from 'primeng/table';
import { CommonModule } from '@angular/common';
import { InputIconModule } from 'primeng/inputicon';
import { IconFieldModule } from 'primeng/iconfield';
import { InputTextModule } from 'primeng/inputtext';
import { CalendarModule } from 'primeng/calendar';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { FileUploadModule } from 'primeng/fileupload';
import { ToastModule } from 'primeng/toast';
import { PaginatorModule } from 'primeng/paginator';
import {UserDetailsComponent} from './userDetails/userDetailsComponent'

@NgModule({
    declarations: [UsersComponent, UserDetailsComponent,ResetPasswordDialogComponent, EditUserDialogComponent, CreateUserDialogComponent, ChangePasswordComponent],
    imports: [FileUploadModule,PaginatorModule,ToastModule,InputTextModule,CalendarModule,InputTextareaModule,SharedModule,CardModule,FormsModule,IconFieldModule, SelectButtonModule,TableModule,UsersRoutingModule,CommonModule,InputIconModule],
})
export class UsersModule {}