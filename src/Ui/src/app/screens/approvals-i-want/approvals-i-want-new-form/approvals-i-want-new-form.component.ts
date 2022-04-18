import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {NgxSmartModalService} from "ngx-smart-modal";
import {AbstractControl, FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Components, FormioComponent, FormioOptions} from "@formio/angular";
import {NewOrderFormService} from "../../../services/new-order-form.service";
import {Subject, takeUntil} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";
import NewApprovalOrderForm from "../../../models/new-approval-order-form";
import {IApprover} from "../../../models/approver";
import {IReference} from "../../../models/reference";
import {CommonService} from "../../../services/common.service";

@Component({
  selector: 'app-approvals-i-want-new-form',
  templateUrl: './approvals-i-want-new-form.component.html',
  styleUrls: ['./approvals-i-want-new-form.component.scss']
})

export class ApprovalsIWantNewFormComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject();
  person;
  form;
  formTitle;
  formDefinitionId;
  @ViewChild(FormioComponent, {static: false}) formio: FormioComponent;
  formGroup: FormGroup;
  submitted = false;
  options: FormioOptions = {
    disableAlerts: true,
  }
  personName;
  dropdownData: any;
  formDropdown: any;

  constructor(private fb: FormBuilder,
              private modal: NgxSmartModalService,
              private newOrderFormService: NewOrderFormService,
              private route: ActivatedRoute,
              private router: Router, private commonService: CommonService) {
    this.formGroup = this.fb.group({
      process: ['', [Validators.required]],
      state: [''],
      processNo: ['', [Validators.required]],
      form: ['']
    });
    this.route.queryParams.pipe(takeUntil(this.destroy$)).subscribe(params => {
      this.formDefinitionId = params['formDefinitionId'];
    });
  }

  ngOnInit(): void {
    this.commonService.getProcessAndState().pipe(takeUntil(this.destroy$)).subscribe(res => {
      this.dropdownData = res.data;
    });
    this.newOrderFormService.getForm().pipe(takeUntil(this.destroy$)).subscribe(res => {
      this.formDropdown = res && res.data;
    })
  }
  formChanged(val){
    if (val){
      this.newOrderFormService.getFormContent(val).pipe(takeUntil(this.destroy$)).subscribe(res => {
        const {data} = res;
        this.form = data && JSON.parse(data.content);
        this.formTitle = data && data.title;
      });
    }
  }
  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  getPersonFromChild(person) {
    this.person = JSON.parse(person);
  }

  get f() {
    return this.formGroup.controls;
  }

  redirectToList() {
    this.router.navigate(['approvals-i-want']);
  }

  submitForm(e) {
    if (!this.person) {
      return;
    }
    if (this.formGroup.valid) {
      //@TODO
      //Ad Soyad servisten çekilip gönderilecek
      const model = new NewApprovalOrderForm(<IApprover>{
        citizenshipNumber: this.person.citizenshipNumber,
        first: this.person.first,
        last: this.person.last
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
