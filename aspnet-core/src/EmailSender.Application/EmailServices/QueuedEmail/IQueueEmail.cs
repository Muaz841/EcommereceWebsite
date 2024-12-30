using Abp.Application.Services;
using Abp.Application.Services.Dto;
using EmailSender.EmailSender.QueueEmail.QueueEmailDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailSender.EmailServices.QueueEmail
{
    public interface IQueueEmailService 
    {
        Task<PagedResultDto<QueuedEmailDto>> GetEmailsInQueue(QueuePagedDto input);
       
    }
}
