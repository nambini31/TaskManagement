using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Tasks
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
