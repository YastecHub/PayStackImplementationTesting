using Newtonsoft.Json;

namespace PayStackImplementationTesting.Dtos.ResponseModel
{
    public class InitializePaymentResponseDto
    {
        [JsonProperty("authorization_url")]
        public string AuthorizationUrl { get; set; }

        [JsonProperty("access_code")]
        public string AccessCode { get; set; }

        [JsonProperty("reference")]
        public string TransactionReference { get; set; }
    }
}
