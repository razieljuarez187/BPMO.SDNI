using System;
using System.Configuration;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class TarifaPersonalizadaPSLUI : System.Web.UI.Page, ITarifaPersonalizadaPSLVIS {
        #region Atributos
        private TarifaPersonalizadaPSLPRE presentador;
        private const string NombreClase = "TarifaPersonalizadaPSLUI";
        #endregion
        #region Propiedades
        /// <summary>
        /// Identificador de la Unidad Operativa
        /// </summary>
        public int? UnidadOperativaID {
            get {
                int? value = null;

                if (!string.IsNullOrEmpty(hdnUnidadOperativaID.Value.Trim()))
                    value = int.Parse(hdnUnidadOperativaID.Value.Trim());

                return value;
            }
            set {
                hdnUnidadOperativaID.Value = value != null ? value.Value.ToString("") : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el identificador del modelo
        /// </summary>
        public int? ModeloID {
            get {
                if (!string.IsNullOrEmpty(this.hdnModeloID.Value) && !string.IsNullOrWhiteSpace(this.hdnModeloID.Value)) {
                    int val;
                    return Int32.TryParse(this.hdnModeloID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnModeloID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        /// <summary>
        /// Obtiene o establece el id de la sucursal de la unidad
        /// </summary>
        public int? SucursalID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnSucursalID.Value.Trim()))
                    id = int.Parse(this.hdnSucursalID.Value.Trim());
                return id;
            }
            set { this.hdnSucursalID.Value = value != null ? value.ToString() : String.Empty; }
        }

        /// <summary>
        /// ModuloID
        /// </summary>
        public int? ModuloID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnModuloID.Value.Trim()))
                    id = int.Parse(hdnModuloID.Value.Trim());
                return id;
            }
            set { this.hdnModuloID.Value = value != null ? value.ToString() : String.Empty; }
        }

        /// <summary>
        /// UsuarioID
        /// </summary>
        public int? UsuarioID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnUsuarioID.Value.Trim()))
                    id = int.Parse(hdnUsuarioID.Value.Trim());
                return id;
            }
            set { this.hdnUsuarioID.Value = value != null ? value.ToString() : String.Empty; }
        }

        /// <summary>
        /// CuentaClienteID
        /// </summary>
        public int? CuentaClienteID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnCuentaClienteID.Value.Trim()))
                    id = int.Parse(hdnCuentaClienteID.Value.Trim());
                return id;
            }
            set { this.hdnCuentaClienteID.Value = value != null ? value.ToString() : String.Empty; }
        }

        #region Personalización Tarifa
        /// <summary>
        /// Obtiene o Establece la etiqueta
        /// </summary>
        public string TarifaPersonalizadaEtiqueta {
            get {
                return (String.IsNullOrEmpty(this.lblTarifaPersonalizadaEtiqueta.Text)) ? null : this.lblTarifaPersonalizadaEtiqueta.Text.Trim().ToUpper();
            }
            set {
                this.lblTarifaPersonalizadaEtiqueta.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece la tarifa diaria para la tarifa personalizada
        /// </summary>
        public decimal? TarifaPersonalizadaTarifa {
            get {
                if (!string.IsNullOrEmpty(this.txtTarifaPersonalizadaTarifa.Text.Trim())) {
                    decimal val;
                    return decimal.TryParse(this.txtTarifaPersonalizadaTarifa.Text.Trim(), out val) ? (decimal?)val : null;
                }
                return null;
            }
            set {
                txtTarifaPersonalizadaTarifa.Text = value.HasValue ? string.Format("{0:#,##0.00##}", value.Value) : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece la tarifa diaria para la tarifa personalizada con descuento
        /// </summary>
        public decimal? TarifaPersonalizadaTarifaConDescuento {
            get {
                if (!string.IsNullOrEmpty(this.txtTarifaPersonalizadaTarifaConDescuento.Text.Trim())) {
                    decimal val;
                    return decimal.TryParse(this.txtTarifaPersonalizadaTarifaConDescuento.Text.Trim(), out val) ? (decimal?)val : null;
                }
                return null;
            }
            set {
                txtTarifaPersonalizadaTarifaConDescuento.Text = value.HasValue ? string.Format("{0:#,##0.00##}", value.Value) : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o Establece el turno
        /// </summary>
        public string TarifaPersonalizadaTurno {
            get {
                return (String.IsNullOrEmpty(this.txtTarifaPersonalizadaTurno.Text)) ? null : this.txtTarifaPersonalizadaTurno.Text.Trim().ToUpper();
            }
            set {
                this.txtTarifaPersonalizadaTurno.Text = value;
            }
        }

        /// <summary>
        /// Obtiene o Establece el código de autorización
        /// </summary>
        public string TarifaPersonalizadaCodigoAutorizacion {
            get {
                return string.IsNullOrEmpty(hdnCodigoAutorizacion.Value.Trim()) ? null : hdnCodigoAutorizacion.Value;
            }
            set {
                hdnCodigoAutorizacion.Value = value ?? "";
                txtTarifaPersonalizadaCodigoAutorizacion.Text = "";
            }
        }

        /// <summary>
        /// Obtiene o establece el porcentaje de descuento a otorgar
        /// </summary>
        public decimal? TarifaPersonalizadaPorcentajeDescuento {
            get {
                decimal? descuento = null;
                if (!String.IsNullOrEmpty(this.txtTarifaPersonalizadaPorcentajeDescuento.Text.Trim()))
                    descuento = decimal.Parse(this.txtTarifaPersonalizadaPorcentajeDescuento.Text.Trim().Replace(",", ""));
                return descuento;
            }
            set { this.txtTarifaPersonalizadaPorcentajeDescuento.Text = value != null ? String.Format("{0:#,##0.00}", value) : String.Empty; }
        }

        /// <summary>
        /// Obtiene o Establece el tipo de tarifa
        /// </summary>
        public string TarifaPersonalizadaTipoTarifa {
            get {
                return (String.IsNullOrEmpty(this.txtTarifaPersonalizadaTipoTarifa.Text)) ? null : this.txtTarifaPersonalizadaTipoTarifa.Text.Trim().ToUpper();
            }
            set {
                this.txtTarifaPersonalizadaTipoTarifa.Text = value;
            }

        }

        /// <summary>
        /// obtiene o establece la tarifa por hr adicional de la tarifa personalizada
        /// </summary>
        public decimal? TarifaPersonalizadaTarifaHrAdicional {
            get {
                if (!String.IsNullOrEmpty(this.txtTarifaPersonalizadaTarifaHrAdicional.Text.Trim()))
                    return Decimal.Parse(this.txtTarifaPersonalizadaTarifaHrAdicional.Text.Trim().Replace(",", ""));
                return null;
            }
            set {
                this.txtTarifaPersonalizadaTarifaHrAdicional.Text = value != null ? String.Format("{0:#,##0.00}", value) : String.Empty;
            }
        }

        ///// <summary>
        ///// Porcentaje de descuento máximo aplicable al cliente
        ///// </summary>
        public decimal? TarifaPersonalizadaDescuentoMax {
            get {
                if (!String.IsNullOrEmpty(hdnTarifaPersonalizaDescuentoMax.Value.Trim()))
                    return Decimal.Parse(hdnTarifaPersonalizaDescuentoMax.Value.Trim());
                return null;
            }
            set {
                this.hdnTarifaPersonalizaDescuentoMax.Value = value != null ? value.ToString() : "";
            }
        }

        public bool esTarifaAlza {
            get {
                bool val = false;
                if (!string.IsNullOrEmpty(this.hdnTarifaAlza.Value) && this.hdnTarifaAlza.Value == "1") {
                    val = true;
                }
                return val;
            }
        }

        public decimal? TarifaBase {
            get {
                decimal val;
                if (string.IsNullOrEmpty(this.txtTarifaPersonalizadaTarifa.Text))
                    this.txtTarifaPersonalizadaTarifa.Text = "0";
                return decimal.TryParse(this.hdnTarifaBase.Value, out val) ? (decimal) val : decimal.Parse(this.txtTarifaPersonalizadaTarifa.Text);
            }
            set {
                hdnTarifaBase.Value = value.HasValue ?  value.Value.ToString() : string.Empty;
            }
        }
        public decimal? DescuentoBase {
            get {
                decimal val = 0;
                if (string.IsNullOrEmpty(this.txtTarifaPersonalizadaPorcentajeDescuento.Text))
                      this.txtTarifaPersonalizadaPorcentajeDescuento.Text = "0";
                return decimal.TryParse(this.hdnDescuentoBase.Value, out val) ? (int)val : decimal.Parse(this.txtTarifaPersonalizadaPorcentajeDescuento.Text);
            }
            set {
                hdnDescuentoBase.Value = value.HasValue ? value.Value.ToString() : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o Establece el porcentaje del seguro de la UO
        /// </summary>
        public decimal? TarifaPersonalizadaPorcentajeSeguro {
            get {
                if (!String.IsNullOrEmpty(txtTarifaPersonalizadaPorcentajeSeguro.Text.Trim()))
                    return Decimal.Parse(txtTarifaPersonalizadaPorcentajeSeguro.Text.Trim());
                return null;
            }
            set {
                this.txtTarifaPersonalizadaPorcentajeSeguro.Text = value != null ? String.Format("{0:##0.00}", value) : string.Empty;
            }
        }

        #region Seguridad
        private string Logueo {
            get { return ConfigurationManager.AppSettings["Logueo"]; }
        }
        /// <summary>
        /// Verifica si el último Post registrado en la página ha superado el timeout de la session
        /// si así fue invoca a finalizar la sesión del usuario
        /// </summary>
        protected void VerificaSession() {
            if (Session["UltimoPost"] == null) {
                FinalizarSession();
            } else {
                DateTime ultimoPost = ((DateTime)Session["UltimoPost"]).AddMinutes(Session.Timeout);
                if (DateTime.Compare(ultimoPost, DateTime.Now) < 0) {
                    FinalizarSession();
                } else {
                    Session["UltimoPost"] = DateTime.Now;
                }
            }
        }
        /// <summary>
        /// Finaliza la sesion del usuario
        /// </summary>
        private void FinalizarSession() {
            this.Session.RemoveAll();
            this.Response.Redirect(this.Logueo);
        }
        #endregion

        #endregion
        #endregion
        #region Constructor
        protected void Page_Load(object sender, EventArgs e) {
            try {
                this.VerificaSession();
                presentador = new TarifaPersonalizadaPSLPRE(this);
                if (!Page.IsPostBack) {
                    Response.AddHeader("Cache-control", "no-store, must-revalidate,private,no-cache");
                    Response.AddHeader("Pragma", "no-cache");
                    Response.AddHeader("Expires", "0");
                    presentador.Inicializar();
                    txtTarifaPersonalizadaSeguro.Text = string.Format("{0:c2}", Math.Round((decimal)(TarifaPersonalizadaTarifa * TarifaPersonalizadaPorcentajeSeguro / 100), 2));
                    this.AplicarPorcentajeDescuento();
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Inicializa el Control
        /// </summary>
        public void Inicializar() {
            //Limpiar campos
            this.txtTarifaPersonalizadaTarifa.Text = string.Empty;
            this.txtTarifaPersonalizadaTipoTarifa.Text = string.Empty;
            this.txtTarifaPersonalizadaPorcentajeDescuento.Text = string.Empty;
            this.txtTarifaPersonalizadaTurno.Text = string.Empty;
            this.txtTarifaPersonalizadaTarifaConDescuento.Text = string.Empty;
            this.txtTarifaPersonalizadaTarifaHrAdicional.Text = string.Empty;
            this.txtTarifaPersonalizadaCodigoAutorizacion.Text = string.Empty;
            this.lblTarifaPersonalizadaEtiqueta.Text = string.Empty;
            
        }
        public void EstablecerPaqueteNavegacion(string key, object value) {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(NombreClase + ".EstablecerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            Session[key] = value;
        }
        public object ObtenerPaqueteNavegacion(string key) {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(NombreClase + ".ObtenerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            return Session[key];
        }
        public void LimpiarPaqueteNavegacion(string key) {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(NombreClase + ".LimpiarPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            if (Session[key] != null)
                Session.Remove(key);
        }
        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipoMensaje">Tipo de mensaje a desplegar</param>
        /// <param name="msjDetalle">Muestra una cadena de texto que contiene los detalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipoMensaje, string msjDetalle = null) {
            string sError = string.Empty;
            if (tipoMensaje == ETipoMensajeIU.ERROR) {
                if (this.hdnMensaje == null)
                    sError += " , hdnDetalle";
                this.hdnDetalle.Value = msjDetalle;
            }
            if (this.hdnMensaje == null)
                sError += " , hdnMensaje";
            if (this.hdnTipoMensaje == null)
                sError += " , hdnTipoMensaje";
            if (sError.Length > 0)
                throw new Exception("No se pudo desplegar correctamente el error. No se encontró el control: " + sError.Substring(2) + " en la MasterPage.");

            this.hdnMensaje.Value = mensaje;
            this.hdnTipoMensaje.Value = ((int)tipoMensaje).ToString();
        }
        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        public void RegistrarScript(string key, string script) {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        /// <summary>
        /// Cerrar el Dialog
        /// </summary>
        /// <param name="valorRetorno">Valor retorno</param>
        public void CerrarDialog(string valorRetorno) {
            RegistrarScript("closeWindow", "closeParentUI('" + valorRetorno + "')");
        }

        protected void txtTarifaPersonalizadaPorcentajeDescuento_OnTextChanged(object sender, EventArgs e) {
            this.hdnTarifaAlza.Value = "0";
            this.txtTarifaPersonalizadaTarifa.Enabled = false;
            this.txtTarifaPersonalizadaTarifa.Text = this.TarifaBase.ToString();
            this.AplicarPorcentajeDescuento();
        }

        private void AplicarPorcentajeDescuento() {
            try {
                presentador.ObtieneTarifaBase();
                presentador.ValidarDescuentoPermitido();
                txtTarifaPersonalizadaSeguro.Text = (TarifaBase * TarifaPersonalizadaPorcentajeSeguro / 100).ToString();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".txtTarifaPersonalizadaPorcentajeDescuento_OnTextChanged: " + ex.Message);
            }
        }
        /// <summary>
        /// Habilita o deshabilita la opción de Validar Código de Autorización
        /// </summary>
        /// <param name="permitir">Parámetro que controla el estatus del control</param>
        public void PermitirValidarCodigoAutorizacion(bool permitir) {
            btnValidarAutorizacion.Enabled = permitir;

            txtTarifaPersonalizadaCodigoAutorizacion.Enabled = permitir;
            txtTarifaPersonalizadaCodigoAutorizacion.ReadOnly = !permitir;

        }

        /// <summary>
        /// Habilita o deshabilita la opción de Solicitar Código de Autorización
        /// </summary>
        /// <param name="permitir">Parámetro que controla el estatus del control</param>
        public void PermitirSolicitarCodigoAutorizacion(bool permitir) {

            btnSolicitarAutorizacion.Enabled = permitir;

            this.txtTarifaPersonalizadaPorcentajeDescuento.Enabled = permitir;
            this.txtTarifaPersonalizadaPorcentajeDescuento.ReadOnly = !permitir;

        }

        /// <summary>
        /// Habilita o deshabilita los controles y botones de tal forma que permita aplicar le descuento sin condigo de autorización
        /// </summary>
        /// <param name="permitir">Parámetro que controla el estatus del control</param>
        public void PermitirAplicarSinCodigoAutorizacion(bool permitir) {

            btnSolicitarAutorizacion.Enabled = !permitir;
            btnValidarAutorizacion.Enabled = permitir;

            if (!esTarifaAlza) {
                //Control de tipo textbox porcentaje de descuento
                this.txtTarifaPersonalizadaPorcentajeDescuento.Enabled = permitir;
                this.txtTarifaPersonalizadaPorcentajeDescuento.ReadOnly = !permitir;
            } else {
                this.txtTarifaPersonalizadaPorcentajeDescuento.Enabled = !permitir;
                this.txtTarifaPersonalizadaPorcentajeDescuento.ReadOnly = permitir;
            }
            //Control de tipo textbox código de autorización
            this.txtTarifaPersonalizadaCodigoAutorizacion.Enabled = !permitir;
            this.txtTarifaPersonalizadaCodigoAutorizacion.ReadOnly = permitir;

        }

        /// <summary>
        /// Establece la etiqueta que deberá mostrar el botón
        /// </summary>
        /// <param name="etiqueta">Establece el valor de la etiqueta que se deberá asignar al botón</param>
        public void EstablecerEtiquetaBoton(string etiqueta) {
            this.btnValidarAutorizacion.Text = etiqueta;
        }

 
        #endregion
        #region Eventos
        #region Personalizar Tarifas
        protected void btnValidarAutorizacion_Click(object sender, EventArgs e) {
            try {
                string mensaje = string.Empty;
                if(!esTarifaAlza)
                    mensaje = presentador.ValidarPersonalizarTarifa(txtTarifaPersonalizadaCodigoAutorizacion.Text.Trim());
                
                if (string.IsNullOrEmpty(mensaje)) {
                    presentador.ActualizarTarifa();
                    this.CerrarDialog("OK");
                } else
                    MostrarMensaje(mensaje, ETipoMensajeIU.ADVERTENCIA);
            } catch (Exception ex) {
                MostrarMensaje("Ocurrió un Error al validar el código de autorización", ETipoMensajeIU.INFORMACION);
            }
        }

        protected void btnSolicitarAutorizacion_Click(object sender, EventArgs e) {
            try {
                PermitirSolicitarCodigoAutorizacion(false);
                presentador.SolicitarAutorizacionTarifaPersonalizada();
            } catch (Exception ex) {
                MostrarMensaje("Ocurrió un error al solicitar el código de autorización", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        protected void txtTarifaPersonalizadaTarifa_TextChanged(object sender, EventArgs e) {
            try {
                this.txtTarifaPersonalizadaPorcentajeDescuento.Enabled = false;
                this.hdnTarifaAlza.Value = "1";
                presentador.ValidarAumentoTarifa();
                txtTarifaPersonalizadaSeguro.Text = string.Format("{0:c2}",  Math.Round( (decimal)(TarifaPersonalizadaTarifa * TarifaPersonalizadaPorcentajeSeguro / 100), 2) );
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".txtTarifaPersonalizadaTarifa_OnTextChanged: " + ex.Message);
            }
        }
        #endregion


        #endregion
    }
}