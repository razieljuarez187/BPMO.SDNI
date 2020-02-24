// Satisface el CU028 – Reporte Días de Renta
// Satisface el CU029 – Reporte Días de Renta por Tipo de Unidad

using System;
using BPMO.SDNI.Reportes.VIS;

namespace BPMO.SDNI.Flota.Reportes.VIS
{
    /// <summary>
    /// Interface que define la funcionalidad para una vista que generá un Reporte de Dias de Renta
    /// </summary>
    public interface IDiasRentaVIS : IReportPageVIS
    {
        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de sucursal
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? SucursalID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre de la sucursal
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String SucursalNombre { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el año de la fecha de inicio
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? AnioFechaInicio { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes de la fecha de inicio
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? MesFechaInicio { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el año de la fecha de fin
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? AnioFechaFin { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes de la fecha de fin
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? MesFechaFin { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del reporte a visualizar
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String IdentificadorReporte { get; }       
    }
}
