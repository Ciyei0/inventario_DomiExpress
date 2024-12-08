using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domi_Express.Data;
using Domi_Express.Models;

namespace Domi_Express.Controllers
{
    [Route("Proveedor")] // Personaliza la ruta base del controlador
    public class ProveedorsController : Controller
    {
        private readonly DomiExpressContext _context;

        public ProveedorsController(DomiExpressContext context)
        {
            _context = context;
        }

        // GET: Proveedor
        [HttpGet("")]
        public async Task<IActionResult> Index(string searchString)
        {
            // Consulta inicial
            var proveedores = _context.Proveedores.AsQueryable();

            // Filtrado de búsqueda
            if (!string.IsNullOrEmpty(searchString))
            {
                proveedores = proveedores.Where(p =>
                    p.Nombre.Contains(searchString) ||
                    p.Telefono.Contains(searchString) ||
                    p.Email.Contains(searchString));
            }

            // Retorna la vista con los datos filtrados
            return View(await proveedores.ToListAsync());
        }

        // GET: Proveedor/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var proveedor = await _context.Proveedores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proveedor == null) return NotFound();

            return View(proveedor);
        }

        // GET: Proveedor/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proveedor/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        // GET: Proveedor/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null) return NotFound();

            return View(proveedor);
        }

        // POST: Proveedor/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Proveedor proveedor)
        {
            if (id != proveedor.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Proveedores.Any(e => e.Id == proveedor.Id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(proveedor);
        }

        // GET: Proveedor/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var proveedor = await _context.Proveedores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proveedor == null) return NotFound();

            return View(proveedor);
        }

        // POST: Proveedor/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor != null)
            {
                _context.Proveedores.Remove(proveedor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
