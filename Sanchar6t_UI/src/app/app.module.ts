import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SharedModule } from "./shared/shared.module";
import { LayoutModule } from './layout/layout.module';
import { AdminModule } from './admin/admin.module';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AuthInterceptor } from './shared/auth.interceptor';
import { LoaderService } from './services/loader.service';
import { LoadingInterceptor } from './shared/loading.interceptor';
import { PhonePayIntegrationComponent } from './phone-pay-integration/phone-pay-integration.component';
import { MatTableModule } from '@angular/material/table';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TicketComponent } from './ticket/ticket.component';
import { NavbarComponent } from './navbar/navbar.component';
import { AgentRechargeComponent } from './agent-recharge/agent-recharge.component';
import { CashCreditComponent } from './agent-recharge/cash-credit/cash-credit.component';
import { BankDepositComponent } from './agent-recharge/bank-deposit/bank-deposit.component';
import { FundTransferComponent } from './agent-recharge/fund-transfer/fund-transfer.component';
import { InstantRechargeComponent } from './agent-recharge/instant-recharge/instant-recharge.component';

@NgModule({
  declarations: [
    AppComponent,
    PhonePayIntegrationComponent,
    TicketComponent,
    NavbarComponent,
    AgentRechargeComponent,
    CashCreditComponent,
    BankDepositComponent,
    FundTransferComponent,
    InstantRechargeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    SharedModule,
    LayoutModule,
    AdminModule,
    MatTableModule,
    FormsModule,
    HttpClientModule,
     ReactiveFormsModule,  
     FormsModule
    
  ],
  providers: [
    provideClientHydration(), LoaderService, { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }, { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
