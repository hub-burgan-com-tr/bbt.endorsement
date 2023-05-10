import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { CommonService } from 'src/app/services/common.service';
import { Subject, takeUntil } from "rxjs";
import { ActivatedRoute, Router } from "@angular/router";
import { saveAs } from 'file-saver';
@Component({
  selector: 'app-render-file',
  templateUrl: './render-file.component.html',
  styleUrls: ['./render-file.component.scss']
})
export class RenderFileComponent implements OnInit, OnDestroy {
  @Input() detail;
  private destroy$ = new Subject();
  orderId: any;
  constructor(private commonService: CommonService, private route: ActivatedRoute) {
    this.route.queryParams.subscribe(params => {
      this.orderId = params['orderId'];
      if (!this.orderId) {
        this.route.params.subscribe(p => {
          this.orderId = p['orderId'];
        })
      }
    });
  }

  ngOnInit(): void {
  }
  getDocumentPdf() {
    this.commonService.getDocumentPdf(this.orderId, this.detail.documentId ? this.detail.documentId : this.detail.actions.documentId).pipe(takeUntil(this.destroy$)).subscribe(
      data => {
        saveAs(data.body, this.detail.fileName);
      });
  }
  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.complete();
  }
}
