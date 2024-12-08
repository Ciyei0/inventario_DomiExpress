using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domi_Express.Models
{
    [Table("Stock")] // Nombre de la tabla en la base de datos
    public class Stock
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio.")]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; } // Relación con Producto

        [Required(ErrorMessage = "La cantidad disponible es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad disponible debe ser mayor a 0.")]
        public int CantidadDisponible { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FechaUltimaModificacion { get; set; } = DateTime.Now;
    }
}
