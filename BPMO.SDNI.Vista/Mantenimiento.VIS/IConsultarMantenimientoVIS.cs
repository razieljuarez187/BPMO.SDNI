// Satisface al CU062 - Obtener Orden de Servicio Idealease
using System;
using System.Collections.Generic;
using BPMO.SDNI.Equipos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.BO;
using System.Data;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.BO.BOF;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Mantenimiento.VIS {

    /// <summary>
    /// Interface que define las acciones que debe de contener una vista que ejecuta la funcionalidad de filtrado y seleccion para Mantenimientos Idealease. 
    /// </summary>
    public interface IConsultarMantenimientoVIS {
        
        #region Propiedades

        /// <summary>
        /// Obtiene o establece un valor que representa un bit para determinar si es un Mantenimiento Unidad o un Mantenimiento Equipo Aliado.
        /// </summary>
        bool EsUnidad { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa un diccionario de datos de la Orden de Servicio Idealease seleccionada.
        /// </summary>
        Dictionary<string, string> MantenimientoToHash { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el Mantenimiento Unidad Idealease seleccionada.
        /// </summary>
        MantenimientoUnidadBO MantenimientoUnidadEncontrada { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la Unidad Idealease seleccionada.
        /// </summary>
        BPMO.SDNI.Equipos.BO.UnidadBO UnidadEncontrada { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el Mantenimiento Equipo Aliado Idealease seleccionado.
        /// </summary>
        MantenimientoEquipoAliadoBO MantenimientoEquipoAliadoEncontrado { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el Equipo Aliado seleccionado.
        /// </summary>
        EquipoAliadoBO EquipoAliadoEncontrado { get; set; }

            #region Seguridad

        /// <summary>
        /// Obtiene un valor que representa el Identificador del Usuario Actual de la sesión en curso.
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

            #region From Búsqueda

                #region Filtro Unidad

        /// <summary>
        /// Obtiene o establece un valor que representa el Número de Serie del Equipo seleccionado.
        /// </summary>
        String NumeroVIN { get; set; }

                #endregion

                #region Filtro Modelo

        /// <summary>
        /// Obtiene o establece un valor que representa el Modelo seleccionado.
        /// </summary>
        ModeloBO ModeloSeleccionado { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el Nombre del Modelo seleccionado.
        /// </summary>
        String ModeloNombre { get; set; }

                #endregion

        /// <summary>
        /// Obtiene o establece un valor que representa el Número Económico de la Unindad seleccionada.
        /// </summary>
        String NumeroEconomico { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el Folio de la Orden de Servicio E-Lider.
        /// </summary>
        int? FolioOrdenServicio { get; set; }

            #endregion

            #region Grid Resultado Búsqueda

        /// <summary>
        /// Obtiene o establece un valor que representa el índice del Mantenimiento Idealease seleccionado.
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el listado de Mantenimientos Unidades Idealease encontradas.
        /// </summary>
        List<MantenimientoUnidadBO> MantenimientosUnidad { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el listado de Mantenimientos Equipos Aliados Idealease encontradas.
        /// </summary>
        List<MantenimientoEquipoAliadoBO> MantenimientosEquipoAliado { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa un diccionario de datos de los Mantenimientos Equipos Aliados Idealease del Mantenimiento Unidad Idealease.
        /// </summary>
        DataSet EquiposAliadosMantenimiento { get; set; }

            #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Despliega un mensaje en pantalla.
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar.</param>
        /// <param name="tipo">Tipo de mensaje a desplegar.</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar.</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

            #region Seguridad

        /// <summary>
        /// Realiza la redirección al visor del usuario a la página de advertencia por falta de permisos.
        /// </summary>
        void RedirigirSinPermisoAcceso();
        
            #endregion

            #region Form Búsqueda

        /// <summary>
        /// Prepara la Vista para una nueva búsqueda.
        /// </summary>
        void PrepararBusqueda();

        /// <summary>
        /// Establece una instancia del diccionario de datos del Mantenimiento seleccionado, la lista de Mantenimientos Idealease encontrados y la 
        /// Lista de Mantenimientos Equipos Aliados Idealease del Mantenimiento Unidad Idealease.
        /// </summary>
        void LimpiarSesion();

        /// <summary>
        /// Despliega el Número de Serie, Número Económico y el Modelo del Equipo E-Lider.
        /// </summary>
        void CargarDatosMantenimientoEncontrado();

        /// <summary>
        /// Construye el diccionario de datos de los Mantenimientos Idealease encontrados.
        /// </summary>
        void CargarListaMantenimientos();
        
            #endregion

        #endregion

    }
}
