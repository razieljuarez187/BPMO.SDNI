// Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al caso de uso CU023 - Editar Contrato Full Service Leasing 
// Satisface al caso de uso CU015 - Registrar Contrato Full Service Leasing

using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;

namespace BPMO.SDNI.Contratos.FSL.VIS
{
	public interface IucClienteContratoVIS
	{
		#region Propiedades
		UnidadOperativaBO UnidadOperativa { get; }

		List<RepresentanteLegalBO> RepresentantesLegalesContrato { get; set; }
		List<ObligadoSolidarioBO> ObligadosSolidariosContrato { get; set; }

		string NombreCuentaCliente { get; set; }
		int? CuentaClienteID { get; set; }
		int? ClienteID { get; set; }

		List<RepresentanteLegalBO> ListadoRepresentantesLegales { get; set; }
		RepresentanteLegalBO RepresentanteLegalSeleccionado { get; }
		List<RepresentanteLegalBO> RepresentantesObligado { get; set; }
		List<ObligadoSolidarioBO> ListadoObligadosSolidarios { get; set; }
		ObligadoSolidarioBO ObligadoSolidarioSeleccionado { get; }

		bool ModoEditar { get; }
		#region SC0001
		string Calle { get; set; }
		string Direccion { get; set; }
		string CodigoPostal { get; set; }
		string Ciudad { get; set; }
		string Estado { get; set; }
		string Municipio { get; set; }
		string Pais { get; set; }
		string Colonia { get; set; }
		

		#endregion

        #region SC0007
        bool? EsFisico { get; set; }
        bool? SoloRepresentantes { get; set; }
        #endregion

        /// <summary>
        /// Numero de Cuenta de Oracle CuentaClienteBO.Numero
        /// </summary>
        String ClienteNumeroCuenta { get; set; }
		bool? ObligadosComoAvales { get; set; }
		List<AvalBO> AvalesTotales { get; set; }
		List<AvalBO> AvalesSeleccionados { get; set; }
		int? AvalSeleccionadoID { get; }
		List<RepresentanteLegalBO> RepresentantesAvalTotales { get; set; }
		List<RepresentanteLegalBO> RepresentantesAvalSeleccionados { get; set; }
		int? RepresentanteAvalSeleccionadoID { get; }
        int? DireccionClienteID { get; set; }
        #endregion

        #region Metodos

        void HabilitarListadoObligadosSolidarios(bool habilitar);

		void HabilitarAgregarObligadoSolidario(bool habilitar);

		void HabilitarListadoRepresentantesLegales(bool habilitar);

		void HabilitarAgregarRepresentanteLegal(bool habilitar);

		void HabilitarConsultaDireccionCliente(bool habilitar);
		void ConfigurarModoConsultar();

		void ConfigurarModoEditar();

		void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

		void LimpiarSesion();

        #region SC0005
        void MostrarDetalleObligado(List<RepresentanteLegalBO> representantes);
        void MostrarRepresentantesObligados(bool p);
        #endregion

        #region SC0007
        void HabilitarSoloRepresentantes(bool habilitar);
        void MostrarObligadosSolidarios(bool p);
        void ConfigurarClienteMoral();
        void ConfigurarClienteFisico();
        #endregion

		void PermitirSeleccionarAvales(bool permitir);
		void PermitirAgregarAvales(bool permitir);
		void MostrarAvales(bool mostrar);
		void MostrarRepresentantesAval(bool mostrar);
		void MostrarDetalleRepresentantesAval(List<RepresentanteLegalBO> representantes);
		void ActualizarAvales();
		void HabilitarObligadosComoAvales(bool habilitar);

		#endregion
	}
}