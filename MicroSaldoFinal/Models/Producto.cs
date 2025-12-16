using System.ComponentModel.DataAnnotations;

namespace MicroSaldoFinal.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
    }
}
