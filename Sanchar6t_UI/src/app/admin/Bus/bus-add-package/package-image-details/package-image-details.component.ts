import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormValidationService } from '../../../../services/form-validation.service';
import { HttpServiceService } from '../../../../services/http-service.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { API_URLS } from '../../../../shared/API-URLs';
import { UserServiceService } from '../../../../services/user-service.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-package-image-details',
  templateUrl: './package-image-details.component.html',
  styleUrls: ['./package-image-details.component.scss', '../bus-add-package.component.scss'],
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
export class PackageImageDetailsComponent {
  @Input() packageID: any;
  selectedFiles: File | undefined;
  imagePreviewUrl: string | null = null;
  packageImageDtlsForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: UserServiceService,
    private httpC: HttpClient,
    private http: HttpServiceService,
    private formValidation: FormValidationService
  ) {}

  ngOnInit() {
    this.formInit();
    this.packageID = this.formValidation.getPackageID();
    this.fetchPackageImageDetails();
  }

  formInit() {
    this.packageImageDtlsForm = this.fb.group({
      uploadPackageBannerImage: ['', Validators.required],
      heading: ['', Validators.required],
      subHeading: ['', Validators.required],
      calltoActionTitle: ['', Validators.required],
      calltoActionUrl: ['', Validators.required]
    });
  }

  onFileChange(event: any) {
    this.selectedFiles = event.target.files[0];
  }

  Submit() {
    if (!this.formValidation.validateForm(this.packageImageDtlsForm)) {
      this.packageImageDtlsForm.markAllAsTouched();
      return;
    }

    if (!this.selectedFiles) {
      alert('Select file.');
      return;
    }

    const formData = new FormData();
    formData.append('UserID', this.userService.userInfo.userId);
    formData.append('PackageID', this.packageID);
    formData.append('Section', 'Bus_Package_Section');
    formData.append('Sortorder', this.packageID);
    formData.append('AttachmentName', 'Bus_Package_AttachmentName');
    formData.append('CreatedBy', this.userService.userInfo.userId);
    formData.append('file', this.selectedFiles, this.selectedFiles.name);

    this.httpC.post(API_URLS.BASE_URL + '/Attachment/addFile', formData).subscribe(
      (response: any) => {
        console.log('Upload response:', response);
        const fileName: string = response?.fileName || '';
        const attachId = this.extractAttachId(fileName);

        if (attachId) {
          const fileUrl = `${API_URLS.BASE_URL}/Attachment/View/${attachId}`;
          this.savePackageImage(fileUrl);
        } else {
          this.formValidation.showAlert('Upload failed: invalid filename', 'danger');
        }
      },
      (error: any) => {
        console.error('Error uploading files', error);
      }
    );
  }

  extractAttachId(fileName: string): number | null {
    if (!fileName) return null;
    const match = fileName.match(/^(\d+)_/); // Extracts number before underscore
    return match ? Number(match[1]) : null;
  }

  savePackageImage(fileUrl: string) {
    const param = {
      flag: 'I',
      pkgImageID: 0,
      packageID: this.packageID || 0,
      pkgImage: fileUrl,
      pkgSection: 'string',
      heading: this.packageImageDtlsForm.controls['heading'].value,
      subHeading: this.packageImageDtlsForm.controls['subHeading'].value,
      btnName: this.packageImageDtlsForm.controls['calltoActionTitle'].value,
      btnUrl: this.packageImageDtlsForm.controls['calltoActionUrl'].value,
      isActive: true,
      createdBy: 1
    };

    this.http.httpPost(API_URLS.save_PACKAGE_IMG_DETAILS, param).subscribe((res: any) => {
      if (res.type === 'S') {
        this.formValidation.showAlert('Successfully Submitted', 'success');
        this.packageImageDtlsForm.reset();
        this.selectedFiles = undefined;
        this.imagePreviewUrl = fileUrl; // Show preview immediately
      } else {
        this.formValidation.showAlert('Error saving image details', 'danger');
      }
    });
  }

  fetchPackageImageDetails() {
    const param = {
      flag: 'S',
      packageID: this.packageID
    };

    this.http.httpPost(API_URLS.get_GetpkgImageDtls, param).subscribe((res: any) => {
      if (res.type === 'S' && res.data && res.data.length > 0) {
        const imgData = res.data[0];
        this.packageImageDtlsForm.patchValue({
          heading: imgData.heading,
          subHeading: imgData.subHeading,
          calltoActionTitle: imgData.btnName,
          calltoActionUrl: imgData.btnUrl
        });
        this.imagePreviewUrl = imgData.pkgImage;
      }
    });
  }
}
