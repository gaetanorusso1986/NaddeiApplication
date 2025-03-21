import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthPageComponent } from './Componenti/auth-page/auth-page.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { CreatePageComponent } from './Componenti/create-page/create-page.component';
import { ViewPageComponent } from './Componenti/view-page/view-page.component';

const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'auth', component: AuthPageComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'create-page', component: CreatePageComponent },
  { path: 'page/:id', component: ViewPageComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
