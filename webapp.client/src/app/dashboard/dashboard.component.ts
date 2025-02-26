import { Component } from '@angular/core';
import { PagesService } from '../Services/pages.service';


@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  pages: any[] = [];
  errorMessage: string = '';

  constructor(private pagesService: PagesService) {}

  ngOnInit(): void {
    this.pagesService.getPages().subscribe(
      (data) => {
        this.pages = data; // Se ci sono pagine, le assegni al componente
      },
      (error) => {
        this.errorMessage = error; // Se non ci sono pagine, setti il messaggio di errore
      }
    );
  }
}
