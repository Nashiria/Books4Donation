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
    public class ShipmentsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ShipmentsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Shipments
        public async Task<IActionResult> Index()
        {
              return _context.Shipments != null ? 
                          View(await _context.Shipments.ToListAsync()) :
                          Problem("Entity set 'ApplicationDBContext.Shipments'  is null.");
        }

        // GET: Shipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Shipments == null)
            {
                return NotFound();
            }

            var shipments = await _context.Shipments
                .FirstOrDefaultAsync(m => m.ShipID == id);
            if (shipments == null)
            {
                return NotFound();
            }

            return View(shipments);
        }
       
        // GET: Shipments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shipments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShipID,ShipCost,ShipDate,TransactionNumber,RecieverMail")] Shipments shipments)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shipments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shipments);
        }

        // GET: Shipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Shipments == null)
            {
                return NotFound();
            }

            var shipments = await _context.Shipments.FindAsync(id);
            if (shipments == null)
            {
                return NotFound();
            }
            return View(shipments);
        }

        // POST: Shipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShipID,ShipCost,ShipDate,TransactionNumber,RecieverMail")] Shipments shipments)
        {
            if (id != shipments.ShipID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shipments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShipmentsExists(shipments.ShipID))
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
            return View(shipments);
        }

        // GET: Shipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Shipments == null)
            {
                return NotFound();
            }

            var shipments = await _context.Shipments
                .FirstOrDefaultAsync(m => m.ShipID == id);
            if (shipments == null)
            {
                return NotFound();
            }

            return View(shipments);
        }

        // POST: Shipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Shipments == null)
            {
                return Problem("Entity set 'ApplicationDBContext.Shipments'  is null.");
            }
            var shipments = await _context.Shipments.FindAsync(id);
            if (shipments != null)
            {
                _context.Shipments.Remove(shipments);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShipmentsExists(int id)
        {
          return (_context.Shipments?.Any(e => e.ShipID == id)).GetValueOrDefault();
        }
    }
}
