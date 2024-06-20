using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain.Entity
{
    public class UserTask
    {
		[Key]
		public int UserTaskId { get; set; }

		[ForeignKey("Tasks")]
		public int taskId { get; set; }


		[ForeignKey("Leave")]
        public int leaveId { get; set; }

        [ValidateNever]
		public int userId { get; set; }

        
        [Required]
		public DateTime datetime { get; set; }

		[Required , Range(0 , 99999999999999999 , ErrorMessage = "Hour must greater than 0")]
		public double hours { get; set; } 
        
    }
}
