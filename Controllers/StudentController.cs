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

            return View(student);
        }

        public IActionResult Profile()
        {
            // Lấy mã sinh viên từ session
            string studentCode = HttpContext.Session.GetString("UserId");

            // Kiểm tra nếu mã sinh viên không tồn tại trong session
            if (string.IsNullOrEmpty(studentCode))
            {
                return RedirectToAction("Login", "Account"); // Nếu không có mã sinh viên, chuyển hướng đến trang đăng nhập
            }

            // Lấy thông tin sinh viên từ dịch vụ
            var student = _studentService.GetStudentByCode(studentCode); 

            // Kiểm tra nếu không tìm thấy sinh viên
            if (student == null)
            {
                return NotFound(); // Nếu không tìm thấy sinh viên
            }

            // Trả về View Profile với thông tin sinh viên
            return View(student); 
        }


        [HttpPost]
        public IActionResult UpdateProfile(Student student)
        {
                _studentService.UpdateStudent(student); // Cập nhật thông tin sinh viên
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                return RedirectToAction("Profile", new { maSV = student.MaSV }); // Quay lại trang Profile sau khi cập nhật thành công
        }

        [HttpGet]
        public IActionResult Timetable(int? hocKy, string? namHoc)
        {
            // Lấy mã sinh viên từ session
            var studentCode = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(studentCode))
            {
                return RedirectToAction("Login", "Account");
            }

            // Truy vấn thời khóa biểu của sinh viên đang đăng nhập
            var query = _context.CourseRegistrations
                .Include(x => x.CourseClasses)
                .ThenInclude(cc => cc.Course)
                .Where(x => x.MaSV == studentCode) // Chỉ lấy của sinh viên hiện tại
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
                .Where(x => x.MaSV == studentCode) // Chỉ lấy năm học mà sinh viên có đăng ký
                .Select(x => x.CourseClasses.Course.NamHoc)
                .Distinct()
                .OrderByDescending(y => y)
                .ToList();

            ViewBag.Years = years;

            return View(list);
        }
    }
}
