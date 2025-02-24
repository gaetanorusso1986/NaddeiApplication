import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../Services/auth.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule]
})
export class AuthComponent implements OnInit {

  registerForm: FormGroup;
  successMessage: string = '';
  errorMessage: string = '';
  roles: any[] = [];

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      roleId: [null, Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadRoles();
  }

  loadRoles(): void {
    this.authService.getRoles().subscribe({
      next: (data) => {
        this.roles = [{ id: null, name: "Choose your role" }, ...data]; // Aggiunge l'opzione in cima
      },
      error: (err) => {
        const errorMessage = err.error?.message || "Failed to load roles";
        this.errorMessage = errorMessage;
      }
    });
  }


  register(): void {
    if (this.registerForm.valid) {
      this.authService.register(this.registerForm.value).subscribe({
        next: () => {
          this.successMessage = 'Registration successful!';
          this.errorMessage = ''; // Pulisce eventuali errori precedenti
          this.registerForm.reset(); // Resetta il form dopo la registrazione
        },
        error: (err) => {
          const errorMessage = err.error?.message || "Registration failed";
          this.errorMessage = errorMessage;
        }
      });
    }
  }
}
