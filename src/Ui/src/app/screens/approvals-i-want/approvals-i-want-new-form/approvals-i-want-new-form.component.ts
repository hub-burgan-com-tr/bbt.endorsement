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
import {NgSelectConfig} from "@ng-select/ng-select";

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
  tags: any;
  sending: boolean = false;

  constructor(private fb: FormBuilder,
              private modal: NgxSmartModalService,
              private newOrderFormService: NewOrderFormService,
              private route: ActivatedRoute,
              private router: Router, private commonService: CommonService) {
    this.formGroup = this.fb.group({
      tag: ['', [Validators.required]],
      form: ['', [Validators.required]],
      processNo: ['', [Validators.required]],
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

  tagChanged(tags) {
    this.formGroup.patchValue({
      form: ''
    })
    if (tags.length) {
      this.commonService.getTagsFormName(tags).pipe(takeUntil(this.destroy$)).subscribe(res => {
        this.formDropdown = res && res.data;
      });
    } else {
      this.form = undefined;
      this.formDropdown = undefined;
    }
  }

  formChanged(val) {
    if (val) {
      this.formDefinitionId = val;
      this.newOrderFormService.getFormContent(this.formDefinitionId).pipe(takeUntil(this.destroy$)).subscribe(res => {
        const {data} = res;
        this.form = data && JSON.parse(data.content);
        this.formTitle = data && data.title;
      });
    } else {
      this.formDefinitionId = null;
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
    this.sending = true;
    let iTypes = '';
    if (e.data.sigortaTuru) {
      iTypes = Object.keys(e.data.sigortaTuru).map(k => {
        return {text: k, data: e.data.sigortaTuru[k]};
      }).filter(i => i.data == true).map(m => {
        return m.text;
      }).join(',');
      if (!this.person) {
        return;
      }
    }
    if (this.formGroup.valid) {
      const model = new NewApprovalOrderForm(<IApprover>{
        citizenshipNumber: this.person.citizenshipNumber,
        first: this.person.first,
        last: this.person.last,
        clientNumber: this.person.clientNumber
      }, JSON.stringify(e.data), this.formDefinitionId, <IReference>{
        processNo: this.f.processNo.value,
        tagId: this.f.tag.value,
        formId: this.f.form.value,
      }, this.formTitle, iTypes);
      this.newOrderFormService.save(model).pipe(takeUntil(this.destroy$)).subscribe(res => {
        this.modal.open('success');
        this.sending = false;
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
