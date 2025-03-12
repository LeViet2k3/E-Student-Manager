using Microsoft.AspNetCore.Mvc;
using StudentApp.Models;
using StudentApp.Services;
using System.Threading.Tasks;

namespace StudentApp.Controllers;

public class StudentController(
    IStudentService studentService,
    IDepartmentService departmentService,
    ICourseService courseService
    ) : Controller
{
    private readonly IStudentService _studentService = studentService;
    private readonly IDepartmentService _departmentService = departmentService;
    private readonly ICourseService _courseService = courseService;

    public IActionResult Index()
    {
        if (HttpContext.Session == null)
        {
            throw new InvalidOperationException("Session is not available. Ensure session middleware is enabled.");
        }

        string studentCode = HttpContext.Session.GetString("StudentCode");

        if (string.IsNullOrEmpty(studentCode))
        {
            return RedirectToAction("Index", "Login");
        }

        var student = _studentService.GetStudentByCode(studentCode);

        ViewBag.Departments = _departmentService.GetDepartments();
        return View(student);
    }

    public IActionResult Courses()
    {
        var courses = _courseService.GetCourses();
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

    [HttpPost]
    public async Task<IActionResult> RegisterCourse(int courseId)
    {
        string studentCode = HttpContext.Session.GetString("StudentCode");

        if (string.IsNullOrEmpty(studentCode))
        {
            return RedirectToAction("Index", "Login");
        }

        var student = _studentService.GetStudentByCode(studentCode);
        if (student == null)
        {
            return NotFound();
        }

        bool isRegistered = _courseService.IsStudentRegistered(student.Id, courseId);
        if (!isRegistered)
        {
            _courseService.RegisterStudentToCourse(student.Id, courseId);
        }

        return RedirectToAction("Timetable");
    }

    public async Task<IActionResult> Timetable()
    {
        string studentCode = HttpContext.Session.GetString("StudentCode");

        if (string.IsNullOrEmpty(studentCode))
        {
            return RedirectToAction("Index", "Login");
        }

        var student = _studentService.GetStudentByCode(studentCode);
        if (student == null)
        {
            return NotFound();
        }

        var registeredCourses = _courseService.GetStudentCourses(student.Id);
        return View(registeredCourses);
    }

    [HttpPost]
    public async Task<IActionResult> UnregisterCourse(int courseId)
    {
        string studentCode = HttpContext.Session.GetString("StudentCode");

        if (string.IsNullOrEmpty(studentCode))
        {
            return RedirectToAction("Index", "Login");
        }

        var student = _studentService.GetStudentByCode(studentCode);
        if (student == null)
        {
            return NotFound();
        }

        _courseService.UnregisterStudentFromCourse(student.Id, courseId);
        return RedirectToAction("Timetable");
    }
}
