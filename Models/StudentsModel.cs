using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student_MVC.Models
{
    public class StudentsModel
    {
        
        public string? FullName {get; set; }
        public string? PhoneNumber {get; set; }
        public int DateOfBirth {get; set; }
        public int ID {get; set; }
        public string? Gender {get; set; }
        
    }
}