import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7158/api/auth';
  private isLoggedInSubject = new BehaviorSubject<boolean>(false); // Inizializzazione sicura

  constructor(private http: HttpClient, private router: Router) {
    this.isLoggedInSubject.next(this.isLoggedIn()); // Verifica stato di login all'avvio
  }

  get isLoggedIn$() {
    return this.isLoggedInSubject.asObservable();
  }

  async login(email: string, password: string) {
    try {
      const response = await this.http.post<{ token?: string }>(`${this.apiUrl}/login`, { email, password }).toPromise();

      if (response?.token) {
        localStorage.setItem('token', response.token);
        this.isLoggedInSubject.next(true);
        this.router.navigate(['/dashboard']);
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
      this.router.navigate(['/auth']);
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

  getUserIdFromToken(): string | null {
    const token = this.getToken();
    if (!token) {
      return null;
    }
    
    try {
      const decodedToken: any = jwtDecode(token);
      return decodedToken.nameid || null; // Usa 'nameid' come userId
    } catch (error) {
      console.error('Errore nel decodificare il token', error);
      return null;
    }
  }

  private isTokenValid(token: string): boolean {
    try {
      const decodedToken: any = jwtDecode(token);
      const currentTime = Date.now() / 1000;

      if (decodedToken.exp < currentTime) {
        this.logout(); // Attenzione: chiama logout durante l'inizializzazione
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
    this.isLoggedInSubject.next(false); // Assicura che lo stato venga aggiornato
    this.router.navigate(['/dashboard']);
  }
}
