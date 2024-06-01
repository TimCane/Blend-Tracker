import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginRedirectComponent } from './login-redirect.component';

const routes: Routes = [{ path: '', component: LoginRedirectComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LoginRedirectRoutingModule { }
