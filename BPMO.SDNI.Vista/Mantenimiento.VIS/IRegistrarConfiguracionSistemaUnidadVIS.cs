// Satisface al CU073 - Catálogo Configuración Sistemas Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Mantenimiento.VIS {

    /// <summary>
    /// Interface que define las acciones que debe de contener una vista que ejecuta la funcionalidad de Registro para 
    /// Configuraciones de Sistemas de Unidad Idealease.
    /// </summary>
    public interface IRegistrarConfiguracionSistemaUnidadVIS {

        #region Atributos

            #region Seguridad

        /// <summary>
        /// Obtiene un valor que representa el identificador del Usuario Actual de la sesión en curso.
        /// </summary>
        int? UsuarioAutenticado { get; }

        /// <summary>
        /// Obtiene un valor que representa el Identificador de la Unidad Operativa Actual de la sesión en curso.
        /// </summary>
        int? UnidadOperativaID { get; }

        /// <summary>
        /// Obtiene un valor que representa el Identificador del Módulo desde el cual se está accesando.
        /// </summary>
        int? ModuloID { get; }

        /// <summary>
        /// Obtiene o establece un valor que representa la Configuración de la Unidad Operativa que indica a que libro corresponden los activos.
        /// </summary>
        string LibroActivos { get; set; }

            #endregion

            #region Grid Configuraciones

        /// <summary>
        /// Obtiene o establece un valor que representa la Lista de Configuraciones Sistemas de Unidad Idealease a registrar.
        /// </summary>
        List<ConfiguracionSistemaUnidadBO> Configuraciones { get; set; }

            #endregion

        #endregion

        #region Métodos

            #region Grid Configuraciones

        /// <summary>
        /// Realiza la vinculación de la lista de Configuraciones Sistema de Unidad Idealease agregadas.
        /// </summary>
        void CargarConfiguraciones();

            #endregion

        /// <summary>
        /// Bloquea los Campos Clave y Nombre de la Configuración Sistema de Unidad Idealease.
        /// </summary>
        void BloquearCampos();

        /// <summary>
        /// Realiza la redirección a la página de advertencia por falta de permisos.
        /// </summary>
        void RedirigirSinPermisoAcceso();

        /// <summary>
        /// Despliega un mensaje en pantalla.
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar.</param>
        /// <param name="tipo">Tipo de mensaje a desplegar.</param>
        /// <param name="cuerpoMensaje">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar.</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string cuerpoMensaje = null);

        #endregion
    }
}
