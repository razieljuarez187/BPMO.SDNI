//Satiface al caso de uso CU098 - Configurar Autorizadores para los Contratos
using System;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Comun.VIS
{
    public interface IRegistrarAutorizadorVIS
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
        bool? Estatus { get; set; }
        bool? SoloNotificacion { get; set; }

        DateTime? FC { get; }
        int? UC { get; }
        DateTime? FUA { get; }
        int? UUA { get; }
        #endregion

        #region Métodos
        void PrepararNuevo();

        void EstablecerPaqueteNavegacion(string nombre, object valor);

        void RedirigirAConsulta();
        void RedirigirADetalles();
        void RedirigirSinPermisoAcceso();

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion
    }
}
