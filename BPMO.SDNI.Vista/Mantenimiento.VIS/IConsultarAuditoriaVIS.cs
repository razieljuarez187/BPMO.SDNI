//Satisface al CU072 - Obtener Auditoría
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Servicio.Catalogos.BO;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Mantenimiento.VIS {

    /// <summary>
    /// Interface que define las acciones que debe de contener una vista que ejecuta la funcionalidad de filtrado y selección para auditorías de Mantenimientos Idealease.
    /// </summary>
    public interface IConsultarAuditoriaVIS {

        #region Propiedades

            #region Seguridad

        /// <summary>
        /// Obtiene un valor que representa el Identificador del Usuario Actual de la sesión en curso.
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

        /// <summary>
        /// Obtiene o establece un valor que representa la Auditoría de Mantenimiento Idealease seleccionada.
        /// </summary>
        AuditoriaMantenimientoBO AuditoriaSeleccionada { get; set; }

            #region Form Búsqueda

                #region Filtro Sucursal

        /// <summary>
        /// Obtiene o establece un valor que representa la Sucursal seleccionada.
        /// </summary>
        SucursalBO SucursalSeleccionada { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de la Sucursal seleccionada.
        /// </summary>
        int? SucursalID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre de la Sucursal a buscar.
        /// </summary>
        String NombreSucursal { get; set; }
                
                #endregion

                #region Filtro Técnico

        /// <summary>
        /// Obtiene o establece un valor que representa un Técnico seleccionado.
        /// </summary>
        TecnicoBO TecnicoSeleccionado { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre del Técnico a buscar.
        /// </summary>
        string NombreTecnico { get; set; }

                #endregion

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de la Orden de Servicio a buscar.
        /// </summary>
        int? OrdenServicioID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el tipo de Paquete de Mantenimiento a buscar.
        /// </summary>
        ETipoMantenimiento? TipoMantenimiento { get; set; }

            #endregion

            #region Grid Resultado Búsqueda

        /// <summary>
        /// Obtiene o establece un valor que represetan una lista de Auditorías de Mantenimientos Idealease encontradas.
        /// </summary>
        List<AuditoriaMantenimientoBO> Auditorias { get; set; }

            #endregion

        #endregion

        #region Métodos

            #region Seguridad

        /// <summary>
        /// Realiza la redirección a la página de advertencia por falta de permisos.
        /// </summary>
        void RedirigirSinPermisoAcceso();
            
            #endregion

        /// <summary>
        /// Este método despliega un mensaje en pantalla.
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar.</param>
        /// <param name="tipo">Tipo de mensaje a desplegar.</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar.</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

            #region Grid Resultado Búsqueda
        
        /// <summary>
        /// Método que despliega la lista de Auditorías de Mantenimientos Idealease encontradas.
        /// </summary>
        void DesplegarListaAuditorias();

            #endregion

        #endregion

        
    }
}
