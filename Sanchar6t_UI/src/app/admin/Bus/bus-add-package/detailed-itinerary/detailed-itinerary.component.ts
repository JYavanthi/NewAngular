import { Component, Input, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormValidationService } from '../../../../services/form-validation.service';
import { HttpServiceService } from '../../../../services/http-service.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { API_URLS } from '../../../../shared/API-URLs';
import { UserServiceService } from '../../../../services/user-service.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-detailed-itinerary',
  templateUrl: './detailed-itinerary.component.html',
  styleUrls: ['./detailed-itinerary.component.scss', '../bus-add-package.component.scss'],
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
export class DetailedItineraryComponent {
  selectedFiles: File | undefined;
  isDetailedItinerary: boolean = false;
  detailedItineraryForm!: FormGroup;
  selectedRow: any = null;
  itineraryList: any[] = [];
  attachmentID!: number | null;
  packageID: number | null = null;

  constructor(private fb: FormBuilder, private userService: UserServiceService, private formValidation: FormValidationService, private http: HttpServiceService, private httpC: HttpClient) {
  }

  onFileChange(event: any) {
    this.selectedFiles = event.target.files[0];
  }

  ngOnInit() {
    this.formInit();
    this.getItineraryList();
    this.packageID = this.formValidation.getPackageID();
  }

  formInit() {
    this.detailedItineraryForm = this.fb.group({
      uploadItineraryImage: ['', Validators.required],
      day: [null, [Validators.required, Validators.min(1)]],
      title: ['', Validators.required],
      from: ['', Validators.required],
      to: ['', Validators.required],
      itineraryDescription: ['', Validators.required],
    })
  }

  edit(item: any) {
    this.detailedItineraryForm.patchValue({
      day: item.day,
      title: item.title,
      from: item.fromTime.split('T')[1].split(':').slice(0, 2).join(':'),
      to: item.toTime.split('T')[1].split(':').slice(0, 2).join(':'),
      itineraryDescription: item.description
    });
    this.isDetailedItinerary = true;
    this.selectedRow = item;
  }

  confirmDelete(item: any) {
    this.selectedRow = item;
    this.submit('delete');
  }

  getItineraryList() {
    this.http.httpGet(API_URLS.Get_Itinerary_List).subscribe((res: any) => {
      this.itineraryList = res.data?.filter((item: any) => item.isActive == true && item.packageId == this.packageID);
    })
  }

  submit(value: any) {
    let param: any;
    if (value == 'submit' || value == 'update') {
      if (!this.formValidation.validateForm(this.detailedItineraryForm)) {
        this.detailedItineraryForm.markAllAsTouched();
        setTimeout(() => { }, 0);
        return;
      }
      param = {
        "flag": this.selectedRow ? "U" : "I",
        "pkgItineraryID": this.selectedRow ? this.selectedRow.pkgItineraryId : 0,
        "packageID": this.packageID || 0,
        "day": this.detailedItineraryForm.controls['day'].value.toString(),
        "fromTime": "2025-03-22T11:26:28.617Z",
        "toTime": "2025-03-22T11:26:28.617Z",
        "title": this.detailedItineraryForm.controls['title'].value,
        "description": this.detailedItineraryForm.controls['itineraryDescription'].value,
        "isActive": true,
        "createdBy": this.userService.userInfo.userId
      }
    }
    else if (value == 'delete' && this.selectedRow) {
      param = {
        "flag": "D",
        "pkgItineraryID": this.selectedRow ? this.selectedRow.pkgItineraryId : 0,
        "packageID": this.packageID || 0,
        "day": '',
        "fromTime": "2025-03-22T11:26:28.617Z",
        "toTime": "2025-03-22T11:26:28.617Z",
        "title": '',
        "description": '',
        "isActive": false,
        "createdBy": this.userService.userInfo.userId
      }
    }
    this.http.httpPost(API_URLS.POST_ITINERARY, param).subscribe((res: any) => {
      if (res.type == "S") {
        if (value == 'submit' || value == 'update') {
          this.addAttachment(this.packageID);
          this.getItineraryList()
        }
        else if (value == 'delete') {
          this.getAttachmentID(this.selectedRow.packageId);
          this.getItineraryList()
        }
      }
      else {
        this.formValidation.showAlert('Error!', 'danger');
      }
    })
  }

  addAttachment(id: any) {
    if (!this.selectedFiles) {
      alert('Select file.')
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

    this.httpC.post(API_URLS.BASE_URL + '/Attachment/addFile', formData).subscribe(
      (response: any) => {
        if (response.attachId) {
          this.formValidation.showAlert('Successfully Submitted!', 'success');
          this.detailedItineraryForm.reset();
          this.selectedFiles = undefined;
          this.selectedRow = null;
          this.getItineraryList();
        }
      },
      (error: any) => {
        console.error('Error uploading files', error);
      }
    );
  }

  getAttachmentID(ID: number) {
    const param = {
      PackageID: ID,
      Section: 'Bus_Package_Section'
    }
    this.http.httpGet(API_URLS.get_AttachmentID, param).subscribe((res: any) => {
      // this.selectedFiles['name']=res[0].attachmentFile
      this.deleteAttachment(res[0].attachmentId)
    })
  }

  deleteAttachment(attachmentID: any) {
    if (attachmentID) {
      this.http.httpDelete(API_URLS.delete_Attachment + '/' + attachmentID).subscribe((res: any) => {
        this.formValidation.showAlert('Successfully Deleted!', 'success');
        this.getItineraryList();
        this.selectedRow = null;
      })
    }
    else {
      this.formValidation.showAlert('Error!', 'danger');
    }
  }

}
