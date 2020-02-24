// Satisface al Caso de uso CU005 - Armar Paquetes Facturacion
// Satisface a la solicitud de cambio SC0016

using System;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.MapaSitio.UI;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI {
    /// <summary>
    /// Control que visualiza la sección de herramientas de cabezera para una factura
    /// </summary>
    public partial class ucInformacionCabeceraUI : System.Web.UI.UserControl, IucInformacionCabeceraVIS {
        #region Eventos
        /// <summary>
        /// Evento que se ejecuta cuando la forma de pago ha cambiado
        /// </summary>
        public event EventHandler FormaPagoChanged;

        #endregion

        #region Atributos
        /// <summary>
        /// Nombre de la clase en la cual estamos trabajando
        /// </summary>
        private string nombreClase = "ucInformacionCabeceraUI";

        /// <summary>
        /// Presentador asociado a la vista
        /// </summary>
        private ucInformacionCabeceraPRE presentador;
        #endregion

        #region Propiedades
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

        /// <summary>
        /// Obtiene o establece un valor que determina si el control se encuentra activo
        /// </summary>
        public bool Enabled {
            set {
                this.ddlFormaPago.Enabled = value;
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
        public ucInformacionCabeceraUI() {
            this.presentador = new ucInformacionCabeceraPRE(this);

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
        #endregion
    }
}