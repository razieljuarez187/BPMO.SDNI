using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.RD.BO;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    /// <summary>
    /// Interfaz para usar en el cambio de Frecuencia de Contratos de RD
    /// </summary>
    public interface IConsultarCambioFrecuenciaRDVIS
    {
        #region Propiedades
        /// <summary>
        /// Obtiene el identificador del usuario que ha iniciado sesion
        /// </summary>
        int? UsuarioID { get; }
        /// <summary>
        /// Unidad Operativa
        /// </summary>
        UnidadOperativaBO UnidadOperativa { get; }
        /// <summary>
        /// Identificador de la Sucursal
        /// </summary>
        int? SucursalId { get; set; }
        /// <summary>
        /// Nombre de la Sucursal
        /// </summary>
        string SucursalNombre { get; set; }
        /// <summary>
        /// Identificador del Cliente
        /// </summary>
        int? ClienteId { get; set; }
        /// <summary>
        /// Nombre del Cliente
        /// </summary>
        string ClienteNombre { get; set; }
        /// <summary>
        /// Identificador del Cotrato
        /// </summary>
        int? ContratoId { get; set; }
        /// <summary>
        /// Numero del Contrato
        /// </summary>
        string NumeroContrato { get; set; }
        /// <summary>
        /// Lista de Contratos Encontrados
        /// </summary>
        List<ContratoRDBO> ContratosConsultados { get; set; }
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
        /// Redirige a la Información a Detalle del Contrato
        /// </summary>
        void RedirigirADetalle();
        /// <summary>
        /// Redirige a la página de acceso denegado
        /// </summary>
        void RedirigirSinPermisoAcceso();
        /// <summary>
        /// Determina el Paquete que sera enviado a la interfaz de detalle
        /// </summary>
        /// <param name="nombre">Nombre del paquete</param>
        /// <param name="valor">Objeto que sera enviado</param>
        void EstablecerPaqueteNavegacion(string nombre, object valor);
        /// <summary>
        /// Presenta en la interfaz los contratos Consultados
        /// </summary>
        /// <param name="contratos">Lista de Cotratos Consultados</param>
        void PresentarResultadoConsulta(List<ContratoRDBO> contratos);
        #endregion
    }
}
