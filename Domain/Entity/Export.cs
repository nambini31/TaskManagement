using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Export
    {
        public int id { get; set; }
        public DateOnly dateFrom { get; set; }
        public DateOnly dateTo { get; set; }
        
        public int? user_maj { get; set; }
    }
}
