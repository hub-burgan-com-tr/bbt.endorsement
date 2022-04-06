import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {MyApprovalComponent} from './my-approval.component';
import {RouterModule, Routes} from "@angular/router";
import {MyApprovalDetailComponent} from "./my-approval-detail/my-approval-detail.component";
import {NgxSmartModalModule} from 'ngx-smart-modal';
import {SharedModule} from "../../modules/shared.module";
import {MyApprovalCompletedComponent} from "./my-approval-completed/my-approval-completed.component";
import {FormsModule} from "@angular/forms";
import {SafePipe} from "../../pipes/safe.pipe";
import {SafeHtmlPipe} from "../../pipes/safe-html.pipe";

const routes: Routes = [
  {path: '', component: MyApprovalComponent},
  {path: 'detail/:orderId', component: MyApprovalDetailComponent},
  {path: 'detail/:orderId/completed', component: MyApprovalCompletedComponent},
]

@NgModule({
  declarations: [
    MyApprovalComponent,
    MyApprovalDetailComponent,
    MyApprovalCompletedComponent,
    SafePipe,
    SafeHtmlPipe
  ],
    imports: [
        SharedModule,
        RouterModule.forChild(routes),
        CommonModule,
        NgxSmartModalModule.forRoot(),
        FormsModule
    ]
})
export class MyApprovalModule {
}
