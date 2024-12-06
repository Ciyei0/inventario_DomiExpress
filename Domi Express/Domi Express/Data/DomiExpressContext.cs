using Microsoft.EntityFrameworkCore;
using Domi_Express.Models; // Asegúrate de que este namespace coincide con tus modelos

namespace Domi_Express.Data
{
    public class DomiExpressContext : DbContext
    {
        public DomiExpressContext(DbContextOptions<DomiExpressContext> options) : base(options) { }

        // Registra las tablas como DbSet
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Venta> Ventas { get; set; }
    }
}
