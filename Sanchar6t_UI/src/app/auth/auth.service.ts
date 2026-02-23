import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { API_URLS } from '../shared/API-URLs';
import { jwtDecode } from 'jwt-decode';
import { HttpServiceService } from '../services/http-service.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public isAuthenticated = new BehaviorSubject<boolean>(this.hasToken());

  constructor(private http: HttpServiceService, private router: Router) { }

  login(credentials: { mobileNo: string; password: string }): Observable<any> {
    return this.http.httpPost(API_URLS.SIGNIN, credentials).pipe(
      tap((response: any) => {
        if (response && response.token) {
          localStorage.setItem('response', response);
          localStorage.setItem('token', response.token);
          localStorage.setItem('user', JSON.stringify(this.decodeToken(response.token)));
          localStorage.setItem('userData', response.userData);
          this.isAuthenticated.next(true);
          this.router.navigate(['/home']);
        } else {
          console.error('No token received');
        }
      })
    );
  }

  logout() {
    if (typeof localStorage !== 'undefined') {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      localStorage.removeItem('response');
    }
    this.isAuthenticated.next(false);
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    return typeof localStorage !== 'undefined' && !!localStorage.getItem('token');
  }

  getUser(): any {
    if (typeof localStorage !== 'undefined') {
      const user = localStorage.getItem('user');
      return user ? JSON.parse(user) : null;
    }
    return null;
  }

  private hasToken(): boolean {
    return typeof localStorage !== 'undefined' && !!localStorage.getItem('token');
  }

  private decodeToken(token: string): any {
    try {
      return jwtDecode(token);
    } catch (e) {
      console.error('Invalid token', e);
      return null;
    }
  }
  
}