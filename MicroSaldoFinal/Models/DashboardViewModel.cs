using System.Collections.Generic;

namespace MicroSaldoFinal.Models
{
    public class DashboardViewModel
    {
        public required Usuario Usuario { get; set; }
        public List<Ingreso> Ingresos { get; set; } = new List<Ingreso>();
        public List<Egreso> Egresos { get; set; } = new List<Egreso>();
        public decimal TotalIngresos { get; set; }
        public decimal TotalEgresos { get; set; }
        public decimal Saldo => TotalIngresos - TotalEgresos;
    }
}
