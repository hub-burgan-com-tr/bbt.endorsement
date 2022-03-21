import {Component, OnDestroy, OnInit} from '@angular/core';
import {NewOrderFormService} from "../../services/new-order-form.service";
import {Subject, takeUntil} from "rxjs";

@Component({
  selector: 'app-main-content-button-group',
  templateUrl: './main-content-button-group.component.html',
  styleUrls: ['./main-content-button-group.component.scss']
})
export class MainContentButtonGroupComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject();
  data;

  constructor(private newOrderFormService: NewOrderFormService) {
  }

  ngOnInit(): void {
    this.newOrderFormService.getForm().pipe(takeUntil(this.destroy$)).subscribe(res => {
      this.data = res && res.data;
    })
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.complete();
  }
}
