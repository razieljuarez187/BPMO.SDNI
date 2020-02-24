//Satisface el caso de uso CU009 – Configuración Notificación de facturación
//Satisface la solicitud de cambio SC0008

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Facturacion.MonitoreoPagos.BO;
using BPMO.SDNI.Facturacion.MonitoreoPagos.PRE;
using BPMO.SDNI.Facturacion.MonitoreoPagos.VIS;
using BPMO.SDNI.MapaSitio.UI;
using System.Drawing;
using System.Collections;
using System.Dynamic;

namespace BPMO.SDNI.Facturacion.MonitoreoPagos.UI
{
    /// <summary>
    /// Forma que realiza el proceso de consulta de notificaciones de empleados
    /// </summary>
    public partial class ConsultarConfigurarAlertaUI : System.Web.UI.Page, IConsultarConfigurarAlertaVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador para la vista que visualiza los campos de un registro
        /// </summary>
        private ConsultarConfigurarAlertaPRE presentador;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "ConsultarConfigurarAlertaUI";

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
        /// Obtiene un valor que representa el identificador del usuario actual de la sesión en curso
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? UsuarioID
        {
            get
            {
                var masterMsj = (Site)this.Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id.Value;
                return null;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el identificador de la unidad operativa actual de la sesión en curso
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? UnidadOperativaID
        {
            get
            {
                Site masterMsj = (Site)this.Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de sucursal para las que aplica la configuración
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? SucursalID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnSucursalID.Value) && !string.IsNullOrWhiteSpace(this.hdnSucursalID.Value))
                    if (Int32.TryParse(this.hdnSucursalID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnSucursalID.Value = value.Value.ToString();
                else
                    this.hdnSucursalID.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre corto de 
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public String ClaveSucursal
        {
            get
            {               
                string val = null;
                if (this.hdnClaveSucursal.Value.Trim().Length > 0)
                    val = this.hdnClaveSucursal.Value.Trim();
                return val;
            }
            set
            {
                if (value != null)
                    this.hdnClaveSucursal.Value = value;
                else
                    this.hdnClaveSucursal.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre de la sucursal para las que aplica la configuración
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string SucursalNombre
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtSucursal.Text) && !string.IsNullOrWhiteSpace(this.txtSucursal.Text))
                    return this.txtSucursal.Text.Trim().ToUpper();

                return null;
            }
            set
            {
                if (value != null)
                    this.txtSucursal.Text = value;
                else
                    this.txtSucursal.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del empleado a quien se le asigna la configuración
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
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

        /// <summary>
        /// Obtiene o establece valor que representa el nombre completo del empleado a quien se le asigna la configuración
        /// </summary>
        /// <value>Objeto de tipo String</value>
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
        /// Obtiene o establece un valor que representa el estatus de la configuración
        /// </summary>
        /// <value>Objeto de tipo Nullable de Boolean</value>
        public bool? Estatus
        {
            get
            {
                bool? estatus = null;
                if (this.ddlEstatus.SelectedValue.ToString().ToUpper() == "TRUE") estatus = true;
                if (this.ddlEstatus.SelectedValue.ToString().ToUpper() == "FALSE") estatus = false;
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

        /// <summary>
        /// Obtiene un valor que representa el último resultado de la consulta de registros solicitado
        /// </summary>
        /// <value>Objeto de tipo List de ConfiguracionAlertaBO</value>
        public List<ConfiguracionAlertaBO> Resultado
        {
            get 
            {
                if (this.Session["ListaConfiguracionesAlerta"] == null)
                    return new List<ConfiguracionAlertaBO>();
                else
                    return (List<ConfiguracionAlertaBO>)Session["ListaConfiguracionesAlerta"];
            }
        }        

        #region Propiedades para el Buscador
        /// <summary>
        /// Obtiene un valor que representa un identificador único para la UI
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string ViewState_Guid
        {
            get
            {
                if (this.ViewState["GuidSession"] == null)
                {
                    Guid guid = Guid.NewGuid();
                    this.ViewState["GuidSession"] = guid.ToString();
                }
                return this.ViewState["GuidSession"].ToString();
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el objeto que fue seleccionado del buscador
        /// </summary>
        /// <value>
        /// Objeto que fue seleccionado de tipo Object
        /// </value>
        protected object Session_BOSelecto
        {
            get
            {
                object objeto = null;
                string nombreSession = String.Format("BOSELECTO_{0}", this.ViewState_Guid);
                if (this.Session[nombreSession] != null)
                    objeto = (this.Session[nombreSession]);

                return objeto;
            }
            set
            {
                string nombreSession = String.Format("BOSELECTO_{0}", this.ViewState_Guid);
                if (value != null)
                    this.Session[nombreSession] = value;
                else
                    this.Session.Remove(nombreSession);
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el objeto que tiene la información de filtrado del buscador
        /// </summary>
        /// <value>
        /// Objeto de filtrado de tipo Object
        /// </value>
        protected object Session_ObjetoBuscador
        {
            get
            {
                object objeto = null;
                if (this.Session[this.ViewState_Guid] != null)
                    objeto = (this.Session[this.ViewState_Guid]);

                return objeto;
            }
            set
            {
                if (value != null)
                    this.Session[this.ViewState_Guid] = value;
                else
                    this.Session.Remove(this.ViewState_Guid);
            }
        }

        /// <summary>
        /// Enumerador que contiene los buscadores existentes en la UI
        /// </summary>
        public ECatalogoBuscador ViewState_Catalogo
        {
            get
            {
                return (ECatalogoBuscador)this.ViewState["BUSQUEDA"];
            }
            set
            {
                this.ViewState["BUSQUEDA"] = value;
            }
        }
        #endregion
        #endregion

        #region Métodos
        /// <summary>
        /// Asigna o guarda el resultado obtenido de una consulta solicitada
        /// </summary>
        /// <param name="resultado">Lista de BO's obtenidos de la consulta</param>
        public void EstablecerResultado(List<ConfiguracionAlertaBO> resultado)
        {
            this.Session["ListaConfiguracionesAlerta"] = resultado;
           
            this.grdConfiguracionesAlerta.DataSource = resultado;
            this.grdConfiguracionesAlerta.DataBind();
        }

        /// <summary>
        /// Establece un paquete de navegación en el visor dentro de la sesión en curso
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        /// <param name="value">Valor a asignar dentro del paquete de navegación</param>
        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            this.Session[key] = value;
        }

        /// <summary>
        /// Obtiene el valor de un paquete de navegación ya guardado previamente
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        /// <returns>Valor de tipo objet dentro del paquete de navegación</returns>
        public object ObtenerPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".ObtenerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            return this.Session[key];
        }

        /// <summary>
        /// Elimina un paquete de navegación ya guardado previamente
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        public void LimpiarPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".LimpiarPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            if (this.Session[key] != null)
                this.Session.Remove(key);
        }

        /// <summary>
        /// Asigna el estatus correspondiente para permitir o denegar la selección del empleado a asignar
        /// </summary>
        /// <param name="permitir">bandera que define la operación a realizar, true para permitir, false para denegar</param>
        public void PermitirSeleccionarEmpleado(bool permitir)
        {
            this.txtEmpleado.Enabled = permitir;
            this.ibtnBuscarEmpleado.Enabled = permitir;
        }

        /// <summary>
        /// Asigna el estatus correspondiente para permitir o deneger el registro de una nueva notificación para un empleado
        /// </summary>
        /// <param name="permitir">bandera que define la operación a realizar, true para permitir, false para denegar</param>
        public void PermitirRegistrar(bool permitir)
        {
            this.hlkRegistrar.Enabled = permitir;
        }

        /// <summary>
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        /// <summary>
        /// Despliega el Detalle del registro Seleccionado
        /// </summary>
        public void RedirigirADetalle()
        {
            this.Response.Redirect(this.ResolveUrl("~/Facturacion.MonitoreoPagos.UI/DetalleConfigurarAlertaUI.aspx"));
        }

        /// <summary>
        /// Libera o elimina los valores en sesión guardados en el visor
        /// </summary>
        public void LimpiarSesion()
        {
            if (this.Session["ListaConfiguracionesAlerta"] != null)
                this.Session.Remove("ListaConfiguracionesAlerta");
        }

        /// <summary>
        /// Realiza la validación de los datos capturados para detectar errores o inconsistencias
        /// </summary>
        /// <returns>Objeto de tipo String que contiene el error detectado, en caso contrario devolverá nulo</returns>
        public String ValidarCampos()
        {
            if (this.IsPostBack && !this.IsValid)
                return "Existen campos que no se han capturado correctamente, favor de revisar";

            return null;
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)this.Master;
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
            this.ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = this.presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscar('" + this.ViewState_Guid + "','" + catalogo + "');");
        }

        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            this.presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;

            if (catalogo.ToString() == "Sucursal")
                this.txtEmpleado.Focus();
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
        /// <summary>
        /// Carga inicial de la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new ConsultarConfigurarAlertaPRE(this);
                if (!Page.IsPostBack)
                {
                    this.presentador.PrepararBusqueda();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistenacias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load: " + ex.GetBaseException().Message);
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
                string nombreSucursal = this.SucursalNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                this.SucursalNombre = nombreSucursal;
                if (this.SucursalNombre != null)
                {
                    this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                    this.SucursalNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtSucursal_TextChanged" + ex.GetBaseException().Message);
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
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarSucursal_Click" + ex.GetBaseException().Message);
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
                string nombreEmpleado = this.EmpleadoNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Empleado);

                this.EmpleadoNombre = nombreEmpleado;
                if (this.EmpleadoNombre != null)
                {
                    this.EjecutaBuscador("Empleado", ECatalogoBuscador.Empleado);
                    this.EmpleadoNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Empleado", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNombreEmpleado_TextChanged" + ex.GetBaseException().Message);
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
                this.MostrarMensaje("Inconsistencias al buscar el usuario", ETipoMensajeIU.ERROR, nombreClase + ".ibtnBuscarEmpleado_Click:" + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Evento que se ejecua cuando se recibe el aviso de resultado del buscador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Sucursal:
                    case ECatalogoBuscador.Empleado:
                        this.DesplegarBOSelecto(this.ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click: " + ex.GetBaseException().Message);
            }
        }
        #endregion

        /// <summary>
        /// Evento que se ejecuta cuand se realiza la búsqueda de datos por los filtros capturados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Consultar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".btnBuscar_Click: " + ex.GetBaseException().Message);
            }
        }        

        /// <summary>
        /// Evento que se ejecuta cuando se realiza un cambio de página en la lista de resultados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdConfiguracionesAlerta_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grdConfiguracionesAlerta.DataSource = this.Resultado;
                this.grdConfiguracionesAlerta.PageIndex = e.NewPageIndex;
                this.grdConfiguracionesAlerta.DataBind();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdOperadores_PageIndexChanging: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se ligan los datos con el Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdConfiguracionesAlerta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ConfiguracionAlertaBO configuracion = e.Row.DataItem as ConfiguracionAlertaBO;
                System.Web.UI.WebControls.Image imgTipoConfiguracion = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgTipoConfiguracion");

                if (configuracion is ConfiguracionAlertaPerfilBO)
                {
                    imgTipoConfiguracion.ImageUrl = "~/Contenido/Imagenes/ico.usuario.certificado.png";
                    imgTipoConfiguracion.ToolTip = String.Format("Configuración Automática para empleado con perfil {0}", (configuracion as ConfiguracionAlertaPerfilBO).Perfil.NombreCorto);
                    //e.Row.ForeColor = Color.FromArgb(0, 113, 188);
                }
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se envia la solicitud de ejecución de un comando de la lista de resultados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdConfiguracionesAlerta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandSource == sender)
                return;
           
            int index;            
            try
            {
                index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                switch (e.CommandName.Trim())
                {
                    case "Detalles":
                        {
                            #region SC0008 Actualización de datos del grid para soporte de configuraciones de facturistas                            
                            HiddenField hdfPerfil = (HiddenField)row.FindControl("hdfPerfil");
                            HiddenField hdfSucursalID = (HiddenField)row.FindControl("hdfSucursalID");
                            HiddenField hdfEmpleadoID = (HiddenField)row.FindControl("hdfEmpleadoID");
                            Label lblNumeroDias = (Label)row.FindControl("lblNumeroDias");
                            Label lblSucursalNombre = (Label)row.FindControl("lblSucursalNombre");
                            Label lblEmpleadoNombreCompleto = (Label)row.FindControl("lblEmpleadoNombreCompleto");
                            Label lblEmail = (Label)row.FindControl("lblEmail");

                            dynamic parameters = new ExpandoObject();
                            parameters.ConfiguracionAlertaID = index;
                            if (!String.IsNullOrEmpty(hdfPerfil.Value))
                                parameters.PerfilID = Convert.ToInt32(hdfPerfil.Value);         
                                      
                            parameters.SucursalID = Convert.ToInt32(hdfSucursalID.Value);
                            parameters.SucursalNombre = lblSucursalNombre.Text;
                            parameters.EmpleadoID = Convert.ToInt32(hdfEmpleadoID.Value);
                            parameters.EmpleadoNombreCompleto = lblEmpleadoNombreCompleto.Text;
                            parameters.NumeroDias = Convert.ToInt16(lblNumeroDias.Text);
                            parameters.EmpleadoEmail = lblEmail.Text;
                            #endregion

                            this.presentador.IrADetalle(parameters);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al ejecutar la acción sobre el operador", ETipoMensajeIU.ERROR, nombreClase + ".grdOperadores_RowCommand:" + ex.GetBaseException().Message);
            }
        }
        
        #endregion        
    }
}