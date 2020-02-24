//Satisface al CU030 - Reporte de Añejamiento de Flota

using System.Collections;
using BPMO.SDNI.Reportes.VIS;

namespace BPMO.SDNI.Flota.Reportes.VIS
{
    public interface IAniejamientoFlotaVIS : IReportPageVIS
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
        /// Identificador del Modelo
        /// </summary>
        int? ModeloID { get; set; }

        /// <summary>
        /// Nombre del Modelo
        /// </summary>
        string ModeloNombre { get; set; }

        /// <summary>
        /// Identificador de Cuenta Cliente
        /// </summary>
        int? CuentaClienteID { get; set; }

        /// <summary>
        /// Nombre del Cliente
        /// </summary>
        string ClienteNombre { get; set; }

        /// <summary>
        /// Identificador del Enumerador de Area/Departamento
        /// </summary>
        int? Area { get; set; }

        /// <summary>
        /// Determina si el reporte a imprimir es detallado
        /// </summary>
        bool? ReporteDetallado { get; set; }

        /// <summary>
        /// Determina si es es por Unidad/EquipoAliado
        /// </summary>
        int? TipoUnidad { get; set; }

        /// <summary>
        /// Etiqueta que sera colocada en el reporte
        /// </summary>
        string EtiquetaReporte { get; set; }

        /// <summary>
        /// Colocal los elementos de la lista en la interfaz.
        /// </summary>
        /// <param name="items">Lista de elementos que se agregaran</param>
        void BindReporteDetallato(ICollection items);

        /// <summary>
        /// Colocal los elementos de la lista en la interfaz.
        /// </summary>
        /// <param name="items">Lista de elementos que se agregaran</param>
        void BindTipoUnidad(ICollection items);

        /// <summary>
        /// Colocal los elementos de la lista en la interfaz
        /// </summary>
        /// <param name="items">Lista de elementos que se agregaran</param>
        void BindArea(ICollection items);
    }
}