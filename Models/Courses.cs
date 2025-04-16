using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentApp.Models
{
    public class Course
    {
        [Key]
        public string MaHP { get; set; }
        public string TenHP { get; set; }
        public int SoTinChi { get; set; }
        public int KiHoc { get; set; }
        public string NamHoc { get; set; }
        public string Khoa { get; set; }
        public string Nganh { get; set; }

        public ICollection<CourseClass> CourseClasses { get; set; }
        public ICollection<StudyPlan> StudyPlans { get; set; }
        public ICollection<TrainingProgram> TrainingPrograms { get; set; }
    }
}
