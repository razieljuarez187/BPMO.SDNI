using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BPMO.SDNI.Contratos.BO;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.Servicio.Catalogos.BO;
using BPMO.SDNI.Reportes.VIS;

namespace BPMO.SDNI.Mantenimiento.Reportes.VIS
{
    public interface IMantenimientoRealizadoVIS : IReportPageVIS
    {

        #region Propiedades

        #region Propiedades Idealise
        /// <summary>
        /// Usuario autenticado en el sistema
        /// </summary>
        int? UsuarioAutenticado { get; }
        /// <summary>
        /// Unidada operativa donde esta corriendo el sistema
        /// </summary>
        int? UnidadOperativaId { get; }
        /// <summary>
        /// Modulo desde donde esta corriendo el sistema
        /// </summary>
        int? ModuloID { get; }
        #endregion

        int? SucursalID { get; set; }

        string SucursalNombre { get; set; }

        int? TallerID { get; }

        string NumeroVIN { get; set; }

        int? UnidadID { get; set; }

        ETipoContrato? Departamento { get; set; }

        String IdentificadorReporte { get; }

        DateTime? FechaInicio { get; }

        DateTime? FechaFin { get; }

        #endregion

        #region Métodos

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        void enlazarControles(List<TallerBO> talleres);

        #endregion


    }
}
