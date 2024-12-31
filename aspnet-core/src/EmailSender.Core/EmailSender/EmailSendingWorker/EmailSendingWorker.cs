using Abp.Dependency;
using Abp.Threading.BackgroundWorkers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using EmailSender.EmailSender.QueueEmail;
using System.Net.Mail;
using System.Net;
using Abp.Threading.Timers;
using Abp.Domain.Uow;
using Abp.Configuration;
using EmailSender.MultiTenancy;
using Abp.Domain.Repositories;
using static EmailSender.EmailSender.EmailSenderManager.SmtpDto.SmtpSettingsMethod;
using System;

namespace EmailSender.EmailSender.EmailWorker
{
    public class EmailSendingWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IQueuedEmailManager _emailQueueRepository;
        private readonly ILogger<EmailSendingWorker> _logger;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ISettingManager _settingManager;
        private readonly IRepository<Tenant, int> _tenantRepository;

        public EmailSendingWorker(
            IUnitOfWorkManager unitOfWorkManager,
            AbpTimer timer,
            IQueuedEmailManager emailQueueRepository,
            IEmailSenderManager emailSenderManager,
            IConfiguration configuration,
            ISettingManager settingManager,
            ILogger<EmailSendingWorker> logger,
            IRepository<Tenant, int> tenantRepository) : base(timer)
        {
            _settingManager = settingManager;
            _emailQueueRepository = emailQueueRepository;
            _logger = logger;
            Timer.Period = (5000 * 1000);
            _unitOfWorkManager = unitOfWorkManager;
            _tenantRepository = tenantRepository;
        }

        protected override async void DoWork()
        
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                try  
                {
                                        
                        int tenantId = 1;
                        var smtpSettings = await SmtpSettingsHelper.GetSmtpSettingsAsync(_settingManager, tenantId);
                        var emails = await _emailQueueRepository.GetPendingEmailsTWAsync(tenantId);

                      

                        foreach (var email in emails)
                        {
                            try
                            {
                                if (smtpSettings == null ||
                                    string.IsNullOrEmpty(smtpSettings.Host) ||
                                    string.IsNullOrEmpty(smtpSettings.Port.ToString()) ||
                                    string.IsNullOrEmpty(smtpSettings.UserName) ||
                                    string.IsNullOrEmpty(smtpSettings.Password))
                                {
                                    await _emailQueueRepository.IncrementRetryCountAsync(email.Id);
                                    continue;
                                }

                                using (var client = new SmtpClient(smtpSettings.Host, Int32.Parse(smtpSettings.Port))
                                {
                                    Credentials = new NetworkCredential(smtpSettings.UserName, smtpSettings.Password),
                                    EnableSsl = smtpSettings.EnableSsl
                                })
                                {
                                    var mailMessage = new MailMessage
                                    {
                                        From = new MailAddress(smtpSettings.SenderEmail),
                                        Subject = email.Subject,
                                        Body = email.Body,
                                        IsBodyHtml = true
                                    };
                                    mailMessage.To.Add(email.To);

                                    // Send the email
                                    await client.SendMailAsync(mailMessage);
                                    await _emailQueueRepository.UpdateEmailStatusAsync(email.Id, "sent");
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"Error sending email ID {email.Id}. Incrementing retry count.");
                                await _emailQueueRepository.IncrementRetryCountAsync(email.Id);
                            }
                        }
                    
                }
                finally  // Finally block for completing the unit of work
                {
                    await unitOfWork.CompleteAsync();
                }
            }
        }
    }
}
