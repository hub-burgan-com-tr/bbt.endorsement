import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {NewOrderService} from "../../../services/new-order.service";
import {NewApprovalOrder} from "../../../models/new-approval-order";
import {Subject, takeUntil} from "rxjs";
import {CommonService} from "../../../services/common.service";

@Component({
  selector: 'app-approvals-i-want-new-order',
  templateUrl: './approvals-i-want-new-order.component.html',
  styleUrls: ['./approvals-i-want-new-order.component.scss']
})
export class ApprovalsIWantNewOrderComponent implements OnInit {
  submitted = false;
  formGroup: FormGroup;
  model: NewApprovalOrder;
  private destroy$ = new Subject();
  titles;
  process;
  states;

  constructor(private newOrderService: NewOrderService, private fb: FormBuilder, private router: Router, private route: ActivatedRoute, private commonService: CommonService) {
    this.model = new NewApprovalOrder();
    this.formGroup = this.fb.group({
      title: ['', Validators.required],
      reference: this.fb.group({
        process: ['', Validators.required],
        state: [''],
        processNo: [''],
      }),
      config: this.fb.group({
        expireInMinutes: ['60', Validators.required],
        retryFrequence: ['15', Validators.required],
        maxRetryCount: ['3', Validators.required],
      })
    })
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

  get f() {
    return this.formGroup.controls;
  }

  get fr() {
    return (<FormGroup>this.formGroup.controls.reference).controls;
  }

  get fc() {
    return (<FormGroup>this.formGroup.controls.config).controls;
  }

  onSubmit() {
    this.submitted = true;
    if (this.formGroup.valid) {
      this.model = {...this.model, ...this.formGroup.getRawValue()};
      this.newOrderService.setModel(this.model);
      this.router.navigate(['../new-order-detail'], {relativeTo: this.route});
    }
  }

  addTag(tag: string) {
    return tag;
  }
}
