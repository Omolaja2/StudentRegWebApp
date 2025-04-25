using Microsoft.AspNetCore.Mvc;
using Student_MVC.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Student_MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _contexts;

        public AdminController(ApplicationDbContext context)
        {
            _contexts = context;
        }

        public IActionResult Login()
        {
            return View();
        }

    
        [HttpPost]
        public IActionResult Login(AdminModel admindetails)
        {
            var IfAdminexist = _contexts.Admins
                .FirstOrDefault(a => a.Username == admindetails.Username && a.Password == admindetails.Password);

            if (IfAdminexist != null)
            {
                HttpContext.Session.SetString("AdminUsername", IfAdminexist.Username!);
                HttpContext.Session.SetString("AdminLoggedIn", "true");
                return RedirectToAction("Index", "Student");
            }

            ViewBag.Message = "Invalid credentials. Please try again!.";
            return View();
        }

    }
}
