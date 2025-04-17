using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApp.Models;

namespace StudentApp.Controllers
{
    public class GradeController : Controller
    {
        private readonly DataContext _context;

        public GradeController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? hocKy, string namHoc)
        {
            // Tạm thời hard-code mã sinh viên
            var maSV = HttpContext.Session.GetString("UserId");

            var namHocList = await _context.Courses
            .Select(c => c.NamHoc)
            .Distinct()
            .OrderByDescending(n => n)
            .ToListAsync();

            ViewBag.NamHocList = namHocList;
            // Join bảng Grade → CourseClass → Course
            var query = _context.Grades
                .Include(g => g.CourseClasses)
                    .ThenInclude(cc => cc.Course)
                .Include(g => g.Students)
                .Where(g => g.MaSV == maSV);

            if (hocKy.HasValue)
            {
                query = query.Where(g => g.CourseClasses.Course.KiHoc == hocKy);
            }

            if (!string.IsNullOrEmpty(namHoc))
            {
                query = query.Where(g => g.CourseClasses.Course.NamHoc == namHoc);
            }

            var result = await query.ToListAsync();
            return View(result);
        }

        // Hiển thị danh sách lớp học phần cho chọn trước khi nhập điểm
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

        public async Task<IActionResult> EnterGrade(string maLHP)
        {
            var studentGrades = await _context.CourseRegistrations
                .Where(r => r.MaLHP == maLHP)
                .Include(r => r.Students)
                .Select(r => new
                {
                    Student = r.Students,
                    Grade = _context.Grades
                        .FirstOrDefault(g => g.MaSV == r.MaSV && g.MaLHP == maLHP)
                })
                .ToListAsync();

            ViewBag.MaLHP = maLHP;

            // Gửi dữ liệu sang View bằng dạng dynamic (hoặc bạn có thể tạo ViewModel như đã nói ở trên)
            return View(studentGrades);
        }


        // 3. Nhập điểm cho sinh viên (hiển thị form)
        public IActionResult ThemDiem(string maSV, string maLHP)
        {
            ViewBag.MaSV = maSV;
            ViewBag.MaLHP = maLHP;
            return View();
        }

        // 4. Xử lý lưu điểm (POST)
        [HttpPost]
        public async Task<IActionResult> ThemDiem(string maSV, string maLHP, double diemQT, double diemThi)
        {
            var diemTong = Math.Round(diemQT * 0.4 + diemThi * 0.6, 2);

            // Tìm điểm hiện có (nếu có)
            var existingGrade = await _context.Grades
                .FirstOrDefaultAsync(g => g.MaSV == maSV && g.MaLHP == maLHP);

            if (existingGrade != null)
            {
                // Nếu đã có điểm → cập nhật lại
                existingGrade.DiemQT = diemQT;
                existingGrade.DiemThi = diemThi;
                existingGrade.DiemTong = diemTong;

                _context.Grades.Update(existingGrade);
            }
            else
            {
                // Nếu chưa có → thêm mới
                var diem = new Grade
                {
                    MaDiem = Guid.NewGuid().ToString(),
                    MaSV = maSV,
                    MaLHP = maLHP,
                    DiemQT = diemQT,
                    DiemThi = diemThi,
                    DiemTong = diemTong
                };

                _context.Grades.Add(diem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("EnterGrade", new { maLHP = maLHP });
        }

    }
}
