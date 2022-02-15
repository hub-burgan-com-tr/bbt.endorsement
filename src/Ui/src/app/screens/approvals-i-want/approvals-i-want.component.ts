import {Component, OnInit} from '@angular/core';

@Component({
  selector: 'app-approvals-i-want',
  templateUrl: './approvals-i-want.component.html',
  styleUrls: ['./approvals-i-want.component.scss']
})
export class ApprovalsIWantComponent implements OnInit {
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
  ]

  constructor() {
  }

  ngOnInit(): void {
  }

}
