using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.BO;

namespace BPMO.SDNI.Mantenimiento.VIS
{
    public interface IRegistrarConfiguracionParametrosAuditoriaVIS
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

        #region Propiedades Interfaz

        ETipoMantenimiento? TipoMantenimiento { get; set; }

        int? Aleatorias { get; set; }

        GridView GridActividadesAuditoria { get; set; }

        GridView GridConfiguracionesAuditoria { get; set; }

        List<DetalleConfiguracionAuditoriaMantenimientoBO> ActividadesAuditoria { get; set; }

        List<ConfiguracionAuditoriaMantenimientoBO> ConfiguracionesAuditoria { get; set; }

        #endregion

        #endregion

        #region Metodos

        /// <summary>
        /// Metodo para mostrar mensajes en la UI
        /// </summary>
        /// <param name="mensaje"></param>
        /// <param name="tipo"></param>
        /// <param name="detalle"></param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        void LimpiarSession();

        #endregion
    }
}
