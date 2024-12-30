using Abp.Domain.Entities;
using Scriban;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.EmailSender.EmailSenderEntities
{
    public class EmailTemplate : Entity<int> , ISoftDelete , IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Bcc { get; set; }
        public bool? IsActive { get; set; }

        public string Token { get; set; }
        public string Cc { get; set; }

        public bool IsDeleted { get; set; }
    }

}
