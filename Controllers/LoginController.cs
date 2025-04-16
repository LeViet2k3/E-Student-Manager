using Microsoft.AspNetCore.Mvc;
using StudentApp.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace StudentApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext _context;

        public LoginController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // ✅ Đăng nhập bằng mã + email
        [HttpPost]
        public IActionResult Login(string code, string email, string role)
        {
            if (role == "Student")
            {
                var student = _context.Students
                    .FirstOrDefault(s => s.MaSV == code && s.Email == email);

                if (student != null)
                {
                    HttpContext.Session.SetString("UserId", student.MaSV);
                    HttpContext.Session.SetString("UserName", student.HoTen);
                    HttpContext.Session.SetString("UserRole", "Student");

                    return RedirectToAction("Index", "Student");
                }
            }
            else if (role == "Teacher")
            {
                var teacher = _context.Teachers
                    .FirstOrDefault(t => t.MaGV == code && t.Email == email);

                if (teacher != null)
                {
                    HttpContext.Session.SetString("UserId", teacher.MaGV);
                    HttpContext.Session.SetString("UserName", teacher.HoTen);
                    HttpContext.Session.SetString("UserRole", "Teacher");

                    return RedirectToAction("Index", "Teacher");
                }
            }

            ViewBag.Error = "Sai mã hoặc email!";
            return View("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
