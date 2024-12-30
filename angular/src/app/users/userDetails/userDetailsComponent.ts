import { Component, OnInit, Injector, ChangeDetectorRef } from '@angular/core';

import { BsModalRef } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '../../../shared/app-component-base';
import { UserServiceProxy, UserDetailsDto, OrderDto } from '../../../shared/service-proxies/service-proxies';
import { log } from 'console';
import { ActivatedRoute } from '@node_modules/@angular/router';

@Component({
  selector: 'app-reset-password',
  templateUrl: './userDetails.html',
  styleUrls: ['./userDetailsStyle.css']
})
export class UserDetailsComponent extends AppComponentBase
  implements OnInit {
userDetails: UserDetailsDto = new UserDetailsDto();

  constructor(
    injector: Injector,
    private _userService: UserServiceProxy,
     private cdr: ChangeDetectorRef,
     private route: ActivatedRoute,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit() {
    const userId = Number(this.route.snapshot.paramMap.get('id'));
      this._userService.userDetails(userId).subscribe((result:UserDetailsDto ) => {
        this.userDetails = result ;                 
        this.cdr.detectChanges();       
       })
  }

   
    getUserStatusLabel(user: UserDetailsDto): string {
      if (user.isActive === true) {
        return "ACTIVE";    
      }
      if (user.isActive === false) {
        return "DELETED";    
      } 
    }
  
    getUserStatusColor(user: UserDetailsDto): string {
      if (user.isActive === true) {
        return "e6f7e6";    
      }
      if (user.isActive === false) {
        return "#f2dede";    
      }
   
    }
  
    getUserStatusClass(user: UserDetailsDto): string {
      if (user.isActive === true) {
        return "  statusactive ";    
      }
      if (user.isActive === false) {
        return " statusdelete";    
      }
    }
 
    getStatusLabel(order: OrderDto): string {
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
  
    getStatusColor(order: OrderDto): string {
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
  
    getStatusClass(order: OrderDto): string {        
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
  
  }



