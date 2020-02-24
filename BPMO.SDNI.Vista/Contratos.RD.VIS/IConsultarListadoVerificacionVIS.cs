//Satisface al CU008 - Consultar Entrega Recepcion de Unidad
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    public interface IConsultarListadoVerificacionVIS
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
        /// Obtiene o establece el identificador de la sucursal
        /// </summary>
        int? SucursalID { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal
        /// </summary>
        string SucursalNombre { get; set; }
        /// <summary>
        /// Obtiene o establece el número de serie de la unidad
        /// </summary>
        string NumeroSerie { get; set; }
        /// <summary>
        /// Obtiene o establece el npumero econpomico de la unidad
        /// </summary>
        string NumeroEconomico { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador del modelo de la unidad
        /// </summary>
        int? ModeloID { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre del modelo de la unida
        /// </summary>
        string ModeloNombre { get; set; }
        /// <summary>
        /// Obtiene o establece la placa de la unidad
        /// </summary>
        string Placas { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador del cliente
        /// </summary>
        int? ClienteID { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador de la cuenta del cliente
        /// </summary>
        int? CuentaClienteID { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre del cliente
        /// </summary>
        string ClienteNombre { get; set; }
        /// <summary>
        /// Obtiene o establece el número del contrato
        /// </summary>
        string NumeroContrato { get; set; }
        /// <summary>
        /// Obtiene o establece el tipo de listado
        /// </summary>
        int? TipoListado { get; set; }
        /// <summary>
        /// Obtiene o establece el indice de la página de resultado
        /// </summary>
        int IndicePaginaResultado { get; set; }
        /// <summary>
        /// Lista de los check lsit resultantes de la consulta
        /// </summary>
        List<object> Resultado { get; set; }
        #endregion

        #region Métodos
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
        /// Limpia los resultados encontrado
        /// </summary>
        void LimpiarListadosEncontrados();
        /// <summary>
        /// Carga en pantalla los elementos que cumplen con loo filtros de consulta
        /// </summary>
        /// <param name="elementos">Elementos que serán desplegados</param>
        void CargarElementosFlotaEncontrados(List<object> elementos);
        /// <summary>
        /// Establece un paquete para dirigir a la pantalla de DetalleFlotaUI
        /// </summary>
        /// <param name="clave">Clave con la cual se guarda en session el paquete de navegación</param>
        /// <param name="unidadID">identificador de la unidad que será desplegada</param>
        void EstablecerPaqueteNavegacionFlota(string clave, int? unidadID);
        /// <summary>
        /// Establece un paquete para redirigir a la pantalla de registro de Check List
        /// </summary>
        /// <param name="clave">Clave con la cual se guarda en session el paquete de navegación</param>
        /// <param name="contratoID">Identificador del Contrato</param>
        void EstablecerPaqueteNavegacion(string clave, int? contratoID);
        /// <summary>
        /// Establece las opciones válidas para los tipos de  check list
        /// </summary>
        /// <param name="opciones">opciones</param>
        void EstablecerOpcionesTipoListado(Dictionary<int, string> opciones);
        /// <summary>
        /// Redirige a la página de Consulta inicial
        /// </summary>
        void RedirigirAConsulta();
        /// <summary>
        /// Redirige a la página de detalle de elemento de flota
        /// </summary>
        void RedirigirAFlota();
        /// <summary>
        /// Redirige a la página de registrar entrega de unidad
        /// </summary>
        void RedirigirARegistrarEntrega();
        /// <summary>
        /// Redirige a la pagina de registrar recepción de unidad
        /// </summary>
        void RedirigirARegistrarRecepcion();
        /// <summary>
        /// Redirige a la página de acceso denegado
        /// </summary>
        void RedirigirSinPermisoAcceso();  
        /// <summary>
        /// Actualiza el resultado de la consulta
        /// </summary>
        void ActualizarResultado();
        #endregion
    }
}