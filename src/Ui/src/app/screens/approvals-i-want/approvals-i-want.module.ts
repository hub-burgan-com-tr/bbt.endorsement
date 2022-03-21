import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ApprovalsIWantComponent} from './approvals-i-want.component';
import {RouterModule, Routes} from "@angular/router";
import {SharedModule} from "../../modules/shared.module";
import {ApprovalsIWantDetailComponent} from './approvals-i-want-detail/approvals-i-want-detail.component';
import {ApprovalsIWantNewOrderComponent} from './approvals-i-want-new-order/approvals-i-want-new-order.component';
import {
  ApprovalsIWantNewOrderDetailComponent
} from './approvals-i-want-new-order-detail/approvals-i-want-new-order-detail.component';
import {NgxSmartModalModule} from "ngx-smart-modal";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ApprovalsIWantNewFormComponent} from './approvals-i-want-new-form/approvals-i-want-new-form.component';
import {FormioAppConfig, FormioModule} from "@formio/angular";
import {AppConfig} from "../../../../formio-config";

const routes: Routes = [
  {path: '', component: ApprovalsIWantComponent},
  {path: 'detail', component: ApprovalsIWantDetailComponent},
  {path: 'new-order', component: ApprovalsIWantNewOrderComponent},
  {path: 'new-order-detail', component: ApprovalsIWantNewOrderDetailComponent},
  {path: 'new-form', component: ApprovalsIWantNewFormComponent},
]

@NgModule({
  declarations: [
    ApprovalsIWantComponent,
    ApprovalsIWantDetailComponent,
    ApprovalsIWantNewOrderComponent,
    ApprovalsIWantNewOrderDetailComponent,
    ApprovalsIWantNewFormComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    RouterModule.forChild(routes),
    NgxSmartModalModule.forRoot(),
    FormsModule,
    ReactiveFormsModule,
    FormioModule
  ],
  providers: [
    {provide: FormioAppConfig, useValue: AppConfig}
  ]
})
export class ApprovalsIWantModule {
}
