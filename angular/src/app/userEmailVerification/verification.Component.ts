import {
  Component,
  Injector,
  OnInit,
  EventEmitter,
  Output,
  ChangeDetectorRef,
} from "@angular/core";
import {
  forEach as _forEach,
  includes as _includes,
  map as _map,
} from "lodash-es";
import { AppComponentBase } from "../../shared/app-component-base";
import { ApiServiceProxy } from "@shared/service-proxies/service-proxies";
import { ActivatedRoute, Router } from "@node_modules/@angular/router";

@Component({
  template: "",
})
export class verificationComponent extends AppComponentBase implements OnInit {
  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _router: Router,
    private userVerfication: ApiServiceProxy,
    private cd: ChangeDetectorRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    const orderId = Number(this.route.snapshot.paramMap.get("id"));
    const token = this.route.snapshot.paramMap.get("token");

    this.userVerfication.userVerification(orderId, token).subscribe({
      next: (response) => {
        this._router.navigate(["./account/login"]);
        abp.notify.success("Your account has been successfully activated!");
      },
      error: (err) => {
        abp.notify.error(
          err.error?.message || "Verification failed. Please try again."
        );
      },
    });
  }
}
