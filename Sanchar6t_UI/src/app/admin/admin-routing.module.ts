import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { BusDashboardComponent } from './Bus/bus-dashboard/bus-dashboard.component';
import { BusAddPackageComponent } from './Bus/bus-add-package/bus-add-package.component';
// import { AddLocationComponent } from './Master/add-location/add-location.component';
import { AgentApprovalComponent } from './agent-approval/agent-approval.component';
import { AgentLandingPageComponent } from './agent-landing-page/agent-landing-page.component';
import { AgenHeaderComponent } from './agen-header/agen-header.component';
import { ETicketComponent } from './e-ticket/e-ticket.component';

const routes: Routes = [
  { path: 'dashboard', component: DashboardComponent },
  { path: 'bus-dashboard', component: BusDashboardComponent },
  { path: 'bus-add-package', component: BusAddPackageComponent },
  // { path: 'add-location', component: AddLocationComponent },
  { path: 'approveagent', component: AgentApprovalComponent },
  { path: 'AgentHomePage', component: AgentLandingPageComponent }, 
  { path: 'agenHeader', component: AgenHeaderComponent },
  { path: 'eticket', component: ETicketComponent },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {
  
}
