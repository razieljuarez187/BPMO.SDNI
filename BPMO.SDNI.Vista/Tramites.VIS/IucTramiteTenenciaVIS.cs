//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Comun.VIS;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Tramites.VIS
{
    public interface IucTramiteTenenciaVIS
    {
        #region Propiedades
        int? UC { get;}
        int? UUA { get; }
        DateTime? FC { get; }
        DateTime? FUA { get; }
        List<ArchivoBO> Archivos { get; set; }
        Decimal? Importe { get; set; }
        string Folio { get; set; }
        DateTime? FechaPago { get; set; }
        TenenciaBO UltimoObjetoTenencia { get; set; }
        List<TipoArchivoBO> TiposArchivo { get; set; }
        IucCatalogoDocumentosVIS VistaDocumentos { get; }
        #endregion

        #region Métodos
        void ModoEdicion(bool Habilitar);
        void MostrarMensaje(string mensaje,ETipoMensajeIU tipo,string detalle);
        void LimpiarSesion();
        void EstablecerTipoAdjunto(ETipoAdjunto tipo);
        void EstablecerIdentificadorListaArchivos(string identificador);
        #endregion
    }
}
