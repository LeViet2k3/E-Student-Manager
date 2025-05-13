using System;
using System.ComponentModel.DataAnnotations;

namespace StudentApp.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MaLHP { get; set; }

        [Required]
        public string Content { get; set; }
        [Required]
        public string OptionA { get; set; }

        [Required]
        public string OptionB { get; set; }

        [Required]
        public string OptionC { get; set; }

        [Required]
        public string OptionD { get; set; }

        [Required]
        public string CorrectAnswer { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
