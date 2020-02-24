//Esta clase satisface los requerimientos especificados en el caso de uso CU006 Consultar Flota de Renta Diaria
using System;
using System.Collections.Generic;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Flota.VIS;

namespace BPMO.SDNI.Flota.UI
{
    public partial class ucEquiposAliadosUnidadUI : System.Web.UI.UserControl, IucEquiposAliadosUnidadVIS
    {
        #region Propiedades
        /// <summary>
        /// Obtiene o establece el identificador de la unidad desplegada
        /// </summary>
        public string UnidadID
        {
            get { return this.hdnUnidadID.Value.Trim().ToUpper(); }
            set { this.hdnUnidadID.Value = value; }
        }
        /// <summary>
        /// Obtiene o establece el identificador del equipo que se despliega
        /// </summary>
        public string EquipoID
        {
            get { return this.hdnEquipoID.Value.Trim().ToUpper(); }
            set { this.hdnEquipoID.Value = value; }
        }
        /// <summary>
        /// Obtiene o establece el identificador de lider de la unidad desplegada
        /// </summary>
        public string LiderID
        {
            get { return this.hdnLiderID.Value.Trim().ToUpper(); }
            set { this.hdnLiderID.Value = value; }
        }
        /// <summary>
        /// Obtiene o establece el identificador de oracle de la unidad desplegada
        /// </summary>
        public string OracleID
        {
            get { return this.hdnOracleID.Value.Trim().ToUpper(); }
            set { this.hdnOracleID.Value = value; }
        }
        /// <summary>
        /// Obtiene o establece el número economico de la unidad desplegada
        /// </summary>
        public string NumeroEconomico
        {
            get { return this.hdnNumeroEconomico.Value.Trim().ToUpper(); }
            set { this.hdnNumeroEconomico.Value = value; }
        }
        /// <summary>
        /// Obtiene o establece el número de serie de la unidad desplegada
        /// </summary>
        public string NumeroSerie
        {
            get { return this.hdnNumeroSerie.Value.Trim().ToUpper(); }
            set { this.hdnNumeroSerie.Value = value; }
        }
        /// <summary>
        /// Obtiene o establece el listado de los equipos aliados de la unidad desplegada
        /// </summary>
        public List<EquipoAliadoBO> EquiposAliados
        {
            get { return (List<EquipoAliadoBO>) Session["EquiposAliadosUnidad"] ?? new List<EquipoAliadoBO>(); }
            set { Session["EquiposAliadosUnidad"] = value; }
        }
        /// <summary>
        /// Obtiene o establece el indice de la página del grid
        /// </summary>
        public int IndicePaginaResultado
        {
            get { return this.grdEquiposAliados.PageIndex; }
            set { this.grdEquiposAliados.PageIndex = value; }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Carga los equipos aliados en el grid de la Interfaz
        /// </summary>
        /// <param name="equipos">Equipos aliados pertenencientes a la unidad</param>
        public void CargarEquiposAliados(List<EquipoAliadoBO> equipos)
        {
            this.grdEquiposAliados.DataSource = equipos;
            this.grdEquiposAliados.DataBind();
        }
        /// <summary>
        /// Actualiza lis resultados en el grid de equipos aliados
        /// </summary>
        public void ActualizarResultado()
        {
            this.grdEquiposAliados.DataSource = this.EquiposAliados;
            this.grdEquiposAliados.DataBind();
        }

        public void CambiarEtiquetas(string FT)
        {
            if(this.grdEquiposAliados.Rows.Count > 0)
                this.grdEquiposAliados.HeaderRow.Cells[4].Text = FT;
        }
        #endregion
        
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion
    }
}