﻿using System.ComponentModel.DataAnnotations;

namespace PasswordStorage.Models
{
    public class Container
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        public ICollection<ContainerItem> Items { get; set; } = new List<ContainerItem>();  

        [DataType(DataType.Date)]
        public DateTime CreateAt { get; set; }

        [DataType(DataType.Date)]
        public DateTime UpdateAt { get; set; }

        public virtual ApplicationUser User { get; set; }   

        public Container(IFormCollection form, ApplicationUser user)
        {
            User = user;
            Title = form["Title"].ToString();  
            Description = form["Description"].ToString();
            CreateAt = DateTime.Now.ToUniversalTime();
            UpdateAt = DateTime.Now.ToUniversalTime();
        }

        public void UpdateContainer(IFormCollection form, ApplicationUser user)
        {
            Title = form["Title"].ToString();
            Description = form["Description"].ToString();
            UpdateAt = DateTime.Now.ToUniversalTime();
        }

        public void AddContainerItem(ContainerItem item)
        {
            Items.Add(item);
        }

        public Container() { }  
    }
}
