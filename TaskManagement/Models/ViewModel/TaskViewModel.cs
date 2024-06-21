using Domain.Entity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models.ViewModel
{
    public class TaskViewModel
    {
        [Key]
        public int taskId { get; set; }

        [ForeignKey("Project")]
        public int projectId { get; set; }

        [ValidateNever]
        public Project? Project { get; set; }
        public int userId { get; set; }
        public string? name { get; set; }
    }
}
