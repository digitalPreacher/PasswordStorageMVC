using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace PasswordStorage.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
