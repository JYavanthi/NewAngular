import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiConverterService {

  constructor() { }

  mapData(data: any) {
    const headers = data.result[0];  
    const rows = data.result.slice(1);  
  
    return rows.map((row: any[]) => {
      let mappedItem: any = {};
  
      row.forEach((value, index) => {
        mappedItem[headers[index]] = value;  
      });
  
      return mappedItem;
    });
  }

  allowOnlyNumbers(event: KeyboardEvent) {
    const charCode = event.which ? event.which : event.keyCode;
    if (charCode < 48 || charCode > 57) {
      event.preventDefault();
    }
  }

}
