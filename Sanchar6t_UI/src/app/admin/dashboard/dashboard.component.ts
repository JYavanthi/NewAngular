import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
  stats = [
    { title: 'Pending Bookings', value: 0, icon: 'fa-solid fa-hourglass-half', color: 'blue' },
    { title: 'Total Tickets Sold', value: 1, icon: 'fa-solid fa-ticket', color: 'green' },
    { title: 'Payments List', value: 1, icon: 'fa-solid fa-dollar-sign', color: 'red' },
    { title: 'Total Terminals', value: 5, icon: 'fa-solid fa-road', color: 'purple' },
    { title: 'Available Schedules', value: 2, icon: 'fa-solid fa-calendar-days', color: 'yellow' },
    { title: 'Available Bus', value: 3, icon: 'fa-solid fa-bus', color: 'indigo' },
  ];

  sidebarItems = [
    'Dashboard',
    'Manage Bus',
    'Manage Terminal',
    'Manage Schedule',
    'List Bookings',
    'Tickets',
    'Payments List',
    'Bank List',
    'Report',
    'User Management'
  ];
}
