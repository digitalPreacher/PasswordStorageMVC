using System.ComponentModel.DataAnnotations;

namespace PasswordStorage.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string Code { get; set; } = string.Empty;
    }
}
