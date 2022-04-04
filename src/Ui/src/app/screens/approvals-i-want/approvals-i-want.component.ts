import {Component, OnInit} from '@angular/core';
import {Subject, takeUntil} from "rxjs";
import {ApprovalsIWantService} from "../../services/approvals-i-want.service";

@Component({
  selector: 'app-approvals-i-want',
  templateUrl: './approvals-i-want.component.html',
  styleUrls: ['./approvals-i-want.component.scss']
})
export class ApprovalsIWantComponent implements OnInit {
  private destroy$ = new Subject();
  data: any[];

  pageSize = 10;
  pageNumber = 1;
  totalPages = 1;
  hasNextPage = true;
  hasPreviousPage = true;

  constructor(private approvalsIWantService: ApprovalsIWantService) {
  }

  ngOnInit(): void {
    this.getWantApproval();
  }

  getWantApproval() {
    let requestModel = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize
    };
    this.approvalsIWantService.getWantApproval(requestModel).pipe(takeUntil(this.destroy$)).subscribe({
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
    this.getWantApproval();
  }

  counter(i: number) {
    return new Array(i);
  }
}
