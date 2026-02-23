import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UiStateServiceService {

  // 🔑 Recharge mode state
  private rechargeModeSource = new BehaviorSubject<boolean>(false);
  rechargeMode$ = this.rechargeModeSource.asObservable();

  // ✅ CALL THIS FROM NAVBAR
enableRechargeMode() {
  console.log('📡 Recharge mode ENABLED (Service)');
  this.rechargeModeSource.next(true);
}

  // (optional)
  disableRechargeMode() {
    this.rechargeModeSource.next(false);
  }
}
