//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Tramites.VIS
{
    public interface IEditarSeguroVIS
    {
        #region Propiedades
        string NumeroPoliza { get; set; }
        string DescripcionTramitable { get; set; }
        #region SC_0008
        int? UnidadOperativaId { get; }
        int? UsuarioId { get; }
        #endregion
        #endregion

        #region Métodos
        void LimpiarSesion();

        object ObtenerDatosNavegacion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        void IrADetalle();

        void EstablecerPaqueteNavegacion(string key, int? id);

        void LimpiarSesionEditar();

        #region SC0008
        void RedirigirSinPermisoAcceso();
        #endregion
        #endregion
    }
}