//Satisface el caso de uso CU009 – Configuración Notificación de facturación

using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Facturacion.MonitoreoPagos.BO;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Facturacion.MonitoreoPagos.VIS
{
    /// <summary>
    /// Interface que define las acciones que debe de contener una vista que ejecuta la funcionalidad de consultar Notificación para un empleado
    /// </summary>
    public interface IConsultarConfigurarAlertaVIS
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
        /// Obtiene o establece valor que representa el nombre completo del empleado a quien se le asigna la configuración
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String EmpleadoNombre { get; set; }        

        /// <summary>
        /// Obtiene o establece un valor que representa el estatus de la configuración
        /// </summary>
        /// <value>Objeto de tipo Nullable de Boolean</value>
        bool? Estatus { get; set; }

        /// <summary>
        /// Obtiene un valor que representa el último resultado de la consulta de registros solicitado
        /// </summary>
        /// <value>Objeto de tipo List de ConfiguracionAlertaBO</value>
        List<ConfiguracionAlertaBO> Resultado { get; }      
        #endregion

        #region Métodos
        /// <summary>
        /// Realiza la validación de los datos capturados para detectar errores o inconsistencias
        /// </summary>
        /// <returns>Objeto de tipo String que contiene el error detectado, en caso contrario devolverá nulo</returns>
        String ValidarCampos();

        /// <summary>
        /// Asigna o guarda el resultado obtenido de una consulta solicitada
        /// </summary>
        /// <param name="resultado">Lista de BO's obtenidos de la consulta</param>
        void EstablecerResultado(List<ConfiguracionAlertaBO> resultado);

        /// <summary>
        /// Establece un paquete de navegación en el visor dentro de la sesión en curso
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        /// <param name="value">Valor a asignar dentro del paquete de navegación</param>
        void EstablecerPaqueteNavegacion(string key, object value);

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
        /// Asigna el estatus correspondiente para permitir o deneger el registro de una nueva notificación para un empleado
        /// </summary>
        /// <param name="permitir">bandera que define la operación a realizar, true para permitir, false para denegar</param>
        void PermitirRegistrar(bool permitir);

        /// <summary>
        /// Asigna el estatus correspondiente para permitir o denegar la selección del empleado a asignar
        /// </summary>
        /// <param name="permitir">bandera que define la operación a realizar, true para permitir, false para denegar</param>
        void PermitirSeleccionarEmpleado(bool permitir);

        /// <summary>
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        void RedirigirSinPermisoAcceso();

        /// <summary>
        /// Despliega el Detalle del registro Seleccionado
        /// </summary>
        void RedirigirADetalle();

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
        #endregion
    }
}
