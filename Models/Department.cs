using System.ComponentModel.DataAnnotations;

namespace StudentApp.Models;

public class Department
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public virtual List<Student> Students { get; set; } = [];
}