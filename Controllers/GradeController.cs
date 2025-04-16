using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApp.Models;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
