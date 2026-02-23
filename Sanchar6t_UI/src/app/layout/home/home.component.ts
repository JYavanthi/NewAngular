import { AfterViewInit, Component, ElementRef, NgZone, Renderer2, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { API_URLS } from '../../shared/API-URLs'
import { HttpServiceService } from '../../services/http-service.service';
import { FormValidationService } from '../../services/form-validation.service';
import { Router } from '@angular/router';
import { ApiConverterService } from '../../services/api-converter.service';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { BitlaService } from '../../services/bitla-service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements AfterViewInit {
  @ViewChild('upperContainer', { static: false }) upperContainer!: ElementRef;
  @ViewChild('middleContainer', { static: false }) middleContainer!: ElementRef;
  @ViewChild('lowerContainer', { static: false }) lowerContainer!: ElementRef;
  isLeftDisabled: { [key: string]: boolean } = {
    upperContent: true,
    middleContent: true,
    lowerContent: true,
  };
  isRightDisabled: { [key: string]: boolean } = {
    upperContent: false,
    middleContent: false,
    lowerContent: false,
  };
  destinationAttachId = 5;
  destinationImageSrc: string | null = null;
  homeForm!: FormGroup;
  showTrackLocation: boolean = false;
  showAdvBusTkt: boolean = false;
  showAdvBusTktOnline: boolean = false;
  showBusSiteTkt: boolean = false;
  showBusCost: boolean = false;
  showBusDiscount: boolean = false;
  showNewBus: boolean = false;
  showGovtTkt: boolean = false;
  showBookTik: boolean = false;
  showChangeDate: boolean = false;
  showPrintout: boolean = false;
  showLostTik: boolean = false;
  showmTicket: boolean = false;
  showResend: boolean = false;
  showWrngNum: boolean = false;
  showCrDr: boolean = false;
  showpurchas: boolean = false;
  showpayment: boolean = false;
  showaccount: boolean = false;
  showBusCancel: boolean = false;
  showHowBusCancel: boolean = false;
  showBusMiss: boolean = false;
  showRefundCase: boolean = false;
  showBusLeave: boolean = false;
  showReschedule: boolean = false;
  showInsurance: boolean = false;
  showPopularBus: boolean = false;
  showPopularCities: boolean = false;
  showPopularOperators: boolean = false;
  cityList: any[] = [];
  cityPairList: any[] = [];
  stageList: any[] = [];
  filteredDepartureCities: any[] = [];
  filteredDestinationCities: any[] = [];
  selectedDepartureID: number | null = null;
  selectedDestinationID: number | null = null;
  activeService: string = 'Buses';
  travelID: any;
  packageList: any[] = [];

  // packageList: any[] = [
  //   { 'image': 'tirupati.jpg', 'routerLink': '/buspackage', 'wishlist': false, 'minPrice': 1500, 'state': 'Andhra Pradesh', 'country': 'India', 'destination': 'ONE DAY TIRUPATI PACKAGE', 'days': 3, 'rating': 4.3, 'reviews': 11, 'shortDesc': 'Tirupati is a city in the Indian state of Andhra Pradesh. Its Sri Venkateswara Temple sits atop one of the the 7 peaks of Tirumala Hills, attracting scores of Hindu pilgrims. Sri Venkateswara National Park, home to the temple, also contains the Sri Venkateswara Zoological Park with lions and primates.' },
  //   { 'image': 'Raigad-Fort-Photos.jpg', 'routerLink': '/buspackage', 'wishlist': true, 'minPrice': 1300, 'state': 'Maharashtra', 'country': 'India', 'destination': 'ONE DAY PUNE PACKAGE', 'days': 4, 'rating': 4.4, 'reviews': 14, 'shortDesc': 'Raigad is a hill fort situated at about 25 Km from Mahad in the Raigad district. Chhatrapati Shivaji renovated this fort and made it his capital in 1674 AD. The rope-way facility is available at Raigad Fort, to reach at the fort from ground in few minutes.. .' },
  //   { 'image': 'jagannath puri.jpg', 'routerLink': '/buspackage', 'wishlist': false, 'minPrice': 1400, 'state': 'Odisha', 'country': 'India', 'destination': 'ONE DAY PURI PACKAGE', 'days': 5, 'rating': 4.3, 'reviews': 15, 'shortDesc': 'Shri Jagannath Puri Temple is one of the most impressing monuments of the Indian State Odisha, was constructed by a famous king of Ganga Dynasty Ananta Varman Chodaganga Deva dating back to 12th century at the seashore Puri.' },
  //   { 'image': 'india gate.jpeg', 'routerLink': '/buspackage', 'wishlist': false, 'minPrice': 1150, 'state': 'Delhi', 'country': 'India', 'destination': 'ONE DAY Delhi PACKAGE', 'days': 3, 'rating': 4.2, 'reviews': 16, 'shortDesc': 'India Gate is a war memorial in New Delhi, India that honors the soldiers who fought for the British Empire in World War I and the Third Anglo-Afghan War. Its also known as the All-India War Memorial or the Delhi Memorial. ' },
  //   { 'image': 'coorg.webp', 'routerLink': '/buspackage', 'wishlist': true, 'minPrice': 1400, 'state': ' Karnataka', 'country': 'India', 'destination': 'ONE DAY COORG PACKAGE', 'days': 4, 'rating': 4.3, 'reviews': 20, 'shortDesc': 'Coorg in Karnataka is known as the Scotland of India. It is also known as Kodagu, and is famous for its exquisite climate, hill ranges, and coffee plantations. It resembles Scotland in terms of climate, terrain, architecture, and waterfalls.' },
  //   { 'image': 'mumbai.jpg', 'routerLink': '/buspackage', 'wishlist': true, 'minPrice': 1400, 'state': 'Maharashtra', 'country': 'India', 'destination': 'ONE DAY MUMBAI PACKAGE', 'days': 3, 'rating': 4.4, 'reviews': 20, 'shortDesc': 'Mumbai (also known as Bombay, the official name until 1995) is the capital city of the Indian state of Maharashtra. Mumbai lies on the Konkan coast on the west coast of India and has a deep natural harbour. In 2008, Mumbai was named an alpha world city.' },
  // ];

  Travelers: any[] = [
    { 'image': 'ravi kumar.png', 'name': 'Ravi Kumar', 'Destination': 'Tirupati', 'shortDesc': 'I had an amazing trip to Tirupati! Booking the trip was seamless, and the experience at the temple was simply spiritual. The local guides were incredibly knowledgeable, and the transport arrangements were comfortable. Highly recommend this service for a hassle-free pilgrimage journey!', 'timeDate': '7:20 AM - Dec 6, 2024' },
    { 'image': 'sushant.jpg', 'name': 'Sushant', 'Destination': 'Mumbai', 'shortDesc': 'Mumbai (also known as Bombay, the official name until 1995) is the capital city of the Indian state of Maharashtra. Mumbai lies on the Konkan coast on the west coast of India and has a deep natural harbour. In 2008, Mumbai was named an alpha world city.', 'timeDate': '9:41 PM - May 3, 2024' },
    { 'image': 'rajiv.jpg', 'name': 'Raja Babu', 'Destination': 'Pune', 'shortDesc': 'Pune is a city in the Indian state of Maharashtra. Its known as the "Oxford of the East" because of its many educational institutions. Pune is also a major IT and manufacturing hub. ', 'timeDate': '8:40 PM - June 7, 2024' },
    { 'image': 'seema.jpg', 'name': 'Seema', 'Destination': 'Odisha', 'shortDesc': 'The Jagannath Temple at Puri is one of the most revered Vaishnava sites of worship in India. One of the oldest Hindu temples to still be in use, its main shrine was built by Anantavarman of the Chodaganga dynasty in the tenth century.', 'timeDate': '10:36 AM - Dec 9, 2024' },
    { 'image': 'sanjiv.jpg', 'name': 'Rajiv.R', 'Destination': 'Karnataka', 'shortDesc': 'Karnataka has many colleges and universities serving the needs of a large Indian population. The silicon valley of India is located in Karnataka. There are many scientific organisations located in Karnataka. Karnataka is the 6th largest state in India by area and 8th largest state in India by population.' },
    { 'image': 'r.jpg', 'name': 'Sanjiv', 'Destination': 'Goa', 'shortDesc': 'Sandy beaches, estuaries, and promontories characterize the 65-mile (105-km) coastline of mainland Goa. In the interior region, low, forested plateaus merge with the wooded slopes of the Western Ghats, which rise to nearly 4,000 feet (1,220 metres) on the eastern edge of the state.', 'timeDate': '3:44 PM - Jan 10, 2024' },
  ];
  schedulesAPIData: any =
    {
      "result": [
        [
          "id",
          "number",
          "name",
          "operator_service_name",
          "origin_id",
          "destination_id",
          "route_id",
          "travel_id",
          "bus_type",
          "dep_time",
          "arr_time",
          "duration",
          "available_seats",
          "total_seats",
          "service_tax_percent",
          "fare_str",
          "is_cancellable",
          "commission",
          "status",
          "op_schedule_id",
          "convenience_charge_percent",
          "amenities",
          "boarding_stages",
          "dropoff_stages",
          "deals",
          "cancellation_policies",
          "trip_id",
          "agent_cancellation_time",
          "updated_at",
          "route_map_id",
          "is_service_tax_applicable",
          "via",
          "is_ac_bus",
          "allow_reschedule",
          "travel_date",
          "route_version",
          "bp_dp_fares",
          "bp_dp_pair_fares",
          "city_seq_order",
          "cc_type",
          "main_dep_time",
          "transaction_charges",
          "no_coach_layout",
          "is_package_fare_allowed",
          "cancellation_reference_type",
          "social_distancing_guaranteed",
          "reschedule_policies",
          "o_available",
          "o_available_gst",
          "is_o_available_fare_enabled_booking",
          "parent_travel_id",
          "show_fare_screen",
          "o_fare_str",
          "last_seats"
        ],
        [
          2025022311391,
          "SAI12345",
          "SP TRAVELS",
          "SP TRAVELS",
          134,
          251,
          583,
          21,
          "2+2, Seater, Non-AC, LCD",
          "00:00",
          "14:30",
          "14:30",
          10,
          12,
          0.0,
          "ST:525.0",
          false,
          "",
          "Update",
          258403,
          0.0,
          "[\"Bathroom\",\"Charging Point\",\"Snacks\",\"WiFi\"]",
          "5889|20:00",
          "11081|14:30",
          " ",
          "",
          "BITLA:21:583",
          0,
          "2025-02-21 15:30:37 UTC",
          null,
          false,
          "",
          false,
          true,
          "2025-02-23",
          "2025022311391-2",
          {},
          {},
          [
            "134",
            "251"
          ],
          "",
          "24/02/2025 00:00",
          "",
          false,
          false,
          "By Main Departure Time",
          false,
          "",
          "",
          "",
          false,
          "",
          "525.0",
          "",
          ""
        ],
        [
          2025022311390,
          "laxminew",
          "123",
          "Siri Travels R5",
          134,
          251,
          1169,
          21,
          "1+2, Sleeper, AC, Non-Video",
          "20:00",
          "06:00",
          "10:00",
          29,
          30,
          5.0,
          "SLB:1000.0,SUB:1000.0,DLB:1000.0,DUB:1000.0",
          true,
          "",
          "Update",
          258404,
          0.0,
          "[\"Pillow\",\"CC Camera\"]",
          "2544|20:00",
          "7338|06:00",
          " ",
          "",
          "BITLA:21:1169",
          0,
          "2025-02-21 13:07:16 UTC",
          null,
          true,
          "",
          true,
          true,
          "2025-02-23",
          "2025022311390-4",
          {},
          {},
          [
            "134",
            "251"
          ],
          "",
          "23/02/2025 20:00",
          "",
          false,
          false,
          "By Main Departure Time",
          false,
          "",
          "",
          "",
          false,
          "",
          "1000.0",
          "",
          ""
        ]
      ],
      "stage_names": {
        "2544": "Anand Rao Circle",
        "5889": "Dairy circle",
        "7338": "Koyembedu",
        "11081": "chennai old bustand"
      },
      "processed_schedules_count": 2
    }
  schedulesList: any;
  popularBusRoutes = [
    "Delhi To Manali Bus",
    "Delhi To Rishikesh Bus",
    "Delhi To Shimla Bus",
    "Delhi To Nainital Bus",
    "Delhi To Katra Bus",
    "Bangalore To Goa Bus",
    "Bangalore To Hyderabad Bus",
    "Bangalore To Tirupathi Bus",
    "Bangalore To Chennai Bus",
    "Bangalore To Pondicherry Bus",
    "Hyderabad To Bangalore Bus",
    "Hyderabad To Goa Bus",
    "Hyderabad To Srisailam Bus",
    "Hyderabad To Vijayawada Bus",
    "Hyderabad To Tirupathi Bus",
    "Pune To Goa Bus",
    "Pune To Mumbai Bus",
    "Pune To Nagpur Bus",
    "Pune To Kolhapur Bus",
    "Pune To Nashik Bus",
    "Mumbai To Goa Bus",
    "Mumbai To Pune Bus",
    "Mumbai To Shirdi Bus",
    "Mumbai To Mahabaleshwar Bus",
    "Mumbai To Kolhapur Bus",
    "Kolkata To Digha Bus",
    "Kolkata To Siliguri Bus",
    "Kolkata To Puri Bus",
    "Kolkata To Bakkhali Bus",
    "Kolkata To Mandarmani Bus",
    "Chennai To Bangalore Bus",
    "Chennai To Pondicherry Bus",
    "Chennai To Coimbatore Bus",
    "Chennai To Madurai Bus",
    "Chennai To Tirupathi Bus",
    "Chandigarh To Manali Bus",
    "Chandigarh To Shimla Bus",
    "Chandigarh To Delhi Bus",
    "Chandigarh To Dehradun Bus",
    "Chandigarh To Amritsar Bus",
    "Coimbatore To Chennai Bus",
    "Coimbatore To Bangalore Bus",
    "Coimbatore To Ooty Bus",
    "Coimbatore To Tiruchendur Bus",
    "Coimbatore To Madurai Bus",
    "Agra to Bareilly Bus",
    "Hisar to Chandigarh Bus",
    "Hisar to Delhi Bus",
    "Lucknow to Ballia Bus",
    "Lucknow to Moradabad Bus",
    "Rajkot to Dwarka Bus",
    "Siliguri to Gangtok Bus",
    "Ahmedabad to Goa Bus",
    "Ahmedabad to Kanpur Bus",
    "Akola to Pune Bus",
    "Delhi to Dehradun Bus",
    "Delhi to Haridwar Bus",
    "Dehradun to Delhi Bus",
    "Delhi to Agra Bus",
    "Delhi to Varanasi Bus"
  ];
  popularCities = [
    "Hyderabad",
    "Bangalore",
    "Chennai",
    "Pune",
    "Delhi",
    "Mumbai",
    "Kolkata",
    "Ernakulam",
    "Ahmedabad",
    "Vijayawada",
    "Jaipur",
    "Indore",
    "Lucknow",
    "Bhopal",
    "Goa",
    "Ayodhya"
  ];
  popularBusOperators = [
    "No 1 Air Travels",
    "YBM Travels",
    "Sri SMS Travels",
    "Svkdt Travels",
    "RKT Tours and Travels",
    "Royal CarsRahul Travels",
    "JBT Travels",
    "Raj ExpressVaishali ExpressRahul Travels",
    "IndoreDelhi Tours And Travels",
    "Ashok Travels",
    "Greenline Travels",
    "Pawan Travels",
    "Ravi Travels",
    "VRL Travels",
    "Dolphin Travels",
    "Ganesh Travels",
    "Kaveri Travels",
    "National Travels",
    "Bharathi Travels",
    "City Land Travels",
    "KKaveri Travels",
    "KK Travels",
    "Mahadev Travels",
    "Maharaja Travels",
    "M R Travels",
    "New Payal Travels",
    "Paras Travels",
    "Shree Parshwanath Travels",
    "Payal Travels",
    "R K TravelsShivam Travels",
    "Shree Mahaveer Travels",
    "SRS Travels",
    "Tulsi TravelsVaibhav Travels",
    "Vikas Travels",
    "Amarnath Travels",
    "Anand Travels",
    "Ashapura Travels",
    "Ashok Bus ServiceAshoka Travels",
    "Chartered",
    "NeugoIntrcity Smart",
    "Uttar Pradesh State Road Transport Corporation (UPSRTC)",
    "HRTC",
    "South Bengal State Road Transport Corporation (SBSTC)",
    "NBSTC"
  ];
  services = [
    { name: 'Buses', icon: 'bus-icon', label: 'Buses', 'status': '' },
    { name: 'Cabs', icon: 'cab-icon', label: 'Cabs', 'status': 'Coming Soon' },
    { name: 'Trains', icon: 'train-icon', label: 'Trains', 'status': 'Coming Soon' },
    { name: 'Flights', icon: 'plane-icon', label: 'Flights', 'status': 'Coming Soon' },
    { name: 'Hotels', icon: 'hotel-icon', label: 'Hotels', 'status': 'Coming Soon' },
    { name: 'Homestays', icon: 'home-icon', label: 'Homestays & Villas', 'status': 'Coming Soon' },
    { name: 'Holiday', icon: 'holiday-icon', label: 'Holiday Packages', 'status': 'Coming Soon' }
  ];
  constructor(private fb: FormBuilder, private http: HttpClient, private renderer: Renderer2, private apiConverterService: ApiConverterService, private httpService: HttpServiceService, private router: Router, private formValidation: FormValidationService, private bitlaService: BitlaService, private ngZone: NgZone) {
  }


  httpGetBlob(endpoint: string): Observable<Blob> {
    return this.http.get(endpoint, { responseType: 'blob' });
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

  ngOnInit(): void {
    this.formInit();
    this.getCities();
    this.getCityPairs();

    this.schedulesList = this.apiConverterService.mapData(this.schedulesAPIData);
    localStorage.setItem('localSchedulesList', JSON.stringify(this.schedulesList));

    this.getAllPackages();
  }

  getImageFromFileData(packageID: number, section: string): Promise<string> {
    return new Promise((resolve, reject) => {
      this.httpService.httpGetNew(`/Attachment/GetFileData?packageId=${packageID}&section=${section}`)
        .subscribe(
          async (data: any) => {
            let attachId: number | undefined;

            if (Array.isArray(data) && data.length > 0) {
              attachId = data[0].attachmentId;
            } else if (data?.attachmentId) {
              attachId = data.attachmentId;
            }

            if (attachId) {
              const imageSrc = await this.getDestinationImage(attachId);
              resolve(imageSrc);
            } else {
              resolve('assets/default-image.jpg');
            }
          },
          error => {
            console.error('Error fetching file data for packageID:', packageID, error);
            resolve('assets/default-image.jpg');
          }
        );
    });
  }


  getDestinationImage(attachId: number): Promise<string> {
    console.log('Fetching image for attachId:', attachId, 'at', new Date().toISOString());

    return new Promise((resolve, reject) => {
      this.httpService
        .httpGetNew(`/Attachment/View/${attachId}`, {}, 'blob')
        .subscribe(
          (res: Blob) => {
            if (res.size > 0) {
              const objectURL = URL.createObjectURL(res);
              resolve(objectURL);
            } else {
              console.error('Empty blob received for attachId:', attachId);
              resolve('assets/default-image.jpg');
            }
          },
          (err) => {
            console.error('Error fetching image blob for attachId:', attachId, err);
            resolve('assets/default-image.jpg');
          }
        );
    });
  }


  formInit() {
    this.homeForm = this.fb.group({
      departure: ['', Validators.required],
      destination: ['', Validators.required],
      origin_id: [null],
      destination_id: [null],
      dateOfDeparture: [new Date().toISOString().split('T')[0], Validators.required],
      search: ['']
    });
  }


  setServiceActive(service: string) {
    this.activeService = service;
  }

  get filteredPackages() {
    if (!this.homeForm.controls['search'].value) {
      return this.packageList.filter((item: any) => item.packageId == 2017);
    }
    return this.packageList.filter(item =>
      item.packageName.toLowerCase().includes(this.homeForm.controls['search'].value.toLowerCase())
    );
  }

  scrollLeft(data: string) {
    const container = this.getContainer(data);
    if (container) {
      container.scrollLeft -= 300;
      this.updateButtonOpacity(container, data);
    }
  }

  scrollRight(data: string) {
    const container = this.getContainer(data);
    if (container) {
      container.scrollLeft += 300;
      this.updateButtonOpacity(container, data);
    }
  }

  updateButtonOpacity(container: HTMLElement, data: string) {
    this.isLeftDisabled[data] = container.scrollLeft === 0;
    this.isRightDisabled[data] = container.scrollLeft + container.clientWidth >= container.scrollWidth;
  }

  getContainer(data: string): HTMLElement | null {
    switch (data) {
      case 'upperContent':
        return this.upperContainer.nativeElement;
      case 'middleContent':
        return this.middleContainer.nativeElement;
      case 'lowerContent':
        return this.lowerContainer.nativeElement;
      default:
        return null;
    }
  }

  getAllPackages() {
    this.httpService.httpGet(API_URLS.GetpackageDetails).subscribe((res: any) => {
      this.packageList = res.data;

      for (let item of this.packageList) {
        const packageID = item.packageId;
        const section = "Bus_Package_Section";

        this.getImageFromFileData(packageID, section).then((imageSrc: string) => {
          item.imageSrc = imageSrc;
        });
      }
    });
  }

  getStages() {
    this.httpService.httpGetNew(API_URLS.STAGES).subscribe((res: any) => {
      this.stageList = this.apiConverterService.mapData(res);
      if (this.stageList.length > 0) {
        localStorage.setItem('localStageList', JSON.stringify(this.stageList))
      }
    })
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


  filterItem(controlName: string) {
    const value = this.homeForm.controls[controlName].value?.toLowerCase() || '';

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
    this.homeForm.patchValue({ [controlName]: city.name });

    if (controlName === 'departure') {
      this.selectedDepartureID = city.id;
      this.homeForm.patchValue({ origin_id: city.id });
      this.filteredDepartureCities = [];
      this.updateDestinationDropdown();
    } else if (controlName === 'destination') {
      this.selectedDestinationID = city.id;
      this.homeForm.patchValue({ destination_id: city.id });
      this.filteredDestinationCities = [];
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

    const currentDest = this.homeForm.controls['destination'].value;
    if (!this.filteredDestinationCities.some(c => c.name === currentDest)) {
      this.homeForm.patchValue({ destination: '' });
    }
  }


  addNow() {
    this.router.navigate(['/bus-add-package'])
  }

  // bookNow() {
  //   if (!this.formValidation.validateForm(this.homeForm)) {
  //     this.homeForm.markAllAsTouched();
  //     return;
  //   }

  //   this.resolveIdsFromNames();

  //   const originId = this.homeForm.value.origin_id;
  //   const destinationId = this.homeForm.value.destination_id;
  //   const DOD = this.homeForm.value.dateOfDeparture;

  //   if (!originId || !destinationId) {
  //     alert('Please select valid Departure and Destination from the dropdown.');
  //     return;
  //   }

  //   console.log('Navigating with IDs:', { originId, destinationId, DOD });

  //   this.router.navigate(['/bus-booking'], {
  //     queryParams: {
  //       origin_id: originId,
  //       destination_id: destinationId,
  //       DOD: DOD
  //     }
  //   });
  // }

  bookNow() {

    if (!this.formValidation.validateForm(this.homeForm)) {
      this.homeForm.markAllAsTouched();
      return;
    }

    const originId = this.homeForm.value.origin_id;
    const destinationId = this.homeForm.value.destination_id;
    const DOD = this.homeForm.value.dateOfDeparture;

    if (!originId || !destinationId) {
      alert('Please select valid Departure and Destination.');
      return;
    }

    console.log("🚀 Navigating immediately...");

    this.ngZone.run(() => {
      this.router.navigate(['/bus-booking'], {
        queryParams: {
          origin_id: originId,
          destination_id: destinationId,
          DOD: DOD
        }
      });
    });

    Promise.resolve().then(() => {
      if (typeof this.resolveIdsFromNames === 'function') {
        this.resolveIdsFromNames();
      }
    });
  }



  private resolveIdsFromNames() {
    const depName = this.homeForm.value.departure?.trim().toLowerCase();
    if (!this.homeForm.value.origin_id && depName) {
      const city = this.cityList.find(c => c.name.toLowerCase() === depName);
      if (city) {
        this.homeForm.patchValue({ origin_id: city.id });
        this.selectedDepartureID = city.id;
      }
    }

    const destName = this.homeForm.value.destination?.trim().toLowerCase();
    if (!this.homeForm.value.destination_id && destName) {
      const city = this.cityList.find(c => c.name.toLowerCase() === destName);
      if (city) {
        this.homeForm.patchValue({ destination_id: city.id });
        this.selectedDestinationID = city.id;
      }
    }
  }



  toggleContent(value: string) {
    switch (value) {
      case "TrackLocation":
        this.showTrackLocation = !this.showTrackLocation;
        break;

      case "AdvBusTkt":
        this.showAdvBusTkt = !this.showAdvBusTkt;
        break;

      case "AdvBusTktOnline":
        this.showAdvBusTktOnline = !this.showAdvBusTktOnline;
        break;

      case "BusSiteTkt":
        this.showBusSiteTkt = !this.showBusSiteTkt;
        break;

      case "BusCost":
        this.showBusCost = !this.showBusCost;
        break;

      case "BusDiscount":
        this.showBusDiscount = !this.showBusDiscount;
        break;

      case "NewBus":
        this.showNewBus = !this.showNewBus;
        break;

      case "GovtTkt":
        this.showGovtTkt = !this.showGovtTkt;
        break;

      case "BookTik":
        this.showBookTik = !this.showBookTik;
        break;

      case "ChangeDate":
        this.showChangeDate = !this.showChangeDate;
        break;

      case "Printout":
        this.showPrintout = !this.showPrintout;
        break;

      case "LostTik":
        this.showLostTik = !this.showLostTik;
        break;

      case "mTicket":
        this.showmTicket = !this.showmTicket;
        break;

      case "Resend":
        this.showResend = !this.showResend;
        break;

      case "WrngNum":
        this.showWrngNum = !this.showWrngNum;
        break;

      case "CrDr":
        this.showCrDr = !this.showCrDr;
        break;

      case "purchas":
        this.showpurchas = !this.showpurchas;
        break;

      case "payment":
        this.showpayment = !this.showpayment;
        break;

      case "account":
        this.showaccount = !this.showaccount;
        break;

      case "BusCancel":
        this.showBusCancel = !this.showBusCancel;
        break;

      case "HowBusCancel":
        this.showHowBusCancel = !this.showHowBusCancel;
        break;

      case "BusMiss":
        this.showBusMiss = !this.showBusMiss;
        break;

      case "RefundCase":
        this.showRefundCase = !this.showRefundCase;
        break;

      case "BusLeave":
        this.showBusLeave = !this.showBusLeave;
        break;

      case "Reschedule":
        this.showReschedule = !this.showReschedule;
        break;

      case "Insurance":
        this.showInsurance = !this.showInsurance;
        break;

      case "PopularBus":
        this.showPopularBus = !this.showPopularBus;
        break;

      case "PopularCities":
        this.showPopularCities = !this.showPopularCities;
        break;

      case "PopularOperators":
        this.showPopularOperators = !this.showPopularOperators;
        break;
      default:
        break;
    }
  }

  popularBusRoutesFunc(item: any) {
    this.router.navigate(['/bus-booking'], {
      queryParams: {
        busDeparture: item.split(' ')[0],
        busDestination: item.split(' ')[2]
      }
    })
  }

  popularCityFunc(val: any) {
    this.router.navigate(['/popular-city'], {
      queryParams: {
        city: val
      }
    })
  }

  popularBusOperatorsFunc(value: any) {
    this.router.navigate(['/popular-city'], {
      queryParams: {
        operatorName: value
      }
    })
  }

  navigateToBusPackage(packageId: any) {
    this.router.navigate(['/buspackage'], {
      queryParams: {
        packageID: packageId
      }
    })
  }

  toggleWishlist(item: any): void {
    item.wishlist = !item.wishlist;

    console.log(`Package ${item.packageId} wishlist status: ${item.wishlist}`);
  }

}