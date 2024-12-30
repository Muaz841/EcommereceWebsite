using System;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.UI;
using Abp.Zero.Configuration;
using EmailSender.Authorization.Accounts.Dto;
using EmailSender.Authorization.Users;
using EmailSender.EmailSender;
using EmailSender.EmailSender.EmailSenderManager;
using Microsoft.AspNetCore.Identity;

namespace EmailSender.Authorization.Accounts
{
    public class AccountAppService : EmailSenderAppServiceBase, IAccountAppService
    {
        // from: http://regexlib.com/REDetails.aspx?regexp_id=1923
        public const string PasswordRegex = "(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$";
        private readonly IEmailSenderManager _emailSenderManager;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly UserManager _userManager;

        public AccountAppService(
            IEmailSenderManager emailSenderManager,
        UserRegistrationManager userRegistrationManager,
        UserManager userManager)
        {
            _emailSenderManager = emailSenderManager;
            _userRegistrationManager = userRegistrationManager;
            _userManager = userManager;
        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
            }

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
        }

        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            // Register the user
            var user = await _userRegistrationManager.RegisterAsync(
                input.Name,
                input.Surname,
                input.EmailAddress,
                input.UserName,
                input.Password,
                false // Assumed email address is always confirmed. Change this if you want to implement email confirmation.
            );

            if (user == null || !user.TenantId.HasValue)
            {
                throw new UserFriendlyException("Please select a valid tenant!");
            }

           
            var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

           
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            //var verificationUrl = $"https://localhost:44311/api/UserVerification?userId={user.Id}&token={Uri.EscapeDataString(token)}";
            var verificationUrl = $"http://localhost:4200/app/verification/verify/{user.Id}/{Uri.EscapeDataString(token)}";

           
            var mailData = new EmailSenderDto
            {
                useremail = user.EmailAddress,
                username = user.UserName,
                verificationurl = verificationUrl,
                userId = user.Id.ToString()
            };

           
            await _emailSenderManager.SendEmailAsync(mailData, "userverification");
            
            return new RegisterOutput
            {
                CanLogin = user.IsActive && (user.IsEmailConfirmed || isEmailConfirmationRequiredForLogin)
            };
        }
    }
}
