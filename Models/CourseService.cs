using Microsoft.EntityFrameworkCore;
using StudentApp.Models;
using System.Collections.Generic;

namespace StudentApp.Services
{
    public interface ICourseService
    {
        List<Course> GetCourses(); // Lấy danh sách tất cả khóa học
        List<Course> GetStudentCourses(int studentId); // Lấy khóa học mà sinh viên đã đăng ký
        bool IsStudentRegistered(int studentId, int courseId); // Kiểm tra sinh viên đã đăng ký khóa học chưa
        void RegisterStudentToCourse(int studentId, int courseId); // Đăng ký khóa học cho sinh viên
        void UnregisterStudentFromCourse(int studentId, int courseId); // Hủy đăng ký khóa học
    }

    public class CourseService : ICourseService
    {
        private readonly DataContext _context;

        public CourseService(DataContext context)
        {
            _context = context;
        }

        public List<Course> GetCourses()
        {
            return _context.Courses.ToList();
        }

        public List<Course> GetStudentCourses(int studentId)
        {
            return _context.StudentCourses
                .Where(sc => sc.StudentId == studentId)
                .Include(sc => sc.Course)
                .Select(sc => sc.Course)
                .ToList();
        }

        public bool IsStudentRegistered(int studentId, int courseId)
        {
            return _context.StudentCourses.Any(sc => sc.StudentId == studentId && sc.CourseId == courseId);
        }

        public void RegisterStudentToCourse(int studentId, int courseId)
        {
            if (!IsStudentRegistered(studentId, courseId))
            {
                _context.StudentCourses.Add(new StudentCourse { StudentId = studentId, CourseId = courseId });
                _context.SaveChanges();
            }
        }

        public void UnregisterStudentFromCourse(int studentId, int courseId)
        {
            var studentCourse = _context.StudentCourses.FirstOrDefault(sc => sc.StudentId == studentId && sc.CourseId == courseId);
            if (studentCourse != null)
            {
                _context.StudentCourses.Remove(studentCourse);
                _context.SaveChanges();
            }
        }
    }
}
