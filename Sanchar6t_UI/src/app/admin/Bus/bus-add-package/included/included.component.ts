import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormValidationService } from '../../../../services/form-validation.service';
import { HttpServiceService } from '../../../../services/http-service.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { API_URLS } from '../../../../shared/API-URLs';

@Component({
  selector: 'app-included',
  templateUrl: './included.component.html',
  styleUrls: ['./included.component.scss', '../bus-add-package.component.scss'],
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

export class IncludedComponent {
  isIncluded: boolean = false;
  includedForm!: FormGroup
  includedList: any[] = [];
  packageID: number | null = null;
  selectedRow: any = null;

  constructor(private fb: FormBuilder, private httpSer: HttpServiceService, private formValidation: FormValidationService) {
  }

  ngOnInit() {
    this.formInit();
    this.getIncludedList();
    this.packageID = this.formValidation.getPackageID();
  }

  formInit() {
    this.includedForm = this.fb.group({
      included: ['', Validators.required],
    })
  }

  getIncludedList() {
    this.httpSer.httpGet(API_URLS.GET_INCLUDED_LIST).subscribe((res: any) => {
      this.includedList = res.data?.filter((item: any) => item.isIncluded == true && item.isActive == true&&
      this.packageID == item.packageId)
    })
  }

  edit(item: any) {
    this.includedForm.patchValue({
      included: item.description,
    });
    this.isIncluded = true;
    this.selectedRow = item;
  }

  confirmDelete(item: any) {
    this.selectedRow = item;
    this.Submit('delete');
  }

  Submit(value: any) {
    let param: any;
    if (value == 'submit' || value == 'update') {
      if (!this.formValidation.validateForm(this.includedForm)) {
        this.includedForm.markAllAsTouched();
        setTimeout(() => { }, 0);
        return;
      }
      param = {
        "flag": this.selectedRow ? "U" : "I",
        "pkgIncludeID": this.selectedRow ? this.selectedRow.pkgIncludeId : 0,
        "packageID": this.packageID || 0,
        "description": this.includedForm.controls['included'].value,
        "isIncluded": true,
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
        "isIncluded": true,
        "isActive": false,
        "createdBy": 1
      }
    }

    this.httpSer.httpPost(API_URLS.POST_INCLUDED_LIST, param).subscribe((res: any) => {
      if (res.type == "S") {
        if (value == 'submit' || value == 'update'){
          this.formValidation.showAlert(this.selectedRow ? 'Successfully Updated!' : 'Successfully Submitted!', 'success');
        }
        else if (value == 'delete'){
          this.formValidation.showAlert('Successfully Deleted!', 'success');
        }
        this.includedForm.reset();
        this.selectedRow = null;
        this.getIncludedList();
      }
      else {
        this.formValidation.showAlert('Error!', 'danger');
      }
    })
  }
}
