// Satisface al CU075 - Reporte Comparativo Mantenimiento
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Servicio.Catalogos.BO;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Mantenimiento.Reportes.VIS {

    /// <summary>
    /// Interface que define las acciones que debe de contener una vista que ejecuta la funcionalidad de Consulta 
    /// para el Reporte Comparativo de Mantenimientos.
    /// </summary>
    public interface IComparativoMantenimientoVIS {

        #region Atributos

            #region Seguridad

        /// <summary>
        /// Obtiene un valor que representa el Identificador del Módulo desde el cual se está accesando.
        /// </summary>
        int? ModuloId { get; }

        /// <summary>
        /// Obtiene un valor que representa el Identificador de la Unidad Operativa Actual de la sesión en curso.
        /// </summary>
        int? UnidadOperativaId { get; }

        /// <summary>
        /// Obtiene un valor que representa el identificador del Usuario Actual de la sesión en curso.
        /// </summary>
        int? UsuarioAutenticado { get; }

        /// <summary>
        /// Obtiene o establece un valor que representa la Configuración de la Unidad Operativa que indica a que libro corresponden los activos.
        /// </summary>
        string LibroActivos { get; set; }

            #endregion

            #region Form Búsqueda

                #region Filtro Sucursal

        /// <summary>
        /// Obtiene o estable el Identificador de la Sucursal seleccionada.
        /// </summary>
        int? SucursalId { get; set; }

        /// <summary>
        /// Obtiene o estable el Nombre de la Sucursal a buscar o la seleccionada.
        /// </summary>
        string NombreSucursal { get; set; }

                #endregion

                #region Filtro Modelo

        /// <summary>
        /// Obtiene o estable el Identificador del Modelo seleccionado.
        /// </summary>
        int? ModeloId { get; set; }

        /// <summary>
        /// Obtiene o estable el Nombre del Modelo a buscar o el seleccionado.
        /// </summary>
        string NombreModelo { get; set; }

                #endregion

                #region Filtro Sucursal Cliente

        /// <summary>
        /// Obtiene o estable el Identificador del Cliente Idealease seleccionado.
        /// </summary>
        int? ClienteId { get; set; }

        /// <summary>
        /// Obtiene o estable el Nombre del Cliente Idealease a buscar o el seleccionado.
        /// </summary>
        string NombreCliente { get; set; }

                #endregion

                #region Filtro Unidad

        /// <summary>
        /// Obtiene o estable el Número de Serie del Equipo a buscar o el seleccionado.
        /// </summary>
        string VIN { get; set; }

                #endregion

            #endregion

        /// <summary>
        /// Obtiene la Fecha de Inicio del reporte.
        /// </summary>
        DateTime? FechaInicio { get; }

        /// <summary>
        /// Obtiene la Fecha de Fin del reporte.
        /// </summary>
        DateTime? FechaFin { get; }

        #endregion

        #region Métodos

            #region Seguridad

        /// <summary>
        /// Realiza la redirección a la página de advertencia por falta de permisos.
        /// </summary>
        void RedirigirSinPermisoAcceso();
            
            #endregion

        /// <summary>
        /// Establece el Paquete de Navegación para las impresiones.
        /// </summary>
        /// <param name="p">Nombre del Reporte</param>
        /// <param name="parametrosReporte">Diccionario de datos del Reporte.</param>
        void EstablecerPaqueteNavegacionImprimir(string p, Dictionary<string, object> parametrosReporte);

        /// <summary>
        /// Despliega el visor de los formatos a imprimir.
        /// </summary>
        void IrAImprimir();

        /// <summary>
        /// Despliega un mensaje en pantalla.
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar.</param>
        /// <param name="eTipoMensajeIU">Tipo de mensaje a desplegar.</param>
        /// <param name="detalleMensaje">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar.</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU eTipoMensajeIU, string detalleMensaje = null);

        #endregion

    }
}
