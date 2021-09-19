using System;
using Xunit;
using basicbanking.api.Data;
using basicbanking.api.Controllers;
using basicbanking.api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

            this.Test_DepositBalanceHandler(1, 500, (float)499.50);
            this.Test_DepositBalanceHandler(2, 750, (float)1749.25);
            this.Test_DepositBalanceHandler(3, 1000, (float)1499);

            this._controllerInstance.Dispose();
        }

        [Fact]
        public void Test_TransferCash()
        {
            this._controllerInstance.InitTest();

            this.Test_TransferCashHandler(2, 1, 500, 500, 500);
            this.Test_TransferCashHandler(1, 3, 250, 250, 750);
            this.Test_TransferCashHandler(2, 3, 250, 250, 1000);
            this.Test_TransferCashHandler(3, 1, 1000, 0, 1250);

            this._controllerInstance.Dispose();
        }

        public void Test_DepositBalanceHandler(long acId1, float amount, float expectedBalance)
        {
            var balance = this._controllerInstance._bankaccountController.Deposit(acId1, amount);

            Assert.Equal<float>(expectedBalance, balance);
        }

        public void Test_TransferCashHandler(long acId1, long acId2, float amount, float expectedBalance1, float expectedBalance2)
        {
            BankAccount account1 = this._controllerInstance._bankaccountController.GetItemById(acId1);
            BankAccount account2 = this._controllerInstance._bankaccountController.GetItemById(acId2);
            account1.Balance -= amount;
            account2.Balance += amount;
            this._controllerInstance._bankaccountController.UpdateItem(account1);
            this._controllerInstance._bankaccountController.UpdateItem(account2);
            Assert.Equal<float>(expectedBalance1, account1.Balance);
            Assert.Equal<float>(expectedBalance2, account2.Balance);
        }


    }
}
