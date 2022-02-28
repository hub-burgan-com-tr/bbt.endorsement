import {Component, OnInit} from '@angular/core';
import {Subscription} from "rxjs";
import {NgxSmartModalService} from "ngx-smart-modal";

@Component({
  selector: 'app-my-approval-detail',
  templateUrl: './my-approval-detail.component.html',
  styleUrls: ['./my-approval-detail.component.scss']
})
export class MyApprovalDetailComponent implements OnInit {
  step: number = 1;

  constructor(public ngxSmartModalService: NgxSmartModalService) {
  }

  ngOnInit(): void {
  }

  send() {
    this.ngxSmartModalService.close('myModal');
    this.ngxSmartModalService.open('confirmModal');
  }

  continue() {
    this.step++;
  }

  redirect() {
  }
}
