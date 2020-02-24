//Satisface al CU030 - Reporte de Añejamiento de Flota
using System;
using System.Collections;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Flota.Reportes.PRE;
using BPMO.SDNI.Flota.Reportes.VIS;
using ReportPage = BPMO.SDNI.Reportes.UI.Page;

namespace BPMO.SDNI.Flota.Reportes.UI
{
    public partial class AniejamientoFlotaUI : ReportPage, IAniejamientoFlotaVIS
    {
        #region Constantes
        /// <summary>
        /// Nombre de la clase en curso
        /// </summary>
        private static readonly String NombreClase = typeof(AniejamientoFlotaUI).Name;
        #endregion

        #region Campos
        /// <summary>
        /// Presentador que esta atendiendo las peticiones de la vista
        /// </summary>
        private AniejamientoFlotaPRE presentador;
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
        /// Identificador del Modelo
        /// </summary>
        public int? ModeloID
        {
            get { return this.Master.ModeloID; }
            set { this.Master.ModeloID = value; }
        }

        /// <summary>
        /// Nombre del Modelo
        /// </summary>
        public string ModeloNombre 
        { 
            get { return this.Master.ModeloNombre; }
            set { this.Master.ModeloNombre = value; } 
        }

        /// <summary>
        /// Identificador de Cuenta Cliente
        /// </summary>
        public int? CuentaClienteID
        {
            get { return this.Master.CuentaClienteID; } 
            set { this.Master.CuentaClienteID = value; }
        }

        /// <summary>
        /// Nombre del Cliente
        /// </summary>
        public string ClienteNombre
        {
            get { return this.Master.ClienteNombre; } 
            set { this.Master.ClienteNombre = value; }
        }

        /// <summary>
        /// Identificador del Enumerador de Area/Departamento
        /// </summary>
        public int? Area
        {
            get
            {
                if(this.ddlDepartamento.SelectedIndex == -1)
                    return null;

                return Int32.Parse(this.ddlDepartamento.SelectedValue);
            }
            set
            {
                if(value == null)
                    this.ddlDepartamento.ClearSelection();
                else
                    this.ddlDepartamento.SelectedValue = ((int)value).ToString();
            }
        }

        /// <summary>
        /// Determina si el reporte a imprimir es detallado
        /// </summary>
        public bool? ReporteDetallado
        {
            get
            {
                if (this.ddlReporteDetallado.SelectedValue != "-1")
                {
                    if (this.ddlReporteDetallado.SelectedItem.Text == "SI")
                        return true;
                    if(this.ddlReporteDetallado.SelectedItem.Text == "NO")
                        return false;
                }

                return null;
            }
            set
            {
                this.ddlReporteDetallado.SelectedValue = value != null ? value.Value ? "1" : "0" : "1";
            }
        }

        /// <summary>
        /// Determina si es es por Unidad/EquipoAliado
        /// </summary>
        public int? TipoUnidad
        {
            get
            {
                if (String.IsNullOrEmpty(this.ddlTipoUnidad.SelectedValue))
                    return null;

                return Int32.Parse(this.ddlTipoUnidad.SelectedValue);
            }
            set
            {
                this.ddlTipoUnidad.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Etiqueta que sera colocada en el reporte
        /// </summary>
        public string EtiquetaReporte
        {
            get
            {
                if (!String.IsNullOrEmpty(this.txtEtiquetaReporte.Text) || !String.IsNullOrWhiteSpace(this.txtEtiquetaReporte.Text))
                    return this.txtEtiquetaReporte.Text;

                return null;
            }
            set
            {
                this.txtEtiquetaReporte.Text = String.IsNullOrEmpty(value) ? "" : value;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método para consultar el reporte
        /// </summary>
        public override void Consultar()
        {
            try
            {
                this.presentador.Consultar();
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al consultar el Reporte", ETipoMensajeIU.ERROR, NombreClase + ex.Message);
            }
        }
        /// <summary>
        /// Evento Iniciado con la página
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new AniejamientoFlotaPRE(this);
            if(!this.IsPostBack)
            {
                this.presentador.ValidarAcceso();
                //Se definen los filtros Visibles                
                this.Master.SucursalFiltroVisible = true;
                this.Master.ModeloFiltroVisible = true;
                this.Master.ClienteFiltroVisible = true;

                this.presentador.PrepararConsulta();
            }
        }
        /// <summary>
        /// Colocal los elementos de la lista en la interfaz.
        /// </summary>
        /// <param name="items">Lista de elementos que se agregaran</param>
        public void BindReporteDetallato(ICollection items)
        {
            this.ddlReporteDetallado.Items.Clear();
            ddlReporteDetallado.DataSource = items;
            ddlReporteDetallado.DataBind();

            ddlReporteDetallado.SelectedIndex = 1;
        }
        /// <summary>
        /// Colocal los elementos de la lista en la interfaz.
        /// </summary>
        /// <param name="items">Lista de elementos que se agregaran</param>
        public void BindTipoUnidad(ICollection items)
        {
            this.ddlTipoUnidad.Items.Clear();
            this.ddlTipoUnidad.DataSource = items;
            this.ddlTipoUnidad.DataBind();

            this.ddlTipoUnidad.SelectedIndex = 0;
        }
        /// <summary>
        /// Colocal los elementos de la lista en la interfaz
        /// </summary>
        /// <param name="items">Lista de elementos que se agregaran</param>
        public void BindArea(ICollection items)
        {
            this.ddlDepartamento.Items.Clear();
            this.ddlDepartamento.DataSource = items;
            this.ddlDepartamento.DataBind();

            this.ddlDepartamento.ClearSelection();
        }

        #endregion
    }
}