import { ChangeDetectorRef, Component, Injector, OnInit } from "@angular/core";
import {
  ActivatedRoute,
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from "@angular/router";
import {
  ProductReviewsDto,
  PublicSiteServiceProxy,
  ReviewProductsDto,
} from "../../shared/service-proxies/service-proxies";
import { AppSessionService } from "../../shared/session/app-session.service";
import { AppRouteGuard } from "@shared/auth/auth-route-guard";

@Component({
  templateUrl: "./reviewScreenView.html",
  styleUrls: ["./reviewScreenStyle.css"],
})
export class ReviewScreenComponent implements OnInit {
  products: ReviewProductsDto[] = [];
  reviews: { [key: number]: { ratings: number; review: string } } = {};

  constructor(
    injector: Injector,
    private _router: Router,
    private _authGuard: AppRouteGuard,
    private route: ActivatedRoute,
    private _sessionService: AppSessionService,
    private _publicSiteService: PublicSiteServiceProxy,
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const canProceed = this._authGuard.checkReviewStatus();
    const orderId = Number(this.route.snapshot.paramMap.get("id"));
    this._publicSiteService.getProductForReview(orderId).subscribe(
      (result) => {
        console.log("Products:", result);
        this.products = result;

        // Initialize reviews object
        this.products.forEach((product) => {
          this.reviews[product.productId] = { ratings: 0, review: "" };
        });

        this.cd.detectChanges();
      },
      () => abp.notify.error("Error loading products")
    );
  }

  handleReviewInput(productId: number, ratings: number, review: string): void {
    this.reviews[productId] = { ratings, review };
  }

  submitAllReviews(): void {
    const reviewDataArray = this.products
      .map((product) => {
        const { ratings, review } = this.reviews[product.productId] || {
          ratings: 0,
          review: "",
        };

        if (!ratings || !review.trim()) {
          return null;
        }

        const reviewData = new ProductReviewsDto();
        reviewData.userId = this._sessionService.userId;
        reviewData.productId = product.productId;
        reviewData.ratings = ratings;
        reviewData.reviews = review.trim();

        return reviewData;
      })
      .filter((review) => review !== null);
    this._publicSiteService.addRatings(reviewDataArray).subscribe(
      (response) => {
        abp.notify.success("Thanks for your reviews!");
        this._sessionService.setReviewSubmitted(true);
        this._router.navigate(["app/reviewScreen/thank-you"]);
      },
      (error) => {
        abp.notify.error("Error submitting reviews");
      }
    );
  }
}
