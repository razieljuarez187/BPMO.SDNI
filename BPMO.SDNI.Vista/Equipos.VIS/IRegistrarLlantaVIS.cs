//Satisface al CU089 - Bitácora de Llantas
using System;
using BPMO.Primitivos.Enumeradores;


namespace BPMO.SDNI.Equipos.VIS
{
    public interface IRegistrarLlantaVIS
    {
        #region Propiedades
        string Codigo { get; set; }
        string Marca { get; set; }
        string Modelo { get; set; }
        string Medida { get; set; }
        decimal? Profundidad { get; set; }
        bool? Revitalizada { get; set; }

        DateTime? FC { get; }
        DateTime? FUA { get; }
        int? UC { get; }
        int? UUA { get; }

        bool? Activo { get; }
        bool? Stock { get; }
		int? UsuarioID { get; }
		int? UnidadOperativaID { get; }
        int? SucursalID { get; set; }
        string SucursalNombre { get; set; }
        #endregion

        #region Métodos
        void EstablecerPaqueteNavegacion(string nombre, object valor);

        void RedirigirAConsulta();
        void RedirigirADetalles();
        void RedirigirSinPermisoAcceso(); //SC_0008

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
	    #endregion
    }
}
