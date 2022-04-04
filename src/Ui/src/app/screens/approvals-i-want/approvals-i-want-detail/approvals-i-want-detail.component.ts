import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {ApprovalsIWantService} from "../../../services/approvals-i-want.service";
import {Subject, takeUntil} from "rxjs";

@Component({
  selector: 'app-approvals-i-want-detail',
  templateUrl: './approvals-i-want-detail.component.html',
  styleUrls: ['./approvals-i-want-detail.component.scss']
})
export class ApprovalsIWantDetailComponent implements OnInit {
  private destroy$ = new Subject();
  data: any;
  orderId: any;
  step;

  constructor(private route: ActivatedRoute, private approvalsIWantService: ApprovalsIWantService) {
    this.route.queryParams.subscribe(params => {
      this.orderId = params['orderId'];
      this.approvalsIWantService.getWantApprovalDetail(this.orderId).pipe(takeUntil(this.destroy$)).subscribe(res => {
        this.data = res && res.data;
      });
    });
  }

  ngOnInit(): void {
  }

}
