using basicbanking.api.Data;
using basicbanking.api.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpPost]
        [Route("deposit")]
        //Deposit
        public float Deposit(long itemId, float amount)
        {
            BankAccount account = base.GetItemById(itemId);

            // Fee 0.1% = 0.001
            account.Balance = (float)(account.Balance + (amount - (amount * 0.001)));
            account.Balance = (float)Math.Round(account.Balance * 100f) / 100f;
            base.UpdateItem(account);
            return account.Balance;
        }

        [HttpPost]
        [Route("transfer")]
        //Transfer
        public IActionResult TransferCash(long item1Id, long item2Id, float amount)
        {
            BankAccount account1 = base.GetItemById(item1Id);
            BankAccount account2 = base.GetItemById(item2Id);
            if(account1.Balance < amount)
            {
                return BadRequest($"Balance Exceed Limit, {amount} exceed {account1.Balance}");
            }
            account1.Balance -= amount;
            account2.Balance += amount;
            base.UpdateItem(account1);
            base.UpdateItem(account2);
            return Ok(account2.Balance);
        }
    }
}
