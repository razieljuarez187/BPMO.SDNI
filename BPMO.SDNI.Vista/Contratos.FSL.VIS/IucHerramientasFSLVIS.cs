// Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al caso de uso CU023 - Editar Contrato Full Service Leasing 

using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;

namespace BPMO.SDNI.Contratos.FSL.VIS
{
    public interface IucHerramientasFSLVIS
    {
        #region Propiedades
        string NumeroContrato { get; set; }
        EEstatusContrato? EstatusContrato { get; set; }
        int? ContratoID { get; set; }
        #endregion

        #region Metodos
        void Configurar(EEstatusContrato? estatus);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void HabilitarOpciones(bool habilitar);
        void MarcarOpcionEditarContrato();
        #region SC0002
        void HabilitarOpcionesEdicion();
        void MarcarOpcionAgregarDocumentos();
        void DeshabilitarOpcionesEditarContratoFSL();
        void DeshabilitarOpcionesAgregarDoc();
        #endregion
        void OcultarNoContrato();
        void OcultarEstatusContrato();
        void OcultarMenuImpresion();
        void OcultarEditarContrato();
        void OcultarEliminarContrato();
        void OcultarCerrarContrato();
        void OcultarFormatosContrato();
        #region SC_0008
        void DeshabilitarOpcionImpresion();
        void DeshabilitarOpcionCerrar();
        void DeshabilitarOpcionEditar();
        void DeshabilitarOpcionBorrar();
        void DeshabilitarMenuImpresion();
        #endregion        
        void OcultarPlantillas();
        void CargarArchivos(List<object> resultado);//SC0038
        void DeshabilitarOpcionesModificarUnidadesContrato();
        void OcultarModificarUnidadesContrato();

        #endregion
    }
}
