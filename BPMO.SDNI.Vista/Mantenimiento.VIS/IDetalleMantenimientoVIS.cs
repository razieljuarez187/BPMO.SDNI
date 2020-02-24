// Satisface al CU062 – Obtener Orden de Servicio Idealease
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.BO;
using System.Data;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Mantenimiento.VIS {

    /// <summary>
    /// Interface que define las acciones que debe de contener una vista que ejecuta la funcionalidad de filtrado y selección para Ordenes de Servicio Idealease
    /// </summary>
    public interface IDetalleMantenimientoVIS {
        
        #region Propiedades

            #region Seguridad

        /// <summary>
        /// Obtiene un valor que representa el identificador del Usuario Actual de la sesión en curso.
        /// </summary>
        int? UsuarioAutenticado { get; }

        /// <summary>
        /// Obtiene un valor que representa el Identificador de la Unidad Operativa Actual de la sesión en curso.
        /// </summary>
        int? UnidadOperativaId { get; }

        /// <summary>
        /// Obtiene un valor que representa el Identificador del Módulo desde el cual se está accesando.
        /// </summary>
        int? ModuloID { get; }

        /// <summary>
        /// Obtiene o establece un valor que representa la Configuración de la Unidad Operativa que indica a que libro corresponden los activos.
        /// </summary>
        string LibroActivos { get; set; }

            #endregion

        /// <summary>
        /// Obtiene o establece un valor que representa el indice de la Orden de Servicio Idealease seleccionada
        /// </summary>
        int Index { get; set; }
        
        /// <summary>
        /// Obtiene o establece un valor que representa el ID de la Orden de Servicio a buscar
        /// </summary>
        int? FolioOrdenServicio { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el tipo de Orden de Servicio Idealease encontrada
        /// </summary>
        bool EsUnidad { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa un diccionario de datos de la Orden de Servicio Idealease seleccionada.
        /// </summary>
        Dictionary<string, string> MantenimientoToHash { get; set; }
        
        /// <summary>
        /// Obtiene o establece un listado de Ordenes de Servicio Idealease para Unidades Idealease.
        /// </summary>
        List<MantenimientoUnidadBO> MantenimientosUnidad { get; set;}

        /// <summary>
        /// Obtiene o establece un listado de Ordenes de Servicio Idealease para Equipos Aliados Idealease.
        /// </summary>
        List<MantenimientoEquipoAliadoBO> MantenimientosEquipoAliado { get; set;}

        /// <summary>
        /// Obtiene o establece una tabla de datos de los Equipos Aliados Idealease de la Unidad Idealease seleccionada.
        /// </summary>
        DataSet EquiposAliadosMantenimiento { get; set; }

        #endregion

        #region Métodos

        /// <summary>
        /// Realiza la redirección a la página de advertencia por falta de permisos.
        /// </summary>
        void RedirigirSinPermisoAcceso();

        /// <summary>
        /// Este método despliega un mensaje en pantalla.
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        
        #endregion
        
    }
}
