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
  roles: any[] = [];  // Array per memorizzare i ruoli

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      roleId: [null, Validators.required]  // Il campo per il ruolo, inizializzato come null
    });
  }

  ngOnInit(): void {
    this.loadRoles();  // Carica i ruoli quando il componente viene inizializzato
  }

  loadRoles(): void {
    this.authService.getRoles().subscribe({
      next: (data) => this.roles = data,  // Popola l'array dei ruoli con i dati ricevuti
      error: (err) => this.errorMessage = 'Failed to load roles: ' + err.error
    });
  }

  register(): void {
    if (this.registerForm.valid) {
      this.authService.register(this.registerForm.value).subscribe({
        next: () => this.successMessage = 'Registration successful!',
        error: err => this.errorMessage = 'Registration failed: ' + err.error
      });
    }
  }
}
