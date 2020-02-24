//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Collections.Generic;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Tramites.VIS
{
    public interface IRegistrarTramitesVIS
    {
        #region Propiedades
        int? UnidadOperativaId { get; }
        int? UsuarioId { get; }
        List<TipoArchivoBO> TiposArchivo { get; set; }
        ETipoTramite? TipoTramite{get; set;}
        Object Tramite{get; set;}
        ITramitable Tramitable { get; set; }
        string NumSerie{get; set;}
        string Modelo { get; set; }
        string Marca { get; set; }
        DateTime? FC { get; }
        DateTime? FUA { get; }
        int? UC { get; }
        int? UUA { get; }

        #endregion

        #region Métodos

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void LimpiarSesionTramite();
        void LimpiarSesionTramitable();
        void RedirigirADetalle();
        void EstablecerPaqueteNavegacion(string nombre, object valor);
        #region SC0008
        void RedirigirSinPermisoAcceso();
        #endregion
        #endregion
    }
}
