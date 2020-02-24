using System;
using System.Collections.Generic;
using System.Data;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IucLineaContratoPSLVIS {
        #region Propiedades
        int? UnidadOperativaID { get; set; }
        int? UnidadID { get; set; }
        int? EquipoID { get; set; }
        string VIN { get; set; }
        string NumeroEconocimico { get; set; }
        int? ModeloID { get; set; }
        string Modelo { get; set; }
        string Marca { get; set; }
        int? Anio { get; set; }
        int? SucursalID { get; set; }
        string Sucursal { get; set; }
        string UnidadPlacaEstatal { get; set; }
        int? UnidadCapacidadTanque { get; set; }

        List<EquipoAliadoBO> ListadoEquiposAliados { get; set; }
        LineaContratoPSLBO UltimoObjeto { get; set; }

        EPeriodosTarifa? PeriodoTarifa { get; set; }
        Enum TarifaTurno { get; set; }
        decimal? Tarifa { get; set; }
        int? TipoTarifaID { get; set; }
        decimal? TarifaHrAdicional { get; set; }
        decimal? Maniobra { get; set; }
        string CodigoMoneda { get; set; }
        Enum EArea { get; set; }
        int? TarifaPSLID { get; set; }
        int? DuracionDiasPeriodo { get; set; }
        decimal? MaximoHrsTurno { get; set; }
        int? ModuloID { get; set; }
        int? UsuarioID { get; set; }
        int? CuentaClienteID { get; set; }
        decimal? PorcentajeMaximoDescuentoTarifa { get; set; }
        decimal? PorcentajeDescuentoTarifa { get; set; }
        decimal? TarifaConDescuento { get; set; }
        string TarifaEtiqueta { get; set; }
        string ModoRegistro { get; set; }
        bool? Activo { get; set; }
        bool Devuelta { get; set; }
        int? LineaOrigenIntercambioID { get; set; }

        //Tarifa Personalizada
        string TarifaPersonalizadaTurno { get; }
        string TarifaPersonalizadaTipoTarifa { get; }
         decimal? PorcentajeSeguro{get; set;}
         decimal? Seguro { get; set; }
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

        void PrepararVistaDetalle(bool activar = false);

        void EstablecerEquiposAliadoUnidad(DataTable dt);

        void LimpiarPaqueteNavegacion(string key);

        void EstablecerPaqueteNavegacion(string key, object value);

        object ObtenerPaqueteNavegacion(string key);

        #endregion
    }
}