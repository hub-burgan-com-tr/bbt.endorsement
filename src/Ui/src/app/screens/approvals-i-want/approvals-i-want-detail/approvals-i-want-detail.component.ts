import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {ApprovalsIWantService} from "../../../services/approvals-i-want.service";
import {Subject, takeUntil} from "rxjs";
import {NgxSmartModalService} from "ngx-smart-modal";

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
  modalDetail: any;

  constructor(private route: ActivatedRoute, private router: Router, private approvalsIWantService: ApprovalsIWantService, private modal: NgxSmartModalService) {
    this.route.queryParams.subscribe(params => {
      this.orderId = params['orderId'];
      this.approvalsIWantService.getWantApprovalDetail(this.orderId).pipe(takeUntil(this.destroy$)).subscribe(res => {
        this.data = res && res.data;
      });
    });
  }

  ngOnInit(): void {
  }

  openModal(d: any) {
    this.modalDetail = d;
    this.modal.open('document');
  }
  redirectToList(){
    this.router.navigate(['..'], {relativeTo: this.route});
  }
  cancelOrder() {
    this.approvalsIWantService.cancelOrder(this.orderId).pipe(takeUntil(this.destroy$)).subscribe(res => {
      this.modal.close('cancelOrderModal');
      this.modal.open('cancelOrderConfirmed');
    });
  }
}
