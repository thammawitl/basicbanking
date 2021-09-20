import { BankAccount } from "./bankaccounts"

export class User {
    id: number;
    name: string;
    bankaccounts: [
        BankAccount
    ]
}
