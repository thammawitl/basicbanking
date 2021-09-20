using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace basicbanking.api.Controllers.Models
{
    public class Deposit
    {
        public long itemId { get; set; }

        public float amount { get; set; }
    }
}
