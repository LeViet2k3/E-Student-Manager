using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApp.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using StudentApp.Services;

namespace StudentApp.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacherService _teacherService;
        private readonly DataContext _context;
     
        public TeacherController(ITeacherService teacherService, DataContext context)
        {
            _teacherService = teacherService;
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

        public IActionResult Profile()
        {
            string teacherCode = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(teacherCode))
            {
                return RedirectToAction("Login", "Account");
            }

            var teacher = _teacherService.GetTeacherByCode(teacherCode);

            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        [HttpPost]
        public IActionResult UpdateProfile(Teacher teacher)
        {
            _teacherService.UpdateTeacher(teacher);
            TempData["SuccessMessage"] = "Cập nhật thông tin giảng viên thành công!";
            return RedirectToAction("Profile");
        }


    }
}
