import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { FormValidationService } from '../../services/form-validation.service';

@Component({
  selector: 'app-global-alert',
  templateUrl: './global-alert.component.html',
  styleUrl: './global-alert.component.scss'
})
export class GlobalAlertComponent implements OnInit, OnDestroy {
  alertMessage: string = '';
  alertType: 'success' | 'danger' = 'danger';
  alertSubscription!: Subscription;
  isAlertVisible: boolean = false;

  constructor(private formValidationService: FormValidationService) { }

  ngOnInit(): void {
    this.alertSubscription = this.formValidationService.alertEmitter.subscribe(
      (alertData) => {
        this.alertMessage = alertData.message;
        this.alertType = alertData.alertType;
        this.isAlertVisible = true;
        // setTimeout(() => {
        //   this.isAlertVisible = false;
        // }, 3000);
      }
    );
  }

  ngOnDestroy(): void {
    if (this.alertSubscription) {
      this.alertSubscription.unsubscribe();
    }
  }

  closeAlert(): void {
    this.isAlertVisible = false;
  }

}
