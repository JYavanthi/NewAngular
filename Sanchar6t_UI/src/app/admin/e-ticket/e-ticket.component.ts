import { Component, OnInit } from '@angular/core';
import { HttpServiceService } from '../../services/http-service.service';
import { API_URLS } from '../../shared/API-URLs';
import { BitlaService } from '../../services/bitla-service';
import { LoaderService } from '../../services/loader.service';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import { ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'app-e-ticket',
  templateUrl: './e-ticket.component.html',
  styleUrls: ['./e-ticket.component.scss']
})
export class ETicketComponent implements OnInit {

  ticketNumber: string = '';
  booking: any;
  passengers: any[] = [];
  boardingPoint: any;
  droppingPoint: any;

  travelDate: string = '';
  busName: string = '';
  busType: string = '';
  departureTime: string = '';
  seatNumbers: string = '';
  constructor(private httpService: HttpServiceService, private bitlaService: BitlaService,
    private loaderService: LoaderService) { }


    @ViewChild('ticketRef', { static: false }) ticketRef!: ElementRef;

downloadPDF(): void {
  const element = this.ticketRef.nativeElement as HTMLElement;

  const images = Array.from(
    element.querySelectorAll('img')
  ) as HTMLImageElement[];

  Promise.all(
    images.map(img => {
      if (img.complete) {
        return Promise.resolve(true);
      }
      return new Promise(resolve => {
        img.onload = img.onerror = () => resolve(true);
      });
    })
  ).then(() => {
    html2canvas(element, {
      scale: 3,
      useCORS: true
    }).then(canvas => {
      const imgData = canvas.toDataURL('image/png');

      const pdf = new jsPDF('p', 'mm', 'a4');
      const pdfWidth = 210;
      const pdfHeight = (canvas.height * pdfWidth) / canvas.width;

      pdf.addImage(imgData, 'PNG', 0, 0, pdfWidth, pdfHeight);
      pdf.save(`Ticket-${this.booking?.pnr_number}.pdf`);
    });
  });
}

ngOnInit(): void {

  // 1️⃣ Try to read from router state (best case)
  const navigation = history.state;
  const stateTicketNumber = navigation?.ticketNumber;

  // 2️⃣ Fallback to localStorage (refresh / direct URL)
  const storedTicketNumber = localStorage.getItem('TICKET_NUMBER');

  // 3️⃣ Final ticket number
  this.ticketNumber = stateTicketNumber || storedTicketNumber || '';

  if (!this.ticketNumber) {
    console.error('❌ Ticket number not found');
    return;
  }

  console.log('🎫 Loading ticket:', this.ticketNumber);

  // 4️⃣ Fetch booking details
  this.fetchBooking(this.ticketNumber);
}
  fetchBooking(ticketNumber: string): void {
    this.loaderService.show();

    // 👇 ticketNumber passed, service maps to pnr internally
    this.bitlaService.getBookingDetails(ticketNumber).subscribe({
      next: (res) => {
        this.loaderService.hide();

        const ticket =
          res?.bitlaResponse?.result?.ticket_details?.[0];

        if (!ticket) {
          console.error('❌ No ticket details found in API response');
          return;
        }

        // ✅ Patch UI here
        this.mapTicketDetails(ticket);
      },
      error: (err) => {
        this.loaderService.hide();
        console.error('❌ Error fetching ticket details:', err);
      }
    });
  }
mapTicketDetails(ticket: any): void {

  console.log('🎫 Mapping ticket details:', ticket);

  this.booking = ticket;

  /* ================= PASSENGERS ================= */
  this.passengers = ticket?.passenger_details || [];

  /* ================= BOARDING ================= */
  this.boardingPoint = {
    name: ticket?.boarding_point_details?.address || '',
    landmark: ticket?.boarding_point_details?.landmark || '',
    address: ticket?.boarding_point_details?.boarding_stage_address || '',
    contact: ticket?.boarding_point_details?.contact_numbers || '',
    time: ticket?.boarding_point_details?.dep_time || ''
  };

  /* ================= DROPPING ================= */
  this.droppingPoint = {
    name: ticket?.drop_off_details?.name || '',
    landmark: ticket?.drop_off_details?.landmark || '',
    address: ticket?.drop_off_details?.drop_off_address || '',
    contact: ticket?.drop_off_details?.contact_numbers || '',
    time: ticket?.drop_off_details?.arrival_time || ''
  };

  /* ================= TRIP INFO ================= */
  this.travelDate = ticket?.travel_date;
  this.busName = ticket?.travels;
  this.busType = ticket?.bus_type;
  this.departureTime = ticket?.dep_time;
  this.seatNumbers = ticket?.seat_numbers;

  console.log('✅ Ticket mapped successfully');
}

}
