import { ChangeDetectorRef, Component, Injector, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { AppSessionService } from "../../../shared/session/app-session.service";

@Component({
  templateUrl: "./thanksScreenHtml.html",
  styleUrls: ["./thanksScreenStyle.css"],
})
export class ThanksScreenComponent implements OnInit {
  constructor(
    injector: Injector,
    private router: Router,
    private route: ActivatedRoute,
    private _sessionService: AppSessionService,
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit(): void {}
}
