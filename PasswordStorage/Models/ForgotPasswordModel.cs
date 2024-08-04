using System.ComponentModel.DataAnnotations;

namespace PasswordStorage.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [EmailAddress(ErrorMessage = "Введи почту в правильном формате: user@example.ru")]
        [Display(Name = "Почта")]
        public string Email { get; set; }
    }
}
