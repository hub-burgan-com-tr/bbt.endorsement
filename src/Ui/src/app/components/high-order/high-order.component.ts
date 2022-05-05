import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-high-order',
  templateUrl: './high-order.component.html',
  styleUrls: ['./high-order.component.scss']
})
export class HighOrderComponent implements OnInit {
  @Input() show;

  constructor() {
  }

  ngOnInit(): void {
  }

}
