using System;
using BPMO.SDNI.Reportes.VIS;

namespace BPMO.SDNI.Contratos.PSL.Reportes.VIS {

    /// <summary>
    /// Vista para funcionalidad de reporte de Detallado por Sucursal en e-renta
    /// </summary>
    public interface IDetalladoPSLSucursalVIS : IReportPageVIS {
        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del modelo
        /// </summary>
        int? ModeloID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre del modelo
        /// </summary>
        String ModeloNombre { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de sucursal
        /// </summary
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
        /// Obtiene o establece un valor que representa el mes que se aplicará al reporte
        /// </summary>
        int? Mes { get; set; }
    }
}
