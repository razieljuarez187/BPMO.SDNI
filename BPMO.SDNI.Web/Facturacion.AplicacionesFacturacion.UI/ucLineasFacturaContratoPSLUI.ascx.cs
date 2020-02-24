using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.eFacturacion.Procesos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI {
    /// <summary>
    /// Control que visualiza la sección de líneas de conceptos para una factura
    /// </summary>
    public partial class ucLineasFacturaContratoPSLUI : System.Web.UI.UserControl, IucLineasFacturaContratoPSLVIS {
        #region Atributos
        /// <summary>
        /// Nombre de la clase en la cual estamos trabajando
        /// </summary>
        private string nombreClase = "ucLineasFacturaContratoPSLUI";

        /// <summary>
        /// Presentador asociado a la vista
        /// </summary>
        private ucLineasFacturaContratoPSLPRE presentador;

        /// <summary>
        /// numerador que contiene la línea actual en curso
        /// </summary>
        public int numeroLinea = 0;

        /// <summary>
        /// Enumeración para controlar los objetos del buscador
        /// </summary>
        public enum ECatalogoBuscador {
            UsoCFDI
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene o establece las líneas de factura
        /// </summary>
        public int? Lineas {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtLineas.Text))
                    id = int.Parse(this.txtLineas.Text.Trim());
                return id;
            }
            set {
                if (value != null)
                    this.txtLineas.Text = value.ToString();
                else
                    this.txtLineas.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el subtotal
        /// </summary>
        public decimal? SubTotal {
            get {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtSubTotal.Text))
                    temp = Decimal.Parse(this.txtSubTotal.Text.Trim().Replace(",", ""));
                return temp;
            }
            set {
                if (value != null)
                    this.txtSubTotal.Text = string.Format("{0:#,##0.00}", value);
                else
                    this.txtSubTotal.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el Impuesto
        /// </summary>
        public decimal? Impuesto {
            get {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtImpuesto.Text))
                    temp = Decimal.Parse(this.txtImpuesto.Text.Trim().Replace(",", ""));
                return temp;
            }
            set {
                if (value != null)
                    this.txtImpuesto.Text = string.Format("{0:#,##0.00}", value);
                else
                    this.txtImpuesto.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el total de la factura 
        /// </summary>
        public decimal? TotalFactura {
            get {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtTotalFactura.Text))
                    temp = Decimal.Parse(this.txtTotalFactura.Text.Trim().Replace(",", ""));
                return temp;
            }
            set {
                if (value != null)
                    this.txtTotalFactura.Text = string.Format("{0:#,##0.00}", value);
                else
                    this.txtTotalFactura.Text = string.Empty;
            }
        }

        /// <summary>
        /// Identificador de Uso CFDI (SAT)
        /// </summary>
        public int? UsoCFDIId {
            get { return (string.IsNullOrEmpty(this.hdnUsoCFDIId.Value)) ? null : (int?)int.Parse(this.hdnUsoCFDIId.Value); }
            set { this.hdnUsoCFDIId.Value = (value != null) ? value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Clave de Uso CFDI (SAT)
        /// </summary>
        public string ClaveUsoCFDI {
            get { return (string.IsNullOrEmpty(this.txtClaveUsoCFDI.Text)) ? null : this.txtClaveUsoCFDI.Text.Trim().ToUpper(); }
            set { this.txtClaveUsoCFDI.Text = (value != null) ? value.ToUpper() : string.Empty; }
        }
        /// <summary>
        /// Descripción UsoCFDI
        /// </summary>
        public string DescripcionUsoCFDI {
            get { return (string.IsNullOrEmpty(this.txtDescripcionUsoCFDI.Text)) ? null : this.txtDescripcionUsoCFDI.Text.Trim().ToUpper(); }
            set { this.txtDescripcionUsoCFDI.Text = (value != null) ? value.ToUpper() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece un valor que determina si el control se encuentra activo
        /// </summary>
        public bool Enabled {
            set {
                this.txtClaveUsoCFDI.Enabled = value;
                this.txtClaveUsoCFDI.ReadOnly = !value;
                this.ibtnBuscarUsoCFDI.Visible = value;
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
            get {
                return (ECatalogoBuscador)ViewState["BUSQUEDA"];
            }
            set {
                ViewState["BUSQUEDA"] = value;
            }
        }
        #endregion
        #endregion

        #region Métodos
        /// <summary>
        /// Proceso que se ejecuta cuando se crea una nueva factura
        /// </summary>
        public void PrepararNuevo() {
            this.rptLineas.DataSource = null;
            this.rptLineas.DataBind();

            this.SubTotal = 0M;
            this.Impuesto = 0M;
            this.TotalFactura = 0M;
            this.ClaveUsoCFDI = string.Empty;
            this.DescripcionUsoCFDI = string.Empty;
            this.UsoCFDIId = null;
        }

        /// <summary>
        /// Vlsualiza la lista de lineas de la factura en cusro
        /// </summary>
        /// <param name="detalles">Lista de objeto de detalle de linea de transacción</param>
        public void MostrarLineasContrato(IList<DetalleTransaccionBO> detalles) //LineasFacturaModel
        {
            if (detalles != null) {
                this.rptLineas.DataSource = detalles.Count > 0 ? detalles : null;
                this.Lineas = detalles.Count;
            } else {
                this.rptLineas.DataSource = null;
                this.Lineas = 0;
            }

            this.rptLineas.DataBind();
        }

        /// <summary>
        /// Visualiza los total de la transacción en curso
        /// </summary>
        /// <param name="transaccion">Transacción en curso</param>
        public void MostrarTotales(TransaccionBO transaccion) {
            this.SubTotal = transaccion.Subtotal;
            this.Impuesto = transaccion.Impuestos;
            this.TotalFactura = transaccion.TotalFactura;
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
            this.RegistrarScript("Events", "BtnBuscarLineas('" + ViewState_Guid + "','" + catalogo + "');");
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
        /// <summary>
        /// Método delegado para el evento de carga de la página
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void Page_Load(object sender, EventArgs e) {
            try {
                this.presentador = new ucLineasFacturaContratoPSLPRE(this);

                RegistrarScript("BuscadorLineaContrato",
                    @"function BtnBuscarLineas(guid, xml) {
                        var width = ObtenerAnchoBuscador(xml);

                        $.BuscadorWeb({
                            xml: xml,
                            guid: guid,
                            btnSender:$(""#" + this.btnResultLineas.ClientID + @"""),
                            features: {
                                dialogWidth: width,
                                dialogHeight: '320px',
                                center: 'yes',
                                maximize: '0',
                                minimize: 'no'
                            }
                        });
                    }");

            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }
        }

        /// <summary>
        /// Método delegado para el evento de bindeo de datos con el repeater
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void rptLineas_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e) {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem) {
                DetalleTransaccionBO detalleTransaccion = e.Item.DataItem as DetalleTransaccionBO;

                Label lblNoLinea = (Label)e.Item.FindControl("lblNoLinea");
                lblNoLinea.Text = String.Format("LÍNEA #{0}", ++this.numeroLinea);

                if (detalleTransaccion.TipoRenglon != null) {
                    TextBox txtTipoRenglon = (TextBox)e.Item.FindControl("txtTipoRenglon");
                    txtTipoRenglon.Text = detalleTransaccion.TipoRenglon.ToString();
                }
            }
        }

        /// <summary>
        /// Método para el evento TextChanged de txtClaveUsoCFDI
        /// </summary>
        /// <param name="sender">Objeto que desencadena el evento</param>
        /// <param name="e">Argumento asociado al evento</param>
        protected void txtClaveUsoCFDI_TextChanged(object sender, EventArgs e) {
            try {
                string clvUso = this.ClaveUsoCFDI;
                Session_BOSelecto = null;
                DesplegarBOSelecto(ECatalogoBuscador.UsoCFDI);

                this.ClaveUsoCFDI = clvUso;
                if (!String.IsNullOrWhiteSpace(clvUso))
                    EjecutaBuscador("UsoCFDI", ECatalogoBuscador.UsoCFDI);
                else {
                    this.UsoCFDIId = null;
                    this.DescripcionUsoCFDI = null;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar un Uso de CFDI", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtClaveUsoCFDI: " + ex.Message);
            }
        }

        /// <summary>
        /// Método para el evento click de ibtnBuscarUsoCFDI
        /// </summary>
        /// <param name="sender">Objeto que desencadena el evento</param>
        /// <param name="e">Argumento asociado al evento</param>
        protected void ibtnBuscarUsoCFDI_Click(object sender, ImageClickEventArgs e) {
            try {
                EjecutaBuscador("UsoCFDI&hidden=0", ECatalogoBuscador.UsoCFDI);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar el Uso de CFDI", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarUsoCFDI_Click: " + ex.Message);
            }
        }

        /// <summary>
        /// Método para el evento clic de btnResult
        /// </summary>
        /// <param name="sender">Objeto que desencadena el evento</param>
        /// <param name="e">Argumento asociado al evento</param>
        protected void btnResultLineas_Click(object sender, EventArgs e) {
            try {
                switch (ViewState_Catalogo) {
                    case ECatalogoBuscador.UsoCFDI:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResultLineas_Click: " + ex.Message);
            }
        }
        #endregion
    }
}