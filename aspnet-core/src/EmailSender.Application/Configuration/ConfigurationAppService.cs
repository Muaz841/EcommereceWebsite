using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using EmailSender.Configuration.Dto;

namespace EmailSender.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : EmailSenderAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
