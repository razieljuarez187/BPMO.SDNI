//Satisface el CU024 - Reporte de Dollar Utilization
using System;
using BPMO.SDNI.Reportes.VIS;

namespace BPMO.SDNI.Flota.Reportes.VIS
{
    /// <summary>
    /// Interface que debe implementar un visor de reporte de Dollar Utilization
    /// </summary>
    public interface IDollarUtilizationVIS : IReportPageVIS
    {
        /// <summary>
        /// Obtiene o establece un valor que representa identificador de la Sucursal
        /// </summary>
        int? SucursalID { get; set; }
        /// <summary>
        /// Obtiene o establece un valor que representa el Año de la fecha inicial
        /// </summary>
        int? AnioFechaInicio { get; set; }
        /// <summary>
        /// Obtiene o establece un valor que representa el Año de la fecha final
        /// </summary>
        int? AnioFechaFin { get; set; }
        /// <summary>
        /// Obtiene o establece un valor que representa el Mes de la fecha inicial
        /// </summary>
        int? MesFechaInicio { get; set; }
        /// <summary>
        /// Obtiene o establece un valor que representa el Mes de la fecha final
        /// </summary>
        int? MesFechaFin { get; set; }
    }
}
