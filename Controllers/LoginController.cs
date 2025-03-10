using Microsoft.AspNetCore.Mvc;
using StudentApp.Models;

namespace StudentApp.Controllers;

public class LoginController : Controller
{
    private readonly DataContext _context;

    public LoginController(DataContext context)
    {
        _context = context;
    }

    // Hiển thị trang đăng nhập
    public IActionResult Index()
    {
        return View();
    }

    // Xử lý đăng nhập
    [HttpPost]
    public IActionResult Login(string Code, string Name)
    {
        var student = _context.Students.FirstOrDefault(s => s.Code == Code && s.Name == Name);

        if (student != null)
        {
            // Nếu tìm thấy sinh viên, chuyển đến trang Student/Index
            return RedirectToAction("Index", "Home");
        }

        // Nếu không tìm thấy, hiển thị lỗi
        ViewBag.Error = "Invalid student code or name.";
        return View("Index");
    }
}
