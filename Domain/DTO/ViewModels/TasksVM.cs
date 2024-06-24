using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.ViewModels
{
    public class TasksVM
    {
        public IEnumerable<ProjectDto> ProjectList { get; set; }
        public IEnumerable<TasksDto> TasksList { get; set; }

    }
}
