using System.ComponentModel.DataAnnotations;

namespace MicroSaldoFinal.Models
{
    public class Ingreso
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        // Navigation property
        public Usuario Usuario { get; set; } = null!;
    }
}
