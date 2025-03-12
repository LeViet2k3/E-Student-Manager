using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApp.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;  // ✅ Thêm thư viện để lấy thông tin đăng nhập

public class GradeController : Controller
{
    private readonly DataContext _context;

    public GradeController(DataContext context)
    {
        _context = context;
    }

    // ✅ Hiển thị điểm của sinh viên đăng nhập
    public async Task<IActionResult> Index()
    {
        // 🔹 Lấy ID của sinh viên từ User đang đăng nhập
        var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(studentId))
        {
            return RedirectToAction("Login", "Account");  // 🔹 Nếu chưa đăng nhập, chuyển hướng đến trang Login
        }

        // 🔹 Lọc điểm theo sinh viên hiện tại
        var grades = await _context.Grades
            .Include(g => g.Course)
            .Where(g => g.StudentId == int.Parse(studentId))  // Chỉ lấy điểm của sinh viên đang đăng nhập
            .ToListAsync();

        return View(grades);
    }
}
