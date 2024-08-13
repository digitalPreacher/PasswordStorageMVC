using System.ComponentModel.DataAnnotations;

namespace PasswordStorage.Models
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [EmailAddress(ErrorMessage = "Введи почту в правильном формате: user@example.ru")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [Display(Name = "Логин")]
        public string LoginName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [Display(Name = "Секретное слово")]
        public string SecretWord { get; set; } = string.Empty;


        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmedPassword { get; set; } = string.Empty;

    }
}
