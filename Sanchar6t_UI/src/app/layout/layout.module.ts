import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LayoutRoutingModule } from './layout-routing.module';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { ContactUsComponent } from './contact-us/contact-us.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BusBookingComponent } from './bus-booking/bus-booking.component';
import { AuthModule } from '../auth/auth.module';
import { BusPackageComponent } from './bus-package/bus-package.component';
import { SharedModule } from '../shared/shared.module';
import { MyBookingComponent } from './my-booking/my-booking.component';
import { MyBusBookingComponent } from './my-booking/my-bus-booking/my-bus-booking.component';
import { MyCabBookingComponent } from './my-booking/my-cab-booking/my-cab-booking.component';
import { PopularCityComponent } from './popular-city/popular-city.component';
import { CancelticketComponent } from './cancelticket/cancelticket.component';
import { HelpComponent } from './help/help.component';

@NgModule({
  declarations: [
    HomeComponent,
    AboutComponent,
    ContactUsComponent,
    BusBookingComponent,
    BusPackageComponent,
    MyBookingComponent,
    MyBusBookingComponent,
    MyCabBookingComponent,
    PopularCityComponent,
    CancelticketComponent,
    HelpComponent
  ],
  imports: [
    BrowserAnimationsModule,
    CommonModule,
    LayoutRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    AuthModule,
    SharedModule
  ]
})
export class LayoutModule { }
