//Satisface al CU087 – Catálogo Tramite Unidad
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Tramites.VIS
{
    public interface IEditarTramitesVIS
    {
        #region Propiedades
        int? UnidadOperativaId { get; }
        int? UsuarioId { get; }
        string NumeroSerie { get; set; }
        string Modelo { get; set; }
        string Marca { get; set; }
        ETipoTramite? Tipo { get; }
        ITramitable Tramitable { get; }
        object UltimoTramite { get; }
        object Tramite { get; set; }
        #endregion
        #region Métodos
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void LimpiarSesion();
        void LimpiarSesionDatosNavegacion();
        void EstablecerDatosNavegacion(string nombre, object objeto);
        void MostrarTenencia();
        void MostrarVerificacionAmbiental();
        void MostrarVerificacionMecanico();
        void MostrarPlacaEstatal();
        void MostrarPlacaFederal();
        void MostrarGPS();
        void MostrarFiltroAK();
        void RedirigirADetalle();
        #region SC0008
        void RedirigirSinPermisoAcceso();
        #endregion
        #endregion
    }
}
