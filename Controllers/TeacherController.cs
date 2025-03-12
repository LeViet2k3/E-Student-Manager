using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApp.Models;

namespace StudentApp.Controllers;

public class TeacherController : Controller
{
    private readonly DataContext _context;

    public TeacherController(DataContext context)
    {
        _context = context;
    }

    // Danh sách giáo viên
    public async Task<IActionResult> Index()
    {
        return View(await _context.Teachers.ToListAsync());
    }

    // Thêm khóa học
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

    // Nhập điểm
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
}
