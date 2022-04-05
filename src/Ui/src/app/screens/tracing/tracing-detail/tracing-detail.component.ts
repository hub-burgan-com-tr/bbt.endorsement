import {Component, OnInit} from '@angular/core';
import {Subject, takeUntil} from "rxjs";
import {ActivatedRoute} from "@angular/router";
import {TracingService} from "../../../services/tracing.service";

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

  constructor(private route: ActivatedRoute, private tracingService: TracingService) {
    this.route.queryParams.subscribe(params => {
      this.orderId = params['orderId'];
      this.tracingService.getWatchApprovalDetail(this.orderId).pipe(takeUntil(this.destroy$)).subscribe(res => {
        this.data = res && res.data;
      });
    });
  }

  ngOnInit(): void {
  }
}
