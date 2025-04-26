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

        public async Task<IActionResult> SelectCourseClass()
        {
            string maGV = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(maGV))
            {
                return Content("Không tìm thấy mã giảng viên trong session. Đã đăng nhập chưa?");
            }
            var classes = await _context.CourseClasses
                .Include(c => c.Course)
                .Where(c => c.MaGV == maGV)
                .ToListAsync();

            return View("SelectCourseClass", classes);
        }

        public async Task<IActionResult> ViewClassList(string maLHP)
        {
            var studentGrades = await _context.CourseRegistrations
                .Where(r => r.MaLHP == maLHP)
                .Include(r => r.Students)
                .Select(r => new
                {
                    MaSV = r.Students.MaSV,
                    HoTen = r.Students.HoTen,
                    NgaySinh = r.Students.NgaySinh,
                    Email = r.Students.Email,
                    Khoa = r.Students.Khoa,
                    Nganh = r.Students.Nganh
                })
                .ToListAsync();

            ViewBag.MaLHP = maLHP;

            return View("ViewClassList", studentGrades);
        }

        public async Task<IActionResult> TeachingSchedule(string namHoc, int? hocKy)
        {
            // Lấy maGV từ Session
            var maGV = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(maGV))
            {
                return RedirectToAction("Login", "Account"); // Nếu không có mã giảng viên trong session, chuyển hướng về trang đăng nhập
            }

            var schedules = _context.TeachingSchedules
                .Include(ts => ts.CourseClasses)
                    .ThenInclude(cc => cc.Course)
                .Where(ts => ts.MaGV == maGV); // Lọc theo giảng viên

            if (!string.IsNullOrEmpty(namHoc))
                schedules = schedules.Where(ts => ts.CourseClasses.Course.NamHoc == namHoc);

            if (hocKy.HasValue)
                schedules = schedules.Where(ts => ts.CourseClasses.Course.KiHoc == hocKy.Value);

            // Lấy danh sách các năm học
            var yearsList = await _context.Courses.Select(c => c.NamHoc).Distinct().ToListAsync();
            ViewBag.Years = yearsList ?? new List<string>(); // Đảm bảo không null

            return View(await schedules.ToListAsync());
        }

    }
}
