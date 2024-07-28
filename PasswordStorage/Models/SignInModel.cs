using System.ComponentModel.DataAnnotations;

namespace PasswordStorage.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }    
    }
}
