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
        [Key]public int taskId { get; set; } 
        public string? name { get; set; }

        [ForeignKey("projectId")]
        public int projectId { get; set; }

        public double? timeTotal { get; set; }
        public double? status {  get; set; }
        public Project? project{ get; set; }


    }
}
