namespace PayStackImplementationTesting.Models.Entities
{
    public class PaymentRequest
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public string TransactionReference { get; set; }
        public string Status { get; set; }
        public DateTime DateRequested { get; set; } = DateTime.Now;
        //public DateTime CreatedOn { get; internal set; }
    }
}
