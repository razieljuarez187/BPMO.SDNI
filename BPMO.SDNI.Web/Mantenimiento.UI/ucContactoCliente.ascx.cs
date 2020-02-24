// Satisface al CU027 - Catálogo Contacto Cliente
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Mantenimiento.UI {

    /// <summary>
    /// Clase que contiene los métodos para la presentación de la información de Registro, Edición, Actualización, Eliminación y Detalle del 
    /// Contacto Cliente Idealease seleccionado.
    /// </summary>
    public partial class ucContactoCliente : System.Web.UI.UserControl, IucContactoClienteVIS {

        #region Atributos

        /// <summary>
        /// Presentador que atiende las peticiones de la vista.
        /// </summary>
        private ucContactoClientePRE presentador;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private readonly string nombreClase = typeof(ucContactoCliente).Name;

            #region Seguridad

        /// <summary>
        /// Obtiene un valor que representa el identificador del Usuario Actual de la sesión en curso.
        /// </summary>
        public int? UsuarioAutenticado {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el Identificador de la Unidad Operativa Actual de la sesión en curso.
        /// </summary>
        public int? UnidadOperativaId {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la Configuración de la Unidad Operativa que indica a que libro corresponden los activos.
        /// </summary>
        public string LibroActivos {
            get {
                string valor = null;
                if (this.hdnLibroActivos.Value.Trim().Length > 0)
                    valor = this.hdnLibroActivos.Value.Trim().ToUpper();
                return valor;

            }
            set {
                if (value != null)
                    this.hdnLibroActivos.Value = value.ToString();
                else
                    this.hdnLibroActivos.Value = string.Empty;
            }
        }

            #endregion

            #region Contacto Cliente Idealease

        /// <summary>
        /// Obtiene o establece un valor que representa al Temporal Contacto Cliente Idealease seleccionado.
        /// </summary>
        public ContactoClienteBO ContactoClienteSeleccionadoEdicion {
            get { return Session["contactoClienteSeleccionadoEdicion"] as ContactoClienteBO; }
            set { Session["contactoClienteSeleccionadoEdicion"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa al Contacto Cliente Idealease seleccionado.
        /// </summary>
        private ContactoClienteBO ContactoClienteSeleccionado {
            get { return Session["contactoClienteSeleccionado"] as ContactoClienteBO; }
        }

                #region Form Búsqueda
        
                    #region Buscador

        /// <summary>
        /// Enumerador de Catálogos para el Buscador.
        /// </summary>
        public enum ECatalogoBuscador {
            Cliente,
            Sucursal
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Enumerador que contiene los buscadores existentes en la UI.
        /// </summary>
        public ECatalogoBuscador ViewState_Catalogo {
            get { return (ECatalogoBuscador)ViewState["BUSQUEDA"]; }
            set { ViewState["BUSQUEDA"] = value; }
        }

        /// <summary>
        /// Obtiene un valor que representa un identificador único para la UI.
        /// </summary>
        public string ViewState_Guid {
            get {
                if (ViewState["GuidSession"] == null) {
                    Guid guid = Guid.NewGuid();
                    ViewState["GuidSession"] = guid.ToString();
                }
                return ViewState["GuidSession"].ToString();
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Objeto con la información de filtrado para el buscador.
        /// </summary>
        protected object Session_ObjetoBuscador {
            get {
                object objeto = null;
                if (Session[ViewState_Guid] != null)
                    objeto = (Session[ViewState_Guid] as object);

                return objeto;
            }
            set {
                if (value != null)
                    Session[ViewState_Guid] = value;
                else
                    Session.Remove(ViewState_Guid);
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Objeto que fue seleccionado del buscador.
        /// </summary>
        protected object Session_BOSelecto {
            get {
                object objeto = null;
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (Session[nombreSession] != null)
                    objeto = (Session[nombreSession] as object);

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

                    #endregion

                    #region Filtro Sucursal

        /// <summary>
        /// Obtiene o establece un valor que representa la Sucursal seleccionada.
        /// </summary>
        public SucursalBO SucursalSeleccionada {
            get { return Session["ucSucursalSeleccionada"] as SucursalBO; }
            set { Session["ucSucursalSeleccionada"] = value; }
        }

        /// <summary>
        /// Obtiene o estable el nombre de la Sucursal a buscar o la seleccionada.
        /// </summary>
        public string NombreSucursal { 
            get {
                String nombreSucursal = null;
                if (this.txtSucursal.Text.Trim().Length > 0)
                    nombreSucursal = this.txtSucursal.Text.Trim().ToUpper();
                return nombreSucursal;
            }
            set {
                if (value != null)
                    this.txtSucursal.Text = value.ToString();
                else
                    this.txtSucursal.Text = String.Empty;
            }
        }

                    #endregion

                    #region Filtro Cliente Idealease

        /// <summary>
        /// Obtiene o establece un valor que representa al Cliente Idealease seleccionado.
        /// </summary>
        public CuentaClienteIdealeaseBO ClienteSeleccionado{
           get {return Session["ucClienteSeleccionado"] as CuentaClienteIdealeaseBO;}
           set {Session["ucClienteSeleccionado"] = value;}
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Nombre del Cliente Idelaease a buscar o el seleccionado.
        /// </summary>
        public string NombreCliente { 
            get {
                String nombreCliente = null;
                if (this.txtNombreCliente.Text.Trim().Length > 0)
                    nombreCliente = this.txtNombreCliente.Text.Trim().ToUpper();
                return nombreCliente;
            }
            set {
                if (value != null)
                    this.txtNombreCliente.Text = value.ToString();
                else
                    this.txtNombreCliente.Text = String.Empty;
            }
        }

                    #endregion
            
                #endregion

            #endregion

            #region Detalle Contacto Cliente Idealease

        /// <summary>
        /// Obtiene o establece un valor que representa el Detalle Contacto Cliente Idealease seleccionado.
        /// </summary>
        public DetalleContactoClienteBO DetalleSeleccionado {
            get { return Session["ucDetalleSeleccionado"] as DetalleContactoClienteBO; }
            set { Session["ucDetalleSeleccionado"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa un bit indicando que se esta editando o no el Detalle Contacto Cliente Idealease.
        /// </summary>
        public bool EditandoDetalle {
            get { return Boolean.Parse(Session["editandoDetalle"].ToString()); }
            set { Session["editandoDetalle"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Nombre o Puesto del Contacto Cliente Idealease.
        /// </summary>
        private string NombreContacto {
            get {
                String nombre = null;
                if (this.txtNombreContacto.Text.Trim().Length > 0)
                    nombre = this.txtNombreContacto.Text.Trim().ToUpper();
                return nombre;
            }
            set {
                if (value != null)
                    this.txtNombreContacto.Text = value.ToString();
                else
                    this.txtNombreContacto.Text = String.Empty;
            }
        }
        
        /// <summary>
        /// Obtiene o establece un valor que representa el Teléfono del Contacto Cliente Idealease.
        /// </summary>
        private string Telefono {
            get {
               String telefono = null;
                if(txtTelefono.Text.Trim().Length > 0)
                    telefono = txtTelefono.Text;
                return telefono;
            }
            set {
                if(value != null)
                    txtTelefono.Text = value.ToString();
                else
                    txtTelefono.Text = String.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Correo del Contacto Cliente Idealease.
        /// </summary>
        private string Correo {
            get {
                String correo = null;
                if (txtCorreo.Text.Trim().Length > 0)
                    correo = txtCorreo.Text;
                return correo;
            }
            set {
                if (value != null)
                    txtCorreo.Text = value.ToString();
                else
                    txtCorreo.Text = String.Empty;
            }
        }
        
        /// <summary>
        /// Obtiene o establece un valor que representa un bit indicando si se envía o no el Correo.
        /// </summary>
        private bool EnviarCorreo {
            get { return cbEnvioCorreo.Checked; }
            set { cbEnvioCorreo.Checked = value; }
        }

            #endregion

        /// <summary>
        /// Obtiene o establece un valor que representa el Tipo de UI a desplegar.
        /// </summary>
        private String TipoUI {
            get { return Session["tipoUI"].ToString(); }
        }  

        #endregion

        #region Constructor

        /// <summary>
        /// Método delegado para el evento de carga de la página.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void Page_Load(object sender, EventArgs e) {
            presentador = new ucContactoClientePRE(this);
            if (!IsPostBack) {
                PrepararBusqueda();
                IniciarCamposSesion();
            }
            CargarDetalles();
        }

        #endregion

        #region Métodos

            #endregion

            #region Contacto Cliente Idealease

        /// <summary>
        /// Limpia los campos de Sucursal y Cliente Idealease.
        /// </summary>
        private void PrepararBusqueda() {
            if(TipoUI.Equals("Agregar")){
                this.txtSucursal.Text = "";
                this.txtNombreCliente.Text = "";
            }
        }

        /// <summary>
        /// Obtiene un nuevo Contacto Cliente Idealease con la información de la UI.
        /// </summary>
        public void FormToContactoCliente() {
            ContactoClienteSeleccionado.CuentaClienteIdealease = ClienteSeleccionado;
            ContactoClienteSeleccionado.Sucursal = SucursalSeleccionada;
            ContactoClienteSeleccionado.Direccion = txtDireccion.Text.ToUpper();
        }

        /// <summary>
        /// Carga los detalles completos del Contacto Cliente Idealease a la UI.
        /// </summary>
        public void ContactoClienteToForm() {
            SucursalSeleccionada = ContactoClienteSeleccionado.Sucursal;
            NombreSucursal = ContactoClienteSeleccionado.Sucursal.Nombre;
            ClienteSeleccionado = ContactoClienteSeleccionado.CuentaClienteIdealease;
            NombreCliente = ContactoClienteSeleccionado.CuentaClienteIdealease.Nombre;
            txtDireccion.Text = ContactoClienteSeleccionado.Direccion;
            CargarDetalles();
        }

        /// <summary>
        /// Verifica que el Nombre del Cliente o el Nombre de la Sucursal han cambiado. En caso de no seleccionar algún Objeto del Buscador 
        /// se establece como nulo el objeto seleccionado.
        /// </summary>
        /// <returns>Retorna True si el Nombre del Objeto seleccionado no es igual al Nombre obteniedo de la UI o no se seleccionado algún Objeto del Buscador, 
        /// en caso contrario retorna False.</returns>
        public bool ValidarForm(){
            if(ValidarFiltro(ClienteSeleccionado, NombreCliente)){
                MostrarMensaje("Debes seleccionar un Cliente Idealease", ETipoMensajeIU.ADVERTENCIA);
                return true;
            }
            if (ValidarFiltro(SucursalSeleccionada, NombreSucursal)) {
                MostrarMensaje("Debes seleccionar una Sucursal", ETipoMensajeIU.ADVERTENCIA);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Verifica que el Objeto seleccionado y el Nombre del Objeto a buscar sean iguales. En caso de no seleccionar algún Objeto del Buscador 
        /// se establece como nulo el objeto seleccionado.
        /// </summary>
        /// <param name="filtro">El Objeto seleccionado.</param>
        /// <param name="nombre">El Nombre del Objeto a buscar.</param>
        /// <returns>Retorna True si el Nombre del Objeto seleccionado no es igual al Nombre obteniedo de la UI o no se seleccionado algún Objeto del Buscador, 
        /// en caso contrario retorna False.</returns>
        private bool ValidarFiltro(CatalogoBaseBO filtro, string nombre) {
            if (nombre != null && nombre.Trim() != null && !nombre.Equals("")) {
                if( filtro == null || filtro.Id == null){
                    return true;
                } else {
                    return !filtro.Nombre.ToUpper().Equals(nombre.ToUpper());
                }
            }else if(filtro != null && filtro.Id != null){
                if(filtro.GetType() == typeof(SucursalBO)){
                    ContactoClienteSeleccionado.Sucursal = null;
                } else{
                    ContactoClienteSeleccionado.CuentaClienteIdealease = null;
                }
                filtro = null;
            }
            return false;
        }

        /// <summary>
        /// Verifica que el campo no sea nulo o contenga el carácter vacío.
        /// </summary>
        /// <param name="parametro">El campo a validar.</param>
        /// <returns>Retorna True si es nulo o tiene el carácter vacío.</returns>
        private bool ValidarString(string param) {
            return param == null || param.Equals(string.Empty) || param.Trim().Equals(string.Empty);
        }

                #region Form Búsqueda

                    #region Buscador

        /// <summary>
        /// Ejecuta el buscador.
        /// </summary>
        /// <param name="catalogo">Nombre del xml.</param>
        /// <param name="catalogoBusqueda">Nombre catálogo.</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda) {
            try {
                ViewState_Catalogo = catalogoBusqueda;
                this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
                this.Session_BOSelecto = null;
                this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
            } catch (Exception ex) {
                this.MostrarMensaje("Error al consultar unidades", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        /// <summary>
        /// Despliega la información del Objeto Seleccionado.
        /// </summary>
        /// <param name="catalogo">Nombre del catálogo.</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo) {
            this.presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;
        }

                    #endregion

                    #region Filtro Sucursal

        /// <summary>
        /// Activa el llamado al Buscador de Sucursales.
        /// </summary>
        /// <param name="campo">Nombre del campo que desencadenó el evento.</param>
        private void DesplegarBusquedaSucursal(string campo) {
            try {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            }catch(Exception e){
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + campo + e.Message);
            }
        }

                    #endregion

                    #region Filtro Cliente Idealease

        /// <summary>
        /// Activa el llamado al Buscador de Clientes Idealease.
        /// </summary>
        private void DesplegarBusquedaCliente() { 
            if(NombreCliente == null || NombreCliente.Equals(string.Empty)){
                this.MostrarMensaje("Es necesario escribir al menos un carácter.", ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            try {
                this.EjecutaBuscador("CuentaClienteIdealease&hidden=0", ECatalogoBuscador.Cliente);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una cuenta de cliente Idealease", ETipoMensajeIU.ADVERTENCIA, nombreClase + " .btnBuscarCliente_Click " + ex.Message);
            }
        }

                    #endregion

                #endregion

            #endregion

            #region Detalle Contacto Cliente Idealease

                #region Form Detalle

        /// <summary>
        /// Obtiene y establece un nuevo Detalle Contacto Cliente Idealease con la información de la UI.
        /// </summary>
        /// <returns>Un Objeto de tipo DetalleContactoClienteBO.</returns>
        private DetalleContactoClienteBO FormDetalleToDetalleContactoCliente() {
            DetalleContactoClienteBO detalle = new DetalleContactoClienteBO() { 
                Nombre = this.NombreContacto,
                Correo = this.Correo,
                Telefono = this.Telefono,
                RecibeCorreoElectronico = this.EnviarCorreo
            };
            if (DetalleSeleccionado != null && DetalleSeleccionado.Id != null) { 
                detalle.Id = DetalleSeleccionado.Id;
                detalle.ContactoCliente = DetalleSeleccionado.ContactoCliente;
            }
            return detalle;
        }

        /// <summary>
        /// Establece los datos del Detalle Contacto Cliente Idealease a la UI.
        /// </summary>
        private void DetalleContactoClienteToForm() {
            NombreContacto = DetalleSeleccionado.Nombre;
            Correo = DetalleSeleccionado.Correo;
            Telefono = DetalleSeleccionado.Telefono;
            EnviarCorreo = DetalleSeleccionado.RecibeCorreoElectronico.Value;
        }

                #endregion

                #region Grid Detalles

        /// <summary>
        /// Realiza la carga de los Detalles Contacto Cliente Idealease.
        /// </summary>
        public void CargarDetalles() {
            gvDetalles.DataSource = ContactoClienteSeleccionado.Detalles;
            gvDetalles.DataBind();
        }

        /// <summary>
        /// Valida los campos obligatorios del Detalle Contacto Cliente Idealease.
        /// </summary>
        /// <returns>Retorna True si algún campo es vacío o nulo, en caso contrario retorna False.</returns>
        private bool ValidarFormDetalle() {
            return ValidarString(NombreContacto) || ValidarString(Telefono) || ValidarString(Correo);
        }

        /// <summary>
        /// Limpia los campos del Detalle Contacto Cliente Idealease.
        /// </summary>
        private void LimpiarFormDetalles() {
            this.NombreContacto = "";
            this.Correo = "";
            this.Telefono = "";
            this.EnviarCorreo = true;
        }

        /// <summary>
        /// Elimina el Detalle Contacto Cliente Idealease del Contacto Cliente Idealease seleccionado.
        /// </summary>
        public void EliminarDetalle() {
            presentador.EliminarDetalle();
            ContactoClienteSeleccionado.Detalles.Remove(DetalleSeleccionado);
            CargarDetalles();
        }

        /// <summary>
        /// Elimina el Detalle Contacto Cliente Idealease del Contacto Cliente Idealease Temporal.
        /// </summary>
        public void RecargarContactoClienteTemp() {
            DetalleContactoClienteBO detalleAEliminar = ContactoClienteSeleccionadoEdicion.Detalles.Find(x => x.Id == DetalleSeleccionado.Id);
            ContactoClienteSeleccionadoEdicion.Detalles.Remove(detalleAEliminar);
        }

                #endregion

            #endregion

        /// <summary>
        /// Crea una nueva instancia del Cliente Idealease seleccionado, la Sucursal seleccionada y el Detalle Contacto Cliente Idealease.
        /// </summary>
        private void IniciarCamposSesion() {
            switch(TipoUI){
                case "Agregar":
                    LimpiarFormDetalles();
                    ClienteSeleccionado = null;
                    SucursalSeleccionada = null;
                    Session["contactoClienteSeleccionado"] = new ContactoClienteBO() { 
                        Detalles = new List<DetalleContactoClienteBO>()
                    };
                break;
                case "Actualizar":
                    LimpiarFormDetalles();
                    EditandoDetalle = false;
                    if (ContactoClienteSeleccionado.Detalles == null) {
                        ContactoClienteSeleccionado.Detalles = new List<DetalleContactoClienteBO>();
                    }
                break;
            }
        }

        /// <summary>
        /// Habilita o inhabilita los campos del Contacto Cliente Idealease y el Detalle Contacto Cliente Idealease.
        /// </summary>
        /// <param name="bloquear"></param>
        public void SetBloqueoCampos(bool bloquear) {
            txtNombreCliente.ReadOnly = true;
            txtNombreCliente.Enabled = false;
            btnBuscarCliente.Visible = false;
            txtSucursal.ReadOnly = true;
            txtSucursal.Enabled = false;
            btnBuscarSucursal.Visible = false;
            txtDireccion.ReadOnly = bloquear;
            txtDireccion.Enabled = !bloquear;
            trNombre.Visible = !bloquear;
            trTelefono.Visible = !bloquear;
            trCorreo.Visible = !bloquear;
            trEnviarCorreo.Visible = !bloquear;
            trAgregarATabla.Visible = !bloquear;
            divCamposRequeridosDetalles.Visible = !bloquear;
        }

        /// <summary>
        /// Despliega un mensaje en pantalla.
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar.</param>
        /// <param name="tipo">Tipo de mensaje a desplegar.</param>
        /// <param name="msjDetalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar.</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null) {
            Site masterMsj = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
            } else {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// Registra un Script en el cliente.
        /// </summary>
        /// <param name="key">Llave del Script.</param>
        /// <param name="script">Script a registrar.</param>
        private void RegistrarScript(string key, string script) {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        #region Eventos

            #region Contacto Cliente Idealease

                #region Form Búsqueda

                    #region Buscador

        /// <summary>
        /// Establece el Objeto Seleccionado del Buscador.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnResult_Click(object sender, EventArgs e) {
            try {
                switch (ViewState_Catalogo) {
                    case ECatalogoBuscador.Cliente:
                    case ECatalogoBuscador.Sucursal:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + " .btnResult_Click " + ex.Message);
            }
        }

                    #endregion

                    #region Filtro Sucursal

        /// <summary>
        /// Evento que activa el llamado al Buscador de Sucursales.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void txtSucursal_TextChanged(object sender, EventArgs e) {
            DesplegarBusquedaSucursal(" .txtSucursal_TextChanged ");
        }

        /// <summary>
        /// Evento que activa el llamado al Buscador de Sucursales.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnBuscarSucursal_Click(object sender, EventArgs e) {
            DesplegarBusquedaSucursal(" .btnBuscarSucursal_Click ");
        }

                    #endregion

                    #region Filtro Cliente Idealease

        /// <summary>
        /// Evento que activa el llamado al Buscador de Clientes Idealease.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void txtNombreCliente_TextChanged(object sender, EventArgs e) { 
            if(NombreCliente == null || NombreCliente.Equals(string.Empty)){
                this.MostrarMensaje("Es necesario escribir al menos un carácter.", ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            try {
                this.EjecutaBuscador("CuentaClienteIdealease&hidden=0", ECatalogoBuscador.Cliente);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una cuenta de cliente Idealease", ETipoMensajeIU.ADVERTENCIA, nombreClase + " .txtNombreCliente_TextChanged " + ex.Message);
            }
        }

        /// <summary>
        /// Evento que activa el llamado al Buscador de Clientes Idealease.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnBuscarCliente_Click(object sender, EventArgs e) {
            DesplegarBusquedaCliente();
        }

                    #endregion

                #endregion

            #endregion

            #region Detalle Contacto Cliente Idealease

        /// <summary>
        /// Agrega o Edita un Detalle Contacto Cliente Idealease. Valida los campos obligatorios. Verifica que el Detalle Contacto Cliente 
        /// Idealease no haya sido agregado con anterioridad.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void AgregarATabla_Click(object sender, EventArgs e) {
            switch(TipoUI){
                case "Agregar":
                case "Actualizar":
                    if (ValidarFormDetalle()) {
                        MostrarMensaje("Algunos campos del Detalle Contacto Cliente son obligatorios", ETipoMensajeIU.ADVERTENCIA);
                        return;
                    }
                    DetalleContactoClienteBO nuevoDetalle = FormDetalleToDetalleContactoCliente();
                    DetalleContactoClienteBO encontrado = ContactoClienteSeleccionado.Detalles.Find(
                        x => x.Nombre.Equals(nuevoDetalle.Nombre) && x.Telefono.Equals(nuevoDetalle.Telefono) && x.Correo.Equals(nuevoDetalle.Correo)
                        );
                    if (encontrado == null) {
                        ContactoClienteSeleccionado.Detalles.Add(nuevoDetalle);
                        CargarDetalles();
                        LimpiarFormDetalles();
                        if (TipoUI.Equals("Actualizar")) {
                            EditandoDetalle = false;
                        }
                    } else {
                        MostrarMensaje("Este Detalle de Contacto Cliente ya ha sido agregado con anterioridad", ETipoMensajeIU.ADVERTENCIA);
                    }
                break;
            }
        }

                #region Grid Detalles

        /// <summary>
        /// Despliega los Detalles Contactos Cliente Idealease en su página correspondiente.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void gvDetalles_PageIndexChanging(Object sender, GridViewPageEventArgs e) {
            try {
                this.gvDetalles.PageIndex = e.NewPageIndex;
                CargarDetalles();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, ".gvDetalles_PageIndexChanging:" + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene el Detalle Contacto Cliente Idealease seleccionado para Eliminación o Edición. En caso de Eliminación se 
        /// despliega un mensaje de confirmación, en caso de aceptar se elimina el Detalle Contacto Cliente Idealease. En caso de Edición 
        /// se cargan los datos del Detalle Contacto Cliente Idealease.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void gvDetalles_RowCommand(object sender, GridViewCommandEventArgs e) {
            int index;
            try {
                if (!string.IsNullOrEmpty(e.CommandArgument.ToString())) {
                    index = Convert.ToInt32(e.CommandArgument.ToString()) + (((GridView)sender).PageIndex * ((GridView)sender).PageSize);
                    DetalleSeleccionado = this.ContactoClienteSeleccionado.Detalles[index];
                    switch (TipoUI) {
                        case "Agregar":
                        case "Actualizar":
                            switch (e.CommandName.Trim()) {
                                case "Eliminar":
                                    if(DetalleSeleccionado.Id != null) {
                                        this.RegistrarScript("ConfirmarEliminacionDetalle", "confirmarEliminarDetalle('" + "eliminar el Detalle de Contacto Cliente" + "');");
                                    } else {
                                        ContactoClienteSeleccionado.Detalles.Remove(DetalleSeleccionado);
                                        CargarDetalles();
                                    }
                                    break;
                                case "Editar":
                                    if (EditandoDetalle) {
                                        MostrarMensaje("Primero debes agregar el detalle seleccionado con anterioridad.", ETipoMensajeIU.ADVERTENCIA);
                                        return;
                                    }
                                    EditandoDetalle = true;
                                    ContactoClienteSeleccionado.Detalles.Remove(DetalleSeleccionado);
                                    CargarDetalles();
                                    DetalleContactoClienteToForm();
                                    break;
                            }
                            break;
                    }
                }
            } catch(Exception ex) { }
        }

                #endregion

            #endregion

        #endregion

    }
}