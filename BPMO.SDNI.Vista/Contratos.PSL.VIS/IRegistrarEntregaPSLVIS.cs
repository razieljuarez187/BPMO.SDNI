using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IRegistrarEntregaPSLVIS {
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
        /// Asigna el valor del Módulo
        /// </summary>
        int? ModuloID { get; }

        /// <summary>
        /// Obtiene o establece el nombre del usuario que realiza el check list
        /// </summary>
        string NombreUsuarioEntrega { get; set; }

        /// <summary>
        /// Nombre del Cliente para el cual se genera el check list
        /// </summary>
        string NombreCliente { get; set; }
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
        /// Obtiene o establece el combustible de entrega
        /// </summary>
        int? Combustible { get; set; }
        /// <summary>
        /// Obtiene o establece el Horómetro de salida de la unidad
        /// </summary>
        int? HorometroSalida { get; set; }
        /// <summary>
        /// Obtiene o Asigna el ultimo valor del Horometro de Entrega de la Unidad
        /// </summary>
        Int32? HorometroAnterior { get; set; }
        /// <summary>
        /// obtiene o establece si el check list es de etrega o recepción
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
        /// Obtiene o establece l tipo de check list que se va a registrar
        /// </summary>
        int? TipoListadoVerificacionPSL { get; set; }

        /// <summary>
        /// Obtiene o establece l tipo de contrato que se va a registrar
        /// </summary>
        int? TipoContrato { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha del contrato
        /// </summary>
        DateTime? FechaContrato { get; set; }
        /// <summary>
        /// Obtiene o establece la hora del contrato
        /// </summary>
        TimeSpan? HoraContrato { get; set; }

        /// <summary>
        /// Obtiene o establece el número de licencia del operador
        /// </summary>
        string NumeroLicencia { get; set; }

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
        /// Capacidad del tanque de la unidad que será entregada
        /// </summary>
        decimal? CapacidadTanque { get; set; }

        /// <summary>
        /// Observaciones generales del check list de entrega
        /// </summary>
        string Observaciones { get; set; }

        /// <summary>
        /// Lista de archivos adjuntos al contrato
        /// </summary>
        List<ArchivoBO> Adjuntos { get; set; }
        /// <summary>
        /// Lista de tipo de archivos
        /// </summary>
        List<TipoArchivoBO> TiposArchivo { get; set; }

        /// <summary>
        /// Obtiene en área de la unidad
        /// </summary>
        EArea? Area { get; set; }

        bool? EsIntercambio { get; set; }
        /// <summary>
        /// Último contrato que se consultó
        /// </summary>
        object UltimoObjeto { get; set; }

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
        /// <param name="error">Mensaje que será mostrado como error</param>
        void RedirigirAImprimir(string error);

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
        /// Obtiene el paquete de navegación
        /// </summary>
        /// <returns>Línea al cual le asignaremos el Check List</returns>
        object ObtenerPaqueteLineaContrato();

        /// <summary>
        /// Limpia la variable de sessión correpsondiente al paquete de navegación
        /// </summary>
        void LimpiarPaqueteNavegacion();

        /// <summary>
        /// Limpia la variable de session correspondiente a la ventana de intercambio de unidad
        /// </summary>
        void LimpiarVariableIntercambio();

        void BloquearRegistro();

        void EstablecerPuntosVerificacionCheckList();

        void EstablecerAcciones(ETipoEmpresa tipoEmpresa);
        #endregion
    }
}