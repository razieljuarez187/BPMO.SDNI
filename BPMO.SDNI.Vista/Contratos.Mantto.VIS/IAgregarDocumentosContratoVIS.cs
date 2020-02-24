// Esta clase satisface los requerimientos del CU028 - Editar Contrato de Mantenimiento
using System;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.Mantto.VIS
{
    public interface IAgregarDocumentosContratoVIS
    {
        #region Propiedades
        /// <summary>
        /// COntrato con la información original que se esta editando
        /// </summary>
        object UltimoObjeto { get; set; }
        /// <summary>
        /// Identificador del contrato
        /// </summary>
        int? ContratoID { get; set; }
        /// <summary>
        /// Estatus del contrato
        /// </summary>
        int? EstatusID { get; set; }
        /// <summary>
        /// Identificador del usuario que esta actualizando el contrato
        /// </summary>
        int? UUA { get; set; }
        /// <summary>
        /// Fecha de la actualización del contrato
        /// </summary>
        DateTime? FUA { get; set; }
        /// <summary>
        /// Usuario autenticado en el sistema
        /// </summary>
        int? UsuarioID { get; }
        /// <summary>
        /// Unidad operativa a la que pertenece el usaurio logueado
        /// </summary>
        int? UnidadOperativaID { get; }
        #endregion

        #region Metodos
        /// <summary>
        /// Estabvlece un paquete en la  variable de session de la vista
        /// </summary>
        /// <param name="key">clave del paquete</param>
        /// <param name="value">paquete que se desea guardar</param>
        void EstablecerPaqueteNavegacion(string key, object value);
        /// <summary>
        /// Obtiene un paquete de la variable de session de la vista
        /// </summary>
        /// <param name="key">clave del paquete que se desea obtener</param>
        /// <returns>paquete recuperadode la vista</returns>
        object ObtenerPaqueteNavegacion(string key);
        /// <summary>
        /// Limpia un paquete de navegación de la sesion de la vista
        /// </summary>
        /// <param name="key">clave del paquete que se desea eliminar</param>
        void LimpiarPaqueteNavegacion(string key);
        /// <summary>
        /// Redirige a la vista del detalle de contrato
        /// </summary>
        void RedirigirADetalles();
        /// <summary>
        /// Redirige a la vista de consultar contrato
        /// </summary>
        void RedirigirAConsulta();
        /// <summary>
        /// Redirige a la vista de acceso denegado
        /// </summary>
        void RedirigirSinPermisoAcceso();
        /// <summary>
        /// Habilita o deshabilita la opcion de registro
        /// </summary>
        /// <param name="permitir">Estatus del control</param>
        void PermitirRegistrar(bool permitir);
        /// <summary>
        /// Limpia la session de la vista
        /// </summary>
        void LimpiarSesion();
        /// <summary>
        /// Despliega un mensaje en la vista
        /// </summary>
        /// <param name="mensaje">mensaje que se va a desplegar</param>
        /// <param name="tipo">Tipo del mensaje</param>
        /// <param name="detalle">Observación del mmensaje</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        #endregion        
    }
}