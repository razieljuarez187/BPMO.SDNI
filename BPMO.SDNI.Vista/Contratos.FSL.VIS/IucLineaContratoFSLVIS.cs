// Satisface al CU015 - Registrar Contrato Full Service Leasing
// Satisface al CU022 - Consultar Contratos Full Service Leasing
// Satisface al CU023 - Editar Contrato Full Service Leasing
// Mejoras Durante Staffing - Cobro de Rangos de Kms /Hrs
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Contratos.FSL.BO;

namespace BPMO.SDNI.Contratos.FSL.VIS
{
    public interface IucLineaContratoFSLVIS
    {
        #region Propiedades
        int? UnidadOperativaID { get; set; }
        int? UnidadID { get; set; }
        int? EquipoID { get; set; }
        string VIN { get; set; }
        string NumeroEconocimico { get; set; }
        string Modelo { get; set; }
        int? Anio { get; set; }
        Decimal? PBV { get; set; }
        Decimal? PBC { get; set; }
        int? KmInicial { get; set; }
        int? KmEstimadoAnual { get; set; }
        Decimal? DepositoGarantia { get; set; }
        Decimal? ComisionApertura { get; set; }
        Decimal? CargoFijoMes { get; set; }
        List<ETipoCotizacion> ListadoTiposCotizacion { get; set; }
        ETipoCotizacion? TipoCotizacionSeleccionada { get; }
        int? CobroKilometrosHoras { get; }
        int? PlazoAnio { get; set; }
        bool OpcionCompra { get; set; }
        List<MonedaBO> ListadoMonedas { get; set; }
        MonedaBO MonedaSeleccionada { get; }
        decimal? ImporteCompra { get; set; }
        List<EquipoAliadoBO> ListadoEquiposAliados { get; set; }
        List<CargoAdicionalEquipoAliadoBO> CargosAdicionalesEquiposAliados{ get; }
        List<TarifaFSLBO> TarifasAdicionales { get; }
        LineaContratoFSLBO UltimoObjeto { get; set; }
        string NumeroPoliza { set; }
        int? ProductoServicioId { get; set; }
        string ClaveProductoServicio { get; set; }
        string DescripcionProductoServicio { get; set; }
        #endregion

        #region Metodos
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        void HabilitarCompra(bool habilitar);

        void RegistrarScript(string key, string script);

        void LimpiarSesion();

        void LimpiarSesionTarifas();

        void Inicializar();

        void EstablecerMonedaCompra(MonedaBO moneda);

        void EstablecerTipoCotizacion(ETipoCotizacion? tipo);

        void EstablecerTarifas(List<TarifaFSLBO> tarifas, bool? cargoPorKm);

        void EstablecerKmsHrs(Int32? indice);

        void EstablecerCargosAdicionalesEquiposAliados(List<CargoAdicionalEquipoAliadoBO> cargos);

        void ConfigurarModoConsultar();

        void ConfigurarModoEditar();

        #endregion
    }
}
