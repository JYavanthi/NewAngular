// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-agent-approval',
//   templateUrl: './agent-approval.component.html',
//   styleUrl: './agent-approval.component.scss'
// })
// export class AgentApprovalComponent {
// agents = [
//   {
//     name: 'Jayanth Kumar',
//     status: 'Agent Request',
//     date: '02/02/2026',
//     company: 'Sanchar6T',
//     email: 'jayanthkumar@gmail.com',
//     phone: '9980567845',
//     place: 'Banglore, Karnataka'
//   },
//   {
//     name: 'Jayanth Kumar',
//     status: 'Agent Pending',
//     date: '02/02/2026',
//     company: 'Sanchar6T',
//     email: 'jayanthkumar@gmail.com',
//     phone: '9980567845',
//     place: 'Banglore, Karnataka'
//   },
//   {
//     name: 'Jayanth Kumar',
//     status: 'Agent Shortlisted',
//     date: '02/02/2026',
//     company: 'Sanchar6T',
//     email: 'jayanthkumar@gmail.com',
//     phone: '9980567845',
//     place: 'Banglore, Karnataka'
//   }
// ];


// }

import { Component, HostListener, OnInit } from '@angular/core';
import { HttpServiceService } from '../../../services/http-service.service';
import { Chart } from 'chart.js/auto';
import { Router } from '@angular/router';

@Component({
  selector: 'app-agent-approval',
  templateUrl: './agent-approval.component.html',
  styleUrl: './agent-approval.component.scss'
})
export class AgentApprovalComponent implements OnInit {

  agentList: any[] = [];
  filteredAgents: any[] = [];
  openMenuId: number | null = null;
  // Summary Counts
  totalCount = 0;
  approvedCount = 0;
  rejectedCount = 0;
  pendingCount = 0;
  blockedCount = 0;
  pieChart: any;
  lineChart: any;
  currentPage = 1;
  itemsPerPage = 6;
  totalPages = 0;
  pagedAgents: any[] = [];
  searchText: string = '';



  constructor(private httpSer: HttpServiceService, private router: Router) { }

  ngOnInit(): void {
    this.getAgentDetails();
  }

  // ✅ API Call
  getAgentDetails() {
    this.httpSer.httpGet('/AgentDtls/GetAgentDtls').subscribe((res: any) => {

      console.log("GET Response =>", res);

      this.agentList = res.data ?? res;

      // Default show all agents
      this.filteredAgents = this.agentList;

      // Update Summary Counts
      this.updateCounts();
      this.setupPagination();
    });
  }

  setupPagination() {
    this.totalPages = Math.ceil(this.filteredAgents.length / this.itemsPerPage);
    this.changePage(1);
  }

  changePage(page: number) {

    if (page < 1 || page > this.totalPages) return;

    this.currentPage = page;

    const startIndex = (page - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;

    this.pagedAgents = this.filteredAgents.slice(startIndex, endIndex);
  }
  // ✅ Count Approved / Rejected / Pending
  updateCounts() {

    this.totalCount = this.agentList.length;

    this.approvedCount = this.agentList.filter(a => a.status === "Approved").length;

    this.rejectedCount = this.agentList.filter(a => a.status === "Rejected").length;

    this.pendingCount = this.agentList.filter(
      a => a.status === null || a.status === "Pending"
    ).length;

    this.blockedCount = this.agentList.filter(
      a => a.status === "Blocked"
    ).length;

    this.refreshCharts();
  }

  refreshCharts() {
    if (this.pieChart) this.pieChart.destroy();
    if (this.lineChart) this.lineChart.destroy();

    this.loadPieChart();
    this.loadLineChart();
  }

  // ✅ Dropdown Filter
  filterStatus(status: string) {

    if (status === "All") {
      this.filteredAgents = this.agentList;
    }

    else if (status === "Pending") {
      this.filteredAgents = this.agentList.filter(
        a => a.status === null || a.status === "Pending"
      );
    }

    else {
      this.filteredAgents = this.agentList.filter(
        a => a.status === status
      );
    }
  }
  toggleMenu(id: number) {
    if (this.openMenuId === id) {
      this.openMenuId = null;
    } else {
      this.openMenuId = id;
    }
  }

  // ✅ Close menu when click outside anywhere
  @HostListener('document:click', ['$event'])
  closeMenu(event: MouseEvent) {
    const target = event.target as HTMLElement;

    if (!target.closest('.dots-wrapper')) {
      this.openMenuId = null;
    }
  }

  // editAgent(agent: any) {
  //   console.log("Edit Agent:", agent);
  //   this.openMenuId = null;
  // }



  blockAgent(id: number) {
    console.log("Block Agent:", id);
    this.openMenuId = null;
  }

  loadPieChart() {

    this.pieChart = new Chart("pieChart", {
      type: 'pie',
      data: {
        labels: ['Approved', 'Rejected', 'Pending', 'Blocked'],
        datasets: [{
          data: [
            this.approvedCount,
            this.rejectedCount,
            this.pendingCount,
            this.blockedCount
          ],
          backgroundColor: [
            '#FFA500',
            '#FF4C4C',
            '#7CFC00',
            '#808080'
          ]
        }]
      },
      options: {
        responsive: true,
        plugins: {
          legend: {
            position: 'right'
          }
        }
      }
    });
  }

  loadLineChart() {

    this.lineChart = new Chart("lineChart", {
      type: 'line',
      data: {
        labels: ['Approved', 'Rejected', 'Pending', 'Blocked'],
        datasets: [{
          data: [
            this.approvedCount,
            this.rejectedCount,
            this.pendingCount,
            this.blockedCount
          ],
          borderColor: '#FF0000',
          tension: 0.4,
          pointRadius: 5
        }]
      },
      options: {
        responsive: true,
        plugins: {
          legend: { display: false }
        }
      }
    });
  }
  updateAgentStatus(agent: any, newStatus: string) {

    let payload = {
      agentDtlId: agent.agentDtlId,
      status: newStatus
    };

    this.httpSer.httpPost('/AgentDtls/AgentApproval', payload)
      .subscribe((res: any) => {

        console.log("Status Updated:", res);

        // ✅ Update status locally
        agent.status = newStatus;

        // ✅ Refresh counts + charts
        this.updateCounts();

        // ✅ Close menu
        this.openMenuId = null;
      });
  }

  editAgent(agentDtlId: number) {
    this.router.navigate(['/agentsignup', agentDtlId]);
  }

  deleteAgent(id: number) {

    if (!confirm("Are you sure you want to delete this agent?")) {
      return;
    }

    this.httpSer.httpDelete(`/AgentDtls/Delete/${id}`)
      .subscribe((res: any) => {

        console.log("Agent Deleted Successfully:", res);

        // ✅ Remove from agentList locally
        this.agentList = this.agentList.filter(agent => agent.agentDtlId !== id);

        // ✅ Update filtered list also
        this.filteredAgents = this.filteredAgents.filter(agent => agent.agentDtlId !== id);

        // ✅ Refresh counts + charts
        this.updateCounts();

        // ✅ Close dropdown menu
        this.openMenuId = null;

      }, error => {
        console.error("Delete Failed:", error);
      });
  }

  searchAgents() {

    let value = this.searchText.trim().toLowerCase();

    // ✅ If empty → show all
    if (value === '') {
      this.filteredAgents = this.agentList;
    }

    // ✅ Filter by firstName or lastName
    else {
      this.filteredAgents = this.agentList.filter(agent =>
        agent.firstName.toLowerCase().includes(value) ||
        agent.lastName.toLowerCase().includes(value)
      );
    }

    // ✅ Refresh Pagination after search
    this.setupPagination();
  }

}
