import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginSignupComponent } from './auth/login-signup/login-signup.component';
import { MyProfileComponent } from './auth/my-profile/my-profile.component';
import { AgentSignupComponent } from './auth/agent-signup/agent-signup.component';
import { AuthGuard } from './auth/auth.guard';
import { PhonePayIntegrationComponent } from './phone-pay-integration/phone-pay-integration.component';
import { TicketComponent } from './ticket/ticket.component';
import { AgentRechargeComponent } from './agent-recharge/agent-recharge.component';
import { AgentLoginComponent } from './auth/agent-login/agent-login.component';

// const routes: Routes = [
//   { path: 'login', component: LoginSignupComponent },
//   { path: 'myprofile', component: MyProfileComponent, canActivate: [AuthGuard] },
//   { path: 'agentsignup', component: AgentSignupComponent },
//   { path: 'agentsignup/:id', component: AgentSignupComponent },
//   { path: '', loadChildren: () => import('./layout/layout-routing.module').then(m => m.LayoutRoutingModule) },
//   { path: 'admin', loadChildren: () => import('./admin/admin-routing.module').then(m => m.AdminRoutingModule), canActivate: [AuthGuard] },
//   { path: 'payment', component: PhonePayIntegrationComponent },
//   { path:  'ticket', component: TicketComponent },
// ];

const routes: Routes = [
    { path: '', redirectTo: 'agentLogin', pathMatch: 'full' },
  { path: 'login', component: LoginSignupComponent },
  { path: 'myprofile', component: MyProfileComponent, canActivate: [AuthGuard] },
  { path: 'agentsignup', component: AgentSignupComponent },
  { path: 'agentsignup/:id', component: AgentSignupComponent },
  { path: 'recharge', component: AgentRechargeComponent },
  { path: 'agentLogin', component: AgentLoginComponent },

  // AGENT LAYOUT
  { 
    path: '', 
    loadChildren: () => import('./layout/layout-routing.module')
      .then(m => m.LayoutRoutingModule) 
  },

  // BACKOFFICE (NEW)
 

  // OLD ADMIN IF YOU WANT KEEP
  {
    path: 'admin',
    loadChildren: () => import('./admin/admin-routing.module')
      .then(m => m.AdminRoutingModule),
    canActivate: [AuthGuard]
  },

  {
  path: 'backoffice',
  loadChildren: () =>
    import('./admin-backoffice/admin-backoffice.module')
      .then(m => m.AdminBackofficeModule)
},

  { path: 'payment', component: PhonePayIntegrationComponent },
  { path: 'ticket', component: TicketComponent },
    { path: 'backoffice', loadChildren: () => import('./admin-backoffice/admin-backoffice.module').then(m => m.AdminBackofficeModule) },
];


@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'enabled' ,  onSameUrlNavigation: 'reload'})],
  exports: [RouterModule],
  
})
export class AppRoutingModule { }
