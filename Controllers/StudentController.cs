using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using StudentApp.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;


namespace StudentApp.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly DataContext _context;

        public StudentController(IStudentService studentService, DataContext context)
        {
            _studentService = studentService;
            _context = context;
        }

        // Hiển thị trang chính của sinh viên
        public IActionResult Index()
        {
            string studentCode = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(studentCode))
                return RedirectToAction("Index", "Login");

            var student = _studentService.GetStudentByCode(studentCode);
            if (student == null)
                return NotFound();
            ViewBag.StudentName = student.HoTen;
            return View(student);
        }

        public IActionResult Profile()
        {
            string studentCode = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(studentCode))
            {
                return RedirectToAction("Login", "Account"); 
            }

            // Lấy thông tin sinh viên từ dịch vụ
            var student = _studentService.GetStudentByCode(studentCode); 

            // Kiểm tra nếu không tìm thấy sinh viên
            if (student == null)
            {
                return NotFound();
            }

            // Trả về View Profile với thông tin sinh viên
            return View(student); 
        }


        [HttpPost]
        public IActionResult UpdateProfile(Student student)
        {
                _studentService.UpdateStudent(student); 
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                return RedirectToAction("Profile", new { maSV = student.MaSV });
        }

        [HttpGet]
        public IActionResult Timetable(int? hocKy, string? namHoc)
        {
            var studentCode = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(studentCode))
            {
                return RedirectToAction("Login", "Account");
            }

            // Truy vấn thời khóa biểu của sinh viên đang đăng nhập
            var query = _context.CourseRegistrations
                .Include(x => x.CourseClasses)
                .ThenInclude(cc => cc.Course)
                .Where(x => x.MaSV == studentCode)
                .AsQueryable();

            if (hocKy.HasValue)
                query = query.Where(x => x.CourseClasses.Course.KiHoc == hocKy.Value);

            if (!string.IsNullOrEmpty(namHoc))
                query = query.Where(x => x.CourseClasses.Course.NamHoc == namHoc);

            var list = query.ToList();

            // Lấy danh sách năm học từ dữ liệu của sinh viên hiện tại
            var years = _context.CourseRegistrations
                .Include(x => x.CourseClasses)
                .ThenInclude(cc => cc.Course)
                .Where(x => x.MaSV == studentCode)
                .Select(x => x.CourseClasses.Course.NamHoc)
                .Distinct()
                .OrderByDescending(y => y)
                .ToList();

            ViewBag.Years = years;

            return View(list);
        }
    }
}
