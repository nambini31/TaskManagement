using System.ComponentModel.DataAnnotations;

namespace Domain.DTO.ViewModels.UserVM
{
    public class RegisterViewModel
    {
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [StringLength(255)]
        public string? Surname { get; set; }

        [StringLength(255)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[^\w]).{8,}$")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [StringLength(50)]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide.")]
        public string? Email { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
