// Satisface al CU097 - Cargar y descargar formatos y plantillas
using System;
using System.Collections.Generic;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.BR;
using BPMO.SDNI.Contratos.VIS;

namespace BPMO.SDNI.Contratos.PRE
{
    /// <summary>
    /// Handler para descargar el archivo
    /// </summary>
    public class hdlrDescargarPlantillaPRE
    {
        #region Atributos
        /// <summary>
        /// Provee la conexión a la BD
        /// </summary>
        private IDataContext dctx = null;
        /// <summary>
        /// Vista para el handler de descarga de archivo
        /// </summary>
        private IhdlrDescargarPlantillaVIS vista;
        #endregion

        #region Constructor
        /// <summary>
        /// COnstructor del handler
        /// </summary>
        /// <param name="iVista">Vista del handler</param>
        public hdlrDescargarPlantillaPRE(IhdlrDescargarPlantillaVIS iVista)
		{
            this.vista = iVista;
            this.dctx = FacadeBR.ObtenerConexion();
		}
        #endregion

        #region Métodos
        /// <summary>
        /// Obtiene el archivo que se desea descargar
        /// </summary>
        /// <param name="archivoID">Identificador del archivo que se desea descargar</param>
        /// <returns>BO con la información del archivo que se desea descargar</returns>
        public PlantillaBO ObtenerArchivoDescargable(int archivoID)
        {
                try
                {
                    PlantillaBR plantillaBR = new PlantillaBR();
                    List<PlantillaBO> archivos = plantillaBR.ConsultarCompleto(dctx, new PlantillaBO{ Id = archivoID });
                    var archivoBO = new PlantillaBO();
                    if(archivos != null)
                        if(archivos.Count > 0)
                            archivoBO = archivos[0];

                    return archivoBO;

                }
                catch (Exception ex)
                {
                    throw new Exception("No se pudo obtener el archivo de descarga");
                }
        }
        #endregion
    }
}
