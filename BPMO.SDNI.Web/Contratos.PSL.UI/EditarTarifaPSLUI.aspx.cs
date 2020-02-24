// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class EditarTarifaPSLUI : System.Web.UI.Page, IEditarTarifaPSLVIS {
        #region Atributos
        private string NombreClase = "EditarTarifaRDUI";
        private EditarTarifaPSLPRE presentador;
        public enum ECatalogoBuscador {
            Sucursal
        }
        #endregion

        #region Propiedaes
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
        public int? UsuarioID {
            get {
                int? id = null;
                Site master = (Site)Page.Master;
                if (master != null && master.Usuario != null && master.Usuario.Id != null)
                    id = master.Usuario.Id;
                return id;
            }
        }
        public decimal? PrecioCombustible {
            set {
                TextBox txtPrecioCombustible = mTarifa.Controls[0].FindControl("txtValue") as TextBox;
                if (txtPrecioCombustible != null) {
                    txtPrecioCombustible.Text = value != null ? String.Format("{0:#,##0.00##}", value) : string.Empty; //RI0062
                }
            }
        }
        public DateTime? FUA {
            get { return DateTime.Now; }
        }
        public int? TarifaPSLID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnTarifaID.Value.Trim()))
                    id = int.Parse(hdnTarifaID.Value.Trim());
                return id;
            }
            set { hdnTarifaID.Value = value != null ? value.ToString() : String.Empty; }
        }
        public string NombreSucursal {
            set { txtSucursal.Text = value ?? String.Empty; }
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
        public string NombreModelo {
            set { txtModelo.Text = value ?? String.Empty; }
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
        public string NombreMoneda {
            set { txtMoneda.Text = value != null ? value.ToUpper() : String.Empty; }
        }
        public string CodigoMoneda {
            get {
                return (String.IsNullOrEmpty(hdnCodigoMoneda.Value.Trim().ToUpper()))
                           ? null
                           : hdnCodigoMoneda.Value.ToUpper();
            }
            set { hdnCodigoMoneda.Value = value ?? String.Empty; }
        }
        public string NombreTipoTarifa {
            set { txtTipo.Text = value ?? String.Empty; }
        }
        public int? TipoTarifa {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnTipo.Value.Trim()))
                    id = int.Parse(hdnTipo.Value.Trim());
                return id;
            }
            set { hdnTipo.Value = value.ToString() ?? String.Empty; }
        }
        public string NombreTarifaTurno {
            get {
                if (string.IsNullOrEmpty(this.txtTarifaTurno.Text.Trim())) return null;
                return this.txtTarifaTurno.Text.Trim();
            }
            set { txtTarifaTurno.Text = value ?? String.Empty; }
        }

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
                Type type = null;
                switch (this.UnidadOperativaID) {
                    case (int)ETipoEmpresa.Generacion:
                        type = typeof(ETarifaTurnoGeneracion);
                        break;
                    case (int)ETipoEmpresa.Construccion:
                        type = typeof(ETarifaTurnoConstruccion);
                        break;
                    case (int)ETipoEmpresa.Equinova:
                        type = typeof(ETarifaTurnoEquinova);
                        break;
                }
                var memInfo = type.GetMember(type.GetEnumName(value));
                var display = memInfo[0]
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() as DescriptionAttribute;
                txtTarifaTurno.Text = display != null ? display.Description : string.Empty;
            }
        }

        public string NombrePeriodoTarifa {
            get {
                if (string.IsNullOrEmpty(this.txtPeriodoTarifa.Text.Trim())) return null;
                return this.txtPeriodoTarifa.Text.Trim();
            }
            set { txtPeriodoTarifa.Text = value ?? String.Empty; }
        }

        public EPeriodosTarifa PeriodoTarifa {
            get {
                int? id = null;

                if (!String.IsNullOrEmpty(hdnPeriodoTarifa.Value.Trim()))
                    id = int.Parse(hdnPeriodoTarifa.Value.Trim());
                return (EPeriodosTarifa)Enum.Parse(typeof(EPeriodosTarifa), id.ToString());
            }
            set { hdnPeriodoTarifa.Value = value.ToString() ?? String.Empty; }
        }

        /// <summary>
        /// Enumerador Turno de Tarifa para Construcción
        /// </summary>
        public ETarifaTurnoConstruccion? ETarifaTurnoConstruccion {
            get {
                int? id = null;
                ETarifaTurnoConstruccion? eTarifaTurno = null;
                if (!String.IsNullOrEmpty(hdnTarifaTurno.Value.Trim()))
                    id = int.Parse(hdnPeriodoTarifa.Value.Trim());
                eTarifaTurno = (ETarifaTurnoConstruccion)Enum.Parse(typeof(ETarifaTurnoConstruccion), id.ToString());
                return eTarifaTurno;
            }
            set {
                hdnTarifaTurno.Value = value.ToString() ?? String.Empty;
            }
        }

        /// <summary>
        /// Enumerador Turno de Tarifa para Generación
        /// </summary>
        public ETarifaTurnoGeneracion? ETarifaTurnoGeneracion {
            get {
                int? id = null;
                ETarifaTurnoGeneracion? eTarifaTurno = null;
                if (!String.IsNullOrEmpty(hdnTarifaTurno.Value.Trim()))
                    id = int.Parse(hdnPeriodoTarifa.Value.Trim());
                eTarifaTurno = (ETarifaTurnoGeneracion)Enum.Parse(typeof(ETarifaTurnoGeneracion), id.ToString());
                return eTarifaTurno;
            }
            set {
                hdnTarifaTurno.Value = value.ToString() ?? String.Empty;
            }
        }

        /// <summary>
        /// Enumerador Turno de Tarifa para Equinova
        /// </summary>
        public ETarifaTurnoEquinova? ETarifaTurnoEquinova {
            get {
                int? id = null;
                ETarifaTurnoEquinova? eTarifaTurno = null;
                if (!String.IsNullOrEmpty(hdnTarifaTurno.Value.Trim()))
                    id = int.Parse(hdnPeriodoTarifa.Value.Trim());
                eTarifaTurno = (ETarifaTurnoEquinova)Enum.Parse(typeof(ETarifaTurnoEquinova), id.ToString());
                return eTarifaTurno;
            }
            set {
                hdnTarifaTurno.Value = value.ToString() ?? String.Empty;
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

        /// <summary>
        /// Identificador del Modulo desde el cual se esta accesando
        /// </summary>
        public int? ModuloID {
            get {
                return ((Site)Page.Master).ModuloID;
            }
        }

        public bool? AplicarOtrasSucursales {
            get {
                return this.chkAplicarSucursales.Checked;
            }
            set { this.chkAplicarSucursales.Checked = (bool)(value ?? false); }
        }
        public int? SucursalNoAplicaID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnSucursalNoAplicaID.Value.Trim()))
                    id = int.Parse(this.hdnSucursalNoAplicaID.Value.Trim());
                return id;
            }
            set { this.hdnSucursalNoAplicaID.Value = value != null ? value.ToString() : String.Empty; }
        }
        public string NombreSucursalNoAplica {
            get { return (String.IsNullOrEmpty(this.txtSucursalNoAplica.Text.Trim())) ? null : this.txtSucursalNoAplica.Text.ToUpper(); }
            set { this.txtSucursalNoAplica.Text = value ?? String.Empty; }
        }
        public List<TarifaPSLBO> TarifasAnteriores {
            get {
                return Session["UltimasTarifas"] != null
                           ? (List<TarifaPSLBO>)Session["UltimasTarifas"]
                           : new List<TarifaPSLBO>();
            }
            set { Session["UltimasTarifas"] = value; }
        }
        public List<SucursalBO> SessionListaSucursalSeleccionada {
            get {
                return Session["ListaSucursales"] != null
                           ? (List<SucursalBO>)Session["ListaSucursales"]
                           : new List<SucursalBO>();
            }
            set {
                List<SucursalBO> lst = value ?? new List<SucursalBO>();
                Session["ListaSucursales"] = lst;
                grdSucursales.DataSource = lst;
                grdSucursales.DataBind();
            }
        }

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
                presentador = new EditarTarifaPSLPRE(this, ucTarifaPSL);
                if (!IsPostBack) {
                    presentador.ValidarAcceso();
                    presentador.RealizarPrimeraCarga();
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al crear la página", ETipoMensajeIU.ERROR,
                                    NombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void LimpiarSesion() {
            if (Session["ListaSucursales"] != null)
                Session.Remove("ListaSucursales");
            if (Session["UltimasTarifas"] != null)
                Session.Remove("UltimasTarifas");
            if (Session["TarifaPSLBO"] != null)
                Session.Remove("TarifaPSLBO");
        }
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            } else {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        public void MostrarAplicarSucursal(bool activo) {
            this.pnlAplicarSucursales.Visible = activo;
        }
        public void RedirigirADetalle() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/DetalleTarifaPSLUI.aspx"));
        }
        public void RedirigirAConsulta() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/ConsultarTarifaPSLUI.aspx"));
        }
        public void PermitirRegistrar(bool activo) {
            hlRegistrar.Enabled = activo;
        }
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        public void PermitirAgregarSucursales(bool activo) {
            pnlSeccionSucursal.Visible = activo;
            chkAplicarSucursales.Checked = !activo;
        }
        public void EstablecerPaqueteNavegacion(object tarifa) {
            Session["TarifaPSLBO"] = tarifa;
        }
        public object ObtenerDatosNavegacion() {
            return (object)Session["TarifaPSLBO"];
        }

        public void MostrarLeyendaSucursales(bool mostrar, string leyenda) {
            this.lblLeyendaSucursales.Text = leyenda;
            this.btnLeyendaSucursales.Visible = mostrar;
        }

        #region Métodos para el buscador
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
        protected void btnCancelar_Click(object sender, EventArgs e) {
            try {
                presentador.Cancelar();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cancelar la edición de la tarifa", ETipoMensajeIU.ERROR, NombreClase + ".btnCancelar_Click:" + ex.Message);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e) {
            try {
                presentador.Guardar();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al guardar la tarifa editada", ETipoMensajeIU.ERROR, NombreClase + ".btnGuardar_Click:" + ex.Message);
            }
        }

        protected void chkAplicarSucursales_CheckedChanged(object sender, EventArgs e) {
            try {
                presentador.AplicarOtrasSucursales();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al realizar la acción", ETipoMensajeIU.ERROR, NombreClase + ".chkAplicarSucursales_CheckedChanged:" + ex.Message);
            }
        }

        protected void txtSucursalNoAplica_TextChanged(object sender, EventArgs e) {
            try {
                string sucursal = NombreSucursalNoAplica;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                NombreSucursalNoAplica = sucursal;
                if (NombreSucursalNoAplica != null) {
                    this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                    NombreSucursalNoAplica = null;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Iconsistencias al buscar la sucursal", ETipoMensajeIU.ERROR,
                                    this.NombreClase + ".txtSucursalNoAplica_TextChanged:" + ex.Message);
            }
        }

        protected void ibtnBuscarSucursalNoAplica_Click(object sender, ImageClickEventArgs e) {
            try {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".ibtnBuscarSucursalNoAplica_Click:" + ex.Message);
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e) {
            try {
                this.presentador.AgregarSucursalNoAplicar();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al agregar la sucursal", ETipoMensajeIU.ERROR, this.NombreClase + ".btnAgregar_Click:" + ex.Message);
            }
        }

        protected void grdSucursales_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                grdSucursales.DataSource = SessionListaSucursalSeleccionada;
                grdSucursales.PageIndex = e.NewPageIndex;
                grdSucursales.DataBind();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al momento de realizar la paginación de las sucursales", ETipoMensajeIU.ERROR, this.NombreClase + ".grdSucursales_PageIndexChanging:" + ex.Message);
            }
        }

        protected void grdSucursales_RowCommand(object sender, GridViewCommandEventArgs e) {
            try {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                if (eCommandNameUpper == "ELIMINAR") {
                    int index = Convert.ToInt32(e.CommandArgument);
                    SucursalBO sucursal = SessionListaSucursalSeleccionada[index];
                    this.presentador.QuitarSucursal(sucursal);
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al ejecutar el evento", ETipoMensajeIU.ERROR, NombreClase + ".grdSucursales_RowCommand:" + ex.Message);
            }
        }
        protected void btnResult_Click(object sender, EventArgs e) {
            try {
                switch (ViewState_Catalogo) {
                    case ECatalogoBuscador.Sucursal:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".btnResult_Click: " + ex.Message);
            }
        }
        #endregion
    }
}