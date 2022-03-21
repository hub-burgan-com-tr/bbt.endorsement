import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {NgxSmartModalService} from "ngx-smart-modal";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Components, FormioComponent, FormioOptions} from "@formio/angular";
import {NewOrderFormService} from "../../../services/new-order-form.service";
import {Subject, takeUntil} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";
import NewApprovalOrderForm from "../../../models/new-approval-order-form";
import content = Components.components.content;
import {IApprover} from "../../../models/approver";
import {IReference} from "../../../models/reference";

@Component({
  selector: 'app-approvals-i-want-new-form',
  templateUrl: './approvals-i-want-new-form.component.html',
  styleUrls: ['./approvals-i-want-new-form.component.scss']
})
export class ApprovalsIWantNewFormComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject();
  form;
  formTitle;
  formDefinitionId;
  @ViewChild(FormioComponent, {static: false}) formio: FormioComponent;
  formGroup: FormGroup;
  submitted = false;
  options: FormioOptions = {
    disableAlerts: true,
  }
  approvalFormValidationMessage = '';

  constructor(private fb: FormBuilder, private modal: NgxSmartModalService, private newOrderFormService: NewOrderFormService, private route: ActivatedRoute, private router: Router) {
    this.formGroup = this.fb.group({
      process: ['', [Validators.required]],
      state: ['', [Validators.required]],
      processNo: ['', [Validators.required]],
      type: ['', [Validators.required]],
      value: '',
    });
    this.route.queryParams.pipe(takeUntil(this.destroy$)).subscribe(params => {
      this.formDefinitionId = params['formDefinitionId'];
    });
  }

  ngOnInit(): void {
    this.newOrderFormService.getFormContent(this.formDefinitionId).pipe(takeUntil(this.destroy$)).subscribe(res => {
      const {data} = res;
      this.form = data && JSON.parse(data.content);
      this.formTitle = data && data.title;
    });
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  get f() {
    return this.formGroup.controls;
  }

  rdoChanged() {
    if (this.f.type.value === 1) {
      this.approvalFormValidationMessage = 'TCKN girilmelidir.'
      this.f.value.setValidators([Validators.required, Validators.minLength(11)]);
      this.f.value.updateValueAndValidity();
    } else {
      this.approvalFormValidationMessage = 'Müşteri No girilmelidir.';
      this.f.value.setValidators(Validators.required);
      this.f.value.updateValueAndValidity();
    }
  }

  redirectToList() {
    this.router.navigate(['../']);
  }

  submitForm(e) {
    if (this.formGroup.valid) {
      //@TODO
      //Ad Soyad servisten çekilip gönderilecek
      const model = new NewApprovalOrderForm(<IApprover>{
        type: this.f.type.value,
        value: this.f.value.value,
        nameSurname: 'Uğur Karataş'
      }, JSON.stringify(e.data), this.formDefinitionId, <IReference>{
        process: this.f.process.value,
        processNo: this.f.processNo.value,
        state: this.f.state.value
      }, this.formTitle);
      this.newOrderFormService.save(model).pipe(takeUntil(this.destroy$)).subscribe(res => {
        this.modal.open('success');
      })
    }
  }

  next() {
    this.submitted = true;
    if (this.formio) {
      this.formio.formio.emit('submitButton');
    }
  }
}
