import {Component, OnInit} from '@angular/core';
import {NgxSmartModalService} from "ngx-smart-modal";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-approvals-i-want-new-form',
  templateUrl: './approvals-i-want-new-form.component.html',
  styleUrls: ['./approvals-i-want-new-form.component.scss']
})
export class ApprovalsIWantNewFormComponent implements OnInit {
  step: number = 1;
  buttonText: string = 'Kaydet'
  subTitle: string = 'Sigorta Onay Belgesi';
  titles = [
    'Sigorta Onay Belgesi',
    'Sigorta Onay Belgesi | Onay'
  ];
  buttonTexts = [
    'Kaydet',
    'Onaya Gönder'
  ];
  formGroup: FormGroup;
  submitted = false;

  constructor(private fb: FormBuilder, private modal: NgxSmartModalService) {
    this.formGroup = this.fb.group({
      process: '',
      stage: '',
      processNo: '',
      identityNo: '',
      name: '',
      choice: '',
      withIdentityNo: '',
      withName: '',
    })
  }

  ngOnInit(): void {
  }

  get f() {
    return this.formGroup.controls;
  }

  rdoChanged() {
    this.formGroup.controls.withIdentityNo.setValidators(this.formGroup.controls.choice.value === 1 ? [Validators.required, Validators.minLength(11)] : null);
    this.formGroup.controls.withIdentityNo.updateValueAndValidity();
    this.formGroup.controls.withName.setValidators(this.formGroup.controls.choice.value === 2 ? [Validators.required] : null);
    this.formGroup.controls.withName.updateValueAndValidity();
  }

  cancel() {
    if (this.step > 1) {
      this.step--;
      this.subTitle = this.titles[this.step - 1];
      this.buttonText = this.buttonTexts[this.step - 1];
    }
  }

  next() {
    this.submitted = true;

    let isInvalid = false;
    this.formGroup.controls['identityNo'].addValidators([Validators.required, Validators.minLength(11)]);
    this.formGroup.controls['identityNo'].updateValueAndValidity();
    this.formGroup.controls['name'].addValidators(Validators.required);
    this.formGroup.controls['name'].updateValueAndValidity();
    this.formGroup.controls['choice'].addValidators(Validators.required);
    this.formGroup.controls['choice'].updateValueAndValidity();
    isInvalid = this.formGroup.invalid;
    if (isInvalid)
      return;

    this.step++;

    if (this.step == 3) {
      this.modal.open('success');
      return;
    } else {
      this.subTitle = this.titles[this.step - 1];
      this.buttonText = this.buttonTexts[this.step - 1];
    }
  }
}
