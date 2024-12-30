using System.Threading.Tasks;
using Abp.Application.Services;
using EmailSender.Sessions.Dto;

namespace EmailSender.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
