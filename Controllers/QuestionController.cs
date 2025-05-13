using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApp.Controllers
{
    public class QuestionController : Controller
    {
        private readonly DataContext _context;

        public QuestionController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> SelectCourse(string? maLHP)
        {
            var studentCode = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(studentCode))
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy danh sách lớp học phần mà sinh viên đã đăng ký
            var courses = await _context.CourseRegistrations
                .Where(cr => cr.MaSV == studentCode)
                .Join(_context.CourseClasses,
                      cr => cr.MaLHP,
                      cc => cc.MaLHP,
                      (cr, cc) => new { cc.MaLHP, cc.Course.TenHP })
                .Distinct()
                .ToListAsync();

            ViewBag.Courses = courses;
            ViewBag.SelectedMaLHP = maLHP;

            // Nếu chọn lớp học phần thì load câu hỏi
            List<Question> questions = new List<Question>();

            if (!string.IsNullOrEmpty(maLHP))
            {
                questions = await _context.Questions
                    .Where(q => q.MaLHP == maLHP)
                    .ToListAsync();
            }

            return View(questions);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitQuiz(IFormCollection form)
        {
            int correctAnswers = 0;
            int totalQuestions = 0;

            foreach (var key in form.Keys)
            {
                if (key.StartsWith("answers_"))
                {
                    totalQuestions++;
                    var questionIdStr = key.Substring("answers_".Length);
                    if (int.TryParse(questionIdStr, out int questionId))
                    {
                        var selectedAnswer = form[key].ToString(); // lấy chuẩn chuỗi

                        var question = await _context.Questions.FindAsync(questionId);

                        if (question != null && 
                            question.CorrectAnswer.Equals(selectedAnswer, StringComparison.OrdinalIgnoreCase))
                        {
                            correctAnswers++;
                        }
                    }
                }
            }

            ViewBag.CorrectAnswers = correctAnswers;
            ViewBag.TotalQuestions = totalQuestions;

            return View("QuizResult");
        }

    }
}
