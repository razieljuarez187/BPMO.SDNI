//Satisface al caso de uso CU068 - Catálogo de Clientes
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Comun.VIS
{
	public interface IucDatosObligadoSolidarioVIS
	{
		#region Propiedades

		ActaConstitutivaBO ActaConstitutiva { get; set; }

		string Direccion { get; set; }

		string Nombre { get; set; }

		int? ObligadoID { get; set; }

		List<RepresentanteLegalBO> RepresentantesInactivos { get; set; }

		List<RepresentanteLegalBO> RepresentantesLegales { get; set; }

		string Telefono { get; set; }

		ETipoObligadoSolidario? TipoObligadoSolidario { get; set; }

        List<CatalogoBaseBO> ListaAcciones { get; set; }

        int? UnidadOperativaId { get; set; }

        string RFC { get; set; }

		#endregion Propiedades

		#region Métodos

		void ActualizarRepresentantesLegales();

		void HabilitarCampos(bool habilitar);

		void ModoCreacion();

		void ModoEdicion();

		void MostrarDatos(ObligadoSolidarioBO obligado);

		ObligadoSolidarioBO ObtenerDatos();

		void PrepararNuevo();

		string ValidarActaConstitutiva(bool? validarEscritura=true, bool? ObligadoSolidario = false);

		string ValidarDatos();

        void EstablecerAcciones(bool mostrar);

	    #endregion Métodos
	}
}