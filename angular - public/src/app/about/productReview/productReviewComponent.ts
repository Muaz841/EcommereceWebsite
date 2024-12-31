import {
  Component,
  Injector,
  OnInit,
  EventEmitter,
  Output,
  ChangeDetectorRef,
} from "@angular/core";
import { BsModalRef } from "ngx-bootstrap/modal";
import { forEach as _forEach, map as _map } from "lodash-es";
import { AppComponentBase } from "@shared/app-component-base";
import {
  PublicSiteServiceProxy,
  ProductReviewsDto,
} from "shared/service-proxies/service-proxies";
import { AppSessionService } from "@shared/session/app-session.service";

@Component({
  selector: "app-add-review-dialog",
  templateUrl: "./productReviewHtml.html",
  styleUrls: ["./productReviewStyle.css"],
})
export class ProductReviewDialogComponent
  extends AppComponentBase
  implements OnInit
{
  productId: number;
  ratings: number = 0;
  review: string = " ";
  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    private cd: ChangeDetectorRef,
    private _sessionService: AppSessionService,
    private _publicSiteService: PublicSiteServiceProxy
  ) {
    super(injector);
  }

  ngOnInit(): void {}

  submitReview() {
    const reviewdata = new ProductReviewsDto();
    reviewdata.userId = this._sessionService.userId;
    reviewdata.reviews = this.review;
    reviewdata.ratings = this.ratings;
    reviewdata.productId = this.bsModalRef.content.productId;
    this._publicSiteService.addRatings([reviewdata]).subscribe(
      (response) => {
        abp.notify.success(`Thanks For Your Review`);
        this.bsModalRef.hide();
      },
      (error) => {
        abp.notify.error("Error adding Review");
      }
    );
  }

  cancel() {
    this.bsModalRef.hide();
  }
}
