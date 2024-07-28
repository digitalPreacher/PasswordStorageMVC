using System.ComponentModel.DataAnnotations;

namespace PasswordStorage.Models
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string LoginName { get; set; }

        [DataType(DataType.Password, ErrorMessage = "Хуйню ввел")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Данные не совпадают")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string ConfirmedPassword { get; set; }   

    }
}
