using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models.ViewModel
{
    public class ProjectViewModel
    {

        [Key]
        public int projectId { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
    }
}
