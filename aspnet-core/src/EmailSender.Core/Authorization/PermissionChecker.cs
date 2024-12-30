using Abp.Authorization;
using EmailSender.Authorization.Roles;
using EmailSender.Authorization.Users;

namespace EmailSender.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
