using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IucCargosAdicionalesCierrePSLVIS {
        #region Propiedades
        int? UnidadOperativaID { get; set; }
        int? UnidadID { get; set; }
        int? EquipoID { get; set; }
        int? ModeloID { get; set; }
        int? SucursalID { get; set; }
        List<EquipoAliadoBO> ListadoEquiposAliados { get; set; }
        LineaContratoPSLBO UltimoObjeto { get; set; }
        string CodigoMoneda { get; set; }
        Enum EArea { get; set; }
        int? TarifaPSLID { get; set; }
        int? DuracionDiasPeriodo { get; set; }
        decimal? MaximoHrsTurno { get; set; }
        string ModoRegistro { get; set; }
        int? ModuloID { get; }

        /**/
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

        int? DiasRentaProgramada { get; set; }
        int? DiasEnTaller { get; set; }
        int? DiasRealesRenta { get; set; }
        int? DiasAdicionales { get; set; }
        decimal? MontoTotalDiasAdicionales { get; set; }
        /**/
        #endregion

        #region Metodos
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        void RegistrarScript(string key, string script);

        void LimpiarSesion();

        void Inicializar();

        void ConfigurarModoConsultar();

        void ConfigurarModoEditar();

        void EstablecerOpcionesTipoTarifa(Dictionary<int, string> tipos);

        void EstablecerOpcionesTarifaTurno(Dictionary<string, string> turno);

        void EstablecerOpcionesPeriodoTarifa(Dictionary<string, string> periodo);

        void EstablecerEquiposAliadoUnidad(System.Data.DataTable dt);

        void PrepararVistaDetalle();

        #endregion
    }
}