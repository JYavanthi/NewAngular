import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminBackofficeRoutingModule } from './admin-backoffice-routing.module';
import { AdminBackofficeComponent } from './admin-backoffice.component';
import { AdminHeaderComponent } from './layout/admin-header/admin-header.component';
import { AdminHomeComponent } from './pages/admin-home/admin-home.component';
import { PackagesComponent } from './pages/packages/packages.component';
import { AdminSidebarComponent } from './layout/admin-sidebar/admin-sidebar.component';
import { AgentApprovalComponent } from './pages/agent-approval/agent-approval.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    AdminBackofficeComponent,
    AdminHeaderComponent,
    AdminHomeComponent,
    PackagesComponent,
    AdminSidebarComponent,
    AgentApprovalComponent
  ],
  imports: [
    CommonModule,
    AdminBackofficeRoutingModule,
     FormsModule
  ]
})
export class AdminBackofficeModule { }
