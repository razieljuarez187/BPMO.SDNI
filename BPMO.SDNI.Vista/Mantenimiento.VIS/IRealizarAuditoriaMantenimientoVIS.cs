//Satisface al CU020 - Imprimir Auditoria Realizada

using System.Collections.Generic;
using BPMO.Servicio.Catalogos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.BO;
using System.Web.UI.WebControls;
using BPMO.Servicio.Procesos.BO;

namespace BPMO.SDNI.Mantenimiento.VIS
{
    public interface IRealizarAuditoriaMantenimientoVIS
    {
        #region Propiedades
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
        /// <summary>
        /// Identificador de la orden de servicio
        /// </summary>
        int? OrdenServicioID { get; set; }
        /// <summary>
        /// Tipo de mantenimiento de la orden de servicio
        /// </summary>
        string TipoMantenimiento { get; set; }
        /// <summary>
        /// Tecnicos aisgnados a al paquete de mantenimiento de la orden de servicio
        /// </summary>
        List<TecnicoBO> Tecnicos { get; set; }
        /// <summary>
        /// Auditoria realizada
        /// </summary>
        AuditoriaMantenimientoBO Resultado { get; set; }
        /// <summary>
        /// Mantenimiento recivido desde el modulo de recepcion CU009
        /// </summary>
        OrdenServicioBO MantenimientoRecibido { get; set; }
        /// <summary>
        /// Detalle de la auditoria 
        /// </summary>
        List<DetalleAuditoriaMantenimientoBO> DetalleAuditoria { get; set; }
        /// <summary>
        /// Grid de la interfaz 
        /// </summary>
        GridView ActividadesAuditoria { get; set;}
        /// <summary>
        /// Archivo de evidencia 
        /// </summary>
        FileUpload Evidencia { get; set; }
        /// <summary>
        /// Observaciones de la auditoria
        /// </summary>
        string Observaciones { get; set; }
        
        #endregion

        #region Metodos
        /// <summary>
        /// Metodo para mostrar mensajes en la UI
        /// </summary>
        /// <param name="mensaje"></param>
        /// <param name="tipo"></param>
        /// <param name="detalle"></param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        List<string> ObtenerConfiguracionFormatos();

        /// <summary>
        /// Redirige a la impresion de la auditoria
        /// </summary>
        void RedirigirAImprimir();

        /// <summary>
        /// Limpia los datos en sesion de la auditoria
        /// </summary>
        void LimpiarSessionAuditoria();

        /// <summary>
        /// Establece el nombre del reporte y los datos en sesion
        /// </summary>
        /// <param name="key">Nombre del reporte</param>
        /// <param name="value">Datos para la impresion del reporte</param>
        void EstablecerValoresImpresion(string key, object value);
        #endregion

        void RedirigirSinPermisoAcceso();
    }
}
