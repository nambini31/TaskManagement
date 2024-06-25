using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.ViewModels
{
    public class FiltreUserTask
    {
        public DateOnly startDate { get; set; }
        public DateOnly endDate { get; set; }
        public List<int>? userId { get; set; }
    }
}
