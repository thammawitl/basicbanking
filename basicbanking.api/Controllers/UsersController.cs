using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using basicbanking.api.Data;
using basicbanking.api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SinKien.IBAN4Net;

namespace basicbanking.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : CRUDController<User>
    {
        private IRepository<User> _userRepo;
        private IRepository<BankAccount> _bankRepo;

        public UsersController(IRepository<User> userRepo, IRepository<BankAccount> bankRepo) : base(userRepo)
        {
            this._userRepo = userRepo;
            this._bankRepo = bankRepo;
        }

        public override User Add(User item)
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
            base.Add(item);
            BankAccount bankAccount = new BankAccount
            {
                IBAN = iban.ToString(),
                Balance = 0,
                UserId = item.Id
            };
            this._bankRepo.Insert(bankAccount);
            return item;
        }

        public override IEnumerable<User> GetItems()
        {
            this._repo.Include(u => u.BankAccounts);
            return base.GetItems();
        }

        public override User GetItemById(long id)
        {
            this._repo.Include(u => u.BankAccounts);
            return base.GetItemById(id);
        }
    }
}
