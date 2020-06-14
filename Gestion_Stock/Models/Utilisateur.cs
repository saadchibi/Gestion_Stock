using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion_Stock.Models
{
    public class Utilisateur : IdentityUser
    {
        public string nom { get; set; }
        public string address { get; set; }
        public string Ville { get; set; }
        public string codePostal { get; set; }
    }
}
