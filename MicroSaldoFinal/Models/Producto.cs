using System.ComponentModel.DataAnnotations;

namespace MicroSaldoFinal.Models
{
    public class Producto
    {
        public int Id { get; set; }

        // Corregido: Un producto SIEMPRE debe pertenecer a un usuario.
        public int UsuarioId { get; set; }

        [Required, StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        // Navigation property
        public Usuario? Usuario { get; set; }

        // Navigation property para la relación con Egresos
        public ICollection<Egreso> Egresos { get; set; } = new List<Egreso>();
    }
}
