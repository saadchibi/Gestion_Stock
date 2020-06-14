using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestion_Stock.Data;
using Gestion_Stock.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gestion_Stock.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategorieController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategorieController(ApplicationDbContext db)
        {
            _db = db;
        }
        //GET
        public async Task<IActionResult> Index()
        {
            return View(await _db.Categorie.ToListAsync());
        }

        //GET -Create
        public IActionResult Create()
        {
            return View();
        }

        //POST -Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categorie category)
        {
            if (ModelState.IsValid)
            {
                _db.Categorie.Add(category);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        //GET -EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _db.Categorie.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        //POST -Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Categorie category)
        {
            if (ModelState.IsValid)
            {
                _db.Categorie.Update(category);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        //GET -Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _db.Categorie.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        //POST -Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var category = await _db.Categorie.FindAsync(id);
            if (category == null)
            {
                return View();
            }
            _db.Categorie.Remove(category);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET - DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _db.Categorie.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
    }
}