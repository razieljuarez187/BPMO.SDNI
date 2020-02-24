// Satisface al CU027 - Catálogo Contacto Cliente
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.BO;

namespace BPMO.SDNI.Mantenimiento.VIS
{
    /// <summary>
    /// Interface que define las acciones que debe de contener una vista que ejecuta la funcionalidad de Edición del 
    /// Contacto Cliente Idealease.
    /// </summary>
    public interface IEditarContactoClienteVIS {

        #region Atributos

        /// <summary>
        /// Obtiene o establece un valor que representa al Contacto Cliente Idealease seleccionado
        /// </summary>
        ContactoClienteBO ContactoClienteSeleccionado { get; set; }

        #region Seguridad

        /// <summary>
        /// Obtiene un valor que representa el Identificador del Módulo desde el cual se está accesando.
        /// </summary>
        int? ModuloID { get; }

        /// <summary>
        /// Obtiene un valor que representa el Identificador de la Unidad Operativa Actual de la sesión en curso.
        /// </summary>
        int? UnidadOperativaID { get; }

        /// <summary>
        /// Obtiene o establece un valor que representa la Configuración de la Unidad Operativa que indica a que libro corresponden los activos.
        /// </summary>
        string LibroActivos { get; set; }

        /// <summary>
        /// Obtiene un valor que representa el identificador del Usuario Actual de la sesión en curso.
        /// </summary>
        int? UsuarioAutenticado { get; }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Reestablece los valores de Sesión establecidos.
        /// </summary>
        void LimpiarDatosSesion();

        /// <summary>
        /// Realiza la redirección a la página de advertencia por falta de permisos.
        /// </summary>
        void RedirigirSinPermisoAcceso();

        /// <summary>
        /// Realiza la redirección al visor de Eliminar Contacto Cliente Idealease.
        /// </summary>
        void RedirigirAEliminarContactoCliente();

        /// <summary>
        /// Realiza la redirección al visor de Edición Contacto Cliente Idealease.
        /// </summary>
        void RedirigirAEditarContactoCliente();

        /// <summary>
        /// Realiza la redirección al visor de Detalle Contacto Cliente Idealease.
        /// </summary>
        void RedirigirADetalleContactoCliente();

        /// <summary>
        /// Estable la variable de Sesión ContactoClienteGuardado como True si la Acción se realizó con éxito.
        /// </summary>
        void setGuardadoExitoso();

        /// <summary>
        /// Despliega un mensaje en pantalla.
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar.</param>
        /// <param name="tipo">Tipo de mensaje a desplegar.</param>
        /// <param name="msjDetalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar.</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        #endregion
    }
}
