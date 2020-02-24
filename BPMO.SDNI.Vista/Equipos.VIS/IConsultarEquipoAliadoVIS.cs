//Satisface al CU075 - Catálogo de Equipo Aliado
// Satisface a la SC0005
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Equipos.VIS
{
    public interface IConsultarEquipoAliadoVIS
    {
        #region Propiedades
        int? UnidadOperativaID { get;}
        int? SucursalID { get; set; }
        string SucursalNombre { get; set; }
        string NumeroSerie { get; set; }
        int? MarcaID { get; set; }
        string Marca { get; set; }
        bool? ActivoOracle { get; set; }
        int? Estatus { get; set; }
        List<EquipoAliadoBO> Equipos { get; set; }
        int IndicePaginaResultado { get; set; }
        int? UsuarioAutenticado { get; }
        int? ModuloID { get; }
        string LibroActivos { get; set; }
        int? TipoEquipoAliado { get; set; }
        #endregion

        #region Métodos
        void PrepararBusqueda();

        void CargarTiposEquipoAliado(); //SC0005
        void CargarEstatusEquipos();
        void ActualizarResultado();
        void EstablecerPaqueteNavegacion(string nombre, object valor);
        void RedirigirADetalles();
        void LimpiarSesion();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        void PermitirRegistrar(bool permitir); //SC_0008
        void RedirigirSinPermisoAcceso(); //SC_0008
        #endregion
    }
}