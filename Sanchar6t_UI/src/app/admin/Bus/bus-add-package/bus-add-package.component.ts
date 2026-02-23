import { Component } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { FormValidationService } from '../../../services/form-validation.service';
import { NavigationEnd, Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-bus-add-package',
  templateUrl: './bus-add-package.component.html',
  styleUrl: './bus-add-package.component.scss',
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

export class BusAddPackageComponent {
  selectedTab: number = 0;
  packageID: number | null = null;
  private routerSubscription!: Subscription;

  constructor(private formValidation: FormValidationService, private router: Router) {
  }

  ngOnInit() {
    this.packageID = this.formValidation.getPackageID();
    this.routerSubscription = this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd && !event.url.includes('/bus-add-package')) {
        this.formValidation.clearPackageID();
      }
    });
  }

  ngOnDestroy() {
    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe();
    }
  }

  setSelectedTab(tabIndex: number) {
    this.selectedTab = tabIndex;
  }

  onPackageGenerated(packageID: number) {
    this.packageID = packageID;
    this.formValidation.setPackageID(packageID);
  }

  onTabChange(event: any) {
    console.log('hi', event.index);
  }
}
