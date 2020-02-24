//Satisface al caso de uso CU071 - Reporte de Auditorias Realizadas
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Reportes.VIS;

namespace BPMO.SDNI.Mantenimiento.Reportes.VIS
{
    /// <summary>
    /// Vista para el reporte de Auditorias Realizadas
    /// </summary>
    public interface IReporteAuditoriasRealizadasVIS : IReportPageVIS
    {
        /// <summary>
        /// Identificador de la Sucursal
        /// </summary>
        int? SucursalID { get; set; }

        /// <summary>
        /// Nombre de la sucursal
        /// </summary>
        string SucursalNombre { get; set; }

        /// <summary>
        /// Fecha inicial
        /// </summary>
        DateTime? FechaInicio { get; set; }

        /// <summary>
        /// Fecha Fin
        /// </summary>
        DateTime? FechaFin { get; set; }

        /// <summary>
        /// Identificador del Tecnico
        /// </summary>
        int? TecnicoID { get; set; }

        /// <summary>
        /// Nombre del Tecnico
        /// </summary>
        string TecnicoNombre { get; set; }
    }
}
