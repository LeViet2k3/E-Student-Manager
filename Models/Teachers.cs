using System.ComponentModel.DataAnnotations;

namespace StudentApp.Models;
public class Teacher
{
    [Key]
    public string MaGV { get; set; }
    public string HoTen { get; set; }
    public string Email { get; set; }
    public string SDT { get; set; }
    public string Khoa { get; set; }

    public ICollection<CourseClass> CourseClasses { get; set; }
    public ICollection<TeachingSchedule> TeachingSchedules { get; set; }
}
