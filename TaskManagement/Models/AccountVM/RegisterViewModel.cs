using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models.AccountVM
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        public string? Surname { get; set; }

        [Required]
        [StringLength(255)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [StringLength(50)]
        public string? Email { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
