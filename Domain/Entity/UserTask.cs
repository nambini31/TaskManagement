using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class UserTask
    {
        [Key]public int UserTaskId { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public int leaveId { get; set; }
        public double Hours { get; set; }
        public DateTime Date { get; set; }
        
    }
}
