//Satisface al caso de uso CU003 - Consultar Contratos Renta Diaria
// Satisface al CU012 - Imprimir Check List de Entrega Recepción de Unidad
// Satisface al CU013 - Cerrar Contrato Renta Diaria

using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    public interface IucHerramientasRDVIS
    {
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
        void DeshabilitarOpcionesEditarContratoRD();
        void DeshabilitarOpcionesAgregarDoc();
        void OcultarNoContrato();
        void OcultarEstatusContrato();
        void OcultarMenuImpresion();
        void OcultarEditarContrato();
        void OcultarEliminarContrato();
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
        void OcultarImprimirPlantillaCheckList();//CU012
        void MarcarOpcionCerrarContrato();
        void OcultarPlantillas();
        void CargarArchivos(List<object> resultado);//SC0038
        #endregion        
    }
}
