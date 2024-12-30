import {
    Component,
    Injector,
    OnInit,
    EventEmitter,
    Output,
    ChangeDetectorRef
  } from '@angular/core';
  import { BsModalRef, BsModalService  } from 'ngx-bootstrap/modal';
  import {SmtpSettingsServiceServiceProxy, SmtpSettingsDto} from '../../shared/service-proxies/service-proxies';
  import { finalize } from 'rxjs';
import { appModuleAnimation } from '@shared/animations/routerTransition';
  
  
  @Component({
    animations: [appModuleAnimation()],    
    templateUrl: 'smtpSettings.html',
  })

  export class SmtpSettingsComponent  implements OnInit {

    smtpSettingsList: SmtpSettingsDto = new SmtpSettingsDto(); 
    editSmtpSetting: SmtpSettingsDto = new SmtpSettingsDto();
    to : string ;
    loading : boolean = false;

    constructor(
      private smtpSettingsService: SmtpSettingsServiceServiceProxy,
      private cdr: ChangeDetectorRef
    ) {}
    
    ngOnInit(): void {
      this.getSmtpSettings();
      this.editSmtpSetting = this.smtpSettingsList;
    }
  
  getSmtpSettings() {
    this.smtpSettingsService.getSmtpSettings()
      .pipe(finalize(() => this.cdr.detectChanges()))
      .subscribe((result) => {
        this.smtpSettingsList = result  ;
        console.log("SMTP Settings List:", result);
      });
  }

  saveChanges (setting : SmtpSettingsDto) {
    this.smtpSettingsService.updateTenantSmtpSettings(setting).subscribe(
      response => {                   
      });      
      abp.notify.success(('SuccessfullySaved'));    
  }

  testMail(to)
  {
    this.smtpSettingsService.testMail(to).subscribe(
      response => {});
      this.to = ''
      abp.notify.success(('TRIED'));
  }

  }
  