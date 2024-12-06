using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domi_Express.Data;
using Domi_Express.Models;

namespace Domi_Express.Controllers
{
    public class VentasController : Controller
    {
        private readonly DomiExpressContext _context;

        public VentasController(DomiExpressContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            var ventas = _context.Ventas
                .Include(v => v.Producto);
            return View(await ventas.ToListAsync());
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .Include(v => v.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre");
            return View();
        }

        // POST: Ventas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductoId,CantidadVendida,FechaVenta,Total")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener el producto asociado
                    var producto = await _context.Productos.FindAsync(venta.ProductoId);
                    if (producto == null)
                    {
                        ModelState.AddModelError("", "El producto seleccionado no existe.");
                        ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", venta.ProductoId);
                        return View(venta);
                    }

                    // Verificar el stock disponible
                    var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.ProductoId == venta.ProductoId);
                    if (stock == null || stock.CantidadDisponible < venta.CantidadVendida)
                    {
                        ModelState.AddModelError("", "No hay suficiente stock disponible para realizar la venta.");
                        ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", venta.ProductoId);
                        return View(venta);
                    }

                    // Actualizar el stock
                    stock.CantidadDisponible -= venta.CantidadVendida;
                    venta.Total = producto.Precio * venta.CantidadVendida;

                    _context.Add(venta);
                    _context.Update(stock);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Ocurrió un error al guardar la venta: {ex.Message}");
                }
            }

            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", venta.ProductoId);
            return View(venta);
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }

            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", venta.ProductoId);
            return View(venta);
        }

        // POST: Ventas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductoId,CantidadVendida,FechaVenta,Total")] Venta venta)
        {
            if (id != venta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venta);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Error al actualizar la venta: {ex.Message}");
                }
            }

            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", venta.ProductoId);
            return View(venta);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .Include(v => v.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
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
