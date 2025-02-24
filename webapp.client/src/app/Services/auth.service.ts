import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RegisterDto } from '../Dtos/RegisterDto';
import { RoleDto } from '../Dtos/RoleDto';
;

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'https://localhost:7158/api/Auth';

  constructor(private http: HttpClient) { }

  register(user: RegisterDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, user);
  }

  getRoles(): Observable<RoleDto[]> {
    return this.http.get<RoleDto[]>(`${this.apiUrl}/roles`);
  }
}
