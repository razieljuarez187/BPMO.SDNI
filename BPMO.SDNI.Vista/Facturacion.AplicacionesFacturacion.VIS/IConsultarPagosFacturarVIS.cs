//Satisface al caso de uso CU006 - Ver Historico de Pagos
//Satisface al caso de uso CU004 - Consulta de Pagos a Facturar
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS
{
    public interface IConsultarPagosFacturarVIS: IPagosBaseVIS
    {
        #region Propiedades
        ReferenciaContratoBO ReferenciaContratoSeleccionada { get; set; }
        UnidadOperativaBO UnidadOperativa { get; }
        int? UC { get; }
        List<PagoUnidadContratoBO> UltimosPagos { get; set; }
        List<PagoContratoPSLBO> UltimosPagosPSL { get; set; }
        int? ModuloID { get; }
        string URLLogoEmpresa { get; }
        string CancelaPagoCodigoAutorizacion { get; set; }
        string MotivoCancelarPago { get; set; }
        int? PagoACancelarID { get; set; }
        AdscripcionBO Adscripcion { get; }
        #endregion

        #region Metodos

        void Consultar();
        void CargarPagosConsultados();
        void IrAImprimirHistorico();
        void IrAConfigurarFacturacion(bool pagoPSL);
        void EstablecerPaqueteNavegacionImprimir(string codigoNavegacion, object paqueteNavegacion);
        void EstablecerPaqueteNavegacionFacturacion(object paqueteNavegacion);
        void PermitirFacturar(bool permitir);
        void PermitirHistorico(bool permitir);
        void RedirigirSinPermisoAcceso();
        void PermitirCancelarPago(bool permitir);
        void PermitirValidarCodigoAutorizacion(bool permitir);
        void PermitirSolicitarCodigoAutorizacion(bool permitir);

        #endregion

        
    }
}
