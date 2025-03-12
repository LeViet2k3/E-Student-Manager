using System.ComponentModel.DataAnnotations;

namespace StudentApp.Models;
public class Teacher
{
    [Key]
    public int TeacherId { get; set; }

    [MaxLength(10)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public DateOnly Birthday { get; set; } = DateOnly.MinValue;

    [MaxLength(255)]
    public string Address { get; set; } = string.Empty;

    // Một giáo viên có thể dạy nhiều khóa học
    public ICollection<Course> Courses { get; set; }
}
