import { Component, ChangeDetectionStrategy, Injector, ChangeDetectorRef, } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { AppAuthService } from '@shared/auth/app-auth.service';
import {AbpSessionService } from 'abp-ng2-module'
import {
  RoleDto,
  UserServiceProxy,} from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'header-user-menu',
  templateUrl: './header-user-menu.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  styleUrls: ['./side-menu-style.css']
})
export class HeaderUserMenuComponent extends AppComponentBase {
  userRoles: RoleDto[] = [];
  constructor(
    private _authService: AppAuthService, 
    injector: Injector,  
    private _userService: UserServiceProxy,
    private cdr: ChangeDetectorRef )
     {super(injector);
      
     }


  shownLoginName = '';
  private _sessionService: AbpSessionService;
  logout(): void {
    this._authService.logout();
  }

  ngOnInit() {
    this.shownLoginName = this.appSession.getShownLoginName();      
    
    this._userService.getRoles().subscribe((result) => {
      this.userRoles = result.items;           
      this.cdr.detectChanges();
    });
    
    
  }
}
