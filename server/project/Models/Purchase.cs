using System.ComponentModel.DataAnnotations;

namespace project.Models
{

    public class Purchase
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int GiftId { get; set; }
        public bool Status { get; set; }
        public string PaymentMethod { get; set; }

        public Customer Customer { get; set; }

        public Gift Gift { get; set; }
    }
}

