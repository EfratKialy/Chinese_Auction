using System.ComponentModel.DataAnnotations;

namespace project.Models
{
    public class Gift
    {
        [Key]
        public int Id { get; set; }
        public int DonorId { get; set; }
        public int WinnerId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int NumOfPurchases { get; set; }
        public string Image { get; set; }
        public Donor Donor { get; set; }
        public Category Category { get; set; }

    }
}
