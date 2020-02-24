//Satisface al CU007 - Registrar entrega recepción de unidad
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    /// <summary>
    /// Vista para el registro de recepción de la unidad
    /// </summary>
    public interface IRegistrarRecepcionVIS
    {
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
        /// Nombre del operador que conducirá la unidad
        /// </summary>
        string NombreOperador { get; set; }
        /// <summary>
        /// Obtiene o establece el número de licencia del operador
        /// </summary>
        string NumeroLicencia { get; set; }
        /// <summary>
        /// Placas de la unidad que será entregada
        /// </summary>
        string PlacasFederales { get; set; }
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
        /// Obtiene o establece la fecha en la que se registra el listado
        /// </summary>
        DateTime? FechaListado { get; set; }
        /// <summary>
        /// Obtiene o establece la hora en la que se registra el listado
        /// </summary>
        TimeSpan? HoraListado { get; set; }
        /// <summary>
        /// Obtiene o establece el kilometraje de recepción
        /// </summary>
        int? Kilometraje { get; set; }
        /// <summary>
        /// Obtiene o establece las horas de recepción
        /// </summary>
        int? Horometro { get; set; }
        /// <summary>
        /// Obtiene o establece el combustible de recepción
        /// </summary>
        int? Combustible { get; set; }
        /// <summary>
        /// Obtiene o establece si se tiene golpes en general
        /// </summary>
        bool? TieneGolpesGeneral { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene documentación completa
        /// </summary>
        bool? TieneDocumentacionCompleta { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene interior limpio
        /// </summary>
        bool? TieneInteriorLimpio { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene las vestiduras limpias
        /// </summary>
        bool? TieneVestidurasLimpias { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene tapetes
        /// </summary>
        bool? TieneTapetes { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene llave original
        /// </summary>
        bool? TieneLlaveOriginal { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene encendedor
        /// </summary>
        bool? TieneEncendedor { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene estereo y bocinas
        /// </summary>
        bool? TieneStereoBocinas { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene alarma de reversa
        /// </summary>
        bool? TieneAlarmaReversa { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene extinguidor
        /// </summary>
        bool? TieneExtinguidor { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene gato y llave de tuerca
        /// </summary>
        bool? TieneGatoLlaveTuerca { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene tres reflejantes
        /// </summary>
        bool? TieneTresReflejantes { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene espejos completos
        /// </summary>
        bool? TieneEspejosCompletos { get; set; }
        /// <summary>
        /// Obtiene o establece si el interior de la caja esta limpio
        /// </summary>
        bool? TieneLimpiezaInteriorCaja { get; set; }
        /// <summary>
        /// Obtiene o establece si cuenta con gps
        /// </summary>
        bool? TieneGPS { get; set; }
        /// <summary>
        /// Obtiene o establece si las baterias son correctas
        /// </summary>
        bool? BateriasCorrectas { get; set; }
        /// <summary>
        /// obtiene o establece laa observaciones de la documentación
        /// </summary>
        string ObservacionesDocumentacionCompleta { get; set; }
        /// <summary>
        /// Obtiene o establece las observaciones de las baterias
        /// </summary>
        string ObservacionesBaterias { get; set; }
        /// <summary>
        /// Obtiene o establece las observaciones de las llantas
        /// </summary>
        string ObservacionesLlantas { get; set; }

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
        /// Obtiene o establece el kilometraje de entrega
        /// </summary>
        int? KilometrajeEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece las horas de entrega
        /// </summary>
        int? HorometroEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece el combustible de entrega
        /// </summary>
        int? CombustibleEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene golpes en general
        /// </summary>
        bool? TieneGolpesGeneralEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene documentación completa
        /// </summary>
        bool? TieneDocumentacionCompletaEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene interior limpio
        /// </summary>
        bool? TieneInteriorLimpioEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene las vestiduras limpias
        /// </summary>
        bool? TieneVestidurasLimpiasEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene tapetes
        /// </summary>
        bool? TieneTapetesEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene llave original
        /// </summary>
        bool? TieneLlaveOriginalEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene encendedor
        /// </summary>
        bool? TieneEncendedorEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene estereo y bocinas
        /// </summary>
        bool? TieneStereoBocinasEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene alarma de reversa
        /// </summary>
        bool? TieneAlarmaReversaEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene extinguidor
        /// </summary>
        bool? TieneExtinguidorEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene gato y llave de tuerca
        /// </summary>
        bool? TieneGatoLlaveTuercaEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene tres reflejantes
        /// </summary>
        bool? TieneTresReflejantesEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece si tiene espejos completos
        /// </summary>
        bool? TieneEspejosCompletosEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece si el interior de la caja esta limpio
        /// </summary>
        bool? TieneLimpiezaInteriorCajaEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece si cuenta con gps
        /// </summary>
        bool? TieneGPSEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece si las baterias son correctas
        /// </summary>
        bool? BateriasCorrectasEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece laa observaciones de la documentación
        /// </summary>
        string ObservacionesDocumentacionCompletaEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece las observaciones de las baterias
        /// </summary>
        string ObservacionesBateriasEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece las observaciones de las llantas
        /// </summary>
        string ObservacionesLlantasEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece las secciones de verificacion de entrega
        /// </summary>
        List<object> VerificacionesSeccionEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece las secciones de llantas de entrega
        /// </summary>
        List<object> VerificacionesLlantaEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece el codigo de la refacción
        /// </summary>
        string RefaccionCodigoEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece el estado de la llanta
        /// </summary>
        bool? RefaccionEstadoEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece las imagenes guardadas de las secciones
        /// </summary>
        List<object> ImagenesSecciones { get; set; }
        /// <summary>
        /// Indice para controlar las secciones de Entrega
        /// </summary>
        int IndicePaginaSeccionesEntrega { get; set; }
        #endregion

        /// <summary>
        /// Obtiene o establece las secciones de verificación
        /// </summary>
        List<object> VerificacionesSeccion { get; set; }
        /// <summary>
        /// Obtiene o establece las secciones de llantas
        /// </summary>
        List<object> VerificacionesLlanta { get; set; }
        /// <summary>
        /// Obtiene o establece el listado de las imagenes agregadas a la sección
        /// </summary>
        object NuevosArchivos { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador de la refacción
        /// </summary>
        int? RefaccionID { get; set; }
        /// <summary>
        /// Obtiene o establece el codigo de la refaccion
        /// </summary>
        string RefaccionCodigo { get; set; }
        /// <summary>
        /// Obtiene o establece el estado de la llanta
        /// </summary>
        bool? RefaccionEstado { get; set; }
        /// <summary>
        /// Obtiene las secciones de unidaes configuradas para el registro de los check list
        /// </summary>
        string SeccionesUnidades { get; }
        /// <summary>
        /// Obtiene el nombre del archivo seleccionado
        /// </summary>
        string NombreArchivoImagen { get; }
        /// <summary>
        /// Obtiene la extención del archivo seleccionado
        /// </summary>
        string ExtencionArchivoImagen { get; }
        /// <summary>
        /// Obtiene la sección de la unidad seleccionada
        /// </summary>
        int? SeccionUnidadID { get; }
        /// <summary>
        /// Obtiene o establece la observación de la sección de la unidad
        /// </summary>
        string ObservacionesSeccion { get; set; }
        /// <summary>
        /// Obtiene o establece el tipo de archivo
        /// </summary>
        object TiposArchivoImagen { get; set; }
        /// <summary>
        /// Indice para controlar las secciones
        /// </summary>
        int IndicePaginaSecciones { get; set; }
        /// <summary>
        /// Indice para controlar las imagenes en las secciones
        /// </summary>
        int IndicePaginaImagenesSecciones { get; set; }
        /// <summary>
        /// Indica la tolerancia en kilometros para la recepción
        /// </summary>
        int? KilometrajeDiario { get; set; }
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
        void PrepararCancelacion(bool estado);
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
        void RedirigirAImprimir();
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
        /// Carga en pantalla las llantas asociadas a la unidad
        /// </summary>
        /// <param name="llantas">Llantas de la unidad</param>
        void CargarLlantas(object unidad);
        /// <summary>
        /// Establece las opciones validas para las secciones de las uniades
        /// </summary>
        /// <param name="opciones">Opciones que serán desplegadas</param>
        void EstablecerOpcionesSeccion(Dictionary<int, string> opciones);
        /// <summary>
        /// Obtiene las verificaciones a las llantas
        /// </summary>
        /// <returns>Listado con las verificaciones de llanta</returns>
        object ObtenerValoresLlanta();
        /// <summary>
        /// Obtiene el arreglo de bytes que corresponde al archivo seleccionado
        /// </summary>
        /// <returns>arreglo de bytes</returns>
        byte[] ObtenerArchivosBytes();
        /// <summary>
        /// Despliega las imagenes de las observaciones de las secciones de unidad
        /// </summary>
        void CargarImagenes();
        /// <summary>
        /// Limpia el control uploadfile
        /// </summary>
        void LimpiarUploadFile();
        /// <summary>
        /// Despliega las secciones de verificación
        /// </summary>
        void CargarSeccionesVerificacion();
        /// <summary>
        /// Limpia los controles correspondientes a las observaciones de la unidad
        /// </summary>
        void LimpiarDatosSeccionUnidad();
        /// <summary>
        /// Establece el número de página que se esta visualizando
        /// </summary>
        /// <param name="pagina">Número de página</param>
        void EstablecerPagina(int pagina);
        /// <summary>
        /// Despliega las observaciones de las llantas al momento de la entrega
        /// </summary>
        void CargarLlantasEntrega();
        /// <summary>
        /// Despliega las observaciones a las secciones de unidad al momento de la entrega
        /// </summary>
        void CargarSeccionesEntrega();
        /// <summary>
        /// Despliega en la vista los nombres de las imagenes en la sección
        /// </summary>
        /// <param name="p">Objeto del cual se desean conocer los detalles</param>
        void CargarDetalleImagenSeccion(object p);
        /// <summary>
        /// Despliega las imagenes de la unidad registradas al momento de la entrega
        /// </summary>
        void CargarImagenesSecciones();
        /// <summary>
        /// Habilita o deshabilita el control de refacción en la vista
        /// </summary>
        /// <param name="estado">Estado para el control de refacción</param>
        void HabilitarRefaccion(bool estado);
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
        /// Actualiza el resultado en la lista de observaciones de sección
        /// </summary>
        void ActualizarSecciones();
        /// <summary>
        /// Actualiza el resultado en la lista de observaciones de sección de entrega
        /// </summary>
        void ActualizarSeccionesEntrega();
        /// <summary>
        /// Actualiza el resultado en la lista de las iamgenes para las seciones
        /// </summary>
        void ActualizarImagenesSecciones();
        #endregion
    }
}