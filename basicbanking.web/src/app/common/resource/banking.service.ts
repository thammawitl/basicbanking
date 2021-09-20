import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class BankingResource {
    constructor(private http: HttpClient) { }
    resourceURL = 'localhost:5001/api/';

    // Get Lists of User Avaliable with BankAccount
    getUserList() {
        return this.http.get(this.resourceURL + 'user');
    }

    // Get Lists of BankAccount without User
    getBankAccountList() {
        return this.http.get(this.resourceURL + 'bankaccount');
    }

    // Add BankAccount to Current Selected User
    addBankAccountByUserId(userId : Number) {
        return this.http.post(this.resourceURL + 'bankaccount', {'userId': userId});
    }

    // Add money to Selected BankAccount with Fee
    makeDepositByUserId(accountId : Number, amount: Number) {
        return this.http.post(this.resourceURL + 'bankaccount', {'itemId': accountId, 'amount': amount});
    }

    // Transfer money from first account to second account without fee
    makeTransfer(accountId_1 : Number, accountId_2 : Number, amount: Number) {
        return this.http.post(this.resourceURL + 'bankaccount', {'item1Id': accountId_1, 'item2Id': accountId_2, 'amount': amount});
    }

}
