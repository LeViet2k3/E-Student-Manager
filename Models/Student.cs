using System.ComponentModel.DataAnnotations;
using StudentApp.Models;
namespace StudentApp.Models;
public class Student
{
    [Key]
    public string MaSV { get; set; }

    [Required]
    public string HoTen { get; set; }

    [Required]
    public DateTime NgaySinh { get; set; }

    [Required]
    public string GioiTinh { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    [Phone]
    public string SDT { get; set; }

    [Required]
    public string DiaChi { get; set; }

    [Required]
    public string Khoa { get; set; }

    [Required]
    public string Nganh { get; set; }

    public ICollection<CourseRegistration> CourseRegistrations { get; set; }
    public ICollection<Grade> Grades { get; set; }
    public ICollection<Schedule> Schedules { get; set; }
}
