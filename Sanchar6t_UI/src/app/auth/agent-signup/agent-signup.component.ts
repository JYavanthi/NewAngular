import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { FormValidationService } from '../../services/form-validation.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { HttpServiceService } from '../../services/http-service.service';
import { API_URLS } from '../../shared/API-URLs';
import { forkJoin, of } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';
@Component({
  selector: 'app-agent-signup',
  templateUrl: './agent-signup.component.html',
  styleUrl: './agent-signup.component.scss'
})
export class AgentSignupComponent {
  userForm: FormGroup;
  filteredDepartureCities: any[] = [];
  filteredDestinationCities: any[] = [];
  httpService: any;
  stageList: any;
  apiConverterService: any;
  cityPairList: any;
  selectedDestinationID!: number;
  selectedDepartureID: any;
  fileName: any;
  editId: number | null = null;
  inFile: File | null = null;
  outFile: File | null = null;
  officeFile: File | null = null;
  inAttachments: any[] = [];
  outAttachments: any[] = [];
  officeAttachments: any[] = [];
stateList: any[] = [];
cityList: any[] = [];

  constructor(private fb: FormBuilder, private formValidation: FormValidationService, private http: HttpServiceService, private router: Router, private route: ActivatedRoute) {
    this.userForm = this.fb.group({
      name: ['', Validators.required],
      LastName: ['', Validators.required],
      phone: [null, [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      email: ['', [Validators.required, Validators.email]],
      organization: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      Comments: ['', [Validators.required]],
      CompanyName: ['', Validators.required],
      CompanyAddress: ['', Validators.required],
      gstNo: ['', Validators.required],
      officename: ['', Validators.required],
      Out: ['', Validators.required],
      In: ['', Validators.required],
      ShopAddress: ['', Validators.required],
      password: ['', Validators.required],
      status: ['', Validators.required],
      registeredCompanyName: [''],
      gender: [''],
      toggleGST: [false]
    });
    this.userForm.get('toggleGST')?.valueChanges.subscribe((value) => {
    });
  }


ngOnInit() {
  this.loadStates();   // ✅ always load states

  const id = this.route.snapshot.params['id'];
  if (id) {
    this.editId = id;
    this.getAgentById(id);
    this.getAttachments(id);
  }

   this.userForm.get('state')?.valueChanges.subscribe((stateId) => {
    if (stateId) {
      this.loadCities(stateId);
    } else {
      this.cityList = [];
      this.userForm.get('city')?.reset();
    }
  });
}
  // getAgentById(id: number) {
  //   this.http.httpGet(`/AgentDtls/GetAgentDtlsById/${id}`).subscribe((res: any) => {
  //     const a = res.data ?? res;

  //     this.userForm.patchValue({
  //       name: a.firstName,
  //       LastName: a.lastName,
  //       phone: a.contactNo,
  //       email: a.email,
  //       organization: a.organisation,
  //       city: a.city,
  //       state: a.state,
  //       Comments: a.comments,
  //       CompanyName: a.companyName,
  //       CompanyAddress: a.companyAddress,
  //       gstNo: a.gst,
  //       ShopAddress: a.shopAddress,
  //       status: a.status
  //     });
  //   });
  // }
 

 getAgentById(agentDtlId: number) {
  this.http
    .httpGet(`/AgentDtls/GetAgentDtlsByAgentId/${agentDtlId}`)
    
    .subscribe({
      next: (res: any) => {
        const a = res; // your API returns direct object, not wrapped

        this.editId = agentDtlId;

        this.userForm.patchValue({
          name: a.firstName,
          LastName: a.lastName,
          phone: a.contactNo,
          email: a.email,
          organization: a.organisation,
          city: a.city,
          state: a.state,
          Comments: a.comments,
          CompanyName: a.companyName,
          CompanyAddress: a.companyAddress,
          gstNo: a.gst,
          ShopAddress: a.shopAddress,
          status: a.status,
          gender: a.gender
        });

        // load attachments
        this.getAttachments(agentDtlId);
      },
      error: err => {
        console.error('GetAgentDtlsByagentDtlId failed', err);
      }
    });
}

  getAttachments(agentDtlId: number) {

    // In
    this.http.httpGet(`/Attachment/GetFileData`,
      { PackageID: agentDtlId, Section: 'In' }
    ).subscribe((res: any) => {
      this.inAttachments = res;
    });

    // Out
    this.http.httpGet(`/Attachment/GetFileData`,
      { PackageID: agentDtlId, Section: 'Out' }
    ).subscribe((res: any) => {
      this.outAttachments = res;
    });

    // Office
    this.http.httpGet(`/Attachment/GetFileData`,
      { PackageID: agentDtlId, Section: 'Office' }
    ).subscribe((res: any) => {
      this.officeAttachments = res;
    });

  }

  getFileUrl(id: number) {
    return `${API_URLS.BASE_URL}/Attachment/View/${id}`;
  }


  Submit() {
    if (!this.formValidation.validateForm(this.userForm)) {
      this.userForm.markAllAsTouched();
      setTimeout(() => { }, 0);
      return;
    } else {
      const param = {
        flag: this.editId ? "U" : "I",
        userID: this.editId ? Number(this.editId) : 0,
        userType: 3,
        status: this.userForm.controls['status'].value || "",
        password: this.userForm.controls['password'].value,
        firstName: this.userForm.controls['name'].value,
        middleName: this.userForm.controls['middleName']?.value || "",
        lastName: this.userForm.controls['LastName'].value,
        email: this.userForm.controls['email'].value,
        contactNo: this.userForm.controls['phone'].value || "", // must be string
        gender: this.userForm.controls['gender'].value,
        aadharNo: this.userForm.controls['AadharNo']?.value || "",
        pancardNo: this.userForm.controls['PancardNo']?.value || "",
        bloodGroup: this.userForm.controls['BloodGroup']?.value || "",
        primaryUser: true,
        age: this.userForm.controls['Age']?.value || "",
        address: this.userForm.controls['Address']?.value || "",
        alternativeNumber: this.userForm.controls['AlternativeNumber']?.value || "",
        remarks: this.userForm.controls['Remarks']?.value || "",
        companyName: this.userForm.controls['CompanyName'].value,
        companyID: 0,
        companyAddress: this.userForm.controls['CompanyAddress'].value,
        shopAddress: this.userForm.controls['ShopAddress'].value,
        organisation: this.userForm.controls['organization'].value,
        city: this.userForm.controls['city'].value,
        state: this.userForm.controls['state'].value,
        comments: this.userForm.controls['Comments'].value,
        gst: this.userForm.controls['gstNo'].value,
        amount: '0',
        type: '',
        transactionLimit: '',
        createdBy: Number(JSON.parse(localStorage.getItem('user') ?? '{}')?.UserId ?? null)
      };

      this.http.httpPost('/User/PostUser', param).subscribe((res: any) => {

        if (res.type === "S") {

          let userId = 0;

          // ⛳ EDIT (ALWAYS FORCE editId)
          if (this.editId) {
            userId = Number(this.editId);
          }

          // 🆕 INSERT get id from response
          if (!userId) {
            userId = res?.data?.userId || res?.data?.UserId || 0;
          }

          console.log("✔ FINAL USER ID:", userId);

          // upload list
          const uploadTasks = [];

          if (this.inFile) {
            uploadTasks.push(this.uploadAttachment(userId, 'In', this.inFile));
          }

          if (this.outFile) {
            uploadTasks.push(this.uploadAttachment(userId, 'Out', this.outFile));
          }

          if (this.officeFile) {
            uploadTasks.push(this.uploadAttachment(userId, 'Office', this.officeFile));
          }

          Promise.all(uploadTasks.map(x => x.toPromise()))
            .finally(() => {
              this.formValidation.showAlert('Successfully Submitted', 'success');
              this.router.navigate(['/home']);
            });

        }
        else {
          this.formValidation.showAlert('Error!!', 'danger');
        }

      });
    }
  }
uploadAttachment(agentDtlId: number, section: string, file: File) {
  const formData = new FormData();

  formData.append('file', file);
  formData.append('PackageID', agentDtlId.toString());
  formData.append('Section', section);
  formData.append('AttachmentName', file.name);
  formData.append('Sortorder', '0');
  formData.append('CreatedBy', agentDtlId.toString());

  return this.http.httpPost('/Attachment/addFile', formData);
}



  
  onFileChange(event: any, type: string) {
    const file = event.target.files[0];
    if (type == 'In') this.inFile = file;
    if (type == 'Out') this.outFile = file;
    if (type == 'Office') this.officeFile = file;
  }

  

  onClear() {
    this.userForm.reset();
  }

loadStates() {
  const headers = new HttpHeaders().set('Country-Id', '101');

  this.http.httpGet('/State/GetState', { headers })
    .subscribe({
      next: (res: any) => {
        console.log('RAW RESPONSE 👉', res);

        // ✅ handle wrapped response
        this.stateList = res.data ?? res;

        console.log('STATE LIST 👉', this.stateList);
      },
      error: (err) => {
        console.error('Error loading states ❌', err);
      }
    });
}

loadCities(stateId: number) {
  this.http.httpGet('/Cities/GetCitiesByState', { stateId })
    .subscribe({
      next: (res: any) => {
        console.log('CITY RESPONSE 👉', res);

        this.cityList = res.data ?? res;
      },
      error: (err) => {
        console.error('Error loading cities ❌', err);
      }
    });
}

}
