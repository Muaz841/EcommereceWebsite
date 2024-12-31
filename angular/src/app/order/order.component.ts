import {
  ChangeDetectorRef,
  Component,
  Injector,
  ViewEncapsulation,
} from "@angular/core";
import {
  OrderServicesServiceProxy,
  OrderListDto,
  OrderCountDto,
  OrderListDtoPagedResultDto,
  PagedOrderResultRequestDto,
} from "@shared/service-proxies/service-proxies";
import { appModuleAnimation } from "../../shared/animations/routerTransition";
import { AppComponentBase } from "../../shared/app-component-base";
import { Router } from "@angular/router";
import {
  PagedListingComponentBase,
  PagedRequestDto,
} from "@shared/paged-listing-component-base";
import { finalize, Subscription } from "rxjs";
import { orderStatusModalComponent } from "./orderStatusModal/orderStatusModalComponent";
import { BsModalRef, BsModalService } from "ngx-bootstrap/modal";
import { EventTriggerServiceComponent } from "../../shared/EventTriggerService";
import * as moment from "moment";

class PagedOrderRequestDto extends PagedRequestDto {
  keyword: string;
  startDate: moment.Moment;
  endDate: moment.Moment;
}

@Component({
  templateUrl: "./order.html",
  styleUrls: ["./order-style.css"],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None,
})
export class OrderComponent extends PagedListingComponentBase<OrderListDto> {
  selectedButton: number = 4;
  AllOrder: OrderListDto[] = [];
  processingOrder: OrderListDto[] = [];
  deliveredOrder: OrderListDto[] = [];
  shippedOrder: OrderListDto[] = [];
  cancelledOrder: OrderListDto[] = [];
  filteredOrders: OrderListDto[] = [...this.AllOrder];
  orderStats: OrderCountDto;
  keyword: string = " ";
  sortby = " ";
  dateRange: Date[] = [];
  private orderUpdatedSubscription: Subscription;

  constructor(
    private _eventTriggerService: EventTriggerServiceComponent,
    private _modalService: BsModalService,
    private injector: Injector,
    cd: ChangeDetectorRef,
    private _orderService: OrderServicesServiceProxy,
    private router: Router
  ) {
    super(injector, cd);
  }

  protected list(
    request: PagedOrderRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    this.orderUpdatedSubscription =
      this._eventTriggerService.orderComponentUpdated$.subscribe(() => {
        this.getDataPage(1);
      });
    request.keyword = this.keyword;
    this.AllOrder = [];
    if (this.dateRange && this.dateRange.length === 2) {
      request.startDate = moment(this.dateRange[0]);
      request.endDate = moment(this.dateRange[1]);
    } else {
      request.startDate = undefined;
      request.endDate = undefined;
    }

    const orderApiData = new PagedOrderResultRequestDto();
    orderApiData.filterText = request.keyword;
    orderApiData.skipCount = request.skipCount;
    orderApiData.endDate = request.endDate;
    orderApiData.startDate = request.startDate;
    orderApiData.maxResultCount = request.maxResultCount;
    this._orderService
      .orderList(orderApiData)
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: OrderListDtoPagedResultDto) => {
        this.filteredOrders = result.items;
        this.AllOrder = result.items;
        this.AllOrder.forEach((order) => {
          if (order.status === 0) {
            this.processingOrder.push(order);
          }
          if (order.status === 1) {
            this.shippedOrder.push(order);
          }
          if (order.status === 2) {
            this.deliveredOrder.push(order);
          }
          if (order.status === 3) {
            this.cancelledOrder.push(order);
          }
        });
        this.showPaging(result, pageNumber);
        this._orderService.orderCount().subscribe((result) => {
          this.orderStats = result;
          this.cd.detectChanges();
        });
        this.dateRange = [];
        this.cd.detectChanges();
      });
  }
  protected delete(entity: OrderListDto): void {
    throw new Error("Method not implemented.");
  }

  exportToExcel() {
    this._orderService.getExcelFile().subscribe(
      () => {
        abp.notify.success("File Saved ");
      },
      (Error) => {
        abp.notify.error(Error);
      }
    );
  }

  getStatusLabel(order: any): string {
    if (order.status === 0) {
      return "Processing";
    }
    if (order.status === 1) {
      return "Shipped";
    }
    if (order.status === 2) {
      return "Delivered";
    }
    if (order.status === 3) {
      return "Cancelled";
    }
  }

  getStatusColor(order: any): string {
    if (order.status === 0) {
      return "#EEA5201A";
    }
    if (order.status === 1) {
      return "#2BB2FE1A";
    }
    if (order.status === 2) {
      return "#87CE6F1A";
    }
    if (order.status === 3) {
      return "#ED47651A";
    }
  }

  getStatusClass(order: any): string {
    if (order.status === 0) {
      return "status-processing";
    }
    if (order.status === 1) {
      return "status-shipped";
    }
    if (order.status === 2) {
      return "status-delivered";
    }
    if (order.status === 3) {
      return "status-cancelled";
    }
  }

  selectButton(button: number): void {
    this.selectedButton = button;
    switch (button) {
      case 0:
        this.filteredOrders = [...this.processingOrder];
        break;
      case 1:
        this.filteredOrders = [...this.shippedOrder];
        break;
      case 2:
        this.filteredOrders = [...this.deliveredOrder];
        break;
      case 3:
        this.filteredOrders = [...this.cancelledOrder];
        break;
      default:
        this.filteredOrders = [...this.AllOrder];
        break;
    }
  }

  onsearch() {
    this.getDataPage(1);
  }

  protected viewOrderDetails(entity: OrderListDto): void {
    this.router.navigate(["app/order/orderdDetails", entity.orderID]);
  }

  ShowOrderStatuModal(entity: OrderListDto): void {
    const initialState = {
      entity: entity,
    };

    let editStatusModal: BsModalRef;
    editStatusModal = this._modalService.show(orderStatusModalComponent, {
      initialState,
    });
  }

  onDateSearch(): void {
    if (this.dateRange && this.dateRange.length === 2) {
      this.getDataPage(1);
    }
  }
}
