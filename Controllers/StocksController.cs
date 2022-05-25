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
    public class StocksController : Controller
    {
        private readonly ApplicationDBContext _context;

        public StocksController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Stocks
        public async Task<IActionResult> Index()
        {
              return _context.Stocks != null ? 
                          View(await _context.Stocks.ToListAsync()) :
                          Problem("Entity set 'ApplicationDBContext.Stocks'  is null.");
        }

        // GET: Stocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Stocks == null)
            {
                return NotFound();
            }

            var stocks = await _context.Stocks
                .FirstOrDefaultAsync(m => m.StockID == id);
            if (stocks == null)
            {
                return NotFound();
            }

            return View(stocks);
        }

        // GET: Stocks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StockID,TransactionNumber,Quantity,ISBN")] Stocks stocks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stocks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stocks);
        }

        // GET: Stocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Stocks == null)
            {
                return NotFound();
            }

            var stocks = await _context.Stocks.FindAsync(id);
            if (stocks == null)
            {
                return NotFound();
            }
            return View(stocks);
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StockID,TransactionNumber,Quantity,ISBN")] Stocks stocks)
        {
            if (id != stocks.StockID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stocks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StocksExists(stocks.StockID))
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
            return View(stocks);
        }

        // GET: Stocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Stocks == null)
            {
                return NotFound();
            }

            var stocks = await _context.Stocks
                .FirstOrDefaultAsync(m => m.StockID == id);
            if (stocks == null)
            {
                return NotFound();
            }

            return View(stocks);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Stocks == null)
            {
                return Problem("Entity set 'ApplicationDBContext.Stocks'  is null.");
            }
            var stocks = await _context.Stocks.FindAsync(id);
            if (stocks != null)
            {
                _context.Stocks.Remove(stocks);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StocksExists(int id)
        {
          return (_context.Stocks?.Any(e => e.StockID == id)).GetValueOrDefault();
        }
    }
}
