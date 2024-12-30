using System.Threading.Tasks;
using Abp.Application.Services;
using EmailSender.Authorization.Accounts.Dto;

namespace EmailSender.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
