import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomerListComponent } from './components/customer-list/customer-list.component';
import { LoginComponent } from './components/login/login.component';
import { AuthNotRequiredGuard } from './services/auth/auth.not-required.guard';
import { AuthRequiredGuard } from './services/auth/auth.required.guard';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [AuthNotRequiredGuard]
  },
  {
    path: 'customer-list',
    component: CustomerListComponent,
    canActivate: [AuthRequiredGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
