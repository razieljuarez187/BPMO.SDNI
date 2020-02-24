//Satisface al CU089 - Bitácora de Llantas
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Equipos.VIS
{
	public interface IConsultarLlantaVIS
	{
		#region Propiedades

		bool? Activo { get; set; }

		String Codigo { get; set; }

		bool? EnStock { get; set; }

		int IndicePaginaResultado { get; set; }

		String Medida { get; set; }

		string NumeroSerie { get; set; }

		List<LlantaBO> Resultado { get; set; }

		bool? Revitalizada { get; set; }

		ETipoEnllantable? TipoEnllantable { get; set; }

		int? UnidadID { get; set; }

        int? SucursalID { get; set; }
        string SucursalNombre { get; set; }

        #region SC_0008

        int? UnidadOperativaID { get; }

		int? UsuarioID { get; }

		#endregion

		#endregion

		#region Métodos

		void ActualizarResultado();

		void EstablecerPaqueteNavegacion(string nombre, object valor);

		void LimpiarSession();

		void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

		void PrepararBusqueda();

		void RedirigirADetalles();

        void PermitirRegistrar(bool permitir); //SC_0008
        void RedirigirSinPermisoAcceso(); //SC_0008

		#endregion
	}
}