using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace EmailSender.Controllers
{
    public abstract class EmailSenderControllerBase: AbpController
    {
        protected EmailSenderControllerBase()
        {
            LocalizationSourceName = EmailSenderConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
