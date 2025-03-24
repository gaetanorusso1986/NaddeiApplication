import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './Home/home/home.component';
import { AuthComponent } from './Pages/auth/auth.component';



const routes: Routes = [
   { path: '', redirectTo: 'Home', pathMatch: 'full' },
   { path: 'Home', component: HomeComponent },
   { path: 'Login', component: AuthComponent },
   //{ path: 'Admin', component: DashboardComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
