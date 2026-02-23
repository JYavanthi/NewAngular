import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpServiceService } from '../../services/http-service.service';
import { API_URLS } from '../../shared/API-URLs';

@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.scss']
})
export class MyProfileComponent implements OnInit {
  selectedSection: 'wallet' | 'walletMain' | 'postpaid' | 'prepaid' | 'profile' = 'wallet';

  userID: number = 0;
  userType: string = '';
  walletID: number = 0; // Postpaid wallet ID
  prepaidWalletID: number = 0;

  postpaidLimit: number = 0;
  postpaidUsed: number = 0;
  postpaidBalance: number = 0;
  prepaidBalance: number = 0;
  minimumBalanceThreshold: number = 0;

  rechargeForm: FormGroup;
  prepaidRechargeForm: FormGroup;

  showLowBalanceError: boolean = false;
  showRechargeRequiredError: boolean = false;
  isAgent: boolean = false;
  isUser: boolean = false;

  bloodGroups = ['A+', 'A-', 'B+', 'B-', 'AB+', 'AB-', 'O+', 'O-'];

  recentTransactions: any[] = [];
  isLoading: boolean = false;
  isSavingWallet: boolean = false;

  currentPage: number = 1;
  pageSize: number = 5;
  totalTransactions: number = 0;

  constructor(
    private fb: FormBuilder,
    private httpService: HttpServiceService
  ) {
    this.rechargeForm = this.fb.group({
      amount: ['', [Validators.required, Validators.min(5000)]]
    });

    this.prepaidRechargeForm = this.fb.group({
      amount: ['', [Validators.required, Validators.min(5000)]]
    });
  }

  ngOnInit(): void {
    this.loadUserData();
    this.getWalletsByUserID();
  }

  loadUserData(): void {
    const userData = localStorage.getItem("user");
    if (userData) {
      try {
        const user = JSON.parse(userData);
        this.userID = parseInt(user?.UserId) || 0;
        this.userType = user?.UserType || 'user';

        this.isAgent = this.userType === 'Agent';
        this.isUser = this.userType === 'User';

        if (this.userID > 0) {
          this.checkAndCreateWallets();
        } else {
          console.error("Invalid userID:", this.userID);
        }
      } catch (error) {
        console.error("Error parsing user data:", error);
      }
    } else {
      console.log("No user data found in localStorage");
    }
  }

  checkAndCreateWallets(): void {
    this.isLoading = true;
    // Use the correct API for getting wallets by UserID
    this.httpService.httpGet(API_URLS.get_wallet_BYID, { userID: this.userID }).subscribe({
      next: (res: any) => {
        if (!res || !res.data) {
          console.error("Invalid response format:", res);
          this.isLoading = false;
          this.createDefaultWallets();
          return;
        }

        const wallets = res.data || [];
        console.log("Wallet data:", res.data);
        

        // Process existing wallets first
        this.processExistingWallets(wallets);

        // Determine which wallets need to be created based on user type
        const promises: Promise<void>[] = [];

        // Check if user needs a prepaid wallet
        const hasPrepaid = wallets.some((w: any) => w.type === 'prepaid');
        // All users get a prepaid wallet
        if (!hasPrepaid) {
             promises.push(this.createPrepaidWallet());
        }


        // Check if agent/user needs a postpaid wallet
        const hasPostpaid = wallets.some((w: any) => w.type === 'postpaid');
        if ((this.isAgent || this.isUser) && !hasPostpaid) {
          // Only agents and users get postpaid wallets
          promises.push(this.createPostpaidWallet(hasPostpaid));
        }

        // Wait for all wallet creation to complete
        if (promises.length > 0) {
          Promise.all(promises).then(() => {
            console.log("All wallets created successfully");
            // Refresh wallet details after creation using the new API
            //this.getWalletsByUserID();
            this.isLoading = false;
          }).catch(err => {
            console.error("Error creating wallets:", err);
            this.isLoading = false;
          });
        } else {
          this.isLoading = false;
        }
      },
      error: (err) => {
        console.error("Error fetching wallet details:", err);
        // If fetching by UserID fails, assume no wallets exist and create defaults
        this.createDefaultWallets();
        this.isLoading = false;
      }
    });
  }

  processExistingWallets(wallets: any[]): void {
    if (this.isAgent || this.isUser) {
      const postpaid = wallets.find((w: any) => w.type === 'postpaid');
      if (postpaid) {
        this.walletID = postpaid.walletID;
        this.postpaidBalance = +postpaid.amount || 0;
        this.postpaidLimit = +postpaid.transactionLimit || this.postpaidBalance;
        this.postpaidUsed = this.postpaidLimit - this.postpaidBalance;
      } else {
        // Reset postpaid details if not found for agent/user
        this.walletID = 0;
        this.postpaidBalance = 0;
        this.postpaidLimit = 0;
        this.postpaidUsed = 0;
      }
    } else {
       // Reset postpaid details if not agent/user
       this.walletID = 0;
       this.postpaidBalance = 0;
       this.postpaidLimit = 0;
       this.postpaidUsed = 0;
    }


    const prepaid = wallets.find((w: any) => w.type === 'prepaid');
    if (prepaid) {
      this.prepaidWalletID = prepaid.walletID;
      this.prepaidBalance = +prepaid.amount || 0;
    } else {
       // Reset prepaid details if not found
       this.prepaidWalletID = 0;
       this.prepaidBalance = 0;
    }


    this.checkBalanceStatus();
    // this.getWalletTransactions(); // Fetches transactions by UserID
  }

  showSection(section: 'wallet' | 'walletMain' | 'postpaid' | 'prepaid' | 'profile'): void {
    if (section === 'postpaid' && !this.isAgent && !this.isUser) {
      alert('Postpaid wallet is only available for agents');
      return;
    }

    this.selectedSection = section;
    if (['wallet', 'walletMain', 'postpaid', 'prepaid'].includes(section)) {
      this.checkBalanceStatus();
      // Force refresh of wallet data when switching to wallet sections
      //this.getWalletsByUserID(); // Use the new API call
    }
  }

  getFormValidity(): boolean {
    if (this.selectedSection === 'postpaid') {
      return this.rechargeForm.invalid;
    } else if (this.selectedSection === 'prepaid') {
      return this.prepaidRechargeForm.invalid;
    }
    return false;
  }
  userwalletID :any;
  // Renamed getWalletDetails to getWalletsByUserID to reflect the API used
  getWalletsByUserID(): void {
    debugger

    if (!this.userID) {
      console.error("Cannot fetch wallet details: userID is not set");
      return;
    }

    this.isLoading = true;
    // Use the correct API for getting wallets by UserID
    this.httpService.httpGet(API_URLS.get_WalletByUserID, { userID: this.userID }).subscribe({
      next: (res: any) => {
        if (!res || !res.data) {
          console.error("Invalid response format:", res);
          this.isLoading = false;
          return;
        }

        const wallets = res.data || [];
        console.log("Wallet data from getWalletsByUserID:", res.data);
        this.userwalletID = res.data[0].walletId
        // Process the fetched wallets
        this.isLoading = false;
      },
      error: (err) => {
        console.error("Error fetching wallet details using getWalletsByUserID:", err);
        this.isLoading = false;
      }
    });
  }

  // Convert to Promise to allow sequential operations
  createPrepaidWallet(): Promise<void> {
    return new Promise((resolve, reject) => {
      const walletData = {
        walletID: this.userwalletID,
        userID: this.userID,
        type: 'prepaid',
        amount: '0',           // Initialize with 0 balance
        transactionLimit: '0',
        createdBy: this.userID
      };

      this.httpService.httpPost(API_URLS.save_Wallet, walletData).subscribe({
        next: (res: any) => {
          if (res?.data?.walletID) {
            this.prepaidWalletID = res.data.walletID;
            this.prepaidBalance = 0;  // Set local balance to 0
            console.log("Prepaid wallet created:", this.prepaidWalletID);
            resolve();
          } else {
            console.error("Prepaid wallet created but no ID returned:", res);
            reject(new Error("No wallet ID returned for prepaid"));
          }
        },
        error: (err) => {
          console.error("Error creating prepaid wallet:", err);
          reject(err);
        }
      });
    });
  }

  // Convert to Promise to allow sequential operations
  createPostpaidWallet(newBalance:any): Promise<void> {
    debugger
    return new Promise((resolve, reject) => {
      const walletData = {
        walletID: this.userwalletID,
        userID: this.userID,
        type: 'postpaid',
        amount: String(newBalance),           // Initialize with 0 balance
        transactionLimit: '0',
        createdBy: this.userID
      };

      this.httpService.httpPost(API_URLS.save_Wallet, walletData).subscribe({
        next: (res: any) => {
          if (res?.data?.walletID) {
            this.walletID = res.data.walletID;
            this.postpaidBalance = 0;  // Set local balance to 0
            this.postpaidLimit = 0;    // Set local limit to 0
            console.log("Postpaid wallet created:", this.walletID);
            resolve();
          } else {
            console.error("Postpaid wallet created but no ID returned:", res);
            reject(new Error("No wallet ID returned for postpaid"));
          }
        },
        error: (err) => {
          console.error("Error creating postpaid wallet:", err);
          reject(err);
        }
      });
    });
  }

  createDefaultWallets(): void {
    // Determine which wallets to create based on user type
    let walletCreationPromise: Promise<void>;

    // All users get a prepaid wallet
    walletCreationPromise = this.createPrepaidWallet();

    // If agent or user, also create postpaid wallet
    if (this.isAgent || this.isUser) {
      walletCreationPromise = walletCreationPromise.then(() => {
        console.log("Prepaid wallet created, now creating postpaid wallet (if needed)");
        return 
      }).catch(err => {
         // If prepaid creation failed, still attempt postpaid if applicable
         console.warn("Prepaid wallet creation failed, attempting postpaid if applicable:", err);
         return 
      });
    }

    // After wallet creation completes
    walletCreationPromise
      .then(() => {
        console.log("Default wallets creation process completed");
        // Refresh wallet data after all wallet creation attempts using the new API
       // this.getWalletsByUserID();
      })
      .catch(err => {
        console.error("Error during default wallets creation process:", err);
        // Even if there's an error, try to get any wallets that might exist using the new API
       // this.getWalletsByUserID();
      });
  }


  checkBalanceStatus(): void {
    // Set minimum threshold - could be configured elsewhere
    this.minimumBalanceThreshold = 1000; // Example threshold

    // Only show low balance/recharge required for postpaid if it's an agent/user and wallet exists
    if (this.isAgent || this.isUser) {
       this.showLowBalanceError = this.postpaidBalance <= (this.minimumBalanceThreshold * 2);
       this.showRechargeRequiredError = this.postpaidBalance <= this.minimumBalanceThreshold;
    } else {
       this.showLowBalanceError = false;
       this.showRechargeRequiredError = false;
    }
  }

  // Update wallet balance in the database
  updateWalletBalance(walletType: 'postpaid' | 'prepaid', newBalance: number): Promise<boolean> {
    return new Promise((resolve, reject) => {
      const walletID = walletType === 'postpaid' ? this.walletID : this.prepaidWalletID;

      // If wallet ID is missing or 0, attempt creation first
      if (!walletID || walletID === 0) {
        console.warn(`Cannot update ${walletType} wallet: No valid wallet ID found. Attempting creation...`);
        const createWalletPromise = walletType === 'postpaid' ?
          this.createPostpaidWallet(newBalance) :
          this.createPrepaidWallet();

        createWalletPromise
          .then(() => {
            // Retry update after wallet creation
            const newWalletID = walletType === 'postpaid' ? this.walletID : this.prepaidWalletID;
            if (newWalletID && newWalletID !== 0) {
              console.log(`Created new ${walletType} wallet with ID ${newWalletID}, retrying update`);
              // Recursive call to retry update with the newly created wallet ID
              this.updateWalletBalance(walletType, newBalance)
                .then(() => resolve(true))
                .catch(() => reject(false));
            } else {
              console.error(`Wallet creation succeeded but no valid ID returned for ${walletType}`);
              reject(false);
            }
          })
          .catch((err) => {
             console.error(`Error during wallet creation attempt for ${walletType}:`, err);
             reject(false); // Creation failed, so update cannot proceed
          });
        return;
      }

      this.isSavingWallet = true;

      const walletData = {
        walletID: walletID,
        userID: this.userID,
        type: walletType,
        amount: newBalance.toString(),
        transactionLimit: walletType === 'postpaid' ? newBalance.toString() : '0', // Assuming limit is updated to new balance for postpaid
        createdBy: this.userID
      };

      console.log(`Sending wallet update with data:`, walletData);

      this.httpService.httpPost(API_URLS.save_Wallet, walletData).subscribe({
        next: (res: any) => {
          console.log(`${walletType} wallet updated:`, res);
          this.isSavingWallet = false;

          // Make sure wallet ID is updated locally in case it was 0 before the update
          if (res?.data?.walletID) {
            if (walletType === 'postpaid') {
              this.walletID = res.data.walletID;
            } else {
              this.prepaidWalletID = res.data.walletID;
            }
          }

          // *** Call the new API to refresh all wallet details after saving ***
         // this.getWalletsByUserID();


          resolve(true);
        },
        error: (err) => {
          console.error(`Error updating ${walletType} wallet:`, err);
          this.isSavingWallet = false;
          reject(false);
        }
      });
    });
  }

  rechargePostpaid(): void {
    debugger
    if (!this.isAgent && !this.isUser) {
      alert('Only agents can recharge postpaid wallet');
      return;
    }
    alert(this.userwalletID)
    if (this.rechargeForm.valid) {
      const amount = this.rechargeForm.value.amount;

      // Check if wallet ID exists before proceeding
      if (!this.userwalletID || this.userwalletID === 0) {
         // Try to create/fetch the wallet if ID is missing
         console.warn("Postpaid wallet ID is missing, attempting to create/fetch...");
         // Call updateWalletBalance which handles creation/retrieval and then updates
         this.updateWalletBalance('postpaid', this.postpaidBalance + amount)
           .then(() => {
            debugger  
             // Local state update and transaction saving are handled in updateWalletBalance's success path now
             this.saveTransaction('postpaid', amount, 'Postpaid Recharge');
             this.rechargeForm.reset();
             alert(`Recharged ₹${amount} successfully.`);
             this.showSection('walletMain');
           })
           .catch(() => {
             alert('Failed to set up or update postpaid wallet. Please try again.');
             // getWalletsByUserID is called in updateWalletBalance's success/error, no need to call here
           });
         return;
      }

      const newBalance = this.postpaidBalance + amount;

      // Update wallet in database
      this.updateWalletBalance('postpaid', newBalance)
        .then(() => {
          // Local state update and transaction saving are handled in updateWalletBalance's success path now
          this.saveTransaction('postpaid', amount, 'Postpaid Recharge');
          this.rechargeForm.reset();
          alert(`Recharged ₹${amount} successfully.`);
          this.showSection('walletMain');
        })
        .catch(() => {
          alert('Failed to update wallet. Please try again.');
          // getWalletsByUserID is called in updateWalletBalance's success/error, no need to call here
        });
    } else {
      this.rechargeForm.markAllAsTouched();
    }
  }

  rechargePrepaid(): void {
    if (this.prepaidRechargeForm.valid) {
      const amount = this.prepaidRechargeForm.value.amount;

      // Check if wallet ID exists before proceeding
      if (!this.prepaidWalletID || this.prepaidWalletID === 0) {
         // Try to create/fetch the wallet if ID is missing
         console.warn("Prepaid wallet ID is missing, attempting to create/fetch...");
         // Call updateWalletBalance which handles creation/retrieval and then updates
         this.updateWalletBalance('prepaid', this.prepaidBalance + amount)
           .then(() => {
             // Local state update and transaction saving are handled in updateWalletBalance's success path now
             this.saveTransaction('prepaid', amount, 'Prepaid Recharge');
             this.prepaidRechargeForm.reset();
             alert(`Added ₹${amount} to prepaid successfully.`);
             this.showSection('walletMain');
           })
           .catch(() => {
             alert('Failed to set up or update prepaid wallet. Please try again.');
             // getWalletsByUserID is called in updateWalletBalance's success/error, no need to call here
           });
         return;
      }

      const newBalance = this.prepaidBalance + amount;

      // Update wallet in database
      this.updateWalletBalance('prepaid', newBalance)
        .then(() => {
           // Local state update and transaction saving are handled in updateWalletBalance's success path now
           this.saveTransaction('prepaid', amount, 'Prepaid Recharge');
           this.prepaidRechargeForm.reset();
           alert(`Added ₹${amount} to prepaid successfully.`);
           this.showSection('walletMain');
        })
        .catch(() => {
          alert('Failed to update wallet. Please try again.');
          // getWalletsByUserID is called in updateWalletBalance's success/error, no need to call here
        });
    } else {
      this.prepaidRechargeForm.markAllAsTouched();
    }
  }



  proceedToPayment(): void {
    // Determine which form to use based on the current section
    const form = this.selectedSection === 'postpaid' ? this.rechargeForm : this.prepaidRechargeForm;
    const walletType = this.selectedSection === 'postpaid' ? 'postpaid' : 'prepaid';
    const walletID = walletType === 'postpaid' ? this.walletID : this.prepaidWalletID;

    if (form.invalid) {
      form.markAllAsTouched();
      alert('Please enter a valid amount (minimum ₹5000)');
      return;
    }

    const amount = form.value.amount;
    const newBalance = (walletType === 'postpaid' ? this.postpaidBalance : this.prepaidBalance) + amount;

    // Check if wallet ID exists before proceeding, and handle creation/fetching if needed
    if (!walletID || walletID === 0) {
        console.warn(`${walletType} wallet ID is missing, attempting to create/fetch...`);
        // Call updateWalletBalance which handles creation/retrieval and then updates
        this.updateWalletBalance(walletType, newBalance)
            .then(() => {
                 // Local state update and transaction saving are handled in updateWalletBalance's success path now
                 this.saveTransaction(walletType, amount, `${walletType.charAt(0).toUpperCase() + walletType.slice(1)} Recharge`);
                 form.reset();
                 alert(`Payment processed successfully! ₹${amount} added to your ${walletType} wallet.`);
                 this.showSection('walletMain');
            })
            .catch(() => {
                 alert(`Failed to set up or update ${walletType} wallet. Payment processing failed. Please try again.`);
                 // getWalletsByUserID is called in updateWalletBalance's success/error, no need to call here
            });
        return;
    }

    // If wallet ID exists, proceed with updating
    this.updateWalletBalance(walletType, newBalance)
      .then(() => {
        // Local state update and transaction saving are handled in updateWalletBalance's success path now
        this.saveTransaction(walletType, amount, `${walletType.charAt(0).toUpperCase() + walletType.slice(1)} Recharge`);
        form.reset();

        alert(`Payment processed successfully! ₹${amount} added to your ${walletType} wallet.`);
        this.showSection('walletMain'); // Show main wallet view after payment
      })
      .catch(() => {
        alert('Payment processing failed. Please try again.');
        // getWalletsByUserID is called in updateWalletBalance's success/error, no need to call here
      });
  }



  bookBusTicket(amount: number): boolean {
    let transactionInitiated = false; // Use a flag to track if a transaction attempt was made

    // First check if it's an agent/user and postpaid is available and has sufficient balance
    if ((this.isAgent || this.isUser) && this.postpaidBalance >= amount && this.walletID && this.walletID !== 0) {
      transactionInitiated = true;
      const newBalance = this.postpaidBalance - amount;

      // Update wallet in database
      this.updateWalletBalance('postpaid', newBalance)
        .then(() => {
          // Local state update and transaction saving are handled in updateWalletBalance's success path now
          this.saveTransaction('postpaid', -amount, 'Bus Booking (postpaid)');
          alert(`₹${amount} deducted from postpaid wallet.`);
          // getWalletsByUserID is called in updateWalletBalance's success/error, no need to call here
        })
        .catch(() => {
          alert('Postpaid transaction failed. Please try again.');
          // getWalletsByUserID is called in updateWalletBalance's success/error, no need to call here
        });

    }
    // If postpaid wasn't used or failed, fall back to prepaid if available and has sufficient balance
    else if (this.prepaidBalance >= amount && this.prepaidWalletID && this.prepaidWalletID !== 0) {
      transactionInitiated = true;
      const newBalance = this.prepaidBalance - amount;

      // Update wallet in database
      this.updateWalletBalance('prepaid', newBalance)
        .then(() => {
          // Local state update and transaction saving are handled in updateWalletBalance's success path now
          this.saveTransaction('prepaid', -amount, 'Bus Booking (prepaid)');
          alert(`₹${amount} deducted from prepaid wallet.`);
          // getWalletsByUserID is called in updateWalletBalance's success/error, no need to call here
        })
        .catch(() => {
          alert('Prepaid transaction failed. Please try again.');
          // getWalletsByUserID is called in updateWalletBalance's success/error, no need to call here
        });
    }

    // If no transaction was initiated
    if (!transactionInitiated) {
       // Check if the issue was missing wallet IDs or insufficient balance
       if ((this.isAgent || this.isUser) && (!this.walletID || this.walletID === 0)) {
            alert('Postpaid wallet setup incomplete. Please refresh the page.');
           // this.getWalletsByUserID(); // Try to refresh wallet data
       } else if (!this.prepaidWalletID || this.prepaidWalletID === 0) {
            alert('Prepaid wallet setup incomplete. Please refresh the page.');
           // this.getWalletsByUserID(); // Try to refresh wallet data
       } else {
            alert('Insufficient balance in both postpaid and prepaid wallets. Please recharge.');
       }
       return false; // Transaction did not happen
    }

    return true; // Transaction was initiated (either postpaid or prepaid)
  }


  saveTransaction(walletType: string, amount: number, message: string): void {
    // Determine the correct wallet ID for the transaction
    const walletIdForTransaction = walletType === 'postpaid' ? this.walletID : this.prepaidWalletID;

    if (!walletIdForTransaction || walletIdForTransaction === 0) {
        console.error(`Cannot save transaction: No valid wallet ID found for ${walletType} wallet.`);
        // Optionally, alert the user or handle this error more gracefully
        return;
    }

    const transactionData = {
      flag: 'I',
      walletTrnsnID: 0, // Assuming this is auto-generated by the backend on insert
      userID: this.userID,
      walletID: walletIdForTransaction, // Include the specific wallet ID
      walletType: walletType, // Include the wallet type
      amount: Math.abs(amount).toString(),
      date: new Date().toISOString(),
      mode: message, // Using message as mode for simplicity
      transactionNumber: this.generateTransactionNumber(),
      errorCode: '',
      transactionCode: '',
      message: message, // Using message as message
      transactionType: amount > 0 ? 'credit' : 'debit',
      createdBy: this.userID
    };

    this.httpService.httpPost(API_URLS.save_WalletTransaction, transactionData).subscribe({
      next: (response) => {
        console.log('Transaction saved successfully', response);
        this.getWalletTransactions(); // Refresh transactions after saving
      },
      error: (error) => {
        console.error('Error saving transaction', error);
      }
    });
  }

  generateTransactionNumber(): string {
    return `TXN-${Date.now()}-${Math.floor(Math.random() * 10000)}`;
  }

  getWalletTransactions(): void {
    // Use the correct API for getting wallet transactions by UserID
    this.httpService.httpGet(API_URLS.get_WalletTransactionByID, { userID: this.userID }).subscribe({
      next: (res: any) => {
        if (res?.data) {
          this.recentTransactions = res.data.map((tx: any) => {
            // Ensure transaction type is set correctly
            const isCredit =
              tx.mode?.toLowerCase().includes('recharge') ||
              tx.message?.toLowerCase().includes('recharge') ||
              tx.description?.toLowerCase().includes('recharge');
            // Prioritize backend's transactionType if available, otherwise infer
            tx.transactionType = tx.transactionType || (isCredit ? 'credit' : 'debit');
            return tx;
          });
          this.totalTransactions = this.recentTransactions.length;
          this.currentPage = 1;
          this.processExistingWallets(this.recentTransactions); 
        } else {
          this.recentTransactions = [];
          this.totalTransactions = 0;
        }
      },
      error: (err) => {
        console.error("Error fetching wallet transactions:", err);
        this.recentTransactions = [];
        this.totalTransactions = 0;
      }
    });
  }

  // This method is no longer needed if get_Wallet is now get_WalletByUserID
  // getWalletByWalletID(walletId: number): void {
  //   console.warn("getWalletByWalletID is deprecated. Use getWalletsByUserID instead.");
  //   // You might keep this if you still need to fetch by WalletID for other reasons,
  //   // but based on your API_URLs, it seems get_Wallet is now for UserID.
  // }


  formatDate(date: string): string {
    if (!date) return '';
    return new Date(date).toLocaleDateString('en-IN');
  }

  getTransactionIcon(type: string): string {
    return type === 'credit' ? 'fa-arrow-down' : 'fa-arrow-up';
  }

  getTransactionColor(type: string): string {
    return type === 'credit' ? 'text-success' : 'text-danger';
  }

  getTransactionSign(type: string): string {
    return type === 'credit' ? '+' : '-';
  }

  get paginatedTransactions(): any[] {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    return this.recentTransactions.slice(startIndex, startIndex + this.pageSize);
  }

  changePage(page: number): void {
    this.currentPage = page;
  }

  get totalPages(): number {
    return Math.ceil(this.totalTransactions / this.pageSize);
  }

  get pageNumbers(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  // Helper method to check if a form is being submitted
  isSubmitting(): boolean {
    return this.isSavingWallet;
  }
}