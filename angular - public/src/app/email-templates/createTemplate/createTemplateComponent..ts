import {
  Component,
  Injector,
  OnInit,
  EventEmitter,
  Output,
  ChangeDetectorRef
} from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import {
  EmailTemplateDto,
  TemplateEmailServiceServiceProxy
} from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-create-template',
  templateUrl: './createTemplate.html',
  providers: [TemplateEmailServiceServiceProxy]
})
export class CreateTemplatesComponent extends AppComponentBase implements OnInit {

  @Output() onSave = new EventEmitter<void>();
  createEmail: EmailTemplateDto = new EmailTemplateDto();
  visible: boolean = false;

  tokenList: string[] = ["{{username}}", "{{useremail}}"];

  constructor(
    injector: Injector,
    private _emailService: TemplateEmailServiceServiceProxy,
    private cdr: ChangeDetectorRef,
    private modalRef: BsModalRef // Inject BsModalRef here

  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.visible = true;
  }

  saveChanges() {
    this.createEmail.token = this.tokenList.join(',');
    this._emailService.createOrEditTemplate(this.createEmail).pipe(
      finalize(() => {
        this.onSave.emit(); // Emit the event after saving
      })
    ).subscribe({
      next: (response) => {        
        this.closeDialog();
      }
    });
  }
  closeDialog() {
    this.visible = false;
    this.modalRef.hide(); // Hide the modal, ensuring overlay disappears
  }
}
