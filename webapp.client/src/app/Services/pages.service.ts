import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { Page, PageDto } from '../Models/pages';

@Injectable({
  providedIn: 'root'
})
export class PagesService {

  private apiUrl = 'https://localhost:7158/api/Pages';

  constructor(private http: HttpClient) { }

  getPages(): Observable<Page[]> {
    return this.http.get<Page[]>(this.apiUrl).pipe(
      catchError((error) => {
        if (error.status === 404) {
          console.warn('Nessuna pagina trovata.');
          return new Observable<Page[]>(observer => {
            observer.next([]);  // Restituisci un array vuoto
            observer.complete();
          });
        }
        console.error('Errore nel recupero delle pagine:', error);
        return throwError(() => new Error('Errore nel recupero delle pagine.'));
      })
    );
  }
  
   // Create a new page
   createPage(page: PageDto): Observable<Page> {
    return this.http.post<Page>(this.apiUrl, page);
  }

  // Delete a page by pageId
  deletePage(pageId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${pageId}`);
  }
  
}