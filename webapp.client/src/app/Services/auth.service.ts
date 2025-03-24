import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { jwtDecode } from 'jwt-decode';  

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7158/api/auth';
  
  private isLoggedInSubject = new BehaviorSubject<boolean>(this.isLoggedIn());  // Gestisce lo stato di login come stream

  constructor(private http: HttpClient, private router: Router) {}

  get isLoggedIn$() {
    return this.isLoggedInSubject.asObservable();  // Espone un Observable per altri componenti
  }

  async login(email: string, password: string) {
    try {
      const response = await this.http.post<{ token?: string }>(`${this.apiUrl}/login`, { email, password }).toPromise();

      if (response?.token) {
        localStorage.setItem('token', response.token);
        this.isLoggedInSubject.next(true);  // Aggiorna lo stato di login
        this.router.navigate(['/Home']);
      } else {
        console.error('Token non ricevuto');
      }
    } catch (error: any) {
      console.error('Login fallito', error);
      throw new Error('Credenziali non valide');
    }
  }

  async register(data: any) {
    try {
      await this.http.post(`${this.apiUrl}/register`, data).toPromise();
      alert('Registrazione completata');
      this.router.navigate(['/Home']);
    } catch (error: any) {
      console.error('Registrazione fallita', error);
      throw new Error('Errore durante la registrazione');
    }
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    return !!token && this.isTokenValid(token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  private isTokenValid(token: string): boolean {
    try {
      const decodedToken: any = jwtDecode(token);
      const currentTime = Date.now() / 1000;

      if (decodedToken.exp < currentTime) {
        this.logout();
        return false;
      }
      return true;
    } catch (error) {
      console.error('Errore nel decodificare il token', error);
      this.logout();
      return false;
    }
  }

  logout(): void {
    localStorage.removeItem('token');
    this.isLoggedInSubject.next(false);  // Aggiorna lo stato di login
  }
}





