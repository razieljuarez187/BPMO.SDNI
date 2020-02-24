//Satisface al Caso de uso CU005 - Armar Paquetes Facturacion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.eFacturacion.Procesos.BO;
using BPMO.eFacturacion.Procesos.Enumeradores;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI {
    /// <summary>
    /// Control que visualiza la sección de Costos adicionales para una factura
    /// </summary>
    public partial class ucCostosAdicionalesFacturaContratoUI : System.Web.UI.UserControl, IucCostosAdicionalesFacturaContratoVIS {
        #region Constantes
        /// <summary>
        /// Clave del Guid asignado la instancia de la página
        /// </summary>
        private const String PAGEGUIDINDEX = "__PAGEGUID";
        #endregion

        #region Campos
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "ucCostosAdicionalesFacturaContratoUI";

        /// <summary>
        /// Número de registro que se esta agregando a la lista
        /// </summary>
        public int numeroCostosAdicionales = 0;

        /// <summary>
        /// Presentador asociado a la vista
        /// </summary>
        private ucCostosAdicionalesFacturaContratoPRE presentador;

        /// <summary>
        /// Id único global de la instancia del control
        /// </summary>        
        private Guid _GUID;

        /// <summary>
        /// Enumeración para controlar los objetos del buscador
        /// </summary>
        public enum ECatalogoBuscador {
            ProductoServicio
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Evento que se ejecuta cuando la moneda a facturar cambia
        /// </summary>
        public event EventHandler MonedaChanged;

        /// <summary>
        /// Manejador de Eventos al hacer clic sobre el botón Cancelar
        /// </summary>
        public EventHandler CancelarClick { get; set; }
        public EventHandler GuardarClick { get; set; }
        #endregion

        #region Propiedades
        ///<summary>
        ///Obtiene un valor que representa un Id único global de la instancia del control
        ///</summary>
        ///<value>
        ///Objeto GUID con clave única de la instancia
        ///</value>
        internal Guid GUID {
            get {
                if (this._GUID == Guid.Empty)
                    this.RegisterGuid();

                return this._GUID;
            }
        }

        /// <summary>
        /// Obtiene o establece la moneda de destino asociada a la prefactua
        /// </summary>
        /// <value>
        /// Objeto de tipo String 
        /// </value>
        public string CodigoMoneda {
            get;
            //{
            //    return this.ddlMoneda.SelectedItem.Value;
            //}
            set;
            //{
            //    this.ddlMoneda.SelectedValue = value;
            //}
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el tipo de contrato de la factura en curso
        /// </summary>
        /// <value>Objeto de tipo ETipoContrato</value>
        public ETipoContrato? TipoContrato {
            get {
                ConfigurarFacturacionUI configurarFacturacionUI = this.Page as ConfigurarFacturacionUI;
                if (configurarFacturacionUI != null && configurarFacturacionUI.PagoActual != null)
                    return configurarFacturacionUI.PagoActual.ReferenciaContrato.TipoContratoID;

                return null;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el tipo de renglón de un concepto a agregar
        /// </summary>
        /// <value>Objeto de tipo ETipoRenglon</value>
        public ETipoRenglon Concepto {
            get {
                if (this.ddlConcepto.SelectedIndex == -1)
                    return null;

                int selectedValue = Convert.ToInt32(this.ddlConcepto.SelectedValue);
                ETipoRenglon result = typeof(ETipoRenglon)
                                            .GetFields(BindingFlags.Public | BindingFlags.Static)
                                            .Where(x => typeof(ETipoRenglon).IsAssignableFrom(x.FieldType) &&
                                                        Object.Equals((x.GetValue(null) as IConvertible).ToInt32(CultureInfo.CurrentCulture), selectedValue))
                                            .Select(x => x.GetValue(null))
                                            .Cast<ETipoRenglon>()
                                            .FirstOrDefault();

                return result;
            }
            set {
                this.ddlConcepto.ClearSelection();
                if (value != null) {
                    ListItem item = this.ddlConcepto.Items.FindByValue(value.ToInt32(CultureInfo.CurrentCulture).ToString());
                    if (item != null)
                        item.Selected = true;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece el precio de un concepto a agregar
        /// </summary>
        /// <value>Objeto de tipo decimal</value>
        public decimal? Precio {
            get {
                decimal value = 0M;
                if (decimal.TryParse(this.txtPrecio.Text, out value))
                    return value;

                return null;
            }
            set {
                if (value != null)
                    this.txtPrecio.Text = String.Format("{0:#,##0.00##}", value);
                else
                    this.txtPrecio.Text = "";
            }
        }

        /// <summary>
        /// Identificador de Producto o Servicio (SAT)
        /// </summary>
        public int? ProductoServicioId {
            get { return (string.IsNullOrEmpty(this.hdnProductoServicioId.Value)) ? null : (int?)int.Parse(this.hdnProductoServicioId.Value); }
            set { this.hdnProductoServicioId.Value = (value != null) ? value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Clave de Producto o Servicio (SAT)
        /// </summary>
        public string ClaveProductoServicio {
            get { return (string.IsNullOrEmpty(this.txtClaveProductoServicio.Text)) ? null : this.txtClaveProductoServicio.Text.Trim().ToUpper(); }
            set { this.txtClaveProductoServicio.Text = (value != null) ? value.ToUpper() : string.Empty; }
        }
        /// <summary>
        /// Descripción de Producto o Servicio (SAT)
        /// </summary>
        public string DescripcionProductoServicio {
            get { return (string.IsNullOrEmpty(this.txtDescripcionProductoServicio.Text)) ? null : this.txtDescripcionProductoServicio.Text.Trim().ToUpper(); }
            set { this.txtDescripcionProductoServicio.Text = (value != null) ? value.ToUpper() : string.Empty; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la descripción de un concepto a agregar
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string Descripcion {
            get {
                return this.txtDescripcion.Text;
            }
            set {
                this.txtDescripcion.Text = value;
            }
        }

        /// <summary>
        /// Obtiene o establece la lista de costos adicionales que se han agregado
        /// </summary>
        /// <value>Objeto de tipo IList de DetalleTransaccionBO</value>
        public IList<DetalleTransaccionBO> CostosAdicionales {
            get {
                IList<DetalleTransaccionBO> objeto = null;
                string nombreSession = String.Format("COSTOS_ADICIONALES_{0}", this.ViewState_Guid);
                if (!(this.Session[nombreSession] is IList<DetalleTransaccionBO>))
                    this.Session[nombreSession] = new List<DetalleTransaccionBO>();

                objeto = (IList<DetalleTransaccionBO>)this.Session[nombreSession];

                return objeto;
            }
            set {
                string nombreSession = String.Format("COSTOS_ADICIONALES_{0}", this.ViewState_Guid);
                if (value != null)
                    this.Session[nombreSession] = value;
                else
                    this.Session.Remove(nombreSession);
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el control se encuentra activo
        /// </summary>
        public bool Enabled {
            get {
                return this.pnlContenedor.Enabled;
            }
            set {
                this.pnlContenedor.Enabled = value;
            }
        }

        /// <summary>
        /// Manejador de Evento para ver el Detalle de la Línea de Contrato
        /// </summary>
        internal CommandEventHandler VerCargoAdicionalContrato { get; set; }

        public List<LineasFacturaModel> ObtenercostosAdicionales {
            get {
                return (object)Session["LineasFacturaModel"] as List<LineasFacturaModel>;
            }
        }

        public object GuardarcostosAdicionalesLineas {
            set {
                Session["LineasFacturaModel"] = value;
            }
        }

        public object LineaCostoFacturaModel {
            get { return Session["LineasFacturaModel"]; }
            set { Session["LineasFacturaModel"] = value; }
        }

        public int PagoContratoID {
            get { return (int)Session["PagoContratoID"]; }
            set { Session["PagoContratoID"] = value; }
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
        /// <summary>
        /// Constructor por default
        /// </summary>
        public ucCostosAdicionalesFacturaContratoUI() {
            this.presentador = new ucCostosAdicionalesFacturaContratoPRE(this);

            Page page = HttpContext.Current.Handler as Page;
            page.PreLoad += new EventHandler(page_PreLoad);

        }
        #endregion

        #region Métodos
        /// <summary>
        /// Limpia los datos de sesión
        /// </summary>
        public void LimpiarSesion() {
            this.CostosAdicionales = null;
            this.MostrarListaCostosAdicionales();
        }

        /// <summary>
        /// Visualiza los costos adicionales actualmente
        /// </summary>
        public void MostrarListaCostosAdicionales() {
            this.grvCostosAdicionales.DataSource = this.CostosAdicionales.Count > 0 ? this.CostosAdicionales : null; 
            this.grvCostosAdicionales.DataBind();
        }

        /// <summary>
        /// Visualiza los renglones permitidos para los costos adicionales
        /// </summary>
        /// <param name="tiposRenglon">Lista de tipo ETipoRenglon</param>
        public void MostrarListaTiposRenglon(IList<ETipoRenglon> tiposRenglon) {
            this.ddlConcepto.Items.Clear();
            foreach (ETipoRenglon tipoRenglon in tiposRenglon) {
                ListItem item = new ListItem();
                item.Value = tipoRenglon.ToInt32(CultureInfo.CurrentCulture).ToString();
                item.Text = tipoRenglon.ToString();
                this.ddlConcepto.Items.Add(item);
            }
        }

        /// <summary>
        /// Desplegar mensaje de Error con detalle
        /// </summary>
        /// <param name="mensaje">Descripción del mensaje</param>
        /// <param name="tipo">EMensaje tipo de mensaje a desplegar</param>
        /// <param name="detalle">Detalle del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null) {
            Site masterMsj = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
                masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
            else
                masterMsj.MostrarMensaje(mensaje, tipo);
        }

        /// <summary>
        /// Provee las monedas que pueden aplicar a los contratos
        /// </summary>
        /// <param name="monedas">Listado de monedas que puede seleccionar el usaurio</param>
        public void EstablecerOpcionesMoneda(Dictionary<string, string> monedas) {
            if (ReferenceEquals(monedas, null))
                monedas = new Dictionary<string, string>();

            monedas.Add("-1", "Moneda del Contrato");

        }

        /// <summary>
        /// Obtiene la carpeta raiz donde se encuentra la aplicación
        /// </summary>
        /// <returns>Dirección donde se encuentra la aplicación</returns>
        public string ObtenerCarpetaRaiz() {
            return this.Server.MapPath("~");
        }

        /// <summary>
        /// Registra la clave única global del control en la página
        /// </summary>
        private void RegisterGuid() {
            string hiddenFieldValue = this.Request.Form[ucCostosAdicionalesFacturaContratoUI.PAGEGUIDINDEX];
            if (hiddenFieldValue == null) {
                this._GUID = Guid.NewGuid();
                hiddenFieldValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(this._GUID.ToString()));
            } else {
                string guidValue = Encoding.UTF8.GetString(Convert.FromBase64String(hiddenFieldValue));
                this._GUID = new Guid(guidValue);
            }

            ScriptManager.RegisterHiddenField(this.Page, ucCostosAdicionalesFacturaContratoUI.PAGEGUIDINDEX, hiddenFieldValue);
        }

        /// <summary>
        /// Proceso que se ejecuta cuando se crea una nueva factura
        /// </summary>
        public void PrepararNuevo() {
            this.LimpiarSesion();
        }

        /// <summary>
        /// Método que se encarga de dispara el evento MonedaChanged
        /// </summary>
        /// <param name="e">Argumentos a enviar al controlador de eventos</param>
        protected virtual void OnMonedaChanged(EventArgs e) {
            if (this.MonedaChanged != null)
                this.MonedaChanged(this, e);
        }

        public void AgregarCostoAdicional() {
            this.presentador.AgregarCostoAdicional();
        }

        public void MostrarCostoAdicionalFactura(int pagoUnidadPSLID) {
            object modelounidad = (object)Session["LineasFacturaModel"];
            int folioPagoUnidad = Convert.ToInt32(Session["PagoUnidadContratoID"]);
            List<LineasFacturaModel> lineasModeloFactura = modelounidad as List<LineasFacturaModel>;

            LineasFacturaModel modeloSelec = lineasModeloFactura.FirstOrDefault(a => a.PagoContratoPSLID == folioPagoUnidad);

            if (modeloSelec != null) {
                this.CostosAdicionales = modeloSelec.detalleTransaccion;
                this.lblTitulo.Text = "UNIDAD: " + modeloSelec.NumeroSerie + ", MODELO: " + modeloSelec.ModeloUnidad;
            }

            LineasFacturaModel lineaCostoAdicionalFactura = lineasModeloFactura.Where(s => s.PagoContratoPSLID == pagoUnidadPSLID).FirstOrDefault();

            MostrarListaCostosAdicionales();
        }

        #region Métodos para el Buscador
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="catalogoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda) {
            ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("BuscarCostos", "BtnBuscarCostos('" + ViewState_Guid + "','" + catalogo + "','" + btnResultCostos.ClientID + "');");
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
        #endregion

        #region Controladores de eventos
        /// <summary>
        /// Método delegado para el evento de pre carga de la página
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        private void page_PreLoad(object sender, EventArgs e) {
            try {
                if (!this.IsPostBack) {
                    this.presentador.PrepararNuevo();
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }
        }

        /// <summary>
        /// Método delegado para el evento de carga de la página
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void Page_Load(object sender, EventArgs e) {
        }

        /// <summary>
        /// Método delegado para el evento de clic al boton de "Agregar"
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void btnAgregar_Click(object sender, EventArgs e) {
            try {
                this.presentador.AgregarCostoAdicional();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al agregar elemento", ETipoMensajeIU.ERROR, this.nombreClase + ".btnAgregar_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// Método delegado para el evento de ejecución de comando de lista de costos adicionales
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void grvCostosAdicionales_RowCommand(object sender, GridViewCommandEventArgs e) {
            try {
                if (e.CommandName == "Eliminar") {
                    int id = Convert.ToInt32(e.CommandArgument);
                    this.presentador.EliminarCostoAdicional(id);
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al eliminar elemento", ETipoMensajeIU.ERROR, this.nombreClase + ".btnContinuar_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// Método delegado para el evento bindeo de datos con la lista de costos adicionales
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void grvCostosAdicionales_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row.RowType == DataControlRowType.DataRow) {
                DetalleTransaccionBO detalle = (DetalleTransaccionBO)e.Row.DataItem;
                Label label = (Label)e.Row.FindControl("lblTipoRenglon");
                label.Text = detalle.TipoRenglon.ToString();
            }
        }

        /// <summary>
        /// Método delegado para el evento de SelectedIndexChanged de ddlMoneda 
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void ddlMoneda_SelectedIndexChanged(object sender, EventArgs e) {
            this.OnConceptoChanged(e);
        }

        /// <summary>
        /// Método que se encarga de dispara el evento MonedaChanged
        /// </summary>
        /// <param name="e">Argumentos a enviar al controlador de eventos</param>
        protected virtual void OnConceptoChanged(EventArgs e) {
            //if (this.MonedaChanged != null)
            //    this.MonedaChanged(this, e);
        }

        /// <summary>
        /// Método delegado para el evento de SelectedIndexChanged de ddlMoneda 
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void ddlConcepto_SelectedIndexChanged(object sender, EventArgs e) {
            //this.OnMonedaChanged(e);
            switch (Convert.ToInt32(ddlConcepto.SelectedValue)) {
                case 11:
                    txtDescripcion.Text = "";
                    txtDescripcion.ReadOnly = false;
                    break;
                case 10:
                    txtDescripcion.Text = "Cobro de daños";
                    txtDescripcion.ReadOnly = true;
                    break;
                case 13:
                    txtDescripcion.Text = "Tramos de cable con terminales";
                    txtDescripcion.ReadOnly = true;
                    break;
                case 12:
                    txtDescripcion.Text = "Servicio de conexión y desconexión";
                    txtDescripcion.ReadOnly = true;
                    break;
                case 9:
                    txtDescripcion.Text = "Servicio de seguro";
                    txtDescripcion.ReadOnly = true;
                    break;
                case 8:
                    txtDescripcion.Text = "Suministro de combustible";
                    txtDescripcion.ReadOnly = true;
                    break;
                case 14:
                    txtDescripcion.Text = "Servicio de maniobra";
                    txtDescripcion.ReadOnly = true;
                    break;
                case 15:
                    txtDescripcion.Text = "Diagnostico, revisión y mantenimiento en general";
                    txtDescripcion.ReadOnly = true;
                    break;
                case 6:
                    txtDescripcion.Text = "Renta de equipo";
                    txtDescripcion.ReadOnly = true;
                    break;
                case 16:
                    txtDescripcion.Text = "Servicio de guardia";
                    txtDescripcion.ReadOnly = true;
                    break;
                default:
                    txtDescripcion.Text = "";
                    txtDescripcion.ReadOnly = false;
                    break;
            }

        }

        /// <summary>
        /// Método para el evento TextChanged de txtClaveProductoServicio
        /// </summary>
        /// <param name="sender">Objeto que desencadena el evento</param>
        /// <param name="e">Argumento asociado al evento</param>
        protected void txtClaveProductoServicio_TextChanged(object sender, EventArgs e) {
            try {
                string clvProducto = this.ClaveProductoServicio;
                Session_BOSelecto = null;
                DesplegarBOSelecto(ECatalogoBuscador.ProductoServicio);

                this.ClaveProductoServicio = clvProducto;
                if (!String.IsNullOrWhiteSpace(clvProducto))
                    EjecutaBuscador("ProductoServicio", ECatalogoBuscador.ProductoServicio);

                this.ProductoServicioId = null;
                this.ClaveProductoServicio = null;
                this.DescripcionProductoServicio = null;
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar un Producto", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtClaveProductoServicio: " + ex.Message);
            }
        }

        /// <summary>
        /// Método para el evento click de ibtnBuscarProductoServicio
        /// </summary>
        /// <param name="sender">Objeto que desencadena el evento</param>
        /// <param name="e">Argumento asociado al evento</param>
        protected void ibtnBuscarProductoServicio_Click(object sender, ImageClickEventArgs e) {
            try {
                EjecutaBuscador("ProductoServicio&hidden=0", ECatalogoBuscador.ProductoServicio);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar el producto", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarProductoServicio_Click: " + ex.Message);
            }
        }

        /// <summary>
        /// Método para el evento clic de btnResult
        /// </summary>
        /// <param name="sender">Objeto que desencadena el evento</param>
        /// <param name="e">Argumento asociado al evento</param>
        protected void btnResultCostos_Click(object sender, EventArgs e) {
            try {
                switch (ViewState_Catalogo) {
                    case ECatalogoBuscador.ProductoServicio:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResultCostos_Click: " + ex.Message);
            }
        }


        /// <summary>
        /// Regresa a la pantalla principal del contrato
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">Argumento</param>
        protected void btnCancelarCargos_Click(object sender, EventArgs e) {
            try {
                if (CancelarClick != null) CancelarClick.Invoke(sender, e);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelarCargos_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Agrega a unidad a la línea del contrato
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">Argumento</param>
        protected void btnAgregarCargos_Click(object sender, EventArgs e) {
            try {
                if (GuardarClick != null) GuardarClick.Invoke(sender, e);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".btnAgregarCargos_Click: " + ex.Message);
            }
        }

        /// <summary>
        /// Permite habilitar / deshabilitar los controles del grid para agregar conceptos
        /// </summary>
        /// <param name="habilitar">Indica si se habilita o deshabilita el control</param>
        public void permitirCaptura(bool habilitar) {
            this.ddlConcepto.Enabled = habilitar;
            this.txtPrecio.Enabled = habilitar;
            this.txtClaveProductoServicio.Enabled = habilitar;
            this.ibtnBuscarProductoServicio.Enabled = habilitar;
            this.txtDescripcion.Enabled = habilitar;
            this.btnAgregar.Enabled = habilitar;
            this.btnAgregarConceptos.Enabled = habilitar;
        }

        #endregion
    }
}
