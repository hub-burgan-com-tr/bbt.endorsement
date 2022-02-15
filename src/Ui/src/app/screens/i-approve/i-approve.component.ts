import {Component, OnInit} from '@angular/core';

@Component({
  selector: 'app-i-approve',
  templateUrl: './i-approve.component.html',
  styleUrls: ['./i-approve.component.scss']
})
export class IApproveComponent implements OnInit {
  data = [
    {id: 1, title: 'Sigorta Teklif Talimatı', file: true, icon: 'check'},
    {id: 2, title: '3. Para Birim Talimatı', file: false, icon: 'check'},
    {id: 3, title: 'Maaş Ödeme Talimatı', file: true, icon: 'block'},
    {id: 4, title: '1021 İşlem Onayı', file: true, icon: 'refresh'},
    {id: 5, title: 'Avans Ödeme Talimatı', file: false, icon: 'check'},
    {id: 6, title: 'Maaş Ödeme Talimatı', file: false, icon: 'check'},
    {id: 7, title: '1021 İşlem Onayı', file: true, icon: 'check'},
    {id: 8, title: 'Avans Ödeme Talimatı', file: true, icon: 'refresh'},
    {id: 9, title: '1021 İşlem Onayı', file: false, icon: 'check'},
  ]

  constructor() {
  }

  ngOnInit(): void {
  }

}
