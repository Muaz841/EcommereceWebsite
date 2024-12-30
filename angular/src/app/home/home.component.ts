import { Component, Injector, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { DashboardRevenueDto, OrderServicesServiceProxy,OrderListDto,DashboardServiceServiceProxy,OrderListDtoPagedResultDto, StatsGraphDto, PagedOrderResultRequestDto } from '@shared/service-proxies/service-proxies';
import { isPlatformBrowser } from '@angular/common';
import { inject, PLATFORM_ID, effect } from '@angular/core';
import * as moment from 'moment';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { finalize } from 'rxjs/operators';
import { Router } from '@node_modules/@angular/router';

class PagedOrderRequestDto extends PagedRequestDto {
  keyword: string;
    startDate: moment.Moment;
        endDate: moment.Moment;
}

@Component({
  templateUrl: './home.component.html',
  animations: [appModuleAnimation()],
  styleUrls: ['./home.style.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent extends  PagedListingComponentBase<OrderListDto> {

  revenueData: DashboardRevenueDto;
  graphStats: StatsGraphDto[] = [];
  AllOrder: OrderListDto[] = [];
  dateRange: Date[] = [];
  data: any;
  options: any;
  keyword: string = " ";  
  platformId = inject(PLATFORM_ID);

  constructor(
    injector: Injector,
     private router: Router,
    private _orderService: OrderServicesServiceProxy,
    private _dashBoardService: DashboardServiceServiceProxy,
    private cdr: ChangeDetectorRef
  ) {
    super(injector, cdr);
  }

 
  protected list(
    request: PagedOrderRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;
    this.AllOrder = [];
    this._dashBoardService.dashboardRevenue().subscribe((result) => {
      this.revenueData = result;
      this.cdr.detectChanges();
    });

    this._dashBoardService.graphStats().subscribe((result: StatsGraphDto[]) => {
      this.graphStats = result;           
      this.aggregateWeeklyData();
      this.initChart();
      this.cdr.detectChanges();
    });
     const orderApiData = new PagedOrderResultRequestDto();
                orderApiData.filterText = request.keyword;
                orderApiData.skipCount = request.skipCount;
                orderApiData.endDate= request.endDate;
                orderApiData.startDate= request.startDate;
                orderApiData.maxResultCount = request.maxResultCount;
        this._orderService.orderList(
          orderApiData
    ).pipe(
      finalize(() => {        
        finishedCallback();
      })
    )
    .subscribe((result: OrderListDtoPagedResultDto) => {      
      this.AllOrder = result.items;    
      console.log(result);
            
      this.showPaging(result, pageNumber);    
      this.cd.detectChanges();
    });    

  }
  protected delete(entity: OrderListDto): void {
    throw new Error('Method not implemented.');
  }

  aggregateWeeklyData() {
   
    const weekData = {
      Sunday: 0,
      Monday: 0,
      Tuesday: 0,
      Wednesday: 0,
      Thursday: 0,
      Friday: 0,
      Saturday: 0
    };

    
    this.graphStats.forEach(item => {
      const dayOfWeek = moment(item.creationtime).format('dddd'); 
      weekData[dayOfWeek] += item.revenue; 
    });

   
    const labels = Object.keys(weekData); 
    const revenueValues = Object.values(weekData); 

   
    this.data = {
      labels: labels,  
      datasets: [
        {
          label: 'Weekly Revenue',
          fill: false,
          borderColor: '#00B5E2',  
          yAxisID: 'y',
          tension: 0.4,
          data: revenueValues 
        }
      ]
    };
  }

  initChart() {
    if (isPlatformBrowser(this.platformId)) {
      const documentStyle = getComputedStyle(document.documentElement);
      const textColor = documentStyle.getPropertyValue('--p-text-color');
      const textColorSecondary = documentStyle.getPropertyValue('--p-text-muted-color');
      const surfaceBorder = documentStyle.getPropertyValue('--p-content-border-color');

      this.options = {
        stacked: false,
        maintainAspectRatio: false,
        aspectRatio: 0.6,
        plugins: {
          legend: {
            labels: {
              color: textColor
            }
          }
        },
        scales: {
          x: {
            ticks: {
              color: textColorSecondary
            },
            grid: {
              color: surfaceBorder
            }
          },
          y: {
            type: 'linear',
            display: true,
            position: 'left',
            ticks: {
              color: textColorSecondary
            },
            grid: {
              color: surfaceBorder
            }
          }
        }
      };
    }
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

  protected viewOrderDetails(entity: OrderListDto): void {
    this.router.navigate(["app/order/orderdDetails", entity.orderID]);
  }

}
