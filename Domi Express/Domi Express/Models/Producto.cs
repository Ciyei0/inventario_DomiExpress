using System.ComponentModel.DataAnnotations.Schema;

namespace Domi_Express.Models
{
    [Table("producto")] // Esto asegura que EF mapee correctamente a la tabla `producto`
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string? Descripcion { get; set; }

        // Relación con Categoría
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        // Relación con Proveedor
        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }
    }
}
