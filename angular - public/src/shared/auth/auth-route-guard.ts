import { Injectable } from '@angular/core';
import { PermissionCheckerService } from 'abp-ng2-module';
import { AppSessionService } from '../session/app-session.service';

import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

@Injectable()
export class AppRouteGuard  {

    constructor(
        private _permissionChecker: PermissionCheckerService,
        private _router: Router,
        private _sessionService: AppSessionService,
    ) { }

    
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        if (!this._sessionService.user) {
            this._router.navigate(['/account/login']);
            return false;
        }     
    }
    
    checkReviewStatus(): boolean {
        const isReviewSubmitted = this._sessionService.isReviewSubmitted();
    
        if (isReviewSubmitted) {
          
          this._router.navigate(['/app/reviewScreen/thank-you']);
          return false; 
        }    
        return true; 
      }


    canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        return this.canActivate(route, state);
    }

    selectBestRoute(): string {
        if (!this._sessionService.user) {
            return '/account/login';
        }

        if (this._permissionChecker.isGranted('Pages.Users')) {
            return '/app/admin/users';
        }

        return '/app/dashboard';
    }
}
