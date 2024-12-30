import { ChangeDetectorRef, Component, Injector, ViewEncapsulation } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '../../shared/animations/routerTransition';
import { AppComponentBase } from '../../shared/app-component-base';
import { Router } from '@angular/router';



@Component({
  templateUrl: './order.html',
  styleUrl: './order-style.css',
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None
})
export class OrderComponent extends AppComponentBase {

  // Select Button 
  stateOptions: any[] = [{ label: 'All', value: 'all' },{ label: 'Processing', value: 'processing' }
    ,{ label: 'Shipped', value: 'shipped' },{ label: 'Delivered', value: 'delivered' },{ label: 'Cancelled', value: 'cancelled' }
  ];
  value: string = 'all';

  //Primeng Table
  products: any;
  selectedProducts: any;
  //////////////////

  
  constructor(
    private injector: Injector,
    private cd: ChangeDetectorRef,
    private router: Router,
  ){
    super(injector)
  }

  viewOrderDetails(){
    this.router.navigate(['app/order/orderdetails']);
  }
}