using System;
using Xunit;
using basicbanking.api.Data;
using basicbanking.api.Controllers;
using basicbanking.api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using basicbanking.api.Controllers.Models;

namespace basicbanking.api.test
{
    public class ControllerTest : IDisposable
    {
        public UsersController _userController;
        public BankAccountController _bankaccountController;

        public void Dispose()
        {

        }

        public void InitTest()
        {
            var options = new DbContextOptionsBuilder<PostgresDbContext>().UseInMemoryDatabase(databaseName: "basicbank").Options;
            var services = new ServiceCollection();
            services.AddTransient<BankAccountController>();
            services.AddTransient<UsersController>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<DbContext>((s) =>
            {
                return new PostgresDbContext(options);
            });
            var serviceProvider = services.BuildServiceProvider();

            this._bankaccountController = serviceProvider.GetService<BankAccountController>();
            this._userController = serviceProvider.GetService<UsersController>();

            User initUser = new User
            {
                Name = "Test User"
            };
            this._userController.Add(initUser);
            BankAccount account1 = new BankAccount
            {
                IBAN = "NL66ABNA7951708135",
                Balance = 1000,
                UserId = initUser.Id
            };
            BankAccount account2 = new BankAccount
            {
                IBAN = "NL30INGB7467539436",
                Balance = 500,
                UserId = initUser.Id
            };
            this._bankaccountController.Add(account1);
            this._bankaccountController.Add(account2);
        }
    }

    public class BankAccountControllerTest : IClassFixture<ControllerTest>
    {
        private ControllerTest _controllerInstance;

        public BankAccountControllerTest(ControllerTest controllerInstance)
        {
            this._controllerInstance = controllerInstance;
        }

        [Fact]
        public void Test_DepositBalance()
        {
            this._controllerInstance.InitTest();

            Deposit deposit = new Deposit
            {
                itemId = 1,
                amount = 500
            };
            this.Test_DepositBalanceHandler(deposit, (float)499.50);
            deposit.itemId = 2;
            deposit.amount = 750;
            this.Test_DepositBalanceHandler(deposit, (float)1749.25);
            deposit.itemId = 3;
            deposit.amount = 1000;
            this.Test_DepositBalanceHandler(deposit, (float)1499);

            this._controllerInstance.Dispose();
        }

        [Fact]
        public void Test_TransferCash()
        {
            this._controllerInstance.InitTest();

            Transfer transfer = new Transfer
            {
                item1Id = 2,
                item2Id = 1,
                amount = 500,
            };

            this.Test_TransferCashHandler(transfer, 500, 500);
            transfer.item1Id = 1;
            transfer.item2Id = 3;
            transfer.amount = 250;
            this.Test_TransferCashHandler(transfer, 250, 750);
            transfer.item1Id = 2;
            transfer.item2Id = 3;
            transfer.amount = 250;
            this.Test_TransferCashHandler(transfer, 250, 1000);
            transfer.item1Id = 3;
            transfer.item2Id = 1;
            transfer.amount = 1000;
            this.Test_TransferCashHandler(transfer, 0, 1250);

            this._controllerInstance.Dispose();
        }

        public void Test_DepositBalanceHandler(Deposit deposit, float expectedBalance)
        {
            var balance = this._controllerInstance._bankaccountController.Deposit(deposit);

            Assert.Equal<float>(expectedBalance, balance);
        }

        public void Test_TransferCashHandler(Transfer transfer, float expectedBalance1, float expectedBalance2)
        {
            BankAccount account1 = this._controllerInstance._bankaccountController.GetItemById(transfer.item1Id);
            BankAccount account2 = this._controllerInstance._bankaccountController.GetItemById(transfer.item2Id);
            account1.Balance -= transfer.amount;
            account2.Balance += transfer.amount;
            this._controllerInstance._bankaccountController.UpdateItem(account1);
            this._controllerInstance._bankaccountController.UpdateItem(account2);
            Assert.Equal<float>(expectedBalance1, account1.Balance);
            Assert.Equal<float>(expectedBalance2, account2.Balance);
        }


    }
}
