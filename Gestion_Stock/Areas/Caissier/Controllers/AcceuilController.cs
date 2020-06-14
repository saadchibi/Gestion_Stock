using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gestion_Stock.Data;
using Gestion_Stock.Models;
using Gestion_Stock.Models.ViewModels;
using Gestion_Stock.Utility;

namespace Gestion_Stock.Controllers
{
    [Area("Caissier")]
    public class AcceuilController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AcceuilController(ApplicationDbContext db)
        {
            _db = db;
        }


        public async Task<IActionResult> Index()
        {
            IndexViewModel IndexVM = new IndexViewModel()
            {
                MenuItem = await _db.Produit.Include(m => m.Categorie).Where(m=>m.qte>0).ToListAsync(),
                Category = await _db.Categorie.ToListAsync(),

            };

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if(claim!=null)
            {
                var cnt = _db.ShoppingCart.Where(u => u.UtilisateurId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.ssShoppingCartCount, cnt);
            }


            return View(IndexVM);
        }
        public async Task<IActionResult> ReptureStock()
        {
            IndexViewModel IndexVM = new IndexViewModel()
            {
                MenuItem = await _db.Produit.Include(m => m.Categorie).Where(m => m.qte < 2).ToListAsync(),
                Category = await _db.Categorie.ToListAsync(),

            };

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                var cnt = _db.ShoppingCart.Where(u => u.UtilisateurId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.ssShoppingCartCount, cnt);
            }
            return View(IndexVM);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var menuItemFromDb = await _db.Produit.Include(m => m.Categorie).Where(m => m.id == id).FirstOrDefaultAsync();

            ShoppingCart cartObj = new ShoppingCart()
            {
                Produit = menuItemFromDb,
                MenuItemId = menuItemFromDb.id
            };

            return View(cartObj);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ShoppingCart CartObject)
        {
            CartObject.Id = 0;
            if(ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                CartObject.UtilisateurId = claim.Value;
                //CartObject.Utilisateur = await _db.Utilisateur.Where(c => c.Id == CartObject.UtilisateurId).FirstOrDefaultAsync();

                ShoppingCart cartFromDb = await _db.ShoppingCart.Where(c => c.UtilisateurId == CartObject.UtilisateurId
                                                && c.MenuItemId == CartObject.MenuItemId).FirstOrDefaultAsync();

                if(cartFromDb==null)
                {
                     _db.ShoppingCart.Add(CartObject);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    cartFromDb.Count = cartFromDb.Count + CartObject.Count;
                }
                await _db.SaveChangesAsync();

                var count = _db.ShoppingCart.Where(c => c.UtilisateurId == CartObject.UtilisateurId).ToList().Count();
                HttpContext.Session.SetInt32(SD.ssShoppingCartCount, count);

                return RedirectToAction("Index");
            }
            else
            {
                var menuItemFromDb = await _db.Produit.Include(m => m.Categorie).Where(m => m.id == CartObject.MenuItemId).FirstOrDefaultAsync();

                ShoppingCart cartObj = new ShoppingCart()
                {
                    Produit = menuItemFromDb,
                    MenuItemId = menuItemFromDb.id
                };

                return View(cartObj);
            }
        }





        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
