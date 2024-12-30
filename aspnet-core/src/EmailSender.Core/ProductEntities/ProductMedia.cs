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
    public class ProductMedia : Entity<int>
    {
        public byte[] image { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public int TenantId { get; set; }


        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
