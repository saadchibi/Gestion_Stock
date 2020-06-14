using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gestion_Stock.Data;
using Gestion_Stock.Models;
using Gestion_Stock.Models.ViewModels;
using Gestion_Stock.Utility;

namespace Spice.Areas.Caissier.Controllers
{
    [Area("Caissier")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public OrderDetailsCart detailCart { get; set; }

        public CartController(ApplicationDbContext db,IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {

            detailCart = new OrderDetailsCart()
            {
                OrderHeader = new Gestion_Stock.Models.OrderHeader()
            };

            detailCart.OrderHeader.OrderTotal = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var cart = _db.ShoppingCart.Where(c => c.UtilisateurId == claim.Value);
            if(cart !=null)
            {
                detailCart.listCart = cart.ToList();
            }

            foreach(var list in detailCart.listCart)
            {
                list.Produit = await _db.Produit.FirstOrDefaultAsync(m => m.id == list.MenuItemId);
                detailCart.OrderHeader.OrderTotal = detailCart.OrderHeader.OrderTotal + (10 * list.Count);
                list.Produit.description = SD.ConvertToRawHtml(list.Produit.description);
                if(list.Produit.description.Length>100)
                {
                    list.Produit.description = list.Produit.description.Substring(0, 99) + "...";
                }
            }
            detailCart.OrderHeader.OrderTotalOriginal = detailCart.OrderHeader.OrderTotal;
            
         
            return View(detailCart);

        }


        public async Task<IActionResult> Summary()
        {

            detailCart = new OrderDetailsCart()
            {
                OrderHeader = new Gestion_Stock.Models.OrderHeader()
            };

            detailCart.OrderHeader.OrderTotal = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            Utilisateur applicationUser = await _db.Utilisateur.Where(c => c.Id == claim.Value).FirstOrDefaultAsync();
            var cart = _db.ShoppingCart.Where(c => c.UtilisateurId == claim.Value);
            if (cart != null)
            {
                detailCart.listCart = cart.ToList();
            }

            foreach (var list in detailCart.listCart)
            {
                list.Produit = await _db.Produit.FirstOrDefaultAsync(m => m.id == list.MenuItemId);
                detailCart.OrderHeader.OrderTotal = detailCart.OrderHeader.OrderTotal + (10 * list.Count);
               
            }
            detailCart.OrderHeader.OrderTotalOriginal = detailCart.OrderHeader.OrderTotal;
            detailCart.OrderHeader.PickupName = applicationUser.nom;
            detailCart.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
            detailCart.OrderHeader.PickUpTime = DateTime.Now;
        


            return View(detailCart);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            detailCart.listCart = await _db.ShoppingCart.Where(c => c.UtilisateurId == claim.Value).ToListAsync();

            detailCart.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            detailCart.OrderHeader.OrderDate = DateTime.Now;
            detailCart.OrderHeader.UserId = claim.Value;
            detailCart.OrderHeader.Status = SD.StatusSubmitted;
            detailCart.OrderHeader.PickUpTime = Convert.ToDateTime(detailCart.OrderHeader.PickUpDate.ToShortDateString() + " " + detailCart.OrderHeader.PickUpTime.ToShortTimeString());

            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            _db.OrderHeader.Add(detailCart.OrderHeader);
            await _db.SaveChangesAsync();

            detailCart.OrderHeader.OrderTotalOriginal = 0;


            foreach (var item in detailCart.listCart)
            {
                item.Produit = await _db.Produit.FirstOrDefaultAsync(m => m.id == item.MenuItemId);
                OrderDetails orderDetails = new OrderDetails
                {
                    ProduitId = item.MenuItemId,
                    OrderId = detailCart.OrderHeader.Id,
                    Description = item.Produit.description,
                    Name = item.Produit.nom,
                    Price = 10,
                    Count = item.Count
                };
                detailCart.OrderHeader.OrderTotalOriginal += orderDetails.Count * orderDetails.Price;
                _db.OrderDetails.Add(orderDetails);

            }
          
                detailCart.OrderHeader.OrderTotal = detailCart.OrderHeader.OrderTotalOriginal;


            _db.ShoppingCart.RemoveRange(detailCart.listCart);
            HttpContext.Session.SetInt32(SD.ssShoppingCartCount, 0);

            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Acceuil");

        }


        public async Task<IActionResult> Plus(int cartId)
        {
            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId);
            Produit produit = await _db.Produit.FindAsync(cart.MenuItemId);
            if (produit.qte > cart.Count)
            {
                cart.Count += 1;
            }
            else
            {
                cart.Count += 0;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Minus(int cartId)
        {
            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId);
            if(cart.Count==1)
            {
                _db.ShoppingCart.Remove(cart);
                await _db.SaveChangesAsync();

                var cnt = _db.ShoppingCart.Where(u => u.UtilisateurId == cart.UtilisateurId).ToList().Count;
                HttpContext.Session.SetInt32(SD.ssShoppingCartCount, cnt);
            }
            else
            {
                cart.Count -= 1;
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int cartId)
        {
            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId);

            _db.ShoppingCart.Remove(cart);
            await _db.SaveChangesAsync();

            var cnt = _db.ShoppingCart.Where(u => u.UtilisateurId == cart.UtilisateurId).ToList().Count;
            HttpContext.Session.SetInt32(SD.ssShoppingCartCount, cnt);


            return RedirectToAction(nameof(Index));
        }

       





    }
}