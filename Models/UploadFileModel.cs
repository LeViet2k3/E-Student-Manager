using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace StudentApp.Models
{
    public class UploadFileModel
    {
        [Required(ErrorMessage = "Vui lòng chọn file PDF.")]
        public IFormFile File { get; set; }

        [Required(ErrorMessage = "Mã học phần không được để trống.")]
        public string MaHP { get; set; }
    }
}
