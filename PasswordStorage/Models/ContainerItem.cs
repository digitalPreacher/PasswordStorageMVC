﻿using System.ComponentModel.DataAnnotations;

namespace PasswordStorage.Models
{
    public class ContainerItem
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Название")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Имя пользователя")]
        public string LoginName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        public void UpdateContainerItem(IFormCollection form)
        {
            Title = form["Title"].ToString();
            LoginName = form["LoginName"].ToString();
            Password = form["Password"].ToString(); 
        }

    }
}
