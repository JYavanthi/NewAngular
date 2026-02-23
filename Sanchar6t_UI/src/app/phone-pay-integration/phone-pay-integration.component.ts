import { Component, Injectable, OnInit } from '@angular/core';
import { HttpServiceService } from '../services/http-service.service';
import { API_URLS } from '../shared/API-URLs';
import { Router } from '@angular/router';
import { HttpHeaders } from '@angular/common/http';

declare global {
  interface Window {
    PhonePeCheckout?: any;
  }
}
@Injectable({
  providedIn: 'root'
})

@Component({
  selector: 'app-phone-pay-integration',
  templateUrl: './phone-pay-integration.component.html',
  styleUrl: './phone-pay-integration.component.scss'
})
export class PhonePayIntegrationComponent implements OnInit {
  showQrCode = false;
  selectedOption = 'upi';
  loading = false;
  paymentRedirectUrl: string = 'ticket';
  amount: number = 0;
  merchantId: string = 'TEST-M222NJL8ZHVEM_25041';
  paymentPurpose: 'BOOKING' | 'RECHARGE' = 'BOOKING';
instantRechargeId: number = 0;

  userID: number = 0;
  walletType: string = 'prepaid';
  bookingdtlsID: number = 2;

  phonePeResponse: any = null;
  merchantOrderId: string = '';

  isAgent: boolean = false
  
  isUser: boolean = false;
  userType: string = '';
  postpaidBalance: number = 0;
  prepaidBalance: number = 0;
  fareDetails: any;
  passengerData: any;
  filteredScheduleList: any[] = [];

  constructor(private httpService: HttpServiceService, private router: Router) { }

  ngOnInit() {
    const navigation = this.router.getCurrentNavigation();
    const state = navigation?.extras?.state;

    if (state) {
      this.amount = state['amount'] ?? 0;
      this.fareDetails = state['fareDetails'] ?? {};
      this.passengerData = state['passengerData'] ?? {};
       this.paymentPurpose = state['paymentPurpose'] ?? 'BOOKING';
  this.instantRechargeId = state['instantRechargeId'] ?? 0;

      console.log(" Payment page received data:", state);

      localStorage.setItem('amount', this.amount.toString());
      localStorage.setItem('fareDetails', JSON.stringify(this.fareDetails));
      localStorage.setItem('passengerData', JSON.stringify(this.passengerData));
    }
    else {

      const savedAmount = localStorage.getItem('amount');
      const savedFare = localStorage.getItem('fareDetails');
      const savedPassenger = localStorage.getItem('passengerData');

      this.amount = savedAmount ? parseFloat(savedAmount) : 0;
      this.fareDetails = savedFare ? JSON.parse(savedFare) : {};
      this.passengerData = savedPassenger ? JSON.parse(savedPassenger) : {};

      console.log("Loaded from localStorage:", {
        amount: this.amount,
        fareDetails: this.fareDetails,
        passengerData: this.passengerData
      });
    }

    this.loadPhonePeScript();
    this.loadUserData();
    this.loadWalletData();

    console.log(" Final payment amount displayed:", this.amount);
  }


  loadWalletData(): void {
    if (!this.userID) return;

    this.httpService.httpGet(API_URLS.get_WalletTransactionByID, { WalletTransactionID: this.userID }).subscribe({
      next: (res: any) => {
        if (res?.data) {
          const wallets = res.data || [];

          const postpaid = wallets.find((w: any) => w.type === 'postpaid');
          if (postpaid) {
            this.postpaidBalance = +postpaid.amount || 0;
          }

          const prepaid = wallets.find((w: any) => w.type === 'prepaid');
          if (prepaid) {
            this.prepaidBalance = +prepaid.amount || 0;
          }
        }
      },
      error: (err) => {
        console.error("Error fetching wallet details:", err);
      }
    });
  }

  loadUserData(): void {
    const userData = localStorage.getItem("user");
    if (userData) {
      try {
        const user = JSON.parse(userData);
        this.userID = parseInt(user?.UserId) || 0;
        this.userType = user?.UserType || 'User';

        this.isAgent = this.userType === 'Agent';
        this.isUser = this.userType === 'User' || this.userType !== 'Agent';

      } catch (error) {
        console.error("Error parsing user data:", error);
      }
    } else {
      console.log("No user data found in localStorage");
    }
  }

  loadPhonePeScript() {

    if (document.querySelector('script[src*="phonepe-checkout.js"]')) {
      console.log('PhonePe SDK already loaded');
      return;
    }

    const script = document.createElement('script');
    script.src = 'https://sdk.phonepe.com/public/checkout/v1/phonepe-checkout.js';
    script.async = true;
    script.onload = () => {
      console.log('PhonePe SDK loaded successfully');
    };
    script.onerror = (error) => {
      console.error('Failed to load PhonePe SDK:', error);
    };
    document.body.appendChild(script);
  }

  toggleQrCode() {
    this.showQrCode = !this.showQrCode;
  }

  onOptionChange() {
    console.log('Selected payment option:', this.selectedOption);
  }

  onWalletTypeChange(type: string) {
    this.walletType = type;
    console.log('Selected wallet type:', this.walletType);
  }

  pay() {
    if (this.selectedOption === 'wallet') {
      this.loading = true;
      this.processWalletPayment();
      return;
    }
    this.loading = true;
    console.log('Initiating payment with option:', this.selectedOption);

    this.merchantOrderId = 'ORDER_' + new Date().getTime();

    const orderRequest = {
      merchantOrderId: this.merchantOrderId,
      amount: Math.round(this.amount * 100),
      expireAfter: 1200,
      metaInfo: {
        udf1: this.userID.toString(),
        udf2: this.selectedOption,
        udf3: this.walletType,
        udf4: "string",
        udf5: "string"
      },
      paymentFlow: {
        type: 'PG_CHECKOUT',
        message: 'Wallet Recharge Payment',
        merchantUrls: {

          redirectUrl:
            this.paymentPurpose === 'BOOKING'
              ? window.location.origin + '/eticket'
              : window.location.origin + '/agent-recharge-success'
        },
        paymentModeConfig: {
          enabledPaymentModes: this.getEnabledPaymentModes()
        }
      }
    };

    this.httpService.httpPost(API_URLS.post_Phonepe, orderRequest).subscribe({
      next: (response: any) => {
        console.log('PhonePe order created:', response);
        this.loading = false;
        this.phonePeResponse = response;

        this.saveInitialPaymentData('PENDING');

        if (response.redirectUrl) {
          this.initializePhonePeCheckout(response.redirectUrl);
        } else {
          alert('Failed to get payment URL');
          this.loading = false;
        }
      },
      error: (error: any) => {
        console.error('Error creating PhonePe order:', error);
        this.loading = false;
        alert(error.message || 'Payment initiation failed. Please try again.');
      }
    });
  }

  processWalletPayment(): void {
    let sufficientBalance = false;
    const walletType = this.isAgent ? 'postpaid' : 'prepaid';
    const availableBalance = this.isAgent ? this.postpaidBalance : this.prepaidBalance;

    if (availableBalance >= this.amount) {
      sufficientBalance = true;


      const transactionData = {
        flag: 'I',
        walletTrnsnID: 0,
        userID: this.userID,
        walletType: walletType,
        amount: this.amount.toString(),
        date: new Date().toISOString(),
        mode: 'Wallet Payment',
        transactionNumber: this.generateTransactionNumber(),
        errorCode: '',
        transactionCode: '',
        message: `Payment for order ${this.merchantOrderId}`,
        createdBy: this.userID
      };


      this.httpService.httpPost(API_URLS.save_WalletTransaction, transactionData).subscribe({
        next: (response: any) => {
          console.log('Wallet transaction saved successfully', response);


          this.updateWalletBalance(walletType, -this.amount).subscribe({
            next: (balanceRes: any) => {
              console.log('Wallet balance updated', balanceRes);


              const successResponse = {
                success: true,
                code: "PAYMENT_SUCCESS",
                transactionId: transactionData.transactionNumber,
                merchantOrderId: this.merchantOrderId,
                amount: this.amount * 100,
                paymentMode: "WALLET",
                message: "Payment successful via wallet"
              };


              this.saveWalletTransaction(successResponse);
              this.updatePaymentData(successResponse, 'SUCCESS');


              setTimeout(() => {
                this.loading = false;
                this.router.navigate(['/eticket'], { replaceUrl: true });
              }, 1000);
            },
            error: (err) => {
              console.error('Error updating wallet balance', err);
              this.handleWalletPaymentError('Failed to update wallet');
            }
          });
        },
        error: (error) => {
          console.error('Error saving wallet transaction', error);
          this.handleWalletPaymentError('Transaction recording failed');
        }
      });
    } else {
      this.handleWalletPaymentError('Insufficient wallet balance');
    }
  }


  handleWalletPaymentError(message: string): void {
    this.loading = false;
    alert(message);
  }

  updateWalletBalance(walletType: string, amountChange: number) {

    const walletData = {
      userID: this.userID,
      type: walletType,
      amount: walletType === 'postpaid' ?
        (this.postpaidBalance + amountChange) :
        (this.prepaidBalance + amountChange),
      transactionLimit: 0,
      minimumBalance: 0,
      createdBy: this.userID
    };

    if (walletType === 'postpaid') {
      this.postpaidBalance += amountChange;
    } else {
      this.prepaidBalance += amountChange;
    }


    return this.httpService.httpPost(API_URLS.save_Wallet, walletData);
  }

  private getEnabledPaymentModes() {
    const paymentModes = [];

    if (this.selectedOption === 'upi') {
      paymentModes.push(
        { type: 'UPI_INTENT', cardTypes: [] },
        { type: 'UPI_COLLECT', cardTypes: [] },
        { type: 'UPI_QR', cardTypes: [] }
      );
    } else if (this.selectedOption === 'card') {
      paymentModes.push({
        type: 'CARD',
        cardTypes: ['DEBIT_CARD', 'CREDIT_CARD']
      });
    } else if (this.selectedOption === 'netbanking') {
      paymentModes.push({ type: 'NET_BANKING' });
    } else if (this.selectedOption === 'all') {
      paymentModes.push(
        { type: 'UPI_INTENT', cardTypes: [] },
        { type: 'UPI_COLLECT', cardTypes: [] },
        { type: 'UPI_QR', cardTypes: [] },
        { type: 'CARD', cardTypes: ['DEBIT_CARD', 'CREDIT_CARD'] },
        { type: 'NET_BANKING' }
      );
    }

    return paymentModes.length > 0 ? paymentModes : null;
  }

  initializePhonePeCheckout(redirectUrl: string) {

    localStorage.setItem('paymentReturnUrl', '/eticket');

    if (window['PhonePeCheckout']) {
      try {
        const PhonePeCheckout = window['PhonePeCheckout'];
        const checkout = new PhonePeCheckout({
          merchantId: this.merchantId,
          env: 'UAT',
onPaymentFailure: (response: any) => {

  console.log('Payment failure:', response);

  this.updatePaymentData(response, 'FAILED');

  // 🔥 UPDATE RECHARGE STATUS = FAILED (ONLY FOR RECHARGE)
  if (this.paymentPurpose === 'RECHARGE') {
    this.httpService.httpPost(
      API_URLS.update_AgentInstantRechargeStatus,
      {
        instantRechargeId: this.instantRechargeId,
        status: 'Failed',
        remarks: 'PhonePe payment failed',
        modifiedBy: this.userID
      }
    ).subscribe();
  }

  window.location.href = window.location.origin + '/eticket';
},

// onPaymentSuccess: (response: any) => {

//   // existing payment table update (keep as-is)
//   this.updatePaymentData(response, 'SUCCESS');

//   if (this.paymentPurpose === 'BOOKING') {

//     // 🔒 BOOKING FLOW (UNCHANGED)
//     const ticketNumber =
//       localStorage.getItem('TICKET_NUMBER') || this.merchantOrderId;

//     this.router.navigate(
//       ['/eticket'],
//       {
//         state: { ticketNumber },
//         replaceUrl: true
//       }
//     );

//   } else {

//     // 🔥 AGENT RECHARGE FLOW

//     // ✅ STEP 1: Update Instant Recharge Status = SUCCESS
//     this.httpService.httpPost(
//       API_URLS.update_AgentInstantRechargeStatus,
//       {
//         instantRechargeId: this.instantRechargeId,
//         status: 'Success',
//         referenceNo: this.extractTransactionId(response),
//         remarks: 'PhonePe recharge successful',
//         modifiedBy: this.userID
//       }
//     ).subscribe(() => {

//       // ✅ STEP 2: Save wallet transaction (already in your code)
//       this.saveWalletTransaction(response);

//       // ✅ STEP 3: Update wallet balance
//       this.updateWalletBalance(
//         this.walletType,
//         +this.amount
//       ).subscribe(() => {

//         // ✅ STEP 4: Navigate to success page
//         this.router.navigate(
//           ['/agent-recharge-success'],
//           {
//             state: {
//               amount: this.amount,
//               transactionId: this.extractTransactionId(response)
//             },
//             replaceUrl: true
//           }
//         );
//       });
//     });
//   }
// },


onPaymentSuccess: (response: any) => {

  // 🔒 KEEP: Update payment table (common for booking + recharge)
  this.updatePaymentData(response, 'SUCCESS');

  // =========================
  // 🎟️ BOOKING FLOW (UNCHANGED)
  // =========================
  if (this.paymentPurpose === 'BOOKING') {

    const ticketNumber =
      localStorage.getItem('TICKET_NUMBER') || this.merchantOrderId;

    this.router.navigate(
      ['/eticket'],
      {
        state: { ticketNumber },
        replaceUrl: true
      }
    );

    return; // ⛔ important: stop here
  }

  // =========================
  // 💰 AGENT RECHARGE FLOW
  // =========================

  const transactionId = this.extractTransactionId(response);

  // 🔐 SAFETY: store recharge id (for redirect / refresh cases)
  if (this.instantRechargeId) {
    localStorage.setItem(
      'instantRechargeId',
      this.instantRechargeId.toString()
    );
  }

  // ✅ STEP 1: Update Instant Recharge Status = SUCCESS
  this.httpService.httpPost(
    API_URLS.update_AgentInstantRechargeStatus,
    {
      instantRechargeId: this.instantRechargeId,
      status: 'Success',
      referenceNo: transactionId,
      remarks: 'PhonePe recharge successful',
      modifiedBy: this.userID
    }
  ).subscribe({
    next: () => {

      // ✅ STEP 2: Save wallet transaction
      this.saveWalletTransaction(response);

      // ✅ STEP 3: Update wallet balance
      this.updateWalletBalance(
        this.walletType,
        +this.amount
      ).subscribe({

        next: () => {

          // ✅ STEP 4: Navigate to recharge success page
          this.router.navigate(
            ['/admin/agenHeader'],
            {
              state: {
                amount: this.amount,
                transactionId: transactionId,
                instantRechargeId: this.instantRechargeId
              },
              replaceUrl: true
            }
          );
        },

        error: (err) => {
          console.error('Wallet balance update failed', err);
          alert('Recharge successful, but wallet update failed. Contact support.');
        }
      });
    },

    error: (err) => {
      console.error('Recharge status update failed', err);
      alert('Payment successful, but recharge status update failed.');
    }
  });
},

          onCancel: (response: any) => {
            console.log('Payment cancelled:', response);
            this.updatePaymentData(response, 'CANCELLED');
            window.location.href = window.location.origin + '/eticket';
          }
        });


        checkout.redirectToPhonePe(redirectUrl);
      } catch (error) {
        console.error('Error initializing PhonePe checkout:', error);

        window.location.href = redirectUrl;
      }
    } else {
      console.warn('PhonePeCheckout not available, using direct redirect');
      window.location.href = redirectUrl;
    }
  }

  saveWalletTransaction(paymentResponse: any): void {
    const amountInRupees = this.amount;
    const transactionId = this.extractTransactionId(paymentResponse);

    const transactionData = {
      flag: 'I',
      walletTrnsnID: 0,
      userID: this.userID,
      walletType: this.walletType,
      amount: amountInRupees.toString(),
      date: new Date().toISOString(),
      mode: 'PhonePe Recharge',
      transactionNumber: transactionId,
      errorCode: this.extractErrorCode(paymentResponse),
      transactionCode: this.extractTransactionCode(paymentResponse),
      message: `${this.walletType} Wallet Recharge via PhonePe`,
      createdBy: this.userID
    };

    console.log('Wallet transaction data to be saved:', transactionData);

    this.httpService.httpPost(API_URLS.save_WalletTransaction, transactionData).subscribe({
      next: (response: any) => {
        console.log('Wallet transaction saved successfully', response);
      },
      error: (error: any) => {
        console.error('Error saving wallet transaction', error);
      }
    });
  }

  saveInitialPaymentData(status: string): void {
    const paymentData = {
      flag: "I",
      paymentID: 0,
      userID: this.userID,
      bookingdtlsID: this.bookingdtlsID,
      amount: this.amount,
      paymentMode: "phonepe",
      transactionID: this.phonePeResponse?.orderId || this.merchantOrderId,
      transactionResponse: JSON.stringify(this.phonePeResponse || {}),
      transactionCode: "",
      paymentStatus: status,
      errorCode: "",
      createdBy: this.userID
    };

    console.log('Initial payment data to be saved:', paymentData);
    this.directSavePayment(paymentData);
  }

  directSavePayment(paymentData: any): void {
    console.log('Starting API call attempt to:', API_URLS.post_Payment);

    this.httpService.httpPost(API_URLS.post_Payment, paymentData).subscribe({
      next: (response: any) => {
        console.log('Payment data saved successfully via service', response);
      },
      error: (serviceError: any) => {
        console.error('Error from service:', serviceError);

        console.log('Trying direct fetch call as backup...');

        const baseUrl = this.getBaseUrl();
        const url = `${baseUrl}/Payment/SavePayment`;
        const token = this.getAuthToken();

        fetch(url, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': token ? `Bearer ${token}` : '',
          },
          body: JSON.stringify(paymentData)
        })
          .then(response => {
            if (!response.ok) {
              throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json();
          })
          .then(data => {
            console.log('Payment saved successfully via direct fetch:', data);
          })
          .catch(fetchError => {
            console.error('Error in direct fetch:', fetchError);

            console.log('Trying with XMLHttpRequest as last resort...');
            this.saveWithXhr(url, paymentData, token);
          });
      }
    });
  }

  saveWithXhr(url: string, data: any, token: string): void {
    const xhr = new XMLHttpRequest();
    xhr.open('POST', url, true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    if (token) {
      xhr.setRequestHeader('Authorization', `Bearer ${token}`);
    }

    xhr.onload = function () {
      if (xhr.status >= 200 && xhr.status < 300) {
        console.log('Payment saved successfully via XHR:', xhr.responseText);
      } else {
        console.error('XHR Error:', xhr.statusText, xhr.responseText);
      }
    };

    xhr.onerror = function () {
      console.error('XHR Network Error');
    };

    xhr.send(JSON.stringify(data));
  }

  getBaseUrl(): string {
    const apiUrl = localStorage.getItem('apiBaseUrl');
    if (apiUrl) {
      return apiUrl;
    }

    const url = window.location.origin;
    if (url.includes('localhost')) {
      return 'http://localhost:4200/payment';
    }
    return url;
  }

  getAuthToken(): string {
    const token = localStorage.getItem('authToken');
    return token || '';
  }

  updatePaymentData(paymentResponse: any, status: string): void {
    const transactionId = this.extractTransactionId(paymentResponse);

    const paymentData = {
      flag: "U",
      paymentID: 0,
      userID: this.userID,
      bookingdtlsID: this.bookingdtlsID,
      amount: this.amount,
      paymentMode: "phonepe",
      transactionID: transactionId,
      transactionResponse: JSON.stringify(paymentResponse || {}),
      transactionCode: this.extractTransactionCode(paymentResponse),
      paymentStatus: status,
      errorCode: this.extractErrorCode(paymentResponse),
      createdBy: this.userID
    };

    console.log('Updated payment data to be saved:', paymentData);
    this.directSavePayment(paymentData);
  }

  extractTransactionId(response: any): string {
    return response?.transactionId ||
      response?.providerReferenceId ||
      response?.merchantTransactionId ||
      this.phonePeResponse?.orderId ||
      this.merchantOrderId ||
      this.generateTransactionNumber();
  }

  extractTransactionCode(response: any): string {
    return response?.code ||
      response?.status ||
      response?.responseCode ||
      '';
  }

  extractErrorCode(response: any): string {
    return response?.errorCode ||
      response?.error?.code ||
      response?.errorMessage ||
      '';
  }

  generateTransactionNumber(): string {
    return `TXN-${Date.now()}-${Math.floor(Math.random() * 10000)}`;
  }
}