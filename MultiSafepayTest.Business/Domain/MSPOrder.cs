using MultiSafePayTest.Business.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiSafepayTest.Business.Domain
{
    public class MSPOrder: Entity
    {
        public string OrderId { get; set; }
        public MSPOrderStatus Status { get; set; }
    }
}
