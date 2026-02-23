
import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormValidationService } from '../../../../services/form-validation.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { HttpServiceService } from '../../../../services/http-service.service';
import { API_URLS } from '../../../../shared/API-URLs';
import { UserServiceService } from '../../../../services/user-service.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-bus-package',
  templateUrl: './bus-package.component.html',
  styleUrls: ['./bus-package.component.scss', '../bus-add-package.component.scss'],
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
export class BusPackageComponent {
  selectedFiles: File | undefined;
  imagePreview: string | ArrayBuffer | null = null;
  busPackageForm!: FormGroup;
  selectedDepartureID!: number | null;
  selectedDestinationID!: number | null;
  selectedCountryID!: number | null;
  selectedStateID!: number | null;
  cityList: any;
  countryList: any;
  stateList: any;
  filteredDepartureCities: any[] = [];
  filteredDestinationCities: any[] = [];
  filteredCountries: any[] = [];
  filteredStates: any[] = [];
  httpService: any;
  stageList: any;
  apiConverterService: any;
  cityPairList: any;
  userId: any;
  @Output() packageGenerated = new EventEmitter<number>();

  constructor(private fb: FormBuilder, private userService: UserServiceService, public http: HttpClient,
    private httpSer: HttpServiceService, private formValidation: FormValidationService) { }

  ngOnInit() {
    this.formInit();
    this.getCities();
    this.getCountries();
  }

  formInit() {
    this.busPackageForm = this.fb.group({
      uploadPackageImage: [''],
      packageName: ['', Validators.required],
      packagePrice: [null, [Validators.required, Validators.min(1)]],
      state: ['', Validators.required],
      country: ['', Validators.required],
      departure: ['', Validators.required],
      destination: ['', Validators.required],
      boardingPoint: ['', Validators.required],
      droppingPoint: ['', Validators.required],
      noOfDays: [null, [Validators.required, Validators.min(1)]],
      shortDescription: ['', Validators.required],
      description: ['', Validators.required],
      additionalNotes: ['', Validators.required],
    })
    this.busPackageForm.get('state')?.disable();
  }

  onFileChange(event: any) {
    this.selectedFiles = event.target.files[0];
  }

  getCountries() {
    this.httpSer.httpGet(API_URLS.getCountries).subscribe((res: any) => {
      this.countryList = res.data;
    })
  }

  getStatesByCountry(ID: number) {
    this.httpSer.httpGet(API_URLS.getStatesByCountryID, { countryID: ID }).subscribe((res: any) => {
      this.stateList = res.data;
      this.busPackageForm.get('state')?.enable(); 
    })
  }

  getCities() {
    const localCityList = localStorage.getItem('localCityList');
    if (localCityList) {
      this.cityList = localCityList ? JSON.parse(localCityList) : [];
    }
  }

  getCityPairs() {
    this.httpService.httpGet(API_URLS.CITY_PAIRS).subscribe((res: any) => {
      this.cityPairList = this.apiConverterService.mapData(res);
      if (this.cityPairList.length > 0) {
        localStorage.setItem('localCityPairsList', JSON.stringify(this.cityPairList))
      }
    })
  }

  getStages() {
    this.httpService.httpGet(API_URLS.STAGES).subscribe((res: any) => {
      this.stageList = this.apiConverterService.mapData(res);
      if (this.stageList.length > 0) {
        localStorage.setItem('localStageList', JSON.stringify(this.stageList))
      }
    })
  }

  selectItem(value: any, controlName: string) {
    console.log(`Selected ${controlName}:`, value);

    this.busPackageForm.patchValue({ [controlName]: value.name });

    if (controlName === 'departure') {
      this.selectedDepartureID = value.id;
      this.busPackageForm.patchValue({ departure: value.name });
    } else if (controlName === 'destination') {
      this.selectedDestinationID = value.id;
      this.busPackageForm.patchValue({ destination: value.name });
    }
  }

  filterItem(controlName: string, param: string) {
    const value = this.busPackageForm.controls[controlName].value;

    if (param === "countryParam" && value) {
      this.busPackageForm.patchValue({ 'state': '' });
      this.filteredStates = [];
      this.selectedStateID = null;
      this.busPackageForm.get('state')?.disable();
      const filteredData = this.countryList.filter((item: any) =>
        item.name.toLowerCase().startsWith(value.toLowerCase())
      );
      this.filteredCountries = filteredData.length ? filteredData : [''];
    }

    if (param === "stateParam" && value) {
      const filteredData = this.stateList.filter((item: any) =>
        item.name.toLowerCase().startsWith(value.toLowerCase())
      );
      this.filteredStates = filteredData.length ? filteredData : [''];
    }

    if (param === "cityParam" && value) {
      const filteredCities = this.cityList.filter((city: any) =>
        city.name.toLowerCase().startsWith(value.toLowerCase())
      );
      if (controlName === 'departure') {
        this.filteredDepartureCities = filteredCities.length ? filteredCities : [''];
      } else if (controlName === 'destination') {
        this.filteredDestinationCities = filteredCities.length ? filteredCities : [''];
      }
    }
  }

  //   Submit() {
  //     if (!this.formValidation.validateForm(this.busPackageForm)) {
  //         this.busPackageForm.markAllAsTouched();
  //         return;
  //     } else {
  //         const param = {
  //             "flag": "I",
  //             "packageID": 0,
  //             "packageName": this.busPackageForm.controls['packageName'].value,
  //             "state": this.selectedStateID ? Number(this.selectedStateID) : 0,
  //             "country": this.selectedCountryID ? Number(this.selectedCountryID) : 0,
  //             "from": this.selectedDepartureID, 
  //             "to": this.selectedDestinationID, 
  //             "noofdays": Number(this.busPackageForm.controls['noOfDays'].value),
  //             "shortdescription": this.busPackageForm.controls['shortDescription'].value,
  //             "description": this.busPackageForm.controls['description'].value,
  //             "additionalNotes": this.busPackageForm.controls['additionalNotes'].value,
  //             "packagePrice": Number(this.busPackageForm.controls['packagePrice'].value),
  //             "createdBy": this.userService.userInfo.userId
  //         }
  //         this.httpSer.httpPost(API_URLS.POST_BUS_PACKAGE, param).subscribe((res: any) => {
  //             if (res.type === "S") {
  //                 this.packageGenerated.emit(res.data.packageID);
  //                 this.formValidation.showAlert('Successfully Submitted', 'success');
  //                 this.formValidation.setPackageID(res.data.packageID);
  //                 if (res.data.packageID) {
  //                     this.addAttachment(res.data.packageID);
  //                 }
  //             } else {
  //                 this.formValidation.showAlert('Error!', 'danger');
  //             }
  //         })
  //     }
  // }

  Submit() {
    if (!this.formValidation.validateForm(this.busPackageForm)) {
      this.busPackageForm.markAllAsTouched();
      return;
    }
    if (!this.selectedFiles) {
      alert('Select file.');
      return;
    }

    const createdBy = this.userService.userInfo?.userId ?? 0;

    const param = {
      flag: "I",
      packageID: 0,
      packageName: this.busPackageForm.controls['packageName'].value,
      state: this.selectedStateID ? Number(this.selectedStateID) : 0,
      country: this.selectedCountryID ? Number(this.selectedCountryID) : 0,
      from: this.selectedDepartureID,
      to: this.selectedDestinationID,
      noofdays: Number(this.busPackageForm.controls['noOfDays'].value),
      shortdescription: this.busPackageForm.controls['shortDescription'].value,
      description: this.busPackageForm.controls['description'].value,
      additionalNotes: this.busPackageForm.controls['additionalNotes'].value,
      packagePrice: Number(this.busPackageForm.controls['packagePrice'].value),
      createdBy: createdBy
    };

    this.httpSer.httpPost(API_URLS.POST_BUS_PACKAGE, param).subscribe((res: any) => {
      if (res.type === "S") {
        this.packageGenerated.emit(res.data.packageID);
        this.formValidation.showAlert('Successfully Submitted!', 'success');
        this.formValidation.setPackageID(res.data.packageID);
        if (res.data.packageID) {
          this.addAttachment(res.data.packageID);
        }
      } else {
        this.formValidation.showAlert('Error!', 'danger');
      }
    });
  }

  addAttachment(id: any) {
    if (!this.selectedFiles) {
      alert('Select file.');
      return;
    }
    const formData = new FormData();
    formData.append('UserID', this.userService.userInfo.userId);
    formData.append('PackageID', id);
    formData.append('Section', 'Bus_Package_Section');
    formData.append('Sortorder', id);
    formData.append('AttachmentName', 'Bus_Package_AttachmentName');
    formData.append('CreatedBy', this.userService.userInfo.userId);
    formData.append('file', this.selectedFiles, this.selectedFiles.name);

    this.http.post(API_URLS.BASE_URL + '/Attachment/addFile', formData).subscribe(
      (response: any) => {
        if (response.attachId) {
          this.formValidation.showAlert('Successfully Submitted', 'success');
          this.busPackageForm.reset();
          this.selectedFiles = undefined;
        }
      },
      (error: any) => {
        console.error('Error uploading files', error);
      }
    );
  }
}


