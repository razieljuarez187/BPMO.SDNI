// Satiface al caso de uso CU015 - Registrar Contrato Full Service Leasing
// Satiface al caso de uso CU022 - Consultar Contratos Full Service Leasing
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.BO;

namespace BPMO.SDNI.Contratos.FSL.VIS
{
    public interface IucDatosAdicionalesAnexoVIS
    {
        #region Propiedades
        string NombreSesion { get; set; }
        int? ContratoID {get; set; }
        int? Detalle_DatoAdicionalID { get; set; }
        string Detalle_Titulo { get; set; }
        string Detalle_Descripcion { get; set; }
        bool? Detalle_EsObservacion { get; set; }
        List<DatoAdicionalAnexoBO> DatosAdicionales { get; set; }
        bool ModoConsultar { get; set; }
        #endregion

        #region Metodos
        void LimpiarSesion();
        void ConfigurarModoEditar();
        void ConfigurarModoConsultar();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void HabilitarEditar(bool p);
        void HabilitarAgregar(bool p);
        void HabilitarCampos(bool p);
        void MostrarControlesDetalle(bool p);
        #endregion
    }
}
