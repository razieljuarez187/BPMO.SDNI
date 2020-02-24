using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.Mantenimiento.VIS;
using System.Configuration;

namespace BPMO.SDNI.Mantenimiento.UI
{
    public partial class RegistrarConfiguracionParametrosAuditoriaUI : System.Web.UI.Page, IRegistrarConfiguracionParametrosAuditoriaVIS
    {
        #region Atributos
        private RegistrarConfiguracionParametrosAuditoriaPRE presentador = null;
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
                        ddTipoMantenimiento.SelectedIndex = 1;
                    if (value == ETipoMantenimiento.PMB)
                        ddTipoMantenimiento.SelectedIndex = 2;
                    if (value == ETipoMantenimiento.PMC)
                        ddTipoMantenimiento.SelectedIndex = 3;

                }
                else
                    ddTipoMantenimiento.SelectedValue.FirstOrDefault();
            }
        }

        private int? aleatorias;

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

        #endregion

        #endregion

        #region Constructores
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                nombreClase = this.GetType().Name;
                presentador = new RegistrarConfiguracionParametrosAuditoriaPRE(this, this.ucConfiguracionParametrosMantenimientoUI1);

                if (!IsPostBack)
                {
                    this.presentador.ValidarAcceso();
                    this.LimpiarSession();
                    this.enlazarControles();
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
            ListItem Select = new ListItem()
            {
                Text = "--Seleccione--",
                Value = "0"
            };
            var responseEnumTipo = Enum.GetNames(typeof(ETipoMantenimiento)).Select(x => new { text = x, value = (int)Enum.Parse(typeof(ETipoMantenimiento), x) });
            ddTipoMantenimiento.DataSource = responseEnumTipo;          
            ddTipoMantenimiento.DataTextField = "text";
            ddTipoMantenimiento.DataValueField = "value";
            ddTipoMantenimiento.DataBind();
            ddTipoMantenimiento.Items.Insert(0,Select);
            ddTipoMantenimiento.SelectedValue = "0";
        }

        #endregion

        #endregion

        #region Eventos

        #region Eventos Grid

        #region Grid Actividades
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

        protected void gvActividade_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvActividade_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var Actividad = ((DetalleConfiguracionAuditoriaMantenimientoBO)e.Row.DataItem);

                TextBox criterio = e.Row.FindControl("txbCriterioActividad") as TextBox;
                CheckBox obligatoria = e.Row.FindControl("chbxObligatoria") as CheckBox;

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
        protected void gvConfigurados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index;

            try
            {
                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                {
                    index = Convert.ToInt32(e.CommandArgument.ToString()) + (((GridView)sender).PageIndex * ((GridView)sender).PageSize);
                    if (e.CommandName.Trim().Equals("Eliminar"))
                    {
                        this.ConfiguracionesAuditoria.Remove(ConfiguracionesAuditoria[index]);
                        this.gvConfigurados.DataSource = this.ConfiguracionesAuditoria;
                        this.gvConfigurados.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al ejecutar el evento: ", ETipoMensajeIU.ERROR, ex.ToString());
            }

        }

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

        #region BtnRegistrar
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConfiguracionesAuditoria != null && ConfiguracionesAuditoria.Count > 0)
                {
                    int exito = presentador.guardarConfiguraciones();
                    if (exito == 1)
                    {
                        ConfiguracionAuditoriaMantenimientoBO bo = presentador.consultarGuardado();
                        this.LimpiarSession();
                        Session["ConfiguracionRecibida"] = bo;
                        Response.Redirect("~/Mantenimiento.UI/DetalleConfiguracionParametrosAuditoriaUI.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al guardar las configuraciones", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        #endregion

        #region BtnCancelar
        protected void btnPrepararCancelar_Click(object sender, EventArgs e)
        {
            this.LimpiarSession();
            Response.Redirect("~/Mantenimiento.UI/ConsultarConfiguracionParametrosAuditoriaUI.aspx");
        }
        #endregion

        protected void AgregarConfiguracion_Click(object sender, EventArgs e)
        {
            this.presentador.agregarGrid();
        }

        protected void ddTipoMantenimiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddTipoMantenimiento.SelectedValue != "0")
                {
                    this.presentador.consultarActividadesPM();
                }

            }
            catch (Exception ex)
            {

                this.MostrarMensaje("Error al buscar las actividades :", ETipoMensajeIU.ERROR, ex.ToString());
            }

        }

        #endregion

       
             
    }
}