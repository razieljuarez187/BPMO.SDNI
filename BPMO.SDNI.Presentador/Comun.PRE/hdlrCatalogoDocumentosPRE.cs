//Satisface al CU010 - Catálogo de Documentos
using System;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.VIS;

namespace BPMO.SDNI.Comun.PRE
{
    public class hdlrCatalogoDocumentosPRE
    {
        #region Atributos
        private IDataContext dctx = null;
        private IhdlrCatalogoDocumentosVIS vista;
        #endregion

        #region Constructor
        public hdlrCatalogoDocumentosPRE(IhdlrCatalogoDocumentosVIS iHdlrAgregarDocumentosVis)
		{
            this.vista = iHdlrAgregarDocumentosVis;
            this.dctx = FacadeBR.ObtenerConexion();
		}
        #endregion

        #region Métodos
        public ArchivoBO ObtenerArchivoDescargable(int archivoID)
        {
                try
                {
                    ArchivoBR archivoBR = new ArchivoBR();
                    ArchivoBO archivoBO = archivoBR.ConsultarCompleto(dctx, archivoID);
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
