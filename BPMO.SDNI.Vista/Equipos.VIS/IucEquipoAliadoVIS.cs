//Satisface al CU075 - Catálogo de Equipo Aliado
//Satisface a la SC0005
//Satisface a la solicitud de cambio SC0035
using System;
using BPMO.Primitivos.Enumeradores;
using BPMO.Basicos.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Equipos.VIS
{
    public interface IucEquipoAliadoVIS
    {
        #region Propiedades
        int? UnidadOperativaID { get; set; }
        int? ModuloID { get; }
        string LibroActivos { get; set; }

        int? SucursalID { get; set; }
        string SucursalNombre { get; set; }
        int? EquipoAliadoID { get; set; }
        int? EquipoID { get; set; }
        string NumeroSerie { get; set; }
        string Fabricante { get; set; }
        string Marca { get; set; }
        string Modelo { get; set; }
        string AnioModelo { get; set; }
        decimal? PBV { get; set; }
        decimal? PBC { get; set; }
        int? TipoEquipoID { get; set; }
        string TipoEquipoNombre { get; set; }
        int? TipoEquipoAliado { get; set; }
        string Dimension { get; set; }
        DateTime? FC { get; }
        DateTime? FUA { get; }
        int? UC { get; }
        int? UUA { get; }
        int? ModeloID { get; set; }
        UsuarioBO Usuario { get; }
        UnidadOperativaBO UnidadOperativa { get; }
        int? EquipoLiderID { get; set; }
        string OracleID { get; set; }
        bool? ActivoOracle { get; set; }//SC0035
        int? HorasIniciales { get; set; }
        int? KilometrosIniciales { get; set; }
        EquipoAliadoBO UltimoObjeto { get; set; }
        int? EstatusEquipo { get; set; }	
        bool? Activo { get; set; }

        #region REQ 13596

        int? MarcaID { get; set; }
        ETipoEmpresa EmpresaConPermiso { set; get; }

        #endregion
        #endregion

        #region Métodos
        void LimpiarSesion();

        void PrepararNuevo();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        
        void PrepararVista();

        void RedirigirAConsulta();

        void PrepararModificacion();

        object ObtenerDatosNavegacion();

        void EstablecerPaqueteNavegacion(string p, object bo);

        void RedirigirADetalles();

        void CargarTiposEquipoAliado();

        void AbilitarKMHRS(bool KmHrs);

        #region REQ 13596

        void EstablecerAcciones();

        void AsignarTipoEquipoAliado();

        #endregion
        #endregion
    }
}