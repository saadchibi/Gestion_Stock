using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gestion_Stock.Data;
using Gestion_Stock.Models;
using Microsoft.AspNetCore.Identity;

namespace Gestion_Stock.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UtilisateurController : Controller
    {
        private readonly ApplicationDbContext _context;


        public UtilisateurController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Utilisateur.ToListAsync());
        }
        // GET: Admin/Produits/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilisateur = await _context.Utilisateur
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilisateur == null)
            {
                return NotFound();
            }

            return View(utilisateur);
        }

        //Get edit

        public async Task<IActionResult> Edit(string id)
        {
            IdentityUser user = await _context.Utilisateur.FindAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email, string phoneNumber)
        {
            IdentityUser user = await _context.Utilisateur.FindAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(email))
                    user.Email = email;
                else
                    ModelState.AddModelError("", "Email cannot be empty");

                if (!string.IsNullOrEmpty(phoneNumber))
                    user.PhoneNumber = phoneNumber;
                else
                    ModelState.AddModelError("", "phone number cannot be empty");

                if (!string.IsNullOrEmpty(email))
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return RedirectToAction(nameof(Index));
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }


        ////// GET: Admin/Produits/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var utilisateur = await _context.Utilisateur.FindAsync(id);
        //    if (utilisateur == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(utilisateur);
        //}

        //// POST: Admin/Produits/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("nom,address,Ville,codePostal")] Utilisateur utilisateur)
        //{
        //    if (id != utilisateur.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(utilisateur);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UtilisateurExists(utilisateur.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(utilisateur);
        //}

        // GET: Admin/Produits/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilisateur = await _context.Utilisateur
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilisateur == null)
            {
                return NotFound();
            }

            return View(utilisateur);
        }

        // POST: Admin/Produits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var utilisateur = await _context.Utilisateur.FindAsync(id);
            _context.Utilisateur.Remove(utilisateur);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtilisateurExists(string id)
        {
            return _context.Utilisateur.Any(e => e.Id == id);
        }
    }
}