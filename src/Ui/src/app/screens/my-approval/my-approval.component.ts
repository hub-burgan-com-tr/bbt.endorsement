import {Component, OnInit} from '@angular/core';
import {MyApprovalService} from "../../services/my-approval.service";
import {GetEndorsementListRequestModel} from "../../models/my-approval";

@Component({
  selector: 'app-my-approval',
  templateUrl: './my-approval.component.html',
  styleUrls: ['./my-approval.component.scss']
})

export class MyApprovalComponent implements OnInit {
  data: any[];

  pageSize = 10;
  pageNumber = 1;
  totalPages = 1;
  hasNextPage = true;
  hasPreviousPage = true;

  constructor(private myApprovalService: MyApprovalService) {
  }

  ngOnInit(): void {
    this.getEndorsementList();
  }

  getEndorsementList() {
    let requestModel: GetEndorsementListRequestModel = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize
    };
    this.myApprovalService.getEndorsementList(requestModel).subscribe({
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
    this.getEndorsementList();
  }

  counter(i: number) {
    return new Array(i);
  }
}
