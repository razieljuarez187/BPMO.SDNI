using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IConsultarListadoVerificacionPSLVIS {
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
        /// Obtiene la Sucursal seleccionada de la interfaz
        /// </summary>
        SucursalBO SucursalSeleccionada { get; }
        /// <summary>
        /// Obtiene o establece el identificador de la unidad
        /// </summary>
        int? UnidadID { get; set; }
        /// <summary>
        /// Obtiene o establece el número de serie de la unidad
        /// </summary>
        string NumeroSerie { get; set; }
        /// <summary>
        /// Obtiene o establece el número económico de la unidad
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
        /// Obtiene el identificador del Modulo del Sistema
        /// </summary>
        int? ModuloID { get; }
        /// <summary>
        /// Obtiene o establece el índice de la página de resultado
        /// </summary>
        int IndicePaginaResultado { get; set; }
        /// <summary>
        /// Lista de los check lsit resultantes de la consulta
        /// </summary>
        List<object> Resultado { get; set; }
        /// <summary>
        /// Lista de Sucursales a las que tiene permiso el usuario
        /// </summary>
        List<SucursalBO> SucursalesAutorizadas { get; set; }
        #endregion

        #region Métodos
        /// <summary>
        /// Despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje que es desplegado</param>
        /// <param name="tipo">Tipo del mensaje que es desplegado</param>
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
        /// Pobla la lista de sucursales en un control de la interfaz
        /// </summary>
        /// <param name="lstSucursales"></param>
        void CargarSucursales(List<SucursalBO> lstSucursales);
        /// <summary>
        /// Establece una sucursal específica en el control de sucursales
        /// </summary>
        /// <param name="Id"></param>
        void EstablecerSucursalSeleccionada(int? Id);
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
        /// <summary>
        /// Guarda un paquete en la variable de session
        /// </summary>
        /// <param name="key">Clave para identificar el paquete</param>
        /// <param name="value">Paquete que se desea guardar en sesion</param>
        void EstablecerPaqueteNavegacion(string key, object value);
        /// <summary>
        /// Redirige a la página de Impresión de Check List
        /// </summary>
        /// <param name="error">Mensaje que será mostrado como error</param>
        void RedirigirAImprimir(string error);
        #endregion
    }
}