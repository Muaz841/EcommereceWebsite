using Abp.AspNetCore.Mvc.Controllers;
using EmailSender.Authorization.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EmailSender.Controllers
{
    [Route("api/UserVerification")]
    public  class UserVerificationController : EmailSenderControllerBase
    {
        private readonly UserManager _userManager;

        public UserVerificationController(UserManager userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> VerifyEmailAsync(long userId, string token)
        {
           
            var user = await _userManager.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return BadRequest("Email verification failed. Invalid or expired token.");
            }

            return Ok();
        }

    }
}
