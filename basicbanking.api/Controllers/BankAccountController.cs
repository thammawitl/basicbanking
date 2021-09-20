using basicbanking.api.Data;
using basicbanking.api.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SinKien.IBAN4Net;
using basicbanking.api.Controllers.Models;

namespace basicbanking.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankAccountController : CRUDController<BankAccount>
    {
        private IRepository<BankAccount> _bankRepo;
        public BankAccountController(IRepository<BankAccount> bankRepo) : base(bankRepo)
        {
            this._bankRepo = bankRepo;
        }

        public override IEnumerable<BankAccount> GetItems()
        {
            this._repo.Include(u => u.User);
            return base.GetItems();
        }

        public override BankAccount Add(BankAccount item)
        {
            Random generator = new Random();
            var accountPrefix = generator.Next(0, 99).ToString("D2");
            var accountNum = generator.Next(0, 1000000000).ToString("D10");
            Iban iban = new IbanBuilder()
            .CountryCode(CountryCode.GetCountryCode("NL"))
            .BankCode("AALB")
            .AccountNumberPrefix(accountPrefix)
            .AccountNumber(accountNum)
            .Build();

            item.IBAN = iban.ToString();
            return base.Add(item);
        }

        [HttpGet]
        [Route("accountbyuser/{id}")]
        //Transfer
        public IActionResult GetAccountByUserId(long id)
        {
            var accounts = base.GetItems().Where(x=> x.UserId == id);
            return Ok(accounts);
        }

        [HttpPost]
        [Route("deposit")]
        //Deposit
        public float Deposit(Deposit deposit)
        {
            BankAccount account = base.GetItemById(deposit.itemId);

            // Fee 0.1% = 0.001
            account.Balance = (float)(account.Balance + (deposit.amount - (deposit.amount * 0.001)));
            account.Balance = (float)Math.Round(account.Balance * 100f) / 100f;
            base.UpdateItem(account);
            return account.Balance;
        }

        [HttpPost]
        [Route("transfer")]
        //Transfer
        public IActionResult TransferCash(Transfer transfer)
        {
            BankAccount account1 = base.GetItemById(transfer.item1Id);
            BankAccount account2 = base.GetItemById(transfer.item2Id);
            if(account1.Balance < transfer.amount)
            {
                return BadRequest($"Balance Exceed Limit, {transfer.amount} exceed {account1.Balance}");
            }
            account1.Balance -= transfer.amount;
            account2.Balance += transfer.amount;
            base.UpdateItem(account1);
            base.UpdateItem(account2);
            return Ok(account2.Balance);
        }
    }
}
