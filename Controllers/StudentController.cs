using Microsoft.AspNetCore.Mvc;
using Student_MVC.Models;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Student_MVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _contexts;
        private readonly EmailSettings _emailSettings;

        public StudentController(ApplicationDbContext context, IOptions<EmailSettings> emailSettings)
        {
            _contexts = context;
            _emailSettings = emailSettings.Value;
        }

        
        public IActionResult Index()
        {
            var students = _contexts.Students.ToList();
            return View(students);
        }

        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("AdminUsername") == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Register(StudentsModel student)
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login", "Admin");
            }

            if (ModelState.IsValid)
            {
                _contexts.Students.Add(student);
                await _contexts.SaveChangesAsync();
                await SendingEmail(student.Email!);

                return RedirectToAction("Index");
            }

            return View(student);
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _contexts.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentsModel student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _contexts.Update(student);
                    await _contexts.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_contexts.Students.Any(e => e.StudentId == student.StudentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(student);
        }

    
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _contexts.Students.FirstOrDefaultAsync(s => s.StudentId == id);
            if (student == null) return NotFound();

            _contexts.Students.Remove(student);
            await _contexts.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    
        private async Task SendingEmail(string email)
        {
            var studentemailmessage = new MimeMessage();
            studentemailmessage.From.Add(new MailboxAddress("MamzyHub📚", _emailSettings.SmtpUser));
            studentemailmessage.To.Add(new MailboxAddress("", email));
            studentemailmessage.Subject = "Welcome to MamzyHub Registration System📚!";

            var builder = new BodyBuilder
            {
                HtmlBody = $@"
                <div style='font-family:Segoe UI,Roboto,sans-serif;background:#f4f4f4;padding:40px 20px;'>
                <div style='max-width:600px;margin:0 auto;background:#ffffff;border-radius:10px;overflow:hidden;box-shadow:0 4px 20px rgba(0,0,0,0.1);'>
                <div style='background:#2c3e50;color:#ffffff;padding:30px;text-align:center;'>
                    <h1 style='margin:0;font-size:28px;'>🎓 Mamzy Student Registration</h1>
                </div>
                <div style='padding:30px;'>
                    <h2 style='color:#2c3e50;font-size:24px;margin-bottom:15px;'>Welcome, Student!</h2>
                         <p style='font-size:16px;color:#555;'>You've successfully registered on the <strong>MamzyHub📚 Student Registration System</strong>.</p>
                        <p style='font-size:16px;color:#555;margin-top:15px;'>We're thrilled to have you on board! If you ever need help or have questions, feel free to reply to this email or reach out to the admin.</p>

                <div style='margin:30px 0;text-align:center;'>
                            <a href='http://localhost:5180/Student' style='background:#2c3e50;color:#fff;text-decoration:none;padding:12px 25px;border-radius:5px;font-weight:bold;'>Visit Portal</a>
                </div>

                     <p style='font-size:14px;color:#888;margin-top:20px;'>This is an automated message. Please do not reply directly to this Email.</p>
            </div>
            <div style='background:#ecf0f1;color:#7f8c8d;text-align:center;padding:15px;font-size:13px;'>
                &copy; {DateTime.Now.Year} MamzyHub Registration. All rights reserved🎓.
            </div>
        </div>
    </div>"

            };

            studentemailmessage.Body = builder.ToMessageBody();
            using var cl = new SmtpClient();
            try
            {
                await cl.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, false);
                await cl.AuthenticateAsync(_emailSettings.SmtpUser, _emailSettings.SmtpPassword);
                await cl.SendAsync(studentemailmessage);
                await cl.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email:{ex.Message}");
            }
        }
    }
}
