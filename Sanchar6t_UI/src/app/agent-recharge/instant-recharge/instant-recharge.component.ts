import { ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpServiceService } from '../../services/http-service.service';
import { API_URLS } from '../../shared/API-URLs';

@Component({
  selector: 'app-instant-recharge',
  templateUrl: './instant-recharge.component.html',
  styleUrl: './instant-recharge.component.scss'
})
export class InstantRechargeComponent {

  amount: number = 0;
  transactionCharge: number = 0;
  netAmount: number = 0;
  isContinueEnabled = false;

  constructor(
    private cdr: ChangeDetectorRef,
    private httpService: HttpServiceService,
    private router: Router
  ) {}

  onAmountChange(event: any) {
    this.amount = Number(event.target.value);

    if (this.amount > 0) {
      this.transactionCharge = Math.round(this.amount * 0.015);
      this.netAmount = this.amount + this.transactionCharge;
      this.isContinueEnabled = true;
    } else {
      this.transactionCharge = 0;
      this.netAmount = 0;
      this.isContinueEnabled = false;
    }
  }

continueRecharge() {

  const userRaw = localStorage.getItem('user');
  if (!userRaw) {
    alert('User not logged in');
    return;
  }

  const user = JSON.parse(userRaw);
  const userId = user?.userId || user?.UserId;

  if (!userId) {
    alert('Invalid user session. Please login again.');
    return;
  }

  this.httpService.httpGet(
    API_URLS.get_AgentDtlsByUserId + '/' + userId
  ).subscribe({
    next: (res: any) => {

      console.log('Agent API response:', res);

      // ✅ EXACT MATCH WITH YOUR API RESPONSE
      const agentDtlId = Number(res?.agentDtlId);

      console.log('Resolved AgentDtlId:', agentDtlId);

      if (!agentDtlId) {
        alert('Agent details not found. Please contact support.');
        return;
      }

      // 🔥 NOW THIS WILL EXECUTE
      this.createRecharge(agentDtlId, userId);
    },
    error: err => {
      console.error('Failed to fetch agent details', err);
      alert('Unable to fetch agent details');
    }
  });
}


  private createRecharge(agentDtlId: number, userId: number) {

    const payload = {
      instantRechargeId: 0,
      agentDtlId: agentDtlId,
      userId: userId,
      amount: this.amount,
      transactionCharge: this.transactionCharge,
      netAmount: this.netAmount,
      paymentMode: 'PHONEPE',
      status: 'Pending',
      createdBy: userId
    };

    console.log('Instant Recharge CREATE payload:', payload);

    this.httpService.httpPost(
      API_URLS.create_AgentInstantRecharge,
      payload
    ).subscribe({
      next: (res: any) => {

        console.log('Recharge create response:', res);

        const instantRechargeId =
          res?.instantRechargeId ??
          res?.InstantRechargeId ??
          0;

        if (!instantRechargeId) {
          alert('Recharge created but ID not returned. Contact support.');
          return;
        }

        // 🔥 STEP 3: GO TO PHONEPE PAYMENT PAGE
        this.router.navigate(
          ['/payment'],
          {
            state: {
              amount: this.netAmount,
              paymentPurpose: 'RECHARGE',
              instantRechargeId: instantRechargeId
            }
          }
        );
      },
      error: err => {
        console.error('Recharge create failed', err);
        alert('Unable to initiate recharge');
      }
    });
  }
}
