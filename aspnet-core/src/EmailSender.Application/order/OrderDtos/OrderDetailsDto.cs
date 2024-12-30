using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.order.OrderDtos
{
    public  class OrderDetailsDto
    {
        public virtual List<OrderProductDto> OrderProducts { get; set; }

        public string ShippingAddress { get; set; }
        public string CustomerName { get; set; }

        public string CustomerMail { get; set; }

        public string Customerphone { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
