using System.ComponentModel.DataAnnotations;

namespace PasswordStorage.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [EmailAddress]
        [Display(Name = "Почта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [Display(Name = "Секретное слово")]
        public string SecretWord { get; set; }
    }
}
