// Satisface al CU015 - Registrar Contrato Full Service Leasing
// Satisface al CU022 - Consultar Contratos Full Service Leasing
// Satisface al CU023 - Editar Contratos Full Service Leasing
using System.Collections.Generic;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Contratos.FSL.VIS
{
    public interface IucCargosAdicionalesEquiposAliadosVIS
    {
        #region Propiedades
        int? Plazo { get; set; }

        int? UnidadID { get; set; }

        int? UnidadOperativaID { get; set; }

        List<EquipoAliadoBO> EquipoAliados { get; set; }

        ETipoCotizacion? TipoCotizacion { get; set; }
        
        #endregion

        #region Metodos
        List<CargoAdicionalEquipoAliadoBO> ObtenerCargosAdicionales();
        void EstablecerCargosAdicionales(List<CargoAdicionalEquipoAliadoBO> cargosAdicionales);

        void InicializarControl(int? plazo, ETipoCotizacion? tipoCotizacion, List<EquipoAliadoBO> equiposAliados, int? unidadID, int? UnidadOperativaID);
        void InicializarControl(int? plazo, ETipoCotizacion? tipoCotizacion, List<CargoAdicionalEquipoAliadoBO> cargos, int? unidadID, int? unidadOperativaID);

        void ConfigurarModoConsultar();
        void ConfigurarModoEditar();

        void LimpiarSesion();
        #endregion
    }
}
