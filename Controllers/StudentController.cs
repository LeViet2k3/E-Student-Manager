using Microsoft.AspNetCore.Mvc;
using StudentApp.Models;

namespace StudentApp.Controllers;

public class StudentController(
    IStudentService studentService,
    IDepartmentService departmentService
    ) : Controller
{
    private readonly IStudentService _studentService = studentService;
    private readonly IDepartmentService _departmentService = departmentService;

    public IActionResult Index()
    {
        // 💡 Kiểm tra nếu Session chưa được cấu hình
        if (HttpContext.Session == null)
        {
            throw new InvalidOperationException("Session is not available. Ensure session middleware is enabled.");
        }

        // Lấy mã số sinh viên từ Session
        string studentCode = HttpContext.Session.GetString("StudentCode");

        if (string.IsNullOrEmpty(studentCode))
        {
            return RedirectToAction("Index", "Login"); // Nếu chưa đăng nhập, chuyển hướng về trang Login
        }

        var student = _studentService.GetStudentByCode(studentCode);

        ViewBag.Departments = _departmentService.GetDepartments();
        return View(student);
    }

    public IActionResult Courses()
    {
        var courses = _studentService.GetCourses(); // Đúng cú pháp
        return View(courses);
    }

    public IActionResult Create()
    {
        var departments = _departmentService.GetDepartments();
        return View(departments);
    }

    public IActionResult Update(int id)
    {
        var student = _studentService.GetStudentById(id);
        ViewBag.Departments = _departmentService.GetDepartments();
        return View(student);
    }

    public IActionResult Delete(int id)
    {
        _studentService.DeleteStudent(id);
        return RedirectToAction("Index");
    }

    public IActionResult Save(Student student)
    {
        if (student.Id == 0)
        {
            _studentService.CreateStudent(student);
        }
        else
        {
            _studentService.UpdateStudent(student);
        }
        return RedirectToAction("Index");
    }
}