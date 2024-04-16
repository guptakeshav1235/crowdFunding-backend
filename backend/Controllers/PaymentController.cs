using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace YourApiNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly string _stripeSecretKey = "sk_test_51Oqs2rK55Ul79p2zjYZk5C1IUgxR6yDfz2bB9cV8orsZSvcTnb02fqJfmYzTzwF2fVJ0oYMDUvG4uKDYHzmaVNNt00MICdFptS"; // Replace with your actual Stripe Secret Key

        [HttpPost("create-payment-intent")]
        public ActionResult<string> CreatePaymentIntent([FromBody] PaymentIntentRequest request)
        {
            StripeConfiguration.ApiKey = _stripeSecretKey;

            var options = new PaymentIntentCreateOptions
            {
                Amount = request.Amount,
                Currency = "usd", // Change the currency code if needed
            };

            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);

            return Ok(new { ClientSecret = paymentIntent.ClientSecret });
        }
    }

    public class PaymentIntentRequest
    {
        public int Amount { get; set; }
        // Add other necessary properties
    }
}
