using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion_Stock.Models
{
    public class Categorie
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string nom { get; set; }
    }
}
