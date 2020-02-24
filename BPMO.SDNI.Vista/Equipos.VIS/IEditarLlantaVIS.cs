//Satisface al CU089 - Bitácora de Llantas
using System;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Equipos.VIS
{
	public interface IEditarLlantaVIS
	{
		#region Propiedades

		bool? Activo { get; set; }

		string Codigo { get; set; }

		LlantaBO DatosNavegacion { get; set; }

		int? EnllantableID { get; set; }

		bool? EsRefaccion { get; set; }

		DateTime? FC { get; set; }

		DateTime? FUA { get; }

		int? LlantaID { get; set; }

		string Marca { get; set; }

		string Medida { get; set; }

		string Modelo { get; set; }

		int? Posicion { get; set; }

		decimal? Profundidad { get; set; }

		bool? Revitalizada { get; set; }

		bool? Stock { get; set; }

		int? TipoEnllantable { get; set; }

		int? UC { get; set; }

		LlantaBO UltimoObjetoLlanta { get; set; }

		int? UUA { get; }

        int? SucursalID { get; set; }
        string SucursalNombre { get; set; }

        #region SC008

        int? UnidadOperativaID { get; }

		int? UsuarioID { get; }

		#endregion SC008

		#endregion Propiedades

		#region Métodos

		void LimpiarSesion();

		void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        void PrepararEdicion();

        void PermitirRegistrar(bool permitir); //SC_0008

		void RedirigirAConsulta();
        void RedirigirADetalle();
        void RedirigirSinPermisoAcceso(); //SC_0008

		#endregion
	}
}