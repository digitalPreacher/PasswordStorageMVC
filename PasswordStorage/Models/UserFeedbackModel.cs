using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace PasswordStorage.Models
{
    public class UserFeedbackModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [EmailAddress(ErrorMessage = "Введите почту в формате info@example.ru")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле обязательно для заполнения"), MaxLength(1000)]
        public string Description {  get; set; } = string.Empty;

    }
}
