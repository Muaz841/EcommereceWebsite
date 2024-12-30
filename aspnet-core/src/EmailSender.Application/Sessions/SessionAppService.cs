using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Authorization.Roles;
using Abp.Domain.Repositories;
using EmailSender.Sessions.Dto;
using Org.BouncyCastle.Security;
using Abp.Authorization.Users;

namespace EmailSender.Sessions
{
    public class SessionAppService : EmailSenderAppServiceBase, ISessionAppService
    {
     
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>()
                }
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }

            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());            
            }

            return output;
        }
    }
}
