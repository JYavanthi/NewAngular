import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FormValidationService } from '../../services/form-validation.service';

@Component({
  selector: 'app-popular-city',
  templateUrl: './popular-city.component.html',
  styleUrl: './popular-city.component.scss'
})
export class PopularCityComponent {
  filteredDepartureCities: any[] = [];
  filteredDestinationCities: any[] = [];
  selectedDepartureID!: number;
  selectedDestinationID!: number;
  todaysDate: any = new Date().toISOString().split('T')[0];
  popularCityForm!: FormGroup;
  cityName!: string;
  operatorName!: string;
  cityList: any[] = [];

  constructor(private fb: FormBuilder, private router: Router, private aRoute: ActivatedRoute, private formValidation: FormValidationService,) {
  }

  ngOnInit() {
    this.formInit();
    const cities = localStorage.getItem('localCityList');
    this.cityList = cities ? JSON.parse(cities) : [];

    this.aRoute.queryParams.subscribe((g: any) => {
      if (g.city) {
        this.popularCityForm.patchValue({
          destination: g.city
        })
        this.cityName = g.city;
      }
      if (g.operatorName) {
        this.operatorName = g.operatorName;
      }
    })
  }
  filterItem(controlName: string) {
    const value = this.popularCityForm.controls[controlName].value;
    if (value) {
      const filteredCities = this.cityList.filter((city: any) =>
        city.name.toLowerCase().startsWith(value.toLowerCase())
      );

      if (controlName === 'departure') {
        this.filteredDepartureCities = filteredCities.length ? filteredCities : [''];
      }
      if (controlName === 'destination') {
        this.filteredDepartureCities = []
        this.filteredDestinationCities = filteredCities.length ? filteredCities.filter((a: any) => a.name != this.popularCityForm.value.departure) : [''];
      }
    }
  }
  selectItem(city: any, controlName: string) {
    this.popularCityForm.patchValue({ [controlName]: city.name });
    if (controlName === 'departure') {
      this.selectedDepartureID = city.id;
      this.filteredDepartureCities = [];
    } else if (controlName === 'destination') {
      this.selectedDestinationID = city.id;
      this.filteredDestinationCities = [];
    }
  }

  formInit() {
    this.popularCityForm = this.fb.group({
      departure: ['', Validators.required],
      destination: ['', Validators.required],
      departureDate: [this.todaysDate, Validators.required]
    })
  }

  search() {
    this.router.navigate(['/bus-booking'], {
      queryParams: {
        depart: this.popularCityForm.controls['departure'].value,
        desti: this.popularCityForm.controls['destination'].value,
        dod: this.popularCityForm.controls['departureDate'].value
      }
    })
  }
}
