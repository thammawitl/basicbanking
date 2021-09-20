using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace basicbanking.api.Controllers.Models
{
    public class Transfer
    {
        public long item1Id { get; set; }

        public long item2Id { get; set; }

        public float amount { get; set; }
    }
}
