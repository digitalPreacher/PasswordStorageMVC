using System.ComponentModel.DataAnnotations;

namespace PasswordStorage.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Почта")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Секретное слово")]
        public string SecretWord { get; set; }
    }
}
