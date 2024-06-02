import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: "",
    pathMatch: "full",
    redirectTo: "login-redirect"
  },
  { path: 'callback', loadChildren: () => import('./features/callback/callback.module').then(m => m.CallbackModule) },
  { path: 'login-redirect', loadChildren: () => import('./features/login-redirect/login-redirect.module').then(m => m.LoginRedirectModule) },
  { path: 'home', loadChildren: () => import('./features/home/home.module').then(m => m.HomeModule) },
  { path: 'songs', loadChildren: () => import('./features/songs/songs.module').then(m => m.SongsModule) },
  { path: 'users', loadChildren: () => import('./features/users/users.module').then(m => m.UsersModule) }];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
