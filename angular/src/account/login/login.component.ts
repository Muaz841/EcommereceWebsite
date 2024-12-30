import { Component, Injector } from '@angular/core';
import { AbpSessionService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/app-component-base';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppAuthService } from '@shared/auth/app-auth.service';

@Component({
  templateUrl: './login.component.html',
  animations: [accountModuleAnimation()],  
})
export class LoginComponent extends AppComponentBase {
  submitting = false;
  value : string = 'Beginner';
  value2 : string = 'Beginner';
  value3 : string = 'Expert';
  options: any[] = [
    { label: 'Beginner', value: 'Beginner' },
    { label: 'Expert', value: 'Expert' },
];
  constructor(
    injector: Injector,
    public authService: AppAuthService,
    private _sessionService: AbpSessionService
  ) {
    super(injector);
  }

  get multiTenancySideIsTeanant(): boolean {
    return this._sessionService.tenantId > 0;
  }

  get isSelfRegistrationAllowed(): boolean {
    if (!this._sessionService.tenantId) {
      return false;
    }
   
    return true;
  }

  login(): void {    
    this.submitting = true;
    this.authService.authenticate(() => (this.submitting = false));
  }

  showTenantChange(): boolean {
    return abp.multiTenancy.isEnabled;
  }
  passwordVisible: boolean = false;
  togglePasswordVisibility() {
    this.passwordVisible = !this.passwordVisible;
  }
}
