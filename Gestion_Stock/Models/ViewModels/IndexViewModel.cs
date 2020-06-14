using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion_Stock.Models.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Produit> MenuItem { get; set; }
        public IEnumerable<Categorie> Category { get; set; }

    }
}
