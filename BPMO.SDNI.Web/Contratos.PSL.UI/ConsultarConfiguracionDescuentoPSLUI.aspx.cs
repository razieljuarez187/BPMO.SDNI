using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;


namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ConsultarConfiguracionDescuentoPSLUI : System.Web.UI.Page, IConsultarConfiuracionDescuentoPSLVIS {
        #region Atributos

        /// <summary>
        /// Instancia al presentador
        /// </summary>
        private ConsultarConfiguracionDescuentoPSLPRE presentador = null;
        /// <summary>
        /// Nombre de la clase para indicar si ocurre un error.
        /// </summary>
        private string nombreClase = "ConsultarConfiguracionDescuentoUI";

        /// <summary>
        /// Catálogo para el buscador.
        /// </summary>
        public enum ECatalogoBuscador {
            Sucursal,
            CuentaClienteIdealease
        }
        #endregion

        #region Constructores

        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new ConsultarConfiguracionDescuentoPSLPRE(this);
                if (!IsPostBack) {
                    this.presentador.PrepararBusqueda();
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }
        }

        #endregion

        #region Propiedades


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
        public ECatalogoBuscador ViewState_Catalogo {
            get {
                return (ECatalogoBuscador)ViewState["BUSQUEDA"];
            }
            set {
                ViewState["BUSQUEDA"] = value;
            }
        }

        /// <summary>
        /// usuario que se encuentra logueado en el sistema.
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
        /// Unidad operativa del usuario.
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
        /// ModuloID desde la página maestra.
        /// </summary>
        public int? ModuloID {
            get {
                return ((Site)Page.Master).ModuloID;
            }
        }

        /// <summary>
        /// Libro activo
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
        /// Nombre del cliente
        /// </summary>
        public string NombreCliente {
            get {
                String nombreCliente = null;
                if (this.txtCliente.Text.Trim().Length > 0)
                    nombreCliente = this.txtCliente.Text.Trim().ToUpper();
                return nombreCliente;
            }
            set {
                if (value != null)
                    this.txtCliente.Text = value.ToString();
                else
                    this.txtCliente.Text = String.Empty;
            }
        }

        /// <summary>
        /// Cliente ID tomado con base al buscador
        /// </summary>
        public int? ClienteID {
            get {
                int? clienteID = null;
                if (!String.IsNullOrEmpty(this.hdnClienteID.Value))
                    clienteID = int.Parse(this.hdnClienteID.Value.Trim());
                return clienteID;
            }
            set {
                if (value != null)
                    this.hdnClienteID.Value = value.ToString();
                else
                    this.hdnClienteID.Value = string.Empty;
            }
        }

        /// <summary>
        /// Nombre de la sucursal
        /// </summary>
        public string Sucursal {
            get {
                String sucursal = null;
                if (this.txtSucursal.Text.Trim().Length > 0)
                    sucursal = this.txtSucursal.Text.Trim().ToUpper();
                return sucursal;
            }
            set {
                if (value != null)
                    this.txtSucursal.Text = value.ToString();
                else
                    this.txtSucursal.Text = String.Empty;
            }
        }

        /// <summary>
        /// Id de la sucursal
        /// </summary>
        public int? SucursalID {
            get {
                int? sucursalID = null;
                if (!String.IsNullOrEmpty(this.hdnSucursalID.Value))
                    sucursalID = int.Parse(this.hdnSucursalID.Value.Trim());
                return sucursalID;
            }
            set {
                if (value != null)
                    this.hdnSucursalID.Value = value.ToString();
                else
                    this.hdnSucursalID.Value = string.Empty;
            }
        }

        /// <summary>
        /// Fecha de inicio
        /// </summary>
        public DateTime? FechaInicial {
            get {
                if (!string.IsNullOrEmpty(this.txtFechaInicial.Text) && !string.IsNullOrWhiteSpace(this.txtFechaInicial.Text)) {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.txtFechaInicial.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtFechaInicial.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }

        }

        /// <summary>
        /// Fecha final
        /// </summary>
        public DateTime? FechaFinal {
            get {
                if (!string.IsNullOrEmpty(this.txtFechaFinal.Text) && !string.IsNullOrWhiteSpace(this.txtFechaFinal.Text)) {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.txtFechaFinal.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtFechaFinal.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }

        }

        /// <summary>
        /// Estatus (Activo/Inactivo)
        /// </summary>
        public int? Estatus {
            get {
                int? estatus = null;

                estatus = Int32.Parse(this.ddlEstatus.SelectedValue);

                return estatus;
            }
            set {
                if (value < 1)
                    this.ddlEstatus.SelectedValue = value.ToString();
                else
                    this.ddlEstatus.SelectedValue = null;
            }
        }

        /// <summary>
        /// lista de los resultados.
        /// </summary>
        public List<ConfiguracionDescuentoBO> Resultado {
            get { return Session["listConfiguracionDescuentoBO"] != null ? Session["listConfiguracionDescuentoBO"] as List<ConfiguracionDescuentoBO> : null; }
            set { Session["listConfiguracionDescuentoBO"] = value; }
        }

        /// <summary>
        /// Número de índice de la página.
        /// </summary>
        public int IndicePaginaResultado {
            get { return this.grvConfiguracionDescuentos.PageIndex; }
            set { this.grvConfiguracionDescuentos.PageIndex = value; }
        }

        /// <summary>
        /// Lista de acciones permitidas.
        /// </summary>
        public List<CatalogoBaseBO> ListaAcciones { get; set; }

        /// <summary>
        /// Sucursales a las que tiene acceso el usuario logueado.
        /// </summary>
        public List<SucursalBO> SucursalesAutorizadas {
            get {
                if (Session["SucursalesAutRO"] != null)
                    return Session["SucursalesAutRO"] as List<SucursalBO>;
                else {
                    var lstRet = new List<SucursalBO>();
                    return lstRet.ConvertAll(x => (SucursalBO)x);
                }
            }

            set { Session["SucursalesAutRO"] = value; }
        }

        /// <summary>
        /// Objeto del usuario
        /// </summary>
        public UsuarioBO Usuario {
            get {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario;
                return null;
            }
        }

        /// <summary>
        /// Objeto de la unidad operativa
        /// </summary>
        public UnidadOperativaBO UnidadOperativa {
            get {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    return new UnidadOperativaBO { Id = masterMsj.Adscripcion.UnidadOperativa.Id };
                return null;
            }
        }
        #endregion

        #region Metodos

        /// <summary>
        /// inicializa los campos
        /// </summary>
        public void PrepararBusqueda() {
            this.txtCliente.Text = "";
            this.txtSucursal.Text = "";
            this.txtFechaInicial.Text = "";
            this.txtFechaFinal.Text = "";
            this.ddlEstatus.SelectedIndex = 0;

        }

        /// <summary>
        /// Actualiza los datos del grid.
        /// </summary>
        public void ActualizarResultado() {
            this.grvConfiguracionDescuentos.DataSource = this.Resultado;
            this.grvConfiguracionDescuentos.DataBind();
        }

        public void EstablecerPaqueteNavegacion(string nombre, object valor) {
            Session[nombre] = valor;
        }

        /// <summary>
        /// Redirecciona a otra página si el usuario no tiene permisos.
        /// </summary>
        public void RedirigirADetalles() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/DetalleConfiguracionDescuentoPSLUI.aspx"));
        }

        /// <summary>
        /// Elimina las sesiones.
        /// </summary>
        public void LimpiarSesion() {
            if (Session["ListaDescuentos"] != null)
                Session.Remove("ListaDescuentos");

            if (Session["listDetalleConfiguracionDescuentoBO"] != null)
                Session.Remove("listDetalleConfiguracionDescuentoBO");

            if (Session["UltimoObjetoConfiguracionDescuento"] != null)
                Session.Remove("UltimoObjetoConfiguracionDescuento");

            if (Session["FilasSeleccionadas"] != null)
                Session.Remove("FilasSeleccionadas");

            if (Session["listDescuentos"] != null)
                Session.Remove("listDescuentos");
            if (Session["listConfiguracionDescuentoBO"] != null)
                Session.Remove("listConfiguracionDescuentoBO");
            if (Session["SucursalesAutRO"] != null)
                Session.Remove("SucursalesAutRO");
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los detalles del mensaje a desplegar</param>
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

        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

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

        private void RegistrarScript(string key, string script) {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }
        #endregion

        #region Eventos

        /// <summary>
        /// Evento para realizar la consulta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e) {
            try {
                this.presentador.Consultar();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al consultar descuentos", ETipoMensajeIU.ERROR, this.nombreClase + ".btnBuscar_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// Evento del grid para administrar las filas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvConfiguracionDescuentoso_RowCommand(object sender, GridViewCommandEventArgs e) {
            int index;

            if (e.CommandName.ToString().ToUpper() == "PAGE") return;

            try {
                index = Convert.ToInt32(e.CommandArgument.ToString()) + (((GridView)sender).PageIndex * ((GridView)sender).PageSize);
                switch (e.CommandName.Trim()) {
                    case "Detalles":
                        this.presentador.VerDetalles(index);
                        break;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al ejecutar la acción sobre el acta de nacimiento", ETipoMensajeIU.ERROR, this.nombreClase + ".grvActasNacimiento_RowCommand:" + ex.Message);
            }
        }

        /// <summary>
        /// Evento para poblar el grid de datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvConfiguracionDescuentos_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row.RowType == DataControlRowType.DataRow) {
                ConfiguracionDescuentoBO Descuento = (ConfiguracionDescuentoBO)e.Row.DataItem;


                Label labelSucursalNombre = e.Row.FindControl("lblSucursal") as Label;
                if (labelSucursalNombre != null) {
                    string sucursalNombre = string.Empty;
                    if (Descuento.Sucursal != null)
                        if (Descuento.Sucursal.Nombre != null) {
                            sucursalNombre = Descuento.Sucursal.Nombre;
                        }
                    labelSucursalNombre.Text = sucursalNombre;
                }

                Label labelClienteNombre = e.Row.FindControl("lblCliente") as Label;
                if (labelClienteNombre != null) {
                    string clienteNombre = string.Empty;
                    if (Descuento.Cliente != null)
                        if (Descuento.Cliente.Nombre != null) {
                            clienteNombre = Descuento.Cliente.Nombre;
                        }
                    labelClienteNombre.Text = clienteNombre;
                }


                CheckBox CheckBoxActivo = e.Row.FindControl("ChkActivo") as CheckBox;
                if (CheckBoxActivo != null) {
                    if (Descuento.Estado != false) {
                        CheckBoxActivo.Checked = true;
                    } else {
                        CheckBoxActivo.Checked = false;
                    }

                }
            }
        }

        /// <summary>
        /// Genera un índice nuevo al grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvConfiguracionDescuentos_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                this.presentador.CambiarPaginaResultado(e.NewPageIndex);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".grvActasNacimiento_PageIndexChanging:" + ex.Message);
            }
        }

        #region Eventos para el buscador



        /// <summary>
        /// TextChanged activa el llamado al Buscador para la búsqueda de Sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtSucursal_TextChanged(object sender, EventArgs e) {
            try {
                string nombreSucursal = Sucursal;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                Sucursal = nombreSucursal;
                if (Sucursal != null) {
                    this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                    Sucursal = null;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtSucursal_TextChanged" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarSucursal_Click(object sender, ImageClickEventArgs e) {
            try {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnBuscarSucursal_Click" + ex.Message);
            }
        }

        /// <summary>
        /// TextChanged activa el llamado al Buscador para la busqueda de Cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCliente_TextChanged(object sender, EventArgs e) {
            try {
                string nombreCliente = NombreCliente;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                NombreCliente = nombreCliente;
                if (NombreCliente != null) {
                    this.EjecutaBuscador("CuentaClienteIdealease", ECatalogoBuscador.CuentaClienteIdealease);
                    NombreCliente = null;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtCliente_TextChanged" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarClientes_Click(object sender, ImageClickEventArgs e) {
            if (txtCliente.Text.Length < 1) {
                this.MostrarMensaje("Es necesario un nombre de cliente.", ETipoMensajeIU.ADVERTENCIA);
                return;
            }

            try {
                this.EjecutaBuscador("CuentaClienteIdealease&hidden=0", ECatalogoBuscador.CuentaClienteIdealease);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnBuscarClientes_Click" + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e) {
            try {
                switch (ViewState_Catalogo) {
                    case ECatalogoBuscador.CuentaClienteIdealease:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;

                    case ECatalogoBuscador.Sucursal:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click" + ex.Message);
            }
        }
        #endregion

        #endregion

    }
}