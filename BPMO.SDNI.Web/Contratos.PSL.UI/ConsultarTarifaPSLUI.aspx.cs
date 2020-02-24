
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ConsultarTarifaPSLUI : System.Web.UI.Page, IConsultarTarifaPSLVIS {
        #region Atributos
        private string nombreClase = "ConsultarTarifaPSLUI";
        private ConsultarTarifaPSLPRE presentador;
        public enum ECatalogoBuscador {
            Modelo,
            Sucursal
        }
        #endregion

        #region Propiedades
        public int? UsuarioID {
            get {
                int? id = null;
                Site master = (Site)Page.Master;
                if (master != null && master.Usuario != null && master.Usuario.Id != null)
                    id = master.Usuario.Id;
                return id;
            }
        }
        public int? UnidadOperativaID {
            get {
                int? id = null;
                Site master = (Site)Page.Master;
                if (master != null && master.Adscripcion != null && master.Adscripcion.UnidadOperativa != null &&
                    master.Adscripcion.UnidadOperativa.Id != null)
                    id = master.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }
        public int? SucursalID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnSucursalID.Value.Trim()))
                    id = int.Parse(hdnSucursalID.Value.Trim());
                return id;
            }
            set { hdnSucursalID.Value = value != null ? value.ToString() : String.Empty; }
        }
        public string NombreSucursal {
            get {
                return (String.IsNullOrEmpty(txtSucursal.Text.Trim().ToUpper()))
                           ? null
                           : txtSucursal.Text.ToUpper();
            }
            set { txtSucursal.Text = value ?? String.Empty; }
        }
        public int? ModeloID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnModeloID.Value.Trim()))
                    id = int.Parse(hdnModeloID.Value.Trim());
                return id;
            }
            set { hdnModeloID.Value = value != null ? value.ToString() : String.Empty; }
        }
        public string NombreModelo {
            get {
                return (String.IsNullOrEmpty(txtNombreModelo.Text.Trim().ToUpper()))
                           ? null
                           : txtNombreModelo.Text.ToUpper();
            }
            set { txtNombreModelo.Text = value ?? String.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el código de la moneda pra la tarifa
        /// </summary>
        public string CodigoMoneda {
            get {
                string codigo = null;
                if (ddlMoneda.SelectedValue != "-1")
                    codigo = ddlMoneda.SelectedValue;
                return codigo;
            }
            set//SC0024
            {
                this.ddlMoneda.SelectedValue = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                   ? value.Trim().ToUpper()
                                                   : "-1";
            }
        }
        /// <summary>
        /// Obtiene o establece el tipo de la tarifa
        /// </summary>
        public int? TipoTarifa {
            get {
                int? id = null;
                if (ddlTipoTarifa.SelectedValue != "-1")
                    id = int.Parse(ddlTipoTarifa.SelectedValue.Trim());
                return id;
            }
            set//SC0024
            {
                this.ddlTipoTarifa.SelectedValue = value.HasValue
                                                   ? value.Value.ToString().Trim().ToUpper()
                                                   : "-1";
            }
        }

        /// <summary>
        /// Obtiene o establece el turno de la tarifa
        /// </summary>
        public Enum TarifaTurno {
            get {
                Enum tarifaTurno = null;
                switch (this.UnidadOperativaID) {
                    case (int)ETipoEmpresa.Generacion:
                        tarifaTurno = ETarifaTurnoGeneracion;
                        break;
                    case (int)ETipoEmpresa.Construccion:
                        tarifaTurno = ETarifaTurnoConstruccion;
                        break;
                    case (int)ETipoEmpresa.Equinova:
                        tarifaTurno = ETarifaTurnoEquinova;
                        break;
                }
                return tarifaTurno;
            }
            set {
                switch (this.UnidadOperativaID) {
                    case (int)ETipoEmpresa.Generacion:
                        this.ddlTarifaTurno.SelectedIndex = (int)(ETarifaTurnoGeneracion)value;
                        break;
                    case (int)ETipoEmpresa.Construccion:
                        this.ddlTarifaTurno.SelectedIndex = (int)(ETarifaTurnoConstruccion)value;
                        break;
                    case (int)ETipoEmpresa.Equinova:
                        this.ddlTarifaTurno.SelectedIndex = (int)(ETarifaTurnoEquinova)value;
                        break;
                }
            }
        }

        /// <summary>
        /// Enumerador Turno de Tarifa para Construcción
        /// </summary>
        public ETarifaTurnoConstruccion? ETarifaTurnoConstruccion {
            get {
                ETarifaTurnoConstruccion? eTarifaTurno = null;
                eTarifaTurno = (ETarifaTurnoConstruccion)Enum.Parse(typeof(ETarifaTurnoConstruccion), ddlTarifaTurno.SelectedIndex.ToString());
                return eTarifaTurno;
            }
            set {
                ddlTarifaTurno.SelectedIndex = (int)(ETarifaTurnoConstruccion)value;
            }
        }

        /// <summary>
        /// Enumerador Turno de Tarifa para Generación
        /// </summary>
        public ETarifaTurnoGeneracion? ETarifaTurnoGeneracion {
            get {
                ETarifaTurnoGeneracion? eTarifaTurno = null;
                eTarifaTurno = (ETarifaTurnoGeneracion)Enum.Parse(typeof(ETarifaTurnoGeneracion), ddlTarifaTurno.SelectedIndex.ToString());
                return eTarifaTurno;
            }
            set {
                ddlTarifaTurno.SelectedIndex = (int)(ETarifaTurnoGeneracion)value;
            }
        }

        /// <summary>
        /// Enumerador Turno de Tarifa para Equinova
        /// </summary>
        public ETarifaTurnoEquinova? ETarifaTurnoEquinova {
            get {
                ETarifaTurnoEquinova? eTarifaTurno = null;
                eTarifaTurno = (ETarifaTurnoEquinova)Enum.Parse(typeof(ETarifaTurnoEquinova), ddlTarifaTurno.SelectedIndex.ToString());
                return eTarifaTurno;
            }
            set {
                ddlTarifaTurno.SelectedIndex = (int)(ETarifaTurnoEquinova)value;
            }
        }

        /// <summary>
        /// Obtiene o establece el período de la tarifa
        /// </summary>
        public EPeriodosTarifa PeriodoTarifa {
            get {
                EPeriodosTarifa? ePeriodoTarifa = null;
                ePeriodoTarifa = (EPeriodosTarifa)Enum.Parse(typeof(EPeriodosTarifa), this.ddlPeriodoTarifa.SelectedIndex.ToString());
                return (EPeriodosTarifa)ePeriodoTarifa;
            }
            set//SC0024
            {
                this.ddlPeriodoTarifa.SelectedIndex = (int)(EPeriodosTarifa)value;
            }
        }

        public List<TarifaPSLBO> ListaTarifas {
            get {
                return Session["ListaTarifas"] != null
                           ? (List<TarifaPSLBO>)Session["ListaTarifas"]
                           : new List<TarifaPSLBO>();
            }
            set { Session["ListaTarifas"] = value; }
        }

        public bool? Estatus {
            get {
                bool? estatus = null;

                if (this.ddlEstatus.SelectedIndex > -1)
                    estatus = bool.Parse(this.ddlEstatus.SelectedValue);

                return estatus;
            }
            set {
                if (value != null)
                    this.ddlEstatus.SelectedValue = value.ToString();
                else
                    this.ddlEstatus.SelectedIndex = -1;
            }
        }

        #region SC0024
        public int IndicePaginaResultado {
            get { return this.grdTarifas.PageIndex; }
            set { this.grdTarifas.PageIndex = value; }
        }
        #endregion

        #region Propiedades Buscador

        public ECatalogoBuscador ViewState_Catalogo {
            get {
                return (ECatalogoBuscador)ViewState["BUSQUEDA"];
            }
            set {
                ViewState["BUSQUEDA"] = value;
            }
        }

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

        #endregion

        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new ConsultarTarifaPSLPRE(this);
                if (!this.IsPostBack) {
                    presentador.PrepararNuevo();
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            } else {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        public void EstablecerOpcionesMoneda(Dictionary<string, string> monedas) {
            ddlMoneda.Items.Clear();
            ddlMoneda.DataSource = monedas;
            ddlMoneda.DataTextField = "Value";
            ddlMoneda.DataValueField = "Key";
            ddlMoneda.DataBind();
            ddlMoneda.SelectedValue = "-1";
        }

        /// <summary>
        /// Método que se utiliza para llenar el combo de los turnos de las tarifas, según el diccionario
        /// </summary>
        public void EstablecerOpcionesTarifaTurno(Dictionary<string, string> turnos) {
            ddlTarifaTurno.Items.Clear();
            ddlTarifaTurno.DataSource = turnos;
            ddlTarifaTurno.DataTextField = "Value";
            ddlTarifaTurno.DataValueField = "Key";
            ddlTarifaTurno.DataBind();
            ddlTarifaTurno.SelectedValue = "-1";
        }

        /// <summary>
        /// Método que se utiliza para llenar el combo de los períodos de las tarifas, según el diccionario
        /// </summary>
        public void EstablecerOpcionesPeriodoTarifa(Dictionary<string, string> periodos) {
            ddlPeriodoTarifa.Items.Clear();
            ddlPeriodoTarifa.DataSource = periodos;
            ddlPeriodoTarifa.DataTextField = "Value";
            ddlPeriodoTarifa.DataValueField = "Key";
            ddlPeriodoTarifa.DataBind();
            ddlPeriodoTarifa.SelectedValue = "-1";
        }
        public void EstablecerOpcionesTipoTarifa(Dictionary<int, string> tipo) {
            ddlTipoTarifa.Items.Clear();
            ddlTipoTarifa.DataSource = tipo;
            ddlTipoTarifa.DataTextField = "Value";
            ddlTipoTarifa.DataValueField = "Key";
            ddlTipoTarifa.DataBind();
            ddlTipoTarifa.SelectedValue = "-1";
        }
        /// <summary>
        /// Establece la información del paquete de navegación para el detalle de tarifas
        /// </summary>
        /// <param name="tarifa">Tarifa que se desea visualizar en modo detalle</param>
        /// <param name="elementosFiltro">SC0024 filtros originales de la consulta</param>
        public void EstablecerDatosNavegacion(object tarifa, Dictionary<string, object> elementosFiltro) {
            Session["TarifaPSLBO"] = tarifa;
            Session["FiltrosTarifa"] = elementosFiltro;
        }
        public void RedirigirADetalle() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/DetalleTarifaPSLUI.aspx"));
        }
        public void ActualizarListaTarifas() {
            grdTarifas.DataSource = ListaTarifas;
            grdTarifas.DataBind();
        }
        public void PermitirRegistrar(bool activo) {
            hlRegistrar.Enabled = activo;
        }
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        public void LimpiarSesion() {
            if (Session["TarifaPSLBO"] != null)
                Session.Remove("TarifaPSLBO");
            if (Session["ListaTarifas"] != null)
                Session.Remove("ListaTarifas");
        }

        #region SC0024
        /// <summary>
        /// SC0024
        /// Obtiene el paquete de navegación donde se ha guardado la consulta original
        /// </summary>
        /// <returns></returns>
        public object ObtenerPaqueteNavegacion() {
            if (Session["FiltrosTarifa"] != null)
                return Session["FiltrosTarifa"] as object;
            return null;
        }
        /// <summary>
        /// SC0024
        /// Remueve de la session el paquete de navegación anterior
        /// </summary>
        public void LimpiarPaqueteNavegacion() {
            Session.Remove("FiltrosTarifa");
        }
        #endregion

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
        protected void btnBuscar_Click(object sender, EventArgs e) {
            presentador.ConsultarTarifas();
        }

        protected void ibtnBuscarModelo_Click(object sender, ImageClickEventArgs e) {
            string s;
            if ((s = this.presentador.ValidarCamposConsultaModelo()) != null) {
                this.MostrarMensaje("Se requiere los siguientes datos " + s.Substring(2), ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }
            try {
                this.EjecutaBuscador("Modelo", ECatalogoBuscador.Modelo);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al buscar el modelo", ETipoMensajeIU.ERROR, this.nombreClase + ".ibtnBuscarModelo_Click:" + ex.Message);
            }
        }

        protected void txtNombreModelo_TextChanged(object sender, EventArgs e) {
            try {
                string modelo = NombreModelo;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Modelo);

                NombreModelo = modelo;
                if (NombreModelo != null) {
                    this.EjecutaBuscador("Modelo", ECatalogoBuscador.Modelo);
                    NombreModelo = null;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Iconsistencias al buscar el Modelo", ETipoMensajeIU.ERROR,
                                    this.nombreClase + ".txtNombreModelo_TextChanged:" + ex.Message);
            }
        }

        protected void txtSucursal_TextChanged(object sender, EventArgs e) {
            try {
                string sucursal = NombreSucursal;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                NombreSucursal = sucursal;
                if (NombreSucursal != null) {
                    this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                    NombreSucursal = null;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Iconsistencias al buscar la sucursal", ETipoMensajeIU.ERROR,
                                    this.nombreClase + ".txtSucursal_TextChanged:" + ex.Message);
            }

        }

        protected void ibtnBuscarSucursal_Click(object sender, ImageClickEventArgs e) {
            try {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarSucursal_Click" + ex.Message);
            }
        }

        protected void grdTarifas_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                grdTarifas.DataSource = ListaTarifas;
                grdTarifas.PageIndex = e.NewPageIndex;
                grdTarifas.DataBind();
            } catch (Exception ex) {
                MostrarMensaje("inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".grdTarifas_PageIndexChanging:" + ex.Message);
            }
        }

        protected void grdTarifas_RowCommand(object sender, GridViewCommandEventArgs e) {
            try {
                switch (e.CommandName.Trim().ToUpper()) {
                    case "DETALLE": {

                            int? ContratoID = (e.CommandArgument != null) ? (int?)Convert.ToInt32(e.CommandArgument) : null;
                            presentador.IrADetalle(ContratoID);
                            break;
                        }
                    case "Page":
                        break;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al intentar ver el detalle de la tarifa", ETipoMensajeIU.ERROR, nombreClase + ".grdTarifas_RowCommand:" + ex.Message);
            }
        }

        protected void grdTarifas_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {
                if (e.Row.RowType == DataControlRowType.DataRow) {
                    TarifaPSLBO tarifa = (TarifaPSLBO)e.Row.DataItem;
                    Label lblSucursal = e.Row.FindControl("lblSucursal") as Label;
                    if (lblSucursal != null) {
                        string sucursal = string.Empty;
                        if (tarifa.Sucursal != null)
                            sucursal = tarifa.Sucursal.Nombre;
                        lblSucursal.Text = sucursal;
                    }
                    Label lblModelo = e.Row.FindControl("lblModelo") as Label;
                    if (lblModelo != null) {
                        string modelo = string.Empty;
                        if (tarifa.Modelo != null)
                            modelo = tarifa.Modelo.Nombre;
                        lblModelo.Text = modelo;
                    }
                    Label lblMoneda = e.Row.FindControl("lblMoneda") as Label;
                    if (lblMoneda != null) {
                        string moneda = string.Empty;
                        if (tarifa.Divisa != null && tarifa.Divisa.MonedaDestino != null)
                            moneda = tarifa.Divisa.MonedaDestino.Nombre;
                        lblMoneda.Text = moneda;
                    }
                    Label lblTipoTarifa = e.Row.FindControl("lblTipoTarifa") as Label;
                    if (lblTipoTarifa != null) {
                        string tipo = string.Empty;
                        if (tarifa.TipoTarifaID != null)
                            tipo = tarifa.TipoTarifaID.ToString();
                        lblTipoTarifa.Text = tipo;
                    }
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al monento de desplegar la lista de tarifas", ETipoMensajeIU.ERROR, nombreClase + ".grdTarifas_RowDataBound:" + ex.Message);
            }
        }
        protected void btnResult_Click(object sender, EventArgs e) {
            try {
                switch (ViewState_Catalogo) {
                    case ECatalogoBuscador.Modelo:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                    case ECatalogoBuscador.Sucursal:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click: " + ex.Message);
            }
        }
        #endregion
    }
}