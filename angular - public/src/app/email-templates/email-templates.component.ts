import {
  Component,
  Injector,
  OnInit,
  EventEmitter,
  Output,
  ChangeDetectorRef
} from '@angular/core';
import { EmailTemplateDto, TemplateEmailServiceServiceProxy, EmailTemplateDtoPagedResultDto } from '@shared/service-proxies/service-proxies';
import { BsModalRef, BsModalService  } from 'ngx-bootstrap/modal';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { finalize } from 'rxjs';
import { CreateTemplatesComponent } from './createTemplate/createTemplateComponent.';

class EmailTemplateRequestDto extends PagedRequestDto {
  keyword: string;
  status : string; 
  sorting: string;
}

@Component({
  selector: 'app-email-templates',
  templateUrl: 'email-templates.html',
})
export class EmailTemplatesComponent extends PagedListingComponentBase<EmailTemplateDto> {
  
  keyword = "";
  status = "";
  sorting = "";
  emailTemplate: EmailTemplateDto[] = [];
  selectedEmail: EmailTemplateDto = new EmailTemplateDto;
  visible: boolean = false;
  tokenList: string[] = [];



  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public _emailService: TemplateEmailServiceServiceProxy,
    public _modalService: BsModalService,
    private cdr: ChangeDetectorRef
  ) {
    super(injector, cdr);
  }
  
ngOnInit(): void {
  this.refresh();
}

protected list(request: EmailTemplateRequestDto, pageNumber: number, finishedCallback: Function): void {
  request.keyword = this.keyword;

  this._emailService.getTemplate( request.keyword,request.status, request.sorting,  request.skipCount,
    request.maxResultCount).pipe(
      finalize(() => {
        finishedCallback();
      })
    ).subscribe((result: EmailTemplateDtoPagedResultDto) => {
        this.emailTemplate = result.items;
        
        this.showPaging(result, pageNumber);  
        console.log(this.tokenList );
             
      });    
}


protected delete(entity: EmailTemplateDto): void {
  abp.message.confirm(
    `Are you sure you want to delete the email template "${entity.name}"?`, 
    undefined,
    (result: boolean) => {
      if (result) {       
        this._emailService.deleteTemplate(entity.id).subscribe(
          response => {
            abp.notify.success('Successfully deleted the email template.'); 
            this.refresh();
          }
        );
      }
    }
  );
}

showDialog(email: EmailTemplateDto) {
  this.selectedEmail = email;
  this.tokenList = this.parseTokens(this.selectedEmail.token);
  this.visible = true;   
}

saveChanges(editemail:EmailTemplateDto){
  console.log(editemail);
  this._emailService.createOrEditTemplate(editemail).subscribe(
    response => {                   
    });
    this.closeDialog();
}
closeDialog() {
  this.visible = false; 
}

parseTokens(tokenString: string | null): string[] {
  return tokenString ? tokenString.split(',').map(token => token.trim()) : [];
}


openCreateTemplateModal() {
  const initialState = { createEmail: new EmailTemplateDto() };
  const modalRef: BsModalRef = this._modalService.show(CreateTemplatesComponent, { initialState });

  // Listen to the onSave event from CreateTemplatesComponent
  modalRef.content?.onSave.subscribe(() => {
    this.refresh(); // Refresh the list when the template is saved
  });
}


}
