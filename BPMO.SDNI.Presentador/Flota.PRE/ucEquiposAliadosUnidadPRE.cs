//Esta clase satisface los requerimientos especificados en el caso de uso CU006 Consultar Flota de Renta Diaria
using System.Collections.Generic;
using System.Globalization;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Flota.BO;
using BPMO.SDNI.Flota.VIS;

namespace BPMO.SDNI.Flota.PRE
{
    /// <summary>
    /// Presentador del user control de equipos aliados de la unidad
    /// </summary>
    public class ucEquiposAliadosUnidadPRE
    {
        #region Atributos
        /// <summary>
        /// Vista del UC de Equipos Aliados para la unidad
        /// </summary>
        private readonly IucEquiposAliadosUnidadVIS vistaEA;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor del user control de equipo aliados
        /// </summary>
        /// <param name="vista"></param>
        public ucEquiposAliadosUnidadPRE(IucEquiposAliadosUnidadVIS vista)
        {
            this.vistaEA = vista;
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Prepara el control para la vista inicial
        /// </summary>
        public void Inicializar()
        {
            this.vistaEA.EquipoID = string.Empty;
            this.vistaEA.EquiposAliados = new List<EquipoAliadoBO>();
            this.vistaEA.LiderID = string.Empty;
            this.vistaEA.NumeroEconomico = string.Empty;
            this.vistaEA.NumeroSerie = string.Empty;
            this.vistaEA.OracleID = string.Empty;
            this.vistaEA.UnidadID = string.Empty;
        }
        /// <summary>
        /// Presenta la información de un objeto de negocio en la vista
        /// </summary>
        /// <param name="obj">objeto que se desea desplegar en la pantalla</param>
        public void DatoAInterfazUsuario(object obj)
        {
            ElementoFlotaBO elemento = obj as ElementoFlotaBO;
            
            if(ReferenceEquals(elemento.Unidad, null))
                elemento.Unidad = new UnidadBO();

            UnidadBO unidad = elemento.Unidad;
            
            if(ReferenceEquals(unidad.EquiposAliados, null))
                unidad.EquiposAliados = new List<EquipoAliadoBO>();

            this.vistaEA.EquipoID = unidad.EquipoID.HasValue ? unidad.EquipoID.Value.ToString(CultureInfo.InvariantCulture).Trim().ToUpper() : string.Empty;
            this.vistaEA.LiderID = unidad.IDLider.HasValue ? unidad.IDLider.Value.ToString(CultureInfo.InvariantCulture).Trim().ToUpper() : string.Empty;
            this.vistaEA.NumeroEconomico = !string.IsNullOrEmpty(unidad.NumeroEconomico) && !string.IsNullOrWhiteSpace(unidad.NumeroEconomico)
                                               ? unidad.NumeroEconomico.Trim().ToUpper()
                                               : string.Empty;
            this.vistaEA.NumeroSerie = !string.IsNullOrEmpty(unidad.NumeroSerie) && !string.IsNullOrWhiteSpace(unidad.NumeroSerie)
                                           ? unidad.NumeroSerie.Trim().ToUpper()
                                           : string.Empty;
            this.vistaEA.OracleID = !string.IsNullOrEmpty(unidad.ClaveActivoOracle) && !string.IsNullOrWhiteSpace(unidad.ClaveActivoOracle)
                                        ? unidad.ClaveActivoOracle.Trim().ToUpper()
                                        : string.Empty;
            this.vistaEA.UnidadID = unidad.UnidadID.HasValue ? unidad.UnidadID.Value.ToString(CultureInfo.InvariantCulture).Trim().ToUpper() : string.Empty;

            this.vistaEA.EquiposAliados = unidad.EquiposAliados;
        }
        /// <summary>
        /// Cambia la página del grid de equipos aliados
        /// </summary>
        /// <param name="nuevoIndicePagina"></param>
        public void CambiarPaginaResultado(int nuevoIndicePagina)
        {
            this.vistaEA.IndicePaginaResultado = nuevoIndicePagina;
            this.vistaEA.ActualizarResultado();
        }
        /// <summary>
        /// Asigna los equipos aliados de la unidad al grid correspondiente
        /// </summary>
        public void CargarEquiposAliados()
        {
            if(ReferenceEquals(this.vistaEA.EquiposAliados, null))
                this.vistaEA.EquiposAliados = new List<EquipoAliadoBO>();

            this.vistaEA.CargarEquiposAliados(this.vistaEA.EquiposAliados);
        }
        #endregion
    }
}