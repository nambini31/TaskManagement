using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class TasksDto
    {
       [Key] public int taskId { get; set; }
        public string? name { get; set; }

        public int projectId { get; set; }
        
        public string? projectName { get; set; }
        public double? timeTotal { get; set; }
        public double? timeElapsed { get; set; }
        public double? status { get; set; }

    }

}
