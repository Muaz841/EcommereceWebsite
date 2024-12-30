import { Component, ChangeDetectionStrategy, Injector, ChangeDetectorRef, } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { AppAuthService } from '@shared/auth/app-auth.service';
import {AbpSessionService } from 'abp-ng2-module'
import {
  RoleDto,
  UserServiceProxy,} from '@shared/service-proxies/service-proxies';
import { AppSessionService,  } from '@shared/session/app-session.service';
import { Router } from '@node_modules/@angular/router';
import { PublicSiteServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'header-user-menu',
  templateUrl: './header-user-menu.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  styleUrls: ['./side-menu-style.css']
})
export class HeaderUserMenuComponent extends AppComponentBase {
  userRoles: RoleDto[] = [];
  cartCount: number;
  constructor(
    private _publicSite: PublicSiteServiceProxy,
    private _authService: AppAuthService, 
     private _usersessionService: AppSessionService,
    injector: Injector,  
    private _userService: UserServiceProxy,
    private router: Router,    
    private cdr: ChangeDetectorRef )
     {super(injector);
      
     }
  shownLoginName = '';  
  logout(): void {
    this._authService.logout();
  }

  ngOnInit() {
    this.shownLoginName = this.appSession.getShownLoginName();  
    this._publicSite.cartCount(this._usersessionService.userId).subscribe((result) => this.cartCount = result);
    this.cdr.detectChanges();
    this._userService.getRoles().subscribe((result) => {
      this.userRoles = result.items;           
      this.cdr.detectChanges();
    });             
  }

  ToCart(): void {   
   
      if (!this._usersessionService.user) {      
        this.router.navigate(['/account/login']);       
        
      } 
      else
      {
        this.router.navigate(['./app/about/cart' ]);
      }
    
}
}
