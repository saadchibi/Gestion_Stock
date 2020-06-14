using System;
using System.Collections.Generic;
using System.Text;
using Gestion_Stock.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gestion_Stock.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Categorie> Categorie { get; set; }
        public DbSet<Produit> Produit { get; set; }
        public DbSet<Utilisateur> Utilisateur { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

    }
}
