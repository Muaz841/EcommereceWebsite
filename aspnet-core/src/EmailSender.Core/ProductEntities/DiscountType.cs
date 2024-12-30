using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.ProductEntities
{
    public class DiscountType : Entity<int>
    {
        public string DiscountName { get; set; }

        public decimal DiscountPercent { get; set; }

    }
}
