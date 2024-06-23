using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class LeaveDto
    {
        [Key]public int leaveId {  get; set; }

        public string? reason { get; set; }
    }
}
