using System.Threading.Tasks;
using EmailSender.Configuration.Dto;

namespace EmailSender.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
