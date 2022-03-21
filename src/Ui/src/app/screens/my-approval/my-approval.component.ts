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

  constructor(private myApprovalService: MyApprovalService) {
  }

  ngOnInit(): void {
    let requestModel: GetEndorsementListRequestModel = {
      pageNumber: 1,
      pageSize: 10
    };
    this.myApprovalService.getEndorsementList(requestModel).subscribe({
      next: res => {
        if (res.data)
          this.data = res.data.items;
        else
          console.error("Kayıt bulunamadı");
      },
      error: err => {
        console.error(err.message);
      }
    });
  }

}
