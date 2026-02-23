import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserServiceService } from '../../services/user-service.service';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
  animations: [
    trigger('expandCollapse', [
      state('void', style({
        height: '0px',
        opacity: '0',
        padding: '0px',
        overflow: 'hidden'
      })),
      state('*', style({
        height: '*',
        opacity: '1',
        padding: '10px',
        overflow: 'visible'
      })),
      transition('void <=> *', [
        animate('300ms ease-in-out')
      ])
    ])
  ]
})
export class HeaderComponent {
  public sidebarShow: boolean = false;
  isMenuOpen: boolean = false;
  isAdminOrAgent: boolean = false;
  userId: string | null = null;
  isUserLoggedIn: boolean = false;
  showContent = false;
  Content = false;

  constructor(private userService: UserServiceService,private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.userService.userId$.subscribe(id => (this.userId = id));
    this.userService.isUserLoggedIn$.subscribe(status => (this.isUserLoggedIn = status));
  }

  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }

  toggleContent() {
    this.showContent = !this.showContent;
  }

  poggleContent() {
    this.Content = !this.Content;
  }

  loginLogout() {
    if (this.isUserLoggedIn) {
      this.authService.logout();
    } else {
      this.router.navigate(['/agentLogin']);
    }
  }

}
