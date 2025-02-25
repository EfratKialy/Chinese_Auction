namespace project.Models.DTO
{
    public class GiftDTO
    {
        public int DonorId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int NumOfPurchases { get; set; }
        public string Image { get; set; }
    }
}
