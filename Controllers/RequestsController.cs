using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BooksForAdoption.Models;
using BooksForDonation.Data;
using Microsoft.AspNetCore.Identity;
using System.Web;
using BooksForAdoption.Models;
using Microsoft.AspNet.Identity;
namespace BooksForDonation.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public RequestsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Requests
        public async Task<IActionResult> Index()
        {
              return _context.Requests != null ? 
                          View(await _context.Requests.ToListAsync()) :
                          Problem("Entity set 'ApplicationDBContext.Requests'  is null.");
        }
        public async Task<IActionResult> MyRequests()
        {
            return _context.Requests != null ?
                        View(await _context.Requests.ToListAsync()) :
                        Problem("Entity set 'ApplicationDBContext.Requests'  is null.");
        }
        public async Task<IActionResult> MyDonations()
        {
            return _context.Requests != null ?
                        View(await _context.Requests.ToListAsync()) :
                        Problem("Entity set 'ApplicationDBContext.Requests'  is null.");
        }
        // GET: Requests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
           
                if (id == null || _context.Requests == null)
                {
                    return NotFound();
                }

                var requests = await _context.Requests
                    .FirstOrDefaultAsync(m => m.RequestID == id);
                if (requests == null)
                {
                    return NotFound();
                }

                return View(requests);
            }
           

        public async Task<IActionResult> History(int? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var requests = await _context.Requests
                .FirstOrDefaultAsync(m => m.RequestID == id);
            if (requests == null)
            {
                return NotFound();
            }

            return View(requests);
        }
        // GET: Requests/Create
        public IActionResult Create()
        {
            return View();
        }
        public ActionResult SearchAct()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search([Bind("RequestID,ISBN")] Requests requests)
        {
            return View(requests);
        }
        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestID,ISBN")] Requests requests)
        {
            bool duplicate = false;
                  foreach(var item in _context.Requests)
            {
                if(item.ISBN == requests.ISBN && item.RecieverMail== User.Identity.GetUserName()) { duplicate = true; return View(requests); }
            }
            if (!duplicate) { 
                    Book b = new Book();
                    b=b.getFromISBN(requests.ISBN,false);
                    b.registerBook(b.ISBN);
                    requests.ShipDate = DateTime.Now;
                    requests.RequestDate = DateTime.Now;
                    requests.RecieverMail = User.Identity.GetUserName();
                    _context.Add(requests);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
            }

            return View(requests);
        }
        public async Task<IActionResult> Donate([Bind("RequestID,DonatorMail,DonatorName,DonatorNote")] Requests requests)
        {
            Transactions transactions = new Transactions();
            int id = requests.RequestID;
            
            Requests req = await _context.Requests.FindAsync(id);
            requests.ISBN= req.ISBN;
            requests.RequestDate = req.RequestDate;
            requests.RecieverMail = req.RecieverMail;
            requests.ShipDate = req.ShipDate;
            transactions.donateRequest(requests);
            return RedirectToAction(nameof(Index));
        }
        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var requests = await _context.Requests.FindAsync(id);
            if (requests == null)
            {
                return NotFound();
            }
            return View(requests);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestID,ISBN,RequestDate,ShipDate,RecieverMail")] Requests requests)
        {
            if (id != requests.RequestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requests);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestsExists(requests.RequestID))
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
            return View(requests);
        }

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var requests = await _context.Requests
                .FirstOrDefaultAsync(m => m.RequestID == id);
            if (requests == null)
            {
                return NotFound();
            }

            return View(requests);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Requests == null)
            {
                return Problem("Entity set 'ApplicationDBContext.Requests'  is null.");
            }
            var requests = await _context.Requests.FindAsync(id);
            if (requests != null)
            {
                _context.Requests.Remove(requests);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestsExists(int id)
        {
          return (_context.Requests?.Any(e => e.RequestID == id)).GetValueOrDefault();
        }
    }
}
