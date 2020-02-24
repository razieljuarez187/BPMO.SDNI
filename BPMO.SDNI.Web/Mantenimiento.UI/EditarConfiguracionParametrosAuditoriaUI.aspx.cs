///Satisface el caso de uso CU070 - COnfigurar Parametros Auditoria
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.MapaSitio.UI;
using System.Configuration;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.VIS;

namespace BPMO.SDNI.Mantenimiento.UI
{
    public partial class EditarConfiguracionParametrosAuditoriaUI : System.Web.UI.Page, IEditarConfiguracionParametrosAuditoriaVIS
    {
        #region Atributos
        private EditarConfiguracionParametrosAuditoriaPRE presentador = null;
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

        #endregion

        #region Propiedades Interfaz

        private int? configuracionID;
        /// <summary>
        /// Identificador de la configuracion
        /// </summary>
        public int? ConfiguracionID
        {
            get
            {
                if (hdnConfiguracionID.Value != string.Empty)
                    configuracionID = int.Parse(hdnConfiguracionID.Value);
                return configuracionID;
            }
            set
            {
                if (value != null)
                    hdnConfiguracionID.Value = value.ToString();
                else
                    hdnConfiguracionID.Value = string.Empty;
            }
        }

        private string tipoMantenimiento;
        /// <summary>
        /// Tipo de servicio de la configuración
        /// </summary>
        public string TipoMantenimiento
        {
            get
            {
                if (txtTipoMantenimiento.Text != string.Empty)
                    tipoMantenimiento = txtTipoMantenimiento.Text;
                return tipoMantenimiento;
            }
            set
            {
                if (value != string.Empty)
                    txtTipoMantenimiento.Text = value;
                else
                    txtTipoMantenimiento.Text = string.Empty;
            }
        }

        private int? tipoMantenimientoID;
        /// <summary>
        /// Tipo de mantenimiento de la configuracion
        /// </summary>
        public int? TipoMantenimientoID
        {
            get
            {
                if (hdnTipoMantenimeinto.Value != string.Empty)
                    tipoMantenimientoID = int.Parse(hdnTallerID.Value);
                return tipoMantenimientoID;
            }
            set
            {
                if (value != null)
                    hdnTipoMantenimeinto.Value = value.ToString();
                else
                    hdnTipoMantenimeinto.Value = string.Empty;
            }
        }
        /// <summary>
        /// Identificador de la sucursal
        /// </summary>
        public int? SucursalID
        {
            get
            {
                int? sucursalID = null;
                if (!String.IsNullOrEmpty(this.hdnSucursalID.Value))
                    sucursalID = int.Parse(this.hdnSucursalID.Value.Trim());
                return sucursalID;
            }
            set
            {
                if (value != null)
                    this.hdnSucursalID.Value = value.ToString();
                else
                    this.hdnSucursalID.Value = string.Empty;
            }
        }
         /// <summary>
         /// Nombre de la sucursal
         /// </summary>
        public string SucursalNombre
        {
            get
            {
                String sucursalNombre = null;
                if (this.txtSucursal.Text.Trim().Length > 0)
                    sucursalNombre = this.txtSucursal.Text.Trim().ToUpper();
                return sucursalNombre;
            }
            set
            {
                if (value != null)
                    this.txtSucursal.Text = value.ToString();
                else
                    this.txtSucursal.Text = String.Empty;
            }
        }

        private string taller;
        /// <summary>
        /// nombre del taller
        /// </summary>
        public string Taller
        {
            get
            {
                if (txtTaller.Text != string.Empty)
                    taller = txtTaller.Text;
                return taller;
            }
            set
            {
                if (value != string.Empty)
                    txtTaller.Text = value;
                else
                    txtTaller.Text = string.Empty;
            }
        }

        private int? tallerID;
        /// <summary>
        /// Identificador del taller
        /// </summary>
        public int? TallerID
        {
            get
            {
                if (hdnTallerID.Value != string.Empty)
                    tallerID = int.Parse(hdnTallerID.Value);
                return tallerID;
            }
            set
            {
                if (value != null)
                    hdnTallerID.Value = value.ToString();
                else
                    hdnTallerID.Value = string.Empty;
            }
        }

        private string modelo;
        /// <summary>
        /// nombre del modelo
        /// </summary>
        public string Modelo
        {
            get
            {
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

        private int? modeloID;
        /// <summary>
        /// Identificador del modelo
        /// </summary>
        public int? ModeloID
        {
            get
            {
                if (hdnModeloID.Value != string.Empty)
                    modeloID = int.Parse(hdnModeloID.Value);
                return modeloID;
            }
            set
            {
                if (value != null)
                    hdnModeloID.Value = value.ToString();
                else
                    hdnModeloID.Value = string.Empty;
            }
        }

        private int? aleatorias;
        /// <summary>
        /// Numero de Actividades aleatorias
        /// </summary>
        public int? Aleatorias
        {
            get
            {
                if (txtAleatorias.Text != string.Empty)
                    aleatorias = int.Parse(txtAleatorias.Text);
                return aleatorias;
            }
            set
            {
                if (value != null)
                    txtAleatorias.Text = value.ToString();
                else
                    txtAleatorias.Text = string.Empty;
            }
        }

        private GridView gridActividadesAuditoria;
        /// <summary>
        /// Grid de las actividades de la auditoria
        /// </summary>
        public GridView GridActividadesAuditoria
        {
            get
            {
                gridActividadesAuditoria = gvActividades;
                return gridActividadesAuditoria;
            }
            set
            {
                gridActividadesAuditoria = value;

            }
        }

        private GridView gridConfiguracionesAuditoria;
        /// <summary>
        /// Grid con las configuraciones de la auditoria
        /// </summary>
        public GridView GridConfiguracionesAuditoria
        {
            get
            {
                gridConfiguracionesAuditoria = gvConfigurados;
                return gridConfiguracionesAuditoria;
            }
            set
            {
                gridConfiguracionesAuditoria = value;

            }
        }

        #endregion

        #region Varaibles Session

        public List<DetalleConfiguracionAuditoriaMantenimientoBO> ActividadesAuditoria
        {
            get
            {
                return Session["ActividadesAuditoria"] as List<DetalleConfiguracionAuditoriaMantenimientoBO>;
            }
            set
            {
                Session["ActividadesAuditoria"] = value;
            }
        }

        public List<ConfiguracionAuditoriaMantenimientoBO> ConfiguracionesAuditoria
        {
            get
            {
                return Session["ConfiguracionesAuditoria"] as List<ConfiguracionAuditoriaMantenimientoBO>;
            }
            set
            {
                Session["ConfiguracionesAuditoria"] = value;
            }
        }

        public ConfiguracionAuditoriaMantenimientoBO ConfiguracionRecibida
        {
            get
            {
                return Session["ConfiguracionRecibida"] as ConfiguracionAuditoriaMantenimientoBO;
            }
            set
            {
                Session["ConfiguracionRecibida"] = value;
            }
        }

        #endregion

        #endregion

        #region Constructores
        /// <summary>
        /// Inicializa con conponentes principales 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                nombreClase = this.GetType().Name;
                presentador = new EditarConfiguracionParametrosAuditoriaPRE(this);
                if (!IsPostBack)
                {
                    this.presentador.ValidarAcceso();

                    if (this.ConfiguracionRecibida != null)
                    {
                        presentador.mostrarDetalles();
                    }

                }
                if (this.ActividadesAuditoria == null)
                {
                    this.gvActividades.DataSource = new List<DetalleConfiguracionAuditoriaMantenimientoBO>();
                    this.gvActividades.DataBind();
                }
                else
                {
                    this.gvActividades.DataSource = this.ActividadesAuditoria;
                    this.gvActividades.DataBind();
                }
                if (this.ConfiguracionesAuditoria == null)
                {
                    this.gvConfigurados.DataSource = new List<ConfiguracionAuditoriaMantenimientoBO>();
                    this.gvConfigurados.DataBind();
                }
                else
                {
                    this.gvConfigurados.DataSource = this.ConfiguracionesAuditoria;
                    this.gvConfigurados.DataBind();
                }

               

            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la pagina", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load" + ex.Message);
            }
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

        #region Limpiar Session

        public void LimpiarSession()
        {
            Session.Remove("ActividadesAuditoria");
            Session.Remove("ConfiguracionesAuditoria");
            Session.Remove("ConfiguracionRecibida");

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
            //ListItem Select = new ListItem()
            //{
            //    Text = "--Seleccione--",
            //    Value = "0"
            //};
            //var responseEnumTipo = Enum.GetNames(typeof(ETipoMantenimiento)).Select(x => new { text = x, value = (int)Enum.Parse(typeof(ETipoMantenimiento), x) });
            //ddTipoMantenimiento.DataSource = responseEnumTipo;
            //ddTipoMantenimiento.DataTextField = "text";
            //ddTipoMantenimiento.DataValueField = "value";
            //ddTipoMantenimiento.DataBind();
            //ddTipoMantenimiento.Items.Add(Select);
            //ddTipoMantenimiento.SelectedValue = "0";
        }

        #endregion

        #endregion

        #region Eventos

        #region Eventos Grid

        #region Grid Actividades
        /// <summary>
        /// Controla los eventos de cambio de pagina del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActividade_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.GridActividadesAuditoria.PageIndex = e.NewPageIndex;
                this.presentador.respaldarSelecciones();
                this.GridActividadesAuditoria.DataSource = this.ActividadesAuditoria;
                this.GridActividadesAuditoria.DataBind();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".gvConfiguraciones_PageIndexChanging:" + ex.Message);
            }
        }

        /// <summary>
        /// Controla los evento cuando se enlaza las actividades con el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActividade_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var Actividad = ((DetalleConfiguracionAuditoriaMantenimientoBO)e.Row.DataItem);

                TextBox criterio = e.Row.FindControl("txbCriterioActividad") as TextBox;
                CheckBox obligatoria = e.Row.FindControl("chbxObligatoria") as CheckBox;
                GridViewRow row = e.Row;

                if (Actividad != null)
                {                  
                        criterio.Text = Actividad.Criterio;
                        if (Actividad.Obligatorio == true)
                            obligatoria.Checked = true;
                        else
                            obligatoria.Checked = false;
                }
            }
        }
        #endregion

        #region Grid Configurados
        /// <summary>
        /// Controla los eventos del cambio de pagina del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConfigurados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.gvConfigurados.PageIndex = e.NewPageIndex;
                this.gvConfigurados.DataSource = this.ConfiguracionesAuditoria;
                this.gvConfigurados.DataBind();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".gvConfiguraciones_PageIndexChanging:" + ex.Message);
            }

        }

        /// <summary>
        /// Controla los eventos de los botones del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConfigurados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index;

            try
            {
                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                {
                    index = Convert.ToInt32(e.CommandArgument.ToString()) + (((GridView)sender).PageIndex * ((GridView)sender).PageSize);
                    if (e.CommandName.Trim().Equals("Ver"))
                    {
                        this.presentador.respaldarSelecciones();
                        this.presentador.agregarGrid(this.ConfiguracionesAuditoria[index]);
                        this.ActividadesAuditoria = ConfiguracionesAuditoria[index].DetalleConfiguracion;
                        this.gvActividades.PageIndex = 0;
                        this.gvActividades.DataSource = this.ActividadesAuditoria;
                        this.gvActividades.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al ejecutar evento: ", ETipoMensajeIU.ERROR, ex.ToString());
            }

        }

        /// <summary>
        /// Controla los eventos cuando se enlaza el grid con las configuraciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConfigurados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var ConfiguracionAuditoria = ((ConfiguracionAuditoriaMantenimientoBO)e.Row.DataItem);

                Label obligatorias = e.Row.FindControl("lbActividadesObligatorias") as Label;
                Label aleatorias = e.Row.FindControl("lbActividadesAleatorias") as Label;

                if (ConfiguracionAuditoria != null)
                {
                    obligatorias.Text = ConfiguracionAuditoria.DetalleConfiguracion.Count(x => x.Obligatorio == true).ToString();
                    aleatorias.Text = ConfiguracionAuditoria.NumeroActividadesAleatorias.ToString();
                }
            }

        }
        #endregion

        #endregion

        /// <summary>
        /// Permite actualizar la configuracion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                var boActualizado = this.presentador.actualizarConfiguracion();
                LimpiarSession();
                boActualizado.Sucursal.Nombre = this.txtSucursal.Text;
                boActualizado.Taller.Nombre = this.txtTaller.Text;
                boActualizado.Modelo.Nombre = this.txtModelo.Text;
                Session["ConfiguracionRecibida"] = boActualizado;
                Response.Redirect("~/Mantenimiento.UI/DetalleConfiguracionParametrosAuditoriaUI.aspx");

            }
            catch (Exception ex)
            {

                this.MostrarMensaje("Error Al Actualizar: ", ETipoMensajeIU.ERROR, ex.ToString());
            }
        }

        /// <summary>
        /// Regresa a los detalles 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            var boActualizar = ConfiguracionesAuditoria.Find(x => x.ConfiguracionAuditoriaMantenimientoId == ConfiguracionID);
            boActualizar.Sucursal.Nombre = this.txtSucursal.Text;
            boActualizar.Taller.Nombre = this.txtTaller.Text;
            boActualizar.Modelo.Nombre = this.txtModelo.Text;
            //LimpiarSession();
            Session["ConfiguracionRecibida"] = boActualizar;

            Response.Redirect("~/Mantenimiento.UI/DetalleConfiguracionParametrosAuditoriaUI.aspx");
        }

        /// <summary>
        /// Controla las accciones del menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void menuSelecionado_MenuItemClick(object sender, MenuEventArgs e)
        {
            switch (e.Item.Value)
            {
                case "EliminarParametro":
                    try
                    {
                        string origen = ((Menu)sender).ID;

                        this.RegistrarScript("eliminar", "confirmarEliminacion('" + origen + "');");

                    }
                    catch (Exception ex)
                    {

                        MostrarMensaje("Error al Eliminar la configuracion : ", ETipoMensajeIU.ERROR, ex.ToString());
                    }

                    break;


            }

        }

        /// <summary>
        /// Permite eliminar una configuracion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            var boEliminado = this.presentador.eliminarConfiguracion();
            LimpiarSession();
            boEliminado.Taller.Nombre = this.txtTaller.Text;
            boEliminado.Modelo.Nombre = this.txtModelo.Text;
            boEliminado.Activo = false;
            Session["ConfiguracionRecibida"] = boEliminado;
            Response.Redirect("~/Mantenimiento.UI/DetalleConfiguracionParametrosAuditoriaUI.aspx");
        }

        #endregion

    }
}