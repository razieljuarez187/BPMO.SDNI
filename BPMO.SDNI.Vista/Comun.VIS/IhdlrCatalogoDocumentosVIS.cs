//Satisface al CU010 - Catálogo de Documentos
using System.Collections.Generic;
using BPMO.SDNI.Comun.BO;
using System.Web;
using System.IO;

namespace BPMO.SDNI.Comun.VIS
{
    public interface IhdlrCatalogoDocumentosVIS
    {
        #region Propiedades
        HttpContext ContextValue
        {
            get;
            set;
        }

        int QS_archivoID
        {
            get;
        }

        ArchivoBO ArchivoTemp
        {
            get;
        }

        #endregion

        #region Métodos
        MemoryStream GetData(int archivoID);
        #endregion
    }
}
