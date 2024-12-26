namespace PayStackImplementationTesting.Dtos.ResponseModel
{
    public class PaymentVerificationResponseDto
    {
        public string Reference { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string GatewayResponse { get; set; }
        public DateTime VerificationDate { get; set; }
    }
}
