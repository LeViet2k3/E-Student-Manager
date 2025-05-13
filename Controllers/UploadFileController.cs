using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentApp.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System;

namespace StudentApp.Controllers
{
    public class UploadFileController : Controller
    {
        private readonly DataContext _context;
        private readonly string _togetherApiKey = "10ebc009df7ab4bfd77baa1708946c7eacd6a59b48953092ced8c8bcb716ef85";
        private readonly string _togetherApiUrl = "https://api.together.xyz/v1/chat/completions";

        public UploadFileController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Upload(string maLHP)
        {
            ViewBag.MaLHP = maLHP;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(UploadFileModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadFolder))
                        Directory.CreateDirectory(uploadFolder);

                    var filePath = Path.Combine(uploadFolder, model.File.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.File.CopyToAsync(stream);
                    }

                    StringBuilder text = new StringBuilder();
                    using (var document = UglyToad.PdfPig.PdfDocument.Open(filePath))
                    {
                        foreach (var page in document.GetPages())
                        {
                            text.AppendLine(page.Text);
                        }
                    }

                    string fullText = text.ToString();
                    if (string.IsNullOrWhiteSpace(fullText))
                        throw new Exception("File tải lên không có nội dung.");

                    var questions = await GenerateQuestionsFromText(fullText, model.MaHP);

                    if (questions == null || questions.Count == 0)
                        throw new Exception("Không tạo được câu hỏi từ tài liệu.");

                    _context.Questions.AddRange(questions);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Tải lên và tạo câu hỏi thành công!";
                    TempData["ShowSuccess"] = true;
                    return RedirectToAction("SelectCourseClass", "Teacher");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                    return View(model);
                }
            }

            return View(model);
        }

        private async Task<List<Question>> GenerateQuestionsFromText(string text, string maHP)
        {
            var prompt = @$"Dựa trên nội dung tài liệu dưới đây, hãy tạo 5 câu hỏi trắc nghiệm. 
                            Mỗi câu có đúng 4 lựa chọn (A, B, C, D). 
                            Định dạng mỗi câu như sau:
                            Câu hỏi: ...
                            A. ...
                            B. ...
                            C. ...
                            D. ...
                            Đáp án: A/B/C/D

                            Tài liệu:
                            {text}";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _togetherApiKey);

                var requestBody = new
                {
                    model = "NousResearch/Nous-Hermes-2-Mixtral-8x7B-DPO",
                    messages = new object[]
                    {
                        new { role = "system", content = "Bạn là trợ lý chuyên tạo câu hỏi trắc nghiệm từ tài liệu." },
                        new { role = "user", content = prompt }
                    },
                    temperature = 0.3,
                    max_tokens = 2048
                };

                var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(_togetherApiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Together API lỗi: {response.StatusCode} - {errorContent}");
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var responseJson = JObject.Parse(responseString);
                var reply = responseJson["choices"]?[0]?["message"]?["content"]?.ToString();

                if (string.IsNullOrWhiteSpace(reply))
                    throw new Exception("Không nhận được phản hồi hợp lệ từ Together AI.");

                var questions = ParseQuestionsFromResponse(reply, maHP);

                return questions;
            }
        }

        private List<Question> ParseQuestionsFromResponse(string response, string maHP)
        {
            var questions = new List<Question>();
            var lines = response.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
            Question current = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("Câu hỏi"))
                {
                    if (current != null && IsQuestionValid(current))
                        questions.Add(current);

                    current = new Question { MaLHP = maHP, Content = line[(line.IndexOf(":") + 1)..].Trim() };
                }
                else if (line.StartsWith("A.") && current != null)
                    current.OptionA = line.Substring(2).Trim();
                else if (line.StartsWith("B.") && current != null)
                    current.OptionB = line.Substring(2).Trim();
                else if (line.StartsWith("C.") && current != null)
                    current.OptionC = line.Substring(2).Trim();
                else if (line.StartsWith("D.") && current != null)
                    current.OptionD = line.Substring(2).Trim();
                else if (line.StartsWith("Đáp án:") && current != null)
                    current.CorrectAnswer = line.Substring(6).Trim().ToUpper();
            }

            if (current != null && IsQuestionValid(current))
                questions.Add(current);

            return questions;
        }

        private bool IsQuestionValid(Question q)
        {
            return !string.IsNullOrEmpty(q.Content) &&
                   !string.IsNullOrEmpty(q.OptionA) &&
                   !string.IsNullOrEmpty(q.OptionB) &&
                   !string.IsNullOrEmpty(q.OptionC) &&
                   !string.IsNullOrEmpty(q.OptionD) &&
                   !string.IsNullOrEmpty(q.CorrectAnswer);
        }
    }
}
