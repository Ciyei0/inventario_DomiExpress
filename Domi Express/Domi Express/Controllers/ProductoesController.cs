using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domi_Express.Data;
using Domi_Express.Models;

namespace Domi_Express.Controllers
{
    [Route("Producto")] // Rutas personalizadas
    public class ProductoesController : Controller
    {
        private readonly DomiExpressContext _context;

        public ProductoesController(DomiExpressContext context)
        {
            _context = context;
        }

        // GET: Producto
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .ToListAsync();
            return View(productos);
        }

        // GET: Producto/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var producto = await ObtenerProductoPorId(id);
            if (producto == null) return NotFound();

            return View(producto);
        }

        // GET: Producto/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            CargarListasDesplegables();
            return View();
        }

        // POST: Producto/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CargarListasDesplegables();
            return View(producto);
        }

        // GET: Producto/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            CargarListasDesplegables(producto.CategoriaId, producto.ProveedorId);
            return View(producto);
        }

        // POST: Producto/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Producto producto)
        {
            if (id != producto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            CargarListasDesplegables(producto.CategoriaId, producto.ProveedorId);
            return View(producto);
        }

        // GET: Producto/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var producto = await ObtenerProductoPorId(id);
            if (producto == null) return NotFound();

            return View(producto);
        }

        // POST: Producto/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Métodos privados reutilizables
        private async Task<Producto> ObtenerProductoPorId(int? id)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        private void CargarListasDesplegables(int? categoriaSeleccionada = null, int? proveedorSeleccionado = null)
        {
            ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nombre", categoriaSeleccionada);
            ViewBag.ProveedorId = new SelectList(_context.Proveedores, "Id", "Nombre", proveedorSeleccionado);
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
