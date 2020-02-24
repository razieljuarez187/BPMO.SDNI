//Satisface al CU081 - Consultar Seguimiento Flota
//Satisface al CU074 - Consultar Expediente de Unidades
using System;

using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Flota.VIS
{
    public interface IDetalleExpedienteUnidadVIS
    {
        #region Propiedades
        int? UsuarioID { get; }
        int? UnidadOperativaID { get; }
        int? ModuloID { get; }

        int? UnidadID { get; set; }
        int? EquipoID { get; set; }
        int? AreaID { get; set; }
        string AreaNombre { get; set; }
        int? EstatusID { get; set; }
        string EstatusNombre { get; set; }
        string NumeroSerie { get; set; }
        string NumeroEconomico { get; set; }
        string ClaveActivoOracle { get; set; }
        int? IDLider { get; set; }
        string SucursalNombre { get; set; }
        string TipoUnidadNombre { get; set; }
        string ModeloNombre { get; set; }
        int? Anio { get; set; }
        object Llantas { set; }
        object EquiposAliados { set; }

        DateTime? FechaCompra { get; set; }
        decimal? MontoFactura { get; set; }
        string FolioFactura { get; set; }
        decimal? ValorLibros { get; set; }
        decimal? ResidualPorcentaje { get; set; }
        decimal? ResidualMonto { get; set; }
        decimal? DepreciacionMensualPorcentaje { get; set; }
        decimal? DepreciacionMensualMonto { get; set; }
        int? MesesVidaUtilTotal { get; set; }
        int? MesesVidaUtilRestante { get; set; }
        DateTime? FechaSustitucion { get; set; }
        int? TiempoUsoActivos { get; set; }

        string NumeroPlaca { get; set; }
        string TipoPlaca { get; set; }
        int? SeguroID { get; set; }
        string Aseguradora { get; set; }
        string NumeroPoliza { get; set; }
        DateTime? FechaVigenciaSeguroInicial { get; set; }
        DateTime? FechaVigenciaSeguroFinal { get; set; }

        int? ContratoID { get; set; }
        int? TipoContratoID { get; set; }
        string NumeroContrato { get; set; }
        DateTime? FechaInicioContrato { get; set; }
        DateTime? FechaVencimientoContrato { get; set; }
        string CuentaClienteNombre { get; set; }
        int? MesesFaltantesContrato { get; set; }

        bool? EstaDisponible { get; set; }
        bool? EstaEnContrato { get; set; }
        bool? TieneEquipoAliado { get; set; }
        #endregion

        #region Métodos
        void PrepararVisualizacion();

        void EstablecerPaqueteNavegacion(string key, object value);
        object ObtenerPaqueteNavegacion(string key);
        void LimpiarPaqueteNavegacion(string key);

        void RedirigirSinPermisoAcceso();
        void RedirigirAConsulta();
        void RedirigirAAltaFlota();
        void RedirigirABajaFlota();
        void RedirigirAReactivacionFlota();
        void RedirigirACambioSucursalUnidad();
        void RedirigirACambioDepartamentoUnidad();
        void RedirigirACambioAsignacionEquiposAliados();
        void RedirigirADetalleActaNacimiento();
        void RedirigirADetalleHistorial();
        void RedirigirADetalleTramites();
        void RedirigirADetalleSeguro();
        void RedirigirADetalleContratoFSL();
        void RedirigirADetalleContratoRD();
        void RedirigirADetalleContratoCM();
        void RedirigirADetalleContratoSD();

        void PermitirRegresar(bool permitir);
        void PermitirRealizarAltaFlota(bool permitir);
        void PermitirRealizarBajaFlota(bool permitir);
        void PermitirRealizarReactivacionFlota(bool permitir);
        void PermitirRealizarCambioSucursalUnidad(bool permitir);
        void PermitirRealizarCambioDepartamentoUnidad(bool permitir);
        void PermitirRealizarCambioAsignacionEquiposAliados(bool permitir);
        void PermitirRealizarCambioSucursalEquipoAliado(bool permitir);
        void PermitirConsultarActaNacimiento(bool permitir);
        void PermitirConsultarHistorial(bool permitir);
        void PermitirConsultarTramites(bool permitir);
        void PermitirConsultarSeguro(bool permitir);
        void PermitirConsultarContrato(bool permitir);

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion
    }
}
