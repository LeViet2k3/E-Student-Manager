using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentApp.Models
{
    public class CourseClass
    {
        [Key]
        public string MaLHP { get; set; }

        public string MaHP { get; set; }
        [ForeignKey("MaHP")]
        public Course Course { get; set; }

        public string MaGV { get; set; }
        [ForeignKey("MaGV")]
        public Teacher Teachers { get; set; }

        public string PhongHoc { get; set; }
        public string Thu { get; set; }
        public string Tiet { get; set; }
        public int SiSoToiDa { get; set; }

        public ICollection<CourseRegistration> CourseRegistrations { get; set; }
        public ICollection<Grade> Grades { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<TeachingSchedule> TeachingSchedules { get; set; }
    }
}