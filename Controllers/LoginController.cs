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
            var student = _context.Students.FirstOrDefault(s => s.Code == Code && s.Name == Name);

            if (student != null)
            {
                // 🔹 Tạo danh sách Claims (chứa thông tin người dùng)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, student.Id.ToString()), // ✅ Lưu ID sinh viên
                    new Claim(ClaimTypes.Name, student.Name) // ✅ Lưu tên sinh viên
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // 🔹 Xác thực đăng nhập
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                HttpContext.Session.SetString("StudentCode", student.Code);
                // Lưu mã số sinh viên vào Session
                return RedirectToAction("Index", "Home"); // 🔹 Chuyển đến trang chính sau khi đăng nhập thành công
            }

            // ❌ Nếu không tìm thấy sinh viên, hiển thị lỗi
            ViewBag.Error = "Mã sinh viên hoặc tên không đúng!";
            return View("Index");
        }

        // 🔥 Action xử lý đăng xuất
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
