import {Component, OnInit} from '@angular/core';
import {Subject, takeUntil} from "rxjs";
import {ActivatedRoute} from "@angular/router";
import {IApproveService} from "../../../services/i-approve.service";
import {saveAs} from 'file-saver'

@Component({
  selector: 'app-i-approve-detail',
  templateUrl: './i-approve-detail.component.html',
  styleUrls: ['./i-approve-detail.component.scss']
})
export class IApproveDetailComponent implements OnInit {
  private destroy$ = new Subject();
  orderId: any;
  data = {
    title: '',
    documents: [],
    history: []
  };

  constructor(private route: ActivatedRoute, private iApproveService: IApproveService) {
  }

  ngOnInit(): void {
    this.route.queryParams.pipe(takeUntil(this.destroy$)).subscribe(params => {
      this.orderId = params['orderId'];
      this.iApproveService.getMyApprovalDetail(this.orderId).pipe(takeUntil(this.destroy$)).subscribe(res => {
        if (res && res.data)
          this.data = res.data;
      })
    });
  }
}
