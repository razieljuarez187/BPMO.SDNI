//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.Tramites.BO;

namespace BPMO.SDNI.Tramites.VIS
{
    public interface IucTramiteVerificacionVIS
    {
        #region Propiedades
        int? UC { get; }
        int? UUA { get; }
        DateTime? FC { get; }
        DateTime? FUA { get; }
        List<ArchivoBO> Archivos { get; set; }
        string Folio { get; set; }
        DateTime? FechaInicio { get; set; }
        DateTime? FechaFinal { get; set; }
        ETipoVerificacion? TipoTramite { get; set; }
        VerificacionBO UltimoObjetoVerificacion { get; set; }
            
        List<TipoArchivoBO> TiposArchivo { get; set;}
        IucCatalogoDocumentosVIS VistaDocumentos { get; }
        #endregion

        #region Métodos
        void ModoEdicion(bool habilitar);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void LimpiarSesion();
        void EstablecerIdentificadorListaArchivos(string identificador);
        void EstablecerTipoAdjunto(ETipoAdjunto tipo);
        #endregion
    }
}
