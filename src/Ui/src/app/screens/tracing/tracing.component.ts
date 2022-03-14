import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-tracing',
  templateUrl: './tracing.component.html',
  styleUrls: ['./tracing.component.scss']
})
export class TracingComponent implements OnInit {
  show = false;
  data = [
    {
      id: 1,
      contractName: 'Sigorta Teklif Talimatı',
      approver: 'Merve Aydın',
      personSeekingApproval: 'Uğur Karataş',
      createdDate: '12 Ocak 2022',
      statusText: 'Bekliyor',
      status: 1,
      file: true
    },
    {
      id: 2,
      contractName: '3. Para Birim Talimatı',
      approver: 'Merve Aydın',
      personSeekingApproval: 'Uğur Karataş',
      createdDate: '12 Ocak 2022',
      statusText: 'Onaylandı',
      status: 2,
      file: false
    },
    {
      id: 3,
      contractName: 'Maaş Ödeme Talimatı',
      approver: 'Merve Aydın',
      personSeekingApproval: 'Uğur Karataş',
      createdDate: '12 Ocak 2022',
      statusText: 'Ret',
      status: 0,
      file: true
    },
    {
      id: 4,
      contractName: '1021 İşlem Onayı',
      approver: 'Merve Aydın',
      personSeekingApproval: 'Uğur Karataş',
      createdDate: '12 Ocak 2022',
      statusText: 'Bekliyor',
      status: 1,
      file: true
    },
    {
      id: 5,
      contractName: 'Avans Ödeme Talimatı',
      approver: 'Merve Aydın',
      personSeekingApproval: 'Uğur Karataş',
      createdDate: '12 Ocak 2022',
      statusText: 'Onaylandı',
      status: 2,
      file: false
    },
    {
      id: 6,
      contractName: 'Maaş Ödeme Talimatı',
      approver: 'Merve Aydın',
      personSeekingApproval: 'Uğur Karataş',
      createdDate: '12 Ocak 2022',
      statusText: 'Ret',
      status: 0,
      file: false
    }
  ];

  formGroup: FormGroup;

  constructor(private fb: FormBuilder) {
    this.formGroup = this.fb.group({
      approving: '',
      seekingApproval: '',
    }, {validators: this.atLeastOneHasValue(['approving', 'seekingApproval']), updateOn: 'submit'});
  }

  ngOnInit(): void {
  }

  onSubmit() {
    this.formGroup.valid ? this.show = true : this.show = false;
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
}
