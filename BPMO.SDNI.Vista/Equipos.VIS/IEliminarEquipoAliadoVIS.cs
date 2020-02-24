//Satisface al CU075 - Catálogo de Equipo Aliado
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Equipos.VIS
{
    public interface IEliminarEquipoAliadoVIS
    {
        #region Propiedades
        int? EquipoAliadoID { get; set; }

        #region REQ 13596

        List<CatalogoBaseBO> ListaAcciones { get; set; }

        #endregion
        #endregion

        #region Métodos
        void PrepararVista();

        object ObtenerDatosNavegacion();
	    void RedirigirSinPermisoAcceso();

	    #endregion
    }
}