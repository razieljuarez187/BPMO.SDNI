// Satisface al CU027 - Catálogo Contacto Cliente
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Mantenimiento.PRE;
using System.Configuration;
using BPMO.Facade.SDNI.BOF;

namespace BPMO.SDNI.Mantenimiento.UI {

    /// <summary>
    /// Clase que contiene los métodos para la presentación de la información de Consulta Contacto Cliente Idealease, al usuario.
    /// </summary>
    public partial class ConsultarContactoClienteUI : System.Web.UI.Page, IConsultarContactoClienteVIS {
        
        #region Atributos

        /// <summary>
        /// Presentador que atiende las peticiones de la vista.
        /// </summary>
        private ConsultarContactoClientePRE presentador;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private readonly string nombreClase = typeof(ConsultarContactoClienteUI).Name;

            #region Seguridad

        /// <summary>
        /// Obtiene un valor que representa el Identificador del Módulo desde el cual se está accesando.
        /// </summary>
        public int? ModuloID {
            get {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                    return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);

                return null;
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

            #endregion

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
            get { return Session["sucursalSeleccionada"] as SucursalBO; }
            set { Session["sucursalSeleccionada"] = value; }
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
        /// Obtiene o establece un valor que representa el Cliente Idealease seleccionado.
        /// </summary>
        public CuentaClienteIdealeaseBO ClienteSeleccionado {
            get { return Session["clienteSeleccionado"] as CuentaClienteIdealeaseBO; }
            set { Session["clienteSeleccionado"] = value; }
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

        /// <summary>
        /// Obtiene un valor que representa el Estado del Contacto Cliente Idealease a buscar.
        /// </summary>
        public bool? Activo {
            get {
                bool? activo = null;
                if (ddlActivo.SelectedIndex > 0)
                    activo = Convert.ToBoolean(this.ddlActivo.SelectedValue);
                return activo;
            }
        }

            #endregion

            #region Grid Resultado Consulta

        /// <summary>
        /// Obtiene o establece un valor que representa el Contacto Cliente Idealease seleccionado.
        /// </summary>
        public ContactoClienteBO ContactoClienteSeleccionado {
            get { return Session["contactoClienteSeleccionado"] as ContactoClienteBO; }
            set { Session["contactoClienteSeleccionado"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la Lista de Contactos Clientes Idealease encontrados.
        /// </summary>
        public List<ContactoClienteBO> Contactos {
            get { return Session["contactosClientes"] as List<ContactoClienteBO>; }
            set { Session["contactosClientes"] = value; }
        }

            #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Método delegado para el evento de carga de la página.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void Page_Load(object sender, EventArgs e) {
            try {
                this.presentador = new ConsultarContactoClientePRE(this);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }

            if (!IsPostBack) {
                this.presentador.PrepararSeguridad();
                IniciarCampos();
            }
            DesplegarListaContactosCliente();
        }

        #endregion

        #region Métodos

            #region Form Búsqueda

                #region Buscador

        /// <summary>
        /// Ejecuta el buscador.
        /// </summary>
        /// <param name="catalogo">Nombre del xml.</param>
        /// <param name="catalogoBusqueda">Nombre del catálogo.</param>
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
            } catch(Exception e) {
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
                this.MostrarMensaje("Inconsistencia al buscar un Cliente Idealease", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnBuscarCliente_Click" + ex.Message);
            }
        }

                #endregion

        /// <summary>
        /// Verifica que el Nombre del Cliente o el Nombre de la Sucursal han cambiado. En caso de no seleccionar algún Objeto del Buscador 
        /// se establece como nulo el objeto seleccionado.
        /// </summary>
        /// <returns>Retorna True si el Nombre del Objeto seleccionado no es igual al Nombre obteniedo de la UI o no se seleccionado algún Objeto del Buscador, 
        /// en caso contrario retorna False.</returns>
        private bool ValidarFiltro(){
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
                if(filtro == null || filtro.Id == null) {
                    return true;
                } else {
                    return !filtro.Nombre.ToUpper().Equals(nombre.ToUpper());
                }
            } else if(filtro != null && filtro.Id != null) {
                if (filtro.GetType() == typeof(SucursalBO)) {
                    SucursalSeleccionada = null;
                } else {
                    ClienteSeleccionado = null;
                }
            }
            return false;
        }

            #endregion

            #region Grid Resultado Búsqueda

        /// <summary>
        /// Realiza la vinculación de la lista de Contactos Clientes Idealease encontrados con la UI.
        /// </summary>
        public void DesplegarListaContactosCliente() {
            this.gvContactoCliente.DataSource = this.Contactos;
            this.gvContactoCliente.DataBind();
        }

        /// <summary>
        /// Realiza la redirección al visor de Detalle Contacto Cliente Idealease.
        /// </summary>
        private void RedirigirADetallesContactoCliente() {
            this.Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/DetalleContactoClienteUI.aspx"));
        }

            #endregion

        /// <summary>
        /// Crea una nueva instancia del Contacto Cliente Idealease seleccionado y la lista de Contactos Clientes Idealease encontrados.
        /// </summary>
        public void IniciarCampos() {
            ContactoClienteSeleccionado = new ContactoClienteBO();
            if (Session["recargarAuditorias"] == null) {
                ClienteSeleccionado = null;
                Contactos = new List<ContactoClienteBO>();
            }else{
                Session["recargarAuditorias"] = null;
            }
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

        /// <summary>
        /// Realiza la redirección a la página de advertencia por falta de permisos.
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        
        #endregion

        #region Eventos

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
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + " .btnResult_Click " + ex.Message);
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
                this.MostrarMensaje("Inconsistencia al buscar un Cliente Idealease", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtNombreCliente_TextChanged" + ex.Message);
            }
        }

        /// <summary>
        /// Evento que activa el llamado al Buscador de Clientes Idealease.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnBuscarCliente_Click(object sender, ImageClickEventArgs e) {
            DesplegarBusquedaCliente();
        }

                #endregion

        /// <summary>
        /// Realiza la consulta de Contactos Clientes Idealease. 
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void BuscarContactosCliente_Click(object sender, EventArgs e) {
            if (ValidarFiltro()) {
                return;
            }
            presentador.MostrarDatosInterfaz();
        }

            #endregion

            #region Grid Resultado Búsqueda

        /// <summary>
        /// Despliega los Contactos Cliente Idealease encontrados en su página correspondiente.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void gvContactoCliente_PageIndexChanging(Object sender, GridViewPageEventArgs e) {
            try {
                this.gvContactoCliente.PageIndex = e.NewPageIndex;
                DesplegarListaContactosCliente();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".gvContactoCliente_PageIndexChanging:" + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene el Contacto Cliente Idealease seleccionado, carga sus detalles completos y redirecciona al visor de Detalle Contacto Cliente Idealease.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void gvContactoCliente_RowCommand(Object sender, GridViewCommandEventArgs e) {
            int index;

            try {
                if (!string.IsNullOrEmpty(e.CommandArgument.ToString())) {
                    index = Convert.ToInt32(e.CommandArgument.ToString()) + (((GridView)sender).PageIndex * ((GridView)sender).PageSize);
                    this.ContactoClienteSeleccionado = this.Contactos[index];
                    switch(e.CommandName.Trim()) {
                        case "Ver":
                            presentador.ConsultarContactoClienteCompleto();
                            RedirigirADetallesContactoCliente();
                        break;
                    }
                }
            }
            catch (Exception er) { }
        }

            #endregion

        #endregion
    }
}