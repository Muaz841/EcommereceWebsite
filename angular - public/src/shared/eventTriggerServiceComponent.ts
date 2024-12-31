import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventTriggerServiceComponent {
  private cartUpdated = new Subject<void>();

  cartUpdated$ = this.cartUpdated.asObservable();


  notifyCartUpdate() {
    this.cartUpdated.next();
  }
}
