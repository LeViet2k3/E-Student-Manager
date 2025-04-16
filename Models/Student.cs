using System.ComponentModel.DataAnnotations;
using StudentApp.Models;
namespace StudentApp.Models;
public class Student()
{
    [Key]
    public string MaSV { get; set; }
    public string HoTen { get; set; }
    public DateTime NgaySinh { get; set; }
    public string GioiTinh { get; set; }
    public string Email { get; set; }
    public string SDT { get; set; }
    public string DiaChi { get; set; }
    public string Khoa { get; set; }
    public string Nganh { get; set; }

    public ICollection<CourseRegistration> CourseRegistrations { get; set; }
    public ICollection<Grade> Grades { get; set; }
    public ICollection<Schedule> Schedules { get; set; }
}