using System.ComponentModel.DataAnnotations;

namespace project.Models.DTO
{
    public class CustomerDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [MaxLength(5)]
        public string Password { get; set; }

    }
}
