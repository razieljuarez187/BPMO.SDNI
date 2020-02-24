//Satisface al CU089 - Bitácora de Llantas
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Equipos.VIS
{
    public interface IDetalleLlantaVIS
    {
        #region Propiedades

        int? LlantaID { get; set; }
        string Codigo { get; set; }
        string Marca { get; set; }
        string Modelo { get; set; }
        string Medida { get; set; }
        decimal? Profundidad { get; set; }
        bool? Revitalizada { get; set; }

        bool? Stock { get; set; }
        string DescripcionEnllantable { get; set; }

        List<ArchivoBO> ArchivosAdjuntos { get; set; }

        DateTime? FC { set; }
        DateTime? FUA { set; }
        int? UC { get; set; }
        int? UUA { get; set; }
        string UsuarioCreacion { set; }
        string UsuarioEdicion { set; }
        bool? Activo { get; set; }
	    bool Actualizada { get; }

        #region SC_0008
        int? UnidadOperativaID { get; } 
        int? UsuarioID { get; }
        #endregion

        int? SucursalID { get; set; }
        string SucursalNombre { get; set; }
        #endregion

        #region Métodos

        void Recargar();
        LlantaBO ObtenerDatosNavegacion();
        void EstablecerDatosNavegacion(string nombre, object valor);
        
        void LimpiarSesion();

        void PermitirEliminar(bool permitir);
        void PermitirEditar(bool permitir);
        void PermitirRegistrar(bool permitir); //SC_0008

        void RedirigirAEdicion();
        void RedirigirAPopUp();
        void RedirigirSinPermisoAcceso(); //SC_0008

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        #endregion
    }
}