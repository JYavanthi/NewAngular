import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit, } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormValidationService } from '../../../services/form-validation.service';
import { HttpServiceService } from '../../../services/http-service.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-my-bus-booking',
  templateUrl: './my-bus-booking.component.html',
  styleUrl: './my-bus-booking.component.scss',

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
export class MyBusBookingComponent {
  mybusBookingForm!: FormGroup;
  fileName: string | null = null;

  constructor(private fb: FormBuilder, private formValidation: FormValidationService,private http: HttpServiceService, private router: Router) {
  }

  ngOnInit(): void {
    this.formInit();
  }

  formInit() {
    this.mybusBookingForm = this.fb.group({
      rating: ['', Validators.required],
      description: ['', Validators.required],
    })
  }

  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.fileName = file.name;
    }
  }

  // Submitbutton(value: any) {
  //   let param: any;
  //   if (value == 'submit' || value == 'update') {
  //     if (!this.formValidation.validateForm(this.mybusBookingForm)) {
  //       this.mybusBookingForm.markAllAsTouched();
  //       setTimeout(() => { }, 0);
  //       return;

  Submitbutton() {
    if (!this.formValidation.validateForm(this.mybusBookingForm)) {
      this.mybusBookingForm.markAllAsTouched();
      setTimeout(() => { }, 0);
      return;
    }
  else{
const param = {
      "flag": "I",
      "reviewID": 0,
      "userID": 0,
      "busBooKingDetailID": 0,
      "rating": this.mybusBookingForm.controls['rating'].value,
      "description": this.mybusBookingForm.controls['description'].value,
      "createdBy": 0
  }
  this.http.httpPost('/api/Reviews/SaveReviews', param).subscribe((res: any) => {
    if (res.type == "S") {
      this.formValidation.showAlert('Successfully Submitted', 'success');
      this.router.navigate(['/home']);
    } else {
      this.formValidation.showAlert('Error!!', 'danger');
    }
  })

}
}


  isReviewInputFields: boolean = false;
  toggleInput() {
    this.isReviewInputFields = !this.isReviewInputFields;
  }

  stars: boolean[] = new Array(5).fill(false);

  setRating(ratingValue: number): void {
    this.mybusBookingForm.patchValue({ rating: ratingValue });
    this.updateStars();
    console.log("Current Rating: ", this.mybusBookingForm.controls['rating'].value);
  }

  updateStars(): void {
    for (let i = 0; i < 5; i++) {
      this.stars[i] = i < this.mybusBookingForm.controls['rating'].value;
    }
  }

}
