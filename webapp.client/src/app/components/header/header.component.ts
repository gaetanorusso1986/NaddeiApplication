import { Component } from '@angular/core';

import { AuthService } from '../../Services/auth.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { PagesService } from '../../Services/pages.service';

@Component({
  selector: 'app-header',
  standalone: false,
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  pages: any[] = [];
  isLoggedIn: boolean = false;
  private authSubscription: Subscription = new Subscription();  // Subscription per gestire lo stato di login

  constructor(private pagesService: PagesService, private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.pagesService.getPages().subscribe((data) => {
      this.pages = data;
    });
    
    // Sottoscrivi a isLoggedIn$ per ottenere aggiornamenti in tempo reale
    this.authSubscription = this.authService.isLoggedIn$.subscribe((isLoggedIn) => {
      this.isLoggedIn = isLoggedIn;
    });
  }

  ngOnDestroy(): void {
    // Non dimenticare di annullare la sottoscrizione quando il componente viene distrutto
    this.authSubscription.unsubscribe();
  }

  goToCreatePage(): void {
    if (this.isLoggedIn) {
      this.router.navigate(['/create-page']);
    } else {
      this.router.navigate(['/auth']);
    }
  }

  login(): void {
    this.router.navigate(['/login']);
  }

  logout(): void {
    this.authService.logout();
  }
}