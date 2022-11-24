import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgxSmartModalService } from "ngx-smart-modal";
import { AbstractControl, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Components, FormioComponent, FormioOptions } from "@formio/angular";
import { NewOrderFormService } from "../../../services/new-order-form.service";
import { Subject, takeUntil } from "rxjs";
import { ActivatedRoute, Router } from "@angular/router";
import NewApprovalOrderForm from "../../../models/new-approval-order-form";
import { IApprover } from "../../../models/approver";
import { IReference } from "../../../models/reference";
import { CommonService } from "../../../services/common.service";
import { NgSelectConfig } from "@ng-select/ng-select";

@Component({
  selector: 'app-approvals-i-want-new-form',
  templateUrl: './approvals-i-want-new-form.component.html',
  styleUrls: ['./approvals-i-want-new-form.component.scss']
})

export class ApprovalsIWantNewFormComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject();
  person;
  form;
  source;
  formTitle;
  formDefinitionId;
  @ViewChild(FormioComponent, { static: false }) formio: FormioComponent;
  formGroup: FormGroup;
  submitted = false;
  options: FormioOptions = {
    disableAlerts: true,
  }
  personName;
  fileBase64: any;
  formDropdown: any;
  applicationForms: any;
  tags: any;
  sending: boolean = false;
  pdfContent: string;
  waiting: boolean = false;
  isPreview: boolean = true;
  isProcessRequired: boolean = false;

  constructor(private fb: FormBuilder,
    private modal: NgxSmartModalService,
    private newOrderFormService: NewOrderFormService,
    private route: ActivatedRoute,
    private router: Router, private commonService: CommonService) {
    this.formGroup = this.fb.group({
      tag: ['', [Validators.required]],
      form: ['', [Validators.required]],
      processNo: [''],
      file: [''],
      fileType: [''],
      dependencyOrderId: ['']
    });
    this.route.queryParams.pipe(takeUntil(this.destroy$)).subscribe(params => {
      this.formDefinitionId = params['formDefinitionId'];
    });
  }

  ngOnInit(): void {
    this.commonService.getTags().pipe(takeUntil(this.destroy$)).subscribe(res => {
      this.tags = res.data;
    });
  }
  preview() {
    this.next();
  }

  tagChanged(tags) {
    this.isProcessRequired = false;
    this.formGroup.patchValue({
      form: ''
    })
    if (tags.length) {
      tags.forEach(el => {
        if (this.tags.find(tag => tag.formDefinitionTagId == el).isProcessNo) {
          this.isProcessRequired = true;
          return;
        }
      });
      this.commonService.getTagsFormName(tags).pipe(takeUntil(this.destroy$)).subscribe(res => {
        this.formDropdown = res && res.data;
      });
    } else {
      this.form = undefined;
      this.formDropdown = undefined;
    }

    if (this.isProcessRequired) {
      this.formGroup.controls.processNo.setValidators([Validators.required]);
      this.formGroup.controls.processNo.updateValueAndValidity();
    } else {
      this.formGroup.controls.processNo.setValidators(null);
      this.formGroup.controls.processNo.updateValueAndValidity();
    }
  }

  formLoaded() {
    this.setFormIoData();
    this.processNoChange();
  }

  formChanged(val) {
    this.isPreview = true;
    this.waiting = false;
    this.sending = false;
    this.f.file.setValue(null);
    this.f.file.updateValueAndValidity();
    this.fileBase64 = '';
    if (val) {
      this.formDefinitionId = val;
      this.newOrderFormService.getFormContent(this.formDefinitionId).pipe(takeUntil(this.destroy$)).subscribe(res => {
        const { data } = res;
        this.form = data && JSON.parse(data.content);
        this.source = data && data.source;
        if (this.source === 'file') {
          this.f.file.addValidators(Validators.required);
        } else {
          this.f.file.clearValidators();
        }
        this.f.file.updateValueAndValidity();
        this.formTitle = data && data.title;
        // this.setFormIoData();
        this.getOrderByFormId();
      });
    } else {
      this.formDefinitionId = null;
    }
  }

  processNoChange() {
    if (this.formio && this.source != 'file')
      this.formio.formio.getComponent('FormInstance_Transaction_Id').setValue(this.f.processNo?.value);
  }

  setFormIoData() {
    if (this.formio && this.source != 'file' && this.person) {
      this.formio.formio.getComponent('FormInstance_Approver_Fullname').setValue(`${this.person.first} ${this.person.last}`);
      this.formio.formio.getComponent('FormInstance_Approver_CitizenshipNumber').setValue(this.person.citizenshipNumber);
    }
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  getPersonFromChild(person) {
    this.person = JSON.parse(person);
    this.setFormIoData();
    this.getOrderByFormId();
  }

  get f() {
    return this.formGroup.controls;
  }

  redirectToList() {
    this.router.navigate(['approvals-i-want']);
  }

  convertFileToBase64(event: any) {
    const reader = new FileReader();
    if (event.target.files && event.target.files.length) {
      const [file] = event.target.files;
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.fileBase64 = reader.result;
        this.formGroup.patchValue({
          fileType: file.type
        });
      };
    } else {
      this.fileBase64 = '';
      this.formGroup.patchValue({
        fileType: ''
      });
    }
  }

  getOrderByFormId() {
    if (this.f.form.value && this.person && this.person.citizenshipNumber && this.source === 'file') {
      this.f.dependencyOrderId.addValidators(Validators.required);
      const model = {
        formDefinitionId: this.f.form.value,
        citizenshipNumber: this.person.citizenshipNumber,
        approver: {
          citizenshipNumber: this.person.citizenshipNumber,
          first: this.person.first,
          last: this.person.last,
          customerNumber: this.person.customerNumber,
          businessLine: this.person.businessLine,
          branchCode: this.person.branchCode
        }
      };
      this.newOrderFormService.getOrderByFormId(model).pipe(takeUntil(this.destroy$)).subscribe(res => {
        this.applicationForms = res.data;
      })
    } else {
      this.f.dependencyOrderId.clearValidators();
    }
    this.f.dependencyOrderId.updateValueAndValidity();
  }

  submitForm(e) {
    if (this.formGroup.valid) {
      this.sending = true;
      let iTypes = '';
      if (e && e.data.sigortaTuru) {
        iTypes = Object.keys(e.data.sigortaTuru).map(k => {
          return { text: k, data: e.data.sigortaTuru[k] };
        }).filter(i => i.data == true).map(m => {
          return m.text;
        }).join(',');
        if (!this.person) {
          return;
        }
      }
      if (e && e.data.sigortaEttirenIleSigortaliAyniKisidir) {
        e.data.SigortaEttirenAd = e.data.FormInstance_Approver_Fullname;
        e.data.SigortaEttirenVKN = e.data.FormInstance_Approver_CitizenshipNumber;
      }
      const model = new NewApprovalOrderForm(<IApprover>{
        citizenshipNumber: this.person.citizenshipNumber,
        first: this.person.first,
        last: this.person.last,
        customerNumber: this.person.customerNumber,
        businessLine: this.person.businessLine,
        branchCode: this.person.branchCode,
      }, this.source === 'file' ? this.fileBase64 : JSON.stringify(e.data), this.f.fileType.value, this.formDefinitionId, <IReference>{
        processNo: this.f.processNo.value,
        tagId: this.f.tag.value,
        formId: this.f.form.value,
      }, this.formTitle, iTypes, this.source, this.f.dependencyOrderId.value);
      if (this.isPreview) {
        this.waiting = true;
        if (this.fileBase64) {
          this.pdfContent = this.fileBase64;
          this.modal.open('document');
          this.waiting = false;
          this.sending = false;
          this.isPreview = false;
        } else {
          this.newOrderFormService.preview(model).pipe(takeUntil(this.destroy$)).subscribe({
            next: (res) => {
              this.pdfContent = res && res.data && res.data.content;
              this.modal.open('document');
              this.waiting = true;
              this.sending = false;
              this.isPreview = false;
            },
            error: () => {
              this.sending = false;
            }
          })
        }
      } else {
        this.newOrderFormService.save(model).pipe(takeUntil(this.destroy$)).subscribe({
          next: () => {
            this.modal.open('success');
            this.sending = false;
          },
          error: () => {
            this.sending = false;
          }
        })
      }
    }
  }

  next() {
    this.submitted = true;
    if (this.formio) {
      this.formio.formio.emit('submitButton');
    } else {
      this.submitForm(null);
    }
  }
}
