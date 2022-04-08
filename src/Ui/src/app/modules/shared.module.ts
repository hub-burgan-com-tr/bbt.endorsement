import {NgModule} from '@angular/core';
import {ListItemComponent} from "../components/list-item/list-item.component";
import {MainContentComponent} from "../components/main-content/main-content.component";
import {CommonModule} from "@angular/common";
import {BackButtonDirective} from "../directives/back-button.directive";
import {
  MainContentButtonGroupComponent
} from "../components/main-content-button-group/main-content-button-group.component";
import {RouterModule} from "@angular/router";
import {OnlyNumberDirective} from "../directives/only-number.directive";
import {SafePipe} from "../pipes/safe.pipe";
import {SafeHtmlPipe} from "../pipes/safe-html.pipe";
import {StatePipe} from "../pipes/state.pipe";
import {StateClassPipe} from "../pipes/state-class.pipe";


@NgModule({
  declarations: [
    ListItemComponent,
    MainContentComponent,
    BackButtonDirective,
    MainContentButtonGroupComponent,
    OnlyNumberDirective,
    SafePipe,
    SafeHtmlPipe,
    StatePipe,
    StateClassPipe
  ],
  imports: [CommonModule, RouterModule],
  exports: [
    ListItemComponent,
    MainContentComponent,
    BackButtonDirective,
    MainContentButtonGroupComponent,
    OnlyNumberDirective,
    SafePipe,
    SafeHtmlPipe,
    StatePipe,
    StateClassPipe
  ]
})
export class SharedModule {
}
