using Microsoft.EntityFrameworkCore;

namespace StudentApp.Models;

public interface IStudentService
{
    List<Student> GetStudents(string? keySearch = null, int? departmentId = null);
    Student? GetStudentById(int id);
    void CreateStudent(Student student);
    void UpdateStudent(Student student);
    void DeleteStudent(int id);
}

public class StudentService(DataContext dataContext) : IStudentService
{
    private readonly DataContext _dataContext = dataContext;

    public void CreateStudent(Student student)
    {
        _dataContext.Students.Add(student);
        _dataContext.SaveChanges();
    }

    public void DeleteStudent(int id)
    {
        var student = _dataContext.Students.FirstOrDefault(x => x.Id == id);
        if (student is null) return;
        _dataContext.Students.Remove(student);
        _dataContext.SaveChanges();
    }

    public Student? GetStudentById(int id)
    {
        return _dataContext.Students.FirstOrDefault(x => x.Id == id);
    }

    public List<Student> GetStudents(string? keySearch = null, int? departmentId = null)
    {
        return _dataContext.Students.Include(s => s.Department)
            .Where(s => string.IsNullOrEmpty(keySearch) ||
                (s.Code.Contains(keySearch) || s.Name.Contains(keySearch) || s.Address.Contains(keySearch)))
            .Where(s => departmentId == null || s.DepartmentId == departmentId)
            .ToList();
    }

    public void UpdateStudent(Student student)
    {
        var updateStudent = _dataContext.Students.FirstOrDefault(x => x.Id == student.Id);
        if (updateStudent is null) return;
        updateStudent.Code = student.Code;
        updateStudent.Name = student.Name;
        updateStudent.Birthday = student.Birthday;
        updateStudent.Address = student.Address;
        updateStudent.DepartmentId = student.DepartmentId;
        _dataContext.SaveChanges();
    }
}