import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { FormValidationService } from '../../services/form-validation.service';
import { HttpServiceService } from '../../services/http-service.service';

@Component({
  selector: 'app-agent-login',
  templateUrl: './agent-login.component.html',
  styleUrl: './agent-login.component.scss'
})
export class AgentLoginComponent {

  isPasswordVisible: boolean = false;
  agentLoginForm: any;

  constructor(
    private fb: FormBuilder,
    private httpSer: HttpServiceService,
    private formValidation: FormValidationService,
    private authService: AuthService,
    private router: Router
  ) {
    this.formInit();
  }

  /* ================= FORM INIT ================= */
  formInit() {
    this.agentLoginForm = this.fb.group({
      loginId: ['', Validators.required],   // email OR phone
      password: ['', Validators.required]
    });
  }

  /* ================= LOGIN SUBMIT ================= */
  submitLogin() {

    if (!this.agentLoginForm.valid) {
      this.formValidation.showAlert(
        'Please enter Email / Phone and Password',
        'danger'
      );
      return;
    }

    const loginId = this.agentLoginForm.controls['loginId'].value.trim();

    const isPhone = /^[0-9]{10}$/.test(loginId);

    if (!isPhone && !this.isValidEmail(loginId)) {
      this.formValidation.showAlert(
        'Please enter valid Email or 10-digit Phone Number',
        'danger'
      );
      return;
    }

    const loginData = {
      mobileNo: isPhone ? loginId : 'string',
      email: isPhone ? 'string' : loginId,
      password: this.agentLoginForm.controls['password'].value
    };

    this.authService.login(loginData).subscribe({
      next: (res: any) => {

        const user = res.userData;
        const token = res.token;

        if (!user || user.userTypeId == null) {
          this.formValidation.showAlert('Invalid user data', 'danger');
          return;
        }

        // SAVE SESSION
        localStorage.setItem('token', token);
        localStorage.setItem('user', JSON.stringify(user));

        const userTypeId = Number(user.userTypeId);

        /* ===================================================
           ADMIN LOGIN
           =================================================== */
        if (userTypeId === 1) {
          this.router.navigate(['/backoffice/home']);
          return;
        }

        /* ===================================================
           AGENT LOGIN (APPROVAL REQUIRED)
           =================================================== */
        if (userTypeId === 3) {

          this.httpSer
            .httpGet(`/AgentDtls/GetAgentDtlsByUserId/${user.userId}`)
            .subscribe({
              next: (agentRes: any) => {

                const agentStatus = (agentRes.status || '').toLowerCase();

                if (agentStatus !== 'approved') {
                  this.formValidation.showAlert(
                    'Your agent account is not approved yet.',
                    'danger'
                  );
                  return;
                }

                // ✅ APPROVED AGENT
                this.router.navigate(['/admin/agenHeader']);
              },
              error: () => {
                this.formValidation.showAlert(
                  'Unable to verify agent approval status.',
                  'danger'
                );
              }
            });

          return;
        }

        /* ===================================================
           OTHER USERS BLOCKED
           =================================================== */
        this.formValidation.showAlert(
          'This page is only for Admin & Agent login.',
          'danger'
        );
      },

      error: (err) => {
        this.formValidation.showAlert(
          err?.error?.message || 'Invalid credentials!',
          'danger'
        );
      }
    });
  }

  /* ================= EMAIL VALIDATION ================= */
  isValidEmail(email: string): boolean {
    const emailPattern =
      /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return emailPattern.test(email);
  }
}
