using System;
using BPMO.SDNI.Reportes.VIS;

namespace BPMO.SDNI.Contratos.PSL.Reportes.VIS {
    /// <summary>
    /// Interfaz para el reporte general de rentas RO/RE/ROC
    /// </summary>
    public interface IRentaGeneralPSLVIS : IReportPageVIS {
        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de sucursal
        /// </summary>
        int? SucursalID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre de la sucursal
        /// </summary>
        String SucursalNombre { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el año
        /// </summary>
        int? Anio { get; set; }

        /// <summary>
        /// Tipo de reporte: Mensual, Trimestral, Semestral
        /// </summary>
        int? TipoReporte { get; set; }

        /// <summary>
        /// Periodo del Reporte: Meses
        /// </summary>
        int? PeriodoReporte { get; set; }

        /// <summary>
        /// Dia de corte para el reporte
        /// </summary>
        int? DiaCorte { get; set; }
    }
}
