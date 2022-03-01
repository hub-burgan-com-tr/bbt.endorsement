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
      title: 'Sigorta Teklif Talimatı',
      name: 'Uğur KARATAŞ, 48545454545',
      date: '12 Ocak 2022',
      statusText: 'Bekliyor',
      status: 1,
      file: true
    },
    {
      id: 2,
      title: '3. Para Birim Talimatı',
      name: 'Uğur KARATAŞ, 48545454545',
      date: '12 Ocak 2022',
      statusText: 'Onaylandı',
      status: 2,
      file: false
    },
    {
      id: 3,
      title: 'Maaş Ödeme Talimatı',
      name: 'Uğur KARATAŞ, 48545454545',
      date: '12 Ocak 2022',
      statusText: 'Ret',
      status: 0,
      file: true
    },
    {
      id: 4,
      title: '1021 İşlem Onayı',
      name: 'Uğur KARATAŞ, 48545454545',
      date: '12 Ocak 2022',
      statusText: 'Bekliyor',
      status: 1,
      file: true
    },
    {
      id: 5,
      title: 'Avans Ödeme Talimatı',
      name: 'Uğur KARATAŞ, 48545454545',
      date: '12 Ocak 2022',
      statusText: 'Onaylandı',
      status: 2,
      file: false
    },
    {
      id: 6,
      title: 'Maaş Ödeme Talimatı',
      name: 'Uğur KARATAŞ, 48545454545',
      date: '12 Ocak 2022',
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
