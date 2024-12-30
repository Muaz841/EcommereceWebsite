using Abp.Domain.Entities;
using EmailSender.Authorization.Users;
using EmailSender.ProductDomain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.CartEntity
{
    public class Cart : Entity<int>
    {
        public long userID { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("userID")]
        public virtual User User { get; set; }
    }
}
