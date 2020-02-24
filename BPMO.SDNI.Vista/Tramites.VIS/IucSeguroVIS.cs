//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Tramites.BO;

namespace BPMO.SDNI.Tramites.VIS
{
    public interface IucSeguroVIS
    {
        #region Propiedades
        int? UnidadOperativaId { get; }
        int? UsuarioId { get; }
        string VIN{  get;   set; }
        string Modelo { get; set; }
        string NumeroPoliza { get; set; }
        string Aseguradora { get; set; }
        string Contacto { get; set; }
        decimal? PrimaAnual { get; set; }
        decimal? PrimaSemestral { get; set; }
        DateTime? VigenciaInicial { get; set; }
        DateTime? VigenciaFinal { get; set; }
        string Observaciones { get; set; }
        bool? Activo { get; set; }
        DateTime? FC { get; }
        DateTime? FUA { get; }
        int? UC { get; }
        int? UUA { get; }
        int? TramitableID { get; set; }
        ETipoTramitable? TipoTramitable { get; set; }
        int? TramiteID { get; set; }
        ETipoTramite? TipoTramite { get; }
        SeguroBO UltimoObjeto { get; set; }
        int? Modo { get; set; }
        #endregion

        #region Métodos
        void PrepararNuevo();
        void PrepararEdicion();
        void PrepararVista();
        void LimpiarSesion();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        void Registrar();
        object ObtenerDatosNavegacion();
        object ObtenerDatosNavegacion(string key);
        void CargarObjeto();
        void CargarObjeto(string p);
        void IrADetalle();
        void Editar();
        void EstablecerPaqueteNavegacion(string p, int? tramiteID);
        void CambiarASoloLectura();
        #region CU0004

        void ModoRegistrar();
        void ModoEditar();

        #endregion

        #endregion
    }
}