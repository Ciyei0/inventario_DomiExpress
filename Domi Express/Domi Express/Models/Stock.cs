using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domi_Express.Models
{
    [Table("Stock")] // Mapear al nombre exacto de la tabla
    public class Stock
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio.")]
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad disponible debe ser mayor a 0.")]
        public int CantidadDisponible { get; set; }

        public DateTime FechaUltimaModificacion { get; set; } = DateTime.Now;
    }
}