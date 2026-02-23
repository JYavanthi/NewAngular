

import {
  Component,
  OnInit,
  AfterViewInit,
  ViewChild,
  ElementRef
} from '@angular/core';

import Chart from 'chart.js/auto';
import { DashboardService } from '../../../services/dashboard.service';

@Component({
  selector: 'app-admin-home',
  templateUrl: './admin-home.component.html',
  styleUrl: './admin-home.component.scss'
})
export class AdminHomeComponent implements OnInit, AfterViewInit {

  // ================= PIE CHART =================
  @ViewChild('destinationChart')
  destinationChart!: ElementRef<HTMLCanvasElement>;

  chart!: Chart;
  showChart: boolean = false;

  @ViewChild('revenueChart')
revenueChart!: ElementRef<HTMLCanvasElement>;

revenueLineChart!: Chart;
  // ================= CALENDAR =================
  currentMonth!: number;
  currentYear!: number;
totalBookings: number = 0;
totalCustomers: number = 0;
totalEarnings: number = 0; 
revenueList: any[] = [];
selectedFilter: string = "weekly";
// revenueLineChart: any;
  constructor(private dashboardService: DashboardService) {}


  monthNames: string[] = [
    'January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December'
  ];

  weekDays: string[] = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];

  calendarDays: any[] = [];

  selectedDay: number | null = null;
  rangeStart: number = 0;
  rangeEnd: number = 0;


  // ================= INIT =================
  // ngOnInit(): void {
    
  //   const today = new Date();

  //   this.currentMonth = today.getMonth();
  //   this.currentYear = today.getFullYear();

  //   this.generateCalendar();
    
  // }

  ngOnInit(): void {

  // ✅ Call Total Booking API
  this.getTotalBookings();
 this.getTotalCustomers();
   this.getTotalEarnings();
    this.getRevenueData();
  // Existing Calendar Code
  const today = new Date();

  this.currentMonth = today.getMonth();
  this.currentYear = today.getFullYear();

  this.generateCalendar();
}

ngAfterViewInit(): void {
  this.showChart = true;

  setTimeout(() => {
    this.createChart(); 
  }, 300);
}



getRevenueData(): void {

  this.dashboardService.getpayment().subscribe({
    next: (res: any) => {

      console.log("Payment Revenue Response:", res);

      // ✅ Because API returns direct array
      this.revenueList = res || [];

      // ✅ Default chart load
      this.updateRevenueChart();
    },
    error: (err) => {
      console.log("Revenue API Error:", err);
      this.revenueList = [];
    }
  });

}
updateRevenueChart() {

  if (!this.revenueList.length) return;

  let labels: string[] = [];
  let values: number[] = [];

  if (this.selectedFilter === "weekly") {
    ({ labels, values } = this.getWeeklyRevenue());
  }

  if (this.selectedFilter === "monthly") {
    ({ labels, values } = this.getMonthlyRevenue());
  }

  if (this.selectedFilter === "yearly") {
    ({ labels, values } = this.getYearlyRevenue());
  }

  this.renderRevenueChart(labels, values);
}

getWeeklyRevenue() {

  const weekDays = ['Sun','Mon','Tue','Wed','Thu','Fri','Sat'];

  let revenueMap = new Array(7).fill(0);

  this.revenueList.forEach(item => {

    const date = new Date(item.createdDt);
    const dayIndex = date.getDay();

    revenueMap[dayIndex] += item.amount;
  });

  return { labels: weekDays, values: revenueMap };
}

getMonthlyRevenue() {

  const labels: string[] = [];
  const revenueMap = new Array(31).fill(0);

  // Labels = 1 to 31
  for (let i = 1; i <= 31; i++) {
    labels.push(i.toString());
  }

  this.revenueList.forEach(item => {

    const date = new Date(item.createdDt);
    const day = date.getDate(); // 1–31

    revenueMap[day - 1] += item.amount;
  });

  return { labels, values: revenueMap };
}

getYearlyRevenue() {

  const labels = [
    'Jan','Feb','Mar','Apr','May','Jun',
    'Jul','Aug','Sep','Oct','Nov','Dec'
  ];

  let revenueMap = new Array(12).fill(0);

  this.revenueList.forEach(item => {

    const date = new Date(item.createdDt);
    const monthIndex = date.getMonth();

    revenueMap[monthIndex] += item.amount;
  });

  return { labels, values: revenueMap };
}
renderRevenueChart(labels: string[], values: number[]) {

  if (this.revenueLineChart) {
    this.revenueLineChart.destroy();
  }

  this.revenueLineChart = new Chart(this.revenueChart.nativeElement, {
    type: 'line',

    data: {
      labels: labels,
      datasets: [
        {
          label: "Revenue",
          data: values,
          borderColor: "#f5a25d",
          backgroundColor: "transparent",
          borderWidth: 3,
          tension: 0.4,
          pointRadius: 4
        }
      ]
    },

    options: {
      responsive: true,
      plugins: {
        legend: { display: false }
      },
      scales: {
        y: {
          beginAtZero: true
        }
      }
    }
  });
}


onFilterChange(event: any) {
  console.log("Selected Filter:", event.target.value);

  this.selectedFilter = event.target.value;
  this.updateRevenueChart();
}

getTotalCustomers(): void {

  this.dashboardService.getUserDetails().subscribe({
    next: (res: any) => {

      console.log("User API Response:", res);

      // ✅ If API gives count directly
      this.totalCustomers = res.count;

      // OR if list comes in data
      // this.totalCustomers = res.data.length;
    },

    error: (err) => {
      console.log("Customer API Error:", err);
    }
  });

}

getTotalBookings(): void {

  this.dashboardService.getBookingDetails().subscribe({
    next: (res: any) => {

      console.log("Booking API Response:", res);

      this.totalBookings = res.count;

    },

    error: (err) => {
      console.log("Booking API Error:", err);
    }
  });

}

getTotalEarnings(): void {

  this.dashboardService.getTotalpayment().subscribe({
    next: (res: any) => {

      console.log("Total Payment Response:", res);

      this.totalEarnings = res;

    },

    error: (err) => {
      console.log("Total Payment API Error:", err);
    }
  });

}


createRevenueChart() {

  if (!this.revenueChart) return;

  if (this.revenueLineChart) {
    this.revenueLineChart.destroy();
  }

  this.revenueLineChart = new Chart(this.revenueChart.nativeElement, {
    type: 'line',

    data: {
      labels: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
      datasets: [
        {
          data: [400, 450, 300, 635, 420, 600, 550],

          borderColor: '#f5a25d',
          backgroundColor: 'transparent',

          tension: 0.4,
          pointRadius: 5,
          pointHoverRadius: 7,

          pointBackgroundColor: '#f5a25d',
          borderWidth: 3,
          fill: false
        }
      ]
    },

    options: {
      responsive: true,
      maintainAspectRatio: false,

      plugins: {
        legend: { display: false }
      },

      scales: {
        x: {
          grid: {
            display: false
          }
        },

        y: {
          grid: {
            display: true
          },
          border: {
            display: false   // ✅ instead of drawBorder
          },
          ticks: {
            stepSize: 200
          }
        }
      }
    }
  });
}

  // ================= CREATE CHART =================
  createChart() {

    if (!this.destinationChart) return;

    if (this.chart) {
      this.chart.destroy();
    }

    this.chart = new Chart(this.destinationChart.nativeElement, {
      type: 'doughnut',
      data: {
        labels: ['Tokyo', 'Sydney', 'Paris', 'Venice'],
        datasets: [
          {
            data: [35, 28, 22, 15],
            backgroundColor: [
              '#ff8c42',
              '#ffa85c',
              '#ffd5b5',
              '#ffe9d6'
            ],
            borderWidth: 0
          }
        ]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        cutout: '70%',
        plugins: {
          legend: { display: false }
        }
      }
    });
  }


  // ================= CALENDAR GENERATE =================
  generateCalendar() {

    this.calendarDays = [];

    const firstDay = new Date(this.currentYear, this.currentMonth, 1).getDay();
    const totalDays = new Date(this.currentYear, this.currentMonth + 1, 0).getDate();

    const prevMonthDays = new Date(this.currentYear, this.currentMonth, 0).getDate();

    // Previous month muted days
    for (let i = firstDay - 1; i >= 0; i--) {
      this.calendarDays.push({
        day: prevMonthDays - i,
        currentMonth: false
      });
    }

    // Current month days
    for (let d = 1; d <= totalDays; d++) {
      this.calendarDays.push({
        day: d,
        currentMonth: true
      });
    }

    // Next month fill
    while (this.calendarDays.length < 42) {
      this.calendarDays.push({
        day: this.calendarDays.length - totalDays - firstDay + 1,
        currentMonth: false
      });
    }
  }


  // ================= NEXT MONTH =================
  nextMonth() {
    if (this.currentMonth === 11) {
      this.currentMonth = 0;
      this.currentYear++;
    } else {
      this.currentMonth++;
    }

    this.generateCalendar();
  }


  // ================= PREVIOUS MONTH =================
  prevMonth() {
    if (this.currentMonth === 0) {
      this.currentMonth = 11;
      this.currentYear--;
    } else {
      this.currentMonth--;
    }

    this.generateCalendar();
  }


  // ================= SELECT DATE =================
  selectDate(date: any) {

    if (!date.currentMonth) return;

    this.selectedDay = date.day;

    if (this.rangeStart === 0 || this.rangeEnd !== 0) {
      this.rangeStart = date.day;
      this.rangeEnd = 0;
    } else {
      this.rangeEnd = date.day;

      if (this.rangeEnd < this.rangeStart) {
        let temp = this.rangeStart;
        this.rangeStart = this.rangeEnd;
        this.rangeEnd = temp;
      }
    }
  }

}

