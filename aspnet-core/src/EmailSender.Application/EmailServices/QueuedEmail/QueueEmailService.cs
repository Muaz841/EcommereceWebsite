using Abp.Application.Services;
using Abp.Application.Services.Dto;
using EmailSender.EmailSender.EmailSenderEntities;
using EmailSender.EmailSender.EmailTempalateManagers;
using EmailSender.EmailSender.EmailTempalateManagers.EmailDto;
using EmailSender.EmailSender.QueueEmail;
using EmailSender.EmailSender.QueueEmail.QueueEmailDto;
using EmailSender.EmailServices.QueueEmail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailSender.EmailService
{

    public  class QueueEmailService : EmailSenderAppServiceBase,IQueueEmailService
    {
        private readonly IEmailTemplateManager _emailtemplate;
        private readonly IQueuedEmailManager _queueemail;

        public QueueEmailService(IEmailTemplateManager emailtemplate, IQueuedEmailManager queueemail)
        {
            _emailtemplate = emailtemplate ?? throw new ArgumentNullException(nameof(emailtemplate)); 
           _queueemail = queueemail ?? throw new ArgumentNullException(nameof(emailtemplate)); 
        }

        public async Task<PagedResultDto<QueuedEmailDto>> GetEmailsInQueue(QueuePagedDto input)
        {

            return await _queueemail.GetAllEmailsInQueueAsync(input);
        }

        public async Task <QueuedEmailDto> GetEmailsInQueueById(int id)
        {
            return await _queueemail.GetQueueMailById(id);
        }

        public async  Task UpdateFailedMails(int id)
        {
             await  _queueemail.UpdateFailedMails(id);
        }
        
    }
}
