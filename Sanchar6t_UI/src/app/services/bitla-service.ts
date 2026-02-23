
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_URLS } from '../shared/API-URLs'
import { Observable } from 'rxjs';

export interface City {
    id: number;
    name: string;
    origin_count?: number;
    destination_count?: number;
}


export interface CityPair {
  origin_id: number;
  destination_id: number;
  travel_ids: string;
}


export interface Stage {
    id: number;
    name: string;
    city_id: number;
    city_name: string;
    latitude: number;
    longitude: number;
    area_name: string;
}
export interface TentativeBookingRequest {
    scheduleId: number;
    noOfSeats: number;
    passengerName: string;
}

@Injectable({
    providedIn: 'root'
})
export class BitlaService {

    private API_URLS = 'http://localhost:5086/api';

    constructor(private http: HttpClient) { }

    getCities(): Observable<City[]> {
        return this.http.get<City[]>(`${this.API_URLS}/Bitla/GetCities`);
    }

    getCityPairs(): Observable<CityPair[]> {
        return this.http.get<CityPair[]>(`${this.API_URLS}/Bitla/GetCityPairs`);
    }

    getMasters(): Observable<any> {
        return this.http.get<any>(`${this.API_URLS}/Bitla/GetMasters`);
    }

    getOperators(): Observable<any> {
        return this.http.get<any>(`${this.API_URLS}/Bitla/GetOperators`);
    }

    getSchedules(originId: number, destinationId: number, travelId: string): Observable<any> {
        return this.http.get(`${this.API_URLS}/Bitla/GetSchedules/${originId}/${destinationId}/${travelId}`);
    }

    getSchedule(scheduleId: string) {
        return this.http.get<any>(`${this.API_URLS}/Bitla/GetSchedule/${scheduleId}`);
    }

    getAvailabilities(
  originId: number,
  destinationId: number,
  travelDate: string
) {
  const url = `${this.API_URLS}/Bitla/GetAvailabilities/${originId}/${destinationId}/${travelDate}`;
  return this.http.get(url);
}


    getAvailabilityBySchedule(scheduleId: number): Observable<any> {
        return this.http.get(`${this.API_URLS}/Bitla/GetAvailabilityBySchedule?scheduleId=${scheduleId}`);
    }

    getTentativeBooking(scheduleId: number): Observable<any> {
        return this.http.get<any>(`${this.API_URLS}/Bitla/GetTentativeBooking?scheduleId=${scheduleId}`);
    }

    postTentativeBooking(scheduleId: number, bitlaRequest: any): Observable<any> {
        return this.http.post<any>(`${this.API_URLS}/Bitla/PostTentativeBooking/${scheduleId}`, bitlaRequest);
    }

    postConfirmBooking(ticketNumber: string, confirmBookingRequest?: any): Observable<any> {
        return this.http.post(`${this.API_URLS}/Bitla/PostConfirmBooking?ticketNumber=${ticketNumber}`, confirmBookingRequest);
    }

    getOperatorSchedules(travelId: string, travelDate: string): Observable<any> {
        return this.http.get<any>(`${this.API_URLS}/Bitla/GetOperatorSchedules/${travelId}/${travelDate}`);
    }

    getStages(): Observable<any> {
        return this.http.get<any>(`${this.API_URLS}/Bitla/GetStages`);
    }

    getBookingDetails(pnrNumber?: string, agentRefNumber?: string): Observable<any> {
        let params: any = {};
        if (pnrNumber) params.pnrNumber = pnrNumber;
        if (agentRefNumber) params.agentRefNumber = agentRefNumber;
        return this.http.get<any>(`${this.API_URLS}/Bitla/GetBookingDetails`, { params });
    }

    getCanCancelTicket(ticketNumber: string, seatNumbers: string): Observable<any> {
        return this.http.get<any>(`${this.API_URLS}/Bitla/getCanCancelTicket`, {
            params: { ticketNumber, seatNumbers }
        });
    }

    getCancelBooking(ticketNumber: string, seatNumbers: string): Observable<any> {
        return this.http.get<any>(`${this.API_URLS}/Bitla/getCancelBooking`, {
            params: { ticketNumber, seatNumbers }
        });
    }

    

    getBalance(travelId: string): Observable<any> {
        return this.http.get<any>(`${this.API_URLS}/Bitla/GetBalance`, { params: { travelId } });
    }

}



