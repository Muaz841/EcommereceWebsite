
using EmailSender.Controllers;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Threading.Tasks;

namespace EmailSender.order.Stripe
{
    [Route("api/[controller]/[action]")]
    public class StripePaymentController : EmailSenderControllerBase
    {

        [HttpPost]
        public async Task<string> CreatePaymentIntent(decimal amount, string email)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100),
                Currency = "usd",                
            };

            var service = new PaymentIntentService();
            PaymentIntent paymentIntent = await service.CreateAsync(options);

            return paymentIntent.ClientSecret;
        }
    }
}
