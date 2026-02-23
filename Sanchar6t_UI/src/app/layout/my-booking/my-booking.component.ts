import { Component } from '@angular/core';

@Component({
  selector: 'app-my-booking',
  templateUrl: './my-booking.component.html',
  styleUrl: './my-booking.component.scss'
})
export class MyBookingComponent {
  selectedTab: number = 0;

  onTabChange(event: any) {
    console.log('hi', event.index);
  }
  
}
