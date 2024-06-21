using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models.ViewModel
{
    public class LeaveViewModel
    {
        [Key]
        public int leaveId { get; set; }
        public string? reason { get; set; }
    }
}
