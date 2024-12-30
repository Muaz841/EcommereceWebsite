using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using EmailSender.Authorization.Users;
using EmailSender.ProductDomain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace EmailSender.ProductEntities
{
    public class ProductReview : FullAuditedEntity<int>
    {
        public long UserId { get; set; }
        public int ProductId { get; set; }
        public string review { get; set; }
        public int ratings { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }  

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
