import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {FormArray, FormBuilder, FormControl, FormGroup, NgForm, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {NgxSmartModalService} from "ngx-smart-modal";
import "../../../extensions/ng-form.extensions";
import {NewOrderService} from "../../../services/new-order.service";
import {NewApprovalOrder} from "../../../models/new-approval-order";
import {Subject, takeUntil} from "rxjs";
import {CommonService} from "../../../services/common.service";

@Component({
  selector: 'app-approvals-i-want-new-order-detail',
  templateUrl: './approvals-i-want-new-order-detail.component.html',
  styleUrls: ['./approvals-i-want-new-order-detail.component.scss']
})
export class ApprovalsIWantNewOrderDetailComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject();
  showUpdatePanel: boolean = false;
  showDocumentAddPanel: boolean = false;
  panelTitle: string = 'Yeni Belge Ekle';
  isEditing: boolean = false;
  submitted = false;
  errorMessage;
  selectedFileName;
  approvalSubmitted = false;
  newDocumentSubmitted = false;
  actionsHasError = false;
  formGroup: FormGroup;
  formNewDocument: FormGroup;
  showChoiceAddPanel: boolean = false;
  model: NewApprovalOrder;
  selectedDocumentIndex: number;
  approvalButtonText: string = 'Kaydet';
  person;
  titles;
  process;
  states;

  constructor(private newOrderService: NewOrderService, private fb: FormBuilder, private router: Router, private route: ActivatedRoute, private modal: NgxSmartModalService, private commonService: CommonService) {
    this.initModel();
    this.formNewDocument = this.fb.group({
      type: ['', Validators.required],
      actions: this.fb.array([]),
      file: '',
      fileName: '',
      fileType: '',
      title: '',
      content: '',
      formId: '',
      identityNo: '',
      nameSurname: '',
      choiceText: ''
    });
    this.formGroup = this.fb.group({
      title: [this.model.title, Validators.required],
      reference: this.fb.group({
        process: [this.model.reference.process, Validators.required],
        state: [this.model.reference.state],
        processNo: [this.model.reference.processNo, Validators.required],
      }),
      config: this.fb.group({
        expireInMinutes: [this.model.config.expireInMinutes, Validators.required],
        retryFrequence: [this.model.config.retryFrequence, Validators.required],
        maxRetryCount: [this.model.config.maxRetryCount, Validators.required],
      })
    });
  }

  ngOnInit(): void {
    this.newOrderService.getOrderFormParameters().pipe(takeUntil(this.destroy$)).subscribe(res => {
      if (res && res.data) {
        this.titles = res.data.titles;
        this.process = res.data.process;
        this.states = res.data.states;
      }
    });
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  getPersonFromChild(person) {
    this.person = JSON.parse(person);
    console.log(this.person);
  }

  initModel() {
    this.model = this.newOrderService.getModel();
    this.model.documents = this.model.documents.filter(i => i.type != 1);
    this.approvalButtonText = this.model.approver && this.model.approver.first ? 'Güncelle' : 'Müşteri Ekle';
    this.newOrderService.setModel(this.model);
  }

  getDocumentName(type: number, i: any) {
    switch (type) {
      case 1:
        return i.fileName;
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
    (<FormArray>this.formNewDocument.get('actions')).push(this.fb.group({
      title: this.formNewDocument.get('choiceText')?.value,
      choice: '1'
    }));
    this.formNewDocument.get('choiceText')?.setValue('');
    this.actionsHasError = false;
  }

  deleteChoice(i: number) {
    (<FormArray>this.formNewDocument.get('actions')).controls.splice(i, 1);
  }

  editDocument(document: any, index: number) {
    this.isEditing = true;
    this.selectedDocumentIndex = index;
    this.formNewDocument.patchValue({
      type: document.type,
      title: document.title,
      content: document.content,
      formId: document.formId,
      identityNo: document.identityNo,
      nameSurname: document.nameSurname,
      file: document.file,
      fileName: document.fileName
    });
    this.selectedFileName = document.fileName;
    document.actions.forEach(k => {
      (<FormArray>this.formNewDocument.get('actions')).push(this.fb.group({
        title: k.title,
        choice: k.choice
      }))
    });

    this.formNewDocument.get('actions').updateValueAndValidity();
    this.panelTitle = 'Belgeyi Düzenle';
    this.formNewDocument.controls.file.setValidators(null);
    this.formNewDocument.controls.file.updateValueAndValidity();
    this.showDocumentAddPanel = true;
  }

  closeAddPanel() {
    this.formNewDocument.reset();
    this.newDocumentSubmitted = false;
    this.isEditing = false;
    this.panelTitle = 'Yeni Belge Ekle';
    this.selectedFileName = '';
    Object.keys(this.formNewDocument.controls).forEach((key, index) => {
      this.formNewDocument.controls[key].setErrors(null);
      if (key != 'actions')
        this.formNewDocument.controls[key].setValue('');
      else {
        (<FormArray>this.formNewDocument.controls[key]) = this.fb.array([]);
      }
    });
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

  get fr() {
    return (<FormGroup>this.formGroup.controls.reference).controls;
  }

  get fc() {
    return (<FormGroup>this.formGroup.controls.config).controls;
  }

  get fnd() {
    return this.formNewDocument.controls;
  }

  getActions() {
    return (<FormArray>this.formNewDocument.controls.actions).controls;
  }

  redirectToList() {
    this.router.navigate(['approvals-i-want']);
  }

  onSubmit() {
    if (this.model.documents.length === 0) {
      this.errorMessage = 'Belge eklemeden ilerleyemezsiniz.';
      this.modal.open('error');
      return;
    }
    if (!this.person) {
      this.errorMessage = 'Müşteri eklemeden ilerleyemezsiniz.';
      this.modal.open('error');
      return;
    }
    this.submitted = true;
    if (this.formGroup.valid) {
      this.model = {...this.model, ...this.formGroup.getRawValue()};
      this.model.documents.map(x => {
        x.content = x.file != '' && x.type == 1 ? x.file : x.content;
        x.title = x.fileName != '' && x.type == 1 ? x.fileName : x.title;
      })
      this.model.config.expireInMinutes = parseInt(this.model.config.expireInMinutes);
      this.model.config.maxRetryCount = parseInt(this.model.config.maxRetryCount);
      this.model.config.retryFrequence = parseInt(this.model.config.retryFrequence);
      this.newOrderService.save(this.model).pipe(takeUntil(this.destroy$)).subscribe(res => {
        this.modal.open('success');
      });
    }
  }


  onSubmitApproval() {
    this.approvalSubmitted = true;
    if (!this.person)
      return;
    this.model.approver = {
      citizenshipNumber: this.person.citizenshipNumber,
      first: this.person.first,
      last: this.person.last,
      clientNumber: this.person.clientNumber
    }
    this.approvalButtonText = 'Güncelle';
    this.newOrderService.setModel(this.model);
    this.closeApproverPanel();
  }

  closeApproverPanel() {
    this.approvalSubmitted = false;
    this.showUpdatePanel = false;
  }

  onSubmitAddDocument() {
    this.newDocumentSubmitted = true;
    if (!(<FormArray>this.formNewDocument.get('actions')).length) {
      this.actionsHasError = true;
      return;
    }
    if (this.formNewDocument.invalid)
      return;

    if (this.isEditing) {
      this.model.documents[this.selectedDocumentIndex] = Object.assign({}, {
        ...this.formNewDocument.getRawValue()
      })
    } else {
      this.model.documents.push({
        ...this.formNewDocument.getRawValue()
      });
    }
    this.newOrderService.setModel(this.model);
    this.closeAddPanel();
  }

  choose(e: any) {
    if (!this.isEditing) {
      this.formNewDocument.controls.file.setValidators(e === 1 ? [Validators.required] : null);
      this.formNewDocument.controls.file.updateValueAndValidity();
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
    const reader = new FileReader();
    if (event.target.files && event.target.files.length) {
      const [file] = event.target.files;
      this.selectedFileName = file.name;
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.formNewDocument.patchValue({
          file: reader.result,
          fileName: this.selectedFileName,
          fileType: file.type
        });
      };
    }
  }

  addTag(tag: string) {
    return tag;
  }
}
