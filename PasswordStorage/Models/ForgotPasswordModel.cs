using System.ComponentModel.DataAnnotations;

namespace PasswordStorage.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [EmailAddress]
        [Display(Name = "Почта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Секретное слово")]
        public string SecretWord { get; set; }
    }
}
