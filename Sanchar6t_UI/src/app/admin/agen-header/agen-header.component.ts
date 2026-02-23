import { Component, HostListener, OnInit } from '@angular/core';
import { BitlaService, City, CityPair } from '../../services/bitla-service';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
// import { BitlaService, City, CityPair } from '../../../services/bitla-service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { HttpServiceService } from '../../services/http-service.service';
import { API_URLS } from '../../shared/API-URLs';

@Component({
  selector: 'app-agen-header',
  templateUrl: './agen-header.component.html',
  styleUrl: './agen-header.component.scss'
})


export class AgenHeaderComponent {
  passengerForm!: FormGroup;
  isBookingInProgress = false;
  rechargeAmount: number = 0;
  // selectedBusId: number | null = null;
  cityList: City[] = [];
  cityPairList: CityPair[] = [];
  isSidebarOpen = false;
  filteredDepartureCities: City[] = [];
  filteredDestinationCities: City[] = [];
  selectedOriginId: number | null = null;
  showOriginList = false;
  showDestinationList = false;
  lowerDeck: any[] = [];
  upperDeck: any[] = [];
  // 🔑 keyboard navigation index
  originActiveIndex = -1;
  destinationActiveIndex = -1;
  showPassengerModal = false;
  originInputText = "";
  destinationInputText = "";
  selectedDestinationId: number | null = null;
  selectedTravelDateISO = '';
  availabilityData: any = null;
  schedules: any[] = [];
  isLoadingSchedules = false;
  selectedBus: any = null;

  allDates: {
    date: Date;
    label: string;
  }[] = [];

  visibleDates: any[] = [];
  isRechargeMode: boolean = false;

  startIndex = 0;
  visibleCount = 4;     // how many dates visible
  activeIndex = 0;
  boardingPoints: any[] = [];
  droppingPoints: any[] = [];

  selectedBoardingPoint: any = null;
  selectedDroppingPoint: any = null;

  // 🔥 stage id → name map
  stageNamesMap: { [key: string]: string } = {};


  showOnwards = false;
  selectedOnwardDate = '';

  days = ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'];
  monthNames = [
    'January', 'February', 'March', 'April',
    'May', 'June', 'July', 'August',
    'September', 'October', 'November', 'December'
  ];

  currentMonth = new Date().getMonth();
  currentYear = new Date().getFullYear();

  calendarDates: (number | null)[] = [];

  bookedSeats = new Set<string>([
    'L-3',
    'U-2'
  ]);

  selectedSeats = new Set<string>();


  getSeatLabel(deck: 'L' | 'U', row: number, col: number): string {
    const seatNo = (row * 3) + col + 1;

    return `${deck}${seatNo}`;
  }


  seatKey(deck: 'L' | 'U', row: number, col: number) {
    return `${deck}-${row}-${col}`;
  }
  getSeatClass(deck: 'L' | 'U', row: number, col: number) {
    const key = this.seatKey(deck, row, col);

    if (this.bookedSeats.has(key)) return 'booked';
    if (this.selectedSeats.has(key)) return 'selected';

    return 'available';
  }

  toggleSeat(deck: 'L' | 'U', row: number, col: number) {
    const key = this.seatKey(deck, row, col);

    if (this.bookedSeats.has(key)) {
      console.warn('🚫 Seat blocked:', key);
      return;
    }

    if (this.selectedSeats.has(key)) {
      this.selectedSeats.delete(key);
    } else {
      this.selectedSeats.add(key);
    }

    console.log('🎯 Selected seats:', Array.from(this.selectedSeats));
  }



  isSelected(deck: 'L' | 'U', row: number, col: number) {
    return this.selectedSeats.has(this.seatKey(deck, row, col));
  }

  constructor(
    private bitlaService: BitlaService,
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router,
        private httpService: HttpServiceService,
    

  ) { }

  buildPassengerForm() {
    const passengers = Array.from(this.selectedSeats).map(seatKey => {
      return this.fb.group({
        seatNo: [seatKey],

        name: ['', Validators.required],
        age: ['', [Validators.required, Validators.min(1)]],
        gender: ['', Validators.required],

        phone: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
        address: ['', Validators.required],

        documentType: ['AADHAR', Validators.required],
        documentNumber: ['', Validators.required],

        isNri: [false],
        isPregnant: [false]
      });
    });

    this.passengerForm = this.fb.group({
      passengers: this.fb.array(passengers),

      // 🔥 common contact details
      alternativePhone: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      email: ['', [Validators.required, Validators.email]]
    });
  }







  get passengers(): FormArray {
    return this.passengerForm.get('passengers') as FormArray;
  }


  // ngOnInit(): void {
  //   this.loadCities();
  //   this.loadCityPairs();
  //   this.initDates();
  //   this.updateVisibleDates();
  //   this.generateCalendar();
  // }


  ngOnInit(): void {

  // ===============================
  // 🔁 HANDLE RETURN FROM RECHARGE
  // ===============================
  const instantRechargeId =
    history.state?.instantRechargeId ||
    Number(localStorage.getItem('instantRechargeId'));

  if (instantRechargeId) {

    console.log('Checking recharge status for:', instantRechargeId);

    // 🔥 VERIFY STATUS FROM BACKEND (SOURCE OF TRUTH)
    this.httpService.httpGet(
      API_URLS.get_AgentInstantRechargeById + '/' + instantRechargeId
    ).subscribe({   
      next: (res: any) => {

        // ✅ Already success → nothing to do
        if (res?.status === 'Success') {
          console.log('Recharge already successful');
          return;
        }

        // 🔁 Payment success but recharge not updated
        this.httpService.httpPost(
          API_URLS.update_AgentInstantRechargeStatus,
          {
            instantRechargeId,
            status: 'Success',
            remarks: 'Auto-updated after PhonePe redirect',
            modifiedBy: res?.userId
          }
        ).subscribe(() => {
          console.log('Recharge auto-updated to Success');
        });
      },
      error: (err) => {
        console.error('Failed to verify recharge status', err);
      }
    });
  }

  // ===============================
  // 📊 EXISTING DASHBOARD LOGIC
  // ===============================
  this.loadCities();
  this.loadCityPairs();
  this.initDates();
  this.updateVisibleDates();
  this.generateCalendar();
}

  toggleBusInfo(bus: any) {

    // 🔁 Toggle: same bus → close
    if (this.selectedBus === bus) {
      this.selectedBus = null;
      return;
    }

    // ✅ Open only this bus
    this.selectedBus = bus;

    console.log('🟢 Bus info toggled for:', bus);
  }

  getSeatFare(bus: any): number {
    const fareStr = bus.o_fare_str || bus.fare_str;
    if (!fareStr) return 0;

    const firstFare = fareStr.split(',')[0];
    const price = firstFare.split(':')[1];

    return Number(price);
  }


  toggleOnwards(event: MouseEvent) {
    event.stopPropagation();
    this.showOnwards = !this.showOnwards;
  }

  @HostListener('document:click', ['$event'])
  closeAllDropdowns(event: MouseEvent) {
    const target = event.target as HTMLElement;

    // 🔥 DO NOT close when clicking inside bus card
    if (target.closest('.bus-card')) return;

    this.showOriginList = false;
    this.showDestinationList = false;
    this.showOnwards = false;

    this.originActiveIndex = -1;
    this.destinationActiveIndex = -1;
  }



  /* 🔹 GENERATE CALENDAR */
  generateCalendar() {
    this.calendarDates = [];

    const firstDay = new Date(this.currentYear, this.currentMonth, 1).getDay();
    const daysInMonth = new Date(
      this.currentYear,
      this.currentMonth + 1,
      0
    ).getDate();

    // empty slots before first date
    for (let i = 0; i < firstDay; i++) {
      this.calendarDates.push(null);
    }

    // actual dates
    for (let d = 1; d <= daysInMonth; d++) {
      this.calendarDates.push(d);
    }
  }

  /* 🔹 NEXT MONTH */
  nextMonth() {
    if (this.currentMonth === 11) {
      this.currentMonth = 0;
      this.currentYear++;
    } else {
      this.currentMonth++;
    }
    this.generateCalendar();
  }

  /* 🔹 PREVIOUS MONTH */
  prevMonth() {
    if (this.currentMonth === 0) {
      this.currentMonth = 11;
      this.currentYear--;
    } else {
      this.currentMonth--;
    }
    this.generateCalendar();
  }

  /* 🔹 SELECT DATE */
  selectOnwardDate(date: number) {
    const d = new Date(this.currentYear, this.currentMonth, date);

    this.selectedOnwardDate =
      `${this.monthNames[this.currentMonth]} ${date}, ${this.currentYear}`;

    // API format
    this.selectedTravelDateISO = d.toISOString().split('T')[0];

    // 🔥 SYNC DATE STRIP
    this.syncDateStripWithSelectedDate(d);

    this.showOnwards = false;
  }


  searchSchedules() {
    if (!this.selectedOriginId || !this.selectedDestinationId || !this.selectedTravelDateISO) {
      alert('Please select Origin, Destination and Onward Date');
      return;
    }

    this.isLoadingSchedules = true;

    this.bitlaService
      .getSchedules(
        this.selectedOriginId,
        this.selectedDestinationId,
        this.selectedTravelDateISO
      )
      .subscribe({
        next: (res: any) => {
          this.stageNamesMap = res.stage_names || {};
          // 🔥 STEP 1: MAP ARRAY-BASED RESPONSE TO OBJECTS
          const rows = res?.result || [];

          if (rows.length > 1) {
            const headers = rows[0]; // first row contains column names

            this.schedules = rows.slice(1).map((row: any[]) => {
              const obj: any = {};
              headers.forEach((key: string, index: number) => {
                obj[key] = row[index];
              });
              return obj;
            });
          } else {
            this.schedules = [];
          }

          // 🔥 STEP 2: PATCH DATE FROM API (optional but recommended)
          if (this.schedules.length > 0 && this.schedules[0].travel_date) {
            const apiDate = new Date(this.schedules[0].travel_date);
            this.patchDateFromApi(apiDate);
          }

          console.log('FINAL SCHEDULE OBJECTS →', this.schedules);

          this.isLoadingSchedules = false;
        },
        error: (err) => {
          console.error('Schedule API Error', err);
          this.isLoadingSchedules = false;
        }
      });
  }

  parseAmenities(amenities: string): string {
    try {
      const arr = JSON.parse(amenities || '[]');
      return `${arr.length} Amenities`;
    } catch {
      return '0 Amenities';
    }
  }


  patchDateFromApi(date: Date) {
    // ISO for API
    this.selectedTravelDateISO = date.toISOString().split('T')[0];

    // Text for Onwards input
    this.selectedOnwardDate = `${this.monthNames[date.getMonth()]} ${date.getDate()}, ${date.getFullYear()}`;

    // 🔥 Sync with date strip
    this.syncDateStripWithSelectedDate(date);
  }
  getCityName(cityId: number): string {
    return this.cityList.find(c => c.id === cityId)?.name || '';
  }

  initDates() {
    const today = new Date();
    this.generateDatesFrom(today, 30);
  }

  generateDatesFrom(startDate: Date, days: number) {
    let d = new Date(startDate);

    for (let i = 0; i < days; i++) {
      this.allDates.push({
        date: new Date(d),
        label: this.formatDate(d)
      });
      d.setDate(d.getDate() + 1);
    }
  }

  /* FORMAT */
  formatDate(date: Date): string {
    return date.toLocaleDateString('en-GB', {
      day: '2-digit',
      month: 'short',
      year: '2-digit',
      weekday: 'short'
    });
  }

  updateVisibleDates() {
    this.visibleDates = this.allDates.slice(
      this.startIndex,
      this.startIndex + this.visibleCount
    );
  }

  nextDates() {
    this.startIndex++;

    if (this.startIndex + this.visibleCount >= this.allDates.length - 5) {
      const lastDate = this.allDates[this.allDates.length - 1].date;
      const nextStart = new Date(lastDate);
      nextStart.setDate(nextStart.getDate() + 1);

      this.generateDatesFrom(nextStart, 30); // add 30 more days
    }

    this.updateVisibleDates();
  }

  prevDates() {
    if (this.startIndex > 0) {
      this.startIndex--;
      this.updateVisibleDates();
    }
  }


  syncDateStripWithSelectedDate(selectedDate: Date) {
    const index = this.allDates.findIndex(d =>
      d.date.toDateString() === selectedDate.toDateString()
    );

    if (index !== -1) {
      this.activeIndex = index;

      if (
        index < this.startIndex ||
        index >= this.startIndex + this.visibleCount
      ) {
        this.startIndex = Math.max(0, index - Math.floor(this.visibleCount / 2));
        this.updateVisibleDates();
      }
    }
  }

  /* SELECT */
  selectDate(index: number) {
    this.activeIndex = this.startIndex + index;

    const d = this.allDates[this.activeIndex].date;
    this.selectedTravelDateISO = d.toISOString().split('T')[0];
    this.selectedOnwardDate = this.formatDate(d);

    this.searchSchedules();
  }
  loadCities() {
    this.bitlaService.getCities().subscribe((res: any) => {
      console.log("CITY RESPONSE →", res);

      const rows = res.result;

      this.cityList = rows.slice(1).map((row: any[]) => ({
        id: row[0],
        name: row[1],
        origin_count: row[2],
        destination_count: row[3]
      }));

      this.filteredDepartureCities = [...this.cityList];
      this.filteredDestinationCities = [...this.cityList];

      console.log("FINAL CITY LIST →", this.cityList);
    });
  }

  loadCityPairs() {
    this.bitlaService.getCityPairs().subscribe((res: any) => {
      console.log("CITY PAIRS RAW RESPONSE →", res);

      const rows = res.result;

      this.cityPairList = rows.slice(1).map((row: any[]) => ({
        origin_id: row[0],
        destination_id: row[1],
        travel_ids: row[2]
      }));

      console.log("MAPPED PAIRS →", this.cityPairList);
    });
  }

  filterOrigin(event: any) {
    const keyword = event.target.value.toLowerCase();
    this.originInputText = event.target.value;

    this.filteredDepartureCities = this.cityList.filter(c =>
      c.name.toLowerCase().includes(keyword)
    );
  }

  filterDestination(event: any) {
    const keyword = event.target.value.toLowerCase();
    this.destinationInputText = event.target.value;

    this.filteredDestinationCities = this.filteredDestinationCities.filter(d =>
      d.name.toLowerCase().includes(keyword)
    );
  }

  selectOrigin(city: City) {
    this.originInputText = city.name;
    this.selectedOriginId = city.id;
    this.showOriginList = false;

    const validDestIds = this.cityPairList
      .filter(p => p.origin_id === city.id)
      .map(p => p.destination_id);

    this.filteredDestinationCities = this.cityList.filter(c =>
      validDestIds.includes(c.id)
    );

    this.destinationInputText = "";
  }
  selectDestination(city: City) {
    this.destinationInputText = city.name;
    this.selectedDestinationId = city.id;
    this.showDestinationList = false;
  }

  convertUIKeyToBitlaSeat(key: string): string {
    const [deck, row, col] = key.split('-');
    const seatNo = Number(row) * 3 + Number(col);
    return `${deck}${seatNo}`;
  }
  convertUISeatToBitla(seatKey: string): string {
    const [deck, rowStr, colStr] = seatKey.split('-');
    const row = Number(rowStr);
    const col = Number(colStr);
    const seatNo = (row * 3) + col + 1;
    return `${deck}${seatNo}`;
  }

  generateTicketAndSave() {

    // 🔒 prevent double click
    if (this.isBookingInProgress) return;
    this.isBookingInProgress = true;

    const seatsArray: string[] = Array.from(this.selectedSeats);

    /* ================= VALIDATIONS ================= */

    if (!this.selectedBus) {
      alert('Please select bus again');
      this.isBookingInProgress = false;
      return;
    }

    if (this.selectedSeats.size === 0) {
      alert('Please select seats');
      this.isBookingInProgress = false;
      return;
    }

    if (!this.passengerForm || this.passengerForm.invalid) {
      alert('Please fill passenger details');
      this.isBookingInProgress = false;
      return;
    }

    /* ================= PREP DATA ================= */

    const firstPassenger = this.passengers.at(0) as FormGroup;

    const boardingStageId =
      this.selectedBoardingPoint?.stageId?.toString();

    const droppingStageId =
      this.selectedDroppingPoint?.stageId?.toString();

    if (!boardingStageId || !droppingStageId) {
      alert('Please select boarding and dropping points');
      this.isBookingInProgress = false;
      return;
    }

    const fareAmount = String(
      this.selectedBus?.show_fare_screen ||
      this.selectedBus?.fare ||
      this.selectedBus?.fare_str ||
      0
    );

    /* ================= BITLA REQUEST ================= */

    const bitlaRequest = {
      pause_notification_for_branch_upi_payment_link: "false",
      book_ticket: {
        pause_notification_for_branch_upi_payment_link: "false",
        seat_details: {
          seat_detail: seatsArray.map((seat, index) => {
            const passenger = this.passengers.at(index) as FormGroup;

            return {
              seat_number: this.convertUISeatToBitla(seat),
              fare: fareAmount,
              title: passenger.get('gender')?.value === 'M' ? 'Mr' : 'Ms',
              name: passenger.get('name')?.value || '',
              age: passenger.get('age')?.value?.toString() || '',
              sex: passenger.get('gender')?.value,
              is_primary: index === 0 ? 'true' : 'false',
              id_card_type: '1',
              id_card_number: passenger.get('documentNumber')?.value || '',
              id_card_issued_by: 'NA'
            };
          })

        },
        contact_detail: {
          mobile_number: this.passengerForm.get('alternativePhone')?.value,
          emergency_name: firstPassenger.get('name')?.value || '',
          email: this.passengerForm.get('email')?.value
        }
      },
      origin_id: String(this.selectedOriginId),
      destination_id: String(this.selectedDestinationId),
      boarding_at: boardingStageId,
      drop_of: droppingStageId,
      no_of_seats: String(this.selectedSeats.size),
      travel_date: this.selectedBus.travel_date,
      customer_company_gst: {
        name: '',
        gst_id: '',
        address: ''
      }
    };

    console.log('✅ BITLA REQUEST:', bitlaRequest);

    /* ================= TENTATIVE BOOKING ================= */

    const scheduleId = Number(this.selectedBus.id);

    this.bitlaService.postTentativeBooking(scheduleId, bitlaRequest).subscribe({
      next: (bitlaRes: any) => {

        const pnrNumber =
          bitlaRes?.bitlaResponse?.result?.ticket_details?.pnr_number;

        if (!pnrNumber) {
          alert('PNR not generated');
          this.isBookingInProgress = false;
          return;
        }

        console.log('🎫 PNR GENERATED:', pnrNumber);

        /* ================= CONFIRM BOOKING ================= */

        this.bitlaService.postConfirmBooking(pnrNumber, {}).subscribe({
          next: (confirmRes: any) => {

            const ticketNo =
              confirmRes?.bitlaResponse?.result?.ticket_details?.ticket_number;

            if (!ticketNo) {
              alert('Ticket not generated');
              this.isBookingInProgress = false;
              return;
            }

            console.log('🎟 Ticket Generated:', ticketNo);

            // ✅ SAVE TO DB + NAVIGATE INSIDE THIS
            this.afterTicketSuccess(ticketNo, confirmRes);
          },
          error: () => {
            alert('Confirm booking failed. Seats expired.');
            this.isBookingInProgress = false;
          }
        });
      },
      error: () => {
        alert('Tentative booking failed');
        this.isBookingInProgress = false;
      }
    });
  }

  // afterTicketSuccess(ticketNo: string, confirmDetails: any) {

  //   const ticketDetails =
  //     confirmDetails?.bitlaResponse?.result?.ticket_details;

  //   if (!ticketDetails) {
  //     alert('Ticket details missing');
  //     this.isBookingInProgress = false;
  //     return;
  //   }

  //   const passengers = ticketDetails.passenger_details || [];

  //   const userId =
  //     Number(JSON.parse(localStorage.getItem('user') || '{}')?.UserId ?? 0);

  //   // 🔥 1️⃣ Create all save requests
  //   const requests = passengers.map((p: any, index: number) => {

  //     // ✅ SP-compatible payload
  //     const payload = {
  //       busBookingSeat: {
  //         flag: 'I',
  //         busBookingSeatID: 0,
  //         userID: userId,
  //         forSelf: true,
  //         isPrimary: index === 0,
  //         seatNo: p.seat_no,
  //         firstName: p.name,
  //         gender: p.gender,
  //         contactNo: p.mobile,
  //         busBookingDetailsId: Number(this.selectedBus.id),
  //         busOperatorId: ticketDetails.travel_id,
  //         journeyDate: new Date(ticketDetails.travel_date),
  //         createdBy: userId
  //       }
  //     };

  //     return this.http.post(
  //       'http://localhost:5086/api/BusBookingSeat/SaveBusBookingSeat',
  //       payload
  //     );
  //   });

  //   // 🔥 2️⃣ Execute all requests together
  //   forkJoin(requests).subscribe({
  //     next: () => {

  //       console.log('✅ All passengers saved successfully');

  //       this.resetAfterSave();

  //       // 🔥 3️⃣ Navigate ONCE after all saves
  // this.router.navigate(
  //   ['/payment'],
  //   {
  //     queryParams: {
  //       ticketNo: ticketNo,
  //       amount: ticketDetails.total_fare   // ✅ ADD THIS
  //     },
  //     state: {
  //       passengerData: passengers,
  //       fareDetails: ticketDetails,
  //       amount: ticketDetails.total_fare   // ✅ KEEP THIS
  //     }
  //   }
  // ); 
  //    },
  //     error: err => {
  //       console.error('❌ Passenger save failed', err);
  //       alert('Passenger save failed');
  //       this.isBookingInProgress = false;
  //     }
  //   });
  // }


  afterTicketSuccess(ticketNo: string, confirmDetails: any) {

    const ticketDetails =
      confirmDetails?.bitlaResponse?.result?.ticket_details;

    if (!ticketDetails) {
      alert('Ticket details missing');
      this.isBookingInProgress = false;
      return;
    }

    // ✅🔥 SAVE TICKET NUMBER (MOST IMPORTANT FIX)
    localStorage.setItem('TICKET_NUMBER', ticketNo);
    console.log('🎫 Ticket Number saved:', ticketNo);

    const passengers = ticketDetails.passenger_details || [];

    const userId =
      Number(JSON.parse(localStorage.getItem('user') || '{}')?.UserId ?? 0);

    // 🔥 1️⃣ Create all save requests
    const requests = passengers.map((p: any, index: number) => {

      const payload = {
        busBookingSeat: {
          flag: 'I',
          busBookingSeatID: 0,
          userID: userId,
          forSelf: true,
          isPrimary: index === 0,
          seatNo: p.seat_no,
          firstName: p.name,
          gender: p.gender,
          contactNo: p.mobile,
          busBookingDetailsId: Number(this.selectedBus.id),
          busOperatorId: ticketDetails.travel_id,
          journeyDate: new Date(ticketDetails.travel_date),
          createdBy: userId
        }
      };

      return this.http.post(
        'http://localhost:5086/api/BusBookingSeat/SaveBusBookingSeat',
        payload
      );
    });

    // 🔥 2️⃣ Execute all requests together
    forkJoin(requests).subscribe({
      next: () => {

        console.log('✅ All passengers saved successfully');

        this.resetAfterSave();

        // 🔥 3️⃣ Navigate to payment
        this.router.navigate(
          ['/payment'],
          {
            queryParams: {
              ticketNo: ticketNo,
              amount: ticketDetails.total_fare
            },
            state: {
              passengerData: passengers,
              fareDetails: ticketDetails,
              amount: ticketDetails.total_fare
            }
          }
        );
      },
      error: err => {
        console.error('❌ Passenger save failed', err);
        alert('Passenger save failed');
        this.isBookingInProgress = false;
      }
    });
  }



  resetAfterSave() {
    this.selectedSeats.clear();
    this.showPassengerModal = false;
    this.isBookingInProgress = false;
  }


  extractFare(bus: any): number {
    const fareStr = bus.o_fare_str || bus.fare_str;
    if (!fareStr) return 0;

    const firstFare = fareStr.split(',')[0];
    const price = firstFare.split(':')[1];

    return Number(price);
  }

  getCityPairs() {
    const localCityPairs = localStorage.getItem('localCityPairsList');

    if (localCityPairs) {
      this.cityPairList = JSON.parse(localCityPairs);
      console.log('Loaded city pairs from localStorage:', this.cityPairList.length);
    } else {
      this.bitlaService.getCityPairs().subscribe({
        next: (res: any) => {
          if (res && res.result) {
            this.cityPairList = res.result.slice(1).map((pair: any[]) => ({
              origin_id: pair[0],
              destination_id: pair[1],
              travel_ids: pair[2]
            }));
            localStorage.setItem('localCityPairsList', JSON.stringify(this.cityPairList));
            console.log('City pairs loaded from API:', this.cityPairList.length);
          }
        },
        error: (err) => console.error('Error fetching city pairs:', err)
      });
    }
  }
  getCities() {
    const localCityList = localStorage.getItem('localCityList');
    if (localCityList) {
      this.cityList = JSON.parse(localCityList);
      console.log('Loaded cities from localStorage:', this.cityList.length);
    } else {
      this.bitlaService.getCities().subscribe({
        next: (res: any) => {
          if (res && res.result) {
            this.cityList = res.result.slice(1).map((city: any[]) => ({
              id: city[0],
              name: city[1],
              origin_count: city[2],
              destination_count: city[3],
            }));
            localStorage.setItem('localCityList', JSON.stringify(this.cityList));
            console.log('Cities loaded from API:', this.cityList.length);
          }
        },
        error: (err) => console.error('Error fetching cities:', err)
      });
    }
  }

  onOriginChange(event: any) {
    const originId = Number(event.target.value);

    const validDestIds = this.cityPairList
      .filter(p => p.origin_id === originId)
      .map(p => p.destination_id);

    this.filteredDestinationCities = this.cityList.filter(city =>
      validDestIds.includes(city.id)
    );

    console.log("DEST DROPDOWN →", this.filteredDestinationCities);
  }

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }
  swapOriginDestination() {
    // 🔁 swap IDs
    const tempId = this.selectedOriginId;
    this.selectedOriginId = this.selectedDestinationId;
    this.selectedDestinationId = tempId;

    // 🔁 swap input text
    const tempText = this.originInputText;
    this.originInputText = this.destinationInputText;
    this.destinationInputText = tempText;

    // 🔁 update destination list based on new origin
    if (this.selectedOriginId) {
      const validDestIds = this.cityPairList
        .filter(p => p.origin_id === this.selectedOriginId)
        .map(p => p.destination_id);

      this.filteredDestinationCities = this.cityList.filter(c =>
        validDestIds.includes(c.id)
      );
    }

    // 🔥 auto-search again if date is selected
    if (this.selectedTravelDateISO) {
      this.searchSchedules();
    }
  }
  openSeatLayout(bus: any) {

    // 🔁 TOGGLE LOGIC
    // If same bus clicked again → close it
    if (this.selectedBus === bus) {
      this.selectedBus = null;
      this.lowerDeck = [];
      this.upperDeck = [];
      this.boardingPoints = [];
      this.droppingPoints = [];
      return;
    }

    // ✅ 1️⃣ SET SELECTED BUS (ONLY ONE AT A TIME)
    this.selectedBus = bus;
    console.log('🟢 Selected Bus:', bus);

    // ✅ 2️⃣ BUILD BOARDING & DROPPING POINTS
    this.buildBoardingDropping(bus);

    // ✅ 3️⃣ BASIC VALIDATION
    if (!this.selectedOriginId || !this.selectedDestinationId || !this.selectedTravelDateISO) {
      console.warn('⚠ Missing origin / destination / date');
      return;
    }

    // ✅ 4️⃣ LOAD SEAT AVAILABILITY
    this.bitlaService
      .getAvailabilities(
        this.selectedOriginId,
        this.selectedDestinationId,
        this.selectedTravelDateISO
      )
      .subscribe({
        next: (res: any) => {

          console.log('🟢 Availability API success');

          this.availabilityData = res;

          // build seat structure
          this.buildDecksFromAvailability(res);

          // mark available / blocked seats
          this.mapSeatAvailability(res);
        },
        error: (err) => {
          console.error('❌ Availability API failed', err);
        }
      });
  }



  buildDecksFromAvailability(res: any) {
    this.lowerDeck = [];
    this.upperDeck = [];

    const rows = res?.result || [];
    if (rows.length <= 1) {
      console.warn('No availability rows found');
      return;
    }

    const headers = rows[0];
    const data = rows.slice(1);

    const availableIndex = headers.indexOf('available');

    if (availableIndex === -1) {
      console.error('❌ available column not found');
      return;
    }

    // Take FIRST ROW (one schedule)
    const availableStr: string = data[0][availableIndex] || '';

    const seatCodes: string[] = availableStr
      .split(',')
      .map((s: string) => s.split('|')[0].trim())
      .filter((s: string) => s.length > 0);

    console.log('🪑 TOTAL SEATS:', seatCodes.length);
    console.log('🪑 SEAT CODES:', seatCodes);

    const lowerSeats = seatCodes.filter((s: string) => s.startsWith('L'));
    const upperSeats = seatCodes.filter((s: string) => s.startsWith('U'));

    const lowerRows = Math.ceil(lowerSeats.length / 3);
    const upperRows = Math.ceil(upperSeats.length / 3);

    this.lowerDeck = Array.from({ length: lowerRows });
    this.upperDeck = Array.from({ length: upperRows });

    console.log('⬇️ Lower seats:', lowerSeats.length, 'rows:', lowerRows);
    console.log('⬆️ Upper seats:', upperSeats.length, 'rows:', upperRows);
  }


  mapSeatAvailability(res: any) {
    this.bookedSeats.clear();
    this.selectedSeats.clear();

    const rows = res?.result || [];
    if (rows.length <= 1) return;

    const headers = rows[0];
    const data = rows.slice(1);

    const availableIndex = headers.indexOf('available');
    const availableSeatsIndex = headers.indexOf('available_seats');
    const routeIdIndex = headers.indexOf('route_id');
    const travelIdIndex = headers.indexOf('travel_id');

    // 🔥 FIND CORRECT ROW FOR CLICKED BUS
    const matchedRow = data.find((row: any[]) =>
      row[routeIdIndex] === this.selectedBus.route_id &&
      row[travelIdIndex] === this.selectedBus.travel_id
    );

    if (!matchedRow) {
      console.error('❌ No matching availability row found');
      return;
    }

    const availableStr: string = matchedRow[availableIndex] || '';
    const availableCount: number = matchedRow[availableSeatsIndex];

    console.log('✅ FINAL Available seats count:', availableCount);
    console.log('✅ FINAL Available seats string:', availableStr);

    // 🔥 NO SEATS → BLOCK ALL
    if (availableCount === 0) {
      this.blockAllSeats();
      return;
    }

    const availableSeatCodes: string[] = availableStr
      .split(',')
      .map((s: string) => s.split('|')[0].trim())
      .filter((s: string) => s.length > 0);

    this.blockSeatsNotInAvailableList(availableSeatCodes);
  }

  convertBitlaSeatToUIKey(code: string): string | null {
    // Example Bitla codes: L1, L2, U10

    if (!code) return null;

    const deck = code.startsWith('U') ? 'U' : 'L';
    const seatNumber = Number(code.substring(1)) - 1;

    const row = Math.floor(seatNumber / 3);
    const col = (seatNumber % 3) + 1;

    return `${deck}-${row}-${col}`;
  }
  getTotalSeats(bus: any): number {
    if (!bus.available) return 0;
    return bus.available.split(',').filter((x: string) => x.trim()).length;
  }

  getBookedSeatsCount(bus: any): number {
    const total = this.getTotalSeats(bus);
    return total - (bus.available_seats || 0);
  }

  buildDecksFromBus(bus: any) {
    this.lowerDeck = [];
    this.upperDeck = [];

    if (!bus.available) {
      console.warn('No seat structure found in bus.available');
      return;
    }

    const seatCodes: string[] = bus.available
      .split(',')
      .map((s: string) => s.split('|')[0].trim())
      .filter((s: string) => s.length > 0);

    console.log('🪑 TOTAL SEATS FROM API:', seatCodes.length);
    console.log('🪑 SEAT CODES:', seatCodes);

    const lowerSeats: string[] = seatCodes.filter((s: string) => s.startsWith('L'));
    const upperSeats: string[] = seatCodes.filter((s: string) => s.startsWith('U'));

    const lowerRows = Math.ceil(lowerSeats.length / 3);
    const upperRows = Math.ceil(upperSeats.length / 3);

    this.lowerDeck = Array.from({ length: lowerRows });
    this.upperDeck = Array.from({ length: upperRows });

    console.log('⬇️ Lower deck rows:', lowerRows, 'seats:', lowerSeats.length);
    console.log('⬆️ Upper deck rows:', upperRows, 'seats:', upperSeats.length);
  }


  blockAllSeats() {
    this.lowerDeck.forEach((_, rowIndex) => {
      for (let col = 1; col <= 3; col++) {
        this.bookedSeats.add(`L-${rowIndex}-${col}`);
      }
    });

    this.upperDeck.forEach((_, rowIndex) => {
      for (let col = 1; col <= 3; col++) {
        this.bookedSeats.add(`U-${rowIndex}-${col}`);
      }
    });
  }

  blockSeatsNotInAvailableList(availableSeatCodes: string[]) {
    const availableKeys = new Set(
      availableSeatCodes
        .map(code => this.convertBitlaSeatToUIKey(code))
        .filter(Boolean) as string[]
    );

    this.lowerDeck.forEach((_, rowIndex) => {
      for (let col = 1; col <= 3; col++) {
        const key = `L-${rowIndex}-${col}`;
        if (!availableKeys.has(key)) {
          this.bookedSeats.add(key);
        }
      }
    });

    this.upperDeck.forEach((_, rowIndex) => {
      for (let col = 1; col <= 3; col++) {
        const key = `U-${rowIndex}-${col}`;
        if (!availableKeys.has(key)) {
          this.bookedSeats.add(key);
        }
      }
    });
  }

  getTotalSeatsFromAvailable(bus: any): number {
    if (!bus.available) return 0;
    return bus.available
      .split(',')
      .map((s: string) => s.split('|')[0].trim())
      .filter((s: string) => s.length > 0).length;
  }

  onOriginKeyDown(event: KeyboardEvent) {
    if (!this.showOriginList) return;

    const max = this.filteredDepartureCities.length - 1;

    switch (event.key) {
      case 'ArrowDown':
        event.preventDefault();
        this.originActiveIndex =
          this.originActiveIndex < max ? this.originActiveIndex + 1 : 0;
        break;

      case 'ArrowUp':
        event.preventDefault();
        this.originActiveIndex =
          this.originActiveIndex > 0 ? this.originActiveIndex - 1 : max;
        break;

      case 'Enter':
        event.preventDefault();
        if (this.originActiveIndex >= 0) {
          this.selectOrigin(this.filteredDepartureCities[this.originActiveIndex]);
        }
        break;

      case 'Escape':
        this.showOriginList = false;
        this.originActiveIndex = -1;
        break;
    }
  }
  onDestinationKeyDown(event: KeyboardEvent) {
    if (!this.showDestinationList) return;

    const max = this.filteredDestinationCities.length - 1;

    switch (event.key) {
      case 'ArrowDown':
        event.preventDefault();
        this.destinationActiveIndex =
          this.destinationActiveIndex < max ? this.destinationActiveIndex + 1 : 0;
        break;

      case 'ArrowUp':
        event.preventDefault();
        this.destinationActiveIndex =
          this.destinationActiveIndex > 0 ? this.destinationActiveIndex - 1 : max;
        break;

      case 'Enter':
        event.preventDefault();
        if (this.destinationActiveIndex >= 0) {
          this.selectDestination(
            this.filteredDestinationCities[this.destinationActiveIndex]
          );
        }
        break;

      case 'Escape':
        this.showDestinationList = false;
        this.destinationActiveIndex = -1;
        break;
    }
  }
  buildBoardingDropping(bus: any) {
    this.boardingPoints = [];
    this.droppingPoints = [];

    // 🚌 BOARDING POINTS
    if (bus?.boarding_stages) {
      this.boardingPoints = bus.boarding_stages
        .split(',')
        .filter((x: string) => x.includes('|') && x.split('|')[0]) // 🔥 avoid empty stageId
        .map((item: string) => {
          const [stageId, time] = item.split('|');
          return {
            stageId: Number(stageId),
            name: this.stageNamesMap[stageId] || 'Unknown',
            time
          };
        });
    }

    // 🏁 DROPPING POINTS
    if (bus?.dropoff_stages) {
      this.droppingPoints = bus.dropoff_stages
        .split(',')
        .filter((x: string) => x.includes('|') && x.split('|')[0]) // 🔥 FIXED
        .map((item: string) => {
          const [stageId, time] = item.split('|');
          return {
            stageId: Number(stageId),
            name: this.stageNamesMap[stageId] || 'Unknown',
            time
          };
        });
    }

    // reset previous selections
    this.selectedBoardingPoint = null;
    this.selectedDroppingPoint = null;

    console.log('🚌 Boarding Points:', this.boardingPoints);
    console.log('🏁 Dropping Points:', this.droppingPoints);
  }
  openPassengerForm() {

    // 🔒 Boarding point validation
    if (!this.selectedBoardingPoint) {
      alert('Please select Boarding Point');
      return;
    }

    // 🔒 Dropping point validation
    // (If your working component allows empty drop, you can remove this check)
    if (!this.selectedDroppingPoint) {
      alert('Please select Dropping Point');
      return;
    }

    // 🔒 Seat validation
    if (this.selectedSeats.size === 0) {
      alert('Please select at least one seat');
      return;
    }

    // 🔥 IMPORTANT: clear old passenger form (avoid stale data)
    this.passengerForm?.reset();

    // 🔥 Build passenger form based on selected seats
    // (same pattern, but now guaranteed to exist before modal opens)
    this.buildPassengerForm();

    // 🔥 DEBUG (keep for now)
    console.log('🪑 Selected seats:', Array.from(this.selectedSeats));
    console.log('👤 Passenger form created:', this.passengerForm.value);

    // ✅ Finally open modal
    this.showPassengerModal = true;
  }

  closePassengerForm() {
    this.showPassengerModal = false;
  }


  openRechargeOnly() {
    this.isRechargeMode = true;
  }


  goToRechargePayment() {
    if (!this.rechargeAmount || this.rechargeAmount <= 0) {
      alert('Enter valid recharge amount');
      return;
    }

    this.router.navigate(
      ['/payment'],
      {
        state: {
          amount: this.rechargeAmount,
          paymentPurpose: 'RECHARGE'
        }
      }
    );
  }

}

