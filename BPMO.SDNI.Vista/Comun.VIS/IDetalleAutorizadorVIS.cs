//Satisface al caso de uso CU098 - Configurar Autorizadores para los Contratos
using System;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Comun.VIS
{
    public interface IDetalleAutorizadorVIS
    {
        #region Propiedades
        int? UsuarioID { get; }
        int? UnidadOperativaID { get; }

        int? AutorizadorID { get; set; }
        string SucursalNombre { get; set; }
        int? SucursalID { get; set; }
        Enum TipoAutorizacion { get; set; }
        string EmpleadoNombre { get; set; }
        int? EmpleadoID { get; set; }
        string Email { get; set; }
        string Telefono { get; set; }
        bool? Estatus { get; set; }
        bool? SoloNotificacion { get; set; }

        DateTime? FC { get; set; }
        DateTime? FUA { get; set; }
        int? UC { get; set; }
        int? UUA { get; set; }
        string UsuarioCreacion { set; }
        string UsuarioEdicion { set; }
        #endregion

        #region Métodos
        void PrepararVisualizacion();

        void EstablecerPaqueteNavegacion(string key, object value);
        object ObtenerPaqueteNavegacion(string key);
        void LimpiarPaqueteNavegacion(string key);

        void RedirigirAEditar();
        void RedirigirAConsulta();
        void RedirigirSinPermisoAcceso();

        void PermitirRegresar(bool permitir);
        void PermitirRegistrar(bool permitir);
        void PermitirEditar(bool permitir);

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion
    }
}
