    using Abp.Configuration;
using Abp.MultiTenancy;
using Abp.Net.Mail;
using Abp.Runtime.Session;
using Abp.UI;
using Castle.Facilities.TypedFactory.Internal;
using EmailSender.EmailSender;
using EmailSender.EmailSender.EmailSenderManager.SmtpDto;
using EmailSender.EmailServices.QueueEmail;
using System;
using System.Threading.Tasks;


namespace EmailSender.EmailServices.EmailSettings
{
    public class SmtpSettingsService : EmailSenderAppServiceBase
    {
        private readonly ISettingManager _settingManager;
        private readonly IAbpSession _abpSession;
        private readonly IEmailSenderManager _emailSenderManager;
        private readonly IQueueEmailService _queueEmailService;
        public SmtpSettingsService(ISettingManager settingManager, IAbpSession abpSession, IQueueEmailService queueEmailService, IEmailSenderManager emailSenderManager)
        {
            _settingManager = settingManager;
            _abpSession = abpSession;
            _queueEmailService = queueEmailService;
            _emailSenderManager = emailSenderManager;
        }

        public async Task<SmtpSettingsDto> GetSmtpSettingsAsync()
        {
            if (!_abpSession.TenantId.HasValue)
            {
                throw new UserFriendlyException("Please Add SMTP SETTINGS.");
            }
            var Id = _abpSession.TenantId.Value;

            var settings = new SmtpSettingsDto
            {
                Host = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Host, Id),
                Port = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Port, Id),
                UserName = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.UserName, Id),
                Password = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Password, Id),
                Domain = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Domain, Id),
                EnableSsl = await _settingManager.GetSettingValueForTenantAsync<bool>(EmailSettingNames.Smtp.EnableSsl, Id),
                SenderEmail = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.DefaultFromAddress, Id),
            };

            if (
                String.IsNullOrWhiteSpace(settings.Host) ||
                String.IsNullOrWhiteSpace(settings.Port) ||
                String.IsNullOrWhiteSpace(settings.Password) ||
                String.IsNullOrWhiteSpace(settings.UserName) )               
                {
                throw new UserFriendlyException("Some SMTP settings are missing or empty.");
                 }
            return settings;
        }


        public async Task CreateSmtpSettingsAsync(SmtpSettingsDto input)
        {
            var id = _abpSession.TenantId.HasValue && _abpSession.TenantId.Value != 0 ? _abpSession.TenantId.Value : MultiTenancyConsts.DefaultTenantId;

            await _settingManager.ChangeSettingForTenantAsync(id, EmailSettingNames.Smtp.Host, input.Host);
            await _settingManager.ChangeSettingForTenantAsync(id, EmailSettingNames.Smtp.Port, input.Port.ToString());
            await _settingManager.ChangeSettingForTenantAsync(id, EmailSettingNames.Smtp.UserName, input.UserName);
            await _settingManager.ChangeSettingForTenantAsync(id, EmailSettingNames.Smtp.Password, input.Password);
            await _settingManager.ChangeSettingForTenantAsync(id, EmailSettingNames.Smtp.Domain, input.Domain);
            await _settingManager.ChangeSettingForTenantAsync(id, EmailSettingNames.Smtp.EnableSsl, input.EnableSsl.ToString());
            await _settingManager.ChangeSettingForTenantAsync(id, EmailSettingNames.DefaultFromAddress, input.SenderEmail); 
        }

        public async Task UpdateTenantSmtpSettingsAsync(SmtpSettingsDto input)
        {
            var id = _abpSession.TenantId.HasValue && _abpSession.TenantId.Value != 0 ? _abpSession.TenantId.Value : MultiTenancyConsts.DefaultTenantId;
            // Update the SMTP settings for the specified tenant
            await _settingManager.ChangeSettingForTenantAsync(id, EmailSettingNames.Smtp.Host, input.Host);
            await _settingManager.ChangeSettingForTenantAsync(id, EmailSettingNames.Smtp.Port, input.Port.ToString());
            await _settingManager.ChangeSettingForTenantAsync(id, EmailSettingNames.Smtp.UserName, input.UserName);
            await _settingManager.ChangeSettingForTenantAsync(id, EmailSettingNames.Smtp.Password, input.Password);
            await _settingManager.ChangeSettingForTenantAsync(id, EmailSettingNames.Smtp.Domain, input.Domain);
            await _settingManager.ChangeSettingForTenantAsync(id, EmailSettingNames.Smtp.EnableSsl, input.EnableSsl.ToString());
            await _settingManager.ChangeSettingForTenantAsync(id, EmailSettingNames.DefaultFromAddress, input.SenderEmail);
        }

         public async Task TestMail(string TO)
        {
           await  _emailSenderManager.TestMail(TO);
        }
    }

}

