//Satisface al CU027 - Registrar Contrato de Mantenimiento
using System.Collections.Generic;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Contratos.Mantto.BO;

namespace BPMO.SDNI.Contratos.Mantto.VIS
{
    public interface IucLineaContratoManttoVIS
    {
        #region Propiedades
        int? UsuarioID { get; }
        int? UnidadOperativaID { get; }
        bool ModoEdicion { get; set; }

        int? LineaContratoID { get; set; }
        int? UnidadID { get; set; }
        int? EquipoID { get; set; }
        string VIN { get; set; }
        string NumeroEconomico { get; set; }
        int? Anio { get; set; }
        string SucursalNombre { get; set; }
        string ModeloNombre { get; set; }
        string MarcaNombre { get; set; }
        string PlacaEstatal { get; set; }
        string PlacaFederal { get; set; }
        decimal? CapacidadCarga { get; set; }
        decimal? CapacidadTanque { get; set; }
        decimal? RendimientoTanque { get; set; }
        int? KmEstimadoAnual { get; set; }
        int? CobrableID { get; set; }
        decimal? CargoFijoMensual { get; set; }
        int? KilometrosLibres { get; set; }//SC051
        decimal? CostoKmRecorrido { get; set; }
        int? HorasLibres { get; set; }//SC051
        decimal? CostoHorasRefrigeradas { get; set; }//SC051
        int? PeriodoTarifaKM { get; set; }//SC051
        int? PeriodoTarifaHRS { get; set; }//SC051
        int? ProductoServicioId { get; set; }
        string ClaveProductoServicio { get; set; }
        string DescripcionProductoServicio { get; set; }

        List<SubLineaContratoManttoBO> SubLineasContrato { get; set; }

        #region SC051
        decimal? CostoFijoMensualEA { get; set; }
        int? KilometrosLibresEA { get; set; }
        decimal? CostoKilometroEA { get; set; }
        int? HorasLibresEA { get; set; }
        decimal? CostoHoraRefrigeradaEA { get; set; }
        int? EquipoAliadoID { get; set; }
        bool? MantenimientoEA { get; set; }
        int? PeriodoTarifaKMEA  { get; set; }
        int? PeriodoTarifaHRSEA  { get; set; }
        #endregion
        #endregion

        #region Métodos
        void PrepararEdicion();
        void PrepararVisualizacion();
        
        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #region SC_0051
        void PermitirAsignarTarifasEA(bool p);
        void MostrarTarifasEquipoAliado();
        void EstablecerOpcionesFrecuencia(Dictionary<int, string> lstTipos);
        #endregion
        #endregion
    }
}
