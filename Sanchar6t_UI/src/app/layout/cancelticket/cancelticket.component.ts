import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormValidationService } from '../../services/form-validation.service';

@Component({
  selector: 'app-cancelticket',
  templateUrl: './cancelticket.component.html',
  styleUrl: './cancelticket.component.scss'
})
export class CancelticketComponent {
  title = 'template-driven-form';
  cancelTktForm!: FormGroup;
  constructor(private fb: FormBuilder, private formValidation: FormValidationService) { }
 
  ngOnInit() {
    this.formInit();
  }
  formInit() {
    this.cancelTktForm = this.fb.group({
      ticketNumber: ['', [Validators.required]],
      phoneNumber: ['', [Validators.required]],
    });
  }
  submit() {
    if (!this.formValidation.validateForm(this.cancelTktForm)) {
      this.cancelTktForm.markAllAsTouched();
      setTimeout(() => { }, 0);
      return;
    }
    else{
      alert('Success')
    }
  }
}

