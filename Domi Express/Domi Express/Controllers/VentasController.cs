using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domi_Express.Data;
using Domi_Express.Models;

namespace Domi_Express.Controllers
{
    [Route("Venta")]
    public class VentasController : Controller
    {
        private readonly DomiExpressContext _context;

        public VentasController(DomiExpressContext context)
        {
            _context = context;
        }

        // GET: Venta
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ventas.Include(v => v.Producto).ToListAsync());
        }

        // GET: Venta/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var venta = await _context.Ventas
                .Include(v => v.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venta == null) return NotFound();

            return View(venta);
        }

        // GET: Venta/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewBag.ProductoId = new SelectList(_context.Productos, "Id", "Nombre");
            return View();
        }

        // POST: Venta/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venta venta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ProductoId = new SelectList(_context.Productos, "Id", "Nombre", venta.ProductoId);
            return View(venta);
        }

        // GET: Venta/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null) return NotFound();

            ViewBag.ProductoId = new SelectList(_context.Productos, "Id", "Nombre", venta.ProductoId);
            return View(venta);
        }

        // POST: Venta/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Venta venta)
        {
            if (id != venta.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venta);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.Id))
                        return NotFound();
                    else
                        throw;
                }
            }
            ViewBag.ProductoId = new SelectList(_context.Productos, "Id", "Nombre", venta.ProductoId);
            return View(venta);
        }

        // GET: Venta/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var venta = await _context.Ventas
                .Include(v => v.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venta == null) return NotFound();

            return View(venta);
        }

        // POST: Venta/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta != null)
            {
                _context.Ventas.Remove(venta);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.Id == id);
        }
    }
}
