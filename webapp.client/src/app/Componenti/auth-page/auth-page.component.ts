import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../Services/auth.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-auth-page',
  standalone: false,
  templateUrl: './auth-page.component.html',
  styleUrls: ['./auth-page.component.css']
})
export class AuthPageComponent implements OnInit {
  isLogin = true;
  email = '';
  password = '';
  firstName = '';
  lastName = '';
  username = '';
  role = '2'; // Default user
  adminAuthCode = '';

  // Error tracking
  emailError = false;
  passwordError = false;
  firstNameError = false;
  lastNameError = false;
  usernameError = false;
  adminAuthCodeError = false;
  
  // Error message
  loginError = '';

  constructor(private authService: AuthService, private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      if (params['role'] === 'Admin') {
        this.role = '1';
      } else {
        this.role = '2';
      }
    });
  }

  toggleMode() {
    this.isLogin = !this.isLogin; // Cambia modalità tra login e registrazione
  
    // Resetta i modelli
    this.email = '';
    this.password = '';
    this.firstName = '';
    this.lastName = '';
    this.username = '';
    this.role = '2'; // Imposta il valore predefinito per il ruolo (User)
    this.adminAuthCode = '';
    this.loginError = ''
  
    // Resetta gli errori
    this.emailError = false;
    this.passwordError = false;
    this.firstNameError = false;
    this.lastNameError = false;
    this.usernameError = false;
    this.adminAuthCodeError = false;
  }
  

  validateForm() {
    // Validation logic
    this.emailError = !this.email || !/\S+@\S+\.\S+/.test(this.email);
    this.passwordError = !this.password;
    this.firstNameError = !this.firstName;
    this.lastNameError = !this.lastName;
    this.usernameError = !this.username;
    this.adminAuthCodeError = this.role === '1' && !this.adminAuthCode;
  }

  async onSubmit() {
    this.validateForm(); // Run validation before submitting
  
    if (this.isLogin) {
      if (!this.emailError && !this.passwordError) {
        try {
          await this.authService.login(this.email, this.password);
        } catch (error: any) {
            this.loginError = 'Credenziali non valide. Riprova.';
        }
      }
    } else {
      if (
        !this.emailError &&
        !this.passwordError &&
        !this.firstNameError &&
        !this.lastNameError &&
        !this.usernameError &&
        (!this.adminAuthCodeError || this.role !== '1')
      ) {
        await this.authService.register({
          firstName: this.firstName,
          lastName: this.lastName,
          username: this.username,
          email: this.email,
          password: this.password,
          roleId: +this.role,
          adminAuthCode: this.role === '1' ? this.adminAuthCode : undefined
        });
      }
    }
  }
  
  
}
