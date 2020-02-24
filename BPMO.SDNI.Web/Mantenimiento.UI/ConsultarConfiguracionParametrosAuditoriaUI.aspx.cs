///Satisface el caso de uso CU070 - Configurar Parametros Auditoria
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Mantenimiento.BO;
using System.Configuration;

namespace BPMO.SDNI.Mantenimiento.UI
{
    public partial class ConsultarConfiguracionParametrosAuditoriaUI : System.Web.UI.Page, IConsultarConfiguracionParametrosAuditoriaVIS
    {
        #region Atributos
        private ConsultarConfiguracionParametrosAuditoriaPRE presentador = null;
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
        /// <summary>
        /// Establece si la configuracion esta activa 
        /// </summary>
        public bool? Estatus
        {
            get
            {
                bool? estatus = null;
                if (this.ddlEstatus.SelectedIndex == 1 )
                    estatus = true;
                if (this.ddlEstatus.SelectedIndex == 2)
                    estatus = false;
                return estatus;
            }
            set
            {
                if (value != null)
                    this.ddlEstatus.SelectedValue = value.ToString();
                else
                    this.ddlEstatus.SelectedIndex = 0;
            }
        }

        private GridView gridConfiguiraciones;
        /// <summary>
        /// GridView de las configuraciones 
        /// </summary>
        public GridView GridConfiguracines
        {
            get
            {
                gridConfiguiraciones = this.gvConfiguraciones;
                return gridConfiguiraciones;
            }
            set
            {
                this.gvConfiguraciones = value;
            }
        }
        #endregion

        #region variables de Session
        /// <summary>
        /// Respaldo en session de las configuraciones en GridView
        /// </summary>
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
        /// <summary>
        /// Inicializa los componentes principales de la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                nombreClase = this.GetType().Name;
                presentador = new ConsultarConfiguracionParametrosAuditoriaPRE(this, this.ucConfiguracionParametrosMantenimientoUI1);         
                if (!IsPostBack)
                {
                    this.presentador.ValidarAcceso();
                    this.LimpiarSession();
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
        /// <summary>
        /// Borra las variables de session 
        /// </summary>
        private void LimpiarSession()
        {
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

        #region Enlazar Grid
        /// <summary>
        /// enlaza las configuraciones encontradas von el GridView
        /// </summary>
        private void enlazarGrid()
        {
            this.GridConfiguracines.DataSource = this.ConfiguracionesAuditoria;
            this.GridConfiguracines.DataBind();
        }
        
        #endregion

        #endregion

        #region Eventos

        #region Eventos Grid
        /// <summary>
        /// Controla los eventod durante el cambio de pagina del Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConfiguraciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.gvConfiguraciones.PageIndex = e.NewPageIndex;
                this.gvConfiguraciones.DataSource = this.ConfiguracionesAuditoria;
                this.gvConfiguraciones.DataBind();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".gvConfiguraciones_PageIndexChanging:" + ex.Message);
            }
        }
        /// <summary>
        /// Controla los eventos de los botones en el Grid
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
                    if (e.CommandName.Trim().Equals("Ver"))
                    {
                        Session["ConfiguracionRecibida"] = this.ConfiguracionesAuditoria[index];
                        LimpiarSession();
                        Response.Redirect("~/Mantenimiento.UI/DetalleConfiguracionParametrosAuditoriaUI.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al ejecutar evento: ", ETipoMensajeIU.ERROR, ex.ToString());
            }

        }
        /// <summary>
        /// Controla el evento en el momento que se enlazan las configuraciones con el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConfiguraciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var ConfiguracionAuditoria = ((ConfiguracionAuditoriaMantenimientoBO)e.Row.DataItem);

                Label actividades = e.Row.FindControl("lbActividades") as Label;
                Label obligatorias = e.Row.FindControl("lbObligatorias") as Label;
                Label aleatorias = e.Row.FindControl("lbAleatorias") as Label;
                Label estatus = e.Row.FindControl("lbEstatus") as Label;


                if (ConfiguracionAuditoria != null)
                {
                    actividades.Text = ConfiguracionAuditoria.DetalleConfiguracion.Count.ToString();
                    obligatorias.Text = ConfiguracionAuditoria.DetalleConfiguracion.Count(x => x.Obligatorio == true).ToString();
                    aleatorias.Text = ConfiguracionAuditoria.NumeroActividadesAleatorias.ToString();
                    if (ConfiguracionAuditoria.Activo == true)
                        estatus.Text = "ACTIVO";
                    else if (ConfiguracionAuditoria.Activo == false)
                        estatus.Text = "INACTIVO";
                }
            }
        }

        #endregion

        /// <summary>
        /// evento de busqueda de las configuraciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClickBuscarConfiguraciones(object sender, EventArgs e)
        {
            try
            {
                this.presentador.buscarConfiguracionesAuditoria();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al buscar la configuracion", ETipoMensajeIU.ERROR, ex.ToString());
            }
        }

        #endregion
    
    }
}