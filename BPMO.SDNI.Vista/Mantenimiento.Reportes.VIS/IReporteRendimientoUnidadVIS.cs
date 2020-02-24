//Satisface al caso de uso CU060 - Reporte de rendimiento por unidad
using System.Collections;
using BPMO.SDNI.Reportes.VIS;

namespace BPMO.SDNI.Mantenimiento.Reportes.VIS
{
    /// <summary>
    /// Vista para el reporte de Rendimiento por Unidad
    /// </summary>
    public interface IReporteRendimientoUnidadVIS : IReportPageVIS
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
        /// Identificador de la Uniad
        /// </summary>
        int? UnidadID { get; set; }

        /// <summary>
        /// Area de la Unidad
        /// </summary>
        EArea? AreaUnidad { get; set; }

        /// <summary>
        /// Serie de la Unidad
        /// </summary>
        string VIN { get; set; }

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

        /// <summary>
        /// Obtiene o establece un valor que representa el mes que se aplicará al reporte
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? MesFinal { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del cliente
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? ClienteID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes que se aplicará al reporte
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        bool? ReporteGlobal { get; set; }

        /// <summary>
        /// Colocal los elementos de la lista en la interfaz.
        /// </summary>
        /// <param name="items">Lista de elementos que se agregaran</param>
        void BindTipoReporte(ICollection items);

        /// <summary>
        /// Colocal los elementos de la lista en la interfaz.
        /// </summary>
        /// <param name="items">Lista de elementos que se agregaran</param>
        void BindMesFinal(ICollection items);
    }
}