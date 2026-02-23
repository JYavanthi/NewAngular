import { Component, OnDestroy, OnInit } from '@angular/core';
import { UiStateServiceService } from '../services/ui-state.service.service';
import { AuthService } from '../auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent  implements OnInit, OnDestroy{
 currentDate: Date = new Date();
  private timer: any;


    constructor(private uiState: UiStateServiceService, 
      private authService: AuthService,
  private router: Router
    ) {}

  openRechargeFromNavbar() {
    this.uiState.enableRechargeMode();
  }
  ngOnInit(): void {
    this.timer = setInterval(() => {
      this.currentDate = new Date();
    }, 1000);
  }

  ngOnDestroy(): void {
    if (this.timer) {
      clearInterval(this.timer);
    }
  }

  recharge() {
  this.router.navigate(['/recharge']);
}

 gotoHome(){
  this.router.navigate(['/admin/agenHeader']);
}
}
