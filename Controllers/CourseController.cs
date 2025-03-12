using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApp.Models;
using System.Linq;
using System.Threading.Tasks;

[Route("api/courses")]
[ApiController]
public class CourseController : ControllerBase
{
    private readonly DataContext _context;

    public CourseController(DataContext context)
    {
        _context = context;
    }

    // ✅ Sinh viên xem danh sách khóa học
    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var courses = await _context.Courses.Include(c => c.Teacher).ToListAsync();
        return Ok(courses);
    }

    // ✅ Giáo viên thêm khóa học
    [HttpPost]
    public async Task<IActionResult> AddCourse([FromBody] Course course)
    {
        if (course == null) return BadRequest();

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return Ok(course);
    }

    // ✅ Giáo viên chỉnh sửa khóa học
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(int id, [FromBody] Course course)
    {
        var existingCourse = await _context.Courses.FindAsync(id);
        if (existingCourse == null) return NotFound();

        existingCourse.Name = course.Name;
        existingCourse.Description = course.Description;
        existingCourse.TeacherId = course.TeacherId;

        await _context.SaveChangesAsync();
        return Ok(existingCourse);
    }

    // ✅ Giáo viên xóa khóa học
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null) return NotFound();

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
