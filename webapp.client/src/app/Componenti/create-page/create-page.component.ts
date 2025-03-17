import { Component } from '@angular/core';
import { Page, PageDto } from '../../Dtos/PageDto';
import { PagesService } from '../../Services/pages.service';
import { AuthService } from '../../Services/auth.service';  

@Component({
  selector: 'app-create-page',
  standalone: false,
  templateUrl: './create-page.component.html',
  styleUrls: ['./create-page.component.css']
})
export class CreatePageComponent {
  pages: Page[] = [];
  newPage: PageDto = {
    title: '',
    userId: '',  // Inizializzato vuoto, verrà aggiornato dal token
    parentId: null,
    pageSections: [
      {
        order: 0,
        pageContents: [
          {
            contentType: 'string',
            contentData: 'string',
          }
        ]
      }
    ]
  };

  constructor(
    private pageService: PagesService,
    private authService: AuthService  
  ) { }

  ngOnInit(): void {
    const userIdFromToken = this.authService.getUserIdFromToken();
    this.newPage.userId = userIdFromToken || '';  // Recupera l'userId dal token
    console.log('User ID recuperato:', this.newPage.userId); // Log per il debug
  
    // Recupera le pagine esistenti dal servizio
    this.pageService.getPages().subscribe(
      (pages) => {
        this.pages = pages;
        console.log('Pagine caricate:', this.pages);  // Aggiungi un log per verificare le pagine
      },
      (error) => {
        console.error('Errore nel recupero delle pagine:', error);
      }
    );
  }

  createPage(): void {
    // Verifica che il titolo e l'userId siano presenti
    console.log('newPage:', this.newPage);  // Debug per vedere il contenuto di newPage
  
    if (!this.newPage.title || !this.newPage.userId) {
      console.error('Title and userId are required');
      return;  // Interrompe l'esecuzione se title o userId sono mancanti
    }
  
    // Aggiungi un log per verificare il valore di parentId prima di inviarlo
    console.log('parentId prima di inviare:', this.newPage.parentId);  // Log per parentId
  
    const pageToCreate = {
      title: this.newPage.title,
      userId: this.newPage.userId,  // Usa il userId recuperato dal token
      parentId: this.newPage.parentId || undefined,  // Usa undefined se parentId è null o vuoto
      pageSections: this.newPage.pageSections.map((section, index) => ({
        order: index,
        pageContents: section.pageContents.map((content) => ({
          contentType: content.contentType,
          contentData: content.contentData,
        })),
      })),
    };
  
    // Aggiungi un log per vedere l'intero oggetto prima di inviarlo
    console.log('Oggetto pagina da inviare:', pageToCreate);
  
    this.pageService.createPage(pageToCreate).subscribe(
      (page) => {
        this.pages.push(page);
        this.newPage = { title: '', userId: '', pageSections: [] }; // Reset new page form
      },
      (error) => {
        console.error('Error creating page:', error);
      }
    );
  }
  
  addContent(sectionIndex: number): void {
    this.newPage.pageSections[sectionIndex].pageContents.push({
      contentType: 'text', // Default tipo testo
      contentData: '',
    });
  }
  
  removeContent(sectionIndex: number, contentIndex: number): void {
    this.newPage.pageSections[sectionIndex].pageContents.splice(contentIndex, 1);
  }
}
