namespace Domi_Express.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }

        // Relación con Producto
        public ICollection<Producto> Productos { get; set; }
    }
}
