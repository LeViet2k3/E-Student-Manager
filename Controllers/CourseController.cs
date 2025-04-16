using Microsoft.AspNetCore.Mvc;
using StudentApp.Models;
using Microsoft.EntityFrameworkCore; 

namespace StudentApp.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly DataContext _context;

        public CourseController(ICourseService courseService, DataContext context)
        {
            _courseService = courseService;
            _context = context;
        }

        public IActionResult Index()
        {
            string maSV = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(maSV))
            {
                TempData["Error"] = "Bạn cần đăng nhập để xem danh sách học phần.";
                return RedirectToAction("Index", "Student");
            }

            var courses = _courseService.GetCourses();
            
            var registeredCourseIds = _context.CourseRegistrations
                .Include(r => r.CourseClasses)
                .ThenInclude(cc => cc.Course)
                .Where(r => r.MaSV == maSV)
                .Select(r => r.CourseClasses.Course.MaHP)
                .Distinct()
                .ToList();

            ViewBag.RegisteredCourseIds = registeredCourseIds;
            ViewBag.MaSV = maSV;  // Truyền maSV vào View nếu cần

            return View(courses);
        }




        [HttpPost]
        public IActionResult Register(string maSV, string maHP)
        {
            if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(maHP))
                return BadRequest();

            bool success = _courseService.RegisterStudentToCourse(maSV, maHP);

            if (success)
                TempData["Message"] = "Đăng ký học phần thành công!";
            else
                TempData["Error"] = "Đã đăng ký học phần này rồi hoặc xảy ra lỗi.";

            return RedirectToAction("Index", new { maSV });
        }

        [HttpPost]
        public IActionResult Unregister(string maSV, string maHP)
        {
            if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(maHP))
                return BadRequest();

            bool success = _courseService.UnregisterStudentFromCourse(maSV, maHP);

            if (success)
                TempData["Message"] = "Hủy đăng ký thành công!";
            else
                TempData["Error"] = "Không thể hủy đăng ký.";

            return RedirectToAction("Index", new { maSV });
        }
    }
}
