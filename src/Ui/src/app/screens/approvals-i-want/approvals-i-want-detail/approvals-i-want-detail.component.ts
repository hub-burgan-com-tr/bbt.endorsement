import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-approvals-i-want-detail',
  templateUrl: './approvals-i-want-detail.component.html',
  styleUrls: ['./approvals-i-want-detail.component.scss']
})
export class ApprovalsIWantDetailComponent implements OnInit {
  step: any;

  constructor(private route: ActivatedRoute) {
    console.log('Called Constructor');
    this.route.queryParams.subscribe(params => {
      this.step = params['step'];
    });
  }

  ngOnInit(): void {
  }

}
