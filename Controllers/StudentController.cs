using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using StudentApp.Models;

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

        // Trang thông tin cá nhân sinh viên
        public IActionResult Profile()
        {
            string studentCode = HttpContext.Session.GetString("UserId");
            var student = _studentService.GetStudentByCode(studentCode);
            if (student == null)
                return NotFound();

            return View(student);
        }

        // [HttpGet]
        // public IActionResult Timetable()
        // {
        //     return View();
        // }

        [HttpGet]
        public IActionResult Timetable(int? hocKy, string? namHoc)
        {
            // Lấy mã sinh viên từ session
            var studentCode = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(studentCode))
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy danh sách năm học từ bảng Course (distinct)
            var years = _context.Courses
                        .Select(c => c.NamHoc)
                        .Distinct()
                        .OrderByDescending(y => y)
                        .ToList();
            ViewBag.Years = years;

            var timetable = _studentService.GetThoiKhoaBieuBySemester(studentCode, hocKy, namHoc);
            return View(timetable);
        }

        // Cập nhật thông tin sinh viên (POST)
        // [HttpPost]
        // public IActionResult Profile(Student updatedStudent)
        // {
        //     var student = _studentService.GetStudentByCode(updatedStudent.MaSV);
        //     if (student != null)
        //     {
        //         _studentService.UpdateStudent(updatedStudent);
        //         TempData["Success"] = "Cập nhật thông tin thành công!";
        //     }

        //     return RedirectToAction("Profile");
        // }

        // Hiển thị danh sách học phần để đăng ký
        // public IActionResult Courses()
        // {
        //     var courses = _courseService.GetCourses();
        //     return View(courses);
        // }

        // // Đăng ký học phần (POST)
        // [HttpPost]
        // public IActionResult RegisterCourses(string maHP)
        // {
        //     string maSV = HttpContext.Session.GetString("UserId");
        //     if (string.IsNullOrEmpty(maSV))
        //         return RedirectToAction("Index", "Login");

        //     var student = _studentService.GetStudentByCode(maSV);
        //     if (student == null)
        //         return NotFound();

        //     if (!_courseService.IsStudentRegistered(maSV, maHP))
        //     {
        //         _courseService.RegisterStudentToCourse(maSV, maHP);
        //         TempData["Success"] = "Đăng ký học phần thành công!";
        //     }
        //     else
        //     {
        //         TempData["Error"] = "Bạn đã đăng ký học phần này rồi!";
        //     }

        //     return RedirectToAction("Courses");
        // }

        // // Hủy đăng ký học phần (POST)
        // [HttpPost]
        // public IActionResult UnregisterCourse(string maHP)
        // {
        //     string maSV = HttpContext.Session.GetString("UserId");
        //     if (string.IsNullOrEmpty(maSV))
        //         return RedirectToAction("Index", "Login");

        //     var student = _studentService.GetStudentByCode(maSV);
        //     if (student == null)
        //         return NotFound();

        //     _courseService.UnregisterStudentFromCourse(maSV, maHP);
        //     TempData["Success"] = "Hủy đăng ký học phần thành công!";

        //     return RedirectToAction("Timetable");
        // }

        // Hiển thị danh sách học phần đã đăng ký
        // public IActionResult Timetable()
        // {
        //     string maSV = HttpContext.Session.GetString("UserId");
        //     if (string.IsNullOrEmpty(maSV))
        //         return RedirectToAction("Index", "Login");

        //     var student = _studentService.GetStudentByCode(maSV);
        //     if (student == null)
        //         return NotFound();

        //     var registeredCourses = _courseService.GetStudentCourses(maSV);
        //     return View(registeredCourses);
        // }
    }
}
