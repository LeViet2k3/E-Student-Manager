using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentApp.Models
{
    public class StudyPlan
    {
        [Key]
        public string MaKH { get; set; }
        public string Khoa { get; set; }
        public string Nganh { get; set; }
        public int HocKi { get; set; }

        public string MaHP { get; set; }
        [ForeignKey("MaHP")]
        public Course Courses { get; set; }
    }
}