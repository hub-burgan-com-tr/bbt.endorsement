import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-approvals-i-want-new-order',
  templateUrl: './approvals-i-want-new-order.component.html',
  styleUrls: ['./approvals-i-want-new-order.component.scss']
})
export class ApprovalsIWantNewOrderComponent implements OnInit {
  submitted = false;
  formGroup: FormGroup;

  constructor(private fb: FormBuilder, private router: Router, private route: ActivatedRoute) {
    this.formGroup = this.fb.group({
      title: ['', Validators.required],
      process: ['', Validators.required],
      step: ['', Validators.required],
      processNo: ['', Validators.required],
      validity: [''],
      reminderFrequency: ['', Validators.required],
      reminderCount: [''],
    })
  }

  ngOnInit(): void {
  }

  get f() {
    return this.formGroup.controls;
  }

  onSubmit() {
    this.submitted = true;
    if (this.formGroup.valid) {
      this.router.navigate(['../new-order-detail'], {relativeTo: this.route});
    }
  }
}
