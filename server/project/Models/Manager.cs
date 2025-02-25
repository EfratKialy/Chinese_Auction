using System.ComponentModel.DataAnnotations;

namespace project.Models
{
    public class Manager
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [MaxLength(5)]
        public int Password { get; set; }


    }
}
