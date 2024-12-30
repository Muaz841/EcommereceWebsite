using Abp.Domain.Entities.Auditing;
using EmailSender.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using EmailSender.ProductDomain;

namespace EmailSender.OrderDomain
{
    public class Order : FullAuditedEntity<int> , IMustHaveTenant
    {
        [Required]
        public long CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual User Customer { get; set; }
       
        public decimal TotalPrice { get; set; }
        
        public string PaymentMethod { get; set; }
       
        public int Status { get; set; } 

        public virtual List<OrderProduct> OrderProducts { get; set; } 

        public string ShippingAddress { get; set; }
        public int TenantId {  get; set; }
    }


    public class OrderProduct : Entity<int>
    {                 
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public  virtual Product products {  get; set; }       
        public int Quantity { get; set; }        
    }

}
