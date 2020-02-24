//Satisface al caso de uso CU068 - Catálogo de Clientes
using BPMO.SDNI.Comun.BO;
using BPMO.Basicos.BO;
using System.Collections.Generic;

namespace BPMO.SDNI.Comun.VIS
{
	public interface IucDatosRepresentanteLegalVIS
	{
		#region Propiedades

	    List<CatalogoBaseBO> ListaAcciones { get; set; }

		ActaConstitutivaBO ActaConstitutiva { get; set; }

		bool? Depositario { get; set; }

		string Direccion { get; set; }

		string Nombre { get; set; }

		int? RepresentanteID { get; set; }

		string Telefono { get; set; }

        int? UnidadOperatiaId { get; set; }

	    string RFC { get; set; }

		#endregion Propiedades

		#region Metodos

		void HabilitarCampos(bool habilitar, bool? veractivo=true);

	    void EstablecerAcciones(List<CatalogoBaseBO> lstAcciones, bool mostrar);

		#region SC0005

		void MostrarDepositario(bool mostrar);

		#endregion SC0005

		#region SC0007

        string ValidarActaConstitutiva(bool? validarfc = false, bool? representanteLegal = null);

		#endregion SC0007

		#endregion Metodos
	}
}