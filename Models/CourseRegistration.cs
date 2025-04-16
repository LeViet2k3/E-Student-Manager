using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentApp.Models
{
    public class CourseRegistration
    {
        [Key, Column(Order = 0)]
        public string MaSV { get; set; }

        [Key, Column(Order = 1)]
        public string MaLHP { get; set; }

        [ForeignKey("MaSV")]
        public Student Students { get; set; }

        [ForeignKey("MaLHP")]
        public CourseClass CourseClasses { get; set; }

        public DateTime NgayDK { get; set; }
    }
}
