using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion_Stock.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Count = 1;
        }


        public int Id { get; set; }

        public string UtilisateurId { get; set; }

        [NotMapped]
        [ForeignKey("UtilisateurId")]
        public virtual Utilisateur Utilisateur { get; set; }

        public int MenuItemId { get; set; }

        [NotMapped]
        [ForeignKey("ProduitId")]
        public virtual Produit Produit { get; set; }



        [Range(1,int.MaxValue, ErrorMessage ="Please enter a value greater than or equal to {1}")]
        public int Count { get; set; }
    }
}
