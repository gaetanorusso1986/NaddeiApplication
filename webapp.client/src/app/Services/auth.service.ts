import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface RegisterDto {
  username: string;
  email: string;
  password: string;
  roleId: number;
}

export interface Role {
  id: number;
  name: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'https://localhost:7158/api/Auth';

  constructor(private http: HttpClient) { }

  register(user: RegisterDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, user);
  }

  getRoles(): Observable<Role[]> {
    return this.http.get<Role[]>(`${this.apiUrl}/roles`);
  }
}
