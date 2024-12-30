using Abp.Configuration;
using Abp.Dependency;
using Abp.Net.Mail;
using Abp.Runtime.Session;
using EmailSender.EmailSender.EmailTempalateManagers;
using EmailSender.EmailSender.QueueEmail;
using EmailSender.EmailSender.QueueEmail.QueueEmailDto;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using static EmailSender.EmailSender.EmailSenderManager.SmtpDto.SmtpSettingsMethod;
using EmailSender.MultiTenancy;
using EmailSender.Authorization.Users;
using Abp.UI;
using EmailSender.EmailSender.EmailSenderManager;


namespace EmailSender.EmailSender.EmailSender
{
    public class EmailSenderManager : ITransientDependency, IEmailSenderManager
    {
        private readonly IEmailTemplateManager _templateManager;
        private readonly IQueuedEmailManager _queuedEmailManager;
       private readonly IAbpSession _abpSession;        
        private readonly TenantManager _tenatmanager;
        private readonly ISettingManager _settingManager;
        public EmailSenderManager(ISettingManager settingManager, IEmailTemplateManager templateManager, IQueuedEmailManager queuedEmailManager, IConfiguration configuration, IAbpSession abpSession, TenantManager tenatmanager)
        {
            this._templateManager = templateManager;
            this._queuedEmailManager = queuedEmailManager;
            this._settingManager = settingManager;
            _abpSession = abpSession;
            _tenatmanager = tenatmanager;
        }

        public async Task<bool> SendEmailAsync(EmailSenderDto input, string templatename)
        {
           int  tenantId = 1;
            var emailtemplate = await _templateManager.GetTemplateByIdAsync(tenantId, templatename);
           var tenantname =  _tenatmanager.FindById(tenantId).TenancyName;

            var tokenReplacements = new Dictionary<string, string>
        {
            { "{{username}}", input.username },
            { "{{orderamount}}", input.TotolPrice },
            { "{{useremail}}", input.useremail },
            { "{{OrderID}}", input.OrderId },
            { "{{OrderStatus}}", input.OrderStatus },
            {"{{verificationurl}}", input.verificationurl },
        };
           
            emailtemplate.Subject = ReplaceTokens(emailtemplate.Subject, tokenReplacements);
            emailtemplate.Content = ReplaceTokens(emailtemplate.Content, tokenReplacements);

            var emailQueue = new QueuedEmailDto
            {
                To = input.useremail,
                EmailPriority = "High",
                ToName = input.username,
                Subject = emailtemplate.Subject,
                Body = emailtemplate.Content,
                TenantId = tenantId,
                Status = "pending",
                From = tenantname,


                FromName = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.DefaultFromDisplayName, tenantId),
            };            
            await _queuedEmailManager.AddQueueEmailAsync(emailQueue);
            return true;
        }

        private string ReplaceTokens(string template, Dictionary<string, string> tokens)
        {
            if (string.IsNullOrEmpty(template))
                return template;

            foreach (var token in tokens)
            {
                template = template.Replace(token.Key, token.Value);
            }

            return template;
        }

        public async Task<bool> TestMail(string To)
            {

           int TenantId = _abpSession.TenantId.Value;
            var smtpSettings = await SmtpSettingsHelper.GetSmtpSettingsAsync(_settingManager, TenantId);

            using (var client = new SmtpClient(smtpSettings.Host, Int32.Parse(smtpSettings.Port))
            {
                Credentials = new NetworkCredential(smtpSettings.UserName, smtpSettings.Password),
                EnableSsl = smtpSettings.EnableSsl
            })
            {
                // Prepare the MailMessage
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings.SenderEmail),
                    Subject = "Testingmail",
                    Body  = "testing!!!",
                    IsBodyHtml = true
                };

                if(string.Equals(smtpSettings.SenderEmail, To))
                {                    
                        throw new UserFriendlyException("CAN'T SEND TO SENDER");                    
                }
                mailMessage.To.Add(To);

                // Send the email
                await client.SendMailAsync(mailMessage);                
            }

            return true;
            }

   
    }
  

}
