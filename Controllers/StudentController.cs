using Microsoft.AspNetCore.Mvc;
using Student_MVC.Models;

namespace Student_MVC.Controllers
{
    public class StudentController : Controller
    {
  
        private static List<StudentsModel> students = new List<StudentsModel>();

       
        public IActionResult Index()
        {
            return View(students);
        }

        
        public IActionResult Register()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult Register(StudentsModel student)
        {
            if (ModelState.IsValid)
            {
                students.Add(student);
                return RedirectToAction("Index");
            }

            return View(student);
        }
    }
}
