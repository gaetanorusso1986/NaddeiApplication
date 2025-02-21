import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { provideHttpClient } from '@angular/common/http';
import { AuthComponent } from './Componenti/auth/auth.component';  
import { AuthService } from './Services/auth.service';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    AppComponent

  ],

  imports: [ 
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    AuthComponent,
    RouterModule 
  ],
  providers: [
    AuthService,  
    provideHttpClient()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
