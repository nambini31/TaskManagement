using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Domain.Entity
{
    public class Role 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //spécifie que la valeur sera générée automatique dans la DB
        public int RoleId { get; set; }

        [Required] //specifié que le champs est obligatoire
        [StringLength(5)] // specifie la longue du caractrère
        public string? Name { get; set; }
    }
}
