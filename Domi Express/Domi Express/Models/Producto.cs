using Domi_Express.Models;
using System.ComponentModel.DataAnnotations;

public class Producto
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; }

    [Required]
    [Range(0.01, 10000)]
    public decimal Precio { get; set; }

    [StringLength(255)]
    public string Descripcion { get; set; }

    [Required]
    public int CategoriaId { get; set; }

    [Required]
    public int ProveedorId { get; set; }

    public Categoria Categoria { get; set; }
    public Proveedor Proveedor { get; set; }
}