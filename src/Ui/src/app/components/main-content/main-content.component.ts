import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-main-content',
  templateUrl: './main-content.component.html',
  styleUrls: ['./main-content.component.scss']
})
export class MainContentComponent implements OnInit {
  @Input('blockTitle') blockTitle: any;
  @Input('subTitle') subTitle: any;

  constructor() {
  }

  ngOnInit(): void {
  }

}
