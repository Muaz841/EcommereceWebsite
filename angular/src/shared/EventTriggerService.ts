import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventTriggerServiceComponent {
  private orderTable = new Subject<void>();

  orderComponentUpdated$ = this.orderTable.asObservable();


  notifyOrderUpdated() {
    this.orderTable.next();
  }
}
