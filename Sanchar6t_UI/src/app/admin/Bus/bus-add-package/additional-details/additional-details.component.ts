import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormValidationService } from '../../../../services/form-validation.service';
import { HttpServiceService } from '../../../../services/http-service.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { API_URLS } from '../../../../shared/API-URLs';

@Component({
  selector: 'app-additional-details',
  templateUrl: './additional-details.component.html',
  styleUrls: ['./additional-details.component.scss', '../bus-add-package.component.scss'],
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

export class AdditionalDetailsComponent {
  scheduleAndPricingForm!: FormGroup;
  packageID: number | null = null;
  days: any[] = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
  filteredWeekdaysFrom: string[] = [];
  filteredWeekdaysTo: string[] = [];
  filteredWeekendFrom: string[] = [];
  filteredWeekendTo: string[] = [];

  constructor(private fb: FormBuilder, private httpSer: HttpServiceService, private formValidation: FormValidationService) {
  }

  ngOnInit() {
    this.formInit();
    this.packageID = this.formValidation.getPackageID();
    this.fetchScheduleAndPricing();  
  }

  formInit() {
    this.scheduleAndPricingForm = this.fb.group({
      service: ['', Validators.required],
      busType: ['', Validators.required],
      departure: ['', Validators.required],
      arrival: [{ value: '', disabled: true }, Validators.required],
      duration: [null],
      weekdaysFrom: ['', Validators.required],
      weekdaysTo: ['', Validators.required],
      weekdaysPrice: [null, [Validators.required, Validators.min(1)]],
      weekendFrom: ['', Validators.required],
      weekendTo: ['', Validators.required],
      weekendPrice: [null, [Validators.required, Validators.min(1)]]
    })
  }

  getDuration() {
    const departureTime = this.scheduleAndPricingForm.get('departure')?.value;
    const arrivalTime = this.scheduleAndPricingForm.get('arrival')?.value;
    
    if (departureTime && arrivalTime) {
      const [depHours, depMinutes] = departureTime.split(':').map(Number);
      const [arrHours, arrMinutes] = arrivalTime.split(':').map(Number);
  
      let totalDepMinutes = depHours * 60 + depMinutes;
      let totalArrMinutes = arrHours * 60 + arrMinutes;
  
      // If the arrival time is before the departure time, add 24 hours (for next day)
      if (totalArrMinutes < totalDepMinutes) {
        totalArrMinutes += 24 * 60;
      }
  
      // Calculate the duration in minutes
      const durationInMinutes = totalArrMinutes - totalDepMinutes;
      const hours = Math.floor(durationInMinutes / 60);
      const minutes = durationInMinutes % 60;
  
      // Set the duration as a string
      const formattedDuration = `${hours} hrs ${minutes} mins`;
      this.scheduleAndPricingForm.get('duration')?.setValue(formattedDuration);
    } else {
      this.scheduleAndPricingForm.get('duration')?.setValue(null);  // If no departure or arrival time is provided
    }
  }
  
  
  
  formatTime(time: string): string {
    if (!time) return '';
    
    const now = new Date();
    const [hours, minutes] = time.split(':').map(Number);
    
    now.setHours(hours, minutes, 0, 0);
  
    return now.toISOString().split('T')[1].slice(0, -1);
  }
  
  submit() {
    if (!this.formValidation.validateForm(this.scheduleAndPricingForm)) {
      this.scheduleAndPricingForm.markAllAsTouched();
      setTimeout(() => {}, 0);
      return;
    }
  
    const param = {
      "flag": "I",
      "serviceID": 0,
      "packageID": this.packageID || 0,
      "servicename": this.scheduleAndPricingForm.controls['service'].value,
      "busType": this.scheduleAndPricingForm.controls['service'].value,
      "departure": this.formatTime(this.scheduleAndPricingForm.controls['departure'].value),
      "duration": this.scheduleAndPricingForm.controls['duration'].value,  // This will now be a string
      "arrival": this.formatTime(this.scheduleAndPricingForm.controls['arrival'].value),
      "fare": 0,
      "wdFrom": this.scheduleAndPricingForm.controls['weekdaysFrom'].value,
      "wdTo": this.scheduleAndPricingForm.controls['weekdaysTo'].value,
      "wdFare": Number(this.scheduleAndPricingForm.controls['weekdaysPrice'].value),
      "weFrom": this.scheduleAndPricingForm.controls['weekendFrom'].value,
      "weTo": this.scheduleAndPricingForm.controls['weekendTo'].value,
      "weFare": Number(this.scheduleAndPricingForm.controls['weekendPrice'].value),
      "createdBy": 1
    };
  
    this.httpSer.httpPost(API_URLS.POST_SCHEDULE_PRICING, param).subscribe((res: any) => {
      if (res.type == "S") {
        this.formValidation.showAlert('Successfully Submitted', 'success');
        this.scheduleAndPricingForm.reset();
      }
      else {
        this.formValidation.showAlert('Error!', 'danger');
      }
    });
  }

  fetchScheduleAndPricing() {
    if (!this.packageID) return;
  
    const url = `${API_URLS.get_ServiceDtlsByID}?packageID=${this.packageID}`;
  
    this.httpSer.httpGet(url).subscribe(
      (res: any) => {
        if (res.type === 'S' && res.data) {
          const data = res.data;
  
          this.scheduleAndPricingForm.patchValue({
            service: data.servicename,
            busType: data.busType,
            departure: data.departure,
            arrival: data.arrival,
            duration: data.duration,
            weekdaysFrom: data.wdFrom,
            weekdaysTo: data.wdTo,
            weekdaysPrice: data.wdFare,
            weekendFrom: data.weFrom,
            weekendTo: data.weTo,
            weekendPrice: data.weFare
          });
  
          // Enable arrival if it was disabled initially
          this.scheduleAndPricingForm.get('arrival')?.enable();
        } else {
          this.formValidation.showAlert('No existing schedule data found.', 'danger');
        }
      },
      error => {
        console.error('Error fetching schedule details:', error);
        this.formValidation.showAlert('Error loading schedule data', 'danger');
      }
    );
  }
  
  
  
  // Helper method to calculate duration in minutes
  getDurationInMinutes(departure: string, arrival: string): number {
    if (!departure || !arrival) return 0;
  
    const [depHours, depMinutes] = departure.split(':').map(Number);
    const [arrHours, arrMinutes] = arrival.split(':').map(Number);
  
    let totalDepMinutes = depHours * 60 + depMinutes;
    let totalArrMinutes = arrHours * 60 + arrMinutes;
  
    // If arrival time is earlier than departure time, add 24 hours to arrival time
    if (totalArrMinutes < totalDepMinutes) {
      totalArrMinutes += 24 * 60;
    }
  
    return totalArrMinutes - totalDepMinutes;
  }
  

  filterDays(controlName: string) {
    const value = this.scheduleAndPricingForm.controls[controlName].value.toLowerCase();

    if (value) {
      const filteredDays = this.days.filter(day =>
        day.toLowerCase().startsWith(value)
      );

      if (controlName === 'weekdaysFrom') {
        this.filteredWeekdaysFrom = filteredDays.length ? filteredDays : [''];
      } else if (controlName === 'weekdaysTo') {
        this.filteredWeekdaysTo = filteredDays.length ? filteredDays : [''];
      } else if (controlName === 'weekendFrom') {
        this.filteredWeekendFrom = filteredDays.length ? filteredDays : [''];
      } else if (controlName === 'weekendTo') {
        this.filteredWeekendTo = filteredDays.length ? filteredDays : [''];
      }
    } else {
      this.clearFilteredLists(controlName);
    }
  }

  selectDay(day: string, controlName: string) {
    this.scheduleAndPricingForm.patchValue({ [controlName]: day });
    this.clearFilteredLists(controlName);
  }

  clearFilteredLists(controlName: string) {
    if (controlName === 'weekdaysFrom') {
      this.filteredWeekdaysFrom = [];
    } else if (controlName === 'weekdaysTo') {
      this.filteredWeekdaysTo = [];
    } else if (controlName === 'weekendFrom') {
      this.filteredWeekendFrom = [];
    } else if (controlName === 'weekendTo') {
      this.filteredWeekendTo = [];
    }
  }

}
