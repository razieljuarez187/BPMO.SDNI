//Satisface a la solicitud de Cambio SC0001
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS
{
    public interface IConsultarMasterFacturacionVIS
    {
        #region Propiedades
        int? UnidadOperativaID { get; }
        int? Ambiente { get; }
        int? UsuarioID { get; }
        int? ModuloID { get; }
        #endregion
        #region Metodos
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void RedirigirMaster(string urlMaster, string uoid, string sucid, string usuarioId, string ambiente);
        void RedirigirSinPermisoAcceso();
        #endregion
    }
}
