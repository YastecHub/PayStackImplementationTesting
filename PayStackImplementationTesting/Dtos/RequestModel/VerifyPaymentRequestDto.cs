namespace PayStackImplementationTesting.Dtos.RequestModel
{
    public class VerifyPaymentRequestDto
    {
        public string Reference { get; set; }
        public string RecipientName { get; set; }
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
        public int Amount { get; set; }
        public string Reason { get; set; }
        public string TransferCode { get; set; }
        public string Status { get; set; }
        public DateTime DateRequested { get; set; }
    }
}
