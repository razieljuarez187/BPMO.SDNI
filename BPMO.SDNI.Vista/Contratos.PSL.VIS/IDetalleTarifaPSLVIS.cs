// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
using System;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IDetalleTarifaPSLVIS {
        #region Propiedades
        int? ModuloID { get; }
        int? UnidadOperativaID { get; }
        int? UsuarioID { get; }
        int? TarifaPSLID { get; set; }
        string NombreSucursal { set; }
        int? SucursalID { get; set; }
        string NombreModelo { set; }
        int? ModeloID { get; set; }
        string NombreMoneda { set; }
        string CodigoMoneda { get; set; }
        string NombreTipoTarifa { set; }
        int? TipoTarifa { get; set; }
        string NombreTarifaTurno { get; set; }
        Enum TarifaTurno { get; set; }
        string NombrePeriodoTarifa { get; set; }
        EPeriodosTarifa PeriodoTarifa { get; set; }
        string Estatus { set; }
        DateTime? FechaRegistro { set; }
        DateTime? FechaModificacion { set; }
        string UsuarioRegistro { set; }
        string UsuarioModificacion { set; }
        decimal? PrecioCombustible { set; }
        #endregion

        #region Métodos

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void PermitirEditar(bool activo);
        void PermitirRegistrar(bool activo);
        void RedirigirSinPermisoAcceso();
        void ModoDetalle(bool activo);
        void LimpiarSesion();
        object ObtenerDatosNavegacion();
        void EstablecerDatosNavegacion(object tarifa);
        void RedirigirAEditar();
        #region SC0024
        void RegresarAConsultar();//SC0024
        void PermitirRegresar(bool permiso); //SC0024
        object ObtenerFiltrosConsulta(); //SC0024
        #endregion

        #endregion
    }
}