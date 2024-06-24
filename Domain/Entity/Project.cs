using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Project
    {
        [Key]public int projectId {  get; set; }

        public string? name { get; set; }

        public string? description { get; set; }

        public ICollection<Tasks> Tasks { get; set; }
    }
}
