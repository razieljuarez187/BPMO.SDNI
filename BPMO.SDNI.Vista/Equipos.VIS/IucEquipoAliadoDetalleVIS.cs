//Satisface al CU075 - Catálogo de Equipo Aliado
//Satisface a la SC0005
using System;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Equipos.VIS
{
    public interface IucEquipoAliadoDetalleVIS
    {
        #region Propiedades
        int? UnidadOperativaID { get; set; }
        string UnidadOperativaNombre { get; set; }
        int? SucursalID { get; set; }
        string SucursalNombre { get; set; }
        int? EquipoAliadoID { get; set; }
        int? EquipoID { get; set; }
        bool? ActivoOracle { get; set; }
        string NumeroSerie { get; set; }
        string Fabricante { get; set; }
        string Marca { get; set; }
        string Modelo { get; set; }
        string AnioModelo { get; set; }
        decimal? PBV { get; set; }
        decimal? PBC { get; set; }
        int? TipoEquipoID { get; set; }
        string TipoEquipoNombre { get; set; }
        string TipoEquipoAliado { get; set; }
        string Dimension { get; set; }
        int? EstatusID { get; set; }
        string EstatusNombre { get; set; }
        DateTime? FC { get; }
        DateTime? FUA { get; }
        int? UC { get; }
        int? UUA { get; }
        int? ModeloID { get; set; }
        UsuarioBO Usuario { get; }
        UnidadOperativaBO UnidadOperativa { get; }
        int? EquipoLiderID { get; set; }
        string OracleID { get; set; }
        int? HorasIniciales { get; set; }
        int? KilometrosIniciales { get; set; }
        bool? Activo { get; set; }		
        BO.EquipoAliadoBO UltimoObjeto { get; set; }
        #endregion

        #region Métodos
        void PrepararVista();

        object ObtenerDatosNavegacion();

        void RedirigirAEdicion();

        void EstablecerDatosNavegacion(string nombre, object valor);

        void LimpiarSesion();

        void RedirigirAEliminar();

        void RedirigirAConsulta();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        #region REQ 13596

        void EstablecerAcciones(ETipoEmpresa tipoEmpresa);

        #endregion
        #endregion
    }
}