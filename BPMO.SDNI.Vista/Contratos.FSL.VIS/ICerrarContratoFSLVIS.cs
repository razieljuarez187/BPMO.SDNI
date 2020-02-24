//Satisface al caso de uso CU026 - Registrar Terminación de Contrato Full Service Leasing
using System;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.FSL.BO;

namespace BPMO.SDNI.Contratos.FSL.VIS {
	public interface ICerrarContratoFSLVIS {

		#region Propiedades
		string Codigo { get; }
		string CodigoUltimoObjeto { get; set; }
		ContratoFSLBO UltimoObjeto { get; set; }
		int? ContratoID { get; set; }
		EEstatusContrato? Estatus { get; set; }
		int? UUA { get; }
		DateTime? FUA { get; }
		int? UnidadOperativaContratoID { get; set; }
		int? UsuarioID { get; }
		int? UnidadAdscripcionID { get; }
		CierreAnticipadoContratoFSLBO DatosCierre { get; }
		#endregion

		#region Métodos
		void LimpiarSesion();
		void CambiaAContrato();
		void CambiarALinea();
		void RegresarAConsultar();
		void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
		void IrADetalleContrato();
		void EstablecerPaqueteNavegacion(string clave, ContratoBO contrato);
		void RedirigirSinPermisoAcceso();
		void PermitirRegistrar(bool permitir);
		#endregion

	}
}