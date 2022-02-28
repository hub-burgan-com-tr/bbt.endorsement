using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Endorsements.Queries.GetOrders
{
    public class OrderItem
    {
        public Guid Id { get; set; }

        public long Customer { get; set; }
        public long Approver { get; set; }
    }
}
