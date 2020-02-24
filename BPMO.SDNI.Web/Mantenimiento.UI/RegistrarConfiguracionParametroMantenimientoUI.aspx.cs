using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.MapaSitio.UI;
using System.Configuration;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Mantenimiento.UI
{
    public partial class RegistrarConfiguracionParametroMantenimientoUI: System.Web.UI.Page, IRegistrarConfiguracionParametroMantenimientoVIS
    {
        #region Atributos

        private RegistrarConfiguracionParametroMantenimientoPRE presentador = null;
        private string nombreClase;

        #endregion

        #region Propiedades

        #region propiedades idealise
        /// <summary>
        /// Usuario autenticado en el sistema
        /// </summary>
        public int? UsuarioAutenticado
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }
        /// <summary>
        /// identificador de la unidad operativa desde el cual se esta accesando
        /// </summary>
        public int? UnidadOperativaId
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }
        /// <summary>
        /// Identificador del Modulo desde el cual se esta accesando
        /// </summary>
        public int? ModuloID
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                    return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);

                return null;
            }
        }

        //public int? UC
        //{
        //    get
        //    {
        //        int? id = null;
        //        Site masterMsj = (Site)Page.Master;

        //        if (masterMsj.Usuario != null)
        //            id = masterMsj.Usuario.Id;
        //        return id;
        //    }
        //}


        #endregion

        #region Propiedades Interfaz


        private TextBox txbModelo;

        public TextBox TxbModelo
        {
            get
            {
                txbModelo = txtModelo;
                return txbModelo; 
            }
            set 
            { 
                txbModelo = value; 
            }
        }

        private ImageButton btnModelo;

        public ImageButton BtnModelo
        {
            get 
            {
                btnModelo = btnBuscarModelo;
                return btnModelo; 
            }
            set 
            { 
                btnModelo = value; 
            }
        }


        public string Modelo
        {
            get
            {
                string modelo = string.Empty;
                if (txtModelo.Text != string.Empty)
                    modelo = txtModelo.Text;
                return modelo;
            }
            set
            {
                if (value != string.Empty)
                    txtModelo.Text = value;
                else
                    txtModelo.Text = string.Empty;
            }
        }

        public int? ModeloID
        {
            get
            {
                int? modeloId = null;
                if (hdnModeloID.Value != string.Empty)
                    modeloId = int.Parse(hdnModeloID.Value);
                return modeloId;
            }
            set
            {
                if (value != null)
                    hdnModeloID.Value = value.ToString();
                else
                    hdnModeloID.Value = string.Empty;
            }
        }

        private ETipoMantenimiento? tipoMantenimiento;

        public ETipoMantenimiento? TipoMantenimiento
        {
            get
            {
                tipoMantenimiento = (ETipoMantenimiento)Enum.Parse(typeof(ETipoMantenimiento), ddTipoMantenimiento.SelectedValue.ToString());
                return tipoMantenimiento;
            }
            set
            {
                if (value != null)
                {
                    if (value == ETipoMantenimiento.PMA)
                        ddTipoMantenimiento.SelectedIndex = 0;
                    if (value == ETipoMantenimiento.PMB)
                        ddTipoMantenimiento.SelectedIndex = 1;
                    if (value == ETipoMantenimiento.PMC)
                        ddTipoMantenimiento.SelectedIndex = 2;
                        
                }
                else
                    ddTipoMantenimiento.SelectedValue.FirstOrDefault();
            }
        }

        private EUnidaMedida? unidadMendida;

        public EUnidaMedida? UnidadMedida
        {
            get
            {
                unidadMendida = (EUnidaMedida)Enum.Parse(typeof(EUnidaMedida), ddUnidadMedida.SelectedValue.ToString());
                return unidadMendida;
            }
            set
            {
                if (value != null)
                {
                    if (value == EUnidaMedida.Kilometros)
                        ddUnidadMedida.SelectedIndex = 0;
                    if (value == EUnidaMedida.Horas)
                        ddUnidadMedida.SelectedIndex = 1;
                    if (value == EUnidaMedida.Dias)
                        ddUnidadMedida.SelectedIndex = 2;
                }
                else
                    ddUnidadMedida.SelectedValue.FirstOrDefault();
            }
        }

        private bool enUso;

        public bool EnUso
        {
            get
            {
                if (ddEstado.SelectedValue == "EN USO")
                    enUso = true;
                else if (ddEstado.SelectedValue == "ESTACIONADO")
                    enUso = false;
                return enUso;
            }
            set
            {
                if (value == true)
                    ddEstado.SelectedValue = "EN USO";
                else if (value == false)
                    ddEstado.SelectedValue = "ESTACIONADO";
            }
        }

        //private bool enUso;

        //public bool EnUso
        //{
        //    get 
        //    {   
        //        if (chbxEnUso.Checked == true)
        //            enUso = true;
        //        else
        //            enUso = false;
        //        return enUso; 
        //    }
        //    set 
        //    {
        //            chbxEnUso.Checked = value; 
        //    }
        //}

        private int? intervalo;

        public int? Intervalo
        {
            get 
            {
                if (txtIntervalo.Text != string.Empty)
                    intervalo = int.Parse(txtIntervalo.Text);
                return intervalo; 
            }
            set 
            {
                if (value != null)
                    txtIntervalo.Text = value.ToString();
                else
                    txtIntervalo.Text = string.Empty;
            }
        }

        private GridView gridConfiguracionesMantenimiento;

        public GridView GridConfiguracionesMantenimiento
        {
            get
            {
                gridConfiguracionesMantenimiento = gvConfiguraciones;
                return gridConfiguracionesMantenimiento;
            }
            set
            {
                gridConfiguracionesMantenimiento = value;

            }
        }

        #endregion

        #region Varaibles Session


        public List<ConfiguracionMantenimientoBO> configuraciones
        {
            get
            {
                return Session["listaconfiguraciones"] as List<ConfiguracionMantenimientoBO>;
            }
            set
            {
                Session["listaconfiguraciones"] = value;
            }
        }

        #endregion

        #region Propiedades Buscador

        public enum ECatalogoBuscador
        {
            Modelo
        }

        public string LibroActivos
        {
            get
            {
                string valor = null;
                if (this.hdnLibroActivos.Value.Trim().Length > 0)
                    valor = this.hdnLibroActivos.Value.Trim().ToUpper();
                return valor;

            }
            set
            {
                if (value != null)
                    this.hdnLibroActivos.Value = value.ToString();
                else
                    this.hdnLibroActivos.Value = string.Empty;
            }
        }

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

        #region Constructores

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                nombreClase = this.GetType().Name;
                presentador = new RegistrarConfiguracionParametroMantenimientoPRE(this);

                if (!IsPostBack)
                {
                    this.presentador.ValidarAcceso();
                    this.enlazarControles();
                    this.configuraciones = new List<ConfiguracionMantenimientoBO>();
                    this.enlazarGrid();
                }
                this.enlazarGrid();

            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la pagina", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load" + ex.Message);
            }

           
        }

        #endregion

        #region Metodos

        private void enlazarGrid()
        {
            this.gvConfiguraciones.DataSource = configuraciones;            
            this.gvConfiguraciones.DataBind();
        }

        #region Accesos y Seguridad

        /// <summary>
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        #endregion

        #region DesplegarConfiguraciones
        //public void DesplegarListaConfiguraciones()
        //{
        //    this.gvConfiguraciones.DataSource = this.configuraciones;
        //    this.gvConfiguraciones.DataBind();
        //}
        #endregion

        #region Mensaje Sistema

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            Site masterMsj = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
            }
            else
            {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        #endregion

        #region Metodos Buscador
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="tipoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            try
            {
                ViewState_Catalogo = catalogoBusqueda;
                this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
                this.Session_BOSelecto = null;
                this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al consultar configuraciones", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            this.presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;
        }

        private void DesplegarFormBusquedaModelo()
        {
            try
            {
                this.EjecutaBuscador("Modelo&hidden=0", ECatalogoBuscador.Modelo);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar el Modelo", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnBuscarModelo_Click" + ex.Message);
            }
        }


        #endregion

        #region EnumToList

        private void enlazarControles()
        {
            var responseEnumTipo = Enum.GetNames(typeof(ETipoMantenimiento)).Select(x => new { text = x, value = (int)Enum.Parse(typeof(ETipoMantenimiento), x) });
            ddTipoMantenimiento.DataSource = responseEnumTipo;
            ddTipoMantenimiento.DataTextField = "text";
            ddTipoMantenimiento.DataValueField = "value";
            ddTipoMantenimiento.DataBind();

            var responseEnumUnidad = Enum.GetNames(typeof(EUnidaMedida)).Select(x => new { text = x, value = (int)Enum.Parse(typeof(EUnidaMedida), x) });
            ddUnidadMedida.DataSource = responseEnumUnidad;
            ddUnidadMedida.DataTextField = "text";
            ddUnidadMedida.DataValueField = "value";
            ddUnidadMedida.DataBind();

        }

        #endregion

        #region Limpiar Session

        public void LimpiarSession()
        {
            Session.Remove("listaconfiguraciones");
        }

        #endregion

        #endregion

        #region Eventos

        #region Eventos Grid

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConfiguraciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //try
            //{
            //    gvConfiguraciones.PageIndex = e.NewPageIndex;
            //    DesplegarListaConfiguraciones();
            //}
            //catch (Exception ex)
            //{
            //    this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".gvConfiguraciones_PageIndexChanging:" + ex.Message);
            //}

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConfiguraciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index;

            try
            {
                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                {
                    index = Convert.ToInt32(e.CommandArgument.ToString()) + (((GridView)sender).PageIndex * ((GridView)sender).PageSize);
                    if (e.CommandName.Trim().Equals("Eliminar"))
                    {
                        this.configuraciones.Remove(configuraciones[index]);
                        this.gvConfiguraciones.DataSource = this.configuraciones;
                        this.gvConfiguraciones.DataBind();
                    }
                    if (e.CommandName.Trim().Equals("Editar"))
                    {
                        this.presentador.editarGrid(configuraciones[index],index);
                    }
                }
            }
            catch (Exception ex) 
            {
                this.MostrarMensaje("Error al ejecutar evento: ",ETipoMensajeIU.ERROR,ex.ToString());
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConfiguraciones_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region Eventos de busqueda

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarModelo_Click(object sender, ImageClickEventArgs e)
        {
            DesplegarFormBusquedaModelo();
            //this.btnBuscarModelo.Enabled = false;
            //this.txtModelo.Enabled = false;

        }
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Modelo:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click" + ex.Message);
            }

        }

        protected void txtModelo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Modelo != string.Empty)
                {
                    switch (ViewState_Catalogo)
                    {
                        case ECatalogoBuscador.Modelo:
                            this.DesplegarBOSelecto(ViewState_Catalogo);
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click" + ex.Message);
            }
        }

        #endregion

        #region Guardar 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.configuraciones != null && this.configuraciones.Count > 0)
                {
                  int Error = this.presentador.GuardarConfiguracion();
                  if (Error == 0)
                  {
                      ConfiguracionMantenimientoBO bo = this.presentador.ConsultarInsertado();
                      if (bo.ConfiguracionMantenimientoId != null)
                      {
                          this.LimpiarSession();
                          Session["configuracionRecibida"] = bo;
                          Response.Redirect("~/Mantenimiento.UI/DetalleConfiguracionParametroMantenimientoUI.aspx");

                      }
                  }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al guardar los parametros de configuracion :", ETipoMensajeIU.ERROR, ex.ToString());
            }

        }

        #endregion

        protected void Button1_Click(object sender, EventArgs e)
        {

            this.presentador.agregarAGrid();
        }

        #endregion  

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.LimpiarSession();
            Response.Redirect("~/Mantenimiento.UI/ConsultarConfiguracionParametroMantenimientoUI.aspx");

        }

        protected void gvConfiguraciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var ConfiguracionMantenimiento = ((ConfiguracionMantenimientoBO)e.Row.DataItem);

                Label EnUso = e.Row.FindControl("lbEstaUso") as Label;

                if (ConfiguracionMantenimiento.EnUso == true)
                    EnUso.Text = "EN USO";
                else
                    EnUso.Text = "ESTACIONADO";

            }
        }

    }
}