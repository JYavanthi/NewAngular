import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpServiceService } from './http-service.service';
import { API_URLS } from '../shared/API-URLs';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  constructor(private httpService: HttpServiceService) { }

  getBookingDetails(): Observable<any> {
    return this.httpService.httpGet(API_URLS.GET_BOOKING_DETAILS);
  }
  getUserDetails(): Observable<any> {
    return this.httpService.httpGet(API_URLS.POST_USER);
  }
  getTotalpayment(): Observable<any> {
    return this.httpService.httpGet(API_URLS.GET_TOTALPAYMENT);
  }
  getpayment(): Observable<any> {
    return this.httpService.httpGet(API_URLS.get_Payment);
  }
}