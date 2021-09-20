import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/users'
import { BankAccount } from '../models/bankaccounts';

@Injectable()
export class BankingResource {
    constructor(private http: HttpClient) { }
    resourceURL = 'https://localhost:5001/api/';

    // Get Lists of User Avaliable with BankAccount
    getUserList() {
        return this.http.get<User[]>(this.resourceURL + 'users');
    }

    // Get Lists of BankAccount without User
    getBankAccountList() {
        return this.http.get<BankAccount[]>(this.resourceURL + 'bankaccount');
    }

    // Get BankAccount list with User id
    getBankAccountListByUserId(userId : Number) {
        return this.http.get<BankAccount[]>(this.resourceURL + `bankaccount/accountbyuser/${userId}`);
    }

    // Add BankAccount to Current Selected User
    addBankAccountByUserId(userId : Number) {
        return this.http.post<BankAccount>(this.resourceURL + 'bankaccount', {'userId': userId});
    }

    // Add money to Selected BankAccount with Fee
    makeDepositByUserId(accountId : Number, amount: Number) {
        return this.http.post<Number>(this.resourceURL + 'bankaccount', {'itemId': accountId, 'amount': amount});
    }

    // Transfer money from first account to second account without fee
    makeTransfer(accountId_1 : Number, accountId_2 : Number, amount: Number) {
        return this.http.post<Number>(this.resourceURL + 'bankaccount', {'item1Id': accountId_1, 'item2Id': accountId_2, 'amount': amount});
    }

}
