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
import {StatePipe} from "../pipes/state.pipe";
import {StateClassPipe} from "../pipes/state-class.pipe";
import {DownloadLinkComponent} from "../components/download-link/download-link.component";
import {RenderFileComponent} from "../components/render-file/render-file.component";
import {PersonSearchComponent} from "../components/person-search/person-search.component";
import {FormsModule} from "@angular/forms";
import {HighOrderComponent} from "../components/high-order/high-order.component";
import {HistoryComponent} from "../components/history/history.component";


@NgModule({
  declarations: [
    ListItemComponent,
    MainContentComponent,
    BackButtonDirective,
    MainContentButtonGroupComponent,
    OnlyNumberDirective,
    SafePipe,
    StatePipe,
    StateClassPipe,
    DownloadLinkComponent,
    RenderFileComponent,
    PersonSearchComponent,
    HighOrderComponent,
    HistoryComponent
  ],
  imports: [CommonModule, RouterModule, FormsModule],
  exports: [
    ListItemComponent,
    MainContentComponent,
    BackButtonDirective,
    MainContentButtonGroupComponent,
    OnlyNumberDirective,
    SafePipe,
    StatePipe,
    StateClassPipe,
    DownloadLinkComponent,
    RenderFileComponent,
    PersonSearchComponent,
    HighOrderComponent,
    HistoryComponent
  ]
})
export class SharedModule {
}
