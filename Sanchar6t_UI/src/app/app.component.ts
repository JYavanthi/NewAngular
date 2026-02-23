// import { Component, HostListener } from '@angular/core';
// import { Observable } from 'rxjs';
// import { LoaderService } from './services/loader.service';

// @Component({
//   selector: 'app-root',
//   templateUrl: './app.component.html',
//   styleUrl: './app.component.scss'
// })
// export class AppComponent {
//   showGoUpButton: boolean = false;
//   isLoading$!: Observable<boolean>;

//   constructor(private loaderService: LoaderService){
//     this.isLoading$ = this.loaderService.loading$;
//   }

//   @HostListener('window:scroll', ['$event'])
//   onWindowScroll() {
//     if (window.scrollY > 500) {
//       this.showGoUpButton = true;
//     } else {
//       this.showGoUpButton = false;
//     }
//   }

//   scrollToTop() {
//     window.scrollTo({ top: 0, behavior: 'smooth' });
//   }
  
// }


import { Component, HostListener } from '@angular/core';
import { Observable } from 'rxjs';
import { LoaderService } from './services/loader.service';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  showGoUpButton: boolean = false;
  isLoading$!: Observable<boolean>;
   showRechargeHeader = false; // ⭐ NEW
  showGlobalLayout = true; // ⭐ add this
 showAdminNavbar = false;
 constructor(
    private loaderService: LoaderService,
    private router: Router
  ) {
    this.isLoading$ = this.loaderService.loading$;

    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(event => {
        const navEnd = event as NavigationEnd;
        const url = navEnd.urlAfterRedirects;

        // ❌ hide everything for backoffice
        if (url.startsWith('/backoffice')) {
          this.showGlobalLayout = false;
          this.showAdminNavbar = false;
          return;
        }

        // ✅ show admin navbar for agenHeader
        if (url.startsWith('/admin/agenHeader')) {
          this.showGlobalLayout = true;   // hide normal header/footer
          this.showAdminNavbar = true;     // show admin navbar
          return;
        }

        if (url.startsWith('/recharge')) {
          this.resetLayouts();
          this.showRechargeHeader = false;
           this.showAdminNavbar = true; 
            this.showGlobalLayout = false; 
          return;
        }


        // ✅ default layout
         this.resetLayouts();
        this.showGlobalLayout = true;
        this.showAdminNavbar = false;
      });
  }

    private resetLayouts() {
    this.showGlobalLayout = false;
    this.showAdminNavbar = false;
    this.showRechargeHeader = false;
  }

  @HostListener('window:scroll', ['$event'])
  onWindowScroll() {
    if (window.scrollY > 500) {
      this.showGoUpButton = true;
    } else {
      this.showGoUpButton = false;
    }
  }

  scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
  
}
