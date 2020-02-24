//Satisface al CU089 - Bitácora de llantas
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Equipos.VIS
{
	public interface IEliminarLlantaVIS
	{
		#region Propiedades

		int? LlantaID { get; }

		int CantidadArchivos { get; }

		string[] Extensiones { get; }

		List<TipoArchivoBO> TiposArchivos { get; set; }

		List<ArchivoBO> DocumentosAdjuntos { get; }

		DateTime? FC { get; }

		DateTime? FUA { get; }

		int? UC { get; }

		int? UUA { get; }

		LlantaBO UltimoObjetoLlanta { get; set; }

		int? UsuarioID { get; }

		int? UnidadOperativaID { get; }

		UsuarioBO Usuario { get; }

		List<DatosConexionBO> ListadoDatosConexion { get; }

		#endregion Propiedades

		#region Metodos

		void RedirigirDetalleLlanta();

		void InicializarControles(List<TipoArchivoBO> tiposArchivo);

		void MostrarMensaje(string mensaje, ETipoMensajeIU tipoMensaje, string msjDetalle = null);

		void EstablecerDatosNavegacion(string nombre, object valor);

		void RedirigirSinPermisoAcceso(); //SC_0008

		#endregion Metodos
	}
}