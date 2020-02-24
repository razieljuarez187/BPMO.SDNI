// Satisface al caso de uso CU038 Ver Pagos de Contrato
// Satisface a la Solicitud de Cambios SC0008

using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Facturacion.AplicacionesPago.VIS
{
    /// <summary>
    /// Interface que define las acciones que debe de contener una vista que ejecuta la funcionalidad de Ver Pagos de Contrato
    /// </summary>
    public interface IVerPagosContratoVIS
    {
        #region Propiedades
        /// <summary>
        /// Obtiene el Numero o Folio de Contrato proporcionado
        /// </summary>
        string NumeroContrato { get; }
        /// <summary>
        /// Identificador de la Unidad Operativa del Usuario actual
        /// </summary>
        int? UnidadOperativaId { get; }
        /// <summary>
        /// URL del Logotipo de la Unidad Operativa
        /// </summary>
        string URLLogoEmpresa { get; }
        /// <summary>
        /// Obtiene un valor que representa el identificador del usuario actual
        /// </summary>
        int? UsuarioId { get; }
        #endregion

        #region Metodos
        /// <summary>
        /// Redirige a la Impresion del Historico de Pagos
        /// </summary>
        void IrAImprimirHistorico();
        /// <summary>
        /// Establece el paquete de navegacion con los datos necesarios para imprimir el historico de pagos
        /// </summary>
        /// <param name="codigoNavegacion">Codigo de acceso a los datos del Historico de Pagos</param>
        /// <param name="paqueteNavegacion"></param>
        void EstablecerPaqueteNavegacionImprimir(string codigoNavegacion, object paqueteNavegacion);
        /// <summary>
        /// Despleiga al usuario un mensaje del sistema.
        /// </summary>
        /// <param name="mensaje">Texto o contenido del mensaje</param>
        /// <param name="tipo">Tipo de Mensaje a desplegar</param>
        /// <param name="detalle">Detalle del Mensaje</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        /// <summary>
        /// Configura al Validador de Formato del Numero o Folio del Contrato.
        /// </summary>
        void ConfigurarValidadorFormato();
        /// <summary>
        /// Redirecciona la Visualización de Sin permiso de Acceso
        /// </summary>
        void RedirigirSinPermisoAcceso();
        /// <summary>
        /// Habilita o Deshabilita la opción de Ver Pagos
        /// </summary>
        /// <param name="ver"></param>
        void PermitirVerPagos(bool ver);
        #endregion
    }
}
