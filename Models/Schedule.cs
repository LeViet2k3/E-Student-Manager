using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentApp.Models
{
    public class Schedule
    {
        [Key]
        public string MaTKB { get; set; }

        public string MaSV { get; set; }
        [ForeignKey("MaSV")]
        public Student Students { get; set; }

        public string MaLHP { get; set; }
        [ForeignKey("MaLHP")]
        public CourseClass CourseClasses { get; set; }
    }
}