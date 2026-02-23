import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class QueryparamEncoderService {
  private stateData: any = {};
  constructor() { }

  setState(data: any): void {
    this.stateData = data;
  }
  getState(): any {
    return this.stateData;
  }

}
