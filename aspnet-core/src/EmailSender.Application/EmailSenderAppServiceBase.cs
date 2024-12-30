using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using EmailSender.Authorization.Users;
using EmailSender.MultiTenancy;
using EmailSender.Authorization.Roles;

namespace EmailSender
{

    public abstract class EmailSenderAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }
        private RoleManager roleManager;
        public UserManager UserManager { get; set; }

        protected EmailSenderAppServiceBase()
        {
            LocalizationSourceName = EmailSenderConsts.LocalizationSourceName;
        }

        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }
            
            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
