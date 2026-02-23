import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormValidationService } from '../../../../services/form-validation.service';
import { HttpServiceService } from '../../../../services/http-service.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { API_URLS } from '../../../../shared/API-URLs';

@Component({
  selector: 'app-important-notes',
  templateUrl: './important-notes.component.html',
  styleUrls: ['./important-notes.component.scss', '../bus-add-package.component.scss'],
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

export class ImportantNotesComponent {
  isImportantNotes: boolean = false;
  importantNotesForm!: FormGroup
  selectedRow: any = null;
  importantNoteList: any[] = [];
  packageID: number | null = null;

  constructor(private fb: FormBuilder, private httpSer: HttpServiceService, private formValidation: FormValidationService) {
  }

  ngOnInit() {
    this.formInit();
    this.getImportantNoteList();
    this.packageID = this.formValidation.getPackageID();
  }

  formInit() {
    this.importantNotesForm = this.fb.group({
      importantNotes: ['', Validators.required],
    })
  }

  edit(item: any) {
    this.importantNotesForm.patchValue({
      importantNotes: item.description,
    });
    this.isImportantNotes = true;
    this.selectedRow = item;
  }

  confirmDelete(item: any) {
    this.selectedRow = item;
    this.Submit('delete');
  }

  getImportantNoteList() {
    this.httpSer.httpGet(API_URLS.GET_IMP_NOTES).subscribe((res: any) => {
      this.importantNoteList = res.data?.filter((item: any) => item.isActive == true && this.packageID == item.packageId)
    })
  }

  Submit(value: any) {
    let param: any;
    if (value == 'submit' || value == 'update') {
      if (!this.formValidation.validateForm(this.importantNotesForm)) {
        this.importantNotesForm.markAllAsTouched();
        setTimeout(() => { }, 0);
        return;
      }
      param = {
        "flag": this.selectedRow ? "U" : "I",
        "pkgImpNoteID": this.selectedRow ? this.selectedRow.pkgImpNoteId : 0,
        "packageID": this.packageID || 0,
        "description": this.importantNotesForm.controls['importantNotes'].value,
        "isActive": true,
        "createdBy": 1
      }
    }
    else if (value == 'delete' && this.selectedRow) {
      param = {
        "flag": "D",
        "pkgImpNoteID": this.selectedRow ? this.selectedRow.pkgImpNoteId : 0,
        "packageID": this.packageID || 0,
        "description": '',
        "isActive": false,
        "createdBy": 1
      }
    }
    this.httpSer.httpPost(API_URLS.POST_IMP_NOTES, param).subscribe((res: any) => {
      if (res.type == "S") {
        if (value == 'submit' || value == 'update') {
          this.formValidation.showAlert(this.selectedRow ? 'Successfully Updated!' : 'Successfully Submitted!', 'success');
          this.getImportantNoteList();
        }
        else if (value == 'delete') {
          this.formValidation.showAlert('Successfully Deleted!', 'success');
          this.getImportantNoteList();
        }
        this.importantNotesForm.reset();
        this.selectedRow = null;
      }
      else {
        this.formValidation.showAlert('Error!', 'danger');
      }
    })
  }

}
