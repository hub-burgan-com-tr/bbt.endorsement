import {Component, OnInit, ViewChild} from '@angular/core';
import {FormArray, FormBuilder, FormControl, FormGroup, NgForm, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {NgxSmartModalService} from "ngx-smart-modal";
import "../../../extensions/ng-form.extensions";
import {NewOrderService} from "../../../services/new-order.service";
import {NewApprovalOrder} from "../../../models/new-approval-order";

@Component({
  selector: 'app-approvals-i-want-new-order-detail',
  templateUrl: './approvals-i-want-new-order-detail.component.html',
  styleUrls: ['./approvals-i-want-new-order-detail.component.scss']
})
export class ApprovalsIWantNewOrderDetailComponent implements OnInit {
  showUpdatePanel: boolean = false;
  showDocumentAddPanel: boolean = false;
  panelTitle: string = 'Yeni Belge Ekle';
  isEditing: boolean = false;
  submitted = false;
  approvalSubmitted = false;
  newDocumentSubmitted = false;
  optionsHasError = false;
  formGroup: FormGroup;
  formGroupApproval: FormGroup;
  formNewDocument: FormGroup;
  files: File[] = [];
  approvalFormValidationMessage = '';
  showChoiceAddPanel: boolean = false;
  model: NewApprovalOrder;
  selectedDocumentIndex: number;
  approvalButtonText: string = 'Kaydet';

  constructor(private newOrderService: NewOrderService, private fb: FormBuilder, private router: Router, private route: ActivatedRoute, private modal: NgxSmartModalService) {
    this.initModel();
    this.formNewDocument = this.fb.group({
      documentType: ['', Validators.required],
      options: this.fb.array([]),
      files: [],
      title: '',
      content: '',
      formId: '',
      identityNo: '',
      nameSurname: '',
      choiceText: ''
    });
    this.formGroup = this.fb.group({
      title: [this.model.title, Validators.required],
      process: [this.model.process, Validators.required],
      step: [this.model.step, Validators.required],
      processNo: [this.model.processNo, Validators.required],
      validity: [this.model.validity],
      reminderFrequency: [this.model.reminderFrequency, Validators.required],
      reminderCount: [this.model.reminderCount],
    });
    this.formGroupApproval = this.fb.group({
      type: ['', Validators.required],
      value: ''
    });
  }

  ngOnInit(): void {
  }

  initModel() {
    this.model = this.newOrderService.getModel();
    this.model.documents = this.model.documents.filter(i => i.documentType != 1);
    this.approvalButtonText = this.model.approver && this.model.approver.nameSurname ? 'Güncelle' : 'Kaydet';
    this.newOrderService.setModel(this.model);
  }

  getDocumentName(documentType: number, i: any) {
    switch (documentType) {
      case 1:
        return i.files.map(item => item.name).join(', ');
      case 2:
        return i.title;
      case 3:
        return 'Sigorta Onay Formu';
      default:
        return '';
    }
  }

  getState(i: string) {
    if (i === '1')
      return 'ONAY';
    return 'RET';
  }

  getType(i: number) {
    switch (i) {
      case 1:
        return 'Belge';
      case 2:
        return 'Metin';
      case 3:
        return 'Form';
      default:
        return '';
    }
  }

  addChoice() {
    if (!this.formNewDocument.get('choiceText')?.value)
      return;
    (<FormArray>this.formNewDocument.get('options')).push(this.fb.group({
      title: this.formNewDocument.get('choiceText')?.value,
      choice: '1'
    }));
    this.formNewDocument.get('choiceText')?.setValue('');
    this.optionsHasError = false;
  }

  deleteChoice(i: number) {
    (<FormArray>this.formNewDocument.get('options')).controls.splice(i, 1);
  }

  editDocument(document: any, index: number) {
    this.isEditing = true;
    this.selectedDocumentIndex = index;
    this.formNewDocument.patchValue({
      documentType: document.documentType,
      title: document.title,
      content: document.content,
      formId: document.formId,
      identityNo: document.identityNo,
      nameSurname: document.nameSurname
    });
    document.options.forEach(k => {
      (<FormArray>this.formNewDocument.get('options')).push(this.fb.group({
        title: k.title,
        choice: k.choice
      }))
    });
    this.formNewDocument.get('options').updateValueAndValidity();
    this.files = document.files;
    this.panelTitle = 'Belgeyi Düzenle';
    this.formNewDocument.controls.files.setValidators(null);
    this.formNewDocument.controls.files.updateValueAndValidity();
    this.showDocumentAddPanel = true;
  }

  closeAddPanel() {
    this.formNewDocument.reset();
    this.newDocumentSubmitted = false;
    this.isEditing = false;
    this.panelTitle = 'Yeni Belge Ekle';
    Object.keys(this.formNewDocument.controls).forEach((key, index) => {
      this.formNewDocument.controls[key].setErrors(null);
      if (key != 'options')
        this.formNewDocument.controls[key].setValue('');
      else {
        (<FormArray>this.formNewDocument.controls[key]) = this.fb.array([]);
      }
    });
    this.files = [];
    this.showDocumentAddPanel = false;
  }


  deleteDocument() {
    this.model.documents.splice(this.selectedDocumentIndex, 1);
    this.newOrderService.setModel(this.model);
    this.modal.close('documentDelete');
  }

  deleteDocumentModal(i: number) {
    this.selectedDocumentIndex = i;
    this.modal.open('documentDelete');
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

  getOptions() {
    return (<FormArray>this.formNewDocument.controls.options).controls;
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
      this.model.approver = {
        type: this.fa.type.value,
        value: this.fa.value.value,
        nameSurname: 'Uğur Karataş'
      }
      this.approvalButtonText = 'Güncelle';
      this.newOrderService.setModel(this.model);
      this.closeApproverPanel();
    }
  }

  closeApproverPanel() {
    this.formGroupApproval.reset();
    this.approvalSubmitted = false;
    Object.keys(this.formGroupApproval.controls).forEach((key, index) => {
      this.formGroupApproval.controls[key].setErrors(null);
      this.formGroupApproval.controls[key].setValue('');
    });
    this.showUpdatePanel = false;
  }

  onSubmitAddDocument() {
    this.newDocumentSubmitted = true;
    if (!(<FormArray>this.formNewDocument.get('options')).length) {
      this.optionsHasError = true;
      return;
    }
    if (this.formNewDocument.invalid)
      return;

    if (this.isEditing) {
      this.model.documents[this.selectedDocumentIndex] = Object.assign({}, {
        ...this.formNewDocument.getRawValue(),
        files: [...this.files]
      })
    } else {
      this.model.documents.push({
        ...this.formNewDocument.getRawValue(),
        files: [...this.files]
      });
    }
    this.newOrderService.setModel(this.model);
    this.closeAddPanel();
  }

  choose(e: any) {
    if (!this.isEditing) {
      this.formNewDocument.controls.files.setValidators(e === 1 ? [Validators.required] : null);
      this.formNewDocument.controls.files.updateValueAndValidity();
    }
    this.formNewDocument.controls.title.setValidators(e === 2 ? [Validators.required] : null);
    this.formNewDocument.controls.title.updateValueAndValidity();
    this.formNewDocument.controls.content.setValidators(e === 2 ? [Validators.required] : null);
    this.formNewDocument.controls.content.updateValueAndValidity();
    this.formNewDocument.controls.formId.setValidators(e === 3 ? [Validators.required] : null);
    this.formNewDocument.controls.formId.updateValueAndValidity();
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
    if (this.formGroupApproval.controls.type.value === 1) {
      this.approvalFormValidationMessage = 'TCKN girilmelidir.'
      this.formGroupApproval.controls.value.setValidators([Validators.required, Validators.minLength(11)]);
      this.formGroupApproval.controls.value.updateValueAndValidity();
    } else {
      this.approvalFormValidationMessage = 'Müşteri No girilmelidir.';
      this.formGroupApproval.controls.value.setValidators(Validators.required);
      this.formGroupApproval.controls.value.updateValueAndValidity();
    }
  }
}
