using System.ComponentModel.DataAnnotations;

namespace PasswordStorage.Models
{
    public class Container
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Поле является обязательным"), MaxLength(31)]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Описание")]
        [MaxLength(31)]
        public string Description { get; set; } = string.Empty;

        public ICollection<ContainerItem> Items { get; set; } = new List<ContainerItem>();  

        [DataType(DataType.Date)]
        public DateTimeOffset CreateAt { get; set; } = DateTimeOffset.Now;

    }
}
