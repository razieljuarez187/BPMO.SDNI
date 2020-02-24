//Satisface el caso de uso CU009 – Configuración Notificación de facturación
//Satisface a la solicitud de cambios SC0008

using System;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Facturacion.MonitoreoPagos.VIS
{
    /// <summary>
    /// Interface que define las acciones que debe de contener una vista que visualiza a detalle los datos de un registro
    /// </summary>
    public interface IucConfigurarAlertaVIS
    {
        #region Propiedades
        /// <summary>
        /// Obtiene un valor que representa el identificador de la unidad operativa actual de la sesión en curso
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? UnidadOperativaID { get; set; }

        /// <summary>
        /// Obtiene un valor que representa el identificador del usuario actual de la sesión en curso
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? UsuarioID { get; }

        #region SC0008
        /// <summary>
        /// Obtiene un valor que representa el perfil al que esta asociado la configuración
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? PerfilID { get; set; }
        #endregion

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

        /// <summary>
        /// Obtiene o establece un valor que representa el estatus de la configuración
        /// </summary>
        /// <value>Objeto de tipo Nullable de Boolean</value>
        bool? Estatus { get; set; }

        #endregion

        #region Metodos
        /// <summary>
        /// Realiza el proceso de inicializar el visor para capturar un nuevo registro
        /// </summary>
        void PrepararNuevo();

        /// <summary>
        /// Realiza el proceso de inicializar el visor para editar un registro existente
        /// </summary>
        void PrepararEdicion();

        /// <summary>
        /// Realiza el proceso de inicializar el visor para mostrar los datos de un registro
        /// </summary>
        void PrepararVisualizacion();

        /// <summary>
        /// Asigna el estatus correspondiente para permitir o denegar la selección del empleado a asignar
        /// </summary>
        /// <param name="permitir">bandera que define la operación a realizar, true para permitir, false para denegar</param>
        void PermitirSeleccionarEmpleado(bool permitir);

        /// <summary>
        /// Libera o elimina los valores en sesión guardados en el visor
        /// </summary>
        void LimpiarSesion();

        /// <summary>
        /// Realiza la validación de los datos capturados para detectar errores o inconsistencias
        /// </summary>
        /// <returns>Objeto de tipo String que contiene el error detectado, en caso contrario devolverá nulo</returns>
        String ValidarCampos();

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        #endregion
    }
}
