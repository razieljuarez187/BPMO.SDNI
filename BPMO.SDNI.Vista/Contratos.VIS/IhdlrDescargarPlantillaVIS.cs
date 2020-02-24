// Satisface al CU097 - Cargar y descargar formatos y plantillas
using System.IO;
using System.Web;
using BPMO.SDNI.Contratos.BO;

namespace BPMO.SDNI.Contratos.VIS
{
    /// <summary>
    /// Handdler para la descarga de los documentos que servirán como plantilla para los contratos
    /// </summary>
    public interface IhdlrDescargarPlantillaVIS
    {
        #region Propiedades
        /// <summary>
        /// Obtiene o establece el contexto
        /// </summary>
        HttpContext ContextValue
        {
            get;
            set;
        }
        /// <summary>
        /// Obtiene el identificador del archivo
        /// </summary>
        int QS_archivoID
        {
            get;
        }
        /// <summary>
        /// Obtiene el archivo que se va adescargar
        /// </summary>
        PlantillaBO ArchivoTemp
        {
            get;
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Obtiene el archivo que se va adescargar
        /// </summary>
        /// <param name="archivoID">Identificador del archivo que se desea decargar</param>
        /// <returns>Flujo con el archivo que se va adescargar</returns>
        MemoryStream GetData(int archivoID);
        #endregion
    }
}