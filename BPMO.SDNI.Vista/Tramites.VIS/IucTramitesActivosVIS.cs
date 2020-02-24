//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Tramites.BO;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.VIS;

namespace BPMO.SDNI.Tramites.VIS
{
    public interface IucTramitesActivosVIS
    {
        #region Propiedades
        int? TramitableID { get; set; }
        ETipoTramitable? TipoTramitable { get; set; }
        string DescripcionEnllantable { get; set; }
        List<TramiteBO> Tramites { get; set; }
        

        #region RQM 13285
        string ConfigurarEtiquetaPrincipal(string cEtiquetaBuscar);
        AdscripcionBO Adscripcion { get; set; }
        string NumeroPedimento { get; set; } //REQ: 13285
        List<CatalogoBaseBO> ListaAcciones { get; set; } //REQ: 13285
        List<ArchivoBO> Archivos { get; set; }
        List<TipoArchivoBO> TiposArchivo { get; set; }
        IucCatalogoDocumentosVIS VistaDocumentos { get; }
        bool CambioPedimento { get; set; }

        #endregion
    
        #endregion

        #region Métodos
        void EstablecerPaqueteNavegacion(string nombre, object valor);

        void OcultarRedireccionTramites(bool ocultar);

        void ActualizarTramites();

        void RedirigirACatalogoTramites();

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        #region RQM 13285
        void EstablecerTipoAdjunto(ETipoAdjunto tipo);
        void EstablecerIdentificadorListaArchivos(string identificador);
        void HabilitarPedimento(bool habilitar);
        void EstablecerAcciones();  //REQ: 13285
        void HabilitarCargaArchivo(bool habilitar);
        void ModoEdicion(bool Habilitar);
        #endregion


        #endregion
    }
}
