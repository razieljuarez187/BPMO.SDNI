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

namespace BPMO.SDNI.Mantenimiento.UI
{
    public partial class EditarConfiguracionParametroMantenimientoUI : System.Web.UI.Page, IEditarConfiguracionParametroMantenimientoVIS
    {
        #region Atributos

        private EditarConfiguracionParametroMantenimientoPRE presentador = null;
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

        public int? ConfiguracionID
        {
            get
            {
                int? configuracionId = null;
                if (hdnConfiguracionID.Value != string.Empty)
                    configuracionId = int.Parse(hdnConfiguracionID.Value);
                return configuracionId = int.Parse(hdnConfiguracionID.Value);
                ;
            }
            set
            {
                if (value != null)
                    hdnConfiguracionID.Value = value.ToString();
                else
                    hdnConfiguracionID.Value = string.Empty;
            }
        }

        //public int? ConfiguracionID
        //{
        //    get
        //    {
        //        TextBox txtCodigo = this.mnuConfiguracionMantenimiento.Controls[0].FindControl("txtValue") as TextBox;
        //        int val = 0;
        //        if (!string.IsNullOrEmpty(txtCodigo.Text) && !string.IsNullOrWhiteSpace(txtCodigo.Text))
        //            if (Int32.TryParse(txtCodigo.Text, out val))
        //                return val;
        //        return null;
        //    }
        //    set
        //    {
        //        TextBox txtCodigo = this.mnuConfiguracionMantenimiento.Controls[0].FindControl("txtValue") as TextBox;
        //        if (value != null)
        //            txtCodigo.Text = value.Value.ToString();
        //        else
        //            txtCodigo.Text = string.Empty;
        //    }
        //}

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
        //        chbxEnUso.Checked = value;
        //    }
        //}

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

        private bool activo;

        public bool Activo
        {
            get
            {
                if (chbxActivo.Checked == true)
                    activo = true;
                else
                    activo = false;
                return activo;
            }
            set
            {
                chbxActivo.Checked = value;
            }
        }

        private bool navegacion;

        public bool Navegacion
        {
            get 
            {
                if (Request.QueryString["Eliminar"] != null)
                    navegacion = Convert.ToBoolean(Request.QueryString["Eliminar"]);
                return navegacion; 
            }
        }

        //private GridView gridConfiguracionesMantenimiento;

        //public GridView GridConfiguracionesMantenimiento
        //{
        //    get
        //    {
        //        gridConfiguracionesMantenimiento = gvConfiguraciones;
        //        return gridConfiguracionesMantenimiento;
        //    }
        //    set
        //    {
        //        gridConfiguracionesMantenimiento = value;

        //    }
        //}

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


        public ConfiguracionMantenimientoBO ConfiguracionRecibida
        {
            get
            {
                return Session["configuracionRecibida"] as ConfiguracionMantenimientoBO;
            }
            set
            {
                Session["configuracionRecibida"] = value;
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
                presentador = new EditarConfiguracionParametroMantenimientoPRE(this);

                if (!IsPostBack)
                {
                    this.presentador.ValidarAcceso();
                    this.enlazarControles();
                    String Valor = Request.QueryString["S"];
                    if (ConfiguracionRecibida != null && Valor == null)
                    {
                        this.lbEstado.Text = "EDITAR CONFIGURACIÓN MANTENIMIENTO";
                        this.presentador.ConsultarDetalle(ConfiguracionRecibida);
                        this.mnuConfiguracionMantenimiento.Items[0].Selected = true;
                        this.mnuConfiguracionMantenimiento.Items[1].Enabled = false;
                    }
                    else if (ConfiguracionRecibida != null && Valor == "1")
                    {
                        this.presentador.ConsultarDetalle(ConfiguracionRecibida); 
                        this.prepararEliminar();
                        this.lbEstado.Text = "ELIMINAR PARÁMETRO DE CONFIGURACIÓN";
                        this.mnuConfiguracionMantenimiento.Items[1].Selected = true;
                        this.mnuConfiguracionMantenimiento.Items[0].Enabled = false;
                    }
                }
              

            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la pagina", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load" + ex.Message);
            }


        }

        private void prepararEliminar()
        {
            ddEstado.Enabled = false;
            ddTipoMantenimiento.Enabled = false;
            ddUnidadMedida.Enabled = false;
            txtIntervalo.Enabled = false;

            chbxActivo.Enabled = true;
            chbxActivo.Checked = false;

            btnActualizar.Visible = false;
            //btnEliminar.Visible = false;
            btnConfirmEliminar.Visible = true;
            
        }

        #endregion

        #region Metodos



        #region Accesos y Seguridad

        /// <summary>
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        #endregion

        private void enlazarGrid()
        {
            //this.gvConfiguraciones.DataSource = configuraciones;
            //this.gvConfiguraciones.DataBind();
        }


        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

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
            //DesplegarListaConfiguraciones();
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
            //int index;

            //try
            //{
            //    if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
            //    {
            //        index = Convert.ToInt32(e.CommandArgument.ToString()) + (((GridView)sender).PageIndex * ((GridView)sender).PageSize);
            //        if (e.CommandName.Trim().Equals("Eliminar"))
            //        {
            //            this.configuraciones.Remove(configuraciones[index]);
            //            this.gvConfiguraciones.DataSource = this.configuraciones;
            //            this.gvConfiguraciones.DataBind();
            //        }
            //        if (e.CommandName.Trim().Equals("Editar"))
            //        {
            //            this.presentador.editarGrid(configuraciones[index]);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    this.MostrarMensaje("Error al ejecutar evento: ", ETipoMensajeIU.ERROR, ex.ToString());
            //}

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
            //DesplegarFormBusquedaModelo();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResult_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    switch (ViewState_Catalogo)
            //    {
            //        case ECatalogoBuscador.Modelo:
            //            this.DesplegarBOSelecto(ViewState_Catalogo);
            //            break;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click" + ex.Message);
            //}

        }

        protected void txtModelo_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (Modelo != string.Empty)
            //    {
            //        switch (ViewState_Catalogo)
            //        {
            //            case ECatalogoBuscador.Modelo:
            //                this.DesplegarBOSelecto(ViewState_Catalogo);
            //                break;
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click" + ex.Message);
            //}
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
                this.presentador.ActualizarConfiguracion();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al guardar los parametros de configuracion :", ETipoMensajeIU.ERROR, ex.ToString());
            }

        }

        #endregion

        protected void Button1_Click(object sender, EventArgs e)
        {

            //this.presentador.agregarAGrid();
        }

        #endregion  

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                int error = this.presentador.ActualizarConfiguracion();
                if (error == 0)
                {
                    Response.Redirect("~/Mantenimiento.UI/DetalleConfiguracionParametroMantenimientoUI.aspx");
                }
                else if (error == 1)
                {
                    this.MostrarMensaje("Debe agregar un intervalo", ETipoMensajeIU.ADVERTENCIA);
                }
                else if (error == 2)
                {
                    this.MostrarMensaje("Ya existe una configuracion para ese Tipo de Mantenimiento y Unidad de Medida", ETipoMensajeIU.ADVERTENCIA);
                }
                
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al actualizar", ETipoMensajeIU.ERROR, ex.ToString());
            }
            
        }

        protected void mnuConfiguracionMantenimiento_MenuItemClick(object sender, MenuEventArgs e)
        {
            switch (e.Item.Value)
            {
                case "EliminarParametro":
                        
                      this.prepararEliminar();
                      this.lbEstado.Text = "ELIMINAR PARAMETRO DE CONFIGURACION";
                        
                    
                 
                    break;


            }

        }

        protected void btnEliminarConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string origen = ((Button)sender).ID;

                this.RegistrarScript("eliminar", "confirmarEliminacion('" + origen + "');");
               
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al Eliminar", ETipoMensajeIU.ERROR, ex.ToString());
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            this.presentador.EliminarConfiguracion();
            this.ConfiguracionRecibida.Estatus = this.Activo;
            Response.Redirect("~/Mantenimiento.UI/DetalleConfiguracionParametroMantenimientoUI.aspx");
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    this.presentador.Regresar();
            Response.Redirect("~/Mantenimiento.UI/DetalleConfiguracionParametroMantenimientoUI.aspx");
            //}
            //catch (Exception ex)
            //{
            //    this.MostrarMensaje("Error al Eliminar", ETipoMensajeIU.ERROR, ex.ToString());
            //}
        }

        //protected void btnPrepararEliminar_Click(object sender, EventArgs e)
        //{
        //    this.prepararEliminar();
        //    this.lbEstado.Text = "ELIMINAR PARAMETRO DE CONFIGURACION";
        //}
    }
}