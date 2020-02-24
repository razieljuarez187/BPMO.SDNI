using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.BOF;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Tramites.BO;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ConsultarListadoVerificacionPSLUI : System.Web.UI.Page, IConsultarListadoVerificacionPSLVIS {
        #region Atributos
        /// <summary>
        /// Presentador de la página
        /// </summary>
        private ConsultarListadoVerificacionPSLPRE presentador = null;
        /// <summary>
        /// Nombre de la clase para usar en los mensajes
        /// </summary>
        private const string nombreClase = "ConsultarListadoVerificacionPSLUI";
        /// <summary>
        /// Enumeración para el control del buscador de bepensa
        /// </summary>
        public enum ECatalogoBuscador {
            Unidad,
            Modelo,
            CuentaClienteIdealease
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene el usuario autenticado en el sistema
        /// </summary>
        public int? UsuarioID {
            get {
                Site masterMsj = (Site)Page.Master;

                return masterMsj != null && masterMsj.Usuario != null ? masterMsj.Usuario.Id : null;
            }
        }
        /// <summary>
        /// Obtiene la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary>
        public int? UnidadOperativaID {
            get {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                           ? masterMsj.Adscripcion.UnidadOperativa.Id
                           : null;
            }
        }
        /// <summary>
        /// Obtiene la Sucursal Seleccionada
        /// </summary>
        public SucursalBO SucursalSeleccionada {
            get {
                SucursalBO sucursalSeleccionada = null;
                if (ddlSucursales.SelectedValue != "0") {
                    sucursalSeleccionada = new SucursalBO {
                        Id = int.Parse(ddlSucursales.SelectedValue),
                        Nombre = ddlSucursales.SelectedItem.Text,
                        UnidadOperativa = new UnidadOperativaBO() { Id = this.UnidadOperativaID }
                    };
                }

                return sucursalSeleccionada;
            }
        }
        /// <summary>
        /// Obtiene o establce el identificador de la unidad
        /// </summary>
        public int? UnidadID {
            get {
                if (!string.IsNullOrWhiteSpace(this.hdnUnidadID.Value)) {
                    int val = 0;
                    return Int32.TryParse(this.hdnUnidadID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnUnidadID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el número de serie de la unidad que se desea consultar
        /// </summary>
        public string NumeroSerie {
            get { return !string.IsNullOrWhiteSpace(this.txtNumeroSerie.Text) ? this.txtNumeroSerie.Text.Trim().ToUpper() : null; }
            set { this.txtNumeroSerie.Text = !string.IsNullOrWhiteSpace(value) ? value.Trim().ToUpper() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el número económico de la unidad que se desea consultar
        /// </summary>
        public string NumeroEconomico {
            get { return !string.IsNullOrWhiteSpace(this.txtNumeroEconomico.Text.Trim()) ? this.txtNumeroEconomico.Text.Trim().ToUpper() : null; }
            set { this.txtNumeroEconomico.Text = !string.IsNullOrWhiteSpace(value) ? value.Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador del modelo de la unidad que se desea consultar
        /// </summary>
        public int? ModeloID {
            get {
                int val;
                return !string.IsNullOrEmpty(this.hdnModeloID.Value) &&
                       !string.IsNullOrWhiteSpace(this.hdnModeloID.Value)
                           ? (Int32.TryParse(this.hdnModeloID.Value, out val) ? (int?)val : null)
                           : null;
            }
            set { this.hdnModeloID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el nombre del modelo de la unidad seleccionada para el filtro de consulta
        /// </summary>
        public string ModeloNombre {
            get { return !string.IsNullOrWhiteSpace(this.txtModelo.Text) ? this.txtModelo.Text.Trim().ToUpper() : null; }
            set { this.txtModelo.Text = !string.IsNullOrWhiteSpace(value) ? value.Trim().ToUpper() : string.Empty; }
        }

        /// <summary>
        /// Obtiene o establece el identificador del cliente
        /// </summary>
        public int? ClienteID {
            get {
                if (!string.IsNullOrEmpty(this.hdnClienteID.Value) && !string.IsNullOrWhiteSpace(this.hdnClienteID.Value)) {
                    int val = 0;
                    return Int32.TryParse(this.hdnClienteID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnClienteID.Value = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty; }
        }

        /// <summary>
        /// Obtiene o establece el número de cuenta del cliente
        /// </summary>
        public int? CuentaClienteID {
            get {
                if (!string.IsNullOrEmpty(this.hdnCuentaClienteID.Value) && !string.IsNullOrWhiteSpace(this.hdnCuentaClienteID.Value)) {
                    int val = 0;
                    return Int32.TryParse(this.hdnCuentaClienteID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set {
                this.hdnCuentaClienteID.Value = value.HasValue
                                                       ? value.Value.ToString(CultureInfo.InvariantCulture).Trim().ToUpper()
                                                       : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el nombre del cliente
        /// </summary>
        public string ClienteNombre { 
            get { return string.IsNullOrWhiteSpace(this.txtNombreCliente.Text) ? null : this.txtNombreCliente.Text.Trim().ToUpper(); }
            set { this.txtNombreCliente.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value) ? value.Trim().ToUpper() 
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el número de contrato
        /// </summary>
        public string NumeroContrato {
            get { return this.txtNumeroContrato.Text.Trim().ToUpper(); }
            set {
                this.txtNumeroContrato.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el tipo de check List
        /// </summary>
        public int? TipoListado {
            get {
                if (this.ddlTipolistado.SelectedValue.CompareTo("-1") != 0) {
                    int val = 0;
                    if (Int32.TryParse(this.ddlTipolistado.SelectedValue, out val))
                        return val;
                }
                return null;
            }
            set {
                this.ddlTipolistado.SelectedValue = value.HasValue
                                                   ? value.ToString()
                                                   : "-1";
            }
        }

        /// <summary>
        /// Obtiene el identificador del Modulo del Sistema
        /// </summary>
        public int? ModuloID {
            get {
                return ((Site)Page.Master).ModuloID;
            }
        }

        /// <summary>
        /// Obtiene o establece el indice para el control del grid de resultados de la consulta
        /// </summary>
        public int IndicePaginaResultado {
            get { return this.grdListadosVerificacion.PageIndex; }
            set { this.grdListadosVerificacion.PageIndex = value; }
        }
        /// <summary>
        /// Obtiene o establece los resultados de la consulta
        /// </summary>
        public List<object> Resultado {
            get {
                List<ListadoVerificacionBOF> lista = null;
                if (Session["ListadosEncontradosPSL"] != null)
                    lista = new List<ListadoVerificacionBOF>();

                return (List<object>)(Session["ListadosEncontradosPSL"] ?? lista);
            }
            set { Session["ListadosEncontradosPSL"] = value; }
        }
        /// <summary>
        /// Obtiene o estable la lista de sucursales a las que el usuario autenticado tiene permiso de acceder
        /// </summary>
        public List<SucursalBO> SucursalesAutorizadas {
            get {
                if (Session["SucursalesAutorizadas"] != null)
                    return Session["SucursalesAutorizadas"] as List<SucursalBO>;

                return null;
            }
            set {
                if (value != null && value.Any()) {
                    Session["SucursalesAutorizadas"] = value;
                } else {
                    Session.Remove("SucursalesAutorizadas");
                    this.ddlSucursales.Enabled = false;
                }
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
            get { return (ECatalogoBuscador)ViewState["BUSQUEDA"]; }
            set { ViewState["BUSQUEDA"] = value; }
        }

        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase ConsultarListadoVerificacionPSLUI
        /// </summary>
        public ConsultarListadoVerificacionPSLUI() {
            try {
                presentador = new ConsultarListadoVerificacionPSLPRE(this);
                if (!IsPostBack) {

                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos

        ///<summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            Site masterMsj = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo, detalle);
            } else {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// Actualiza la información que se  visualiza en el grid de detalles
        /// </summary>
        public void ActualizarResultado() {
            this.grdListadosVerificacion.DataSource = this.Resultado.ConvertAll(x => (ListadoVerificacionBOF)x);
            this.grdListadosVerificacion.DataBind();
        }

        /// <summary>
        /// Método que redirecciona a una página diferente
        /// </summary>
        /// <param name="url"></param>
        /// <param name="target"></param>
        /// <param name="windowFeatures"></param>
        private static void Redirect(string url, string target, string windowFeatures) {
            HttpContext context = HttpContext.Current;

            if ((String.IsNullOrEmpty(target) ||
                target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
                String.IsNullOrEmpty(windowFeatures)) {

                context.Response.Redirect(url);
            } else {
                Page page = (Page)context.Handler;
                if (page == null) {
                    throw new InvalidOperationException(
                        "La página que desea abrir no se encuentra dentro del contexto. No se puede abrir uan neuva ventana.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures)) {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                } else {
                    script = @"window.open(""{0}"", ""{1}"");";
                }

                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page,
                    typeof(Page),
                    "Redirect",
                    script,
                    true);
            }
        }

        /// <summary>
        /// Redirige al detalle de flota
        /// </summary>
        public void RedirigirAFlota() {
            var url = Page.ResolveClientUrl("~/Flota.UI/DetalleFlotaUI.aspx");
            Redirect(url, "_blank", null);
        }
        /// <summary>
        /// Redirige al registro de entrega de la unidad
        /// </summary>
        public void RedirigirARegistrarEntrega() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/RegistrarEntregaPSLUI.aspx"));
        }
        /// <summary>
        /// Redirige al registro de recepción de la unidad
        /// </summary>
        public void RedirigirARegistrarRecepcion() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/RegistrarRecepcionPSLUI.aspx"));
        }

        /// <summary>
        /// Crear en la variable de session el contrato al cual se le desea registrar su check list
        /// </summary>
        /// <param name="clave">Clave con la que se guarda en session el contrato</param>
        /// <param name="contratoID">Contrato que se desea guardar</param>
        public void EstablecerPaqueteNavegacion(string clave, int? contratoID) {
            Session[clave] = contratoID;
        }

        /// <summary>
        /// Crean en la variable de session la unidad que se desea desplegar
        /// </summary>
        /// <param name="clave">Clave con la qu se guarda en session la unidad</param>
        /// <param name="unidadID">Unidad que se desea guardar</param>
        public void EstablecerPaqueteNavegacionFlota(string clave, int? unidadID) {
            if (unidadID.HasValue) {
                object obj = this.presentador.ObtenerElementoFlota(unidadID.Value);

                if (obj != null)
                    Session[clave] = obj;
            }
        }

        /// <summary>
        /// Redirige a la pantalla de página sin acceso
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        /// <summary>
        /// Carga las sucursales autorizadas en el combobox
        /// </summary>
        /// <param name="lstSucursales">Lista de sucursales autorizadas</param>
        public void CargarSucursales(List<SucursalBO> lstSucursales) {
            try {
                List<SucursalBO> lstSucCargar = new List<SucursalBO>();
                // Agregar el SucursalBO de fachada
                lstSucCargar.Insert(0, new SucursalBO { Id = 0, Nombre = "TODAS" });
                // Se agregan las sucursales enviadas
                if (lstSucursales != null) lstSucCargar.AddRange(lstSucursales);
                //Limpiar el DropDownList Actual
                ddlSucursales.Items.Clear();
                // Asignar Lista al DropDownList
                ddlSucursales.DataTextField = "Nombre";
                ddlSucursales.DataValueField = "Id";
                ddlSucursales.DataSource = lstSucCargar;
                ddlSucursales.DataBind();

                if (lstSucursales.Count == 1) this.EstablecerSucursalSeleccionada(lstSucursales[0].Id);
            } catch (Exception ex) {
                this.MostrarMensaje("Error al cargar las sucursales autorizadas.", ETipoMensajeIU.ERROR, "ConsultarContratoROUI.CargarSucursales: " + ex.Message);
            }
        }

        /// <summary>
        /// Establece la Sucursal Seleccionada
        /// </summary>
        /// <param name="Id"></param>
        public void EstablecerSucursalSeleccionada(int? Id) {
            if (Id != null)
                ddlSucursales.SelectedValue = Id.Value.ToString(CultureInfo.InvariantCulture);
            else
                ddlSucursales.SelectedIndex = 0;
        }

        /// <summary>
        /// Establece las opciones disponibles para la lsita
        /// </summary>
        /// <param name="opciones">Lista con las posibles opciones</param>
        public void EstablecerOpcionesTipoListado(Dictionary<int, string> opciones) {
            this.ddlTipolistado.Items.Clear();
            this.ddlTipolistado.DataSource = opciones;
            this.ddlTipolistado.DataTextField = "Value";
            this.ddlTipolistado.DataValueField = "Key";
            this.ddlTipolistado.DataBind();
            this.ddlTipolistado.SelectedValue = "-1";
        }

        /// <summary>
        /// Redirige a la pantalla de consulta de check list
        /// </summary>
        public void RedirigirAConsulta() {
            Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/ConsultarListadoVerificacionPSLUI.aspx"));
        }

        /// <summary>
        /// Guarda en la variable de Session un paquete para su posterior consulta
        /// </summary>
        /// <param name="key">Clave del paquete de navegación</param>
        /// <param name="value">Paquete que se desea guardar en la session</param>
        public void EstablecerPaqueteNavegacion(string key, object value) {
            Session["NombreReporte"] = key;
            Session["DatosReporte"] = value;
        }
        /// <summary>
        /// Redirige a la página de impresión del reporte
        /// </summary>
        /// <param name="error">Mensaje de error que se mostrará en la interfaz</param>
        public void RedirigirAImprimir(string error) {
            Response.Redirect("../Buscador.UI/VisorReporteUI.aspx?error=" + error, false);
        }

        /// <summary>
        /// Carga y despliega los check list que cumplen con los filtros de búsqueda en el grid de detalles
        /// </summary>
        /// <param name="elementos">Lista de Check List que cumplen con los filtros proporcionados para la consulta</param>
        public void CargarElementosFlotaEncontrados(List<object> elementos) {
            this.grdListadosVerificacion.DataSource = elementos;
            this.grdListadosVerificacion.DataBind();
            this.grdListadosVerificacion.Columns[4].Visible = false;
            this.grdListadosVerificacion.Columns[5].Visible = false;
            this.grdListadosVerificacion.Columns[8].Visible = false;
        }

        /// <summary>
        /// Limpia de Session los valores a usar
        /// </summary>
        public void LimpiarSesion() {
            Session.Remove("ListadosEncontradosPSL");
            Session.Remove("DetalleFlotaUI");
            Session.Remove("RegistrarEntregaUI");
        }

        /// <summary>
        /// Limpia el grid de check list
        /// </summary>
        public void LimpiarListadosEncontrados() {
            this.LimpiarSesion();

            this.CargarElementosFlotaEncontrados((new List<ListadoVerificacionBOF>()).ConvertAll(x => (object)x));
        }

        #region Métodos para el Buscador

        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo) {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            Session_BOSelecto = null;
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
        /// metodo para registrar script en el cliente
        /// </summary>
        /// <param name="key"> llave del script que se va a registrar</param>
        /// <param name="script">script que se va a registrar</param>
        private void RegistrarScript(string key, string script) {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }
        #endregion

        #endregion

        #region Eventos

        /// <summary>
        /// Evento Load de la página ConsultarListadoVerificaciónPSLUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new ConsultarListadoVerificacionPSLPRE(this);

                if (!IsPostBack) {
                    this.presentador.PrepararBusqueda();
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }

        /// <summary>
        /// Ejecuta la busqueda de la flota
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e) {
            try {
                this.presentador.ConsultarListados();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al consultar Check List", ETipoMensajeIU.ERROR, nombreClase + ".btnBuscar_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// Buscar una unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtNumeroSerie_TextChanged(object sender, EventArgs e) {
            try {
                string numeroSerie = NumeroSerie;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.Unidad);

                NumeroSerie = numeroSerie;
                if (!string.IsNullOrWhiteSpace(NumeroSerie)) {
                    EjecutaBuscador("UnidadIdealeaseSimple", ECatalogoBuscador.Unidad);
                    NumeroSerie = null;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNumeroSerie_TextChanged: " + ex.Message);
            }
        }

        /// <summary>
        /// TextChanged activa el llamado al Buscador para el Modelo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtModelo_TextChanged(object sender, EventArgs e) {
            try {
                string modeloNombre = this.ModeloNombre;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.Modelo);

                this.ModeloNombre = modeloNombre;
                if (!string.IsNullOrWhiteSpace(this.ModeloNombre)) {
                    EjecutaBuscador("Modelo", ECatalogoBuscador.Modelo);
                    this.ModeloNombre = null;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar un modelo", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtModelo_TextChanged: " + ex.Message);
            }
        }

        /// <summary>
        /// Busca un cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtNombreCliente_TextChanged(object sender, EventArgs e) {
            try {
                string nombreCuentaCliente = this.ClienteNombre;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                this.ClienteNombre = nombreCuentaCliente;
                if (!string.IsNullOrWhiteSpace(this.ClienteNombre)) {
                    EjecutaBuscador("CuentaClienteIdealeaseSimple", ECatalogoBuscador.CuentaClienteIdealease);
                    this.ClienteNombre = null;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNombreCliente_TextChanged:" + ex.Message);
            }
        }

        /// <summary>
        /// Buscar Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarVin_Click(object sender, ImageClickEventArgs e) {
            try {
                if (!string.IsNullOrWhiteSpace(this.txtNumeroSerie.Text))
                    this.EjecutaBuscador("UnidadIdealeaseSimple&hidden=0", ECatalogoBuscador.Unidad);
                else
                    MostrarMensaje("Ingrese un parámetro de búsqueda.", ETipoMensajeIU.INFORMACION);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarVin_Click" + ex.Message);
            }
        }

        /// <summary>
        /// Buscar Modelo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaModelo_Click(object sender, ImageClickEventArgs e) {
            try {
                if (!string.IsNullOrWhiteSpace(this.txtModelo.Text))
                    this.EjecutaBuscador("Modelo&hidden=0", ECatalogoBuscador.Modelo);
                else
                    MostrarMensaje("Ingrese un parámetro de búsqueda.", ETipoMensajeIU.INFORMACION);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar un Modelo", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscaModelo_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// Busca un cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscarCliente_Click(object sender, ImageClickEventArgs e) {
            try {
                if (!string.IsNullOrWhiteSpace(this.txtNombreCliente.Text))
                    EjecutaBuscador("CuentaClienteIdealeaseSimple&hidden=0", ECatalogoBuscador.CuentaClienteIdealease);
                else
                    MostrarMensaje("Ingrese un parámetro de búsqueda.", ETipoMensajeIU.INFORMACION);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarCliente_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// Ejecuta los comandos del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdListadosVerificacion_RowCommand(object sender, GridViewCommandEventArgs e) {
            int? lineaContratoPSLID = null;
            try {
                switch (e.CommandName.Trim().ToUpper()) {
                    case "DETALLES":
                        int? unidadID = (e.CommandArgument != null) ? (int?)Convert.ToInt32(e.CommandArgument) : null;
                        break;

                    case "IMPRIMIR":
                        lineaContratoPSLID = (e.CommandArgument != null) ? (int?)Convert.ToInt32(e.CommandArgument) : null;
                        ListadoVerificacionBOF listadoVerificacion = (ListadoVerificacionBOF)this.Resultado
                            .FirstOrDefault(p => { var lineaContratoPSLId = ((ListadoVerificacionBOF)p).LineaContratoPSLID;
                                return lineaContratoPSLId != null && lineaContratoPSLId.Value == lineaContratoPSLID;
                            });

                        this.presentador.ImprimirListadoVerificacion(listadoVerificacion, lineaContratoPSLID);
                        break;

                    case "REGISTRAR":
                        lineaContratoPSLID = (e.CommandArgument != null) ? (int?)Convert.ToInt32(e.CommandArgument) : null;
                        if (lineaContratoPSLID != null) {
                            var tipo =
                                (ListadoVerificacionBOF)this.Resultado.FirstOrDefault(
                                    p =>
                                    {
                                        var lineaContratoPSLId = ((ListadoVerificacionBOF)p).LineaContratoPSLID;
                                        return lineaContratoPSLId != null && lineaContratoPSLId.Value == lineaContratoPSLID;
                                    });

                            switch (tipo.TipoListado) {
                                case ETipoListadoVerificacion.ENTREGA:
                                    this.presentador.RedirigirARegistrarEntrega(tipo.ContratoID, lineaContratoPSLID);
                                    break;

                                case ETipoListadoVerificacion.RECEPCION:
                                    this.presentador.RedirigirARegistrarRecepcion(tipo.ContratoID, lineaContratoPSLID);
                                    break;
                            }
                        }
                        break;
                    case "PAGE":
                        break;

                    default: {
                            MostrarMensaje("Comando no encontrado", ETipoMensajeIU.ERROR, "El comando no está especificado en el sistema");
                            break;
                        }
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al intentar realizar una acción sobre un Check List", ETipoMensajeIU.ERROR, nombreClase + ".grdListadosVerificacion_RowCommand" + ex.Message);
            }
        }

        /// <summary>
        /// Completa la información que se presenta en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdListadosVerificacion_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {
                if (e.Row.RowType == DataControlRowType.DataRow) {
                    ListadoVerificacionBOF bof = (ListadoVerificacionBOF)e.Row.DataItem;
                    Label lblPlacaFederal = e.Row.FindControl("lblPlacaFederal") as Label;
                    if (lblPlacaFederal != null) {
                        var placaFederal = bof.Tramites.FirstOrDefault(p => p.Tipo != null && p.Tipo.Value == ETipoTramite.PLACA_FEDERAL);
                        if (!ReferenceEquals(placaFederal, null)) {
                            lblPlacaFederal.Text = !string.IsNullOrEmpty(placaFederal.Resultado) && !string.IsNullOrWhiteSpace(placaFederal.Resultado)
                                                   ? placaFederal.Resultado.Trim().ToUpper()
                                                   : string.Empty;
                        }
                    }

                    Label lblPlacaEstatal = e.Row.FindControl("lblPlacaEstatal") as Label;
                    if (lblPlacaEstatal != null) {
                        var placaEstatal =
                            bof.Tramites.FirstOrDefault(
                                p => p.Tipo != null && p.Tipo.Value == ETipoTramite.PLACA_ESTATAL);
                        if (!ReferenceEquals(placaEstatal, null)) {
                            lblPlacaEstatal.Text = !string.IsNullOrEmpty(placaEstatal.Resultado) && !string.IsNullOrWhiteSpace(placaEstatal.Resultado)
                                                       ? placaEstatal.Resultado.Trim().ToUpper()
                                                       : string.Empty;
                        }
                    }

                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".grdListadosVerificacion_RowDataBound: " + ex.Message);
            }
        }

        /// <summary>
        /// Cambiar página en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdListadosVerificacion_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                this.presentador.CambiarPaginaResultado(e.NewPageIndex);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".grdListadosVerificacion_PageIndexChanging:" + ex.Message);
            }
        }

        /// <summary>
        /// Ejecuta el buscador de bepensa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResult_Click(object sender, EventArgs e) {
            try {
                switch (ViewState_Catalogo) {
                    case ECatalogoBuscador.Unidad:
                    case ECatalogoBuscador.CuentaClienteIdealease:
                    case ECatalogoBuscador.Modelo:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click" + ex.Message);
            }
        }

        #endregion
    }
}