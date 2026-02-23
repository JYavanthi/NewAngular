import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from './admin-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { SharedModule } from '../shared/shared.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { BusDashboardComponent } from './Bus/bus-dashboard/bus-dashboard.component';
import { BusAddPackageComponent } from './Bus/bus-add-package/bus-add-package.component';
import { BusPackageComponent } from './Bus/bus-add-package/bus-package/bus-package.component';
import { PackageImageDetailsComponent } from './Bus/bus-add-package/package-image-details/package-image-details.component';
import { HighlightsComponent } from './Bus/bus-add-package/highlights/highlights.component';
import { DetailedItineraryComponent } from './Bus/bus-add-package/detailed-itinerary/detailed-itinerary.component';
import { IncludedComponent } from './Bus/bus-add-package/included/included.component';
import { NotIncludedComponent } from './Bus/bus-add-package/not-included/not-included.component';
import { ImportantNotesComponent } from './Bus/bus-add-package/important-notes/important-notes.component';
import { AdditionalDetailsComponent } from './Bus/bus-add-package/additional-details/additional-details.component';
// import { AddLocationComponent } from './Master/add-location/add-location.component';
import { AgentApprovalComponent } from './agent-approval/agent-approval.component';
import { AgentLandingPageComponent } from './agent-landing-page/agent-landing-page.component';
import { AgenHeaderComponent } from './agen-header/agen-header.component';
import { ETicketComponent } from './e-ticket/e-ticket.component';


@NgModule({
  declarations: [
    DashboardComponent,
    BusDashboardComponent,
    BusAddPackageComponent,
    BusPackageComponent,
    PackageImageDetailsComponent,
    HighlightsComponent,
    DetailedItineraryComponent,
    IncludedComponent,
    NotIncludedComponent,
    ImportantNotesComponent,
    AdditionalDetailsComponent,
    // AddLocationComponent,
    AgentApprovalComponent,
    AgentLandingPageComponent,
    AgenHeaderComponent,
    ETicketComponent,
  ],
  imports: [
    BrowserModule,
    CommonModule,
    SharedModule,
    AdminRoutingModule,
  ]
})
export class AdminModule { }