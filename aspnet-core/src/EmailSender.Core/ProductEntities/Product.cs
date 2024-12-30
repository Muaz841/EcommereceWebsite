using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using EmailSender.ProductEntities;

namespace EmailSender.ProductDomain
{
    public class Product : FullAuditedEntity<int>, IMustHaveTenant, ISoftDelete
    {
      
        public string Name { get; set; }
        
        public bool IsActive { get; set; }

        public byte[] Thumbnail { get; set; }

        public int TenantId { get; set; }

        public bool main { get; set; }

        public virtual ICollection<ProductDetail> ProductDetails { get; set; }

        public virtual ICollection<ProductReview> ProductReviews { get; set; }
        public virtual ICollection<ProductMedia> ProductMedia { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }    

}
