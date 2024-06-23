using Domain.Entity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Models.ViewModel
{
    public class UserTaskViewModel
    {
        [Key]
        public int UserTaskId { get; set; }

        [ForeignKey("Tasks")]
        public int taskId { get; set; }

        [ValidateNever]
        public Tasks? Tasks { get; set; }


        [ForeignKey("Leaves")]
        public int leaveId { get; set; }

        [ValidateNever]
        public Leave? Leaves { get; set; }

        [ValidateNever]
        [ForeignKey("User")]
        public int userId { get; set; }


        [ValidateNever]
        public User? User { get; set; }


        [Required]
        public DateTime date { get; set; }

        [Required, Range(0, 99999999999999999, ErrorMessage = "Hour must greater than 0")]
        public double hours { get; set; }
    }
}
