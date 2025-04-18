using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentApp.Models
{
    public class Grade
    {
        [Key]
        public string MaDiem { get; set; }

        public string MaSV { get; set; }
        [ForeignKey("MaSV")]
        public Student Students { get; set; }

        public string MaLHP { get; set; }
        [ForeignKey("MaLHP")]
        public CourseClass CourseClasses { get; set; }

        public double DiemQT { get; set; }
        public double DiemThi { get; set; }
        public double DiemTong { get; set; }
    }
}
