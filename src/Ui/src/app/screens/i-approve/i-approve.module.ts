import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {IApproveComponent} from "./i-approve.component";
import {SharedModule} from "../../modules/shared.module";
import {IApproveDetailComponent} from "./i-approve-detail/i-approve-detail.component";

const routes: Routes = [
  {path: '', component: IApproveComponent},
  {path: 'detail', component: IApproveDetailComponent}
]

@NgModule({
  declarations: [
    IApproveComponent,
    IApproveDetailComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(routes),
    CommonModule
  ]
})
export class IApproveModule {
}
