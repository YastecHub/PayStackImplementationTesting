using Newtonsoft.Json;

namespace PayStackImplementationTesting.Dtos.ResponseModel
{
    public class PaystackResponseDto<T>
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
