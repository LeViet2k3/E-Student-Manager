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

            // Xác định năm học và học kỳ hiện tại nếu người dùng chưa chọn
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;

            // Mặc định học kỳ: tháng 9-12 hoặc 1-2 => Học kỳ 1, còn lại => Học kỳ 2
            string currentHocKy = (currentMonth >= 9 || currentMonth <= 2) ? "1" : "2";
            string currentNamHoc = (currentMonth >= 9)
                ? $"{currentYear}-{currentYear + 1}"
                : $"{currentYear - 1}-{currentYear}";

            hocKy ??= currentHocKy;
            namHoc ??= currentNamHoc;

            // Truy vấn học phần theo ngành, học kỳ và năm học
            var courseList = (from st in _context.Students
                            join tp in _context.TrainingPrograms on st.Nganh equals tp.Nganh
                            join c in _context.Courses on tp.MaHP equals c.MaHP
                            join cl in _context.CourseClasses on c.MaHP equals cl.MaHP
                            where st.MaSV == maSV
                                    && c.KiHoc == int.Parse(hocKy)
                                    && c.NamHoc == namHoc
                            select new
                            {
                                c.MaHP,
                                c.TenHP,
                                c.SoTinChi,
                                cl.PhongHoc,
                                cl.Thu,
                                cl.Tiet
                            }).ToList();

            // Lấy danh sách học phần đã đăng ký
            var registeredCourseIds = _context.CourseRegistrations
                .Include(r => r.CourseClasses)
                .ThenInclude(cc => cc.Course)
                .Where(r => r.MaSV == maSV)
                .Select(r => r.CourseClasses.Course.MaHP)
                .Distinct()
                .ToList();

            // Gửi dữ liệu ra View
            ViewBag.RegisteredCourseIds = registeredCourseIds;
            ViewBag.MaSV = maSV;
            ViewBag.HocKy = hocKy;
            ViewBag.NamHoc = namHoc;
            ViewBag.Courses = courseList;

            return View();
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
