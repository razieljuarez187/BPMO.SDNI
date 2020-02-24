//Satisface al caso de uso PLEN.BEP.15.MODMTTO.CU017.Imprimir.Calcomania.Mantenimiento
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ReportPage = BPMO.SDNI.Reportes.UI.Page;
using BPMO.SDNI.Mantenimiento.Reportes.VIS;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Mantenimiento.Reportes.PRE;
using System.Configuration;
using System.Data;
using BPMO.SDNI.Contratos.BO;

namespace BPMO.SDNI.Mantenimiento.Reportes.UI
{
    public partial class CalcomaniaMantenimientoUI : ReportPage, IConsultarCalcomaniaVIS
    {
        #region Atributos

        /// <summary>
        /// Controlador del reporte de consulta de calcomanías
        /// </summary>
        private ConsultarCalcomaniaPRE presentador = null;
        private ReporteCalcomaniaPRE preReports = null;

        /// <summary>
        /// Enumerador para el buscador
        /// </summary>
        public enum ECatalogoBuscador
        {
            Unidad,
            UnidadIdealease,
            Sucursal,
            Cliente
        }
        #endregion

        #region Constantes
        /// <summary>
        /// Nombre de la clase en uso
        /// </summary>
        //private static readonly String nombreClase = typeof(CalcomaniaMantenimiento).Name;
        private string nombreClase = "CalcomaniaMantenimiento";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase en uso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ConsultarCalcomaniaPRE(this);
                preReports = new ReporteCalcomaniaPRE(this);

                if (!this.IsPostBack)
                {
                    presentador.ValidarAcceso();
                    preReports.ValidarAcceso();

                    this.Master.SucursalFiltroVisible = true;
                    this.Master.ClienteFiltroVisible = true;
                    this.Master.UnidadEtiqueta = "VIN";
                    this.Master.UnidadFiltroVisible = true;
                    this.Master.DepartamentoFiltroVisible = true;
                    this.presentador.PrepararBusqueda();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
                
            }
        }
        #endregion

        #region Propiedades

        /// <summary>
        /// Identificador del uuario auntentcado
        /// </summary>
        public int? UsuarioAutenticado
        {
            get
            {
                int? id = null;
                id = this.Master.UsuarioID;
                return id;
            }
        }

        /// <summary>
        /// Identificador de la unidad operativa
        /// </summary>
        public int? UnidadOperativaId
        {
            get
            {
                return this.Master.UnidadOperativaID;
            }
        }

        public int? ModuloID
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                    return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);

                return null;
            }
        }

        /// <summary>
        /// Libros activos
        /// </summary>
        public string LibroActivos
        {
            get
            {
                string valor = null;
                if (this.hdnLibroActivos.Value.Trim().Length > 0)
                {
                    valor = this.hdnLibroActivos.Value.Trim().ToUpper();
                }
                return valor;
            }
            set
            {
                if (value != null)
                {
                    this.hdnLibroActivos.Value = value.ToString();
                }
                else
                {
                    this.hdnLibroActivos.Value = string.Empty;
                }
            }
        }

        /// <summary>
        /// VIN/serie
        /// </summary>
        public string NumeroVIN
        {
            get
            {
                return this.Master.NumeroSerie;
            }
            set
            {
                this.Master.NumeroSerie = value;
            }
        }

        public string SucursalNombre
        {
            get
            {
                return this.Master.SucursalNombre;
            }
            set
            {
                this.Master.SucursalNombre = value;
            }
        }

        /// <summary>
        /// Identificador de la sucursal
        /// </summary>
        public int? SucursalID
        {
            get
            {
                return this.Master.SucursalID;

            }
            set
            {

                this.Master.SucursalID = value;

            }
        }

        /// <summary>
        /// Número económico
        /// </summary>
        public string NumeroEconomico
        {
            get
            {
                String numeroEconomico = null;
                if (this.txtNumeroEconomico.Text.Trim().Length > 0)
                    numeroEconomico = this.txtNumeroEconomico.Text.Trim().ToUpper();
                return numeroEconomico;
            }
            set
            {
                if (value != null)
                    this.txtNumeroEconomico.Text = value.ToString();
                else
                    this.txtNumeroEconomico.Text = String.Empty;
            }
        }

        /// <summary>
        /// Identificador del cliente
        /// </summary>
        public int? ClienteID
        {
            get
            {
                return this.Master.ClienteID;
            }
            set
            {
                this.Master.ClienteID = value;
            }
        }

        /// <summary>
        /// Nombre del cliente
        /// </summary>
        public string ClienteNombre
        {
            get
            {
                return this.Master.ClienteNombre;
            }
            set
            {
                this.Master.ClienteNombre = value;
            }
        }

        /// <summary>
        /// GridView Unidades
        /// </summary>
        public GridView GvUnidadesCtes
        {
            get { return this.gvUnidades; }
        }

        /// <summary>
        /// índice de la página de resultado
        /// </summary>
        public int IndicePaginaResultado
        {
            get
            {
                return this.gvUnidades.PageIndex;
            }
            set
            {
                this.gvUnidades.PageIndex = value;
            }
        }

        /// <summary>
        /// Departamento o tipo de contrato
        /// </summary>
        public ETipoContrato? Departamento
        {
            get
            {
                return this.Master.Departamento;
            }
            set
            {
                this.Master.Departamento = value;
            }
        }

        /// <summary>
        /// Index
        /// </summary>
        public int Index
        {
            get
            {
                return Int32.Parse(Session["indexMantenimientoSeleccionado"].ToString());
            }
            set
            {
                Session["indexMantenimientoSeleccionado"] = value;
            }
        }

        /// <summary>
        /// DataSet resultado
        /// </summary>
        public DataSet Resultado
        {
            get { return Session["listUnidades"] != null ? Session["listUnidades"] as DataSet : null; }
            set { Session["listUnidades"] = value; }
        }

        #region Propiedades para el Buscador

        /// <summary>
        /// ViewState Guid
        /// </summary>
        public string ViewState_Guid
        {
            get
            {
                if (ViewState["GuidSession"] == null)
                {
                    Guid guid = Guid.NewGuid();
                    ViewState["GuidSession"] = guid.ToString();
                }
                return ViewState["GuidSession"].ToString();
            }
        }

        //variable de Sesión BOSelecto
        protected object Session_BOSelecto
        {
            get
            {
                object objeto = null;
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (Session[nombreSession] != null)
                    objeto = (Session[nombreSession] as object);

                return objeto;
            }
            set
            {
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (value != null)
                    Session[nombreSession] = value;
                else
                    Session.Remove(nombreSession);
            }
        }

        /// <summary>
        /// Variable de Sesión del buscador
        /// </summary>
        protected object Session_ObjetoBuscador
        {
            get
            {
                object objeto = null;
                if (Session[ViewState_Guid] != null)
                    objeto = (Session[ViewState_Guid] as object);

                return objeto;
            }
            set
            {
                if (value != null)
                    Session[ViewState_Guid] = value;
                else
                    Session.Remove(ViewState_Guid);
            }
        }

        /// <summary>
        /// Enumerator del cátalogo para el buscador
        /// </summary>
        public ECatalogoBuscador ViewState_Catalogo
        {
            get
            {
                return (ECatalogoBuscador)ViewState["BUSQUEDA"];
            }
            set
            {
                ViewState["BUSQUEDA"] = value;
            }
        }
        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método que realiza la consulta de las calcomanías
        /// </summary>
        public override void Consultar()
        {
            try
            {
                this.presentador.Consultar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }
        }

        /// <summary>
        /// Prepara los campos de búsqueda
        /// </summary>
        public void PrepararBusqueda()
        {
            this.txtNumeroEconomico.Text = "";
        }

        /// <summary>
        /// Método para desplegar los mensajes de notificación
        /// </summary>
        /// <param name="mensaje"></param>
        /// <param name="tipo"></param>
        /// <param name="msjDetalle"></param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            this.Master.MostrarMensaje(mensaje, tipo, msjDetalle);
        }

        /// <summary>
        /// Método que limpia las variables de sesión
        /// </summary>
        public void LimpiarSesion()
        {
            if (Session["listUnidades"] != null)
                Session.Remove("listUnidades");
        }

        //Método que establece los datos del GridView
        public void ActualizarResultado()
        {
            this.gvUnidades.DataSource = this.Resultado;
            this.gvUnidades.DataBind();
        }

        /// <summary>
        /// Realiza la redirección a la página de advertencia por falta de permisos.
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        #region Métodos para el buscador

        /// <summary>
        /// Ejecuta el buscador general
        /// </summary>
        /// <param name="catalogo"></param>
        /// <param name="catalogoBusqueda"></param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscarUnidad('" + ViewState_Guid + "','" + catalogo + "', '" + this.btnResult2.ClientID + "');");
        }

        /// <summary>
        /// Desplega el resultado de la búsqueda
        /// </summary>
        /// <param name="catalogo"></param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;
        }

        /// <summary>
        /// Método para registrar ScriptStartup
        /// </summary>
        /// <param name="key"></param>
        /// <param name="script"></param>
        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        #endregion

        #endregion

        #region Eventos

        /// <summary>
        /// Evento que se dipara cuando tecleo en la caja de texto del VIN
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtNumVin_TextChanged(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Evento clic del botón Buscar VIN
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarVin_Click(object sender, ImageClickEventArgs e)
        {
            
        }

        /// <summary>
        /// Evento PageIndexChanging del GridView gvUnidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.presentador.CambiarPaginaResultado(e.NewPageIndex);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".grvLlantas_PageIndexChanging:" + ex.Message);
            }
        }

        /// <summary>
        /// Evento RowCommand del GridView gvUnidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidades_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int item = Convert.ToInt32(e.CommandArgument.ToString());
                switch (e.CommandName.Trim())
                {
                    case "Detalles":
                        string OrdenServicio = gvUnidades.Rows[item].Cells[0].Text;
                        this.preReports.Consultar(OrdenServicio);
                        
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al ejecutar Reporte de Calcomanías", ETipoMensajeIU.ERROR, this.nombreClase + ".grvUnidades_RowCommand:" + ex.Message);
            }
        }

        /// <summary>
        /// Evento clic del botón BuscarNumEconomico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarNumEconomico_Click(object sender, ImageClickEventArgs e)
        {
            if (txtNumeroEconomico.Text.Length < 1)
            {
                this.MostrarMensaje("Es necesario ingresar el número ecoómico de la unidad", ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            try
            {
                this.EjecutaBuscador("UnidadIdealease&hidden=0", ECatalogoBuscador.UnidadIdealease);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar unidad", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnBuscarNumEconomico_Click" + ex.Message);
            }
        }

        /// <summary>
        /// Evento TextChanged de a caja de texto txtNumeroEconomico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtNumeroEconomico_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string NoEconomico = this.NumeroEconomico;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.UnidadIdealease);

                NumeroEconomico = NoEconomico;
                if (NumeroEconomico != null)
                {
                    this.EjecutaBuscador("UnidadIdealease", ECatalogoBuscador.UnidadIdealease);
                    NumeroEconomico = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtNumeroEconomico_TextChanged" + ex.Message);
            }
        }

        /// <summary>
        /// Evento cli del botón del buscador general
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResult2_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.UnidadIdealease:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click:" + ex.Message);
            }
        }

        #endregion

    }
}