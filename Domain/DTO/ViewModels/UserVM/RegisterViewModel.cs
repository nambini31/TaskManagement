using System.ComponentModel.DataAnnotations;

namespace Domain.DTO.ViewModels.UserVM
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string? Surname { get; set; }

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
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
