// Satisface al Caso de uso CU015 - Registrar Contrato Full Service Leasing
// Satisface al Caso de uso CU022 - Consultar Contrato Full Service Leasing
using System.Collections.Generic;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.VIS;

namespace BPMO.SDNI.Contratos.FSL.PRE
{
    public class ucCargosAdicionalesEquiposAliadosPRE
    {
        #region Atributos
        private readonly IucCargosAdicionalesEquiposAliadosVIS vista;
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor del Presentador
        /// </summary>
        /// <param name="cargosAdiciones">Vista de los cargos adicionales de los equipos aliados</param>
        public ucCargosAdicionalesEquiposAliadosPRE(IucCargosAdicionalesEquiposAliadosVIS cargosAdiciones)
        {
            vista = cargosAdiciones;
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Inicializa el control
        /// </summary>
        public void Inicializar()
        {
            vista.Plazo = null;
            vista.TipoCotizacion = null;
            vista.UnidadID = null;
            vista.UnidadOperativaID = null;
            vista.EquipoAliados = null;
        }

        /// <summary>
        /// Inicializa el control
        /// </summary>
        /// <param name="plazo">plazo del contrato en años</param>
        /// <param name="tipoCotizacion">tipo de cotización para los cargos adicionales</param>
        /// <param name="equiposAliados">Equipos Aliados de la Unidad a agregar al contrato</param>
        /// <param name="unidadID">Identificador de la Unidad a Agregar al contrato</param>
        /// <param name="unidadOperativaID">Identificador de la Unidad Operativa</param>
        public void Inicializar(int? plazo, ETipoCotizacion? tipoCotizacion, List<Equipos.BO.EquipoAliadoBO> equiposAliados, int? unidadID, int? unidadOperativaID)
        {

            vista.Plazo = plazo;
            vista.TipoCotizacion = tipoCotizacion;
            vista.UnidadOperativaID = unidadOperativaID;
            vista.UnidadID = unidadID;
            vista.EquipoAliados = equiposAliados;
        }
        #endregion
    }
}