using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domi_Express.Data;
using Domi_Express.Models;

namespace Domi_Express.Controllers
{
    [Route("Stock")]
    public class StocksController : Controller
    {
        private readonly DomiExpressContext _context;

        public StocksController(DomiExpressContext context)
        {
            _context = context;
        }

        // GET: Stock
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var stocks = await _context.Stocks.Include(s => s.Producto).ToListAsync();
            return View(stocks);
        }

        // GET: Stock/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var stock = await _context.Stocks.Include(s => s.Producto)
                                             .FirstOrDefaultAsync(m => m.Id == id);
            if (stock == null) return NotFound();

            return View(stock);
        }

        // GET: Stock/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewBag.ProductoId = new SelectList(_context.Productos, "Id", "Nombre");
            return View();
        }

        // POST: Stock/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductoId, CantidadDisponible")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                stock.FechaUltimaModificacion = DateTime.Now;
                _context.Add(stock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ProductoId = new SelectList(_context.Productos, "Id", "Nombre", stock.ProductoId);
            return View(stock);
        }

        // GET: Stock/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null) return NotFound();

            ViewBag.ProductoId = new SelectList(_context.Productos, "Id", "Nombre", stock.ProductoId);
            return View(stock);
        }

        // POST: Stock/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, ProductoId, CantidadDisponible")] Stock stock)
        {
            if (id != stock.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    stock.FechaUltimaModificacion = DateTime.Now;
                    _context.Update(stock);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Stocks.Any(e => e.Id == stock.Id)) return NotFound();
                    else throw;
                }
            }
            ViewBag.ProductoId = new SelectList(_context.Productos, "Id", "Nombre", stock.ProductoId);
            return View(stock);
        }

        // GET: Stock/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var stock = await _context.Stocks.Include(s => s.Producto)
                                             .FirstOrDefaultAsync(m => m.Id == id);
            if (stock == null) return NotFound();

            return View(stock);
        }

        // POST: Stock/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock != null)
            {
                _context.Stocks.Remove(stock);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
