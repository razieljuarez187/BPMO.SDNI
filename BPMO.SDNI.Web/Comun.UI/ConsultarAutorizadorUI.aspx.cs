//Satisface al caso de uso CU098 - Configurar Autorizadores para los Contratos
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.MapaSitio.UI;
using System.ComponentModel;
using System.Linq;

namespace BPMO.SDNI.Comun.UI
{
    public partial class ConsultarAutorizadorUI : System.Web.UI.Page, IConsultarAutorizadorVIS
    {
        #region Atributos

        /// <summary>
        /// Presentador de Consultar Autorizador para los Contratos
        /// </summary>
        private ConsultarAutorizadorPRE presentador;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "ConsultarAutorizadorUI";

        /// <summary>
        /// Enumerador de Catalogos para el Buscador
        /// </summary>
        public enum ECatalogoBuscador
        {
            Sucursal = 0,
            Empleado = 1
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Usuario que se ha identificado en el Sistema
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id.Value;
                return null;
            }
        }
        /// <summary>
        /// Obtiene la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary> 
        public int? UnidadOperativaID
        {
            get
            {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
            }
        }

        /// <summary>
        /// Obtiene o establece el nombre de la sucursal seleccionada para el filtro de consulta
        /// </summary>
        public string SucursalNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtSucursal.Text)) ? null : this.txtSucursal.Text.Trim().ToUpper();
            }
            set
            {
                this.txtSucursal.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                            ? value.Trim().ToUpper()
                                            : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la sucursal de la cual se desean filtrar los autorizadores
        /// </summary>
        public int? SucursalID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnSucursalID.Value))
                    id = int.Parse(this.hdnSucursalID.Value.Trim());
                return id;
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
        /// Tipo de Autorización 
        /// </summary>
        public Enum TipoAutorizacion
        {
            get
            {
                Enum tipoAutorizacion = null;
                switch (this.UnidadOperativaID)
                {
                    case (int)ETipoEmpresa.Generacion:
                        tipoAutorizacion = ETipoAutorizacionGeneracion;
                        break;
                    case (int)ETipoEmpresa.Construccion:
                        tipoAutorizacion = ETipoAutorizacionConstruccion;
                        break;
                    case (int)ETipoEmpresa.Equinova:
                        tipoAutorizacion = ETipoAutorizacionEquinova;
                        break;
                    default:
                        tipoAutorizacion = ETipoAutorizacion;
                        break;
                }
                return tipoAutorizacion;
            }
            set
            {
                if (value != null)
                {
                    switch (this.UnidadOperativaID)
                    {
                        case (int)ETipoEmpresa.Generacion:
                            this.ddlTipoAutorizacion.SelectedIndex = (int)(ETipoAutorizacionGeneracion)value;
                            break;
                        case (int)ETipoEmpresa.Construccion:
                            this.ddlTipoAutorizacion.SelectedIndex = (int)(ETipoAutorizacionConstruccion)value;
                            break;
                        default:
                            this.ddlTipoAutorizacion.SelectedIndex = (int)(ETipoAutorizacion)value;
                            break;
                    }
                }
                else
                    this.ddlTipoAutorizacion.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Enumerador Turno de Tarifa para Construcción
        /// </summary>
        public ETipoAutorizacion? ETipoAutorizacion
        {
            get
            {
                ETipoAutorizacion? eTipoAutorizacion = null;
                if (this.ddlTipoAutorizacion.SelectedIndex > 0)
                    eTipoAutorizacion = (ETipoAutorizacion)Enum.Parse(typeof(ETipoAutorizacion), ddlTipoAutorizacion.SelectedValue);
                return eTipoAutorizacion;
            }
            set
            {
                ddlTipoAutorizacion.SelectedIndex = (int)(ETipoAutorizacion)value;
            }
        }

        /// <summary>
        /// Enumerador Turno de Tarifa para Construcción
        /// </summary>
        public ETipoAutorizacionConstruccion? ETipoAutorizacionConstruccion
        {
            get
            {
                ETipoAutorizacionConstruccion? eTipoAutorizacion = null;
                if (this.ddlTipoAutorizacion.SelectedIndex > 0)
                    eTipoAutorizacion = (ETipoAutorizacionConstruccion)Enum.Parse(typeof(ETipoAutorizacionConstruccion), ddlTipoAutorizacion.SelectedValue);
                return eTipoAutorizacion;
            }
            set
            {
                ddlTipoAutorizacion.SelectedIndex = (int)(ETipoAutorizacionConstruccion)value;
            }
        }

        /// <summary>
        /// Enumerador Turno de Tarifa para Generación
        /// </summary>
        public ETipoAutorizacionGeneracion? ETipoAutorizacionGeneracion
        {
            get
            {
                ETipoAutorizacionGeneracion? eTipoAutorizacion = null;
                if (this.ddlTipoAutorizacion.SelectedIndex > 0)
                    eTipoAutorizacion = (ETipoAutorizacionGeneracion)Enum.Parse(typeof(ETipoAutorizacionGeneracion), ddlTipoAutorizacion.SelectedValue);
                return eTipoAutorizacion;
            }
            set
            {
                ddlTipoAutorizacion.SelectedIndex = (int)(ETipoAutorizacionGeneracion)value;
            }
        }

        /// <summary>
        /// Enumerador Turno de Tarifa para Equinova
        /// </summary>
        public ETipoAutorizacionEquinova? ETipoAutorizacionEquinova {
            get {
                ETipoAutorizacionEquinova? eTipoAutorizacion = null;
                if (this.ddlTipoAutorizacion.SelectedIndex > 0)
                    eTipoAutorizacion = (ETipoAutorizacionEquinova)Enum.Parse(typeof(ETipoAutorizacionEquinova), ddlTipoAutorizacion.SelectedValue);
                return eTipoAutorizacion;
            }
            set {
                ddlTipoAutorizacion.SelectedIndex = (int)(ETipoAutorizacionEquinova)value;
            }
        }

        /// <summary>
        /// Nombre del Empleado
        /// </summary>
        public string EmpleadoNombre
        {
            get
            {
                string empleadoNombre = null;
                if (this.txtEmpleado.Text.Trim().Length > 0)
                    empleadoNombre = this.txtEmpleado.Text.Trim().ToUpper();
                return empleadoNombre;
            }
            set
            {
                if (value != null)
                    this.txtEmpleado.Text = value.ToString();
                else
                    this.txtEmpleado.Text = string.Empty;
            }
        }
        /// <summary>
        /// Identificador del Empleado
        /// </summary>
        public int? EmpleadoID
        {
            get
            {
                int? empleadoID = null;
                if (!String.IsNullOrEmpty(this.hdnEmpleadoID.Value))
                    empleadoID = int.Parse(this.hdnEmpleadoID.Value.Trim());
                return empleadoID;
            }
            set
            {
                if (value != null)
                    this.hdnEmpleadoID.Value = value.ToString();
                else
                    this.hdnEmpleadoID.Value = string.Empty;
            }
        }
        public bool? Estatus
        {
            get
            {
                bool? estatus = null;
                if (ddlEstatus.SelectedValue.ToString().ToUpper() == "TRUE") estatus = true;
                if (ddlEstatus.SelectedValue.ToString().ToUpper() == "FALSE") estatus = false;
                return estatus;
            }
            set
            {
                if (value != null)
                    this.ddlEstatus.SelectedValue = value.ToString();
                else
                    this.ddlEstatus.SelectedIndex = -1;
            }
        }

        public List<AutorizadorBO> Resultado
        {
            get
            {
                if (Session["ListadoAutorizadores"] != null)
                    return Session["ListadoAutorizadores"] as List<AutorizadorBO>;

                return null;
            }
        }

        #region Propiedades Buscador
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
                    objeto = (Session[nombreSession]);

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
                    objeto = (Session[ViewState_Guid]);

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
        #endregion Propiedades Buscador
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new ConsultarAutorizadorPRE(this);
                if (!Page.IsPostBack)
                {
                    presentador.PrepararBusqueda();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistenacias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load: " + ex.Message);
            }

        }
        #endregion

        #region Métodos
        public void EstablecerResultado(List<AutorizadorBO> resultado)
        {
            Session["ListadoAutorizadores"] = resultado;

            this.grdAutorizadores.DataSource = resultado;
            this.grdAutorizadores.DataBind();
        }

        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".EstablecerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            Session[key] = value;
        }
        public object ObtenerPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".ObtenerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            return Session[key];
        }
        public void LimpiarPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".LimpiarPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            if (Session[key] != null)
                Session.Remove(key);
        }

        public void PermitirRegistrar(bool permitir)
        {
            this.hlRegistroOrden.Enabled = permitir;
        }

        public void EstablecerOpcionesTiposAutorizacion(Dictionary<int, string> tipos)
        {
            if (ReferenceEquals(tipos, null))
                tipos = new Dictionary<int, string>();

            this.ddlTipoAutorizacion.Items.Clear();
            this.ddlTipoAutorizacion.DataSource = tipos;
            this.ddlTipoAutorizacion.DataValueField = "key";
            this.ddlTipoAutorizacion.DataTextField = "value";
            this.ddlTipoAutorizacion.DataBind();
            this.ddlTipoAutorizacion.SelectedValue = "-1";
        }

        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        public void RedirigirADetalle()
        {
            this.Response.Redirect(this.ResolveUrl("~/Comun.UI/DetalleAutorizadorUI.aspx"));
        }

        public void LimpiarSesion()
        {
            if (Session["ListadoAutorizadores"] != null)
                Session.Remove("ListadoAutorizadores");
        }

        /// <summary>
        /// Despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            }
            else
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        #region Métodos para el Buscador
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="tipoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
        }
        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;
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
        #endregion

        #region Eventos
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ConsultarAutorizadores();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".btnBuscar_Click: " + ex.Message);
            }
        }

        protected void grdAutorizadores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdAutorizadores.DataSource = this.Resultado;
                grdAutorizadores.PageIndex = e.NewPageIndex;
                grdAutorizadores.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdAutorizadores_PageIndexChanging: " + ex.Message);
            }
        }

        protected void grdAutorizadores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index;
            if (e.CommandName.ToString().Trim().ToUpper() == "PAGE") return;
            try
            {
                index = Convert.ToInt32(e.CommandArgument.ToString());
                switch (e.CommandName.Trim())
                {
                    case "Detalles":
                        {
                            this.presentador.IrADetalle(index);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al ejecutar la acción sobre el autorizador", ETipoMensajeIU.ERROR, nombreClase + ".grdAutorizadores_RowCommand:" + ex.Message);
            }
        }

        protected void grdAutorizadores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    AutorizadorBO autorizador = (AutorizadorBO)e.Row.DataItem;                    

                    Label labelSucursalNombre = e.Row.FindControl("lblSucursal") as Label;
                    if (labelSucursalNombre != null)
                    {
                        string sucursalNombre = string.Empty;
                        if (autorizador.Sucursal != null)
                            if (autorizador.Sucursal.Nombre != null)
                            {
                                sucursalNombre = autorizador.Sucursal.Nombre;
                            }
                        labelSucursalNombre.Text = sucursalNombre;
                    }
					Label labelTipoAutorizacion = e.Row.FindControl("lblTipoAutorizacion") as Label;
                    if (labelTipoAutorizacion != null)
                    {
                        string tipoAutorizacion = string.Empty;
                        if (autorizador.TipoAutorizacion != null)
                        {
                            switch (this.UnidadOperativaID)
                            {
                                case (int)ETipoEmpresa.Generacion:
                                case (int)ETipoEmpresa.Equinova:
                                case (int)ETipoEmpresa.Construccion:
                                    Type type = this.UnidadOperativaID == (int)ETipoEmpresa.Construccion ? typeof(ETipoAutorizacionConstruccion) : 
                                        this.UnidadOperativaID == (int)ETipoEmpresa.Generacion ? typeof(ETipoAutorizacionGeneracion) :
                                        typeof(ETipoAutorizacionEquinova);
                                    var memInfo = type.GetMember(type.GetEnumName(autorizador.TipoAutorizacion));
                                    var display = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                                    if (display != null)
                                    {
                                        tipoAutorizacion = display.Description.ToUpper();
                                    }
                                    break;
                                default:
                                    tipoAutorizacion = Enum.GetName(typeof(ETipoAutorizacion), autorizador.TipoAutorizacion);
                                    break;
                            }
                        }
                        labelTipoAutorizacion.Text = tipoAutorizacion.Replace("_", " ");
                    }
                    Label labelEmpleadoNombre = e.Row.FindControl("lblEmpleadoNombre") as Label;
                    if (labelEmpleadoNombre != null)
                    {
                        string empleadoNombre = string.Empty;
                        if (autorizador.Empleado != null)
                            if (autorizador.Empleado.NombreCompleto != null)
                            {
                                empleadoNombre = autorizador.Empleado.NombreCompleto;
                            }
                        labelEmpleadoNombre.Text = empleadoNombre;
                    }

                    Label labelEmpleadoEmail = e.Row.FindControl("lblEmpleadoEmail") as Label;
                    if (labelEmpleadoEmail != null)
                    {
                        string empleadoEmail = string.Empty;
                        if (autorizador.Empleado != null)
                            if (autorizador.Empleado.Email != null)
                            {
                                empleadoEmail = autorizador.Empleado.Email;
                            }
                        labelEmpleadoEmail.Text = empleadoEmail;
                    }

                    Label labelEmpleadoTelefono = e.Row.FindControl("lblEmpleadoTelefono") as Label;
                    if (labelEmpleadoTelefono != null)
                    {
                        string empleadoTelefono = string.Empty;
                        if (autorizador.Empleado != null)
                            if (autorizador.Empleado.Telefonos != null && autorizador.Empleado.Telefonos.Count>0)
                            {
                                empleadoTelefono = autorizador.Empleado.Telefonos[0].Numero;
                            }
                        labelEmpleadoTelefono.Text = empleadoTelefono;
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".grdAutorizadores_RowDataBound: " + ex.Message);
            }
        }

        #region Eventos para el Buscador
        /// <summary>
        /// TextChanged activa el llamado al Buscador para la busqueda de Sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtSucursal_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreSucursal = SucursalNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                SucursalNombre = nombreSucursal;
                if (SucursalNombre != null)
                {
                    this.EjecutaBuscador("Sucursal", ECatalogoBuscador.Sucursal);
                    SucursalNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtSucursal_TextChanged" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscarSucursal_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("Sucursal&hidden=0", ECatalogoBuscador.Sucursal);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarSucursal_Click" + ex.Message);
            }
        }

        /// <summary>
        /// TextChanged activa el llamado al Buscador para la busqueda de Empleado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtNombreEmpleado_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreEmpleado = EmpleadoNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Empleado);

                EmpleadoNombre = nombreEmpleado;
                if (EmpleadoNombre != null)
                {
                    this.EjecutaBuscador("Empleado", ECatalogoBuscador.Empleado);
                    EmpleadoNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Empleado", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNombreEmpleado_TextChanged" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Empleado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscarEmpleado_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("Empleado", ECatalogoBuscador.Empleado);
                
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al buscar el usuario", ETipoMensajeIU.ERROR, nombreClase + ".ibtnBuscarEmpleado_Click:" + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Sucursal:
                    case ECatalogoBuscador.Empleado:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click: " + ex.Message);
            }
        }
        #endregion
        #endregion
    }
}