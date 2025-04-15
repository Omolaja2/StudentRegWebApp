using Microsoft.AspNetCore.Mvc;
using Student_MVC.Models;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Options;

namespace Student_MVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailSettings _emailSettings;

                public StudentController(ApplicationDbContext context, IOptions<EmailSettings> emailSettings)
        {
            _context = context;
            _emailSettings = emailSettings.Value;
        }

        public IActionResult Index()
        {
            var students = _context.Students.ToList();
            return View(students);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(StudentsModel student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();

               
                await SendEmailAsync(student.Email!);

                return RedirectToAction("Index");
            }

            return View(student);
        }

   private async Task SendEmailAsync(string email)
{
    var message = new MimeMessage();
    message.From.Add(new MailboxAddress("MamzyHub", _emailSettings.SmtpUser));
    message.To.Add(new MailboxAddress("", email));
    message.Subject = "Welcome to MamzyHub Registration System!";

    var bodyBuilder = new BodyBuilder
    {
        HtmlBody = $@"
            <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f9f9f9;'>
                <div style='max-width: 600px; margin: auto; background-color: white; border-radius: 10px; padding: 30px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                    <h2 style='color: #2c3e50;'>Registered, Student!</h2>
                    <p style='font-size: 16px; color: #34495e;'>
                        Thank you for registering with <strong>MamzyStudentReg Student Registration System</strong>.
                        We're excited to have you on board!
                    </p>
                    <p style='font-size: 15px; color: #7f8c8d;'>
                        If you have any questions or need help, feel free to reply to this email Many Thanks.
                    </p>
                    <br />
                    <p style='font-size: 14px; color: #95a5a6;'>Best regards,<br><strong>MamzyHub Team.</strong></p>
                </div>
            </div>"
    };

    message.Body = bodyBuilder.ToMessageBody();

    using var client = new SmtpClient();
    try
    {
        await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, false);
        await client.AuthenticateAsync(_emailSettings.SmtpUser, _emailSettings.SmtpPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error sending email: {ex.Message}");
    }
}


    }
}
