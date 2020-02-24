//Satisface al caso de uso CU098 - Configurar Autorizadores para los Contratos
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Comun.UI {
    public partial class ucAutorizadorUI : System.Web.UI.UserControl, IucAutorizadorVIS {
        #region Atributos

        /// <summary>
        /// Presentador del ucAutorizador para autorizadores
        /// </summary>
        private ucAutorizadorPRE presentador;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "ucAutorizadorUI";

        /// <summary>
        /// Enumerador de Catalogos para el Buscador
        /// </summary>
        public enum ECatalogoBuscador {
            Sucursal = 0,
            Empleado = 1
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary> 
        public int? UnidadOperativaID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUnidadOperativaID.Value))
                    id = int.Parse(this.hdnUnidadOperativaID.Value.Trim());
                return id;
            }
            set {
                if (value != null)
                    this.hdnUnidadOperativaID.Value = value.ToString();
                else
                    this.hdnUnidadOperativaID.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el identificador del Autorizador de la cual se desean filtrar
        /// </summary>
        public int? AutorizadorID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnAutorizadorId.Value))
                    id = int.Parse(this.hdnAutorizadorId.Value.Trim());
                return id;
            }
            set {
                if (value != null)
                    this.hdnAutorizadorId.Value = value.ToString();
                else
                    this.hdnAutorizadorId.Value = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal seleccionada para el filtro de consulta
        /// </summary>
        public string SucursalNombre {
            get {
                return (String.IsNullOrEmpty(this.txtSucursal.Text)) ? null : this.txtSucursal.Text.Trim().ToUpper();
            }
            set {
                this.txtSucursal.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                            ? value.Trim().ToUpper()
                                            : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la sucursal de la cual se desean filtrar los autorizadores
        /// </summary>
        public int? SucursalID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnSucursalID.Value))
                    id = int.Parse(this.hdnSucursalID.Value.Trim());
                return id;
            }
            set {
                if (value != null)
                    this.hdnSucursalID.Value = value.ToString();
                else
                    this.hdnSucursalID.Value = string.Empty;
            }
        }
        /// <summary>
        /// Tipo de Autorización 
        /// </summary>
        public Enum TipoAutorizacion {
            get {
                Enum tipoAutorizacion = null;
                switch (this.UnidadOperativaID) {
                    case (int)ETipoEmpresa.Generacion:
                        tipoAutorizacion = TipoAutorizacionGeneracion;
                        break;
                    case (int)ETipoEmpresa.Construccion:
                        tipoAutorizacion = TipoAutorizacionConstruccion;
                        break;
                    case (int)ETipoEmpresa.Equinova:
                        tipoAutorizacion = TipoAutorizacionEquinova;
                        break;
                    default:
                        tipoAutorizacion = TipoAutorizacionIdealease;
                        break;
                }
                return tipoAutorizacion;
            }
            set {
                if (value != null) {
                    switch (this.UnidadOperativaID) {
                        case (int)ETipoEmpresa.Generacion:
                            this.ddlTipoAutorizacion.SelectedValue = ((int)(ETipoAutorizacionGeneracion)value).ToString();
                            HabilitarSoloNotificacion((ETipoAutorizacionGeneracion)value == ETipoAutorizacionGeneracion.Tarifa_Renta && this.ModoRegistro != "DET");
                            break;
                        case (int)ETipoEmpresa.Construccion:
                            this.ddlTipoAutorizacion.SelectedValue = ((int)(ETipoAutorizacionConstruccion)value).ToString();
                            HabilitarSoloNotificacion((ETipoAutorizacionConstruccion)value == ETipoAutorizacionConstruccion.Tarifa_Renta && this.ModoRegistro != "DET");
                            break;
                        case (int)ETipoEmpresa.Equinova:
                            this.ddlTipoAutorizacion.SelectedValue = ((int)(ETipoAutorizacionEquinova)value).ToString();
                            HabilitarSoloNotificacion((ETipoAutorizacionEquinova)value == ETipoAutorizacionEquinova.Tarifa_Renta && this.ModoRegistro != "DET");
                            break;
                        default:
                            this.ddlTipoAutorizacion.SelectedValue = ((int)(ETipoAutorizacion)value).ToString();
                            break;
                    }
                } else
                    this.ddlTipoAutorizacion.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Enumerador Turno de Tarifa para Construcción
        /// </summary>
        public ETipoAutorizacion? TipoAutorizacionIdealease {
            get {
                ETipoAutorizacion? eTipoAutorizacion = null;
                if (this.ddlTipoAutorizacion.SelectedIndex > 0)
                    eTipoAutorizacion = (ETipoAutorizacion)Enum.Parse(typeof(ETipoAutorizacion), ddlTipoAutorizacion.SelectedValue);
                return eTipoAutorizacion;
            }
            set {
                ddlTipoAutorizacion.SelectedIndex = (int)(ETipoAutorizacion)value;
            }
        }

        /// <summary>
        /// Enumerador Turno de Tarifa para Construcción
        /// </summary>
        public ETipoAutorizacionConstruccion? TipoAutorizacionConstruccion {
            get {
                ETipoAutorizacionConstruccion? eTipoAutorizacion = null;
                if (this.ddlTipoAutorizacion.SelectedIndex > 0)
                    eTipoAutorizacion = (ETipoAutorizacionConstruccion)Enum.Parse(typeof(ETipoAutorizacionConstruccion), ddlTipoAutorizacion.SelectedValue);
                return eTipoAutorizacion;
            }
            set {
                ddlTipoAutorizacion.SelectedIndex = (int)(ETipoAutorizacionConstruccion)value;
            }
        }

        /// <summary>
        /// Enumerador Turno de Tarifa para Generación
        /// </summary>
        public ETipoAutorizacionGeneracion? TipoAutorizacionGeneracion {
            get {
                ETipoAutorizacionGeneracion? eTipoAutorizacion = null;
                if (this.ddlTipoAutorizacion.SelectedIndex > 0)
                    eTipoAutorizacion = (ETipoAutorizacionGeneracion)Enum.Parse(typeof(ETipoAutorizacionGeneracion), ddlTipoAutorizacion.SelectedValue);
                return eTipoAutorizacion;
            }
            set {
                ddlTipoAutorizacion.SelectedIndex = (int)(ETipoAutorizacionGeneracion)value;
            }
        }

        /// <summary>
        /// Enumerador Turno de Tarifa para Equinova
        /// </summary>
        public ETipoAutorizacionEquinova? TipoAutorizacionEquinova {
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
        public string EmpleadoNombre {
            get {
                string empleadoNombre = null;
                if (this.txtEmpleado.Text.Trim().Length > 0)
                    empleadoNombre = this.txtEmpleado.Text.Trim().ToUpper();
                return empleadoNombre;
            }
            set {
                if (value != null)
                    this.txtEmpleado.Text = value.ToString();
                else
                    this.txtEmpleado.Text = string.Empty;
            }
        }
        /// <summary>
        /// Identificador del Empleado
        /// </summary>
        public int? EmpleadoID {
            get {
                int? empleadoID = null;
                if (!String.IsNullOrEmpty(this.hdnEmpleadoID.Value))
                    empleadoID = int.Parse(this.hdnEmpleadoID.Value.Trim());
                return empleadoID;
            }
            set {
                if (value != null)
                    this.hdnEmpleadoID.Value = value.ToString();
                else
                    this.hdnEmpleadoID.Value = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene  o establece el Email del autorizador
        /// </summary>
        public string Email {
            get {
                return (String.IsNullOrEmpty(txtEmail.Text)) ? null : this.txtEmail.Text.Trim().ToUpper();
            }
            set {
                this.txtEmail.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                              ? value.Trim().ToUpper()
                                              : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene  o establece el Telefono del autorizador
        /// </summary>
        public string Telefono {
            get {
                return (String.IsNullOrEmpty(txtTelefono.Text)) ? null : this.txtTelefono.Text.Trim().ToUpper();
            }
            set {
                this.txtTelefono.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                              ? value.Trim().ToUpper()
                                              : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene  o establece el Tipo de Notificación del autorizador
        /// </summary>
        public bool? SoloNotificacion {
            get {
                if (this.rbNotificacion.SelectedValue == "") return null;
                return rbNotificacion.SelectedValue == "1" ? true : false;
            }
            set {
                if (value == null) return;
                rbNotificacion.SelectedValue = value == true ? "1" : "0";
            }
        }
        public bool? Estatus {
            get {
                bool? estatus = null;
                if (ddlEstatus.SelectedValue.ToString().ToUpper() == "TRUE") estatus = true;
                if (ddlEstatus.SelectedValue.ToString().ToUpper() == "FALSE") estatus = false;
                return estatus;
            }
            set {
                if (value != null)
                    this.ddlEstatus.SelectedValue = value.ToString();
                else
                    this.ddlEstatus.SelectedIndex = -1;
            }
        }
        public string ModoRegistro {
            get {
                return this.hdnModoRegistro.Value != null ? this.hdnModoRegistro.Value : string.Empty;
            }
            set {
                this.hdnModoRegistro.Value = value != null ? value : string.Empty;
            }
        }
        public int? UC {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUC.Value))
                    id = int.Parse(this.hdnUC.Value.Trim());
                return id;
            }
            set {
                if (value != null)
                    this.hdnUC.Value = value.ToString();
                else
                    this.hdnUC.Value = string.Empty;
            }
        }
        public int? UUA {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUUA.Value))
                    id = int.Parse(this.hdnUUA.Value.Trim());
                return id;
            }
            set {
                if (value != null)
                    this.hdnUUA.Value = value.ToString();
                else
                    this.hdnUUA.Value = string.Empty;
            }
        }
        public string UsuarioCreacion {
            set {
                if (value != null)
                    this.txtUsuarioRegistro.Text = value;
                else
                    this.txtUsuarioRegistro.Text = string.Empty;
            }
        }
        public string UsuarioEdicion {
            set {
                if (value != null)
                    this.txtUsuarioEdicion.Text = value;
                else
                    this.txtUsuarioEdicion.Text = string.Empty;
            }
        }
        public DateTime? FC {
            get {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.txtFechaRegistro.Text))
                    temp = DateTime.Parse(this.txtFechaRegistro.Text.Trim());
                return temp;
            }
            set {
                if (value != null)
                    this.txtFechaRegistro.Text = value.Value.ToString();
                else
                    this.txtFechaRegistro.Text = string.Empty;
            }
        }
        public DateTime? FUA {
            get {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.txtFechaEdicion.Text))
                    temp = DateTime.Parse(this.txtFechaEdicion.Text.Trim());
                return temp;
            }
            set {
                if (value != null)
                    this.txtFechaEdicion.Text = value.Value.ToString();
                else
                    this.txtFechaEdicion.Text = string.Empty;
            }
        }

        #region Propiedades Buscador
        public string ViewState_Guid {
            get {
                if (ViewState["GuidSession"] == null) {
                    Guid guid = Guid.NewGuid();
                    ViewState["GuidSession"] = guid.ToString();
                }
                return ViewState["GuidSession"].ToString();
            }
        }
        protected object Session_BOSelecto {
            get {
                object objeto = null;
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (Session[nombreSession] != null)
                    objeto = (Session[nombreSession]);

                return objeto;
            }
            set {
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (value != null)
                    Session[nombreSession] = value;
                else
                    Session.Remove(nombreSession);
            }
        }
        protected object Session_ObjetoBuscador {
            get {
                object objeto = null;
                if (Session[ViewState_Guid] != null)
                    objeto = (Session[ViewState_Guid]);

                return objeto;
            }
            set {
                if (value != null)
                    Session[ViewState_Guid] = value;
                else
                    Session.Remove(ViewState_Guid);
            }
        }
        public ECatalogoBuscador ViewState_Catalogo {
            get {
                return (ECatalogoBuscador)ViewState["BUSQUEDA"];
            }
            set {
                ViewState["BUSQUEDA"] = value;
            }
        }
        #endregion Propiedades Buscador
        #endregion

        #region Constructor
        protected void Page_Load(object sender, EventArgs e) {
            try {
                this.presentador = new ucAutorizadorPRE(this);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear de página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo() {
            this.txtEmail.Text = "";
            this.txtEmpleado.Text = "";
            this.txtFechaEdicion.Text = "";
            this.txtFechaRegistro.Text = "";
            this.txtSucursal.Text = "";
            this.txtTelefono.Text = "";
            this.txtUsuarioEdicion.Text = "";
            this.txtUsuarioRegistro.Text = "";
            this.hdnAutorizadorId.Value = "";
            this.hdnEmpleadoID.Value = "";
            this.hdnSucursalID.Value = "";
            this.hdnUC.Value = "";
            this.hdnUUA.Value = "";

            this.ddlEstatus.SelectedIndex = -1;
            this.ddlTipoAutorizacion.SelectedIndex = -1;
            this.rbNotificacion.SelectedIndex = -1;

            this.txtEmail.Enabled = false;
            this.txtTelefono.Enabled = false;
            this.txtFechaEdicion.Enabled = false;
            this.txtFechaRegistro.Enabled = false;
            this.txtUsuarioEdicion.Enabled = false;
            this.txtUsuarioRegistro.Enabled = false;
            this.ddlEstatus.Enabled = false;
        }
        public void PrepararEdicion() {
            this.txtSucursal.Enabled = false;
            this.txtEmpleado.Enabled = false;
            this.ddlTipoAutorizacion.Enabled = false;
            this.txtEmail.Enabled = false;
            this.txtTelefono.Enabled = false;
            this.txtFechaEdicion.Enabled = false;
            this.txtFechaRegistro.Enabled = false;
            this.txtUsuarioEdicion.Enabled = false;
            this.txtUsuarioRegistro.Enabled = false;
            this.ddlEstatus.Enabled = true;
        }
        public void PrepararVisualizacion() {
            this.txtEmail.Enabled = false;
            this.txtEmpleado.Enabled = false;
            this.txtFechaEdicion.Enabled = false;
            this.txtFechaRegistro.Enabled = false;
            this.txtSucursal.Enabled = false;
            this.txtTelefono.Enabled = false;
            this.txtUsuarioEdicion.Enabled = false;
            this.txtUsuarioRegistro.Enabled = false;
            this.ddlEstatus.Enabled = false;
            this.ddlTipoAutorizacion.Enabled = false;
            this.rbNotificacion.Enabled = false;
        }

        public void EstablecerOpcionesTiposAutorizacion(Dictionary<int, string> tipos) {
            if (ReferenceEquals(tipos, null))
                tipos = new Dictionary<int, string>();

            this.ddlTipoAutorizacion.Items.Clear();
            this.ddlTipoAutorizacion.Items.Add(new ListItem("Seleccione una opción", "-1"));

            this.ddlTipoAutorizacion.DataSource = tipos;
            this.ddlTipoAutorizacion.DataValueField = "key";
            this.ddlTipoAutorizacion.DataTextField = "value";
            this.ddlTipoAutorizacion.DataBind();
        }

        public void HabilitarSoloNotificacion(bool habilitar) {
            this.rbNotificacion.Enabled = habilitar;
        }
        public void HabilitarEstatus(bool habilitar) {
            this.ddlEstatus.Enabled = habilitar;
        }

        public void MostrarEstatus(bool mostrar) {
            this.trEstatus.Visible = mostrar;
        }
        public void MostrarDatosRegistro(bool mostrar) {
            this.trFC.Visible = mostrar;
            this.trUC.Visible = mostrar;
        }
        public void MostrarDatosActualizacion(bool mostrar) {
            this.trFUA.Visible = mostrar;
            this.trUUA.Visible = mostrar;
        }

        public void PermitirSeleccionarSucursal(bool permitir) {
            this.ibtnBuscarSucursal.Enabled = permitir;
            this.ibtnBuscarSucursal.Visible = permitir;
        }
        public void PermitirSeleccionarEmpleado(bool permitir) {
            this.ibtnBuscarEmpleado.Enabled = permitir;
            this.ibtnBuscarEmpleado.Visible = permitir;
        }

        public void LimpiarSesion() {
        }

        /// <summary>
        /// Despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            } else {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        #region Métodos para el Buscador
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="tipoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda) {
            ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
        }
        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo) {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;
        }

        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script) {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }
        #endregion
        #endregion

        #region Eventos
        #region Eventos para el Buscador
        /// <summary>
        /// TextChanged activa el llamado al Buscador para la busqueda de Sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtSucursal_TextChanged(object sender, EventArgs e) {
            try {
                string nombreSucursal = SucursalNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                SucursalNombre = nombreSucursal;
                if (SucursalNombre != null) {
                    this.EjecutaBuscador("Sucursal", ECatalogoBuscador.Sucursal);
                    SucursalNombre = null;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtSucursal_TextChanged" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscarSucursal_Click(object sender, ImageClickEventArgs e) {
            try {
                this.EjecutaBuscador("Sucursal&hidden=0", ECatalogoBuscador.Sucursal);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarSucursal_Click" + ex.Message);
            }
        }

        /// <summary>
        /// TextChanged activa el llamado al Buscador para la busqueda de Empleado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtNombreEmpleado_TextChanged(object sender, EventArgs e) {
            try {
                this.Email = null;
                this.Telefono = null;
                string nombreEmpleado = EmpleadoNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Empleado);

                EmpleadoNombre = nombreEmpleado;
                if (EmpleadoNombre != null) {
                    this.EjecutaBuscador("Empleado", ECatalogoBuscador.Empleado);
                    EmpleadoNombre = null;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar un Empleado", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNombreEmpleado_TextChanged" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Empleado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscarEmpleado_Click(object sender, ImageClickEventArgs e) {
            try {
                this.EjecutaBuscador("Empleado", ECatalogoBuscador.Empleado);

            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al buscar el usuario", ETipoMensajeIU.ERROR, nombreClase + ".ibtnBuscarEmpleado_Click:" + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e) {
            try {
                switch (ViewState_Catalogo) {
                    case ECatalogoBuscador.Sucursal:
                    case ECatalogoBuscador.Empleado:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click: " + ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// Evento que controla la selección del DropDownList de Tipo Autorización.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoAutorización_SelectedIndexChanged(object sender, EventArgs e) {
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativaID;
            switch (empresa) {
                case ETipoEmpresa.Construccion:
                    switch (Convert.ToInt32(ddlTipoAutorizacion.SelectedValue)) {
                        case (int)ETipoAutorizacionConstruccion.Tarifa_Renta:
                            HabilitarSoloNotificacion(true);
                            break;
                        case (int)ETipoAutorizacionConstruccion.RE_Vencimiento:
                        case (int)ETipoAutorizacionConstruccion.Baja_Unidad:
                        case (int)ETipoAutorizacionConstruccion.Entrega_Unidad:
                        case (int)ETipoAutorizacionConstruccion.Recepcion_Unidad:
                        case (int)ETipoAutorizacionConstruccion.Orden_Servicio:
                            HabilitarSoloNotificacion(false);
                            this.rbNotificacion.SelectedIndex = 0;
                            break;
                        case (int)ETipoAutorizacionConstruccion.Cancelar_Solicitud_Pago:
                            HabilitarSoloNotificacion(false);
                            this.rbNotificacion.SelectedIndex = 1;
                            break;
                    }
                    break;
                case ETipoEmpresa.Generacion:
                    switch (Convert.ToInt32(ddlTipoAutorizacion.SelectedValue)) {
                        case (int)ETipoAutorizacionGeneracion.Tarifa_Renta:
                            HabilitarSoloNotificacion(true);
                            break;
                        case (int)ETipoAutorizacionGeneracion.RE_Vencimiento:
                        case (int)ETipoAutorizacionGeneracion.Baja_Unidad:
                        case (int)ETipoAutorizacionGeneracion.Entrega_Unidad:
                        case (int)ETipoAutorizacionGeneracion.Recepcion_Unidad:
                        case (int)ETipoAutorizacionGeneracion.Orden_Servicio:
                            HabilitarSoloNotificacion(false);
                            this.rbNotificacion.SelectedIndex = 0;
                            break;
                        case (int)ETipoAutorizacionGeneracion.Cancelar_Solicitud_Pago:
                            HabilitarSoloNotificacion(false);
                            this.rbNotificacion.SelectedIndex = 1;
                            break;
                    }
                    break;
                case ETipoEmpresa.Equinova:
                    switch (Convert.ToInt32(ddlTipoAutorizacion.SelectedValue)) {
                        case (int)ETipoAutorizacionEquinova.Tarifa_Renta:
                            HabilitarSoloNotificacion(true);
                            break;
                        case (int)ETipoAutorizacionEquinova.RE_Vencimiento:
                        case (int)ETipoAutorizacionEquinova.Baja_Unidad:
                        case (int)ETipoAutorizacionEquinova.Entrega_Unidad:
                        case (int)ETipoAutorizacionEquinova.Recepcion_Unidad:
                        case (int)ETipoAutorizacionEquinova.Orden_Servicio:
                            HabilitarSoloNotificacion(false);
                            this.rbNotificacion.SelectedIndex = 0;
                            break;
                        case (int)ETipoAutorizacionEquinova.Cancelar_Solicitud_Pago:
                            HabilitarSoloNotificacion(false);
                            this.rbNotificacion.SelectedIndex = 1;
                            break;
                    }
                    break;
            }
        }
        #endregion
    }
}