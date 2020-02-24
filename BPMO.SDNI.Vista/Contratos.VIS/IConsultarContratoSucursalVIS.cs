using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Contratos.VIS
{
    /// <summary>
    /// Interfaz que se utilizara para la consulta de Contratos
    /// </summary>
    public interface IConsultarContratoSucursalVIS
    {
        #region Propiedades
        /// <summary>
        /// Obtiene el identificador del usuario que ha iniciado sesion
        /// </summary>
        int? UsuarioID { get; }
        /// <summary>
        /// Obtiene el identificador de la unidad operativa correspondiente al usuario
        /// </summary>
        int? UnidadOperativaID { get; }
        /// <summary>
        /// Unidad Operativa
        /// </summary>
        UnidadOperativaBO UnidadOperativa {get;}
        /// <summary>
        /// Identificador de la Sucursal
        /// </summary>
        Int32? SucursalId { get; set; }
        /// <summary>
        /// Nombre de la Sucursal
        /// </summary>
        String SucursalNombre { get; set; }
        /// <summary>
        /// Identificador del Cliente
        /// </summary>
        Int32? ClienteId { get; set; }
        /// <summary>
        /// Nombre del Cliente
        /// </summary>
        String ClienteNombre { get; set; }
        /// <summary>
        /// Identificador del Cotrato
        /// </summary>
        Int32? ContratoId { get; set; }
        /// <summary>
        /// Numero del Contrato
        /// </summary>
        String NumeroContrato { get; set; }
        /// <summary>
        /// Tipo del Contrato: RD/FSL/CM/SD
        /// </summary>
        ETipoContrato? TipoContrato { get; set; }
        /// <summary>
        /// Lista de Contratos Encontrados
        /// </summary>
        List<ContratoBO> ContratosEncontrados { get; set; }
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
        void PresentarResultadoConsulta(List<ContratoBO> contratos);
        #endregion
    }
}
