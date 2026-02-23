import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminHeaderComponent } from './layout/admin-header/admin-header.component';
import { AdminHomeComponent } from './pages/admin-home/admin-home.component';
import { AdminBackofficeComponent } from './admin-backoffice.component';
import { PackagesComponent } from './pages/packages/packages.component';
import { AgentApprovalComponent } from './pages/agent-approval/agent-approval.component';


// const routes: Routes = [
//   {
//     path: '',
//     component: AdminBackofficeComponent,
//     children: [

//       {
//         path: '',
//         component: AdminHomeComponent 
//       },
//        {
//         path: 'packages',
//         component: PackagesComponent    // 👈 packages page
//       }
//     ]
//   }
// ];


const routes: Routes = [
  {
    path: '',
    component: AdminBackofficeComponent,
    children: [

      // ✅ Default Redirect
      {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full'
      },

      // ✅ Home Page
      {
        path: 'home',
        component: AdminHomeComponent
      },

      // ✅ Packages Page
      {
        path: 'packages',
        component: PackagesComponent
      },

      {
        path: 'agentapprove',
        component: AgentApprovalComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminBackofficeRoutingModule { }
