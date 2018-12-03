using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinanceWebScraper.Models;
using FinanceWebScraper1._1.Data;
using Microsoft.AspNetCore.Authorization;
using FinanceWebScraper.Services;

namespace FinanceWebScraper1._1.Controllers
{
    public class StocksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StocksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stocks
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stock.ToListAsync());
        }

        // GET: Stocks
        [Authorize]
        public async Task<IActionResult> History()
        {
            return View(await _context.Stock.ToListAsync());
        }

        // GET: Stocks/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock
                .FirstOrDefaultAsync(m => m.ID == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // GET: Stocks/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Symbol,Change,PercentChange,Currency,AverageVolume,MarketCap,Price,SnapshotTime")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                Scraper s = new Scraper("asangeethu@yahoo.com", "@nuk1978");
                var snapShot = s.Scrape();
                foreach ( var item in snapShot)
                {
                    item.SnapshotTime = DateTime.Now;
                    _context.Add(item);//adds the object to the context
                    await _context.SaveChangesAsync();//saves to the database

                }
                
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Stocks/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return View(stock);
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Symbol,Change,PercentChange,Currency,AverageVolume,MarketCap,Price,SnapshotTime")] Stock stock)
        {
            if (id != stock.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(stock.ID))
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
            return View(stock);
        }

        // GET: Stocks/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock
                .FirstOrDefaultAsync(m => m.ID == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // POST: Stocks/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stock = await _context.Stock.FindAsync(id);
            _context.Stock.Remove(stock);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockExists(int id)
        {
            return _context.Stock.Any(e => e.ID == id);
        }
    }
}
