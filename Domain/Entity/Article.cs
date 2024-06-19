using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Article
    {
        [Key , DatabaseGenerated(databaseGeneratedOption:DatabaseGeneratedOption.Identity)]
        public int articleId { get; set; }
        public required string design { get; set; }

        [Range(0,99999999999999)]
        public double pu { get; set; }

        [ForeignKey("Category")]
        public required int categoryId { get; set; }

        [Range(0, 99999999999999)]
        public int quantite{ get; set; } 

        //[ValidateNever]
        //public Category? category { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }

    }
}
