import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { ContactUsComponent } from './contact-us/contact-us.component';
import { BusBookingComponent } from './bus-booking/bus-booking.component';
import { BusPackageComponent } from './bus-package/bus-package.component';
import { MyBookingComponent } from './my-booking/my-booking.component';
import { PopularCityComponent } from './popular-city/popular-city.component';
import { CancelticketComponent } from './cancelticket/cancelticket.component';
import { HelpComponent } from './help/help.component';
import { AuthGuard } from '../auth/auth.guard';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'about', component: AboutComponent },
  { path: 'contact', component: ContactUsComponent },
  { path: 'bus-booking', component: BusBookingComponent },
  { path: 'buspackage', component: BusPackageComponent },
  { path: 'my-booking', component: MyBookingComponent, canActivate: [AuthGuard] },
  { path: 'popular-city', component: PopularCityComponent },
  { path: 'cancelticket', component: CancelticketComponent },
  { path: 'help', component: HelpComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LayoutRoutingModule { }
