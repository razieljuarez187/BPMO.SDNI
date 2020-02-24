//Satisface al CU087 – Catálogo Tramite Unidad
using System.Collections.Generic;
using BPMO.SDNI.Equipos.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Tramites.VIS
{
    public interface IConsultarTramitesVIS
    {
        #region Propiedades
        List<UnidadBO> Tramitables { get; set; }
        string NumeroSerie { get; set; }
        int? TramitableID { get; set; }
        
        #region SC0008
        int? UsuarioId { get; }
        int? UnidadOperativaId { get; }
        #endregion
        #endregion

        #region Métodos
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void LimpiarSesion();
        void MostrarDatos();
        void RedirigirADetalles();
        void EstablecerPaqueteNavegacion(string nombre, object valor);

        void RedirigirSinPermisoAcceso(); //SC0008
        #endregion
    }
}
