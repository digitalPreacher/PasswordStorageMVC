using System.ComponentModel.DataAnnotations;

namespace PasswordStorage.Models
{
    public class ContainerItem
    {

        [Key]
        public int Id { get; set; }

        [Display(Name = "Название")]
        [Required]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Имя пользователя")]
        [Required]
        public string LoginName { get; set; } = string.Empty;

        [Display(Name = "Пароль")]
        [Required]
        public string Password { get; set; } = string.Empty;

        public void UpdateContainerItem(IFormCollection form)
        {
            Title = form["Title"].ToString();
            LoginName = form["LoginName"].ToString();
            Password = form["Password"].ToString(); 
        }

    }
}
