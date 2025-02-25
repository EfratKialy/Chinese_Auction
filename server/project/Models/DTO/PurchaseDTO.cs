namespace project.Models.DTO
{
    public class PurchaseDTO
    {
        public int CustomerId { get; set; }
        public int GiftId { get; set; }
        public bool Status { get; set; }
        public string PaymentMethod { get; set; }
    }
}
