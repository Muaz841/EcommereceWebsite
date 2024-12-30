using Abp.Application.Services.Dto;
using EmailSender.EmailSender.EmailSenderEntities;
using EmailSender.EmailSender.QueueEmail.QueueEmailDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.EmailSender.QueueEmail
{
    public interface IQueuedEmailManager
    {
        Task AddQueueEmailAsync(QueuedEmailDto email);
        Task<List<QueuedEmailDto>> GetPendingEmailsAsync();
        Task UpdateEmailStatusAsync(int emailId, string status);
        Task IncrementRetryCountAsync(int emailId);
        Task<PagedResultDto<QueuedEmailDto>> GetAllEmailsInQueueAsync(QueuePagedDto input);
        Task<QueuedEmailDto> GetQueueMailById(int emailId);

        Task<List<QueuedEmailDto>> GetPendingEmailsTWAsync(int tenantid);

        Task UpdateFailedMails(int emailId);
    }

}
