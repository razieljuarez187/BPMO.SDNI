//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using BPMO.SDNI.Tramites.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Tramites.VIS
{
    public interface IDetalleSeguroVIS
    {
        #region Propiedades
        int? TramiteID { get; }
        string NumeroPoliza { get; set; }
        string DescripcionTramitable { get; set; }
        string Codigo { get; }
        SeguroBO Seguro { get; set; }
        #region SC_0008
        int? UnidadOperativaId { get; }
        int? UsuarioId { get; }
        #endregion
        #endregion

        #region Métodos
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        object ObtenerDatosNavegacion();
        void LimpiarSesion();
        void EstablecerPaqueteNavegacion(string p);

        #region SC_0008
        void PermitirEditar(bool permitir);
        void RedirigirSinPermisoAcceso();
        #endregion
        #endregion
    }
}