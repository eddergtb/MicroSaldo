namespace MicroSaldoFinal.Models
{
    public class ReportViewModel
    {
        public DateTime Fecha { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalEgresos { get; set; }
        public decimal Resultado => TotalIngresos - TotalEgresos;
    }
}
