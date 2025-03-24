import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { provideHttpClient } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthPageComponent } from './Componenti/auth-page/auth-page.component';
import { HeaderComponent } from './Componenti/header/header.component';
import { FooterComponent } from './Componenti/footer/footer.component';
import { CreatePageComponent } from './Componenti/create-page/create-page.component';
import { ViewPageComponent } from './Componenti/view-page/view-page.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    AuthPageComponent,
    HeaderComponent,
    FooterComponent,
    CreatePageComponent,
    ViewPageComponent

  ],

  imports: [ 
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    RouterModule
  ],
  providers: [ 
    provideHttpClient()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
