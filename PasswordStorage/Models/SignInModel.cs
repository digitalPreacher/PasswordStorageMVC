using System.ComponentModel.DataAnnotations;

namespace PasswordStorage.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [Display(Name = "Логин")]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }    
    }
}
