//Esta clase satisface los requerimientos especificados en el caso de uso CU006 Consultar Flota de Renta Diaria
//Esta clase satisface los requerimientos especificados en el caso de uso CU008 - Consultar Entrega Recepcion de Unidad
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Flota.VIS
{
    /// <summary>
    /// Vista de la página de detalle de unidad
    /// </summary>
    public interface IDetalleFlotaVIS
    {
        #region Propiedades
        UsuarioBO Usuario { get; }
        UnidadOperativaBO UnidadOperativa { get; }
        int? UnidadID { get; set; }
        int? EquipoID { get; set; }
        string NumeroEconomico { get; set; }
        string NumeroSerie { get; set; }
        #endregion

        #region Metodos
        void LimpiarSesion();
        void InicializarControles();
        object ObtenerDatosNavegacion();
        void EstablecerDatosNavegacion(string nombre, object valor);
        void RegresarAConsultar();
        void RedirigirSinPermisoAcceso();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void PermitirRegresar(bool permiso);//CU008
        object ObtenerFiltrosConsulta();//CU008
        #endregion
    }
}