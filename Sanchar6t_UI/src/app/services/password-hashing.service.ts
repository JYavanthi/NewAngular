import { Injectable } from '@angular/core';
import * as CryptoJS from 'crypto-js';

@Injectable({
  providedIn: 'root'
})
export class PasswordHashingService {

  constructor() { }

  hashPassword(password: string) {
    return CryptoJS.SHA256(password).toString(CryptoJS.enc.Hex);
  }
}
