import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PagesService {

  private apiUrl = 'https://localhost:7158/api/Pages';

  constructor(private http: HttpClient) { }

  getPages(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl).pipe(
      catchError((error) => {
        if (error.status === 404) {
          // Restituisce un messaggio di errore nel caso non ci siano pagine
          return throwError('Non esistono pagine.');
        }
        return throwError('Errore nel recupero delle pagine.');
      })
    );
  }
  
}
