import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {LoginComponent} from "./screens/login/login.component";
import {DefaultLayoutComponent} from "./layouts/default-layout/default-layout.component";
import {HeaderComponent} from "./components/header/header.component";
import {HttpClientModule} from "@angular/common/http";
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    DefaultLayoutComponent,
    HeaderComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  exports: [
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

}
