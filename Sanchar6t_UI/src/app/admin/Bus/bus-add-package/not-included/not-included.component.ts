import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormValidationService } from '../../../../services/form-validation.service';
import { HttpServiceService } from '../../../../services/http-service.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { API_URLS } from '../../../../shared/API-URLs';

@Component({
  selector: 'app-not-included',
  templateUrl: './not-included.component.html',
  styleUrls: ['./not-included.component.scss', '../bus-add-package.component.scss'],
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

export class NotIncludedComponent {
  isNotIncluded: boolean = false;
  notIncludedForm!: FormGroup
  selectedRow: any = null;
  notIncludedList: any[] = [];
  packageID: number | null = null;

  constructor(private fb: FormBuilder, public httpSer: HttpServiceService, private formValidation: FormValidationService) {
  }

  ngOnInit() {
    this.formInit();
    this.getNotIncludedList();
    this.packageID = this.formValidation.getPackageID();
  }

  formInit() {
    this.notIncludedForm = this.fb.group({
      notIncluded: ['', Validators.required],
    })
  }

  getNotIncludedList() {
    this.httpSer.httpGet(API_URLS.GET_INCLUDED_LIST).subscribe((res: any) => {
      this.notIncludedList = res.data?.filter((item: any) => item.isIncluded == false && item.isActive == true);
    })
  }

  edit(item: any) {
    this.notIncludedForm.patchValue({
      notIncluded: item.description,
    });
    this.isNotIncluded = true;
    this.selectedRow = item;
  }

  confirmDelete(item: any) {
    this.selectedRow = item;
    this.Submit('delete');
  }

  Submit(value: any) {
    let param: any;
    if (value == 'submit' || value == 'update') {
      if (!this.formValidation.validateForm(this.notIncludedForm)) {
        this.notIncludedForm.markAllAsTouched();
        setTimeout(() => { }, 0);
        return;
      }
      param = {
        "flag": this.selectedRow ? "U" : "I",
        "pkgIncludeID": this.selectedRow ? this.selectedRow.pkgIncludeId : 0,
        "packageID": this.packageID || 0,
        "description": this.notIncludedForm.controls['notIncluded'].value,
        "isIncluded": false,
        "isActive": true,
        "createdBy": 1
      }
    }
    else if (value == 'delete' && this.selectedRow) {
      param = {
        "flag": "D",
        "pkgIncludeID": this.selectedRow ? this.selectedRow.pkgIncludeId : 0,
        "packageID": this.packageID || 0,
        "description": '',
        "isIncluded": false,
        "isActive": false,
        "createdBy": 1
      }
    }

    this.httpSer.httpPost(API_URLS.POST_INCLUDED_LIST, param).subscribe((res: any) => {
      if (res.type == "S") {
        if (value == 'submit' || value == 'update') {
          this.formValidation.showAlert(this.selectedRow ? 'Successfully Updated!' : 'Successfully Submitted!', 'success');
        }
        else if (value == 'delete') {
          this.formValidation.showAlert('Successfully Deleted!', 'success');
        }
        this.notIncludedForm.reset();
        this.selectedRow = null;
        this.getNotIncludedList();
      }
      else {
        this.formValidation.showAlert('Error!', 'danger');
      }
    })
  }

}
