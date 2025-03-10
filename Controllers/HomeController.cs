using Microsoft.AspNetCore.Mvc;

namespace StudentApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult StudentInfo()
    {
        return View();
    }

    public IActionResult StudyPlan()
    {
        return View();
    }

    public IActionResult RegisterSubject()
    {
        return View();
    }

    public IActionResult ExamSchedule()
    {
        return View();
    }

    public IActionResult StudyResult()
    {
        return View();
    }

    public IActionResult EvaluateSubject()
    {
        return View();
    }

    public IActionResult Tuition()
    {
        return View();
    }

    public IActionResult Timetable()
    {
        return View();
    }
}
