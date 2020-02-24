//Satisface al CU069 - Reporte de UpTime

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Reportes.VIS;
using System.Collections;

namespace BPMO.SDNI.Mantenimiento.Reportes.VIS
{
    public interface IUpTimeVIS : IReportPageVIS
    {
        /// <summary>
        /// Libros activos
        /// </summary>
        string LibroActivos { get; set; }

        /// <summary>
        /// Identificador de la Sucursal
        /// </summary>
        int? SucursalID { get; set; }

        /// <summary>
        /// Nombre de la sucursal
        /// </summary>
        string SucursalNombre { get; set; }

        /// <summary>
        /// Identificador de Cliente
        /// </summary>
        int? ClienteID { get; set; }

        /// <summary>
        /// Nombre del Cliente
        /// </summary>
        string ClienteNombre { get; set; }

        /// <summary>
        /// Serie de la Unidad
        /// </summary>
        string VIN { get; set; }

        /// <summary>
        /// Tipo de contrato
        /// </summary>
        int? Area { get; set; }

        /// <summary>
        /// Filtro Año
        /// </summary>
        Int32? Anio { get; set; }

        /// <summary>
        /// Filtro mes
        /// </summary>
        Int32? Mes { get; set; }

        /// <summary>
        /// Inicializa los campos de búsqueda
        /// </summary>
        void PrepararBusqueda();

    }
}
