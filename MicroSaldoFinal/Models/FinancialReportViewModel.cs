using System;
using System.Collections.Generic;

namespace MicroSaldoFinal.Models
{
    public class FinancialReportViewModel
    {
        public List<ReportViewModel> DailyReports { get; set; } = new List<ReportViewModel>();
        public decimal TotalIngresosPeriodo { get; set; }
        public decimal TotalEgresosPeriodo { get; set; }
        public decimal SaldoPeriodo => TotalIngresosPeriodo - TotalEgresosPeriodo;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
