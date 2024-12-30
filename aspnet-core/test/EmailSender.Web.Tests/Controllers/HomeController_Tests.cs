using System.Threading.Tasks;
using EmailSender.Models.TokenAuth;
using EmailSender.Web.Controllers;
using Shouldly;
using Xunit;

namespace EmailSender.Web.Tests.Controllers
{
    public class HomeController_Tests: EmailSenderWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}