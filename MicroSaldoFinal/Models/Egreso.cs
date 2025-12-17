using System.ComponentModel.DataAnnotations;

namespace MicroSaldoFinal.Models
{
    public class Egreso
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        public int? ProductoId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        // Navigation properties
        public Usuario Usuario { get; set; } = null!;
        public Producto? Producto { get; set; }
    }
}
