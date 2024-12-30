using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.EmailSender.QueueEmail.QueueEmailDto
{
    public class QueuedEmailDto : Entity<int>
    {
        public int? TenantId { get; set; }
        public string? EmailPriority { get; set; }
        public string From { get; set; }
        public string? FromName { get; set; }
        public string To { get; set; }
        public string ToName { get; set; }
        public string? ReplyTo { get; set; }
        public string? ReplyToName { get; set; }
        public string? Cc { get; set; }
        public string? Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string? Status { get; set; }
        public int RetryCount { get; set; }
    }

    public class QueuePagedDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public string status { get; set; }
        public string sorting { get; set; }
    }
}
