import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { LoaderService } from '../services/loader.service';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import { ActivatedRoute, Router } from '@angular/router';
import { BitlaService } from '../services/bitla-service';
import { FormValidationService } from '../services/form-validation.service';

@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.scss']
})

export class TicketComponent implements OnInit {
  @ViewChild('ticketContainer')
  ticketContainer!: ElementRef;


  itinerary = [
    {
      day: 'Day 01',
      events: [
        { time: '8:30 PM', description: 'All guests are requested to be present at the Pickup point for departure at 9:00 PM.' },
        { time: '9:00 PM', description: 'Departure from Bangalore to Tirupati By Volvo Multi Axle AC Semi Sleeper Bus & AC sleeper' }
      ]
    },
    {
      day: 'Day 02',
      events: [
        { time: '4:00 to 4:30 AM', description: 'Arrival at Tirupati town. Check-in to the hotel to relax and freshen up. (1 hour time)' },
        { time: '5:45 AM', description: 'The Temple Darshan starts by first visiting Goddess Padmavathi Temple below the Tirumala Hill.' },
        { time: '6:45 AM', description: 'Breakfast for guests.' },
        { time: '7:15 to 7:45 AM', description: 'Guests are taken from Tirupati to Tirumala hill in an APSRTC bus.' },
        { time: '9:00 AM to 11:30 AM', description: 'Interested guests can take part in head shave if time permits. All guests are taken inside the temple premises through Sheeghra Darshan Tickets to seek the blessings of Lord Venkateshwara.' },
        { time: '1:15 PM to 2:00 PM', description: 'Guests return back to Tirupati and are taken to a restaurant for lunch.' },
        { time: '3:00 PM', description: 'Depart from Tirupati to Bangalore in the same comfortable bus.' },
        { time: '9:00 PM to 9:30 PM', description: 'Guests return to Bengaluru at the scheduled drop point.' }
      ]
    }
  ];

  included = [
    'Round-trip transportation from Bangalore to Tirupati in a comfortable air-conditioned bus.',
    'Accommodation for freshening up.',
    'Visit to Padhmavathi Temple',
    'Assistance of a knowledgeable tour guide.',
    'VIP Darshan ticket at Tirumala Tirupati Balaji Temple.',
    'Breakfast and lunch.',
    'Temple Laddu Prasadam (1 Free).',
    'All applicable taxes and service charges.'
  ];

  notIncluded = [
    'Any personal expenses and additional meals.',
    'Tips and gratuities.'
  ];

  termsAndConditions = [
    'Smoking and consumption of alcohol is strictly prohibited inside the coach.',
    'Contraband and explosive articles are not allowed, if found, passengers will not be allowed inside the coach.',
    'Management is not responsible for your luggage.',
    'Co-Seats of lady passengers to be confirmed to lady passenger only, no accommodation for male passengers.',
    'Changes to the origin and/or destination of travel and customer name changes are not permitted.',
    'Animals and live fish are not allowed inside the vehicle.',
    'Carrying Liquor Bottles, any Explosive Items, Fireworks/Crackers are prohibited. If found, passengers are not allowed to board the bus and the ticket fare will not be refunded.'
  ];

  cancellationPolicy = [
    'Between 1 to 7 days before station departure time: 100% Cancellation Charges',
    'Between 8 to 10 days before station departure time: 50% Cancellation Charges',
    'Between 11 to 14 days before station departure time: 20% Cancellation Charges'
  ];

  dressCode = {
    male: 'Dhoti, Shirt / Kurtha, Pyjama. (Male Traditional Attire)',
    female: 'Saree / Chudidar with Dupatta. (Female Traditional Attire)',
    avoid: 'Please avoid Jeans, Tshirts and shorts for the Lord Balaji temple visit.'
  };

  importantNotes = [
    'Devotees must carry the original documents (Aadharcard/voterID) provided at the time of booking for producing at the time of temple visit.',
    'Tariff and package operated by AP Tourism operational terms apply.',
    'Package includes Free accommodation for fresh up, breakfast, lunch & quick darshan of 300 rupees.',
    'As per TTD Rules Sheegra Darshanam Tirupati Balaji Package Tours Tickets have to be booked 7 days in advance. Hence, No Refund will be considered if tirumala package tickets are cancelled within 7 days of departure date.'
  ];

  activeTab = 'details';
  isLoading = false;
  previousActiveTab = '';

  constructor(private loaderService: LoaderService, private bitlaService: BitlaService,
    private router: Router, private formValidation: FormValidationService) { }

  ticketDetails: any = {};

  ngOnInit(): void {
    const state = history.state;

    if (!state?.pnrNumber) {
      console.error("No PNR number found!");
      return;
    }

    const pnr = state.pnrNumber;

    this.fetchTicketDetails(pnr);
  }

  setActiveTab(tab: string): void {
    this.activeTab = tab;
  }

  printTicket(): void {
    window.print();
  }

  private showAllTabContent(): void {
    this.previousActiveTab = this.activeTab;

    const tabContents = this.ticketContainer.nativeElement.querySelectorAll('.tab-content > div');

    tabContents.forEach((content: HTMLElement) => {
      content.style.display = 'block';
    });
  }

  private restoreTabState(): void {
    if (this.previousActiveTab) {
      this.activeTab = this.previousActiveTab;
      this.previousActiveTab = '';

      const tabContents = this.ticketContainer.nativeElement.querySelectorAll('.tab-content > div');

      tabContents.forEach((content: HTMLElement) => {
        if (!content.classList.contains(this.activeTab + '-container')) {
          content.style.display = 'none';
        }
      });
    }
  }

  ticketNo: string = '';
  pnrNumber: string = '';
  seatNo: string = '';

  CancelTicket() {
    const ticketNumber = this.ticketNo || this.pnrNumber;
    let seatNumbers = this.seatNo;

    if (!ticketNumber || !seatNumbers) {
      this.formValidation.showAlert("Ticket number or seat details missing!", "danger");
      return;
    }

    seatNumbers = seatNumbers.replace(/\s+/g, '');

    if (!confirm(`Are you sure you want to cancel Ticket: ${ticketNumber}?`)) {
      return;
    }

    this.loaderService.show();

    this.bitlaService.getCancelBooking(ticketNumber, seatNumbers).subscribe({

      next: (canRes: any) => {
        const canCancel = canRes?.bitlaResponse?.result?.can_cancel;

        if (!canCancel) {
          this.loaderService.hide();
          this.formValidation.showAlert("This ticket cannot be cancelled based on policy.", "danger");
          return;
        }

        this.bitlaService.getCancelBooking(ticketNumber, seatNumbers).subscribe({

          next: (cancelRes: any) => {
            this.loaderService.hide();

            const status = cancelRes?.bitlaResponse?.result?.status?.toLowerCase() || 'failed';

            if (status === "success") {
              this.formValidation.showAlert("Ticket cancelled successfully!", "success");
              setTimeout(() => this.router.navigate(['/bus-booking']), 2000);

            } else {
              this.formValidation.showAlert(
                cancelRes?.bitlaResponse?.result?.message || "Cancellation failed!",
                "danger"
              );
            }
          },

          error: (err) => {
            this.loaderService.hide();
            console.error("Error while cancelling:", err);
            this.formValidation.showAlert(
              err?.error?.response?.message || "Something went wrong during cancellation!",
              "danger"
            );
          }
        });
      },

      error: (err) => {
        this.loaderService.hide();
        console.error("Eligibility check error:", err);
        this.formValidation.showAlert("Unable to verify cancellation eligibility!", "danger");
      }

    });
  }


  updateLocalBookingStatus(ticketNo: string) {
    const loggedUser = JSON.parse(localStorage.getItem('user') ?? '{}');
    const userId = Number(loggedUser?.UserId ?? 0);

    const payload = {
      ticketNo: ticketNo,
      status: "Cancelled",
      modifiedBy: userId
    };
  }

  fetchTicketDetails(pnr: string): void {
    this.loaderService.show();

    this.bitlaService.getBookingDetails(pnr).subscribe({
      next: (res) => {
        this.loaderService.hide();
        const ticket = res.bitlaResponse?.result?.ticket_details?.[0];
        if (!ticket) {
          console.error("No ticket details found in API response");
          return;
        }
        this.mapTicketDetails(ticket);
      },
      error: (err) => {
        this.loaderService.hide();
        console.error("Error fetching ticket details:", err);
      }
    });
  }

  mapTicketDetails(ticket: any): void {

    const ticketNumber = ticket?.pnr_number || ticket?.ticket_number || '';
    const seatData = ticket?.seat_numbers || ticket?.passenger_details?.map((x: any) => x.seat_number);

    this.seatNo = Array.isArray(seatData) ? seatData.join(',') : seatData;
    this.ticketNo = ticketNumber;
    this.pnrNumber = ticketNumber;

    this.ticketDetails = {
      companyName: ticket?.travels ?? "-",
      companyAddress: `${ticket?.boarding_point_details?.address ?? '-'} - ${ticket?.boarding_point_details?.boarding_stage_address ?? '-'}`,
      companyPhone: [ticket?.boarding_point_details?.contact_numbers ?? '-'],
      companyEmail: "support@sanchar6t.com",
      companyWebsite: "www.sanchar6t.com",

      ticketNo: ticketNumber,
      route: `${ticket?.origin ?? '-'} → ${ticket?.destination ?? '-'}`,

      seatNumbers: `${this.seatNo} (${ticket?.no_of_seats ?? 1} seat)`,
      journeyDate: ticket?.travel_date ?? '-',
      depTime: ticket?.dep_time ?? '-',
      reportTime: ticket?.boarding_point_details?.address ?? '-',
      totalFare: ticket?.total_fare ?? '0',
      coachType: ticket?.bus_type ?? '-',

      boardingTime: ticket?.boarding_point_details?.dep_time ?? '-',
      boardingPoint: `${ticket?.boarding_point_details?.landmark ?? ''} - ${ticket?.boarding_point_details?.boarding_stage_address ?? ''}`,

      bookedOn: new Date().toLocaleString(),
      bookedBy: ticket?.passenger_details?.[0]?.name ?? '-',

      dropOff: `${ticket?.drop_off_details?.arrival_time ?? '-'} - ${ticket?.drop_off_details?.name ?? '-'} - ${ticket?.drop_off_details?.landmark ?? '-'} - ${ticket?.drop_off_details?.drop_off_address ?? '-'}`,
    };

    console.log("Mapped Ticket Details:", this.ticketDetails);
    console.log("Stored SeatNo:", this.seatNo);
    console.log("Stored PNR:", this.ticketNo);
  }


  generateWhatsAppText(): string {
    let whatsAppText = `🎫 *${this.ticketDetails.companyName}* 🎫\n\n`;

    whatsAppText += `*TICKET #${this.ticketDetails.ticketNo}*\n`;
    whatsAppText += `*${this.ticketDetails.route}*\n\n`;

    whatsAppText += `*JOURNEY DETAILS*\n`;
    whatsAppText += `📅 Date: ${this.ticketDetails.journeyDate}\n`;
    whatsAppText += `🕙 Time: ${this.ticketDetails.depTime}\n`;
    whatsAppText += `💺 Seat(s): ${this.ticketDetails.seatNumbers}\n`;
    whatsAppText += `💰 Fare: ${this.ticketDetails.totalFare}\n`;
    whatsAppText += `🚌 Coach: ${this.ticketDetails.coachType}\n\n`;

    whatsAppText += `*BOARDING DETAILS*\n`;
    whatsAppText += `🕙 Time: ${this.ticketDetails.boardingTime}\n`;
    whatsAppText += `📍 Point: ${this.ticketDetails.boardingPoint}\n`;
    whatsAppText += `⚠️ Please report ${this.ticketDetails.reportTime}\n\n`;

    whatsAppText += `*DROP-OFF DETAILS*\n`;
    whatsAppText += `📍 ${this.ticketDetails.dropOff}\n\n`;

    whatsAppText += `*ITINERARY SUMMARY*\n`;
    this.itinerary.forEach(day => {
      whatsAppText += `*${day.day}*\n`;
      day.events.forEach(event => {
        whatsAppText += `• ${event.time}: ${event.description}\n`;
      });
      whatsAppText += `\n`;
    });

    whatsAppText += `*DRESS CODE FOR TEMPLE*\n`;
    whatsAppText += `👨 Male: ${this.dressCode.male}\n`;
    whatsAppText += `👩 Female: ${this.dressCode.female}\n`;
    whatsAppText += `⚠️ ${this.dressCode.avoid}\n\n`;

    whatsAppText += `*IMPORTANT NOTES*\n`;
    whatsAppText += `• ${this.importantNotes[0]}\n`;
    whatsAppText += `• ${this.importantNotes[3]}\n\n`;

    whatsAppText += `*CONTACT US*\n`;
    whatsAppText += `📱 ${this.ticketDetails.companyPhone.join(' / ')}\n`;
    whatsAppText += `✉️ ${this.ticketDetails.companyEmail}\n`;
    whatsAppText += `🌐 ${this.ticketDetails.companyWebsite}\n\n`;

    whatsAppText += `Thank you for choosing ${this.ticketDetails.companyName}!`;

    return whatsAppText;
  }

  shareOnWhatsApp(): void {
    const whatsAppText = encodeURIComponent(this.generateWhatsAppText());
    const whatsappURL = `https://wa.me/?text=${whatsAppText}`;
    window.open(whatsappURL, '_blank');
  }

  downloadTicket(): void {
    this.loaderService.show();

    const pdf = new jsPDF({
      orientation: 'portrait',
      unit: 'mm',
      format: 'a4'
    });

    const margin = 15;
    const pageWidth = pdf.internal.pageSize.getWidth();
    const contentWidth = pageWidth - (margin * 2);

    const addSectionHeader = (title: string) => {
      pdf.setFillColor(140, 82, 255);
      pdf.rect(margin, margin, contentWidth, 15, 'F');
      pdf.setTextColor(255, 255, 255);
      pdf.setFontSize(16);
      pdf.text(title, pageWidth / 2, margin + 10, { align: 'center' });
      pdf.setTextColor(0, 0, 0);
      pdf.setFontSize(12);
    };

    const originalElement = this.ticketContainer.nativeElement;
    const clone = originalElement.cloneNode(true) as HTMLElement;

    const actionButtons = clone.querySelector('.ticket-actions');
    if (actionButtons) {
      actionButtons.remove();
    }

    const tabNav = clone.querySelector('.tabs');
    if (tabNav) {
      tabNav.remove();
    }

    clone.classList.add('pdf-generation-mode');

    const tempContainer = document.createElement('div');
    tempContainer.style.position = 'absolute';
    tempContainer.style.left = '-9999px';
    tempContainer.appendChild(clone);
    document.body.appendChild(tempContainer);

    const detailsContainer = clone.querySelector('.details-container') as HTMLElement;
    const itineraryContainer = clone.querySelector('.itinerary-container') as HTMLElement;
    const inclusionsContainer = clone.querySelector('.inclusions-container') as HTMLElement;
    const termsContainer = clone.querySelector('.terms-container') as HTMLElement;

    const processInclusionsContent = () => {
      if (inclusionsContainer) {
        const dressCodeSection = inclusionsContainer.querySelector('.dress-code');

        if (dressCodeSection) {
          const inclusionsPart1 = document.createElement('div');
          inclusionsPart1.className = 'inclusions-part1';

          const inclusionsPart2 = document.createElement('div');
          inclusionsPart2.className = 'inclusions-part2';

          const importantNotes = inclusionsContainer.querySelector('.important-notes');

          Array.from(inclusionsContainer.childNodes).forEach((node: Node) => {
            if ((node === dressCodeSection) || (importantNotes && node === importantNotes)) {
              return;
            }
            if (node.nodeType === Node.ELEMENT_NODE) {
              inclusionsPart1.appendChild((node as Element).cloneNode(true));
            }
          });

          if (dressCodeSection) {
            inclusionsPart2.appendChild(dressCodeSection.cloneNode(true));
          }

          if (importantNotes) {
            inclusionsPart2.appendChild(importantNotes.cloneNode(true));
          }

          return {
            part1: inclusionsPart1,
            part2: inclusionsPart2
          };
        }
      }
      return null;
    };

    const inclusionsParts = processInclusionsContent();

    const captureAndAddSection = (element: HTMLElement, title: string, isFirstPage: boolean = false, extraSpacing: number = 25): Promise<void> => {
      return new Promise<void>((resolve) => {
        if (!isFirstPage) {
          pdf.addPage();
        }

        addSectionHeader(title);

        if (detailsContainer) detailsContainer.style.display = 'none';
        if (itineraryContainer) itineraryContainer.style.display = 'none';
        if (inclusionsContainer) inclusionsContainer.style.display = 'none';
        if (termsContainer) termsContainer.style.display = 'none';

        element.style.display = 'block';

        const headerElement = clone.querySelector('.ticket-header') as HTMLElement;
        const currentElements = [headerElement, element];

        const sectionContainer = document.createElement('div');
        sectionContainer.style.backgroundColor = 'white';
        sectionContainer.style.padding = '0';
        sectionContainer.style.margin = '0';
        sectionContainer.style.width = '210mm';

        currentElements.forEach(el => {
          if (el) {
            const elClone = el.cloneNode(true);
            sectionContainer.appendChild(elClone);
          }
        });

        const sectionTempContainer = document.createElement('div');
        sectionTempContainer.style.position = 'absolute';
        sectionTempContainer.style.left = '-9999px';
        sectionTempContainer.appendChild(sectionContainer);
        document.body.appendChild(sectionTempContainer);

        html2canvas(sectionContainer, {
          scale: 2,
          useCORS: true,
          allowTaint: true,
          backgroundColor: '#FFFFFF'
        }).then(canvas => {
          const imgData = canvas.toDataURL('image/png');
          const imgWidth = contentWidth;
          const imgHeight = (canvas.height * imgWidth) / canvas.width;

          pdf.addImage(imgData, 'PNG', margin, margin + extraSpacing, imgWidth, imgHeight);

          document.body.removeChild(sectionTempContainer);
          resolve();
        });
      });
    };

    const captureAndAddInclusionsPart = (part: HTMLElement, title: string, extraSpacing: number = 25): Promise<void> => {
      return new Promise<void>((resolve) => {
        if (!part) {
          resolve();
          return;
        }

        pdf.addPage();
        addSectionHeader(title);

        const sectionContainer = document.createElement('div');
        sectionContainer.style.backgroundColor = 'white';
        sectionContainer.style.padding = '0';
        sectionContainer.style.margin = '0';
        sectionContainer.style.width = '210mm';

        const headerElement = clone.querySelector('.ticket-header') as HTMLElement;
        if (headerElement) {
          sectionContainer.appendChild(headerElement.cloneNode(true));
        }
        sectionContainer.appendChild(part);

        const tempContainer = document.createElement('div');
        tempContainer.style.position = 'absolute';
        tempContainer.style.left = '-9999px';
        document.body.appendChild(tempContainer);
        tempContainer.appendChild(sectionContainer);

        html2canvas(sectionContainer, {
          scale: 2,
          useCORS: true,
          allowTaint: true,
          backgroundColor: '#FFFFFF'
        }).then(canvas => {
          const imgData = canvas.toDataURL('image/png');
          const imgWidth = contentWidth;
          const imgHeight = (canvas.height * imgWidth) / canvas.width;

          pdf.addImage(imgData, 'PNG', margin, margin + extraSpacing, imgWidth, imgHeight);

          document.body.removeChild(tempContainer);
          resolve();
        });
      });
    };

    Promise.resolve()
      .then(() => captureAndAddSection(detailsContainer, 'Ticket Details', true, 30))
      .then(() => captureAndAddSection(itineraryContainer, 'Itinerary', false, 30))
      .then(() => {
        if (inclusionsParts) {
          return captureAndAddInclusionsPart(inclusionsParts.part1, 'Inclusions', 30)
            .then(() => captureAndAddInclusionsPart(inclusionsParts.part2, 'Important Information', 30));
        } else {
          return captureAndAddSection(inclusionsContainer, 'Inclusions', false, 30);
        }
      })
      .then(() => captureAndAddSection(termsContainer, 'Terms & Conditions', false, 30))
      .then(() => {
        const totalPages = pdf.internal.pages.length - 1;
        for (let i = 1; i <= totalPages; i++) {
          pdf.setPage(i);
          pdf.setFontSize(10);
          pdf.setTextColor(100, 100, 100);
          pdf.text(
            `Sanchar6t Tours & Travels - Ticket #${this.ticketDetails.ticketNo} - Page ${i} of ${totalPages}`,
            pageWidth / 2,
            pdf.internal.pageSize.getHeight() - 10,
            { align: 'center' }
          );
        }

        pdf.save(`Sanchar6t_Ticket_${this.ticketDetails.ticketNo}.pdf`);

        document.body.removeChild(tempContainer);
        this.loaderService.hide();

        this.restoreTabState();
      })
      .catch(error => {
        console.error("Error generating PDF:", error);

        if (document.body.contains(tempContainer)) {
          document.body.removeChild(tempContainer);
        }
        this.loaderService.hide();
        this.restoreTabState();

        alert("Error generating PDF. Please try again.");
      });
  }
}