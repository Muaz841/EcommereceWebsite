using EmailSender.EmailSender.EmailSenderManager;
using EmailSender.EmailSender.QueueEmail.QueueEmailDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.EmailSender
{
    public interface IEmailSenderManager
    {
        Task<bool> SendEmailAsync(EmailSenderDto input, string templatename);
        Task<bool> TestMail(string To);
    }
}
