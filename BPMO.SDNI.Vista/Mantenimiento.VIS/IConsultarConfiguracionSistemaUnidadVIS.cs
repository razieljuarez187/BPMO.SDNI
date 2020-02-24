// Satisface al CU073 - Catálogo Configuración Sistemas Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.BO;

namespace BPMO.SDNI.Mantenimiento.VIS {

    /// <summary>
    /// Interface que define las acciones que debe de contener una vista que ejecuta la funcionalidad de filtrado y selección de
    /// Configuración Sistemas Unidad Idealease.
    /// </summary>
    public interface IConsultarConfiguracionSistemaUnidadVIS {

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
        /// Obtiene o establece un valor que representa la Configuración de la Unidad Operativa que indica a que libro corresponden los activos.
        /// </summary>
        string LibroActivos { get; set; }

        /// <summary>
        /// Obtiene un valor que representa el Identificador del Módulo desde el cual se está accesando.
        /// </summary>
        int? ModuloID { get; }

            #endregion

            #region Form Búsqueda

        /// <summary>
        /// Obtiene o establece un valor que representa el Nombre de la Configuración Sistema de Unidad Idealease.
        /// </summary
        string Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la Clave de la Configuración Sistema de Unidad Idealease.
        /// </summary
        string Clave { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el Estado de la Configuración Sistema de Unidad Idealease.
        /// </summary>
        bool? Activo { get; }

            #endregion

            #region Grid Resultado Búsqueda

        /// <summary>
        /// Obtiene o establece un valor que representa la Lista de Configuraciones de Sistemas de Unidad Idealease.
        /// </summary>
        List<ConfiguracionSistemaUnidadBO> Configuraciones { get; set; }

            #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Realiza la redirección a la página de advertencia por falta de permisos.
        /// </summary>
        void RedirigirSinPermisoAcceso();

        /// <summary>
        /// Despliega un mensaje en pantalla.
        /// </summary>
        /// <param name="mensaje">Mensaje a desplega.r</param>
        /// <param name="tipo">Tipo de mensaje a desplegar.</param>
        /// <param name="detalleMensaje">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar.</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalleMensaje = null);

            #region Grid Resultado Búsqueda

        /// <summary>
        /// Realiza la vinculación de la lista de Configuraciones Sistema de Unidad Idealease encontradas con la UI.
        /// </summary>
        void DesplegarListaConfiguraciones();

            #endregion

        #endregion
    }
}
