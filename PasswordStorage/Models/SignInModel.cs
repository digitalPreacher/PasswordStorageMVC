using System.ComponentModel.DataAnnotations;

namespace PasswordStorage.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [Display(Name = "Логин")]
        public string LoginName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; } = false;   
    }
}
