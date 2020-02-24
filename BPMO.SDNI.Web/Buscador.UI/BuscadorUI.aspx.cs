//Satisface al CU062 - Menú Principal
//Satisface al CU068 - Catálogo de Clientes
//Satisface al CU075 - Catálogo de Equipo Aliado
//Satisface al CU079 - Consultar Acta de Nacimiento de Unidad
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Buscador.PRE;
using BPMO.Buscador.PRE;
using BPMO.Buscador.VIS;

namespace BPMO.SDNI.Buscador.UI {
    public partial class BuscadorUI : System.Web.UI.Page, IBuscadorVIS {
        #region Atributos
        private string rutaXML;
        private BuscadorPRE presentador = null;
        #endregion
        
        #region Propiedades
        /// <summary>
        /// Obtiene el listado de conexiones a la base de datos
        /// </summary>
        public System.Collections.Generic.List<BPMO.Basicos.BO.DatosConexionBO> ListadoDatosConexion {
            get { return new BuscadorIdealeasePRE().ObtenerDatosDeConexion(); }
        }
        /// <summary>
        /// Obtiene el nombre del ambiente en sesión
        /// </summary>
        private string Ambiente {
            get {
                return this.Session["EstiloCss"] != null ? this.Session["EstiloCss"].ToString() : null;
            }
        }
        /// <summary>
        /// Obtiene la ruta a los archivos CSS
        /// </summary>
        private string DireccionCSS {
            get {
                string s = this.ResolveUrl("~/Contenido/Estilos/");
                if (Session["ConfiguracionModuloSDNI"] != null && Session["ConfiguracionModuloSDNI"] is ConfiguracionModuloBO) {
                    string config = ((ConfiguracionModuloBO)Session["ConfiguracionModuloSDNI"]).DireccionCSS;
                    if (!string.IsNullOrEmpty(config) && !string.IsNullOrWhiteSpace(config))
                        return config;
                }
                return s;
            }
        }
        /// <summary>
        /// Obtiene o establece el KeySelecto
        /// </summary>
        /// <remarks>Contiene la llave del registro seleccionado</remarks>
        public string KeySelecto {
            get;
            set;
        }
        /// <summary>
        /// Establece el TituloBuscador
        /// </summary>
        public string TituloBuscador {
            set {
                this.lblTitulo.Text = value;
            }
        }
        /// <summary>
        /// Obtiene la RutaXML
        /// </summary>
        public string RutaXML {
            get {
                if (Request.QueryString["cfg"] != null)
                    rutaXML = MapPath("~/Contenido/XML/BPMO.SDNI.Buscador." + Request.QueryString["cfg"] + ".xml");
                else
                    rutaXML = null;
                return rutaXML;
            }
        }
        /// <summary>
        /// Obtiene el AutoOcultaUI
        /// </summary>
        public bool AutoOcultaUI {
            get {
                bool ocultar = true;
                if (Request.QueryString["hidden"] != null)
                    bool.TryParse(Request.QueryString["hidden"].ToString(), out ocultar);
                return ocultar;
            }
        }
        /// <summary>
        /// Obtiene o establece el ListadoObjetos
        /// </summary>
        public DataTable ListadoObjetos {
            get {
                return (this.Session[String.Format("RESULT_{0}", Nombre_Session_Guid)] != null) ? (DataTable)this.Session[String.Format("RESULT_{0}", Nombre_Session_Guid)] : null;
            }
            set
            {
                if (value != null) {
                    this.Session.Add(String.Format("RESULT_{0}", Nombre_Session_Guid), value);
                } else {
                    this.Session.Remove(String.Format("RESULT_{0}", Nombre_Session_Guid));
                }
                this.grdBuscador.DataSource = value;
                this.grdBuscador.DataBind();
            }
        }
        /// <summary>
        /// Obtiene o establece el ListadoObjetosBase
        /// </summary>
        public object[] ListadoObjetosBase {
            get {
                return (this.Session[String.Format("RESULT_BASE_{0}", Nombre_Session_Guid)] != null) ? (object[])this.Session[String.Format("RESULT_BASE_{0}", Nombre_Session_Guid)] : null;
            }
            set {
                if (value != null) {
                    this.Session.Add(String.Format("RESULT_BASE_{0}", Nombre_Session_Guid), value);
                } else {
                    this.Session.Remove(String.Format("RESULT_BASE_{0}", Nombre_Session_Guid));
                }
            }
        }
        /// <summary>
        /// Obtiene o establece si TieneMetodoSeleccion
        /// </summary>
        public bool TieneMetodoSeleccion {
            get { return ViewState["TieneMetodoSeleccion"] == null ? false : (bool)ViewState["TieneMetodoSeleccion"]; }
            set { ViewState["TieneMetodoSeleccion"] = value; }
        }
        /// <summary>
        /// Obtiene o establece si FiltraSobreConsulta
        /// </summary>
        public bool FiltrarSobreConsulta {
            get { return this.cbBuscaEnBD.Checked; }
            set { this.cbBuscaEnBD.Checked = value; }
        }
        /// <summary>
        /// Obtiene o establece si PermiteBusquedaVacia
        /// </summary>
        public bool PermiteBusquedaVacia {
            get { return ViewState["PermiteBusquedaVacia"] == null ? true : (bool)ViewState["PermiteBusquedaVacia"]; }
            set { ViewState["PermiteBusquedaVacia"] = value; }
        }
        /// <summary>
        /// Obtiene o establece si Convierte a mayusculas
        /// </summary>
        public bool ConvertirAMayusculas {
            get { return ViewState["ToUpper"] == null ? false : (bool)ViewState["ToUpper"]; }
            set { ViewState["ToUpper"] = value; }
        }
        /// <summary>
        /// Obtiene o establece el DsXML
        /// </summary>
        public DataSet DsXML {
            get {
                return (this.ViewState["DSXML"] != null) ? (DataSet)this.ViewState["DSXML"] : null;
            }
            set {
                this.ViewState.Add("DSXML", value);
            }
        }
        /// <summary>
        /// Obtiene o establece la OrdenListado
        /// </summary>
        public string OrdenListado {
            get {
                return (ViewState["SORT"] == null) ? "ASC" : (string)ViewState["SORT"];
            }
            set {
                ViewState.Add("SORT", value);
            }
        }
        /// <summary>
        /// Obtiene o establece el DatoLlave
        /// </summary>
        public string DatoLlave {
            get {
                return (ViewState["KEY"] != null) ? (string)ViewState["KEY"] : null;
            }
            set {
                ViewState.Add("KEY", value);
            }
        }
        /// <summary>
        /// Obtiene o establece los Filtros
        /// </summary>
        ///<remarks>Contiene duplas: Propiedad y Valor</remarks>
        public string Filtros {
            get {
                return (ViewState["FILTRO"] != null) ? (string)ViewState["FILTRO"] : null;
            }
            set {
                if (value == null)
                    ViewState.Remove("FILTRO");
                else
                    ViewState.Add("FILTRO", value);
            }
        }
        /// <summary>
        /// Obtiene o establece el FiltroBO
        /// </summary>
        ///<remarks>Contiene duplas: Propiedad y Valor</remarks>
        public object FiltroBO {
            get {
                if (Nombre_Session_Guid == null)
                    return null;
                return (this.Session[Nombre_Session_Guid] != null) ? (object)this.Session[Nombre_Session_Guid] : null;
            }
            set {
                if (value == null)
                    this.Session.Remove(Nombre_Session_Guid);
                else
                    this.Session.Add(Nombre_Session_Guid, value);
            }
        }
        /// <summary>
        /// Obtiene o establece el FiltroBO
        /// </summary>
        ///<remarks>Objeto seleccionado</remarks>
        public object BOSelecto {
            set {
                if (value == null)
                    this.Session.Remove(String.Format("BOSELECTO_{0}", Nombre_Session_Guid));
                else
                    this.Session.Add(String.Format("BOSELECTO_{0}", Nombre_Session_Guid), value);
            }
        }
        /// <summary>
        /// Obtiene o establece el Nombre_Session_Guid
        /// </summary>
        public string Nombre_Session_Guid {
            get {
                string guidBO = null;
                if (Request.QueryString["pktId"] != null)
                    guidBO = Request.QueryString["pktId"].ToString();
                return guidBO;
            }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Carga de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
            try {
                this.RegistrarEventoTxt();
                presentador = new BuscadorPRE(this);
                presentador.DefinirColumnas();
                if (!IsPostBack) {
                    presentador.CargarInformacion();
                    if (AutoOcultaUI && ListadoObjetos != null && ListadoObjetos.Rows.Count == 1) {
                        KeySelecto = ListadoObjetos.Rows[0][DatoLlave].ToString();
                        presentador.ObtenerObjetoSeleccionado(KeySelecto);
                        this.CerrarDialog(KeySelecto);
                    }
                }
            } catch (Exception ex) {
                MostrarMensaje("Existen parámetros inconsistentes en la Configuración proporcionada al Buscador.", ex.Message);
            }
        }
        /// <summary>
        /// Inicializa la configuración de la página
        /// </summary>
        /// <param name="sender">Parametro de tipo object</param>
        /// <param name="e">Parametro de tipo EventArgs</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            //Asignación de estilos con base en la configuración del módulo y el ambiente
            if (this.Ambiente == null)
                this.ltEstilo.Text = "<link href='" + this.DireccionCSS + "EstiloDesarrollo.css" + "' rel='Stylesheet' type='text/css'/>";
            else
                this.ltEstilo.Text = "<link rel='stylesheet' type='text/css' href='" + this.DireccionCSS + this.Ambiente + ".css" + "'/>";
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Generar las Columnas a Desplegar de acuerdo la configuración cargada
        /// </summary>
        /// <param name="dt">DataTable con la configuración de la columnas a mostrar</param>
        public void GenerarColumnasADesplegar(DataTable dt) {
            this.hdnFiltro.Value = string.Empty;
            this.grdHeader.Columns.Clear();
            this.grdBuscador.Columns.Clear();
            int width = 0;
            foreach (DataRow row in dt.Rows) {
                //Generar Cabecera
                TemplateField templateField = new TemplateField();
                templateField.HeaderTemplate = new GridViewHeaderTemplate(row);
                grdHeader.Columns.Add(templateField);
                width += int.Parse(row["Width"].ToString());
                templateField.HeaderStyle.Width = int.Parse(row["Width"].ToString());
                //Generar Contenido
                TemplateField tempField = new TemplateField();
                tempField.ShowHeader = true;
                tempField.HeaderText = row["Header"].ToString();
                tempField.SortExpression = row["NameProperty"].ToString();
                tempField.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                tempField.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                tempField.ItemStyle.Wrap = true;
                if (row.Table.Columns.Contains("DataType")) {
                    if (row["DataType"] != null && row["DataType"].ToString().Equals(DbType.Boolean.ToString()))
                        tempField.ItemTemplate = new GridViewItemTemplate(row, GridViewItemTemplate.ETemplateField.CheckBox);
                    else
                        tempField.ItemTemplate = new GridViewItemTemplate(row, GridViewItemTemplate.ETemplateField.Literal);
                } else {
                    tempField.ItemTemplate = new GridViewItemTemplate(row, GridViewItemTemplate.ETemplateField.Literal);
                }
                tempField.ItemStyle.Width = int.Parse(row["Width"].ToString());
                this.grdBuscador.Columns.Add(tempField);
                this.hdnFiltro.Value += row["NameProperty"].ToString() + "'" + row["Value"].ToString().Trim() + "'";
            }
            //Buscar la Propiedad marcada como Key
            DataRow rowKey = dt.Select("Key='True'").FirstOrDefault();
            if (rowKey == null)
                throw new ArgumentException("Es necesario marcar una Propiedad como clave.");
            DatoLlave = (rowKey).Field<string>("NameProperty");
            this.grdBuscador.DataKeyNames = new String[] { DatoLlave };
            //Agregar Header Imagen
            TemplateField templateImgHeader = new TemplateField();
            templateImgHeader.ItemStyle.Width = 18;
            this.grdHeader.Columns.Add(templateImgHeader);
            //Agregar Imagen de Selección
            TemplateField tempFieldImgBtn = new TemplateField();
            tempFieldImgBtn.ItemTemplate = new GridViewItemTemplate(rowKey, GridViewItemTemplate.ETemplateField.ImageButton);
            tempFieldImgBtn.ItemStyle.Width = 18;

            width += 18;
            this.grdBuscador.Width = width + 50;
            this.grdHeader.Width = width + 50;

            this.grdBuscador.Columns.Add(tempFieldImgBtn);
            this.grdBuscador.ShowHeaderWhenEmpty = true;
            this.grdHeader.ShowHeaderWhenEmpty = true;
            this.grdHeader.DataSource = dt.Select("NameProperty='n'");
            this.grdHeader.DataBind();
            if (!String.IsNullOrEmpty(this.Filtros)) {
                this.hdnFiltro.Value = this.Filtros;
            } else {
                this.Filtros = this.hdnFiltro.Value;
            }
        }

        /// <summary>
        /// Configuración del GridView
        /// </summary>
        /// <param name="dtGrid"></param>
        public void ConfigurarGridView(DataTable dtGrid) {
            if(dtGrid != null && dtGrid.Rows.Count > 0) {
                DataRow rowGrid = dtGrid.Rows[0];
                if (dtGrid.Columns.Contains("AllowPaging"))
                    this.grdBuscador.AllowPaging = (rowGrid["AllowPaging"].ToString().ToUpper() == "TRUE") ? true : false;
                else
                    this.grdBuscador.AllowPaging = true;
                if (dtGrid.Columns.Contains("AllowSorting"))
                    this.grdBuscador.AllowSorting = (rowGrid["AllowSorting"].ToString().ToUpper() == "TRUE") ? true : false;
                else
                    this.grdBuscador.AllowSorting = true;
                if (dtGrid.Columns.Contains("PageSize")) {
                    int pageSize = 0;
                    this.grdBuscador.PageSize = (int.TryParse(rowGrid["PageSize"].ToString(), out pageSize)) ? pageSize : 10;
                }else{
                     this.grdBuscador.PageSize = 10;
                }
            }
        }

        /// <summary>
        /// Desplegar mensaje
        /// </summary>
        /// <param name="mensaje">Mensaje</param>
        /// <param name="tipo">Tipo de mensaje</param>
        /// <param name="detalle">Detalle del mensaje</param>
        public void MostrarMensaje(string mensaje, string detalle = null, int? tipo = null) {
            if (!string.IsNullOrEmpty(detalle))
                detalle = detalle.Replace("\r", "").Replace("\n", "<br/>").Replace("\"", "").Replace("\'", "").Replace("-->", " ");
            mensaje = mensaje.Replace("\n", "<br/>");
            string script = @"$(function(){ " +
                                        "$(\"#divMsj\").css('display','block');" + "\n" +
                                        "$(\"#divMsj\").append(\"" + mensaje + " " + ((detalle != null) ? detalle : "") + "\");" + "\n" +
                                    "});";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msj", script, true);
        }

        /// <summary>
        /// Cerrar el Dialog
        /// </summary>
        /// <param name="valorRetorno">Valor retorno</param>
        private void CerrarDialog(string valorRetorno) {
            string script = @"$(function(){ closeParentUI(); });";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeWindow", script, true);
        }

        /// <summary>
        /// Configura los eventos por cada llamada realizada al servidor
        /// </summary>
        private void RegistrarEventoTxt() {
            string script = string.Empty;
            if (Page.Request.Form.AllKeys.Any(c => !string.IsNullOrEmpty(c) && c.ToLower().Contains("btnReiniciarFiltro".ToLower())))
                script = "LimpiarText(); EventTxt(); \n JLabelTxt(); \n EventImg();";
            else
                script = "EventTxt(); \n JLabelTxt(); \n EventImg();";
            this.Filtros = this.hdnFiltro.Value;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "Events", script, true);
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Disparador de la acción Búsqueda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string hdnFiltro = this.hdnFiltro.Value;
                Filtros = hdnFiltro;
                presentador.AplicarFiltro();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al aplicar los Filtros de Búsqueda. ", ex.Message);
            }
        }

        /// <summary>
        /// Disparador de la acción de Selección de un elemento de los resultados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                KeySelecto = this.hdnSelect.Value;
                presentador.ObtenerObjetoSeleccionado(KeySelecto);
                this.CerrarDialog(KeySelecto);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al seleccionar un elemento del resultado. ", ex.Message);
            }
        }

        /// <summary>
        /// Paginación de los registros encontrados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdBuscador_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grdBuscador.PageIndex = e.NewPageIndex;
                this.ListadoObjetos = ListadoObjetos;
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ex.Message);
            }
        }

        /// <summary>
        /// Aplicar Ordenamiento a los resultados de la búsqueda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdBuscador_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                presentador.OrdenarResultados(e.SortExpression, this.OrdenListado);
                this.OrdenListado = (this.OrdenListado.Equals("ASC")) ? "DESC" : "ASC";
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al aplicar ordenamiento a los resultados.", ex.Message);
            }
        }

        /// <summary>
        /// Reinicia la consulta en el buscador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReiniciarFiltro_Click(object sender, EventArgs e) {
            this.FiltrarSobreConsulta = false;
            this.ListadoObjetos = null;
        }
        #endregion
    }
}