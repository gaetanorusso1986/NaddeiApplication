import { Component } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AuthService } from "../../Services/auth.service";
import { ActivatedRoute, Router } from "@angular/router";

@Component({
  selector: 'app-auth',
  standalone: false,
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.css'
})
export class AuthComponent {
  // Definiamo i due form separati per login e registrazione
  loginForm: FormGroup;
  registerForm: FormGroup;
  isLogin = true;
  loginError = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    // Form per il login
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });

    // Form per la registrazione
    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [
        Validators.required,
        Validators.minLength(12),
        Validators.pattern(/(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[.!@#$%^&*])/)
      ]],
      role: ['2'],  // Default come "User"
      adminAuthCode: ['']
    });
  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      if (params['role'] === 'Admin') {
        this.registerForm.patchValue({ role: '1' });
      }
    });

    // Gestiamo la validazione dinamica per il codice di autenticazione amministratore
    this.registerForm.get('role')?.valueChanges.subscribe(role => {
      const adminCodeControl = this.registerForm.get('adminAuthCode');
      if (role === '1') {
        adminCodeControl?.setValidators(Validators.required);
      } else {
        adminCodeControl?.clearValidators();
      }
      adminCodeControl?.updateValueAndValidity();
    });
  }

  // Funzione per il cambio tra login e registrazione
  toggleMode() {
    this.isLogin = !this.isLogin;
  }

  // Gestione dell'invio del form (login o registrazione)
  async onSubmit() {
    if (this.isLogin) {
      if (this.loginForm.invalid) return;

      const { email, password } = this.loginForm.value;
      try {
        await this.authService.login(email, password);
        this.router.navigate(['/home'], { replaceUrl: true });
      } catch (error: any) {
        this.loginError = 'Credenziali non valide. Riprova.';
      }
    } else {
      if (this.registerForm.invalid) return;

      const { firstName, lastName, username, email, password, role, adminAuthCode } = this.registerForm.value;

      // Registra l'utente, considerando se è un admin o un utente
      await this.authService.register({
        firstName,
        lastName,
        username,
        email,
        password,
        roleId: +role,
        adminAuthCode: role === '1' ? adminAuthCode : undefined
      });
      this.router.navigate(['/home'], { replaceUrl: true });
    }
  }
}
