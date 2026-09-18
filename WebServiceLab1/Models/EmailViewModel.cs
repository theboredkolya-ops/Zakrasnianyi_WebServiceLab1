using System.ComponentModel.DataAnnotations;

namespace WebServiceLab1.Models
{
    public class EmailViewModel
    {
        [Required(ErrorMessage = "Введіть email")]
        [EmailAddress(ErrorMessage = "Некоректний email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть повідомлення")]
        public string Message { get; set; } = string.Empty;
    }
}