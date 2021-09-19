using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace basicbanking.api.Domain
{
    public class BankAccount : EntityBase
    {
        public string IBAN { get; set; }

        public float Balance { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; }
    }
}
