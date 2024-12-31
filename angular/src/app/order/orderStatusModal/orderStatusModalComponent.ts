import {
  Component,
  Injector,
  OnInit,
  EventEmitter,
  Output,
  ChangeDetectorRef,
} from "@angular/core";
import { BsModalRef } from "ngx-bootstrap/modal";
import { AppComponentBase } from "../../../shared/app-component-base";
import { OrderListDto } from "@shared/service-proxies/service-proxies";
import { OrderServicesServiceProxy } from "@shared/service-proxies/service-proxies";
import { EventTriggerServiceComponent } from "../../../shared/EventTriggerService";

@Component({
  selector: "status-modal",
  templateUrl: "./orderStatusModal.html",
  styleUrls: ["./orderStatusModalStyle.css"],
})
export class orderStatusModalComponent
  extends AppComponentBase
  implements OnInit
{
  entity: OrderListDto;
  selectedStatus: number;
  availableStatus = [
    { id: 0, name: "processing" },
    { id: 1, name: "Shipped" },
    { id: 2, name: "Delivered" },
    { id: 3, name: "Cancelled" },
  ];

  @Output() onSave = new EventEmitter<any>();

  constructor(
    private _eventTriggerService: EventTriggerServiceComponent,
    injector: Injector,
    private _orderService: OrderServicesServiceProxy,
    public bsModalRef: BsModalRef,
    private cd: ChangeDetectorRef
  ) {
    super(injector);
  }

  ngOnInit(): void {}

  saveStatus() {
    if (this.entity === null && this.selectedStatus == null) {
      abp.notify.error("Order Status Not Changed");
      return;
    }

    this._orderService
      .orderStatusChange(this.entity.orderID, this.selectedStatus)
      .subscribe(() => {
        abp.notify.success("Status Changed");
        this.onSave.emit({
          orderId: this.entity.orderID,
          status: this.selectedStatus,
        });
        this.bsModalRef.hide();
        this._eventTriggerService.notifyOrderUpdated();
      });
  }

  close() {
    this.bsModalRef.hide();
  }
}
