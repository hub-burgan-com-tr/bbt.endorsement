import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-render-file',
  templateUrl: './render-file.component.html',
  styleUrls: ['./render-file.component.scss']
})
export class RenderFileComponent implements OnInit {
  @Input() detail;

  constructor() {
  }

  ngOnInit(): void {
  }

}
