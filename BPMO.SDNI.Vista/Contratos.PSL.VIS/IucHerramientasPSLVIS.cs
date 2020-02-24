using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IucHerramientasPSLVIS {
        #region Propiedades

        string NumeroContrato { get; set; }
        EEstatusContrato? EstatusContrato { get; set; }
        int? ContratoID { get; set; }

        #endregion

        #region Métodos
        void Configurar(EEstatusContrato? estatus);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void HabilitarOpciones(bool habilitar);
        void MarcarOpcionEditarContrato();
        void HabilitarOpcionesEdicion();
        void MarcarOpcionAgregarDocumentos();
        void DeshabilitarOpcionesEditarContratoPSL();
        void DeshabilitarOpcionesAgregarDoc();
        void OcultarNoContrato();
        void OcultarEstatusContrato();
        void OcultarMenuImpresion();
        void OcultarEditarContrato();
        void OcultarCerrarContrato();
        void OcultarImprimirPlantilla();
        void DeshabilitarOpcionImpresion();
        void DeshabilitarOpcionCerrar();
        void DeshabilitarOpcionEditar();
        void DeshabilitarOpcionBorrar();
        void DeshabilitarMenuImpresion();
        /// <summary>
        /// Oculta el menú que imprime la plantilla
        /// </summary>
        void OcultarImprimirPlantillaCheckList();
        void MarcarOpcionCerrarContrato();
        void OcultarPlantillas();
        void OcultarSolicitudPago();

        #endregion
    }
}