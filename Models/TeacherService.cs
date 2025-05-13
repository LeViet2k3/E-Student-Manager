using StudentApp.Models;
using System.Linq;


namespace StudentApp.Services
{
    public interface ITeacherService
    {
        Teacher GetTeacherByCode(string maGV);
        void UpdateTeacher(Teacher teacher);
    }

    public class TeacherService : ITeacherService
    {
        private readonly DataContext _dataContext;

        public TeacherService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Teacher GetTeacherByCode(string maGV)
        {
            return _dataContext.Teachers.FirstOrDefault(t => t.MaGV == maGV);
        }

        public void UpdateTeacher(Teacher teacher)
        {
            var existingTeacher = _dataContext.Teachers.FirstOrDefault(t => t.MaGV == teacher.MaGV);
            if (existingTeacher != null)
            {
                existingTeacher.HoTen = teacher.HoTen;
                existingTeacher.Email = teacher.Email;
                existingTeacher.SDT = teacher.SDT;
                existingTeacher.Khoa = teacher.Khoa;

                _dataContext.SaveChanges();
            }
        }
    }
}
