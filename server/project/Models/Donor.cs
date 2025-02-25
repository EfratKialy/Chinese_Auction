using System.ComponentModel.DataAnnotations;

namespace project.Models
{
    public class Donor
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public IEnumerable<Gift> Gifts { get; set; }

    }
}
