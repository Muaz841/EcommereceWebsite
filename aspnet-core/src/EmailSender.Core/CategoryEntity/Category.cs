using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.CategoryEntity
{
    public class Category : FullAuditedEntity<int>, IMustHaveTenant, ISoftDelete
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Sold { get; set; }

        public decimal quantity { get; set; }

        public string Thumbnail { get; set; }
        public int TenantId { get; set; }

    }
}
