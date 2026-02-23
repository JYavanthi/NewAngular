import { Injectable, OnDestroy } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AuthService } from '../auth/auth.service';
import { API_URLS } from '../shared/API-URLs';
import { HttpServiceService } from './http-service.service';

@Injectable({
  providedIn: 'root'
})
export class UserServiceService implements OnDestroy {

  private destroy$ = new Subject<void>();
  private userIdSource = new BehaviorSubject<string | null>(null);
  userId$ = this.userIdSource.asObservable();
  private isUserLoggedInSource = new BehaviorSubject<boolean>(false);
  isUserLoggedIn$ = this.isUserLoggedInSource.asObservable();
  userInfo: any;

  constructor(private authService: AuthService, private http: HttpServiceService) {
    this.authService.isAuthenticated.pipe(takeUntil(this.destroy$)).subscribe(status => {
      this.isUserLoggedInSource.next(status);
      this.userIdSource.next(status ? this.authService.getUser()?.UserId : null);
    });
    if (this.userIdSource.getValue()) {
      this.getUserDtls();
    }
  }

  getUserDtls() {
    this.http.httpGet(API_URLS.getUser, { userID: Number(this.userIdSource.getValue()) }).subscribe((res:any) => {
      this.userInfo = res.data[0];
      console.log('userinfo', this.userInfo)
    })
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

}
