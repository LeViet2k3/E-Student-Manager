using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentApp.Models
{
    public interface ICourseService
    {
        List<Course> GetCourses(); 
        List<Course> GetStudentCourses(string maSV);
        bool IsStudentRegistered(string maSV, string maHP);
        bool RegisterStudentToCourse(string maSV, string maHP);
        bool UnregisterStudentFromCourse(string maSV, string maHP);
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

        public List<Course> GetStudentCourses(string maSV)
        {
            return _context.CourseRegistrations
                .Where(cr => cr.MaSV == maSV)
                .Include(cr => cr.CourseClasses)
                    .ThenInclude(cc => cc.Course)
                .Select(cr => cr.CourseClasses.Course)
                .Distinct()
                .ToList();
        }

        public bool IsStudentRegistered(string maSV, string maHP)
        {
            return _context.CourseRegistrations
            .Include(cr => cr.CourseClasses)
            .ThenInclude(cc => cc.Course)
            .Any(cr => cr.MaSV == maSV && cr.CourseClasses.Course.MaHP == maHP);

        }

        public bool RegisterStudentToCourse(string maSV, string maHP)
        {
            if (IsStudentRegistered(maSV, maHP))
                return false;

            var courseClass = _context.CourseClasses.FirstOrDefault(cc => cc.MaHP == maHP);
            if (courseClass == null)
                return false;

            var registration = new CourseRegistration
            {
                MaSV = maSV,
                MaLHP = courseClass.MaLHP,
                NgayDK = DateTime.Now
            };

            _context.CourseRegistrations.Add(registration);
            _context.SaveChanges();
            return true;
        }

        public bool UnregisterStudentFromCourse(string maSV, string maHP)
        {
            var registration = _context.CourseRegistrations
                .Include(cr => cr.CourseClasses)
                .FirstOrDefault(cr => cr.MaSV == maSV && cr.CourseClasses.MaHP == maHP);

            if (registration != null)
            {
                _context.CourseRegistrations.Remove(registration);
                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
