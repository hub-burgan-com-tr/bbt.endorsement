import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginComponent} from "./screens/login/login.component";
import {DefaultLayoutComponent} from "./layouts/default-layout/default-layout.component";

const routes: Routes = [
  {path: '', redirectTo: 'my-approval', pathMatch: 'full'},
  {
    path: '', component: DefaultLayoutComponent,
    children: [
      {
        path: 'my-approval',
        loadChildren: () => import('./screens/my-approval/my-approval.module').then(m => m.MyApprovalModule)
      },
      {
        path: 'i-approve',
        loadChildren: () => import('./screens/i-approve/i-approve.module').then(m => m.IApproveModule)
      },
      {
        path: 'approvals-i-want',
        loadChildren: () => import('./screens/approvals-i-want/approvals-i-want.module').then(m => m.ApprovalsIWantModule)
      },
      {
        path: 'tracing',
        loadChildren: () => import('./screens/tracing/tracing.module').then(m => m.TracingModule)
      }
    ]
  },
  {
    path: 'login', component: LoginComponent
  },
  {path: '**', redirectTo: 'my-approval'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
