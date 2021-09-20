import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BankingResource } from '../common/resource/banking.service'
import Swal from 'sweetalert2'
import { User } from '../common/models/users';
import { BankAccount } from '../common/models/bankaccounts';
import { FormControl, Validators } from '@angular/forms';


@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})

export class UserComponent implements OnInit {
  userList: User[];

  userselectControl = new FormControl('', Validators.required);
  accountModel: BankAccount

  constructor(
    private bankingResource: BankingResource
  ) { }

  ngOnInit(): void {
    this.loadUserData();
  }

  addAccount(){
    Swal.showLoading();
    this.bankingResource.addBankAccountByUserId(this.userselectControl.value.id).subscribe(x => {
      this.loadUserData();
      this.userselectControl.value.bankAccounts.push(x);
    })
    Swal.fire();
  }

  loadUserData(){
    Swal.showLoading()
    this.bankingResource.getUserList().subscribe((user) => {
      this.userList = user;
    });
    Swal.close();
  }  

  selectionChange(e: any){
    this.accountModel = e.value
  }
}
