using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace project.Models
{
    public class Customer
    { 
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [MaxLength(5)]
        public string Password { get; set; }
        public string Role { get; set; }
        [JsonIgnore]
        public IEnumerable<Purchase> Gifts { get; set; }



    }
}
