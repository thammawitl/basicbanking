using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace basicbanking.api.Domain
{
    public class User : EntityBase
    {
        public string Name { get; set; }

        public virtual ICollection<BankAccount> BankAccounts { get; set; }
    }
}
