namespace Domi_Express.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
        public int CantidadDisponible { get; set; }
        public DateTime FechaUltimaModificacion { get; set; } = DateTime.Now;
    }
}