using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Configuration;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Net.Mail;


namespace EmailSender.EntityFrameworkCore.Seed.Host
{
    public class DefaultSettingsCreator
    {
        private readonly EmailSenderDbContext _context;

        public DefaultSettingsCreator(EmailSenderDbContext context)
        {
            _context = context;
        }

        public void Create()
        {  
            int? tenantId = null;

            if (EmailSenderConsts.MultiTenancyEnabled == true)
            {
                tenantId = MultiTenancyConsts.DefaultTenantId;
            }

            // Emailing +     //smtp
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, " ", tenantId);
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, " ", tenantId);        
            AddSettingIfNotExists(EmailSettingNames.Smtp.Host, " ", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.Port, " ", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.UserName, " ", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.Password, " ", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.EnableSsl," " , tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.Domain, " false", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.UseDefaultCredentials, " ", tenantId);
            // Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "en", tenantId);

        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.IgnoreQueryFilters().Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}
