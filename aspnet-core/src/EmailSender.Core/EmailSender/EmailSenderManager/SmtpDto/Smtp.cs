using Abp.Configuration;
using Abp.Net.Mail;
using System.Threading.Tasks;

namespace EmailSender.EmailSender.EmailSenderManager.SmtpDto
{
    public class SmtpSettingsDto
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public bool EnableSsl { get; set; }
        public string SenderEmail { get; set; }
    }

    //public class SmtpSettingsMethod
    //{
    //    private readonly ISettingManager _settingManager;

    //    public SmtpSettingsMethod(ISettingManager settingManager)
    //    {
    //        _settingManager = settingManager;
    //    }     
    //}

    public static class SmtpSettingsHelper
    {
        public static async Task<SmtpSettingsDto> GetSmtpSettingsAsync(ISettingManager settingManager, int tenantId)
        {
            return new SmtpSettingsDto
            {
                Host = await settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Host, tenantId),
                Port = await settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Port, tenantId),
                UserName = await settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.UserName, tenantId),
                Password = await settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Password, tenantId),
                SenderEmail = await settingManager.GetSettingValueForTenantAsync(EmailSettingNames.DefaultFromAddress, tenantId),
                EnableSsl = bool.TryParse(await settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.EnableSsl, tenantId), out var enableSsl) && enableSsl

            };
        }
    }
}
