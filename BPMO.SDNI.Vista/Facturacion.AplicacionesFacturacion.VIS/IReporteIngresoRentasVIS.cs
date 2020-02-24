using System;
using BPMO.SDNI.Reportes.VIS;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS {
    public interface IReporteIngresoRentasVIS : IReportPageVIS {
        /// <summary>
        /// Estable u obtiene el id de la sucursal por la que se va filtrar
        /// </summary>
        int? SucursalID { get; set; }

        /// <summary>
        /// Establece u obtiene el nombre de la sucursal por la cual se va a filtrar
        /// </summary>
        string Sucursal { get; set; }

        /// <summary>
        /// Establece u obtiene la fecha de inicio del rango de fechas por la cual se va a filtrar
        /// </summary>
        DateTime? FechaInicio { get; set; }

        /// <summary>
        /// Establece u obtiene la fecha final del rango de fechas por la cual se va a filtrar
        /// </summary>
        DateTime? FechaFin { get; set; }

        string URLImage { get; }

        AdscripcionBO Adscripcion { get; }
    }
}
