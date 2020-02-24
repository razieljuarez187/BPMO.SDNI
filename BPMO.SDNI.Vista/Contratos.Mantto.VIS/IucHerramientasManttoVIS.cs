//Satisface al caso de uso CU029 - Consultar Contratos de mantenimiento
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;

namespace BPMO.SDNI.Contratos.Mantto.VIS
{
    public interface IucHerramientasManttoVIS
    {
        #region Propiedades

        string NumeroContrato { get; set; }
        EEstatusContrato? EstatusContrato { get; set; }
        int? ContratoID { get; set; }

        #endregion

        #region Métodos
        void Configurar(EEstatusContrato? estatus);

        void HabilitarOpciones(bool habilitar);
        void HabilitarOpcionesEdicion();

        void MarcarOpcionEditarContrato();
        void MarcarOpcionAgregarDocumentos();
        void MarcarOpcionCerrarContrato();
        void MarcarOpcionCancelarContrato();

        void OcultarNoContrato();
        void OcultarEstatusContrato();
        void OcultarMenuImpresion();
        void OcultarEditarContrato();
        void OcultarEliminarContrato();
        void OcultarCerrarContrato();
        void OcultarPlantillas();

        void DeshabilitarOpcionImpresion();
        void DeshabilitarOpcionCerrar();
        void DeshabilitarOpcionEditar();
        void DeshabilitarOpcionBorrar();
        void DeshabilitarMenuImpresion();
        void DeshabilitarOpcionesEditarContrato();
        void DeshabilitarOpcionesAgregarDoc();

        void CargarArchivos(List<object> resultado);

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        #endregion        
    }
}
