import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormValidationService } from '../../../../services/form-validation.service';
import { HttpServiceService } from '../../../../services/http-service.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { API_URLS } from '../../../../shared/API-URLs';

@Component({
  selector: 'app-highlights',
  templateUrl: './highlights.component.html',
  styleUrls: ['./highlights.component.scss', '../bus-add-package.component.scss'],
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

export class HighlightsComponent {
  isHighlightInputOpen: boolean = false;
  highlightsForm!: FormGroup
  highlightList: any[] = [];
  selectedHighlight: any = null;
  @Input() packageID: number | null = null;


  constructor(private fb: FormBuilder, private http: HttpServiceService, private formValidation: FormValidationService) {
  }

  ngOnInit() {
    this.formInit();
    this.getHighlightList();
    this.packageID = this.formValidation.getPackageID();
  }

  formInit() {
    this.highlightsForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
    })
  }

  getHighlightList() {
    this.http.httpGet(API_URLS.Get_HIGHLIGHTS).subscribe((res: any) => {
      this.highlightList = res.data?.filter((item: any) => item.isActive == true && this.packageID == item.packageId);
    })
  }

  edit(item: any) {
    this.highlightsForm.patchValue({
      title: item.title,
      description: item.description,
    });
    this.isHighlightInputOpen = true;
    this.selectedHighlight = item;
  }

  confirmDelete(item: any) {
    this.selectedHighlight = item;
    this.Submit('delete');
  }

  // Submit(value: any) {
  //   let param: any;
  //   if (value == 'submit' || value == 'update') {
  //     if (!this.formValidation.validateForm(this.highlightsForm)) {
  //       this.highlightsForm.markAllAsTouched();
  //       setTimeout(() => { }, 0);
  //       return;
  //     }
  //     param = {
  //       "flag": this.selectedHighlight ? "U" : "I",
  //       "pkgHighlightID": this.selectedHighlight ? this.selectedHighlight.pkgHighlightId : 0,
  //       "packageID": this.packageID || 0,
  //       "title": this.highlightsForm.controls['title'].value,
  //       "description": this.highlightsForm.controls['description'].value,
  //       "isActive": true,
  //       "createdBy": 1
  //     }
  //   }
  //   else if (value == 'delete' && this.selectedHighlight) {
  //     param = {
  //       "flag": 'D',
  //       "pkgHighlightID": this.selectedHighlight ? this.selectedHighlight.pkgHighlightId : 0,
  //       "packageID": this.packageID || 0,
  //       "title": '',
  //       "description": '',
  //       "isActive": false,
  //       "createdBy": 1
  //     }
  //   }
  //   this.http.httpPost(API_URLS.POST_HIGHLIGHTS, param).subscribe((res: any) => {
  //     if (res.type == "S") {
  //       if (value == 'submit' || value == 'update') {
  //         this.formValidation.showAlert(this.selectedHighlight ? 'Successfully Updated' : 'Successfully Submitted', 'success');
  //         this.getHighlightList();
  //       }
  //       else if (value == 'delete') {
  //         this.formValidation.showAlert('Successfully Deleted!', 'success');
  //         this.getHighlightList();
  //       }
  //       this.highlightsForm.reset();
  //       this.selectedHighlight = null;
  //     }
  //     else {
  //       this.formValidation.showAlert('Error!', 'danger');
  //     }
  //   })
  // }


  Submit(value: any) {
    let param: any;

    if (value === 'submit' || value === 'update') {
      if (!this.formValidation.validateForm(this.highlightsForm)) {
        this.highlightsForm.markAllAsTouched();
        return;
      }

      if (!this.packageID) {
        alert('PackageID is missing!');
        return;
      }

      param = {
        flag: this.selectedHighlight ? "U" : "I",
        pkgHighlightID: this.selectedHighlight ? this.selectedHighlight.pkgHighlightId : 0,
        packageID: this.packageID,
        title: this.highlightsForm.controls['title'].value,
        description: this.highlightsForm.controls['description'].value,
        isActive: true,
        createdBy: 1
      };

    } else if (value === 'delete' && this.selectedHighlight) {
      param = {
        flag: 'D',
        pkgHighlightID: this.selectedHighlight ? this.selectedHighlight.pkgHighlightId : 0,
        packageID: this.packageID,
        title: '',
        description: '',
        isActive: false,
        createdBy: 1
      };
    }

    this.http.httpPost(API_URLS.POST_HIGHLIGHTS, param).subscribe((res: any) => {
      if (res.type === "S") {

        if (value === 'submit' || value === 'update') {
          const newItem = {
            pkgHighlightId: res.data?.pkgHighlightId || 0, 
            packageId: this.packageID,
            title: this.highlightsForm.controls['title'].value,
            description: this.highlightsForm.controls['description'].value,
            isActive: true
          };

          if (!this.selectedHighlight) {
            this.highlightList.push(newItem);
          } else {
            const index = this.highlightList.findIndex(x => x.pkgHighlightId === this.selectedHighlight.pkgHighlightId);
            if (index !== -1) this.highlightList[index] = newItem;
          }

          this.formValidation.showAlert(this.selectedHighlight ? 'Successfully Updated' : 'Successfully Submitted', 'success');

        }
        else if (value === 'delete') {
          this.highlightList = this.highlightList.filter(x => x.pkgHighlightId !== this.selectedHighlight.pkgHighlightId);
          this.formValidation.showAlert('Successfully Deleted!', 'success');
        }

        this.highlightsForm.reset();
        this.selectedHighlight = null;

      } else {
        this.formValidation.showAlert('Error!', 'danger');
      }
    });
  }


}
