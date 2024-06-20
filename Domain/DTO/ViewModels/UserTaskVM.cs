using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace Domain.DTO.ViewModels
{

        public class UserTaskVM
        {
             public int UserTaskId { get; set; }

            public int taskId { get; set; }

            public string? taskName { get; set; }

            public int leaveId { get; set; }

            string? leaveName { get; set; }

            public int projectId { get; set; }

            public string? projectName { get; set; }

            public int userId { get; set; }

            public int userName { get; set; }

            public DateTime datetime { get; set; }

            public double hours { get; set; }

        }
    }
