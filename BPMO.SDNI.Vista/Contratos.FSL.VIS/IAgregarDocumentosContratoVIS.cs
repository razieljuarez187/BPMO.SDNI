// Satisface al Caso de Uso CU023 - Editar Contrato Full Service Leasing
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.FSL.BO;

namespace BPMO.SDNI.Contratos.FSL.VIS
{
    public interface IAgregarDocumentosContratoVIS
    {
        #region Propiedades
        string Codigo { get; }
        string CodigoUltimoObjeto { get; set; }
        ContratoFSLBO UltimoObjeto { get; set; }
        int? ContratoID { get; set; }
        EEstatusContrato? Estatus { get; set; }
        int? UUA { get; }
        DateTime? FUA { get; }
        int? UnidadOperativaContratoID { get; set; }
        //SC_0008
        int? UsuarioID { get; }
        int? UnidadAdscripcionID { get; }
        //
        #endregion

        #region Metodos
        void LimpiarSesion();
        void RegresarAConsultar();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void CargarTiposArchivos(List<TipoArchivoBO> tipos);
        void IrADetalleContrato();
        void EstablecerPaqueteNavegacion(string Clave, ContratoBO contrato);
        #region SC_0008
        void RedirigirSinPermisoAcceso();
        void PermitirRegistrar(bool p);
        #endregion
        #endregion        
    }
}