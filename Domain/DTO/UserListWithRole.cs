using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class UserListWithRole
    {
        public int UserId { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? Username { get; set; }


        public string? Email { get; set; }
        
        public string? RoleName { get; set; }

        public string? Password { get; set; }
    }
}
