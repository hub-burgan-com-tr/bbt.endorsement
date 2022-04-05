import {Component, OnInit} from '@angular/core';
import {Subject, takeUntil} from "rxjs";
import {NgxSmartModalService} from "ngx-smart-modal";
import {ActivatedRoute} from "@angular/router";
import {MyApprovalService} from "../../../services/my-approval.service";
import {
  GetApprovalDetailRequestModel,
  GetApprovalPhysicallyDocumentDetailRequestModel
} from "../../../models/my-approval";

@Component({
  selector: 'app-my-approval-detail',
  templateUrl: './my-approval-detail.component.html',
  styleUrls: ['./my-approval-detail.component.scss']
})
export class MyApprovalDetailComponent implements OnInit {
  private destroy$ = new Subject();
  step: number = 0;
  orderId: any;
  title: '';
  buttonText: string = 'Devam Et';
  details = [{
    documentId: '',
    name: '',
    content: '',
    choice: false,
    actions: []
  }];
  physicallyDocuments = [{
    name: '',
    documentLink: '',
    content: '',
    actions: []
  }];

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

  send() {
    this.ngxSmartModalService.close('myModal');
    this.ngxSmartModalService.open('confirmModal');
  }

  continue() {
    // this.getApprovalPhysicallyDocumentDetail();
    this.step++;
    if (this.step >= (this.details.length - 1)) {
      this.buttonText = 'Kaydet';
    }
    //Post data
    if (this.step > (this.details.length - 1)) {
      this.step--;
      const model = {
        orderId: this.orderId,
        documents: []
      };
      this.details.forEach(i => {
        const actionId = i.actions.find(f => f.value == i.choice)?.documentActionId;
        if (actionId) {
          model.documents.push({
            documentId: i.documentId,
            actionId: actionId,
            // choice: i.choice
          });
        }
      });
      this.myApprovalService.saveApproveOrderDocument(model).pipe(takeUntil(this.destroy$)).subscribe(res => {
        console.log(res);
      });
      return;
    }
  }

  getApprovalDetail() {
    let requestModel: GetApprovalDetailRequestModel = {
      orderId: this.orderId
    };
    this.myApprovalService.getApprovalDetail(requestModel).pipe(takeUntil(this.destroy$))
      .subscribe({
        next: res => {
          if (res.data) {
            this.title = res.data.title
            this.details = res.data.documents
            if (this.details.length === 1) {
              this.buttonText = 'Kaydet';
            }
          } else
            console.error("Kayıt bulunamadı");
        },
        error: err => {
          console.error(err.message);
        }
      });
  }

  getApprovalPhysicallyDocumentDetail() {
    let requestModel: GetApprovalPhysicallyDocumentDetailRequestModel = {
      orderId: this.orderId
    };
    this.myApprovalService.getApprovalPhysicallyDocumentDetail(requestModel).pipe(takeUntil(this.destroy$))
      .subscribe({
        next: res => {
          if (res.data) {
            this.title = res.data.title
            this.physicallyDocuments = res.data.documents
          } else
            console.error("Kayıt bulunamadı");
        },
        error: err => {
          console.error(err.message);
        }
      });
  }
}
