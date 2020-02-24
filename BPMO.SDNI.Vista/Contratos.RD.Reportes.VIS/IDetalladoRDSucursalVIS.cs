//Satisface el CU018 – Reporte Detallado de Renta Diaria por Sucursal

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Reportes.VIS;

namespace BPMO.SDNI.Contratos.RD.Reportes.VIS
{
    /// <summary>
    /// Interface que define la funcionalidad para una vista que generá un Reporte Detallado de Renta Diaria por Sucursal
    /// </summary>
    public interface IDetalladoRDSucursalVIS : IReportPageVIS
    {
        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del ´modelo
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? ModeloID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre del modelo
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String ModeloNombre { get; set; }

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
        /// Obtiene o establece un valor que representa el año
        /// </summary>
        /// <value>Valor de tipo ETipoContrato</value>
        int? Anio { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes que se aplicará al reporte
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? Mes { get; set; }
    }
}
