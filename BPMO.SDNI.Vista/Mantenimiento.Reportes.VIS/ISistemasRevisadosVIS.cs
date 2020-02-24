//Satisface el CU066 - Reporte Sistemas Revisados
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Reportes.VIS;

namespace BPMO.SDNI.Mantenimiento.Reportes.VIS
{
    /// <summary>
    /// Interface para el control de la vista
    /// </summary>
    public interface ISistemasRevisadosVIS : IReportPageVIS
    {
        #region Propiedades
        /// <summary>
        /// Obtiene o establece un identificador de unidad
        /// </summary>
        int? UnidadID { get; set; }

        /// <summary>
        /// Obtiene o establece un identificador de unidad operativa
        /// </summary>
        int? UnidadOperativaID { get; }

        /// <summary>
        /// Obtiene o establece un identificador de sucursal
        /// </summary>
        int? SucursalID { get; set; }

        /// <summary>
        /// Obtiene o establece un identificador de cliente
        /// </summary>
        int? ClienteID { get; set; }

        /// <summary>
        /// Obtiene o establece el numero de serie de la unidad
        /// </summary>
        string NumeroSerie { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la sucursal
        /// </summary>
        string NombreSucursal { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del cliente
        /// </summary>
        string NombreCliente { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha inicio
        /// </summary>
        DateTime? FechaInicio{ get; set; }

        /// <summary>
        /// Obtiene o establece la fecha fin
        /// </summary>
        DateTime? FechaFin { get; set; }
        #endregion

        #region Metodos
        /// <summary>
        /// Obtiene o establece un identificador de unidad
        /// <param name="mensaje">Mensaje por desplegar</param>
        /// <param name="tipo">Tipo de mensaje</param>
        /// <param name="msjDetalle">Detalle del mensaje</param>
        /// </summary>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion
    }
}
