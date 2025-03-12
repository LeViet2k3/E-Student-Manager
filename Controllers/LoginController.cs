using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using StudentApp.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext _context;

        public LoginController(DataContext context)
        {
            _context = context;
        }

        // ✅ Hiển thị trang đăng nhập
        public IActionResult Index()
        {
            return View();
        }

        // ✅ Xử lý đăng nhập
        [HttpPost]
        public async Task<IActionResult> Login(string Code, string Name)
        {
            // 📌 Kiểm tra xem có phải Student không
            var student = _context.Students.FirstOrDefault(s => s.Code == Code && s.Name == Name);
            if (student != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, student.Id.ToString() ?? ""), // ✅ Đảm bảo giá trị là string
                    new Claim(ClaimTypes.Name, student.Name ?? ""), // ✅ Tránh lỗi null
                    new Claim(ClaimTypes.Role, "Student")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                HttpContext.Session.SetString("StudentCode", student.Code);
                HttpContext.Session.SetString("UserRole", "Student");

                return RedirectToAction("Index", "Home"); // 🔹 Chuyển đến trang Student
            }

            // 📌 Kiểm tra xem có phải Teacher không
            var teacher = _context.Teachers.FirstOrDefault(t => t.Code == Code && t.Name == Name);
            if (teacher != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, teacher.TeacherId.ToString() ?? ""),
                    new Claim(ClaimTypes.Name, teacher.Name ?? ""),
                    new Claim(ClaimTypes.Role, "Teacher")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                HttpContext.Session.SetString("TeacherCode", teacher.Code);
                HttpContext.Session.SetString("UserRole", "Teacher");

                return RedirectToAction("Index", "Teacher"); // 🔹 Chuyển đến trang Teacher
            }

            // ❌ Nếu không tìm thấy
            ViewBag.Error = "Mã số hoặc tên không đúng!";
            return View("Index");
        }

        // 🔥 Xử lý đăng xuất
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("UserCode");
            HttpContext.Session.Remove("UserRole");
            return RedirectToAction("Index", "Login");
        }
    }
}
