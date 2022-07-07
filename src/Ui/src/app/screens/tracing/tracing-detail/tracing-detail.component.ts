import {Component, OnInit} from '@angular/core';
import {Subject, takeUntil} from "rxjs";
import {ActivatedRoute} from "@angular/router";
import {TracingService} from "../../../services/tracing.service";
import {NgxSmartModalService} from "ngx-smart-modal";

@Component({
  selector: 'app-tracing-detail',
  templateUrl: './tracing-detail.component.html',
  styleUrls: ['./tracing-detail.component.scss']
})
export class TracingDetailComponent implements OnInit {
  private destroy$ = new Subject();
  data: any;
  orderId: any;
  step;
  modalDetail: any;
  constructor(private route: ActivatedRoute, private tracingService: TracingService, private modal: NgxSmartModalService) {
    this.route.queryParams.subscribe(params => {
      this.orderId = params['orderId'];
      this.tracingService.getWatchApprovalDetail(this.orderId).pipe(takeUntil(this.destroy$)).subscribe(res => {
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
}
