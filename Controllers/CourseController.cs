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

        public IActionResult Index(string hocKy, string namHoc)
        {
            string maSV = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(maSV))
            {
                TempData["Error"] = "Bạn cần đăng nhập để xem danh sách học phần.";
                return RedirectToAction("Index", "Login");
            }

            // Lấy năm hiện tại
            int currentYear = DateTime.Now.Year;
            string currentNamHoc = $"{currentYear - 1}-{currentYear}";

            // Nếu người dùng không chọn năm học thì lấy năm học hiện tại
            namHoc ??= currentNamHoc;

            // Lấy danh sách các khóa học
            var courses = _courseService.GetCourses();
            
            // Lọc theo năm học
            courses = courses.Where(c => c.NamHoc == namHoc).ToList();

            // Lọc theo kỳ học nếu có
            if (!string.IsNullOrEmpty(hocKy))
            {
                if (int.TryParse(hocKy, out int hocKyInt))
                {
                    courses = courses.Where(c => c.KiHoc == hocKyInt).ToList();
                }
            }

            // Lấy danh sách các khóa học đã đăng ký của sinh viên
            var registeredCourseIds = _context.CourseRegistrations
                .Include(r => r.CourseClasses)
                .ThenInclude(cc => cc.Course)
                .Where(r => r.MaSV == maSV)
                .Select(r => r.CourseClasses.Course.MaHP)
                .Distinct()
                .ToList();

            ViewBag.RegisteredCourseIds = registeredCourseIds;
            ViewBag.MaSV = maSV;
            ViewBag.HocKy = hocKy;
            ViewBag.NamHoc = namHoc;
            

            return View(courses);
        }




        [HttpPost]
        public IActionResult Register(string maSV, string maHP)
        {
            if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(maHP))
                return BadRequest();

            // Lấy tất cả lớp học phần của học phần này
            var courseClasses = _context.CourseClasses
                .Where(cc => cc.MaHP == maHP)
                .ToList();

            if (courseClasses == null || courseClasses.Count == 0)
            {
                TempData["Error"] = "Không tìm thấy lớp học phần nào cho học phần này.";
                return RedirectToAction("Index", new { maSV });
            }

            // Lấy danh sách lớp học phần đã đăng ký của sinh viên
            var registeredClasses = _context.CourseRegistrations
                .Include(r => r.CourseClasses)
                .Where(r => r.MaSV == maSV)
                .Select(r => r.CourseClasses)
                .ToList();

            // Duyệt qua các lớp học phần của học phần muốn đăng ký
            foreach (var newClass in courseClasses)
            {
                // Phân tích tiết học của lớp học phần mới
                var newTietParts = newClass.Tiet?.Split('-');
                if (newTietParts == null || newTietParts.Length != 2 ||
                    !int.TryParse(newTietParts[0], out int newStart) ||
                    !int.TryParse(newTietParts[1], out int newEnd))
                {
                    continue; // Bỏ qua lớp này nếu tiết không hợp lệ
                }

                // Kiểm tra trùng lịch với các lớp đã đăng ký
                bool isConflict = registeredClasses.Any(rc =>
                {
                    if (!string.Equals(rc.Thu, newClass.Thu, StringComparison.OrdinalIgnoreCase))
                        return false;

                    var tietParts = rc.Tiet?.Split('-');
                    if (tietParts == null || tietParts.Length != 2 ||
                        !int.TryParse(tietParts[0], out int regStart) ||
                        !int.TryParse(tietParts[1], out int regEnd))
                        return false;

                    return Math.Max(newStart, regStart) <= Math.Min(newEnd, regEnd);
                });

                if (!isConflict)
                {
                    // Không trùng lịch → đăng ký lớp học phần này
                    var registration = new CourseRegistration
                    {
                        MaSV = maSV,
                        MaLHP = newClass.MaLHP
                    };

                    _context.CourseRegistrations.Add(registration);
                    _context.SaveChanges();

                    TempData["Message"] = $"Đăng ký học phần thành công vào lớp {newClass.MaLHP}!";
                    return RedirectToAction("Index", new { maSV });
                }
            }

            // Nếu tất cả các lớp học phần đều trùng lịch
            TempData["Error"] = "Không thể đăng ký học phần vì trùng lịch với học phần đã đăng ký.";
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
