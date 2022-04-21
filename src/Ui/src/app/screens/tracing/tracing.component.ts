import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup} from "@angular/forms";
import {TracingService} from "../../services/tracing.service";
import {Subject, takeUntil} from "rxjs";
import {CommonService} from "../../services/common.service";

@Component({
  selector: 'app-tracing',
  templateUrl: './tracing.component.html',
  styleUrls: ['./tracing.component.scss']
})
export class TracingComponent implements OnInit {
  private destroy$ = new Subject();

  data: any;
  dropdownData: any;
  pageSize = 10;
  pageNumber = 1;
  totalPages = 1;
  hasNextPage = true;
  hasPreviousPage = true;
  formGroup: FormGroup;

  constructor(private fb: FormBuilder, private tracingService: TracingService, private commonService: CommonService) {
    this.formGroup = this.fb.group({
      customer: '',
      approver: '',
      process: '',
      state: '',
      processNo: '',
    }, {
      validators: this.atLeastOneHasValue(['customer', 'approver', 'process', 'state', 'processNo']),
      updateOn: 'submit'
    });
  }

  ngOnInit(): void {
    this.initData();
  }

  initData() {
    const model = localStorage.getItem('tracingForm') ? JSON.parse(localStorage.getItem('tracingForm')) : null;
    this.formGroup.patchValue({...model});
    if (model) {
      this.getWatchApproval();
      this.pageNumber = model.pageNumber;
    }
    localStorage.removeItem('tracingForm');
  }

  onSubmit() {
    if (!this.formGroup.valid) {
      return;
    }
    this.getWatchApproval();
  }

  clear(form) {
    form.submitted = false;
    this.formGroup.patchValue({
      customer: '',
      approver: '',
      process: '',
      state: '',
      processNo: '',
    });
    this.data = null;
  }

  atLeastOneHasValue = (fields: Array<string>) => {
    return (group: FormGroup) => {
      for (const fieldName of fields) {
        if (group.get(fieldName)!.value) {
          return null;
        }
      }
      return {atLeastOneValueRequired: true};
    };
  };

  getWatchApproval() {
    const model = {
      customer: this.formGroup.get('customer')?.value,
      approver: this.formGroup.get('approver')?.value,
      process: this.formGroup.get('process')?.value,
      state: this.formGroup.get('state')?.value,
      processNo: this.formGroup.get('processNo')?.value,
      pageNumber: this.pageNumber,
      pageSize: this.pageSize
    };
    localStorage.setItem('tracingForm', JSON.stringify(model));
    this.tracingService.getWatchApproval(model).pipe(takeUntil(this.destroy$)).subscribe({
      next: res => {
        if (res.data) {
          this.data = res.data.items;
          this.pageNumber = res.data.pageNumber;
          this.totalPages = res.data.totalPages;
          this.hasNextPage = res.data.hasNextPage;
          this.hasPreviousPage = res.data.hasPreviousPage;
        } else
          console.error("Kayıt bulunamadı");
      },
      error: err => {
        console.error(err.message);
      }
    });
  }

  changePage(changeValue) {
    this.pageNumber = this.pageNumber + changeValue;
    this.getWatchApproval();
  }

  counter(i: number) {
    return new Array(i);
  }
}
