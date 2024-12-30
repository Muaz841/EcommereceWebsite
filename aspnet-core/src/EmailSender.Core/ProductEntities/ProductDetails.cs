using Abp.Domain.Entities;
using EmailSender.ProductDomain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.ProductEntities
{
    public class ProductDetail : Entity<int>
    {

        public int ProductId { get; set; }

        public int Stock { get; set; }

        public string Description { get; set; }

        public decimal BasePrice { get; set; }

        public decimal DiscountedPrice { get; set; }

        public int DiscountId { get; set; }


        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
