//Satisface al caso de uso CU026 - Registrar Terminación de Contrato FSL
using System;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.FSL.VIS {
	public interface IucFinalizacionContratoFSLVIS {
		#region Propiedades
		string MotivoCierreAnticipado{get;set;}
		DateTime? FechaCierre { get; set; }
		string ObservacionesCierre { get; set; }
		decimal? Penalizacion { get; set; }
		DateTime? FechaFinContrato { get; set; }
		DateTime? FechaInicioContrato { get; set; }
		bool? ModoEdicion { get; set; }
		decimal? Mensualidad { get; set; }
		int? Plazo { get; set; }
		decimal? PorcentajePenalizacion { get; set; }

		#endregion

		#region Métodos
		void MostrarPenalizacion(bool mostrar);
		void MostrarMotivos(bool mostrar);
		void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
		void ConfigurarModoEdicion();
		void ConfigurarModoConsulta();
		void ObservacionObligatoria(bool obligatorio);

		#endregion
	}
}
