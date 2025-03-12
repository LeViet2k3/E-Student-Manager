using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentApp.Models
{
    public class Grade
    {
        [Key]
        public int GradeId { get; set; }

        // 🔥 Liên kết với Student
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        // 🔥 Liên kết với Course
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Required]
        public double Score { get; set; }
    }
}
