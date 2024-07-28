using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PasswordStorage.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Key]
        public override int Id { get; set; }
        public string LoginName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string SecretWord {  get; set; } = string.Empty;

        public DateTime CreateAt { get; set; } 

    }
}
