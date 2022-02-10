import {Component, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup, NgForm, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {NgxSmartModalService} from "ngx-smart-modal";
import "../../../extensions/ng-form.extensions";

@Component({
  selector: 'app-approvals-i-want-new-order-detail',
  templateUrl: './approvals-i-want-new-order-detail.component.html',
  styleUrls: ['./approvals-i-want-new-order-detail.component.scss']
})
export class ApprovalsIWantNewOrderDetailComponent implements OnInit {
  showUpdatePanel: boolean = false;
  showDocumentAddPanel: boolean = false;
  showDocumentUpdatePanel: boolean = false;
  submitted = false;
  approvalSubmitted = false;
  newDocumentSubmitted = false;
  formGroup: FormGroup;
  formGroupApproval: FormGroup;
  formNewDocument: FormGroup;
  files: any[] = [];

  constructor(private fb: FormBuilder, private router: Router, private route: ActivatedRoute, private modal: NgxSmartModalService) {
    this.formNewDocument = this.fb.group({
      choice: ['', Validators.required],
      files: [],
      title: '',
      content: '',
      formDd: null,
      identityNo: '',
      nameSurname: ''
    });
    this.formGroup = this.fb.group({
      title: ['Maaş Ödeme Talimatı', Validators.required],
      process: ['Şube Operasyon', Validators.required],
      step: ['Acil Ödeme', Validators.required],
      processNo: ['845445415', Validators.required],
      validity: ['60 dk'],
      reminderFrequency: ['10 dk', Validators.required],
      reminderCount: ['4'],
    });
    this.formGroupApproval = this.fb.group({
      choice: ['', Validators.required],
      withIdentityNo: '',
      withName: '',
    });
  }

  ngOnInit(): void {
  }

  closeAddPanel() {
    this.formNewDocument.reset();
    this.newDocumentSubmitted = false;
    Object.keys(this.formNewDocument.controls).forEach((key, index) => {
      this.formNewDocument.controls[key].setErrors(null);
    });
    this.showDocumentAddPanel = false;
  }

  get f() {
    return this.formGroup.controls;
  }

  get fa() {
    return this.formGroupApproval.controls;
  }

  get fnd() {
    return this.formNewDocument.controls;
  }

  onSubmit() {
    this.submitted = true;
    if (this.formGroup.valid) {
      this.modal.open('success');
    }
  }

  onSubmitApproval() {
    this.approvalSubmitted = true;
    if (this.formGroupApproval.valid) {
      this.showUpdatePanel = false
    }
  }

  onSubmitAddDocument() {
    this.newDocumentSubmitted = true;
    if (this.formNewDocument.invalid)
      return;
    this.showDocumentAddPanel = false
  }

  choose(e: any) {
    this.formNewDocument.controls.files.setValidators(e === 1 ? [Validators.required, Validators.minLength(11)] : null);
    this.formNewDocument.controls.files.updateValueAndValidity();
    this.formNewDocument.controls.title.setValidators(e === 2 ? [Validators.required] : null);
    this.formNewDocument.controls.title.updateValueAndValidity();
    this.formNewDocument.controls.content.setValidators(e === 2 ? [Validators.required] : null);
    this.formNewDocument.controls.content.updateValueAndValidity();
    this.formNewDocument.controls.formDd.setValidators(e === 3 ? [Validators.required] : null);
    this.formNewDocument.controls.formDd.updateValueAndValidity();
    this.formNewDocument.controls.identityNo.setValidators(e === 3 ? [Validators.required] : null);
    this.formNewDocument.controls.identityNo.updateValueAndValidity();
    this.formNewDocument.controls.nameSurname.setValidators(e === 3 ? [Validators.required] : null);
    this.formNewDocument.controls.nameSurname.updateValueAndValidity();
  }

  onClickUploadDocument(event: any) {
    const files = event.target.files;
    for (let i = 0; i < files.length; i++) {
      this.files.push(files[i]);
    }
  }

  rdoChanged() {
    this.formGroupApproval.controls.withIdentityNo.setValidators(this.formGroupApproval.controls.choice.value === 1 ? [Validators.required, Validators.minLength(11)] : null);
    this.formGroupApproval.controls.withIdentityNo.updateValueAndValidity();
    this.formGroupApproval.controls.withName.setValidators(this.formGroupApproval.controls.choice.value === 2 ? [Validators.required] : null);
    this.formGroupApproval.controls.withName.updateValueAndValidity();
  }
}
