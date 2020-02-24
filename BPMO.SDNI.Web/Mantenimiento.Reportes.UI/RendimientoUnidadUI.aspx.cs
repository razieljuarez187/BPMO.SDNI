//Satisface al caso de uso CU060 - Reporte de Rendimiento de Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.Reportes.VIS;
using BPMO.SDNI.Mantenimientos.Reportes.PRE;
using ReportPage = BPMO.SDNI.Reportes.UI.Page;
using BPMO.Primitivos.Enumeradores;
using System.Collections;

namespace BPMO.SDNI.Mantenimiento.Reportes.UI
{
    public partial class RendimientoUnidadUI : ReportPage, IReporteRendimientoUnidadVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase en curso
        /// </summary>
        private static readonly String NombreClase = typeof(RendimientoUnidadUI).Name;
        /// <summary>
        /// Presentador que esta atendiendo las peticiones de la vista
        /// </summary>
        private ReporteRendimientoUnidadPRE presentador;
        #endregion
        #region Propiedades
        /// <summary>
        /// Identificador de la Sucursal
        /// </summary>
        public int? SucursalID
        {
            get { return this.Master.SucursalID; }
            set { this.Master.SucursalID = value; }
        }

        /// <summary>
        /// Nombre de la sucursal
        /// </summary>
        public string SucursalNombre
        {
            get { return this.Master.SucursalNombre; }
            set { this.Master.SucursalNombre = value; }
        }

        /// <summary>
        /// Identificador de la Uniad
        /// </summary>
        public int? UnidadID
        {
            get { return this.Master.UnidadID; }
            set { this.Master.UnidadID = value; }
        }

        /// <summary>
        /// Identificador del cliente de la unidad
        /// </summary>
        public int? ClienteID
        {
            get { return this.Master.ClienteID; }
            set { this.Master.ClienteID = value; }
        }

        /// <summary>
        /// Area de la Unidad
        /// </summary>
        public EArea? AreaUnidad
        {
            get { return this.Master.AreaUnidad; }
            set { this.Master.AreaUnidad = value; }
        }

        /// <summary>
        /// Serie de la Unidad
        /// </summary>
        public string VIN
        {
            get { return this.Master.NumeroSerie; }
            set { this.Master.NumeroSerie = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el año
        /// </summary>
        public int? Anio
        {
            get { return this.Master.Anio; }
            set { this.Master.Anio = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes
        /// </summary>
        public int? Mes
        {
            get { return this.Master.Mes; }
            set { this.Master.Mes = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes que se aplicará al reporte
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        public int? MesFinal
        {
            get
            {
                if (this.ddlMesFinal.SelectedIndex == -1 || this.ddlMesFinal.SelectedValue == "-1")
                    return null;

                return Convert.ToInt32(this.ddlMesFinal.SelectedValue);
            }
            set
            {
                this.ddlMesFinal.ClearSelection();
                if (value != null)
                {
                    ListItem item = this.ddlMesFinal.Items.FindByValue(value.ToString());
                    if (item != null)
                        item.Selected = true;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de periodo es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool MesFinalFiltroVisible
        {
            get { return this.trMesFinal.Visible; }
            set
            {
                if (this.trMesFinal.Visible != value)
                {
                    this.trMesFinal.Visible = value;
                    if (this.trMesFinal.Visible)
                        this.FillMeses();
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de periodo es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool MesFinalFiltroRequerido
        {
            get { return this.rfvtxtMesFinal.Visible; }
            set
            {
                if (this.rfvtxtMesFinal.Visible != value)
                {
                    this.rfvtxtMesFinal.Visible = value;
                    this.lblMesFinalRequired.Visible = value;
                    this.FillMeses();
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes que se aplicará al reporte
        /// </summary>
        public bool? ReporteGlobal
        {
            get
            {
                if (this.ddlReporteGlobal.SelectedIndex == -1 || this.ddlReporteGlobal.SelectedValue == "-1")
                    return null;

                return this.ddlReporteGlobal.SelectedValue == "1" ? true : false;
            }
            set
            {
                this.ddlReporteGlobal.ClearSelection();
                if (value != null)
                {
                    ListItem item = this.ddlReporteGlobal.Items.FindByValue(value.ToString());
                    if (item != null)
                        item.Selected = true;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de periodo es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool ReporteGlobalFiltroVisible
        {
            get { return this.trReporteGlobal.Visible; }
            set
            {
                if (this.trReporteGlobal.Visible != value)
                {
                    this.trReporteGlobal.Visible = value;
                    if (this.trReporteGlobal.Visible)
                        this.FillReporteGlobal();
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de periodo es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool ReporteGlobalFiltroRequerido
        {
            get { return this.rfvReporteGlobal.Visible; }
            set
            {
                if (this.rfvReporteGlobal.Visible != value)
                {
                    this.rfvReporteGlobal.Visible = value;
                    this.lblReporteGlobalRequired.Visible = value;
                    this.FillReporteGlobal();
                }
            }
        }
        #endregion
        #region Constructor
        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new ReporteRendimientoUnidadPRE(this);
            if (!this.IsPostBack)
            {
                this.presentador.ValidarAcceso();
                //Se definen los filtros Visibles                
                this.Master.SucursalFiltroVisible = true;
                this.Master.AnioFiltroVisible = true;
                this.Master.AnioFiltroRequerido = true;
                this.Master.MesFiltroVisible = true;
                this.Master.MesEtiqueta = "MES INICIAL";
                this.Master.UnidadFiltroVisible = true;
                this.MesFinalFiltroVisible = true;
                this.MesFinalFiltroRequerido = true;
                this.ReporteGlobalFiltroVisible = true;
                this.ReporteGlobalFiltroRequerido = true;
                this.Master.ClienteFiltroVisible = true;
                this.Master.ClienteFiltroRequerido = false;
                this.Master.AreaUnidadFiltroVisible = true;
                this.Master.AreaUnidadFiltroRequerido = false;

                this.presentador.PrepararConsulta();
            }
        } 
        #endregion
        #region Metodos
        /// <summary>
        /// Método para consultar el reporte
        /// </summary>
        public override void Consultar()
        {
            try
            {
                this.presentador.Consultar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al consultar el Reporte", ETipoMensajeIU.ERROR, NombreClase + ex.Message);
            }
        }
        /// <summary>
        /// Carga los meses del reporte al control asignado
        /// </summary>
        protected virtual void FillMeses()
        {
            this.presentador.BinMesFinal();
        }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void FillReporteGlobal()
        {
            this.presentador.BindReporteGlobal();
        }
        /// <summary>
        /// Liga los meses del reporte al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        public void BindMesFinal(ICollection items)
        {
            this.ddlMesFinal.Items.Clear();
            this.ddlMesFinal.DataSource = items;
            this.ddlMesFinal.DataBind();

            this.ddlMesFinal.ClearSelection();
            if (this.MesFinalFiltroRequerido)
                this.ddlMesFinal.SelectedValue = DateTime.Now.Month.ToString();
        }
        /// <summary>
        /// Liga los meses del reporte al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        public void BindTipoReporte(ICollection items)
        {
            this.ddlReporteGlobal.Items.Clear();
            this.ddlReporteGlobal.DataSource = items;
            this.ddlReporteGlobal.DataBind();

            this.ddlReporteGlobal.ClearSelection();
            if (this.ReporteGlobalFiltroRequerido)
                this.ddlReporteGlobal.SelectedValue = "0";
        }
        #endregion
    }
}