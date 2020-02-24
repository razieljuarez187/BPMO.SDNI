// Satisface al CU013 - Cerrar Contrato Renta Diaria
using System;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    public interface ICerrarContratoRDVIS
    {
        #region Propiedades
        int? UsuarioID { get; }
        int? UnidadOperativaID { get; }
        int? ModuloID { get; }

        object UltimoObjeto { get; set; }

        int? ContratoID { get; set; }
        int? EstatusID { get; set; }
        int? UUA { get; set; }
        DateTime? FUA { get; set; }

        int? UnidadID { get; set; }
        int? EquipoID { get; set; }

        int? KmEntrega { get; set; }
        int? KmRecepcion { get; set; }
        int? KmRecorrido { get; set; }
        int? TarifaKmLibres { get; set; }
        int? KmExcedido { get; set; }
        decimal? TarifaKmExcedido { get; set; }
        decimal? MontoKmExcedido { get; set; }

        int? HrsEntrega { get; set; }
        int? HrsRecepcion { get; set; }
        int? HrsConsumidas { get; set; }
        int? TarifaHrsLibres { get; set; }
        int? HrsExcedidas { get; set; }
        decimal? TarifaHrsExcedidas { get; set; }
        decimal? MontoHrsExcedidas { get; set; }

        decimal? ImporteUnidadCombustible { get; set; }
        decimal? DiferenciaCombustible { get; set; }
        decimal? ImporteTotalCombustible { get; set; }
        decimal? CargoAbusoOperacion { get; set; }
        decimal? CargoDisposicionBasura { get; set; }

        decimal? ImporteDeposito { get; set; }
        decimal? ImporteReembolso { get; set; }
        string PersonaRecibeReembolso { get; set; }

        int? DiasRentaProgramada { get; set; }
        int? DiasEnTaller { get; set; }
        int? DiasRealesRenta { get; set; }
        int? DiasAdicionales { get; set; }
        decimal? MontoTotalDiasAdicionales { get; set; }

        string ObservacionesCierre { get; set; }
        DateTime? FechaCierre { get; set; }
		DateTime? FechaContrato { get; set; }
        DateTime? FechaRecepcion { get; set; }
        #endregion

        #region Métodos
        void PrepararEdicion();

        void EstablecerPaqueteNavegacion(string key, object value);
        object ObtenerPaqueteNavegacion(string key);
        void LimpiarPaqueteNavegacion(string key);

        void RedirigirACancelar();
        void RedirigirADetalles();
        void RedirigirAConsulta();
        void RedirigirSinPermisoAcceso();

        void PermitirCerrar(bool permitir);
        void PermitirRegistrar(bool permitir);

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        #endregion
    }
}
