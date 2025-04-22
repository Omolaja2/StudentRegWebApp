using System.ComponentModel.DataAnnotations;

namespace Student_MVC.Models
{
    public class AdminModel
    {
        [Key]
        public int AdminId { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
