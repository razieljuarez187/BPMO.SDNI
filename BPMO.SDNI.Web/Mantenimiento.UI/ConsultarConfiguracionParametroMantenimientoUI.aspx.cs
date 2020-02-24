// Satisface al caso de Uso CU053 - Configurar Parametros Mantenimiento
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimientos.PRE;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using System.Configuration;
using BPMO.SDNI.Mantenimiento.BO;

namespace BPMO.SDNI.Mantenimiento.UI
{
    public partial class ConsultarConfiguracionParametroMantenimientoUI: System.Web.UI.Page, IConsultarConfiguracionParametroMantenimientoVIS
    {
        #region Atributos      
        private ConsultarConfiguracionParametroMantenimientoPRE presentador = null;
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
        /// Nombre del modelo asociado a la configuración
        /// </summary>
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
        /// <summary>
        /// ID del modelo acosiado a la configuración
        /// </summary>
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
        /// <summary>
        /// Tipo de Mantenimiento de la configuración
        /// </summary>
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
                    ddTipoMantenimiento.SelectedValue = value.ToString();
                else
                    ddTipoMantenimiento.SelectedValue.FirstOrDefault();
            }
        }
        /// <summary>
        /// Unidad de medida de la configuración
        /// </summary>
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
                    ddUnidadMedida.SelectedValue = value.ToString();
                else
                    ddUnidadMedida.SelectedValue.FirstOrDefault();
            }
        }
        /// <summary>
        /// Estatus del registro en la base de datos para eliminacion logica
        /// </summary>
        public bool? Estatus
        {
            get
            {
                bool? estatus = null;
                if (this.ddlEstatus.SelectedIndex == 1)
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
        /// <summary>
        /// GrindView para los resultados de la consulta
        /// </summary>
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
        /// <summary>
        /// Variable de Session donde se guardan las configuraciones de la consulta 
        /// </summary>
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

        /// <summary>
        /// Inicializa los metodos y objetos principales al inicio de la carga de la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
             try
            {
                nombreClase = this.GetType().Name;
                presentador = new ConsultarConfiguracionParametroMantenimientoPRE(this);
                if(!IsPostBack)
                {
                    this.presentador.ValidarAcceso();
                    this.LimpiarSession();
                    this.gvConfiguraciones.DataSource = null;
                    this.gvConfiguraciones.DataBind();
                    this.enlazarControles();
                }
                this.DesplegarListaConfiguraciones();
     
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

        /// <summary>
        /// Asigna el estatus correspondiente para permitir o deneger el registro de una nueva notificación para un empleado
        /// </summary>
        /// <param name="permitir">bandera que define la operación a realizar, true para permitir, false para denegar</param>
        public void PermitirRegistrar(bool permitir)
        {
            this.hlkRegistrarMantenimiento.Enabled = permitir;
        }

        #endregion

        #region Desplegar Lista
        /// <summary>
        /// Vincula los resultados de la consulta para mostrarlos en el gridView
        /// </summary>
        public void DesplegarListaConfiguraciones()
        {
            this.gvConfiguraciones.DataSource = this.configuraciones;
            this.gvConfiguraciones.DataBind();
        }
        #endregion

        #region Limpiar Session
        /// <summary>
        /// Elimina todas la variables de session 
        /// </summary>
        private void LimpiarSession()
        {
            Session.Remove("listaconfiguraciones"); 
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

        private void  DesplegarFormBusquedaModelo()
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

        #region Enlazar Controles
        /// <summary>
        /// Enlaza el control DropDownList con el enumerable con los tipos de Mantenimiento del sistema. 
        /// </summary>
        private void enlazarControles()
        {
            ListItem todos = new ListItem()
            {
                Text = "TODOS",
                Value = "0"
            };

            var responseEnumTipo = Enum.GetNames(typeof(ETipoMantenimiento)).Select(x => new { text = x, value = (int)Enum.Parse(typeof(ETipoMantenimiento), x) });
            ddTipoMantenimiento.DataSource = responseEnumTipo;
            ddTipoMantenimiento.DataTextField = "text";
            ddTipoMantenimiento.DataValueField = "value";
            ddTipoMantenimiento.DataBind();
            ddTipoMantenimiento.Items.Add(todos);
            ddTipoMantenimiento.SelectedValue = "0";

            var responseEnumUnidad = Enum.GetNames(typeof(EUnidaMedida)).Select(x => new { text = x, value = (int)Enum.Parse(typeof(EUnidaMedida), x) });
            ddUnidadMedida.DataSource = responseEnumUnidad;
            ddUnidadMedida.DataTextField = "text";
            ddUnidadMedida.DataValueField = "value";
            ddUnidadMedida.DataBind();
            ddUnidadMedida.Items.Add(todos);
            ddUnidadMedida.SelectedValue = "0";
            
        }

        #endregion

        #endregion

        #region Eventos

        #region Eventos Grid

        /// <summary>
        /// Este evemto se encarga de volver a vincular los datos de la session con el grid cuan de produce un cambio de paginado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConfiguraciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvConfiguraciones.PageIndex = e.NewPageIndex;
                DesplegarListaConfiguraciones();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".gvConfiguraciones_PageIndexChanging:" + ex.Message);
            }

        }

        /// <summary>
        /// permite cachar el eventop del control del grid y redirigir a al detalle la configuracion
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
                        Session["configuracionRecibida"] = configuraciones[index];
                        Response.Redirect("~/Mantenimiento.UI/DetalleConfiguracionParametroMantenimientoUI.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al ejecutar evento: ", ETipoMensajeIU.ERROR, ex.ToString());
            }
        }
        /// <summary>
        /// permite mapiar los valores del objeto al grid durante su vinculacion. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConfiguraciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var ConfiguracionMantenimiento = ((ConfiguracionMantenimientoBO)e.Row.DataItem);

                Label EnUso = e.Row.FindControl("lbEnUso") as Label;
                Label Estatus = e.Row.FindControl("lbEstatus") as Label;

                if (ConfiguracionMantenimiento.EnUso == true)
                    EnUso.Text = "En Uso";
                else
                    EnUso.Text = "Estacionado";
                if (ConfiguracionMantenimiento.Estatus == true)
                    Estatus.Text = "Activo";
                else
                    Estatus.Text = "Inactivo";
                
            }
        }
        #endregion

        #region Eventos de busqueda

        /// <summary>
        /// Ejecuta la busqueda del catalogo general de modelos 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarModelo_Click(object sender, ImageClickEventArgs e)
        {
            DesplegarFormBusquedaModelo();

        }

        /// <summary>
        /// ejecuta la busqueda de las configuraciones con los parametros de la interfaz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClickBuscarConfiguraciones(object sender, EventArgs e)
        {
            try
            {
                this.presentador.buscarConfiguracion();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al buscar los parametros de configuracion :", ETipoMensajeIU.ERROR, ex.ToString());
            }

        }
        /// <summary>
        /// Despliega el resultado seleccionado del buscador general
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
        /// <summary>
        /// Ejecuta la busqueda del modelo en el buscador general cuando se produce el evento TextChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtModelo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Modelo != string.Empty)
                {
                    DesplegarFormBusquedaModelo();
                }

            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click" + ex.Message);
            }
        }

        #endregion

        #endregion

    }
}