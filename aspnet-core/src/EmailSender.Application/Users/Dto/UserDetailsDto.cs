using Abp.Domain.Entities;
using EmailSender.order.OrderDtos;
using MimeKit.Tnef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.Users.Dto
{
    public class UserDetailsDto
    {
        public virtual List<OrderDto> Orders { get; set; }

        public long userId { get; set; }

        public string Useremail { get; set; }

        public string Username { get; set; }

        public string UserPhone { get; set; }

        public int processingOrders {  get; set; }

        public int cancelledOrders { get; set; }

        public byte[] UserThumbnail {  get; set; }

        public bool IsActive { get; set; }

    }
 
}
