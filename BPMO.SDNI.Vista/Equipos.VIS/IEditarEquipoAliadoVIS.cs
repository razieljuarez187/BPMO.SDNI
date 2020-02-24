//Satisface al CU075 - Catálogo de Equipo Aliado
using System.Collections.Generic;
using BPMO.Basicos.BO;
namespace BPMO.SDNI.Equipos.VIS
{
    public interface IEditarEquipoAliadoVIS
    {
        #region Propiedades
        int? EquipoAliadoID { get; set; }

        #region REQ 13596

        List<CatalogoBaseBO> ListaAcciones { get; set; }

        #endregion
        #endregion

        #region Métodos
        object ObtenerDatosNavegacion();
		void RedirigirSinPermisoAcceso();
        void PrepararVista();
		void PermitirRegistrar(bool permitir);

	    #endregion




    }
}