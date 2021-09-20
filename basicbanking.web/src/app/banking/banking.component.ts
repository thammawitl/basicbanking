import { Component, Input, OnInit } from '@angular/core';
import { AbstractControl, FormControl, Validators } from '@angular/forms';
import { BankAccount } from '../common/models/bankaccounts';
import { BankingResource } from '../common/resource/banking.service';
import Swal from 'sweetalert2'

@Component({
  selector: 'app-banking',
  templateUrl: './banking.component.html',
  styleUrls: ['./banking.component.scss']
})
export class BankingComponent implements OnInit {
  private _account: BankAccount;

  @Input() set account(bankaccount: BankAccount){
    this._account = bankaccount;
    this.refreshBankList();
  }

  get account(): BankAccount{
    return this._account;
  }

  transferChoice: BankAccount;
  bankList: BankAccount[]
  transactionChoice: number ;
  amountDeposit: number;
  amountTransferControl = new FormControl('', Validators.required) ;

  constructor(
    private bankingResource: BankingResource
    ) { }

  ngOnInit(): void {
    this.bankingResource.getBankAccountList().subscribe( res => {
      this.bankList = (res.filter(x => x.id != this.account.id))
    })
  }


  refreshBankList() {
    Swal.showLoading();
    this.bankingResource.getBankAccountList().subscribe( res => {
      this.bankList = (res.filter(x => x.id != this.account.id))
      Swal.close();
    })
  }

  makeDeposit(){
    Swal.showLoading();
    this.bankingResource.makeDepositByAccountId(this.account.id, this.amountDeposit).subscribe( (res: any) => {
      this.account.balance += res;
      Swal.fire({
        icon: 'success',
        title: `${this.amountDeposit} Cash Deposit!`,
        showConfirmButton: false,
        timer: 1500
      })
    }, error => {
      Swal.fire({
        icon: 'error',
        title: `${error.error}`,
        showConfirmButton: false,
        timer: 1500
      })
    })
  }

  makeTransfer(){
    this.bankingResource.makeTransfer(this.account.id, this.transferChoice.id, this.amountTransferControl.value).subscribe( res => {
      this.transferChoice.balance = res;
      this.account.balance -= this.amountTransferControl.value;
      Swal.fire({
        icon: 'success',
        title: 'Transfer Complete!',
        text: `${this.amountTransferControl.value} transfered to ${this.transferChoice.id}`,
        showConfirmButton: false,
        timer: 1500
      })
    }, error => {
      Swal.fire({
        icon: 'error',
        title: `${error.error}`,
        showConfirmButton: false,
        timer: 1500
      })
    })
  }

  selectionChange(e: any){
    this.transferChoice = e.value;
  }

}
