//Satisface al caso de uso CU012 - Ver Pagos No Facturados
using BPMO.Basicos.BO;
namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS
{
    public interface IConsultarPagosNoFacturadosVIS: IPagosBaseVIS
    {
        #region Propiedades
        int? ModuloID { get; }
        string CancelaPagoCodigoAutorizacion { get; set; }
        string MotivoCancelarPago { get; set; }
        int? PagoACancelarID { get; set; }
        AdscripcionBO Adscripcion { get; }
        #endregion

        #region Metodos
        void Consultar();
        void CargarPagosConsultados();
		void RedirigirSinPermisoAcceso();
        void PermitirMover(bool permitir);
        void PermitirCancelarPago(bool permitir);
        void PermitirValidarCodigoAutorizacion(bool permitir);
        void PermitirSolicitarCodigoAutorizacion(bool permitir);
        void ActualizarMarcadoresEnviarAFacturar();
        #endregion
    }
}
