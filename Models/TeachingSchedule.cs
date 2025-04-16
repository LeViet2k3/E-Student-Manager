using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentApp.Models
{
    public class TeachingSchedule
    {
        [Key]
        public string MaLD { get; set; }

        public string MaGV { get; set; }
        [ForeignKey("MaGV")]
        public Teacher Teachers { get; set; }

        public string MaLHP { get; set; }
        [ForeignKey("MaLHP")]
        public CourseClass CourseClasses { get; set; }

        public string Phong { get; set; }
        public DateTime NgayDay { get; set; }
    }
}