import {Component, OnInit} from '@angular/core';
import {Subject, takeUntil} from "rxjs";
import {NgxSmartModalService} from "ngx-smart-modal";
import {ActivatedRoute, Router} from "@angular/router";
import {MyApprovalService} from "../../../services/my-approval.service";
import {
  GetApprovalDetailRequestModel,
  GetApprovalPhysicallyDocumentDetailRequestModel
} from "../../../models/my-approval";
import {NgForm} from "@angular/forms";

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
    choice: null,
    actions: [],
    type: '',
    mimeType: ''
  }];
  physicallyDocuments = [{
    name: '',
    documentLink: '',
    content: '',
    actions: []
  }];
  showError: boolean = false;

  constructor(public ngxSmartModalService: NgxSmartModalService,
              private route: ActivatedRoute,
              private myApprovalService: MyApprovalService,
              private router: Router) {
  }

  ngOnInit(): void {
    this.route.params.pipe(takeUntil(this.destroy$)).subscribe(params => {
      this.orderId = params['orderId'];
    });
    this.getApprovalDetail();
  }

  continue(f: NgForm) {
    if (!f.valid) {
      this.showError = true;
      return;
    }
    this.showError = false;
    this.step++;
    if (this.step >= (this.details.length - 1)) {
      this.buttonText = 'Kaydet';
    }
    //Post data
    if (this.step > (this.details.length - 1)) {
      this.step--;
      this.ngxSmartModalService.open('sendModal');
      return;
    }

  }

  redirectToList() {
    this.router.navigate(['/i-approve/detail'], {queryParams: {orderId: this.orderId}});
  }

  send() {
    const model = {
      orderId: this.orderId,
      documents: []
    };
    this.details.forEach(i => {
      let actionId = '';
      if (i.choice === true) {
        actionId = i.actions[0].value;
      } else {
        actionId = i.actions.find(f => f.value == i.choice)?.documentActionId;
      }
      if (actionId) {
        model.documents.push({
          documentId: i.documentId,
          actionId: actionId,
        });
      }
    });
    this.myApprovalService.saveApproveOrderDocument(model).pipe(takeUntil(this.destroy$)).subscribe(res => {
      this.ngxSmartModalService.close('sendModal');
      this.ngxSmartModalService.open('confirmModal');
    });
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
}
