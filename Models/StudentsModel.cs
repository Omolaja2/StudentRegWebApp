using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Student_MVC.Models
{
    public class StudentsModel
    {
        [Key]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Student Full Name is required")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Phone number is required!")]
        [Phone(ErrorMessage = "Enter a valid phone number")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string? Gender { get; set; }
        

    }
}

    
