//Satiface al caso de uso CU068 - Catáloglo de Clientes
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Comun.UI {
    public partial class ConsultarCuentaClienteUI : System.Web.UI.Page, IConsultarCuentaClienteVIS {
        #region Atributos
        private ConsultarCuentaClientePRE presentador;
        private string nombreClase = "ConsultarCuentaClienteUI";

        public enum ECatalogoBuscador {
            Cliente
        }
        #endregion

        #region Propiedades
        public UnidadOperativaBO UnidadOperativa {
            get {
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    return new UnidadOperativaBO { Id = masterMsj.Adscripcion.UnidadOperativa.Id };
                return null;
            }
        }
        public List<CuentaClienteIdealeaseBO> ListaClientes {
            get {
                if (Session["ListaClientes"] == null)
                    return new List<CuentaClienteIdealeaseBO>();
                else
                    return (List<CuentaClienteIdealeaseBO>)Session["ListaClientes"];

            }
            set {
                Session["ListaClientes"] = value;
            }
        }
        public string NombreCuenta {
            get { return (String.IsNullOrEmpty(this.txtNombreCuentaCliente.Text.Trim())) ? null : this.txtNombreCuentaCliente.Text.Trim().ToUpper(); }
            set { this.txtNombreCuentaCliente.Text = value != null ? value : string.Empty; }
        }
        public string Nombre {
            get { return (String.IsNullOrEmpty(this.txtNombre.Text.Trim())) ? null : this.txtNombre.Text.Trim(); }
            set { this.txtNombre.Text = value != null ? value : string.Empty; }

        }
        public bool? Fisica {
            set {
                if (value != null) {
                    if (value == true)
                        this.ddlTipoContribuyente.SelectedValue = "FÍSICA";
                    if (value == false)
                        this.ddlTipoContribuyente.SelectedValue = "MORAL";
                } else
                    this.ddlTipoContribuyente.SelectedValue = "";
            }
            get {
                if (this.ddlTipoContribuyente.SelectedValue.Trim().ToUpper() == "FÍSICA")
                    return true;
                if (this.ddlTipoContribuyente.SelectedValue.Trim().ToUpper() == "MORAL")
                    return false;

                return null;
            }

        }
        public string RFC {
            get { return (String.IsNullOrEmpty(this.txtRFC.Text.ToUpper())) ? null : this.txtRFC.Text.ToUpper(); }
            set { this.txtRFC.Text = value != null ? value : string.Empty; }
        }

        #region SC0008
        public int? UsuarioId {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }
        #endregion

        /// <summary>
        /// Identificador de la cuenta del cliente
        /// </summary>
        public int? CuentaClienteID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnCuentaClienteID.Value))
                    id = int.Parse(this.hdnCuentaClienteID.Value.Trim());
                return id;
            }
            set {
                if (value != null)
                    this.hdnCuentaClienteID.Value = value.ToString();
                else
                    this.hdnCuentaClienteID.Value = string.Empty;
            }
        }
        /// <summary>
        /// Identificador del cliente
        /// </summary>
        public int? ClienteID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnClienteID.Value))
                    id = int.Parse(this.hdnClienteID.Value.Trim());
                return id;
            }
            set {
                if (value != null)
                    this.hdnClienteID.Value = value.ToString();
                else
                    this.hdnClienteID.Value = string.Empty;
            }
        }

        #region Propiedades para el Buscador
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
        #endregion
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new ConsultarCuentaClientePRE(this);
                if (!Page.IsPostBack) {
                    presentador.Inicializar();
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistenacias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            Site master = (Site)Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);

            } else {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        public void LimpiarSesion() {
            if (Session["ListaClientes"] != null)
                Session.Remove("ListaClientes");

        }
        public void MostrarDatos() {
            this.grdClientes.DataSource = this.ListaClientes;
            this.grdClientes.DataBind();
        }
        public void RedirigirADetalles() {
            this.Response.Redirect(this.ResolveUrl("~/Comun.UI/DetalleCuentaClienteUI.aspx"));
        }
        public void EstablecerPaqueteNavegacion(string nombre, object valor) {
            Session[nombre] = valor;
        }

        #region SC0008
        public void PermitirRegistrar(bool permitir) {
            this.hlRegistroOrden.Enabled = permitir;
        }
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        #endregion

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
        protected void btnBuscar_Click(object sender, EventArgs e) {
            this.presentador.ConsultarClientes();
        }
        protected void grdClientes_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                grdClientes.DataSource = this.ListaClientes;
                grdClientes.PageIndex = e.NewPageIndex;
                grdClientes.DataBind();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdClientes_PageIndexChanging: " + ex.Message);
            }
        }

        protected void grdClientes_RowCommand(object sender, GridViewCommandEventArgs e) {
            int index;
            if (e.CommandName.ToString().Trim().ToUpper() == "PAGE") return;
            try {
                index = Convert.ToInt32(e.CommandArgument.ToString());
                switch (e.CommandName.Trim()) {
                    case "Detalles": {
                            this.presentador.VerDetalles(index);
                            break;
                        }
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al ejecutar la acción sobre el cliente", ETipoMensajeIU.ERROR, this.nombreClase + ".grdClientes_RowCommand:" + ex.Message);
            }
        }

        #region Eventos para el Buscador
        protected void ibtnBuscarCliente_Click(object sender, EventArgs e) {
            try {
                this.EjecutaBuscador("Cuenta&hidden=0", ECatalogoBuscador.Cliente);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al buscar un cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarCliente_Click:" + ex.Message);
            }
        }
        protected void txtNombreCuentaCliente_TextChanged(object sender, EventArgs e) {
            try {
                string nombreCuentaCliente = this.NombreCuenta;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.Cliente);

                this.NombreCuenta = nombreCuentaCliente;
                if (this.NombreCuenta != null) {
                    EjecutaBuscador("Cuenta&hidden=0", ECatalogoBuscador.Cliente);
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txbNombreCliente_TextChanged:" + ex.Message);
            }
        }
        protected void btnResult_Click(object sender, EventArgs e) {
            try {
                switch (ViewState_Catalogo) {
                    case ECatalogoBuscador.Cliente:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click: " + ex.Message);
            }
        }
        #endregion
        #endregion
    }
}