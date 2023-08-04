import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { LoginCallbackComponent } from './components/login-callback/login-callback.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'login-callback', component: LoginCallbackComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
