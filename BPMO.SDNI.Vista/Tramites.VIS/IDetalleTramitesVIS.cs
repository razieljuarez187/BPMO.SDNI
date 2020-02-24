//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Collections.Generic;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Tramites.VIS
{
    public interface IDetalleTramitesVIS
    {
        #region Propiedades
        string NumeroSerie { get; set; }
        string Modelo { get; set; }
        string Marca { get; set; }
        ITramitable Tramitable { get; set; }
        TenenciaBO Tenencia { get; set; }
        VerificacionBO VerificacionAmbiental { get; set; } 
        
        VerificacionBO VerificacionMecanica { get; set; }
        PlacaEstatalBO PlacaEstatal { get; set; }
        PlacaFederalBO PlacaFederal { get; set; }
        GPSBO GPS { get; set; }
        FiltroAKBO FiltroAK { get; set; }
        SeguroBO Seguro { get; set; }
        string Poliza{get;set;}
        string Aseguradora{get;set;}
        DateTime? FechaInicial{get;set;}
        DateTime? FechaFinal{get;set;}
        List<TenenciaBO> Tenencias { get; set; }
        List<VerificacionBO> VerificacionesAmbientales { get; set; }
        List<VerificacionBO> VerificacionesMecanicas { get; set; }
        List<PlacaEstatalBO> PlacasEstatales { get; set; }
        List<PlacaFederalBO> PlacasFederales { get; set; }
        List<GPSBO> ListaGPS { get; set; }
        List<FiltroAKBO> FiltroAKs { get; set; }
        #region SC_0008
        int? UnidadOperativaId { get; }
        int? UsuarioId { get; }
        #endregion
        #endregion

        #region Metodos
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void EstablecerDatosNavegacion(string nombre, object unidad, string nombreTramite, object tramite,string nombreTipo,ETipoTramite tipo);
        void EstablecerDatosNavegacionSeguro(string nombre, object unidad);
        void LimpiarSesion();
        void RedirigirARegistrar();
        void RedigirAEdicion();
        void RedirigirARegistrarSeguro();
        void RedirigirAEditarSeguro();
        void EstablecerPagina(int numeroPagina);
        void MostrarTenencias();
        void MostrarGPSs();
        void MostrarFiltros();
        void MostrarPlacasFederales();
        void MostrarPlacaEstatales();
        void MostrarVerificacionAmbiental();
        void MostrarVerificacionMecanico();

        #region SC0008
        void PermitirEditar(bool permitir);
        void PermitirRegistrar(bool permitir);
        void RedirigirSinPermisoAcceso(); 
        #endregion
        #endregion
    }
}
