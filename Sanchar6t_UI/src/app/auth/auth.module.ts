import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { LoginSignupComponent } from './login-signup/login-signup.component';
import { MyProfileComponent } from './my-profile/my-profile.component';
import { AgentSignupComponent } from './agent-signup/agent-signup.component';
import { AgentLoginComponent } from './agent-login/agent-login.component'; // ✅ ADD
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    LoginSignupComponent,
    MyProfileComponent,
    AgentSignupComponent,
    AgentLoginComponent        // ✅ ADD THIS
  ],
  imports: [
    CommonModule,
    SharedModule,
    ReactiveFormsModule,       // ✅ REQUIRED for [formGroup]
    FormsModule                // (safe to include)
  ],
  exports: [
    LoginSignupComponent
  ]
})
export class AuthModule { }
