using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BooksForAdoption.Models;
using BooksForDonation.Data;

namespace BooksForDonation.Controllers
{
    public class WrittenBiesController : Controller
    {
        private readonly ApplicationDBContext _context;

        public WrittenBiesController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: WrittenBies
        public async Task<IActionResult> Index()
        {
              return _context.WrittenBy != null ? 
                          View(await _context.WrittenBy.ToListAsync()) :
                          Problem("Entity set 'ApplicationDBContext.WrittenBy'  is null.");
        }

        // GET: WrittenBies/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.WrittenBy == null)
            {
                return NotFound();
            }

            var writtenBy = await _context.WrittenBy
                .FirstOrDefaultAsync(m => m.AuthorName == id);
            if (writtenBy == null)
            {
                return NotFound();
            }

            return View(writtenBy);
        }

        // GET: WrittenBies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WrittenBies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorName,ISBN,OrderOfAuthorship")] WrittenBy writtenBy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(writtenBy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(writtenBy);
        }

        // GET: WrittenBies/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.WrittenBy == null)
            {
                return NotFound();
            }

            var writtenBy = await _context.WrittenBy.FindAsync(id);
            if (writtenBy == null)
            {
                return NotFound();
            }
            return View(writtenBy);
        }

        // POST: WrittenBies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AuthorName,ISBN,OrderOfAuthorship")] WrittenBy writtenBy)
        {
            if (id != writtenBy.AuthorName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(writtenBy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WrittenByExists(writtenBy.AuthorName))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(writtenBy);
        }

        // GET: WrittenBies/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.WrittenBy == null)
            {
                return NotFound();
            }

            var writtenBy = await _context.WrittenBy
                .FirstOrDefaultAsync(m => m.AuthorName == id);
            if (writtenBy == null)
            {
                return NotFound();
            }

            return View(writtenBy);
        }

        // POST: WrittenBies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.WrittenBy == null)
            {
                return Problem("Entity set 'ApplicationDBContext.WrittenBy'  is null.");
            }
            var writtenBy = await _context.WrittenBy.FindAsync(id);
            if (writtenBy != null)
            {
                _context.WrittenBy.Remove(writtenBy);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WrittenByExists(string id)
        {
          return (_context.WrittenBy?.Any(e => e.AuthorName == id)).GetValueOrDefault();
        }
    }
}
