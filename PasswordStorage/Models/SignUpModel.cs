using System.ComponentModel.DataAnnotations;

namespace PasswordStorage.Models
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [EmailAddress(ErrorMessage = "ВВедите корректный почтовый адрес")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [Display(Name = "Логин")]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [Display(Name = "Секретное слово")]
        public string SecretWord { get; set; }


        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(100)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmedPassword { get; set; }   

    }
}
