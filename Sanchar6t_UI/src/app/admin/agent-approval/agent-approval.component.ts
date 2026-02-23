
import { Component, OnInit } from '@angular/core';
import { HttpServiceService } from '../../services/http-service.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-agent-approval',
  templateUrl: './agent-approval.component.html',
  styleUrls: ['./agent-approval.component.scss']
})

export class AgentApprovalComponent implements OnInit {

  agentList: any[] = [];
  approvedCount = 0;
  rejectedCount = 0;
  pendingCount = 0;

  constructor(
    public httpSer: HttpServiceService,
    private httpClient: HttpClient,
    private router: Router
  ) {}

  ngOnInit() {
    this.getAgentDetails();
  }

  getAgentDetails() {
    this.httpSer.httpGet('/AgentDtls/GetAgentDtls').subscribe((res: any) => {

      console.log('GET response => ', res);

      this.agentList = res.data ?? res;

      this.updateCounts();
    });
  }


  updateCounts() {
    this.approvedCount = this.agentList.filter(a =>
      a.status?.toLowerCase() === 'approved'
    ).length;

    this.rejectedCount = this.agentList.filter(a =>
      a.status?.toLowerCase() === 'rejected'
    ).length;

    // pending = all others or null
    this.pendingCount = this.agentList.filter(a => {
      const s = a.status?.toLowerCase();
      return s !== 'approved' && s !== 'rejected';
    }).length;
  }


  approveAgent(agentDtlID: number) {
    const body = { agentDtlID, status: 'Approved', createdBy: 0 };

    this.sendApprovalRequest(body).subscribe(() => {

      // refresh from DB
      setTimeout(() => {
        this.getAgentDetails();
      }, 300);

    });
  }


  rejectAgent(agentDtlID: number) {
    const body = { agentDtlID, status: 'Rejected', createdBy: 0 };

    this.sendApprovalRequest(body).subscribe(() => {

      // refresh from DB
      setTimeout(() => {
        this.getAgentDetails();
      }, 300);

    });
  }


  sendApprovalRequest(body: any): Observable<any> {
    return this.httpSer.httpPost('/AgentDtls/AgentApproval', body);
  }


  editAgent(agentDtlId: number) {
    this.router.navigate(['/agentsignup', agentDtlId]);
  }

  deleteAgent(agentDtlId: number) {
  if (!confirm("Are you sure want to delete?")) return;

  this.httpSer.httpDelete(`/AgentDtls/Delete/${agentDtlId}`)
    .subscribe(res => {

      this.getAgentDetails();   // refresh

    });
}

}

