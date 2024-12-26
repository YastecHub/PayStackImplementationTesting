using Microsoft.AspNetCore.Mvc;
using PayStackImplementationTesting.Dtos.RequestModel;
using PayStackImplementationTesting.IPayStackServices;

namespace PayStackImplementationTesting.Controllers
{
    public class PayStackController : Controller
    {
        private readonly IPayStackService _paystackService;

        public PayStackController(IPayStackService paystackService)
        {
            _paystackService = paystackService;
        }

        [HttpGet("InitiatePaymentForm")]
        public IActionResult InitiatePaymentForm()
        {
            // This will render a view that contains a form for initiating payment
            return View();
        }

        [HttpPost("InitiatePayment")]
        public async Task<IActionResult> InitiatePayment(InitializePaymentRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return View("InitiatePaymentForm", requestDto); // Return the form with the model data if invalid
            }

            var result = await _paystackService.InitializePaymentAsync(requestDto);

            if (result.IsSuccess)
            {
                // Redirect to a success page or return a view with the result
                return Redirect(result.Data.AuthorizationUrl);
            }

            // Handle the failure by showing an error message
            ModelState.AddModelError(string.Empty, result.Message);
            return View("InitiatePaymentForm", requestDto);
        }

        [HttpGet("VerifyPayment")]
        public IActionResult VerifyPaymentForm()
        {
            // Render a view that contains a form for entering the payment reference
            return View();
        }

        [HttpGet("PaymentCallback")]
        public async Task<IActionResult> PaymentCallback(string reference)
        {
            if (string.IsNullOrEmpty(reference))
            {
                return RedirectToAction("InitiatePayment"); // Redirect to the payment initiation if no reference is provided
            }

            // Call the service to verify the payment using the reference
            var result = await _paystackService.VerifyPaymentAsync(reference);

            if (result.IsSuccess)
            {
                // Payment successful, display the success view
                ViewBag.Reference = reference;
                return View("PaymentSuccess");
            }
            else
            {
                // Payment failed, display an error or handle as needed
                ModelState.AddModelError(string.Empty, "Payment verification failed. Please try again.");
                return View("PaymentFailed");
            }
        }


        public IActionResult PaymentSuccess(string reference)
        {
            // You can show the payment success page
            ViewBag.Reference = reference;
            return View();
        }
    }
}
