using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace StudentApp.Controllers
{
    public class TeacherController : Controller
    {
        private readonly DataContext _context;

        public TeacherController(DataContext context)
        {
            _context = context;
        }

        // ✅ Hiển thị danh sách giáo viên
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teachers.ToListAsync());
        }

        // ✅ Hiển thị thông tin giảng viên đang đăng nhập
        public async Task<IActionResult> Profile()
        {
            string teacherCode = HttpContext.Session.GetString("TeacherCode");
            if (string.IsNullOrEmpty(teacherCode))
            {
                return RedirectToAction("Index", "Login");
            }

            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Code == teacherCode);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // ✅ Thêm khóa học
        public IActionResult AddCourse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // ✅ Chỉnh sửa khóa học
        public async Task<IActionResult> EditCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> EditCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Update(course);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // ✅ Xóa khóa học
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // ✅ Nhập điểm
        public IActionResult AddGrade()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddGrade(Grade grade)
        {
            if (ModelState.IsValid)
            {
                _context.Grades.Add(grade);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(grade);
        }

        // ✅ Chỉnh sửa điểm
        public async Task<IActionResult> EditGrade(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null)
            {
                return NotFound();
            }
            return View(grade);
        }

        [HttpPost]
        public async Task<IActionResult> EditGrade(Grade grade)
        {
            if (ModelState.IsValid)
            {
                _context.Grades.Update(grade);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(grade);
        }

        // ✅ Xóa điểm
        public async Task<IActionResult> DeleteGrade(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null)
            {
                return NotFound();
            }

            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
