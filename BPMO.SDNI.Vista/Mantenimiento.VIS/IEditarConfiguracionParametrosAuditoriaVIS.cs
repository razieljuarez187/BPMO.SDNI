using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Mantenimiento.VIS
{
    public interface IEditarConfiguracionParametrosAuditoriaVIS
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

        int? ConfiguracionID { get; set; }

        string TipoMantenimiento { get; set; }

        int? TipoMantenimientoID { get; set; }

        int? SucursalID { get; set; }

        string SucursalNombre { get; set; }

        string Taller { get; set; }

        int? TallerID { get; set; }

        string Modelo { get; set; }

        int? ModeloID { get; set; }

        int? Aleatorias { get; set; }

        GridView GridActividadesAuditoria { get; set; }

        GridView GridConfiguracionesAuditoria { get; set; }

        List<DetalleConfiguracionAuditoriaMantenimientoBO> ActividadesAuditoria { get; set; }

        List<ConfiguracionAuditoriaMantenimientoBO> ConfiguracionesAuditoria { get; set; }

        ConfiguracionAuditoriaMantenimientoBO ConfiguracionRecibida { get; set; }

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

        void RedirigirSinPermisoAcceso();
    }
}
