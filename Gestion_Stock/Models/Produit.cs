using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion_Stock.Models
{
    public class Produit
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Nom du Produit")]
        [Required]
        public string nom { get; set; }
        [Required]
        [Display(Name = "Categorie")]
        public int CategorieId { get; set; }
        [ForeignKey("CategorieId")]
        public virtual Categorie Categorie { get; set; }
        [Required]
        public string marque { get; set; }
        [Required]
        [Display(Name = "Quantité de stock")]
        public int qte { get; set; }
        [Required]
        [Display(Name = "Date d’ajout")]
        public DateTime dateStock { get; set; }
        [Required]
        [Display(Name = "Etat de livraison")]
        public bool livre { get; set; }
        public string description { get; set; }
    }
}
