using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Primitivos.Enumeradores;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.BO;

namespace BPMO.SDNI.Mantenimiento.VIS
{
    public interface IDetalleConfiguracionParametrosAuditoriaVIS
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
        /// <summary>
        /// Identificador de la configuracion de la auditoria
        /// </summary>
        int? ConfiguracionID { get; set; }
        /// <summary>
        /// Nombre del tipo de servicio
        /// </summary>
        string TipoMantenimiento { get; set; }
        /// <summary>
        /// Identificador del tipo de servicio
        /// </summary>
        int? TipoMantenimientoID { get; set; }
        /// <summary>
        /// Nombre del taller 
        /// </summary>
        string Taller { get; set; }
        /// <summary>
        /// identificador de la sucursal
        /// </summary>
        int? SucursalID { get; set; }
        /// <summary>
        /// Nomnre de la sucursal
        /// </summary>
        string SucursalNombre { get; set; }
        /// <summary>
        /// Identificador del taller
        /// </summary>
        int? TallerID { get; set; }
        /// <summary>
        /// nombre del modelo
        /// </summary>
        string Modelo { get; set; }
        /// <summary>
        /// identificador del modelo
        /// </summary>
        int? ModeloID { get; set; }
        /// <summary>
        /// Numero de actividades aleatorias
        /// </summary>
        int? Aleatorias { get; set; }
        /// <summary>
        /// Grid de las actividades 
        /// </summary>
        GridView GridActividadesAuditoria { get; set; }
        /// <summary>
        /// Grid de las configuraciones
        /// </summary>
        GridView GridConfiguracionesAuditoria { get; set; }
        /// <summary>
        /// Detalles de las configuraciones
        /// </summary>
        List<DetalleConfiguracionAuditoriaMantenimientoBO> ActividadesAuditoria { get; set; }
        /// <summary>
        /// lista de Configuraciones 
        /// </summary>
        List<ConfiguracionAuditoriaMantenimientoBO> ConfiguracionesAuditoria { get; set; }
        /// <summary>
        /// Configuracion recibida
        /// </summary>
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
        /// <summary>
        /// Borrar todoa las variables de session
        /// </summary>
        void LimpiarSession();
        /// <summary>
        /// Redirige si no hay permisos
        /// </summary>
        void RedirigirSinPermisoAcceso();
        #endregion

        
    }
}
