using Microsoft.EntityFrameworkCore;
using Domi_Express.Models; // Asegúrate de usar el namespace correcto para tus modelos

namespace Domi_Express.Data
{
    public class DomiExpressContext : DbContext
    {
        public DomiExpressContext(DbContextOptions<DomiExpressContext> options) : base(options) { }

        // Define las tablas como DbSet
        public DbSet<Producto> Productos { get; set; }
    }
}