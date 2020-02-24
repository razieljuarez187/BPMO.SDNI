using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI {
    /// <summary>
    /// Control que visualiza la sección de herramientas de cabecera para una factura
    /// </summary>
    public partial class ucInformacionGeneralPSLUI : System.Web.UI.UserControl, IucInformacionGeneralPSLVIS {
        #region Eventos
        /// <summary>
        /// Evento que se ejecuta cuando la forma de pago ha cambiado
        /// </summary>
        public event EventHandler FormaPagoChanged;
        /// <summary>
        /// Evento que se ejecuta cuando la moneda a facturar cambia
        /// </summary>
        public event EventHandler MonedaChanged;
        #endregion

        #region Atributos
        /// <summary>
        /// Nombre de la clase en la cual estamos trabajando
        /// </summary>
        private string nombreClase = "ucInformacionCabeceraPSLUI";

        /// <summary>
        /// Presentador asociado a la vista
        /// </summary>
        private ucInformacionGeneralPSLPRE presentador;
        #endregion

        #region Propiedades
        /// <summary>
        /// Manejador de Evento para ver el Detalle de la Línea de Contrato
        /// </summary>
        internal CommandEventHandler VerDetalleCargoAdicionalContrato { get; set; }
        /// <summary>
        /// Manejador de Evento para remover una línea de contrato
        /// </summary>
        internal EventHandler CambioMonedaPagoContrato { get; set; }
        /// <summary>
        /// Manejador de Evento para ver el Detalle de la Línea de Contrato
        /// </summary>
        internal CommandEventHandler ActualizarDescripcionHoraLineaPago { get; set; }

        public string DescripcionMotivoLinea {
            get {
                return (String.IsNullOrEmpty(this.txtMotivoCancelacion.Text)) ? null : this.txtMotivoCancelacion.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtMotivoCancelacion.Text = value;
                else
                    this.txtMotivoCancelacion.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el identificador de la sucursal
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
        /// Obtiene o establece el nombre de la sucursal
        /// </summary>
        public string SucursalNombre {
            get {
                return (String.IsNullOrEmpty(this.txtSucursal.Text)) ? null : this.txtSucursal.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtSucursal.Text = value;
                else
                    this.txtSucursal.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el sistema de origen
        /// </summary>
        public string SistemaOrigen {
            get {
                return (String.IsNullOrEmpty(this.txtSistemaOrigen.Text)) ? null : this.txtSistemaOrigen.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtSistemaOrigen.Text = value;
                else
                    this.txtSistemaOrigen.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el tipo de transacción
        /// </summary>
        public ETipoContrato? TipoTransaccion {
            get {
                if (!String.IsNullOrEmpty(this.txtTipoTransaccion.Text)) {
                    ETipoContrato tipo = (ETipoContrato)Enum.Parse(typeof(ETipoContrato), this.txtTipoTransaccion.Text.Trim());
                    return tipo;
                }

                return null;
            }
            set {
                if (value.HasValue)
                    this.txtTipoTransaccion.Text = value.ToString();
                else
                    this.txtTipoTransaccion.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el número de referencia
        /// </summary>
        public string NumeroReferencia {
            get {
                return (String.IsNullOrEmpty(this.txtReferencia.Text)) ? null : this.txtReferencia.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtReferencia.Text = value;
                else
                    this.txtReferencia.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el código de la moneda seleccionada
        /// </summary>
        public string CodigoMoneda {
            get {
                return (String.IsNullOrEmpty(this.txtCodigoMoneda.Text)) ? null : this.txtCodigoMoneda.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtCodigoMoneda.Text = value;
                else
                    this.txtCodigoMoneda.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece la forma del pago
        /// </summary>
        public string FormaPago {
            get {
                return this.ddlFormaPago.SelectedItem.Value;
            }
            set {
                this.ddlFormaPago.ClearSelection();
                if (!String.IsNullOrEmpty(value)) {
                    ListItem item = this.ddlFormaPago.Items.FindByValue(value.ToUpper());
                    if (item != null)
                        item.Selected = true;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece la forma del pago
        /// </summary>
        public string CodigoMonedaDestino {
            get {
                return this.ddlMoneda.SelectedItem.Value;
            }
            set {
                this.ddlMoneda.ClearSelection();
                if (!String.IsNullOrEmpty(value)) {
                    ListItem item = this.ddlMoneda.Items.FindByValue(value.ToUpper());
                    if (item != null)
                        item.Selected = true;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece el tipo de cambio
        /// </summary>
        public decimal? TipoCambio {
            get {
                decimal value = 0M;
                if (decimal.TryParse(this.txtTipoCambio.Text, out value))
                    return value;

                return null;
            }
            set {
                if (value != null)
                    this.txtTipoCambio.Text = String.Format("{0:#,##0.00}", value);
                else
                    this.txtTipoCambio.Text = "0";
            }
        }

        /// <summary>
        /// Obtiene o establece los días de factura
        /// </summary>
        public int? DiasFactura {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtDiasFactura.Text))
                    id = int.Parse(this.txtDiasFactura.Text.Trim());
                return id;
            }
            set {
                if (value != null)
                    this.txtDiasFactura.Text = value.ToString();
                else
                    this.txtDiasFactura.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el tipo de taza de cambio
        /// </summary>
        public String TipoTasaCambiario {
            get {
                return (String.IsNullOrEmpty(this.txtTipoTazaCambiario.Text)) ? null : this.txtTipoTazaCambiario.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtTipoTazaCambiario.Text = value;
                else
                    this.txtTipoTazaCambiario.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece los días de crédito
        /// </summary>
        public int? DiasCredito {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtDiasCredito.Text))
                    id = int.Parse(this.txtDiasCredito.Text.Trim());
                return id;
            }
            set {
                if (value != null)
                    this.txtDiasCredito.Text = value.ToString();
                else
                    this.txtDiasCredito.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el limite de crédito
        /// </summary>
        public decimal? LimiteCredito {
            get {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtLimiteCredito.Text))
                    temp = Decimal.Parse(this.txtLimiteCredito.Text.Trim().Replace(",", ""));
                return temp;
            }
            set {
                if (value != null)
                    this.txtLimiteCredito.Text = string.Format("{0:#,##0.00}", value);
                else
                    this.txtLimiteCredito.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el crédito disponible
        /// </summary>
        public decimal? CreditoDisponible {
            get {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtCreditoDisponible.Text))
                    temp = Decimal.Parse(this.txtCreditoDisponible.Text.Trim().Replace(",", ""));
                return temp;
            }
            set {
                if (value != null)
                    this.txtCreditoDisponible.Text = string.Format("{0:#,##0.00}", value);
                else
                    this.txtCreditoDisponible.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el departamento
        /// </summary>
        public string Departamento {
            get {
                return (String.IsNullOrEmpty(this.txtDepartamento.Text)) ? null : this.txtDepartamento.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtDepartamento.Text = value;
                else
                    this.txtDepartamento.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece bandera cores
        /// </summary>
        public bool BanderaCores {
            get {
                if (this.txtBanderaCores.Text == "SI")
                    return true;

                if (this.txtBanderaCores.Text == "NO")
                    return false;

                return false;
            }
            set {
                if (value)
                    this.txtBanderaCores.Text = "SI";
                else
                    this.txtBanderaCores.Text = "NO";
            }
        }

        public List<LineaContratoPSLBO> LineasContrato {
            get;
            set;
        }

        /// <summary>
        /// Obtiene o establece el campo de Observaciones
        /// </summary>
        public string Observaciones {
            get {
                return (String.IsNullOrEmpty(this.txtObservaciones.Text)) ? null : this.txtObservaciones.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtObservaciones.Text = value;
                else
                    this.txtObservaciones.Text = string.Empty;
            }
        }

        /// <summary>
        /// Identificador del pago id en el grid de pago
        /// </summary>
        public int pagoIdGrid {
            get {
                return Convert.ToInt32(this.hdnPagoIDGrid.Value);
            }
        }

        /// <summary>
        /// Tipo de actualización  si es hora o descripción
        /// </summary>
        public string tipoLinea {
            get {
                return this.hdnTipoLinea.Value;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Proceso que se ejecuta cuando se crea una nueva factura
        /// </summary>
        public void PrepararNuevo() {
            this.SucursalID = null;
            this.SucursalNombre = String.Empty;
            this.SistemaOrigen = String.Empty;
            this.TipoTransaccion = null;
            this.NumeroReferencia = String.Empty;
            this.CodigoMoneda = String.Empty;
            this.FormaPago = String.Empty;
            this.TipoCambio = 0M;
            this.DiasFactura = 0;
            this.TipoTasaCambiario = String.Empty;
            this.DiasCredito = 0;
            this.LimiteCredito = 0M;
            this.CreditoDisponible = 0M;
            this.Departamento = String.Empty;
            this.BanderaCores = false;
        }

        /// <summary>
        /// Método que se encarga de dispara el evento FormaPagoChanged
        /// </summary>
        /// <param name="e">Argumentos a enviar al controlador de eventos</param>
        protected virtual void OnFormaPagoChanged(EventArgs e) {
            if (this.FormaPagoChanged != null)
                this.FormaPagoChanged(this, e);
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

        /// <summary>
        /// Visualiza las formas de pago para una factura
        /// </summary>
        /// <param name="formasPago">Lista de formas de pago</param>
        public void MostrarFormasPago(IList<String> formasPago) {
            this.ddlFormaPago.Items.Clear();
            foreach (String valor in formasPago) {
                ListItem item = new ListItem();
                item.Value = valor.ToUpper();
                item.Text = valor.ToUpper();
                this.ddlFormaPago.Items.Add(item);
            }
        }

        /// <summary>
        /// Obtiene la carpeta raiz donde se encuentra la aplicación
        /// </summary>
        /// <returns>Dirección donde se encuentra la aplicación</returns>
        public string ObtenerCarpetaRaiz() {
            return this.Server.MapPath("~");
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por default
        /// </summary>
        public ucInformacionGeneralPSLUI() {
            this.presentador = new ucInformacionGeneralPSLPRE(this);

            Page currentPage = HttpContext.Current.Handler as Page;
            currentPage.PreLoad += new EventHandler(Page_PreLoad);

        }
        #endregion

        #region Eventos
        /// <summary>
        /// Método delegado para el evento de pre carga de la página
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        private void Page_PreLoad(object sender, EventArgs e) {
            try {
                if (!this.IsPostBack) {
                    this.presentador.PrepararNuevo();
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_PreLoad:" + ex.Message);
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
        /// Método delegado para el evento de SelectedIndexChanged de ddlFormaPago 
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void ddlFormaPago_SelectedIndexChanged(object sender, EventArgs e) {
            this.OnFormaPagoChanged(e);
        }

        /// <summary>
        /// Método que se encarga de dispara el evento MonedaChanged
        /// </summary>
        /// <param name="e">Argumentos a enviar al controlador de eventos</param>
        protected virtual void OnMonedaChanged(EventArgs e) {
            if (this.MonedaChanged != null)
                this.MonedaChanged(this, e);
        }

        /// <summary>
        /// Método delegado para el evento de SelectedIndexChanged de ddlMoneda 
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void ddlMoneda_SelectedIndexChanged(object sender, EventArgs e) {
            this.OnMonedaChanged(e);
        }



        /// <summary>
        /// Despliega en pantalla las unidades del contrato
        /// </summary>
        /// <param name="dt">Tabla con la información de las unidades</param>
        public void EstablecerUnidadesContrato(List<LineaContratoPSLBO> LineasContrato) {
            this.GridLineasUnidad.DataSource = LineasContrato;
            this.GridLineasUnidad.DataBind();
        }

        public void MostrarUnidadesContrato(object LineasContrato) {
            List<LineasFacturaModel> listaUnidades = LineasContrato as List<LineasFacturaModel>;

            this.GridLineasUnidad.DataSource = listaUnidades;
            this.GridLineasUnidad.DataBind();
        }

        /// <summary>
        /// Provee las monedas que pueden aplicar a los contratos
        /// </summary>
        /// <param name="monedas">Listado de monedas que puede seleccionar el usuario</param>
        public void EstablecerOpcionesMoneda(Dictionary<string, string> monedas) {
            if (ReferenceEquals(monedas, null))
                monedas = new Dictionary<string, string>();

            monedas.Add("-1", "Moneda del Contrato");

            this.ddlMoneda.DataSource = monedas;
            this.ddlMoneda.DataValueField = "key";
            this.ddlMoneda.DataTextField = "value";
            this.ddlMoneda.DataBind();
        }

        protected void GridLineasUnidad_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {
                if (e.Row.RowType == DataControlRowType.DataRow) {
                    LineasFacturaModel linea = ((LineasFacturaModel)e.Row.DataItem);
                    var label = e.Row.FindControl("lblVIN") as Label;

                    var button = e.Row.FindControl("ibtnDetalles") as ImageButton;
                    button.Visible = true;

                    // Numero de Serie
                    if (label != null) {
                        if (linea != null && linea.NumeroSerie != null) label.Text = linea.NumeroSerie;
                        else label.Text = string.Empty;
                    }

                    // Modelo
                    label = e.Row.FindControl("lblModelo") as Label;
                    if (label != null) {
                        if (linea != null && linea.ModeloUnidad != null) label.Text = linea.ModeloUnidad;
                        else label.Text = string.Empty;
                    }

                    //Tipo Tarifa
                    label = e.Row.FindControl("lblTipoTarifa") as Label;
                    if (label != null) {
                        label.Text = linea.TipoTarifa != null ? linea.TipoTarifa.ToString() : string.Empty;
                    }

                    //Turno
                    label = e.Row.FindControl("lblTurno") as Label;
                    if (label != null) {
                        label.Text = linea.Turno != null ? linea.Turno.ToString() : string.Empty;
                    }

                    //Tarifa
                    label = e.Row.FindControl("lblTarifa") as Label;
                    if (label != null) {
                        label.Text = linea.Tarifa != null ? linea.Tarifa.ToString() : string.Empty;
                    }

                    //Hora adicional
                    label = e.Row.FindControl("lblAdicional") as Label;
                    if (label != null) {
                        label.Text = linea.DescripcionhoraAdicional != null ? linea.DescripcionhoraAdicional.ToString() : string.Empty;
                    }

                    //Descripción
                    label = e.Row.FindControl("lblDescripcion") as Label;
                    if (label != null) {
                        label.Text = linea.DescripcionhoraAdicional != null ? linea.DescripcionhoraAdicional.ToString() : string.Empty;
                    }

                    //# Cargos
                    label = e.Row.FindControl("lblCargos") as Label;
                    if (label != null) {
                        label.Text = linea.NumeroCargo != null ? linea.NumeroCargo.ToString() : string.Empty;
                    }

                    //Monto de Cargos
                    label = e.Row.FindControl("lblMontoCargos") as Label;
                    if (label != null) {
                        label.Text = linea.MontoCargo != null ? string.Format("{0:c2}", linea.MontoCargo.Value) : string.Empty;
                    }

                }
            } catch (Exception ex) {
                MostrarMensaje("Se han encontrado Inconsistencias al presentar el detalle del contrato.",
                               ETipoMensajeIU.ERROR, nombreClase + ".grdLineasContrato_RowDataBound: " + ex.Message);
            }
        }

        protected void GridLineasUnidad_RowCommand(object sender, GridViewCommandEventArgs e) {
            try {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                if (e.CommandArgument == null) return;

                int index = 0;

                if (!Int32.TryParse(e.CommandArgument.ToString(), out index))
                    return;

                switch (eCommandNameUpper) {
                    case "CMDDETALLES": {
                            if (VerDetalleCargoAdicionalContrato != null) {
                                var c = new CommandEventArgs("PagoUnidadContratoID", index);

                                VerDetalleCargoAdicionalContrato.Invoke(sender, c);
                            }
                        }
                        break;
                    case "ACTUALIZARHORA":
                        presentador.DesplegarDescripcionLinea("HORA", index);
                        this.hdnPagoIDGrid.Value = index.ToString();
                        this.hdnTipoLinea.Value = "HORA";
                        RegistrarScript(null, "MostrarDialogo(true,'HORA EXCEDENTE');");
                        break;
                    case "ACTUALIZARDESC":
                        presentador.DesplegarDescripcionLinea("LINEA", index);
                        this.hdnPagoIDGrid.Value = index.ToString();
                        this.hdnTipoLinea.Value = "LINEA";
                        RegistrarScript(null, "MostrarDialogo(true,'DESCRIPCIÓN LÍNEA');");
                        break;
                }

            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al eliminar la unidad del contrato.", ETipoMensajeIU.ERROR, nombreClase + ".grdLineasContrato_RowCommand: " + ex.Message);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e) {
            try {
                var btn = sender as ImageButton;
                if (btn != null) {
                    Int32 pagoId = int.Parse(btn.CommandArgument);
                    this.RegistrarScript("EditarLinea", "confirmarCancelarPago();");
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al procesar el pago a cancelar.", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click" + ex.Message);
            }
        }

        protected void btnCancelarPagoPendiente_Click(object sender, EventArgs e) {
            try {
                RegistrarScript(null, "MostrarDialogo(true);");
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al cancelar el pago.", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelarPagoPendiente_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script) {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        /// <summary>
        /// Actualiza la información de la descripción de la línea o de la hora adicional
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnValidarAutorizacion_Click(object sender, EventArgs e) {
            try {

                if (ActualizarDescripcionHoraLineaPago != null) {
                    var c = new CommandEventArgs("PagoUnidadContratoID", Convert.ToInt32(this.hdnPagoIDGrid.Value));

                    ActualizarDescripcionHoraLineaPago.Invoke(sender, c);
                }

                this.txtMotivoCancelacion.Text = "";
                this.hdnPagoIDGrid.Value = "0";
                this.hdnTipoLinea.Value = "";
            } catch (Exception ex) {
                MostrarMensaje("Ocurrió un Error al validar el código de autorización", ETipoMensajeIU.INFORMACION);
            }
        }

        /// <summary>
        /// Guarda en sesión el modelo armado para las líneas
        /// </summary>
        public object InformacionLineasFacturaModel {
            get {
                return (object)Session["LineasFacturaModel"];
            }

            set {
                Session["LineasFacturaModel"] = value;
            }
        }

        protected void btnDescartarActualizacion_Click(object sender, EventArgs e) {
            hdnMostrarDialogoPago.Value = "0";
        }

        public void permitirCaptura(bool habilitar) {
            this.ddlFormaPago.Enabled = habilitar;
            this.ddlMoneda.Enabled = habilitar;
            this.txtObservaciones.Enabled = habilitar;
            this.btnValidarAutorizacion.Enabled = habilitar;
        }

        protected void ddlMoneda_SelectedIndexChanged1(object sender, EventArgs e) {
            if (CambioMonedaPagoContrato != null)
                CambioMonedaPagoContrato.Invoke(sender, e);
        }

        #endregion

    }
}