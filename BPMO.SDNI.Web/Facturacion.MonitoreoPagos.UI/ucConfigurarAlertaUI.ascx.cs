//Satisface el caso de uso CU009 – Configuración Notificación de facturación
//Satisface a la solicitud de cambios SC0008

using System;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Facturacion.MonitoreoPagos.PRE;
using BPMO.SDNI.Facturacion.MonitoreoPagos.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Facturacion.MonitoreoPagos.UI
{
    /// <summary>
    /// Control de usuario que realiza el proceso de visualización de los campos de un registro
    /// </summary>
    public partial class ucConfigurarAlertaUI : System.Web.UI.UserControl, IucConfigurarAlertaVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador para la vista que visualiza los campos de un registro
        /// </summary>
        private ucConfigurarAlertaPRE presentador;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "ucConfigurarAlertaUI";

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
        /// Obtiene un valor que representa el identificador de la unidad operativa actual de la sesión en curso
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? UnidadOperativaID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnUnidadOperativaID.Value) && !string.IsNullOrWhiteSpace(this.hdnUnidadOperativaID.Value))
                {
                    if (Int32.TryParse(this.hdnUnidadOperativaID.Value, out val))
                        return val;
                    else
                    {
                        Site masterMsj = (Site)Page.Master;

                        if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                            return masterMsj.Adscripcion.UnidadOperativa.Id.Value;
                    }
                }
                else
                {
                    Site masterMsj = (Site)Page.Master;

                    if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                        return masterMsj.Adscripcion.UnidadOperativa.Id.Value;
                }
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnUnidadOperativaID.Value = value.Value.ToString();
                else
                    this.hdnUnidadOperativaID.Value = string.Empty;
            }
        }

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
        /// Obtiene o establece un valor que representa el identificador de la configuración
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? ConfiguracionAlertaID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnConfiguracionAlertaID.Value))
                    id = int.Parse(this.hdnConfiguracionAlertaID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnConfiguracionAlertaID.Value = value.ToString();
                else
                    this.hdnConfiguracionAlertaID.Value = string.Empty;
            }
        }

        #region SC0008
        /// <summary>
        /// Obtiene un valor que representa el perfil al que esta asociado la configuración
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? PerfilID 
        {
            get 
            {
                if (String.IsNullOrEmpty(this.hdfPerfilID.Value))
                    return null;

                return Convert.ToInt32(this.hdfPerfilID.Value);
            }
            set
            {
                this.hdfPerfilID.Value = value != null ? value.ToString() : String.Empty;
            }
        }
        #endregion

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
        /// Obtiene o establece un valor que representa el nombre completo del empleado a quien se le asigna la configuración
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
        /// Obtiene o establece un valor que representa la cuenta de correo electrónico que tiene actualmente el empleado donde se enviarán las notificaciones
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string CorreoElectronico
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtCorreoElectronico.Text)) ? null : this.txtCorreoElectronico.Text.Trim().ToUpper();
            }
            set
            {
                this.txtCorreoElectronico.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                              ? value.Trim().ToUpper()
                                              : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el número de días para recibir la notificación
        /// </summary>
        /// <value>Objeto de tipo Int16</value>
        public short? NumeroDias
        {
            get
            {
                short val = 0;
                if (!string.IsNullOrEmpty(this.txtNumeroDeDias.Text) && !string.IsNullOrWhiteSpace(this.txtNumeroDeDias.Text))
                    if (Int16.TryParse(this.txtNumeroDeDias.Text, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.txtNumeroDeDias.Text = value.Value.ToString();
                else
                    this.txtNumeroDeDias.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha de creación del registro
        /// </summary>
        /// <value>Objeto de tipo Nullable de DateTime</value>
        public DateTime? FC
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.hdFC.Value))
                    temp = DateTime.Parse(this.hdFC.Value.Trim());
                else
                    temp = DateTime.Now;

                return temp;
            }
            set
            {
                if (value != null)
                    this.hdFC.Value = value.Value.ToString();
                else
                    this.hdFC.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha de última actualización del registros
        /// </summary>
        /// <value>Objeto de tipo Nullable de DateTime</value>
        public DateTime? FUA
        {
            get
            {
                return DateTime.Now;
            }           
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del usuario que creo el registro
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? UC
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUC.Value))
                    id = int.Parse(this.hdnUC.Value.Trim());
                else
                {                   
                    Site masterMsj = (Site)this.Page.Master;

                    if (masterMsj.Usuario != null)
                        id = masterMsj.Usuario.Id;
                }

                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUC.Value = value.ToString();
                else
                    this.hdnUC.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del usuario que actualizó por última vez el registro
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? UUA
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)this.Page.Master;

                if (masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;

                return id;
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

        #region Propiedades Buscador

        /// <summary>
        /// Obtiene un valor que representa un identificador único para la UI
        /// </summary>
        /// <value>Objeto de tipo String</value>
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

        /// <summary>
        /// Enumerador que contiene los buscadores existentes en la UI
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
        #endregion Propiedades Buscador
        #endregion

        #region Métodos
        /// <summary>
        /// Realiza el proceso de inicializar el visor para capturar un nuevo registro
        /// </summary>
        public void PrepararNuevo()
        {
            this.hdnConfiguracionAlertaID.Value = null;
            this.txtSucursal.Text = null;
            this.txtEmpleado.Text = null;
            this.txtCorreoElectronico.Text = null;
            this.txtNumeroDeDias.Text = null;
            this.ddlEstatus.ClearSelection();

            this.hdnEmpleadoID.Value = "";
            this.hdnSucursalID.Value = "";
            this.hdnClaveSucursal.Value = "";
            this.hdfPerfilID.Value = "";
            this.hdnUC.Value = "";
            this.hdFC.Value = null;
            
            this.txtSucursal.Enabled = true;
            this.txtEmpleado.Enabled = true;
            this.txtCorreoElectronico.Enabled = false;
            this.txtNumeroDeDias.Enabled = true;
            this.ddlEstatus.Enabled = true;

            this.ibtnBuscarSucursal.Enabled = true;
            this.ibtnBuscarEmpleado.Enabled = true;
            this.PermitirSeleccionarEmpleado(false);
        }

        /// <summary>
        /// Realiza el proceso de inicializar el visor para editar un registro existente
        /// </summary>
        public void PrepararEdicion()
        {
            this.txtSucursal.Enabled = false;
            this.txtEmpleado.Enabled = false;
            this.txtCorreoElectronico.Enabled = false;
            this.txtNumeroDeDias.Enabled = true;
            this.ddlEstatus.Enabled = true;

            this.ibtnBuscarSucursal.Enabled = false;
            this.ibtnBuscarEmpleado.Enabled = false;           
        }

        /// <summary>
        /// Realiza el proceso de inicializar el visor para mostrar los datos de un registro
        /// </summary>
        public void PrepararVisualizacion()
        {
            this.txtSucursal.Enabled = false;
            this.txtEmpleado.Enabled = false;
            this.txtCorreoElectronico.Enabled = false;
            this.txtNumeroDeDias.Enabled = false;
            this.ddlEstatus.Enabled = false;

            this.ibtnBuscarSucursal.Enabled = false;
            this.ibtnBuscarEmpleado.Enabled = false;           
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
        /// Libera o elimina los valores en sesión guardados en el visor
        /// </summary>
        public void LimpiarSesion()
        {           
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, Primitivos.Enumeradores.ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)this.Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            }
            else
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
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
            this.RegistrarScript("Events", "BtnBuscar('" + this.ViewState_Guid + "','" + catalogo + "', '" + this.btnResult.ClientID + "');");
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
        /// Realiza la validación de los datos capturados para detectar errores o inconsistencias
        /// </summary>
        /// <returns>Objeto de tipo String que contiene el error detectado, en caso contrario devolverá nulo</returns>
        public String ValidarCampos()
        {
            if (this.IsPostBack)
            {
                this.Page.Validate("ActualizarRegistro");
                if (!this.Page.IsValid)
                    return "Existen campos que no se han capturado correctamente, favor de revisar";
            }

            return null;
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Evento que se ejecuta cuando se carga el visor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new ucConfigurarAlertaPRE(this);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear de página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.GetBaseException().Message);
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
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
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
        #endregion
    }
}