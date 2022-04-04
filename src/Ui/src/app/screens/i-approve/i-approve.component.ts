import {Component, OnInit} from '@angular/core';
import {Subject, takeUntil} from "rxjs";
import {IApproveService} from "../../services/i-approve.service";

@Component({
  selector: 'app-i-approve',
  templateUrl: './i-approve.component.html',
  styleUrls: ['./i-approve.component.scss']
})
export class IApproveComponent implements OnInit {
  private destroy$ = new Subject();
  data: any = [];

  pageSize = 10;
  pageNumber = 1;
  totalPages = 1;
  hasNextPage = true;
  hasPreviousPage = true;

  constructor(private iApproveService: IApproveService) {
  }

  ngOnInit(): void {
    this.getMyApproval();
  }

  getMyApproval() {
    let requestModel = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize
    };
    this.iApproveService.getMyApproval(requestModel).pipe(takeUntil(this.destroy$)).subscribe({
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
    this.getMyApproval();
  }

  counter(i: number) {
    return new Array(i);
  }
}
