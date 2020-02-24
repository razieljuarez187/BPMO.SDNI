//Esta clase satisface los requerimientos especificados en el caso de uso CU006 Consultar Flota de Renta Diaria
using System.Collections.Generic;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Flota.VIS
{
    /// <summary>
    /// Vista del user control de equipos aliados de la unidad
    /// </summary>
    public interface IucEquiposAliadosUnidadVIS
    {
        #region Propiedades
        string UnidadID { get; set; }
        string EquipoID { get; set; }
        string LiderID { get; set; }
        string OracleID { get; set; }
        string NumeroEconomico { get; set; }
        string NumeroSerie { get; set; } 
        List<EquipoAliadoBO> EquiposAliados { get; set; }
        int IndicePaginaResultado { get; set; }
        #endregion

        #region Métodos
        void CargarEquiposAliados(List<EquipoAliadoBO> equipos);
        void ActualizarResultado();
        #endregion
    }
}