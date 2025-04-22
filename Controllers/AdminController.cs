using Microsoft.AspNetCore.Mvc;
using Student_MVC.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Student_MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

    
        [HttpPost]
        public IActionResult Login(AdminModel admin)
        {
            var existingAdmin = _context.Admins
                .FirstOrDefault(a => a.Username == admin.Username && a.Password == admin.Password);

            if (existingAdmin != null)
            {
                HttpContext.Session.SetString("AdminUsername", existingAdmin.Username!);
                HttpContext.Session.SetString("AdminLoggedIn", "true");
                return RedirectToAction("Index", "Student");
            }

            ViewBag.Message = "Invalid credentials. Please try again.";
            return View();
        }



    }
}
