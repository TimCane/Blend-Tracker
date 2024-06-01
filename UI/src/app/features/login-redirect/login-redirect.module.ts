import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LoginRedirectRoutingModule } from './login-redirect-routing.module';
import { LoginRedirectComponent } from './login-redirect.component';
import { SpinnerModule } from 'src/app/components/spinner/spinner.module';


@NgModule({
  declarations: [
    LoginRedirectComponent
  ],
  imports: [
    CommonModule,
    LoginRedirectRoutingModule,
    SpinnerModule
  ]
})
export class LoginRedirectModule { }
