import { appModuleAnimation } from "@shared/animations/routerTransition";
import { ChangeDetectorRef, Component, Injector } from "@angular/core";
import {
  PagedListingComponentBase,
  PagedRequestDto,
} from "shared/paged-listing-component-base";
import {
  QueueEmailServiceServiceProxy,
  QueuedEmailDto,
  QueuedEmailDtoPagedResultDto,
} from "@shared/service-proxies/service-proxies";
import { finalize } from "rxjs";

class queuedEmailRequestDto extends PagedRequestDto {
  keyword: string;
  status: string;
  sorting: string;
}

@Component({
  animations: [appModuleAnimation()],
  templateUrl: "./queued-email.html",
})
export class queuedEmailComponent extends PagedListingComponentBase<QueuedEmailDto> {
  queuedEmails: QueuedEmailDto[] = [];
  keyqord = "";
  status = "";
  sorting = "";
  selectedEmail: QueuedEmailDto;
  visible: boolean = false;

  constructor(
    injector: Injector,
    private _queuedService: QueueEmailServiceServiceProxy,
    cd: ChangeDetectorRef
  ) {
    super(injector, cd);
  }

  ngOnInit(): void {
    this.refresh();
  }
  protected list(
    request: queuedEmailRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;

    this._queuedService
      .getEmailsInQueue(
        request.keyword,
        request.status,
        request.sorting,
        request.skipCount,
        20
      )
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: QueuedEmailDtoPagedResultDto) => {
        this.queuedEmails = result.items;

        this.showPaging(result, pageNumber);
        console.log(result.items, "result");
      });
  }

  protected delete(entity: QueuedEmailDto): void {
    throw new Error("Method not implemented.");
  }
  queuedemails: QueuedEmailDto[] = [];
  keyword = "";

  protected updateFailed(id) {
    this._queuedService.updateFailedMails(id).subscribe((response) => {});
    abp.notify.success("status updated");
    this.refresh();
    window.location.reload();
  }

  showDialog(email: QueuedEmailDto) {
    this.selectedEmail = email;
    this.visible = true;
  }
}
