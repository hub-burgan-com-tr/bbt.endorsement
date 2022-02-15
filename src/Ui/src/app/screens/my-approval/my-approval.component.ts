import {Component, OnInit} from '@angular/core';

@Component({
  selector: 'app-my-approval',
  templateUrl: './my-approval.component.html',
  styleUrls: ['./my-approval.component.scss']
})
export class MyApprovalComponent implements OnInit {
  data = [
    {id: 1, title: 'Sigorta Teklif Talimatı', file: true, icon: null},
    {id: 2, title: '3. Para Birim Talimatı', file: false, icon: null},
    {id: 2, title: 'Maaş Ödeme Talimatı', file: true, icon: null},
  ]

  constructor() {
  }

  ngOnInit(): void {
  }

}
