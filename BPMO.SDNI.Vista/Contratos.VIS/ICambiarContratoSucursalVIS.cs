using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;

namespace BPMO.SDNI.Contratos.VIS
{
    /// <summary>
    /// Interfaz para implementar en la UI de Cambio de Sucursal de un Contrato
    /// </summary>
    public interface ICambiarContratoSucursalVIS
    {
        #region Propiedades
        /// <summary>
        /// Id del Contrato
        /// </summary>
        int? ContratoId { get; set; }
        /// <summary>
        /// Numero del Contrato
        /// </summary>
        string NumeroContrato { get; set; }
        /// <summary>
        /// Tipo del Contrato: RD/FSL/CM/SD
        /// </summary>
        ETipoContrato? TipoContrato { get; set; }
        /// <summary>
        /// Contrato Completo que sera Actualizado
        /// </summary>
        ContratoBO ContratoConsultado { get; set; }
        /// <summary>
        /// Contrato de referencia para actualizar
        /// </summary>
        ContratoBO ObjetoAnterior { get; set; }
        /// <summary>
        /// Id de la Sucursal Original
        /// </summary>
        int? SucursalIdAntigua { get; set; }
        /// <summary>
        /// Nombre de la Sucursal Original
        /// </summary>
        string SucursalNombreAntigua { get; set; }
        /// <summary>
        /// Id de la Sucursal Nueva
        /// </summary>
        int? SucursalIdNueva { get; set; }
        /// <summary>
        /// Nombre de la Sucursal Nueva
        /// </summary>
        string SucursalNombreNueva { get; set; }
        /// <summary>
        /// Observaciones sobre el cambio del Contrato
        /// </summary>
        string Observaciones { get; set; }
        /// <summary>
        /// Id del Usuario Logueado
        /// </summary>
        int? UsuarioId { get; }
        /// <summary>
        /// Unidad Operativa del Usuario Logueadoo
        /// </summary>
        UnidadOperativaBO UnidadOperativa { get; }
        #endregion
        #region Metodos
        /// <summary>
        /// Despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje que es desplegado</param>
        /// <param name="tipo">Tipo del mensaje que es desplegao</param>
        /// <param name="detalle">Detalle del mensaje que es desplegado</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        /// <summary>
        /// Limpia la sesion de la página
        /// </summary>
        void LimpiarSesion();
        /// <summary>
        /// Redirige a la Información a Consulta de otro Contrato
        /// </summary>
        void RedirigirAConsulta();
        /// <summary>
        /// Redirige a la página de acceso denegado
        /// </summary>
        void RedirigirSinPermisoAcceso();
        /// <summary>
        /// Determina el Paquete que sera enviado a la interfaz de detalle
        /// </summary>
        /// <param name="nombre">Nombre del paquete</param>
        /// <param name="valor">Objeto que sera enviado</param>
        ContratoBO ObtenerPaqueteNavegacion();
        /// <summary>
        /// Establece en la Interfaz el Objeto de Navegación
        /// </summary>
        /// <param name="objetoNavegacion">Contrato que cambiara de sucursal</param>
        void EstablecerPaqueteNavegacion(ContratoBO objetoNavegacion);
        /// <summary>
        /// Deshabilita los campos antiguos del Contrato
        /// </summary>
        void DesactivarCamposIniciales();
        /// <summary>
        /// Desactiva los objetos de la interfaz una vez realizado el cambio de sucursal
        /// </summary>
        void PermitirGuardar(bool permitir);
        #endregion
    }
}
