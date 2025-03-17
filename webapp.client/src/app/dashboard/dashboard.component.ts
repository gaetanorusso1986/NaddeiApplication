import { Component } from '@angular/core';
import { PagesService } from '../Services/pages.service';


@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  pages: any[] = [];
  errorMessage: string = '';

  constructor(private pagesService: PagesService) {}

  ngOnInit(): void {
    this.pagesService.getPages().subscribe(
      (data) => {
        this.pages = data; // Se ci sono pagine, le assegni al componente
        if (this.pages.length === 0) {
          this.errorMessage = 'Nessuna pagina trovata.';
        }
      },
      (error) => {
        this.errorMessage = 'Errore nel recupero delle pagine: ' + error.message; // Mostra errore specifico
      }
    );
  }
}

