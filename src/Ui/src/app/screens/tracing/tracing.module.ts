import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {TracingComponent} from './tracing.component';
import {RouterModule, Routes} from "@angular/router";
import {SharedModule} from "../../modules/shared.module";
import {ReactiveFormsModule} from "@angular/forms";
import {TracingDetailComponent} from './tracing-detail/tracing-detail.component';

const routes: Routes = [
  {path: '', component: TracingComponent},
  {path: 'detail', component: TracingDetailComponent},
]

@NgModule({
  declarations: [
    TracingComponent,
    TracingDetailComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule
  ]
})
export class TracingModule {
}
