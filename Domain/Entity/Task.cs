using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Task
    {
        [Key]public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string name { get; set; }
        
    }
}
