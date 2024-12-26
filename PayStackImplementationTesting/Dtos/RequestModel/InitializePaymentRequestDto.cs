namespace PayStackImplementationTesting.Dtos.RequestModel
{
    public class InitializePaymentRequestDto
    {
        public decimal Amount { get; set; }
        public string Email { get; set; }
        public string? Reference { get; set; }
    }
}
