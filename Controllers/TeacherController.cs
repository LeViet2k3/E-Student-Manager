using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApp.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;

namespace StudentApp.Controllers
{
    public class TeacherController : Controller
    {
        private readonly DataContext _context;

        public TeacherController(DataContext context)
        {
            _context = context;
        }

        // Trang chủ của giáo viên
        public async Task<IActionResult> Index()
        {
            // Lấy mã giáo viên từ session
            var maGV = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(maGV))
            {
                return RedirectToAction("Index", "Login");
            }

            // Lấy thông tin giáo viên từ DB
            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(t => t.MaGV == maGV);

            if (teacher == null)
            {
                return NotFound("Không tìm thấy thông tin giáo viên.");
            }

            // Nếu cần truyền dữ liệu cho View
            ViewBag.TeacherName = teacher.HoTen;

            return View();
        }
    }
}
