using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IRegistrarRecepcionPSLVIS {
        #region Propiedades

        /// <summary>
        /// Obtiene el número de la página actual
        /// </summary>
        int? PaginaActual { get; }
        /// <summary>
        /// Obtiene el identificador de la unidad operativa
        /// </summary>
        int? UnidadOperativaID { get; }
        /// <summary>
        /// Obtiene el identificador del módulo
        /// </summary>
        int? ModuloID { get; }
        /// <summary>
        /// Obtiene el identificador del usuario autenticado en el sistema
        /// </summary>
        int? UsuarioID { get; }
        /// <summary>
        /// Obtiene o establece el nombre del usuario que realiza el check list
        /// </summary>
        string NombreUsuarioRecibe { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador del contrato para el cual se esta registrando el check list
        /// </summary>
        int? ContratoID { get; set; }
        /// <summary>
        /// Obtiene o establece el número del contrato de renta diaria
        /// </summary>
        string NumeroContrato { get; set; }
        /// <summary>
        /// Obtiene o establece el estatus del contrato
        /// </summary>
        int? EstatusContratoID { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador de la línea de contrato a la cual pertenece el check List
        /// </summary>
        int? LineaContratoID { get; set; }
        /// <summary>
        /// Obtiene o establece la fecha del contrato
        /// </summary>
        DateTime? FechaContrato { get; set; }
        /// <summary>
        /// Obtiene o establece la hora del contrato
        /// </summary>
        TimeSpan? HoraContrato { get; set; }
        /// <summary>
        /// Nombre del Cliente para el cual se genera el check list
        /// </summary>
        string NombreCliente { get; set; }
        /// <summary>
        /// Obtiene o establece el número de licencia del operador
        /// </summary>
        string NumeroLicencia { get; set; }
        /// <summary>
        /// Placas de la unidad que será entregada
        /// </summary>
        string PlacasEstatales { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador de la unidad
        /// </summary>
        int? UnidadID { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador del equipo
        /// </summary>
        int? EquipoID { get; set; }
        /// <summary>
        /// Obtiene o establece el número de serie de la unidad que será entregada
        /// </summary>
        string NumeroSerie { get; set; }
        /// <summary>
        /// Obtiene o establece el número económico de la unidad que será entregada
        /// </summary>
        string NumeroEconomico { get; set; }
        /// <summary>
        /// Capacidad total del tanque de conbustible de la unidad seleccionada en el contrato
        /// </summary>
        decimal? CapacidadTanque { get; set; }
        /// <summary>
        /// Obtiene o establece el tipo de listado que se va registrar
        /// </summary>
        int? TipoListado { get; set; }
        /// <summary>
        /// Obtiene o establece el tipo de Unidad al que se le realizará su check list
        /// </summary>
        int? TipoListadoVerificacionPSL { get; set; }
        /// <summary>
        /// Obtiene o establece el tipo de contrato que se va registrar
        /// </summary>
        int? TipoContrato { get; set; }
        /// <summary>
        /// Obtiene o establece la fecha en la que se registra el listado
        /// </summary>
        DateTime? FechaListado { get; set; }
        /// <summary>
        /// Obtiene o establece la hora en la que se registra el listado
        /// </summary>
        TimeSpan? HoraListado { get; set; }
        /// <summary>
        /// Obtiene o establece las horas de recepción
        /// </summary>
        int? Horometro { get; set; }
        /// <summary>
        /// Obtiene o establece el combustible de recepción
        /// </summary>
        int? Combustible { get; set; }

        #region Información Entrega
        /// <summary>
        /// Obtiene o establece el identificador del checkList de entrega
        /// </summary>
        int? CheckListEntregaID { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre del usuario que entregó la unidad
        /// </summary>
        string NombreUsuarioEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece la fecha del check list
        /// </summary>
        DateTime? FechaListadoEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece la hora en la que se registra el listado
        /// </summary>
        TimeSpan? HoraListadoEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece las horas de entrega
        /// </summary>
        int? HorometroEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece el combustible de entrega
        /// </summary>
        int? CombustibleEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece laa observaciones de la documentación
        /// </summary>
        string ObservacionesEntrega { get; set; }

        #endregion

        // <summary>
        /// Obtiene o establece la observación de la sección de la unidad
        /// </summary>
        string ObservacionesRecepcion { get; set; }

        /// <summary>
        /// Indica la tolerancia en kilometros para la recepción
        /// </summary>
        int? KilometrajeDiario { get; set; }

        /// <summary>
        /// Obtiene o establece el listado de las imagenes agregadas a la sección
        /// </summary>
        object NuevosArchivos { get; set; }
        /// <summary>
        /// Obtiene en área de la unidad
        /// </summary>
        EArea? Area { get; set; }

        #region REQ 13285 Lista de acciones permitirdas para el usuario.

        List<CatalogoBaseBO> ListaAcciones { get; set; }

        #endregion
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
        /// Limpia la session de la página
        /// </summary>
        void LimpiarSesion();
        /// <summary>
        /// Prepara la interfaz para un nuevo registro de CheckList
        /// </summary>
        void PrepararNuevo();
        /// <summary>
        /// Prepara la vista para una cancelación
        /// </summary>
        //void PrepararCancelacion(bool estado);
        /// <summary>
        /// Redirige a la página de acceso denegado
        /// </summary>
        void RedirigirSinPermisoAcceso();
        /// <summary>
        /// Redirige a la página de consulta de Check List
        /// </summary>
        void RedirigirAConsulta();
        /// <summary>
        /// Redirige a la página de Impresión de Check LIst
        /// </summary>
        /// <param name="error">Mensaje que será mostrado como error</param>
        void RedirigirAImprimir(string error);
        /// <summary>
        /// Redirige a la página de cancelación de contrato
        /// </summary>
        void RedirigirACancelarContrato();
        /// <summary>
        /// Guarda un paquete en la variable de session
        /// </summary>
        /// <param name="key">Clave para identificar el paquete</param>
        /// <param name="value">Paquete que se desea guardar en session</param>
        void EstablecerPaqueteNavegacion(string key, object value);
        /// <summary>
        /// Guarda un paquete en la variable de session
        /// </summary>
        /// <param name="key">Clave para identificar el paquete</param>
        /// <param name="value">Paqquete que se desea guardar en session</param>
        void EstablecerPaqueteNavegacionCancelar(string key, object value);
        /// <summary>
        /// Limpia el paquete de navegación pára el registro del check list
        /// </summary>
        void LimpiarPaqueteNavegacion();
        /// <summary>
        /// Obtiene el paquete de navegación
        /// </summary>
        /// <returns>Contrato al cual le asignaremos el Check List</returns>
        object ObtenerPaqueteContrato();
        /// <summary>
        /// Obtiene un paquete guardado en la session
        /// </summary>
        /// <returns>Objeto guardado en la session</returns>
        object ObtenerPaqueteLineaContrato();
        /// <summary>
        /// habilita o deshabilita la opción de regresar
        /// </summary>
        /// <param name="habilitar">Estado</param>
        void PermitirRegresar(bool habilitar);
        /// <summary>
        /// Habilita o deshabilita la opción de continuar
        /// </summary>
        /// <param name="habilitar">Estado</param>
        void PermitirContinuar(bool habilitar);
        /// <summary>
        /// Habilita o deshabilita la opción de cancelar
        /// </summary>
        /// <param name="habilitar">Estado</param>
        void PermitirCancelar(bool habilitar);
        /// <summary>
        /// Habilita o deshabilita la opcion de terminar el registro
        /// </summary>
        /// <param name="habilitar">Estado</param>
        void PermitirGuardarTerminada(bool habilitar);
        /// <summary>
        /// Hace visible o invisible la opción continuar
        /// </summary>
        /// <param name="ocultar">Estado</param>
        void OcultarContinuar(bool ocultar);
        /// <summary>
        /// Hace visible o invisible la opción terminar
        /// </summary>
        /// <param name="ocultar">Estado</param>
        void OcultarTerminar(bool ocultar);
        /// <summary>
        /// Establece el número de página que se esta visualizando
        /// </summary>
        /// <param name="pagina">Número de página</param>
        void EstablecerPagina(int pagina);

        /// <summary>
        /// Obtiene o establece el tipo de archivo
        /// </summary>
        object TiposArchivoImagen { get; set; }

        void EstablecerPuntosVerificacionCheckList();

        void EstablecerAcciones(ETipoEmpresa tipoEmpresa);
        #endregion
    }
}