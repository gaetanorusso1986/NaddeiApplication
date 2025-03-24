import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../Services/auth.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-auth',
  standalone: false,
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.css'
})
export class AuthComponent {

  authForm: FormGroup;
  isLogin = true;
  loginError = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private route: ActivatedRoute
  ) {
    this.authForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      firstName: [''],
      lastName: [''],
      username: [''],
      role: ['2'],
      adminAuthCode: ['']
    });
  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      if (params['role'] === 'Admin') {
        this.authForm.patchValue({ role: '1' });
      }
    });

    // Aggiorna la validazione dinamicamente quando il ruolo cambia
    this.authForm.get('role')?.valueChanges.subscribe(role => {
      const adminCodeControl = this.authForm.get('adminAuthCode');
      if (role === '1') {
        adminCodeControl?.setValidators(Validators.required);
      } else {
        adminCodeControl?.clearValidators();
      }
      adminCodeControl?.updateValueAndValidity();
    });
  }

  toggleMode() {
    this.isLogin = !this.isLogin;
  }

  async onSubmit() {
    if (this.authForm.invalid) return;

    const { email, password, firstName, lastName, username, role, adminAuthCode } = this.authForm.value;

    if (this.isLogin) {
      try {
        await this.authService.login(email, password);
      } catch (error: any) {
        this.loginError = 'Credenziali non valide. Riprova.';
      }
    } else {
      await this.authService.register({
        firstName,
        lastName,
        username,
        email,
        password,
        roleId: +role,
        adminAuthCode: role === '1' ? adminAuthCode : undefined
      });
    }
  }
}

