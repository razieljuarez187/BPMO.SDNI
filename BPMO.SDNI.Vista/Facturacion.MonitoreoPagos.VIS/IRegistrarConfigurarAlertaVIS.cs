//Satisface el caso de uso CU009 – Configuración Notificación de facturación

using System;

using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Facturacion.MonitoreoPagos.VIS
{
    /// <summary>
    /// Interface que define las acciones que debe de contener una vista que ejecuta la funcionalidad de registra un nuevo registro
    /// </summary>
    public interface IRegistrarConfigurarAlertaVIS
    {
        #region Propiedades
        /// <summary>
        /// Obtiene un valor que representa el identificador de la unidad operativa actual de la sesión en curso
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? UnidadOperativaID { get; }

        /// <summary>
        /// Obtiene un valor que representa el identificador del usuario actual de la sesión en curso
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? UsuarioID { get; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de la configuración
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? ConfiguracionAlertaID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de sucursal para las que aplica la configuración
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? SucursalID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre corto de 
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String ClaveSucursal { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre de la sucursal para las que aplica la configuración
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String SucursalNombre { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del empleado a quien se le asigna la configuración
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? EmpleadoID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre completo del empleado a quien se le asigna la configuración
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String EmpleadoNombre { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la cuenta de correo electrónico que tiene actualmente el empleado donde se enviarán las notificaciones
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String CorreoElectronico { get; set; }

        /// <summary>
        /// Obtiene o establece el número de días para recibir la notificación
        /// </summary>
        /// <value>Objeto de tipo Int16</value>
        short? NumeroDias { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el estatus de la configuración
        /// </summary>
        /// <value>Objeto de tipo Nullable de Boolean</value>
        bool? Estatus { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha de creación del registro
        /// </summary>
        /// <value>Objeto de tipo Nullable de DateTime</value>
        DateTime? FC { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha de última actualización del registros
        /// </summary>
        /// <value>Objeto de tipo Nullable de DateTime</value>
        DateTime? FUA { get; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del usuario que creo el registro
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? UC { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del usuario que actualizó por última vez el registro
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? UUA { get; }
               
        #endregion

        #region Metodos
        /// <summary>
        /// Realiza el proceso de inicializar el visor para capturar un nuevo registro
        /// </summary>
        void PrepararNuevo();

        /// <summary>
        /// Establece un paquete de navegación en el visor dentro de la sesión en curso
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        /// <param name="value">Valor a asignar dentro del paquete de navegación</param>
        void EstablecerPaqueteNavegacion(string nombre, object valor);

        /// <summary>
        /// Obtiene el valor de un paquete de navegación ya guardado previamente
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        /// <returns>Valor de tipo objet dentro del paquete de navegación</returns>
        object ObtenerPaqueteNavegacion(string key);

        /// <summary>
        /// Elimina un paquete de navegación ya guardado previamente
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        void LimpiarPaqueteNavegacion(string key);

        /// <summary>
        /// Realiza la dirección al visor correspondiente para realiza una nueva consulta de registros
        /// </summary>
        void RedirigirAConsulta();

        /// <summary>
        /// Despliega el Detalle del registro Seleccionado
        /// </summary>
        void RedirigirADetalles();

        /// <summary>
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        void RedirigirSinPermisoAcceso();

        /// <summary>
        /// Libera o elimina los valores en sesión guardados en el visor
        /// </summary>
        void LimpiarSesion();

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        #endregion Metodos
    }
}
