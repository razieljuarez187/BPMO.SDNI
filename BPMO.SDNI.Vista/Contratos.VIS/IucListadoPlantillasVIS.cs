// Satisface al CU097 - Cargar y descargar formatos y plantillas
using System.Collections.Generic;

namespace BPMO.SDNI.Contratos.VIS
{
    /// <summary>
    /// Vista del user control para los documentos que servirán de plantillas en los contratos
    /// </summary>
    public interface IucListadoPlantillasVIS
    {
        #region Propiedades
        /// <summary>
        /// Obtiene o establece el identificador de la vista
        /// </summary>
        string Identificador { get; set; }
        /// <summary>
        /// Obtiene o estabece si en la vista se puede eliminar las plantillas listadas
        /// </summary>
        bool? PermitirEliminar { get; set; }
        /// <summary>
        /// Obtiene o establece las plantillas a desplegar
        /// </summary>
        List<object> Documentos { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador del archivo que se desea eliminar
        /// </summary>
        int? ArchivoEliminarID { get; set; }
        /// <summary>
        /// Obtiene o establece el número de la página que se desea desplegar
        /// </summary>
        int IndicePaginaResultado { get; set; }
        #endregion

        #region Métodos
        /// <summary>
        /// Despliega en la vista las plantillas que correspondan
        /// </summary>
        /// <param name="elementos">lista con las plantillas qeu se desean desplegar</param>
        void CargarElementosEncontrados(List<object> elementos);
        /// <summary>
        /// Actualiza el npumero de pagina que es visualizado
        /// </summary>
        void ActualizarResultado();
        /// <summary>
        /// Prepara la vista para es despliegue de los resultados
        /// </summary>
        /// <param name="status">Estatus que tomarán lso controles en la vista</param>
        void PrepararVista(bool status);
        /// <summary>
        /// Prepara la vista para la edición de lso resutlados desplegados
        /// </summary>
        /// <param name="status">Estatus que toamrán los controles en la vista</param>
        void PrepararEdicion(bool status);
        #endregion        
    }
}