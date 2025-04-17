using Microsoft.EntityFrameworkCore;
using StudentApp.Models; 

namespace StudentApp.Models;

public interface IStudentService
{
    Student GetStudentByCode(string maSV);
    List<Student> GetStudents(string? keySearch = null);
    Student? GetStudentByCodeOrNull(string maSV);
    void CreateStudent(Student student);
    void UpdateStudent(Student student);
    void DeleteStudent(string maSV);
    List<Course> GetCourses();
    List<CourseRegistration> GetThoiKhoaBieuBySemester(string maSV, int? kiHoc = null, string? namHoc = null);

}

public class StudentService(DataContext dataContext) : IStudentService
{
    private readonly DataContext _dataContext = dataContext;

        public void SaveChanges()
    {
        _dataContext.SaveChanges(); 
    }

    public Student GetStudentByCode(string maSV)
    {
        return _dataContext.Students.FirstOrDefault(s => s.MaSV == maSV);
    }

    public Student? GetStudentByCodeOrNull(string maSV)
    {
        return _dataContext.Students.FirstOrDefault(s => s.MaSV == maSV);
    }

    public List<Student> GetStudents(string? keySearch = null)
    {
        return _dataContext.Students
            .Where(s => string.IsNullOrEmpty(keySearch) ||
                        s.MaSV.Contains(keySearch) ||
                        s.HoTen.Contains(keySearch) ||
                        s.DiaChi.Contains(keySearch) ||
                        s.Email.Contains(keySearch) ||
                        s.SDT.Contains(keySearch) ||
                        s.Khoa.Contains(keySearch) ||
                        s.Nganh.Contains(keySearch))
            .ToList();
    }

    public void CreateStudent(Student student)
    {
        _dataContext.Students.Add(student);
        _dataContext.SaveChanges();
    }

   public void UpdateStudent(Student student)
        {
            var existingStudent = _dataContext.Students.FirstOrDefault(s => s.MaSV == student.MaSV);
            if (existingStudent != null)
            {
                // Cập nhật thông tin sinh viên
                existingStudent.HoTen = student.HoTen;
                existingStudent.NgaySinh = student.NgaySinh;
                existingStudent.GioiTinh = student.GioiTinh;
                existingStudent.Email = student.Email;
                existingStudent.SDT = student.SDT;
                existingStudent.DiaChi = student.DiaChi;
                existingStudent.Khoa = student.Khoa;
                existingStudent.Nganh = student.Nganh;

                // Lưu lại thay đổi vào cơ sở dữ liệu
                _dataContext.SaveChanges();
            }
        }


    public void DeleteStudent(string maSV)
    {
        var student = _dataContext.Students.FirstOrDefault(s => s.MaSV == maSV);
        if (student == null) return;

        _dataContext.Students.Remove(student);
        _dataContext.SaveChanges();
    }

    public List<Course> GetCourses()
    {
        return _dataContext.Courses.ToList();
    }

    public List<CourseRegistration> GetThoiKhoaBieuBySemester(string studentCode, int? kiHoc = null, string? namHoc = null)
    {
        var query = _dataContext.CourseRegistrations
            .Include(cr => cr.CourseClasses)
                .ThenInclude(cc => cc.Course)
            .Where(cr => cr.MaSV == studentCode);

        if (kiHoc.HasValue)
            query = query.Where(cr => cr.CourseClasses.Course.KiHoc == kiHoc.Value);

        if (!string.IsNullOrEmpty(namHoc))
            query = query.Where(cr => cr.CourseClasses.Course.NamHoc == namHoc);

        return query.ToList();
    }


}
