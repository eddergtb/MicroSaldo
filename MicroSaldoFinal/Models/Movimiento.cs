using System.ComponentModel.DataAnnotations;

namespace MicroSaldoFinal.Models
{
    public enum TipoMovimiento
    {
        Ingreso = 1,
        Egreso = 2
    }

    public class Movimiento
    {
        public int Id { get; set; }

        [Required]
        public TipoMovimiento Tipo { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;
    }
}
