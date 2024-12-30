using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.order.OrderDtos
{
    public class OrderCountDto
    {
        public int ProcessingOrders {  get; set; }

        public int ShippedOrders { get; set; }

        public int DeliveredOrders { get; set; }

        public int CancelledOrders { get; set; }
    }
}
