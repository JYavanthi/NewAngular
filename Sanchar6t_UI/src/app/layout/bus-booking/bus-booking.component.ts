
import { Component, HostListener, NgZone, Renderer2 } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpServiceService } from '../../services/http-service.service';
import { API_URLS } from '../../shared/API-URLs'
import { FormValidationService } from '../../services/form-validation.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiConverterService } from '../../services/api-converter.service';
import { HttpClient } from '@angular/common/http';
import { __values } from 'tslib';
import { UserServiceService } from '../../services/user-service.service';
import { BitlaService } from '../../services/bitla-service';


interface Seat {
  seatNumber: string;
  seatType: string;
  price: number;
  availability: boolean;
  ladies_seats?: string[];
  gents_seats?: string[];
  label?: string;
  isBlocked?: boolean;
}

interface Schedule {
  id: number;
  number: string;
  name: string;
  operator_service_name: string;
  origin_id: number;
  destination_id: number;
  dep_time: string;
  arr_time: string;
  duration: string;
  bus_types: string;
  show_fare_screen: string;
  amenitiesList: string[];
  boarding_stages: string[];
  dropoff_stages: string[];
  origin_name?: string;
  destination_name?: string;
  [key: string]: any;
}

interface Availability {
  route_id: number;
  schedule_id: string;
  available_seats: number;
  status: string;
  fare_str: string;
  updated_at: string;
  origin_id: number;
  destination_id: number;
  travel_id: number;
  available: string;
  ladies_seats: string;
  gents_seats: string;
  available_gst: string;
}

@Component({
  selector: 'app-bus-booking',
  templateUrl: './bus-booking.component.html',
  styleUrl: './bus-booking.component.scss',
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
    ]),
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateX(10px)' }),
        animate('300ms 0s', style({ opacity: 1, transform: 'translateX(0)' }))
      ]),
      transition(':leave', [
        style({ opacity: 1, transform: 'translateX(0)' }),
        animate('300ms 0s', style({ opacity: 0, transform: 'translateX(10px)' }))
      ])
    ])
  ]
})


export class BusBookingComponent {
  layoutMode: 'seater' | 'sleeper' | null = null;
  busBookingForm!: FormGroup;
  userId: string | null = null;
  selectedTab: number = 0;
  bloodGroups: string[] = ['A+', 'A-', 'B+', 'B-', 'AB+', 'AB-', 'O+', 'O-'];
  seatLegends: string[] = ['Available', 'Unavailable', 'Female', 'Male', 'Selected'];
  idCardList: string[] = ['Aadhar Card', 'PAN Card', 'D/L', 'Passport', 'Voter ID', 'Ration Card'];
  isBoardingDroppingDtlsOpen: boolean = false;
  isSeatLegendOpen: boolean = true;
  isPopupVisible: boolean = false;
  isCancellationPolicy: boolean = true;
  showChoosingProfile: boolean[] = [];
  showPassengerInputs: boolean[] = [];
  isMobile: boolean = false;
  stageList: any[] = [];
  cityList: any[] = [];
  cityPairList: any[] = [];
  operatorScheduleList: Schedule[] = [];
  // operatorOpenSections: { [key: number]: any } = {};
  isDetailsVisible = false;
  selectedBus: any = null;
  operatorViewSeat: { [scheduleId: string]: boolean } = {};
  filteredDepartureCities: any[] = [];
  filteredDestinationCities: any[] = [];
  originId!: number;
  destinationId!: number;
  travelDate: any
  dateOfDeparture: any;
  travelId: any;
  minDate: string = '';
  lowerDeckSeats: { seatNumber: number, available: boolean }[] = [];
  upperDeckSeats: { seatNumber: number, available: boolean }[] = [];
  seatNumber!: number | null;
  stage_names: { [id: string]: string } = {};
  selectedDepartureID: number | null = null;
  selecteddestinationId: number | null = null;
  filteredBoardingPoints: string[] = [];
  filteredDroppingPoints: string[] = [];
  allBoardingPoints: string[] = [];
  allDroppingPoints: string[] = [];
  savedPassengerDtls: any[] = [];
  selectedFilters: string[] = [];
  busTypeFilters: string[] = [];
  filteredSchedules: any = { departure: {}, arrival: {} };
  filteredBusTypes: any = { seater: [], sleeper: [], ac: [], nonAc: [] };
  masterAPI: any = null;
  scheduleData: any[] = [];
  availabelSeats: string[] = [];
  availabilitiesData: any[] = [];
  scheduleColumns: string[] = [];
  selectedSeats: string[] = [];
  schedulesList: any[] = [];
  filteredScheduleList: any[] = [];
  boardingOptions: string[] = [];
  droppingOptions: string[] = [];
  operatorOptions: string[] = [];
  sleeperLowerDeck: Seat[][] = [];
  sleeperUpperDeck: Seat[][] = [];
  seaterSeats: Seat[][] = [];
  parsedSeatLayout: Seat[][] = [];
  ladies_seats: string[] = [];
  gents_seats: string[] = [];
  isPassengerDtlsOpen: boolean = false;
  isUserLoggedIn: boolean = true;
  availabelList: Availability[] = [];
  tentativeBookingData: any = null;
  confirmedBookingData: any = null;
  bookingDetails: any = null;
  cancellableInfo: any = null;
  cancelBookingResponse: any = null;
  balanceData: any = null;
  loading = false;
  selectedSeatsMap: { [scheduleId: string]: string[] } = {};

  dropdownOpen: { [key: string]: boolean } = {
    boarding: false,
    dropping: false,
    operator: false,
  };

  selectedBoarding = new Set<string>();
  selectedDropping = new Set<string>();
  selectedOperator = new Set<string>();

  loadMasters(): void {
    this.bitlaService.getMasters().subscribe({
      next: (res: any) => {
        console.log('Master API Raw Response:', res);

        const result = res?.result || res;

        this.masterAPI = {
          seat_layout_identifiers: result?.seat_layout_identifiers || {},
          bus_types: this.parseBusTypes(result?.bus_types || []),
          bus_categories: this.parseBusCategories(result?.bus_categories || []),
          id_card_types: this.parseIdCardTypes(result?.id_card_types || []),
          coach_type_categories: this.parseCoachTypeCategories(result?.coach_type_categories || '{}'),
          destination_count: result?.destination_count || 0,
          boarding_stages_count: result?.boarding_stages_count || 0,
        };

        console.log(' Processed Master Data:', this.masterAPI);
      },
      error: (err) => {
        console.error(' Error fetching master data:', err);
      }
    });
  }

  loadOperators(): void {
    this.bitlaService.getOperators().subscribe({
      next: (res: any) => {
        console.log('Operators API Data:', res);

        if (res.result?.operators) {
          this.operatorPolicyList = res.result.operators;
        } else if (Array.isArray(res)) {
          this.operatorPolicyList = res;
        } else {
          this.operatorPolicyList = res.operators || [];
        }

        console.log('Processed operatorsList:', this.operatorPolicyList);
      },
      error: (err) => {
        console.error('Error fetching operators:', err);
      }
    });
  }


  parseBusTypes(busTypes: any[]): any[] {
    if (!Array.isArray(busTypes) || busTypes.length <= 1) return [];
    const [, ...dataRows] = busTypes;
    return dataRows.map(([id, name]) => ({ id, name }));
  }


  parseBusCategories(busCategories: any[]): any[] {
    if (!Array.isArray(busCategories) || busCategories.length <= 1) return [];
    const [, ...dataRows] = busCategories;
    return dataRows.map(([id, name]) => ({ id, name }));
  }


  parseIdCardTypes(idCards: any[]): any[] {
    if (!Array.isArray(idCards)) return [];
    return idCards.map(([name, id]) => ({ id, name }));
  }


  parseCoachTypeCategories(coachTypeCategories: string): any {
    try {
      return JSON.parse(coachTypeCategories);
    } catch {
      return {};
    }
  }


  async loadSchedules(originId: number, destinationId: number, travelDate: string) {
    this.resetTimeFiltersData();

    this.originId = originId;
    this.destinationId = destinationId;
    this.travelDate = travelDate;

    await this.getStages();

    this.bitlaService.getSchedules(originId, destinationId, travelDate).subscribe({
      next: (res: any) => {

        const stageNames = res.stage_names || {};

        this.schedulesList = this.processSchedules(res);

        this.schedulesList.forEach((item: any) => {
          item.boarding_stage_address = this.parseStageDataWithNames(item.boarding_stages);
          item.drop_off_address = this.parseStageDataWithNames(item.dropoff_stages);

          if (res?.result && Array.isArray(res.result)) {
            const matchingRow = res.result.find((row: any[]) => row[1] === item.schedule_id);
            if (matchingRow && matchingRow.length > 10) {
              item.show_fare_screen = matchingRow.includes('true') ? true : false;
            }
          }

          if (item.show_fare_screen === undefined) item.show_fare_screen = false;
        });

        this.filteredScheduleList = [...this.schedulesList];

        const boardingSet = new Set<string>();
        const droppingSet = new Set<string>();
        const operatorSet = new Set<string>();

        this.schedulesList.forEach((bus: any) => {
          (bus.boarding_stage_address || []).forEach((stage: string) => {
            const name = stage.split('|')[0];
            if (name) boardingSet.add(name.trim());
          });
          (bus.drop_off_address || []).forEach((stage: string) => {
            const name = stage.split('|')[0];
            if (name) droppingSet.add(name.trim());
          });
          if (bus.operator_service_name) operatorSet.add(bus.operator_service_name.trim());
        });

        this.boardingOptions = Array.from(boardingSet);
        this.droppingOptions = Array.from(droppingSet);
        this.operatorOptions = Array.from(operatorSet);

        this.getScheduleList();
        this.gettingBusTypes();

        console.log('Schedules reloaded successfully:', this.filteredScheduleList);
        console.log('Filters fully reset and repopulated.');
      },
      error: (err) => console.error('Error fetching schedules:', err)
    });
  }

  resetTimeFiltersData() {
    this.filteredSchedules = {
      departure: {
        before6am: [],
        '6amTo12pm': [],
        '12pmTo6pm': [],
        after6pm: []
      },
      arrival: {
        before6am: [],
        '6amTo12pm': [],
        '12pmTo6pm': [],
        after6pm: []
      }
    };

    Object.keys(this.busBookingForm.controls).forEach(key => {
      if (key.startsWith('departureTime') || key.startsWith('arrivalTime')) {
        this.busBookingForm.get(key)?.setValue(false, { emitEvent: false });
      }
    });

    this.filteredScheduleList = [];
  }


  parseStageDataWithNames(stageData: string): string[] {
    if (!stageData) return [];

    const stages = stageData.split(',').map((s: string) => s.trim()).filter(Boolean);
    const mappedStages: string[] = [];

    stages.forEach((stageStr: string) => {
      const parts = stageStr.split('|');
      const stageId = Number(parts[0]);

      const matchedStage = this.stageList.find(s => s.id === stageId || s.city_id === stageId);

      const stageName = matchedStage
        ? `${matchedStage.name} (${matchedStage.city_name})`
        : `Stage ${stageId}`;

      const time = parts[1] || '';
      const extra1 = parts[2] || '';
      const extra2 = parts[3] || '';
      const extra3 = parts[4] || '';

      mappedStages.push(`${stageName}|${time}|${extra1}|${extra2}|${extra3}`);
    });

    return mappedStages;
  }


  loadSingleSchedule(scheduleId: string): void {
    if (!scheduleId) {
      return;
    }
    this.selectedScheduleId = Number(scheduleId);
    this.bitlaService.getSchedule(scheduleId).subscribe({
      next: (res: any) => {
        this.selectedBus = res?.result || res;

        const availability = this.availabelList.find(
          (a) => String(a.schedule_id) === String(scheduleId)
        );

        if (availability) {
          console.log('Matched availability:', availability);
        } else {
          console.warn('No seat availability found for schedule:', scheduleId);
        }

        this.splitSeats();

        console.log('Seat layout built for Schedule ID:', this.selectedScheduleId);
      },
      error: (err) => {
        console.error('Error fetching schedule for ID:', scheduleId, err);
      },
    });
  }


  loadOperatorSchedules(travelId: string, travelDate: string): void {
    this.bitlaService.getOperatorSchedules(travelId, travelDate).subscribe({
      next: (res: any) => {
        console.log('Operator schedules fetched successfully:', res);

        const stageNames = res.stage_names || {};
        this.schedulesList = this.processSchedules(res);

        if (!this.schedulesList?.length) {
          console.warn('No valid schedules found for this travelId and date');
          return;
        }
        this.schedulesList.forEach((item: any) => {
          item.boarding_stage_address = this.parseStageData(item.boarding_stages, stageNames);
          item.drop_off_address = this.parseStageData(item.dropoff_stages, stageNames);

          item.operatorPolicyList = item.operatorPolicyList?.map((policy: any) => ({
            ...policy,
            cancellationDetails: policy.cancellation_policies
              ? policy.cancellation_policies.split(',').map((entry: string) => {
                const [time, charge] = entry.split('|');
                return { cancellation_time: time, cancellation_charges: charge };
              })
              : [],
            rescheduleDetails: policy.reschedule_policies
              ? policy.reschedule_policies.split(',').map((entry: string) => {
                const [time, charge] = entry.split('|');
                return { reschedule_time: time, reschedule_charges: charge };
              })
              : []
          })) || [];
        });

        this.filteredScheduleList = [...this.schedulesList];

        const boardingSet = new Set<string>();
        const droppingSet = new Set<string>();
        const operatorSet = new Set<string>();

        this.schedulesList.forEach((bus: any) => {
          (bus.boarding_stage_address || []).forEach((stage: string) => {
            const [name] = stage.split('|');
            if (name) boardingSet.add(name.trim());
          });

          (bus.drop_off_address || []).forEach((stage: string) => {
            const [name] = stage.split('|');
            if (name) droppingSet.add(name.trim());
          });

          if (bus.operator_service_name) operatorSet.add(bus.operator_service_name.trim());
        });

        this.boardingOptions = Array.from(boardingSet);
        this.droppingOptions = Array.from(droppingSet);
        this.operatorOptions = Array.from(operatorSet);

        this.getScheduleList();
        this.gettingBusTypes();
      },

      error: (err) => {
        console.error('Error fetching operator schedules:', err);
      },
    });
  }

  isTicketCancellable: boolean = false;
  cancellableDetails: any = null;
  operatorPolicyList: any[] = [];

  checkBalance(travelId: string) {
    if (!travelId) {
      console.error('Travel ID is required');
      return;
    }

    this.bitlaService.getBalance(travelId).subscribe({
      next: (res) => {
        console.log('Balance Data:', res);
        this.balanceData = res;
      },
      error: (err) => {
        console.error('Error fetching balance:', err);
        this.balanceData = null;
      }
    });
  }

  loadSeatAvailability(scheduleId: string): void {
    if (!this.originId || !this.destinationId || !this.travelDate) {
      console.warn(' Missing originId, destinationId, or travelDate — please check loadSchedules()');
      return;
    }

    console.log(' Fetching seat availabilities for:', {
      origin: this.originId,
      destination: this.destinationId,
      travelDate: this.travelDate
    });

    this.bitlaService.getAvailabilities(this.originId, this.destinationId, this.travelDate).subscribe({
      next: (res: any) => {
        console.log(' Availabilities fetched:', res);

        if (res?.result && res.result.length > 1) {
          this.availabelList = res.result.slice(1).map((row: any[]) => ({
            route_id: row[0],
            schedule_id: row[1],
            available_seats: row[2],
            status: row[3],
            fare_str: row[4],
            updated_at: row[5],
            origin_id: row[6],
            destination_id: row[7],
            travel_id: row[8],
            available: row[9],
            ladies_seats: row[10]?.split(',').map((s: string) => s.trim()).filter(Boolean) || [],
            gents_seats: row[11]?.split(',').map((s: string) => s.trim()).filter(Boolean) || [],
            available_gst: row[12]
          }));

          console.log('Parsed availabelList:', this.availabelList);
        } else {
          console.warn(' No availability data found');
          this.availabelList = [];
        }

        this.loadSingleSchedule(scheduleId);
      },
      error: (err) => {
        console.error(' Error fetching availabilities:', err);
      }
    });
  }

  loadAvailabilityBySchedule(scheduleId: number): void {
    console.log('Loading availability for schedule:', scheduleId);

    this.bitlaService.getAvailabilityBySchedule(scheduleId).subscribe({
      next: (res: any) => {
        console.log('Schedule availability fetched:', res);

        const result = res?.result;
        if (result && result.length > 1) {
          this.scheduleColumns = result[0];
          this.scheduleData = result.slice(1).map((row: any[]) => {
            const obj: any = {};
            this.scheduleColumns.forEach((col, idx) => (obj[col] = row[idx]));
            return obj;
          });

          this.ladies_seats = this.scheduleData
            .filter((seat) => seat['Gender']?.toLowerCase() === 'female' && seat['Availability'] === 'Available')
            .map((seat) => seat['SeatNo']);

          this.gents_seats = this.scheduleData
            .filter((seat) => seat['Gender']?.toLowerCase() === 'male' && seat['Availability'] === 'Available')
            .map((seat) => seat['SeatNo']);

          this.selectedSeats = this.selectedSeatsMap[scheduleId] || [];

        } else {
          this.scheduleColumns = [];
          this.scheduleData = [];
          this.ladies_seats = [];
          this.gents_seats = [];
          console.warn('No schedule availability found.');
        }

        console.log('Processed seats:', {
          total: this.scheduleData.length,
          ladies: this.ladies_seats,
          gents: this.gents_seats,
          storedSelected: this.selectedSeats
        });
      },
      error: (err) => console.error('Error fetching schedule availability:', err)
    });
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

  processSchedules(apiData: any): any[] {
    if (!apiData?.result || apiData.result.length < 2) return [];
    const header = apiData.result[0];
    const dataRows = apiData.result.slice(1);

    return dataRows.map((row: any[]) => {
      const obj: any = {};
      header.forEach((key: string, idx: number) => obj[key] = row[idx]);

      const departure = this.busBookingForm.value.departure;
      const destination = this.busBookingForm.value.destination;

      obj.origin_name = departure?.name || '';
      obj.destination_name = destination?.name || '';


      return obj;
    });
  }

  parseStageData(stageString: any, stageNames: any): string[] {
    if (!stageString) return [];

    if (typeof stageString === 'string' && stageString.trim().startsWith('[')) {
      try { stageString = JSON.parse(stageString); } catch { return []; }
    }

    if (typeof stageString === 'string') {
      stageString = stageString.split(',');
    }

    if (Array.isArray(stageString)) {
      return stageString.map(stage => {
        const [id, time] = stage.split('|');
        const name = stageNames[id] || `Stage ${id}`;
        return `${name}|${time}`;
      }).filter(Boolean);
    }

    return [];
  }

  constructor(private fb: FormBuilder, private ngZone: NgZone, private route: ActivatedRoute, private httpService: HttpServiceService, private apiConverterService: ApiConverterService, private bitlaService: BitlaService, private userService: UserServiceService, private httpc: HttpClient, private router: Router, private renderer: Renderer2, public apiConverter: ApiConverterService, private http: HttpServiceService, private formValidation: FormValidationService, private aRoute: ActivatedRoute) {
    this.busBookingForm = this.fb.group({
      boardingPoints: [''],
      droppingPoints: [''],
      operators: [''],
    });
  }

  ngAfterViewInit(): void {
    fetch('/assets/SVG Icons/home-icons.html')
      .then(response => response.text())
      .then(svgData => {
        const div = this.renderer.createElement('div');
        div.innerHTML = svgData;
        document.body.appendChild(div);
      });
  }


  ngOnInit() {
    const today = new Date();
    this.minDate = today.toISOString().split('T')[0];

    console.log('mindate', this.minDate)

    this.busBookingForm.patchValue({
      dateOfDeparture: this.minDate
    });

    this.formInit();
    this.checkScreenSize();
    this.loadMasters();
    this.loadOperators();
    this.getCities();
    this.getCityPairs();
    this.getStages();
    this.getScheduleList();

    this.route.queryParams.subscribe(params => {
      const originId = +params['origin_id'];
      const destinationId = +params['destination_id'];
      const dateOfDeparture = params['DOD'];

      if (originId && destinationId && dateOfDeparture) {
        const checkCitiesLoaded = setInterval(() => {
          if (this.cityList && this.cityList.length > 0) {
            clearInterval(checkCitiesLoaded);

            const originCity = this.cityList.find((c: { id: number; name: string }) => c.id === originId);
            const destCity = this.cityList.find((c: { id: number; name: string }) => c.id === destinationId);

            this.busBookingForm.patchValue({
              departure: originCity ? originCity.name : '',
              destination: destCity ? destCity.name : '',
              dateOfDeparture: dateOfDeparture
            });

            this.loadSchedules(originId, destinationId, dateOfDeparture);
          }
        }, 100);
      }
    });

    this.getSavedPassengerDtls();
    this.assignPassengerDtls();
    this.getOperatorPolicy();
  }





  getUniqueOperators() {
    const seen = new Set();
    return this.filteredScheduleList.filter(bus => {
      const name = bus.operator_service_name;
      if (seen.has(name)) return false;
      seen.add(name);
      return true;
    });
  }


  togglePassengerInputs(index: number): void {
    this.showPassengerInputs[index] = !this.showPassengerInputs[index];
  }

  toggleChoosingProfile(index: number): void {
    this.showChoosingProfile[index] = !this.showChoosingProfile[index];
  }

  getOperatorPolicy() {
    if (this.operatorPolicyList.length > 0) {
      this.operatorPolicyList = this.operatorPolicyList.map(policy => ({
        ...policy,
        cancellationDetails: policy.cancellation_policies ? policy.cancellation_policies.split(',').map((entry: any) => {
          const [time, charge] = entry.split('|'); return { cancellation_time: time, cancellation_charges: charge };
        }) : [],
        rescheduleDetails: policy.reschedule_policies ? policy.reschedule_policies.split(',').map((entry: any) => {
          const [time, charge] = entry.split('|'); return { reschedule_time: time, reschedule_charges: charge };
        }) : []
      }
      ));
    }
  }

  getScheduleList() {
    if (!this.schedulesList?.length) return;
    const processedSchedules = this.schedulesList.map(s => {
      const originCity = this.cityList.find(city => city.id === s.origin_id);
      const destinationCity = this.cityList.find(city => city.id === s.destination_id);

      const parseAmenities = (data: any): any[] => {
        if (!data) return [];
        if (Array.isArray(data)) return data;
        if (typeof data === 'string') {
          try {
            return JSON.parse(data);
          } catch {
            console.warn('Invalid amenities JSON:', data);
            return [];
          }
        }
        return [];
      };

      return {
        ...s,
        origin_name: originCity?.name || '',
        destination_name: destinationCity?.name || '',
        amenitiesList: parseAmenities(s.amenities),
        boarding_stage_address: s.boarding_stage_address || [],
        drop_off_address: s.drop_off_address || []
      };
    });

    this.schedulesList = processedSchedules;

    this.filteredScheduleList = [...processedSchedules];

    const firstSchedule = processedSchedules[0];
    if (firstSchedule) {
      const originNameItem = this.cityList.find(item => item.id === Number(firstSchedule.origin_id));
      const destinationNameItem = this.cityList.find(item => item.id === Number(firstSchedule.destination_id));

      this.busBookingForm.patchValue({
        departure: originNameItem?.name || '',
        destination: destinationNameItem?.name || '',
        dateOfDeparture: firstSchedule.travel_date || ''
      });
    }

    this.gettingBusByTime();
    this.gettingBusTypes();

    console.log(' Processed scheduleList:', this.schedulesList);
  }

  // onBoardingDroppingChange() {
  //   const boardingPoint = this.busBookingForm.get('boardingPoint')?.value?.toLowerCase() || '';
  //   const droppingPoint = this.busBookingForm.get('droppingPoint')?.value?.toLowerCase() || '';
  //   const operator = this.busBookingForm.get('operator')?.value?.toLowerCase() || '';

  //   this.filteredScheduleList = this.schedulesList.filter(bus => {
  //     const boardingStages = Array.isArray(bus.boarding_stages)
  //       ? bus.boarding_stages
  //       : typeof bus.boarding_stages === 'string'
  //         ? bus.boarding_stages.split(',').map((s: string) => s.trim())
  //         : [];

  //     const droppingStages = Array.isArray(bus.dropoff_stages)
  //       ? bus.dropoff_stages
  //       : typeof bus.dropoff_stages === 'string'
  //         ? bus.dropoff_stages.split(',').map((s: string) => s.trim())
  //         : [];

  //     const boardingLocations = boardingStages.map((stage: string) => {
  //       const [id, time] = stage.split('|');
  //       return id?.toLowerCase() || '';
  //     });

  //     const droppingLocations = droppingStages.map((stage: string) => {
  //       const [id, time] = stage.split('|');
  //       return id?.toLowerCase() || '';
  //     });

  //     return (
  //       (!boardingPoint || boardingLocations.some((loc: string) => loc.includes(boardingPoint))) &&
  //       (!droppingPoint || droppingLocations.some((loc: string) => loc.includes(droppingPoint))) &&
  //       (!operator || bus.operator_service_name?.toLowerCase()?.includes(operator))
  //     );
  //   });

  //   if (!boardingPoint && !droppingPoint && !operator) {
  //     this.filteredScheduleList = [...this.schedulesList];
  //   }
  // }


  toggleDropdown(type: string) {
    this.dropdownOpen[type] = !this.dropdownOpen[type];
  }

  onOptionToggle(type: string, option: string, event: any) {
    let set =
      type === 'boarding' ? this.selectedBoarding :
        type === 'dropping' ? this.selectedDropping :
          this.selectedOperator;

    if (event.target.checked) {
      set.add(option);
    } else {
      set.delete(option);
    }
    this.applyFilter();
  }

  applyFilter() {
    this.filteredScheduleList = this.schedulesList.filter(bus => {
      const boardingMatch = [...this.selectedBoarding].length === 0 ||
        bus.boarding_stage_address?.some((stage: string) =>
          this.selectedBoarding.has(stage.split('|')[0])
        );

      const droppingMatch = [...this.selectedDropping].length === 0 ||
        bus.drop_off_address?.some((stage: string) =>
          this.selectedDropping.has(stage.split('|')[0])
        );

      const operatorMatch = [...this.selectedOperator].length === 0 ||
        this.selectedOperator.has(bus.operator_service_name);

      return boardingMatch && droppingMatch && operatorMatch;
    });
  }


  gettingBusTypes() {
    const types = ['seater', 'sleeper', 'ac', 'non-ac'];
    types.forEach(type => {
      this.filteredBusTypes[type.replace('-', '')] = this.schedulesList.filter(bus =>
        bus.bus_type.toLowerCase().split(',').map((t: string) => t.trim()).includes(type)
      );
    });
  }

  populateFilteredSchedules() {
    const timeRanges: Record<string, [number, number]> = {
      before6am: [0, 360],
      '6amTo12pm': [360, 720],
      '12pmTo6pm': [720, 1080],
      after6pm: [1080, 1440]
    };

    ['departure', 'arrival'].forEach(timeType => {
      Object.keys(timeRanges).forEach(key => {
        this.filteredSchedules[timeType][key] = this.schedulesList.filter(schedule => {
          const time = schedule[timeType === 'departure' ? 'dep_time' : 'arr_time'];
          const [h, m] = time.split(':').map(Number);
          const minutes = h * 60 + m;
          return minutes >= timeRanges[key][0] && minutes < timeRanges[key][1];
        });
      });
    });
  }

  onBusTypeChange(controlName: string) {
    const isChecked = this.busBookingForm.get(controlName)?.value;
    this.filteredScheduleList = [];

    const selectedBusTypes = ['busTypeSeater', 'busTypeSleeper', 'busTypeAC', 'busTypeNonAC']
      .filter(k => this.busBookingForm.get(k)?.value);

    selectedBusTypes.forEach(k => {
      const type = k.replace('busType', '').toLowerCase();
      this.filteredScheduleList.push(...this.filteredBusTypes[type]);
    });

    if (selectedBusTypes.length === 0) this.filteredScheduleList = [...this.schedulesList];
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.checkScreenSize();
  }

  checkScreenSize() {
    this.isMobile = window.innerWidth <= 574;
    if (this.isMobile) {
      this.isDetailsVisible = false;
    } else {
      this.isDetailsVisible = true;
    }
  }

  operatorOpenSections: any = {};

  togglePolicy(isCancellation: boolean) {
    this.isCancellationPolicy = isCancellation;
  }

  togglePopup() {
    this.isPopupVisible = !this.isPopupVisible;
  }

  toggleDetails() {
    this.isDetailsVisible = !this.isDetailsVisible;
  }

  toggleSection(operatorId: string, section: string) {
    if (!this.operatorOpenSections[operatorId]) {
      this.operatorOpenSections[operatorId] = {
        amenities: false,
        boardingDroppingPoints: false,
        bookingPolicies: false
      };
    }

    const sectionMap: { [key: string]: string } = {
      'Amenities': 'amenities',
      'BoardingDroppingPoints': 'boardingDroppingPoints',
      'BookingPolicies': 'bookingPolicies'
    };
    const key = sectionMap[section];
    const currentState = this.operatorOpenSections[operatorId];

    this.operatorOpenSections[operatorId] = {
      amenities: false,
      boardingDroppingPoints: false,
      bookingPolicies: false
    };

    if (!(currentState as any)[key]) {
      (this.operatorOpenSections[operatorId] as any)[key] = true;
    }
  }

  splitSeats(): void {
    if (!this.selectedBus || !this.selectedBus.bus_layout) return;

    const seatLayoutIdentifiers = this.selectedBus.bus_layout?.seat_layout_identifiers || {};
    const availableData: string = this.selectedBus.bus_layout.available || '';
    const scheduleAvailability = this.availabelList.find(a => a.schedule_id === this.selectedBus.schedule_id);
    const availabilitySeats: string[] = scheduleAvailability?.available_seats
      ? String(scheduleAvailability.available_seats).split(',').map(s => s.trim())
      : [];

    const ladiesSeats: string[] = Array.isArray(scheduleAvailability?.ladies_seats)
      ? scheduleAvailability!.ladies_seats
      : (scheduleAvailability?.ladies_seats
        ? String(scheduleAvailability.ladies_seats).split(',').map(s => s.trim())
        : []);

    const gentsSeats: string[] = Array.isArray(scheduleAvailability?.gents_seats)
      ? scheduleAvailability!.gents_seats
      : (scheduleAvailability?.gents_seats
        ? String(scheduleAvailability.gents_seats).split(',').map(s => s.trim())
        : []);

    const busType = (this.selectedBus?.bus_type || '').toLowerCase();
    const isSleeper = busType.includes('sleeper');

    console.log('Bus Type detected:', this.selectedBus?.bus_type);
    console.log('Layout Mode:', isSleeper ? 'Sleeper' : 'Seater');

    this.sleeperLowerDeck = [];
    this.sleeperUpperDeck = [];
    this.seaterSeats = [];

    const lowerSeats: Seat[] = [];
    const upperSeats: Seat[] = [];
    const seaterSeats: Seat[] = [];

    const seatFareMap = new Map<string, number>();
    availableData.split(',').forEach((seatInfo: string) => {
      if (seatInfo) {
        const [seatNumber, fare] = seatInfo.split('|');
        if (seatNumber) seatFareMap.set(seatNumber.trim(), fare ? parseFloat(fare) : 0);
      }
    });

    seatFareMap.forEach((fare, seatNumber) => {
      const isAvailable = availabilitySeats.includes(seatNumber);
      const displaySeatType = seatLayoutIdentifiers[seatNumber] || seatNumber;

      const seatObj: Seat = {
        seatNumber,
        seatType: displaySeatType,
        price: fare,
        availability: isAvailable,
        ladies_seats: ladiesSeats,
        gents_seats: gentsSeats
      };

      if (isSleeper) {
        if (seatNumber.startsWith('L')) lowerSeats.push(seatObj);
        else if (seatNumber.startsWith('U')) upperSeats.push(seatObj);
        else lowerSeats.push(seatObj);
      } else {
        seaterSeats.push(seatObj);
      }
    });

    const groupIntoRows = (arr: Seat[]): Seat[][] => {
      const seatsPerRow = 5;
      const rows: Seat[][] = [];
      for (let i = 0; i < arr.length; i += seatsPerRow) {
        rows.push(arr.slice(i, i + seatsPerRow));
      }
      return rows;
    };

    if (isSleeper) {
      this.sleeperLowerDeck = groupIntoRows(lowerSeats);
      this.sleeperUpperDeck = groupIntoRows(upperSeats);
      this.seaterSeats = [];
    } else {
      this.seaterSeats = groupIntoRows(seaterSeats);
      this.sleeperLowerDeck = [];
      this.sleeperUpperDeck = [];
    }

    this.layoutMode = isSleeper ? 'sleeper' : 'seater';
    console.log('Final Layout Mode Set To:', this.layoutMode);

    console.log(' Total Seats Found:', seatFareMap.size);
  }

  toggleSeat(seat: Seat): void {
    const index = this.selectedSeats.indexOf(seat.seatNumber);

    if (index > -1) {
      this.selectedSeats.splice(index, 1);
    }
    else {
      this.selectedSeats.push(seat.seatNumber);
    }

    this.applyBlockSeatColor();
  }

  applyBlockSeatColor() {
    const allSeats = [
      ...this.sleeperLowerDeck.flat(),
      ...this.sleeperUpperDeck.flat(),
      ...this.seaterSeats.flat()
    ];

    allSeats.forEach(seat => {
      seat['isBlocked'] = false;
    });
  }



  isSelected(seat: Seat): boolean {
    return this.selectedSeats.includes(seat.seatNumber);
  }

  getTooltipText(seat: Seat): string {
    const identifiers = this.masterAPI?.seat_layout_identifiers || {};
    const displayName = identifiers[seat.seatNumber] || seat.seatNumber;

    const genderLabel = seat.ladies_seats?.includes(seat.seatNumber)
      ? ' (Ladies Seat)'
      : seat.gents_seats?.includes(seat.seatNumber)
        ? ' (Gents Seat)'
        : '';

    return `${displayName}${genderLabel} — ₹${seat.price.toFixed(2)}`;
  }

  getTotalAmount(): number {
    const allSeats = [...this.sleeperLowerDeck.flat(), ...this.sleeperUpperDeck.flat(), ...this.seaterSeats.flat()];
    return this.selectedSeats.reduce((total, seatNumber) => {
      const seat = allSeats.find(s => s.seatNumber === seatNumber);
      return total + (seat ? seat.price : 0);
    }, 0);
  }

  viewSeat(scheduleId: string): void {
    if (this.operatorViewSeat[scheduleId]) {
      this.operatorViewSeat[scheduleId] = false;
      this.selectedBus = null;
      this.sleeperLowerDeck = [];
      this.sleeperUpperDeck = [];
      this.seaterSeats = [];
      return;
    }

    Object.keys(this.operatorViewSeat).forEach(id => {
      this.operatorViewSeat[id] = false;
    });

    this.operatorViewSeat[scheduleId] = true;

    const selected = this.filteredScheduleList.find((bus: any) => bus.id === scheduleId);
    if (selected) {
      this.selectedBus = selected;
      console.log('Selected Bus:', this.selectedBus);
      this.loadSeatAvailability(scheduleId);
    } else {
      console.warn(' No matching bus found for', scheduleId);
    }
  }


  onRadioChange(val: string, point: string) {
    if (point == "boardingPoint") {
      this.selectedTab = 1;
      this.busBookingForm.controls['boardingPointRadio'].patchValue(val);
    }
    if (point == "droppingPoint") {
      this.busBookingForm.controls['droppingPointRadio'].patchValue(val);
    }
    if (this.busBookingForm.controls['boardingPointRadio'].value &&
      this.busBookingForm.controls['droppingPointRadio'].value) {
      this.isBoardingDroppingDtlsOpen = true;
    }
  }

  proceedToBook() {
    if (this.isUserLoggedIn) {
      this.isPassengerDtlsOpen = true;

      this.getSavedPassengerDtls();
    }
  }

  resetFilters() {

    this.busBookingForm.patchValue({
      liveTracking: false,

      departureTimebefore6am: false,
      departureTime6amTo12pm: false,
      departureTime12pmTo6pm: false,
      departureTimeafter6pm: false,

      arrivalTimebefore6am: false,
      arrivalTime6amTo12pm: false,
      arrivalTime12pmTo6pm: false,
      arrivalTimeafter6pm: false,

      busTypeSeater: false,
      busTypeSleeper: false,
      busTypeAC: false,
      busTypeNonAC: false,

      boardingPoints: '',
      droppingPoints: '',
      operators: ''
    });

    this.selectedBoarding.clear();
    this.selectedDropping.clear();
    this.selectedOperator.clear();

    this.dropdownOpen = {
      boarding: false,
      dropping: false,
      operator: false
    };

  }


  formInit() {
    const timeKeys = ['before6am', '6amTo12pm', '12pmTo6pm', 'after6pm'];
    const busTypes = ['Seater', 'Sleeper', 'AC', 'NonAC'];
    const group: any = {
      dateOfDeparture: [new Date().toISOString().split('T')[0], Validators.required],
      departure: [null, Validators.required],
      destination: [null, Validators.required],
      liveTracking: [''],
      departureTimeBefore6am: [false],
      departureTime6amTo12pm: [false],
      departureTime12pmTo6pm: [false],
      departureTimeAfter6pm: [false],
      busTypeSeater: [false],
      busTypeSleeper: [false],
      busTypeAC: [false],
      busTypeNonAC: [false],
      arrivalTimeBefore6am: [false],
      arrivalTime6amTo12pm: [false],
      arrivalTime12pmTo6pm: [false],
      arrivalTimeAfter6pm: [false],
      boardingPoint: [''],
      droppingPoint: [''],
      operator: [''],
      boardingPointRadio: [''],
      droppingPointRadio: [''],
      passengerNameCheckbox: [false],
      passengerDetails: this.fb.array([this.createPassengerGroup()]),
      savePassengerDetails: [false],
      toggleGST: [false],
      gstNo: ['', Validators.required],
      registeredCompanyName: ['', Validators.required],
      passengerName: ['', Validators.required],
      passengerAge: ['', [Validators.required, Validators.pattern('^[0-9]{1,2}$')]],
      passengerPhoneNo: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      passengerGender: ['', Validators.required],
      passengerIDCard: ['', Validators.required],
      passengerAlternativeNo: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      passengerEmail: ['', [Validators.required, Validators.email]],
      passengerAddress: ['', Validators.required],
      passengerRemarks: ['', Validators.required]
    };

    timeKeys.forEach(k => {
      group['departureTime' + k] = [false];
      group['arrivalTime' + k] = [false];
    });

    busTypes.forEach(b => {
      group['busType' + b] = [false];
    });

    this.busBookingForm = this.fb.group(group);

    this.busBookingForm.get('toggleGST')?.valueChanges.subscribe(value => {
      if (value) {
        this.busBookingForm.get('gstNo')?.setValidators([Validators.required]);
        this.busBookingForm.get('registeredCompanyName')?.setValidators([Validators.required]);
      } else {
        this.busBookingForm.get('gstNo')?.clearValidators();
        this.busBookingForm.get('registeredCompanyName')?.clearValidators();
      }
      this.busBookingForm.get('gstNo')?.updateValueAndValidity();
      this.busBookingForm.get('registeredCompanyName')?.updateValueAndValidity();
    });

    this.updatePassengerList();
  }


  get passengerDetails(): FormArray<FormGroup> {
    return this.busBookingForm.get('passengerDetails') as FormArray<FormGroup>;
  }

  updatePassengerList() {
    const passengerArray = this.passengerDetails;
    passengerArray.clear();
    this.selectedSeats.forEach(() => {
      passengerArray.push(this.createPassengerGroup());
    });
  }

  createPassengerGroup(): FormGroup {
    return this.fb.group({
      passengerGender: ['', Validators.required],
      passengerName: ['', Validators.required],
      passengerAge: ['', [Validators.required, Validators.pattern('^[0-9]{1,2}$')]],
      passengerPhoneNo: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      passengerIsWhatsApp: [false],
      passengerAddress: ['', Validators.required],
      passengerIDCard: ['', Validators.required],
      passengerIDCardNo: ['', Validators.required]
    });
  }


  copyFirstPassengerDtls(event: any) {
    const isChecked = event.target.checked;
    const formArray = this.passengerDetails;
    if (formArray.length < 2) return;
    const firstPassenger = formArray.at(0).value;
    formArray.controls.slice(1).forEach(control => {
      if (isChecked) {
        control.patchValue(firstPassenger);
      } else {
        control.patchValue({
          passengerGender: '',
          passengerName: '',
          passengerAge: null,
          passengerPhoneNo: null,
          passengerIsWhatsApp: false,
          passengerAddress: '',
          passengerIDCard: '',
          passengerIDCardNo: ''
        })
      }
    });
  }

  gettingBusByTime() {
    const timeRanges = {
      before6am: { start: 0, end: 6 * 60 },
      '6amTo12pm': { start: 6 * 60, end: 12 * 60 },
      '12pmTo6pm': { start: 12 * 60, end: 18 * 60 },
      after6pm: { start: 18 * 60, end: 24 * 60 }
    };

    const filterSchedulesByTimeRange = (timeRange: { start: number, end: number }, timeType: 'dep_time' | 'arr_time') => {
      return this.schedulesList.filter(schedule => {
        const timeInMinutes = (time: string) => {
          const [hour, minute] = time.split(':').map(Number);
          return hour * 60 + minute;
        };

        const timeInRange = timeInMinutes(schedule[timeType]);
        return timeInRange >= timeRange.start && timeInRange < timeRange.end;
      });
    };

    this.filteredSchedules.departure.before6am = filterSchedulesByTimeRange(timeRanges.before6am, 'dep_time');
    this.filteredSchedules.departure['6amTo12pm'] = filterSchedulesByTimeRange(timeRanges['6amTo12pm'], 'dep_time');
    this.filteredSchedules.departure['12pmTo6pm'] = filterSchedulesByTimeRange(timeRanges['12pmTo6pm'], 'dep_time');
    this.filteredSchedules.departure.after6pm = filterSchedulesByTimeRange(timeRanges.after6pm, 'dep_time');

    this.filteredSchedules.arrival.before6am = filterSchedulesByTimeRange(timeRanges.before6am, 'arr_time');
    this.filteredSchedules.arrival['6amTo12pm'] = filterSchedulesByTimeRange(timeRanges['6amTo12pm'], 'arr_time');
    this.filteredSchedules.arrival['12pmTo6pm'] = filterSchedulesByTimeRange(timeRanges['12pmTo6pm'], 'arr_time');
    this.filteredSchedules.arrival.after6pm = filterSchedulesByTimeRange(timeRanges.after6pm, 'arr_time');
  }


  onDepartureArrivalChange(controlName: string) {
    const isChecked = this.busBookingForm.get(controlName)?.value;
    this.filteredScheduleList = [];

    const selectedTimes = Object.keys(this.busBookingForm.controls).filter(key =>
      key.startsWith('departureTime') || key.startsWith('arrivalTime')
    ).filter(k => this.busBookingForm.get(k)?.value);

    selectedTimes.forEach(k => {
      const type = k.startsWith('departureTime') ? 'departure' : 'arrival';
      const timeKey = k.replace(type + 'Time', '');
      this.filteredScheduleList.push(...this.filteredSchedules[type][timeKey]);
    });

    if (selectedTimes.length === 0) this.filteredScheduleList = [...this.schedulesList];
  }


  openDatePicker() {
    const dateInput: any = document.querySelector('input[formControlName="dateOfDeparture"]');
    if (dateInput) {
      dateInput.showPicker();
    }
  }

  updateDate(event: any) {
    this.busBookingForm.patchValue({ dateOfDeparture: event.target.value });
  }

  getStages(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.bitlaService.getStages().subscribe({
        next: (res: any) => {
          if (res && res.result) {
            this.stageList = res.result.slice(1).map((stage: any[]) => ({
              id: stage[0],
              name: stage[1],
              city_id: stage[2],
              city_name: stage[3],
              latitude: stage[4],
              longitude: stage[5],
              area_name: stage[6]
            }));
            resolve();
          } else {
            this.stageList = [];
            resolve();
          }
        },
        error: (err: any) => {
          console.error('Error fetching stages:', err);
          reject(err);
        }
      });
    });
  }



  activeIndex: { departure: number; destination: number } = {
    departure: -1,
    destination: -1
  };


  handleKeyDown(event: KeyboardEvent, controlName: 'departure' | 'destination') {
    const list = controlName === 'departure'
      ? this.filteredDepartureCities
      : this.filteredDestinationCities;

    if (!list || list.length === 0) return;

    const dropdown = document.getElementById(`${controlName}Dropdown`);

    if (event.key === 'ArrowDown') {
      event.preventDefault();
      this.activeIndex[controlName] = (this.activeIndex[controlName] + 1) % list.length;
      this.scrollToActive(dropdown, this.activeIndex[controlName]);
    }
    else if (event.key === 'ArrowUp') {
      event.preventDefault();
      this.activeIndex[controlName] = (this.activeIndex[controlName] - 1 + list.length) % list.length;
      this.scrollToActive(dropdown, this.activeIndex[controlName]);
    }
    else if (event.key === 'Enter') {
      event.preventDefault();
      if (this.activeIndex[controlName] >= 0) {
        this.selectItem(list[this.activeIndex[controlName]], controlName);
        this.activeIndex[controlName] = -1;
      }
    }
  }

  scrollToActive(dropdown: HTMLElement | null, index: number) {
    if (!dropdown) return;

    const items = dropdown.querySelectorAll('li');
    if (!items || items.length === 0) return;

    const activeItem = items[index] as HTMLElement;
    if (activeItem) {
      activeItem.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    }
  }


  updateDestinationDropdown() {
    if (!this.selectedDepartureID) {
      this.filteredDestinationCities = [];
      return;
    }

    const validDestIds = this.cityPairList
      .filter(pair => pair.origin_id === this.selectedDepartureID)
      .map(pair => pair.destination_id);

    this.filteredDestinationCities = this.cityList.filter(city =>
      validDestIds.includes(city.id)
    );

    const currentDestination = this.busBookingForm.get('destination')?.value;
    if (!this.filteredDestinationCities.some(c => c.name === currentDestination)) {
      this.busBookingForm.patchValue({ destination: null });
      this.selecteddestinationId = null;
    }
  }

  filterItem(controlName: string) {
    const value = this.busBookingForm.controls[controlName].value?.toLowerCase() || '';

    // 🚫 Prevent typing destination before departure
    if (controlName === 'destination' && !this.busBookingForm.get('departure')?.value) {
      this.formValidation.showAlert("Please select the Departure first!", "danger");
      this.busBookingForm.patchValue({ destination: '' });
      this.filteredDestinationCities = [];
      return;
    }

    if (controlName === 'departure') {
      this.filteredDepartureCities = this.cityList.filter(city =>
        city.name.toLowerCase().includes(value)
      );
    } else if (controlName === 'destination') {

      let possibleDestinations = this.cityList;

      if (this.selectedDepartureID) {
        const validDestIds = this.cityPairList
          .filter(pair => pair.origin_id === this.selectedDepartureID)
          .map(pair => pair.destination_id);

        possibleDestinations = this.cityList.filter(city =>
          validDestIds.includes(city.id)
        );
      }

      this.filteredDestinationCities = possibleDestinations.filter(city =>
        city.name.toLowerCase().includes(value)
      );
    }
  }

  selectItem(city: any, controlName: string) {

    if (controlName === 'destination' && !this.busBookingForm.get('departure')?.value) {
      this.formValidation.showAlert("Please select the Departure first!", "danger");

      this.busBookingForm.patchValue({ destination: '' });

      this.filteredDestinationCities = [];
      return;
    }

    this.busBookingForm.patchValue({ [controlName]: city.name });

    if (controlName === 'departure') {
      this.selectedDepartureID = city.id;
      this.busBookingForm.patchValue({ origin_id: city.id });
      this.filteredDepartureCities = [];
      this.updateDestinationDropdown();
    }
    else if (controlName === 'destination') {
      this.selecteddestinationId = city.id;
      this.busBookingForm.patchValue({ destination_id: city.id });
      this.filteredDestinationCities = [];
    }
  }


  modify() {

    if (!this.busBookingForm.get("departure")?.value) {
      this.formValidation.showAlert("Select the Departure", "danger")
      return
    }
    if (!this.busBookingForm.get("destination")?.value) {
      this.formValidation.showAlert("Select the Destination", "danger")
      return
    }
    if (!this.busBookingForm.get("dateOfDeparture")?.value) {
      this.formValidation.showAlert("Select the Traveling Date", "danger")
      return
    }

    const departureName = this.busBookingForm.value.departure?.trim()?.toLowerCase();
    const destinationName = this.busBookingForm.value.destination?.trim()?.toLowerCase();
    const travelId = this.busBookingForm.value.dateOfDeparture;

    const originCity = this.cityList.find(city => city.name.toLowerCase() === departureName);
    const destinationCity = this.cityList.find(city => city.name.toLowerCase() === destinationName);

    if (originCity && destinationCity && travelId) {
      const originId = originCity.id;
      const destinationId = destinationCity.id;

      console.log('Fetching schedules for:', { originId, destinationId, travelId });

      this.busBookingForm.patchValue({
        departure: originCity.name,
        destination: destinationCity.name,
        dateOfDeparture: travelId
      });

      this.router.navigate([], {
        relativeTo: this.route,
        queryParams: {
          origin_id: originId,
          destination_id: destinationId,
          DOD: travelId
        },
        replaceUrl: true,
        queryParamsHandling: 'merge'
      });

      this.loadSchedules(originId, destinationId, travelId);
    } else {
      console.warn('Departure, Destination, or Date is missing or invalid.');
    }
  }

  getSavedPassengerDtls() {
    this.http.httpGet(API_URLS.GetBusBookingSeat).subscribe((res: any) => {
      this.savedPassengerDtls = res;
    })
  }

  assignPassengerDtls() {
    const getSavedPassengerDtls = this.savedPassengerDtls.find((item: any) => item.busBookingSeatId == Number(this.busBookingForm.controls['savedPassengerID'].value));
    if (getSavedPassengerDtls) {
      this.busBookingForm.patchValue({
        fName: getSavedPassengerDtls?.firstName,
        mName: getSavedPassengerDtls?.middleName,
        lName: getSavedPassengerDtls?.lastName,
        gender: getSavedPassengerDtls?.gender,
        passengerEmail: getSavedPassengerDtls?.email,
        passengerPhoneNo: getSavedPassengerDtls?.contactNo,
        aadharNo: getSavedPassengerDtls?.aadharNo,
        PANNo: getSavedPassengerDtls?.pancardNo,
        emergencyNo: getSavedPassengerDtls?.emergencyNo,
        bloodGroup: getSavedPassengerDtls?.bloodGroup,
        passengerDOB: getSavedPassengerDtls?.dob?.split('T')[0],
        foodPref: getSavedPassengerDtls?.foodPref
      });
    }
  }


  saveMyDetails() {

    for (let i = 0; i < this.passengerDetails.length; i++) {
      const p = this.passengerDetails.at(i);
      if (!p.get("passengerGender")?.value) { this.formValidation.showAlert(`Select gender for passenger ${i + 1}`, "danger"); return; }
      if (!p.get("passengerName")?.value?.trim()) { this.formValidation.showAlert(`Enter name for passenger ${i + 1}`, "danger"); return; }
      if (!p.get("passengerAge")?.value) { this.formValidation.showAlert(`Enter age for passenger ${i + 1}`, "danger"); return; }
      if (!p.get("passengerPhoneNo")?.value) { this.formValidation.showAlert(`Enter phone number for passenger ${i + 1}`, "danger"); return; }
      if (!p.get("passengerAddress")?.value?.trim()) { this.formValidation.showAlert(`Enter address for passenger ${i + 1}`, "danger"); return; }
      if (!p.get("passengerIDCard")?.value) { this.formValidation.showAlert(`Select ID Card for passenger ${i + 1}`, "danger"); return; }
      if (!p.get("passengerIDCardNo")?.value?.trim()) { this.formValidation.showAlert(`Enter ID number for passenger ${i + 1}`, "danger"); return; }
    }

    if (!this.busBookingForm.get("passengerAlternativeNo")?.value) { this.formValidation.showAlert("Enter Alternative Number", "danger"); return; }
    if (!this.busBookingForm.get("passengerEmail")?.value) { this.formValidation.showAlert("Enter Vaild Email ID", "danger"); return; }

    const firstPassenger = this.passengerDetails.at(0) as FormGroup;
    const loggedUser = JSON.parse(localStorage.getItem('user') ?? '{}');
    const userId = Number(loggedUser?.UserId ?? 0);

    const selectedSchedule =
      this.selectedSchedule ||
      this.schedulesList.find((s: any) => s.id === this.selectedScheduleId);

    const boardingStageId = selectedSchedule?.boarding_stages?.split('|')[0]?.trim();
    const droppingStageId = selectedSchedule?.dropoff_stages?.split('|')[0]?.trim();

    const fareAmount = Number(
      selectedSchedule?.show_fare_screen ||
      selectedSchedule?.fare ||
      selectedSchedule?.fare_str || 0
    ).toFixed(2);


    const bitlaRequest = {
      pause_notification_for_branch_upi_payment_link: "false",
      book_ticket: {
        pause_notification_for_branch_upi_payment_link: "false",
        seat_details: {
          seat_detail: this.selectedSeats.map((seat: any, index: number) => ({
            seat_number: seat,
            fare: fareAmount,
            title: firstPassenger.get('passengerGender')?.value?.toUpperCase() === 'M' ? 'Mr' : 'Ms',
            name: firstPassenger.get('passengerName')?.value?.trim() || '',
            age: firstPassenger.get('passengerAge')?.value?.toString() || '',
            sex: firstPassenger.get('passengerGender')?.value?.toUpperCase() === 'M' ? 'M' : 'F',
            is_primary: index === 0 ? 'true' : 'false',
            id_card_type: '1',
            id_card_number: firstPassenger.get('passengerIDCardNo')?.value || '',
            id_card_issued_by: 'NA',
            pause_notification_for_branch_upi_payment_link: "false"
          })),
        },
        contact_detail: {
          mobile_number: firstPassenger.get('passengerPhoneNo')?.value?.toString() || '',
          emergency_name: firstPassenger.get('passengerName')?.value?.trim() || '',
          email: this.busBookingForm.get('passengerEmail')?.value?.trim() || '',
        },
      },
      origin_id: this.originId?.toString() || '',
      destination_id: this.destinationId?.toString() || '',
      boarding_at: boardingStageId,
      drop_of: droppingStageId,
      no_of_seats: this.selectedSeats.length.toString(),
      travel_date: this.travelDate || '',
      customer_company_gst: { name: '', gst_id: '', address: '' }
    };

    const scheduleId = Number(this.selectedScheduleId || 0);

    this.bitlaService.postTentativeBooking(scheduleId, bitlaRequest).subscribe({
      next: (bitlaRes: any) => {

        const ticketDetails = bitlaRes?.bitlaResponse?.result?.ticket_details;
        const pnrNumber = ticketDetails?.pnr_number;

        this.bitlaService.postConfirmBooking(pnrNumber, {}).subscribe({
          next: (confirmRes: any) => {

            const confirmDetails = confirmRes?.bitlaResponse?.result?.ticket_details;
            const pnrNumber = confirmDetails.ticket_number || confirmDetails.pnr_number;
            const ticket_status = confirmDetails.ticket_status;

            const param = {
              "flag": 'I',
              "busBookingSeatID": 0,
              "busBookingDetailsID": 0,
              "busOperatorID": 0,
              "userID": userId,
              "forSelf": true,
              "isPrimary": true,
              "seatNo": this.selectedSeats.join(', '),
              "firstName": firstPassenger.get('passengerName')?.value?.trim() || '',
              "middleName": '',
              "lastName": '',
              "email": this.busBookingForm.get('passengerEmail')?.value?.trim() || '',
              "contactNo": firstPassenger.get('passengerPhoneNo')?.value?.toString() || '',
              "gender": firstPassenger.get('passengerGender')?.value || '',
              "aadharNo": firstPassenger.get('passengerIDCardNo')?.value || '',
              "pancardNo": firstPassenger.get('passengerIDCardNo')?.value || '',
              "bloodGroup": this.busBookingForm.get('bloodGroup')?.value || '',
              "dob": new Date().toISOString(),
              "foodPref": this.busBookingForm.get('foodPref')?.value || '',
              "disabled": false,
              "pregnant": false,
              "registeredCompanyNumber": '',
              "registeredCompanyName": '',
              "drivingLicence": '',
              "passportNo": '',
              "rationCard": '',
              "voterID": '',
              "others": '',
              "NRI": false,
              "savePassengerDetails": 'Yes',
              "Status": ticket_status,
              "LockStatus": null,
              "CancelledBy": null,
              "CancelledDate": null,
              "PaymentStatus": null,
              "JourneyDate": this.travelDate || '',
              "TicketNo": pnrNumber,
              "createdBy": userId
            };

            this.http.httpPost('/BusBookingSeat/SaveBusBookingSeat', param).subscribe({
              next: (saveRes: any) => {

                this.formValidation.showAlert("Booking Confirmed Successfully!", "success");
                console.log("ticket_status", ticket_status);
                setTimeout(() => {
                  this.router.navigate(
                    ['/payment', pnrNumber],
                    {
                      queryParams: {  
                        amount: fareAmount,
                        status: ticket_status
                      },
                      state: {
                        passengerData: param,
                        bitlaResponse: confirmDetails,
                        pnrNumber: pnrNumber
                      }
                    }
                  );

                }, 1500);

              },
              error: (err) => {
                console.error("Error saving bus booking", err);
                this.formValidation.showAlert("Something went wrong!", "danger");
              }
            });

          }
        });
      }
    });

  }

  onSelectSchedule(schedule: any) {
    this.selectedScheduleId = schedule.id;
    this.selectedSchedule = schedule;
    console.log('Selected Schedule:', schedule);
  }


  selectedScheduleId: any = null
  selectedSchedule: any = null;

  getFareDetails() {
    let fare = 0;

    const allSeats = [
      ...(this.sleeperLowerDeck?.flat() ?? []),
      ...(this.sleeperUpperDeck?.flat() ?? []),
      ...(this.seaterSeats?.flat() ?? [])
    ];

    this.selectedSeats.forEach(seatNumber => {
      const seat = allSeats.find(s => s.seatNumber === seatNumber);
      if (seat) fare += seat.price;
    });

    const gstPercent = this.selectedBus?.service_tax_percent ?? 0;

    const gstAmount = (gstPercent / 100) * fare;
    const netTotal = fare + gstAmount;

    return {
      fare,
      gstAmount,
      netTotal
    };
  }

  clearSavedDtls() {
    this.busBookingForm.reset();
    this.passengerDetails.clear();
    this.selectedSeats.forEach(() => {
      this.passengerDetails.push(this.createPassengerGroup());
    });
    this.busBookingForm.patchValue({ toggleGST: false });
  }

  formatDate(input: string): string {
    if (!input) return '';

    const [date, time] = input.split(' ');
    if (!date || !time) return '';

    try {
      return new Date(`${date}T${time}:00.000Z`).toISOString();
    } catch (error) {
      console.error("Invalid date format:", input);
      return '';
    }
  }

  isValidEmail(email: string): boolean {
    const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return emailPattern.test(email);
  }

}



