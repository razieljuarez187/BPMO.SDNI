//Satisface al CU007 - Registrar entrega recepción de unidad
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    /// <summary>
    /// Vista para registrar entrega de unidad
    /// </summary>
    public interface IRegistrarEntregaVIS
    {
        #region Propiedades
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
        string NombreUsuarioEntrega { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador del contrato para el cual se esta registrando el check list
        /// </summary>
        int? ContratoID { get; set; }
        /// <summary>
        /// Obtiene o establece el número del contrato de renta diaría
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
        /// Nombre del operador que conducira la unidad
        /// </summary>
        string NombreOperador { get; set; }
        /// <summary>
        /// Obtiene o establece el número de licencia del operador
        /// </summary>
        string NumeroLicencia { get; set; }
        /// <summary>
        /// Placas de la unida que será entregada
        /// </summary>
        string PlacasFederales { get; set; }
        /// <summary>
        /// Placas de la unida que será entregada
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
        /// Número de serie de la unidad que será entregada
        /// </summary>
        string NumeroSerie { get; set; }
        /// <summary>
        /// Número económico de la unidad que será entregada
        /// </summary>
        string NumeroEconomico { get; set; }
        /// <summary>
        /// Capacidad total del tanque de conbustible de la unidad seleccionada en el contrato
        /// </summary>
        decimal? CapacidadTanque { get; set; }
        /// <summary>
        /// obtiene o establece el tipo de listado que se va registrar
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
        /// obtiene o establece el kilometraje de entrega
        /// </summary>
        int? Kilometraje { get; set; }
        /// <summary>
        /// Obtiene o Asigna el valor del Ultimo Kilometraje de Entrega de la Unidad
        /// </summary>
        Int32? KilometrajeAnterior { get; set; }
        /// <summary>
        /// Obtiene o establece las horas de entrega
        /// </summary>
        int? Horometro { get; set; }
        /// <summary>
        /// Obtiene o Asigna el ultimo valor del Horometro de Entrega de la Unidad
        /// </summary>
        Int32? HorometroAnterior { get; set; }
        /// <summary>
        /// Obtiene o establece el combustible de entrega
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
        /// Obtiene o establece si tiene stereo y bocinas
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
        /// Obtiene o establece si el interior de al caja esta limpio
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
        /// Obtiene o establece el código de la refacción
        /// </summary>
        string RefaccionCodigo { get; set; }
        /// <summary>
        /// Obtiene o establece el estado de la llanta de refacción
        /// </summary>
        bool? RefaccionEstado { get; set; }
        /// <summary>
        /// Obtiene las secciones de unidaes configuradas para el registro de los check list
        /// </summary>
        string SeccionesUnidades { get;}
        /// <summary>
        /// Obtiene el nombre del archivo seleccionado
        /// </summary>
        string NombreArchivoImagen { get; }
        /// <summary>
        /// Onbtiene la extención del archivo seleccionado
        /// </summary>
        string ExtencionArchivoImagen { get; }
        /// <summary>
        /// Obtiene el identificador de al sección seleccionada
        /// </summary>
        int? SeccionUnidadID { get; }
        /// <summary>
        /// Obtiene o establece la observacion a la sección
        /// </summary>
        string ObservacionesSeccion { get; set; }
        /// <summary>
        /// Obtiene o establece lso tipos de archivos de iamgén configurados
        /// </summary>
        object TiposArchivoImagen { get; set; }
        /// <summary>
        /// Indice para controlar las secciones
        /// </summary>
        int IndicePaginaSecciones { get; set; }
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
        /// Prepara la interfaz para un nuevo registro de CheckList
        /// </summary>
        void PrepararNuevo();
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
        /// Guarda un paquete en la variable de session
        /// </summary>
        /// <param name="key">Clave para identificar el paquete</param>
        /// <param name="value">Paquete que se desea guardar en sesion</param>
        void EstablecerPaqueteNavegacion(string key, object value);
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
        /// Obtiene el arreglo de bytes que corresponde a las imagenes
        /// </summary>
        /// <returns>arreglo de byte</returns>
        byte[] ObtenerArchivosBytes();
        /// <summary>
        /// Despliega las imagenes en la vista
        /// </summary>
        void CargarImagenes();
        /// <summary>
        /// Despliega las secciones de verificación en la vista
        /// </summary>
        void CargarSeccionesVerificacion();
        /// <summary>
        /// Limpia los controles de la vista para la captura de sección
        /// </summary>
        void LimpiarDatosSeccionUnidad();
        /// <summary>
        /// Despliega en la vista los nombres de las imagenes en la sección
        /// </summary>
        /// <param name="p">Objeto del cual se desean conocer los detalles</param>
        void CargarDetalleImagenSeccion(object p);
        /// <summary>
        /// Limpia la variable de sessión correpsondiente al paquete de navegación
        /// </summary>
        void LimpiarPaqueteNavegacion();
        /// <summary>
        /// Habilita o deshabilita el control de refacción en la vista
        /// </summary>
        /// <param name="estado">Estado para el control de refacción</param>
        void HabilitarRefaccion(bool estado);
        /// <summary>
        /// Actualiza el resultado en la lista de observaciones de sección
        /// </summary>
        void ActualizarSecciones();

		void BloquearRegistro();
        #endregion        
    }
}