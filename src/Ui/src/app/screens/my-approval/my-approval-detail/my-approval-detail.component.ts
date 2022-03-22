import {Component, OnInit} from '@angular/core';
import {Subject, takeUntil} from "rxjs";
import {NgxSmartModalService} from "ngx-smart-modal";
import {ActivatedRoute} from "@angular/router";
import {MyApprovalService} from "../../../services/my-approval.service";
import {GetApprovalDetailRequestModel} from "../../../models/my-approval";

@Component({
  selector: 'app-my-approval-detail',
  templateUrl: './my-approval-detail.component.html',
  styleUrls: ['./my-approval-detail.component.scss']
})
export class MyApprovalDetailComponent implements OnInit {
  private destroy$ = new Subject();
  step: number = 1;
  orderId: any;
  detail = {
    title: '',
    name: '',
    content: '',
    actions: []
  }

  constructor(public ngxSmartModalService: NgxSmartModalService,
              private route: ActivatedRoute,
              private myApprovalService: MyApprovalService) {
  }

  ngOnInit(): void {
    this.route.params.pipe(takeUntil(this.destroy$)).subscribe(params => {
      this.orderId = params['orderId'];
    });
    this.getApprovalDetail();
  }

  getApprovalDetail() {
    let requestModel: GetApprovalDetailRequestModel = {
      orderId: this.orderId
    };
    this.myApprovalService.getApprovalDetail(requestModel).pipe(takeUntil(this.destroy$)).subscribe({
      next: res => {
        if (res.data) {
          this.detail = res.data
        } else
          console.error("Kayıt bulunamadı");
      },
      error: err => {
        console.error(err.message);
      }
    });
  }

  send() {
    this.ngxSmartModalService.close('myModal');
    this.ngxSmartModalService.open('confirmModal');
  }

  continue() {
    this.step++;
  }

  redirect() {
  }
}
