import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {NewOrderService} from "../../../services/new-order.service";
import {NewApprovalOrder} from "../../../models/new-approval-order";

@Component({
  selector: 'app-approvals-i-want-new-order',
  templateUrl: './approvals-i-want-new-order.component.html',
  styleUrls: ['./approvals-i-want-new-order.component.scss']
})
export class ApprovalsIWantNewOrderComponent implements OnInit {
  submitted = false;
  formGroup: FormGroup;
  model: NewApprovalOrder;

  constructor(private newOrderService: NewOrderService, private fb: FormBuilder, private router: Router, private route: ActivatedRoute) {
    this.model = new NewApprovalOrder();
    this.formGroup = this.fb.group({
      title: ['', Validators.required],
      reference: this.fb.group({
        process: ['', Validators.required],
        state: ['', Validators.required],
        processNo: ['', Validators.required],
      }),
      config: this.fb.group({
        expireInMinutes: [''],
        retryFrequence: ['', Validators.required],
        maxRetryCount: [''],
      })
    })
  }

  ngOnInit(): void {
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
}
