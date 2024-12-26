using Newtonsoft.Json;
using PayStackImplementationTesting.Dtos.RequestModel;
using PayStackImplementationTesting.Dtos.ResponseModel;
using PayStackImplementationTesting.IPayStackServices;
using PayStackImplementationTesting.Models.Data;
using PayStackImplementationTesting.Models.Entities;
using System.Text;

namespace PayStackImplementationTesting.PayStackServices
{
    public class PayStackService : IPayStackService
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _dbcontext;
        private readonly string _secretKey;

        public PayStackService(HttpClient httpClient, ApplicationDbContext dbcontext, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _dbcontext = dbcontext;
            _secretKey = configuration["Paystack:SecretKey"];
        }
        public async Task<BaseResponse<InitializePaymentResponseDto>> InitializePaymentAsync(InitializePaymentRequestDto requestDto)
        {
            try
            {
                // Create a new payment request record in the database
                var paymentRequest = new PaymentRequest
                {
                    Email = requestDto.Email,
                    Amount = requestDto.Amount,
                    Status = "Pending",
                    DateRequested = DateTime.Now,
                    TransactionReference = Guid.NewGuid().ToString("N"),
                    //CreatedOn = DateTime.Now,
                };

                _dbcontext.PaymentRequests.Add(paymentRequest);
                await _dbcontext.SaveChangesAsync();

                string callbackUrl = "https://localhost:7009/PaymentCallback";
                // Prepare the request payload
                var requestPayload = new
                {
                    amount = requestDto.Amount * 100, // Convert amount to kobo
                    email = requestDto.Email.Trim(),
                    reference = paymentRequest.TransactionReference,
                    callback_url = callbackUrl
                };

                // Convert the payload to JSON and create the StringContent object
                var requestBody = new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json");

                // Create the HttpRequestMessage
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.paystack.co/transaction/initialize")
                {
                    Headers = { Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _secretKey) },
                    Content = requestBody
                };

                // Send the request and get the response
                var response = await _httpClient.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response content
                    var paystackResponse = JsonConvert.DeserializeObject<PaystackResponseDto<InitializePaymentResponseDto>>(responseContent);

                    if (paystackResponse != null && paystackResponse.Status)
                    {
                        var initializeResponse = paystackResponse.Data;

                        // Update the payment request status in the database
                        paymentRequest.Status = "Initialized";
                        await _dbcontext.SaveChangesAsync();

                        return new BaseResponse<InitializePaymentResponseDto>
                        {
                            Message = "Payment initialization successful",
                            IsSuccess = true,
                            Data = initializeResponse
                        };
                    }
                    else
                    {
                        return new BaseResponse<InitializePaymentResponseDto>
                        {
                            Message = $"Payment initialization failed. Response: {paystackResponse?.Message ?? "Unknown error"}",
                            IsSuccess = false,
                            Data = null
                        };
                    }
                }
                else
                {
                    return new BaseResponse<InitializePaymentResponseDto>
                    {
                        Message = $"Payment initialization failed. Status Code: {response.StatusCode}. Response: {responseContent}",
                        IsSuccess = false,
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<InitializePaymentResponseDto>
                {
                    Message = $"An error occurred while initializing payment: {ex.Message}",
                    IsSuccess = false,
                    Data = null
                };
            }
        }


        public async Task<BaseResponse<VerifyPaymentRequestDto>> VerifyPaymentAsync(string reference)
        {
            try
            {
                // Create the HTTP request to Paystack API
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://api.paystack.co/transaction/verify/{reference}");
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _secretKey);

                // Send the request to Paystack API
                var response = await _httpClient.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new BaseResponse<VerifyPaymentRequestDto>
                    {
                        Message = $"Payment verification failed. Status Code: {response.StatusCode}. Response: {responseContent}",
                        IsSuccess = false
                    };
                }

                // Deserialize the response content to your DTO
                var verifyResponse = JsonConvert.DeserializeObject<VerifyPaymentRequestDto>(responseContent);

                return new BaseResponse<VerifyPaymentRequestDto>
                {
                    Message = "Payment verification successful",
                    IsSuccess = true,
                    Data = verifyResponse
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<VerifyPaymentRequestDto>
                {
                    Message = $"An error occurred while verifying payment: {ex.Message}",
                    IsSuccess = false
                };
            }
        }

    }
}
