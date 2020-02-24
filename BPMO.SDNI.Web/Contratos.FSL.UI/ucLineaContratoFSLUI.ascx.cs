// Satisface al caso de uso CU015 - Registrar Contrato Full Service Leasing
// Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al caso de uso CU023 - Editar Contrato Full Service Leasing
// Mejoras Durante Staffing - Cobro de Rangos de Kms /Hrs
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.PRE;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.FSL.UI
{
    public partial class ucLineaContratoFSLUI : UserControl, IucLineaContratoFSLVIS
    {
        #region Atributos
        private ucLineaContratoFSLPRE presentador;
        private const string NombreClase = "ucLineaContratoFSLUI";

        public enum ECatalogoBuscador {
            ProductoServicio = 1
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Identificador de la Unidad Operativa
        /// </summary>
        public int? UnidadOperativaID
        {
            get
            {
                int? value = null;

                if (!string.IsNullOrEmpty(hdnUnidadOperativaID.Value.Trim()))
                    value = int.Parse(hdnUnidadOperativaID.Value.Trim());

                return value;
            }
            set
            {
                hdnUnidadOperativaID.Value = value != null ? value.Value.ToString("") : string.Empty;
            }
        }
        /// <summary>
        /// Identificador de la Unidad sobre la que se esta creando la linea del contrato
        /// </summary>
        public int? UnidadID
        {
            get
            {
                int? value = null;

                if (!string.IsNullOrEmpty(hdnUnidaID.Value.Trim()))
                    value = int.Parse(hdnUnidaID.Value.Trim());

                return value;
            }
            set
            {
                hdnUnidaID.Value = value != null ? value.ToString() : string.Empty;
            }
        }
        /// <summary>
        /// Identificador del Equipo de la Unidad
        /// </summary>
        public int? EquipoID
        {
            get
            {
                int? value = null;

                if (!string.IsNullOrEmpty(hdnEquipoID.Value.Trim()))
                    value = int.Parse(hdnEquipoID.Value.Trim());

                return value;
            }
            set
            {
                hdnEquipoID.Value = value != null ? value.ToString() : string.Empty;
            }
        }
        /// <summary>
        /// Presentador del User Control para uso interno
        /// </summary>
        public ucLineaContratoFSLPRE Presentador
        {
            get { return presentador; }
            set { presentador = value; }
        }
        /// <summary>
        /// Numero de Serie o codigo VIN
        /// </summary>
        public string VIN
        {
            get
            {
                return txtVIN.Text.Trim().ToUpper();
            }
            set
            {
                txtVIN.Text = value != null ? value.Trim() : string.Empty;
            }
        }
        /// <summary>
        /// Numero Economico
        /// </summary>
        public string NumeroEconocimico
        {
            get
            {
                return txtNumeroEconomico.Text.Trim().ToUpper();
            }
            set
            {
                txtNumeroEconomico.Text = value != null ? value.Trim() : string.Empty;
            }
        }
        /// <summary>
        /// Nombre del Modelo de la Unidad
        /// </summary>
        public string Modelo
        {
            get
            {
                return txtModelo.Text.Trim().ToUpper();
            }
            set
            {
                txtModelo.Text = value != null ? value.Trim() : string.Empty;
            }
        }
        /// <summary>
        /// Año del Unidad
        /// </summary>
        public int? Anio
        {
            get
            {
                int? value = null;

                if (!string.IsNullOrEmpty(txtAnio.Text.Trim()))
                    value = int.Parse(txtAnio.Text.Trim());

                return value;
            }
            set
            {
                txtAnio.Text = value != null ? value.ToString() : string.Empty;
            }
        }
        /// <summary>
        /// Peso Bruto Vehiculo Maximo Recomendado por el Fabricante
        /// </summary>
        public decimal? PBV
        {
            get
            {
                decimal? value = null;

                if (!string.IsNullOrEmpty(txtPBV.Text.Trim())) 
                    value = decimal.Parse(txtPBV.Text.Trim().Replace(",","")); //RI0012

                return value;
            }
            set
            {
				txtPBV.Text = value != null ? string.Format("{0:#,##0.0000}", value) : string.Empty; //RI012
            }
        }
        /// <summary>
        /// Peso Bruto Carga Maximo Recomendado por el Fabricante
        /// </summary>
        public decimal? PBC
        {
            get
            {
                decimal? value = null;

                if (!string.IsNullOrEmpty(txtPBC.Text.Trim()))
                    value = decimal.Parse(txtPBC.Text.Trim().Replace(",","")); //RI0012

                return value;
            }
            set
            {
				txtPBC.Text = value != null ? string.Format("{0:#,##0.0000}", value) : string.Empty; //RI0012
            } 
        }
        /// <summary>
        /// KM Inicial de la Unidad
        /// </summary>
        public int? KmInicial
        {
            get
            {
                int? value = null;

                if (!string.IsNullOrEmpty(txtKmInicial.Text.Trim()))
                    value = int.Parse(txtKmInicial.Text.Trim().Replace(",","")); //RI0012

                return value;
            }
            set
            {
				txtKmInicial.Text = value != null ? string.Format("{0:#,##0}", value) : string.Empty; //RI0012
            }
        }
        /// <summary>
        /// Kilimetraje recorrido Estimado Anual
        /// </summary>
        public int? KmEstimadoAnual
        {
            get
            {
                int? value = null;

                if (!string.IsNullOrEmpty(txtKmEstimadoAnual.Text.Trim()))
                    value = int.Parse(txtKmEstimadoAnual.Text.Trim().Replace(",","")); //RI0012

                return value;
            }
            set
            {
				txtKmEstimadoAnual.Text = value != null ? string.Format("{0:#,##0}", value) : string.Empty; //RI0012
            }
        }
        /// <summary>
        /// Monto del Deposito en Garantia
        /// </summary>
        public decimal? DepositoGarantia
        {
            get
            {
                decimal? value = null;

                if (!string.IsNullOrEmpty(txtDepositoGarantia.Text.Trim()))
                    value = decimal.Parse(txtDepositoGarantia.Text.Trim().Replace(",","")); //RI0012

                return value;
            }
            set
            {
				txtDepositoGarantia.Text = value != null ? string.Format("{0:#,##0.0000}", value) : string.Empty; //RI0012
            }
        }
        /// <summary>
        /// Comision por Apertura
        /// </summary>
        public decimal? ComisionApertura
        {
            get
            {
                decimal? value = null;

                if (!string.IsNullOrEmpty(txtComisionApertura.Text.Trim()))
                    value = decimal.Parse(txtComisionApertura.Text.Trim().Replace(",","")); //RI0012

                return value;
            }
            set
            {
				txtComisionApertura.Text = value != null ? string.Format("{0:#,##0.0000}", value) : string.Empty; //RI0012
            }
        }
        /// <summary>
        /// Cargo Fijo Mensual
        /// </summary>
        public decimal? CargoFijoMes
        {
            get
            {
                decimal? value = null;

                if (!string.IsNullOrEmpty(txtCargoFijoMes.Text.Trim()))
                    value = decimal.Parse(txtCargoFijoMes.Text.Trim().Replace(",","")); //RI0012

                return value;
            }
            set
            {
                txtCargoFijoMes.Text = value != null ?string.Format("{0:#,##0.0000}",value) : string.Empty; //RI0012
            }
        }
        /// <summary>
        /// Listado de Tipos de Cotizacion
        /// </summary>
        public List<ETipoCotizacion> ListadoTiposCotizacion
        {
            get
            {
                var Lista = new List<ETipoCotizacion>();

                var dataSource = ddlTipoCotizacion.DataSource as List<KeyValuePair<int, string>>;

                if (dataSource != null)
                {
                    Lista.AddRange(from kp in dataSource where Enum.IsDefined(typeof(ETipoCotizacion), kp.Key) select ((ETipoCotizacion)kp.Key));
                }

                return Lista;
            }
            set
            {
                var Lista = new List<KeyValuePair<int, string>>();

                if (value != null)
                    Lista.AddRange(value.Select(tipoCotizacion => new KeyValuePair<int, string>((int)tipoCotizacion, tipoCotizacion.ToString())));


                // Agregar el Item de fachada
                Lista.Insert(0, new KeyValuePair<int, string>(-1, "SELECCIONE UNA OPCION"));
                //Limpiar el DropDownList Actual
                ddlTipoCotizacion.Items.Clear();
                // Asignar Lista al DropDownList
                ddlTipoCotizacion.DataTextField = "Value";
                ddlTipoCotizacion.DataValueField = "Key";
                ddlTipoCotizacion.DataSource = Lista;
                ddlTipoCotizacion.DataBind();
            }
        }
        /// <summary>
        /// Tipo de Cotizacion Seleccionado
        /// </summary>
        public ETipoCotizacion? TipoCotizacionSeleccionada
        {
            get
            {
                ETipoCotizacion? tipoCotizacion = null;

                if (ddlTipoCotizacion.SelectedValue != "-1")
                    tipoCotizacion = ((ETipoCotizacion)int.Parse(ddlTipoCotizacion.SelectedValue));

                return tipoCotizacion;
            }
        }
        /// <summary>
        /// Cobro por Kilometros u Horas Termo
        /// </summary>
        public Int32? CobroKilometrosHoras
        {
            get
            {
                Int32? kmHoras = null;

                if(ddlKMHRS.SelectedValue != "-1")
                    kmHoras = (int.Parse(ddlTipoCotizacion.SelectedValue));

                return kmHoras;
            }
        }
        /// <summary>
        /// Plazo del contrato en Años
        /// </summary>
        public int? PlazoAnio
        {
            get
            {
                int? value = null;

                if (!string.IsNullOrEmpty(hdnPlazo.Value)) value = int.Parse(hdnPlazo.Value);

                return value;
            }
            set
            {
                hdnPlazo.Value = value != null ? value.ToString() : string.Empty;
            }
        }
        /// <summary>
        /// Si se incluye Opcion de Compra de la Unidad
        /// </summary>
        public bool OpcionCompra
        {
            get
            {
                return cbOpcionCompra.Checked;
            }
            set
            {
                cbOpcionCompra.Checked = value;
            }
        }
        /// <summary>
        /// Listado de Monedas para la opcion de compra
        /// </summary>
        public List<MonedaBO> ListadoMonedas
        {
            get
            {
                return (List<MonedaBO>)ddlMonedas.DataSource;
            }
            set
            {
                // Clonar la Lista para no afectar la lista original
                var Lista = new List<MonedaBO>(value);
                // Agregar el SucursalBO de fachada
                Lista.Insert(0, new MonedaBO { Codigo = "0", Nombre = "Seleccione una opción" });
                //Limpiar el DropDownList Actual
                ddlMonedas.Items.Clear();
                // Asignar Lista al DropDownList
                ddlMonedas.DataTextField = "Nombre";
                ddlMonedas.DataValueField = "Codigo";
                ddlMonedas.DataSource = Lista;
                ddlMonedas.DataBind();
            }
        }
        /// <summary>
        /// Tipo de Moneda Seleccionada
        /// </summary>
        public MonedaBO MonedaSeleccionada
        {
            get
            {
                MonedaBO monedaSeleccionada = null;

                if (ddlMonedas.SelectedValue != "0")
                    monedaSeleccionada = new MonedaBO { Codigo = ddlMonedas.SelectedValue };

                return monedaSeleccionada;
            }
        }
        /// <summary>
        /// Monto del Importe de Compra
        /// </summary>
        public decimal? ImporteCompra
        {
            get
            {
                decimal? value = null;

                if (!string.IsNullOrEmpty(txtImporteCompra.Text.Trim()))
                    value = decimal.Parse(txtImporteCompra.Text.Trim().Replace(",","")); //RI0012

                return value;
            }
            set
            {
				txtImporteCompra.Text = value != null ? string.Format("{0:#,##0.0000}", value) : string.Empty; //RI0012
            }
        }
        /// <summary>
        /// Listado de Equipos Aliados
        /// </summary>
        public List<EquipoAliadoBO> ListadoEquiposAliados
        {
            get
            {
                if (Session["EquiposAliados_Unidad_LineaContrato"] == null)
                    Session["EquiposAliados_Unidad_LineaContrato"] = new List<EquipoAliadoBO>();

                return (List<EquipoAliadoBO>)Session["EquiposAliados_Unidad_LineaContrato"];
            }
            set
            {
                if (value != null)
                    Session["EquiposAliados_Unidad_LineaContrato"] = value;
                else
                    Session["EquiposAliados_Unidad_LineaContrato"] = new List<EquipoAliadoBO>();
            }
        }
        /// <summary>
        /// Listado de Equipos Aliados de la Unidad a Agregar al Contrato
        /// </summary>
        public List<CargoAdicionalEquipoAliadoBO> CargosAdicionalesEquiposAliados
        {
            get
            {
                ucCargosEquiposAliados.ObtenerCargosAdicionales();

                return ucCargosEquiposAliados.ObtenerCargosAdicionales();
            }
        }
        /// <summary>
        /// Tarifas por cargos Adicionales de la Unidad
        /// </summary>
        public List<TarifaFSLBO> TarifasAdicionales
        {
            get
            {
                return ucTarifasLinea.ObtenerTarifas();
            }
        }
        /// <summary>
        /// Manejado de Eventos al hacer clic sobre el boton Agregar
        /// </summary>
        public EventHandler AgregarClick { get; set; }
        /// <summary>
        /// Manejador de Eventos al hacer clic sobre el boton Cancelar
        /// </summary>
        public EventHandler CancelarClick { get; set; }
        /// <summary>
        /// Linea de Contrato antes de Editarla
        /// </summary>
        public LineaContratoFSLBO UltimoObjeto
        {
            get { return Session["UltimaLineaContrato"] as LineaContratoFSLBO; } 
            set { Session["UltimaLineaContrato"] = value; }
        }
        /// <summary>
        /// Numero de Poliza de Seguro de la Unidad
        /// </summary>
        public string NumeroPoliza
        {
            set { txtNumeroPoliza.Text = value; }
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

        #region Buscador
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
        public ucLineaContratoFSLUI.ECatalogoBuscador ViewState_Catalogo {
            get {
                return (ucLineaContratoFSLUI.ECatalogoBuscador)ViewState["BUSQUEDA"];
            }
            set {
                ViewState["BUSQUEDA"] = value;
            }
        }
        #endregion Buscador
        #endregion

        #region Metodos
        /// <summary>
        /// Muestra los mensajes en la UI
        /// </summary>
        /// <param name="mensaje">Mensaje a Mostrar</param>
        /// <param name="tipo">Tipo de Mensaje</param>
        /// <param name="detalle">Detalle o sub mensaje</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if(master != null)
                master.MostrarMensaje(mensaje, tipo, detalle);
            }
            else
            {
                if(master != null)
                master.MostrarMensaje(mensaje, tipo);
            }
        }
        /// <summary>
        /// Habilita o deshabilita las opciones de compra
        /// </summary>
        /// <param name="habilitar"></param>
        public void HabilitarCompra(bool habilitar)
        {
            txtImporteCompra.Enabled = habilitar;
            
            ddlMonedas.Enabled = habilitar;

            txtImporteCompra.Text = string.Empty;
            ddlMonedas.SelectedIndex = 0;
        }
        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        public void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }
        /// <summary>
        /// Limpia la sesion del control
        /// </summary>
        public void LimpiarSesion()
        {
            Session.Remove("EquiposAliados_Unidad_LineaContrato");
            Session.Remove("TarifasAdicionalesLinea");
            ucCargosEquiposAliados.LimpiarSesion();
        }
        /// <summary>
        /// Limpia la Session del User Control de Tarifas
        /// </summary>
        public void LimpiarSesionTarifas()
        {
            ucTarifasLinea.LimpiarSesion();
        }
        /// <summary>
        /// Inicializa el Control
        /// </summary>
        public void Inicializar()
        {
            ddlTipoCotizacion.SelectedIndex = 0;
            ddlKMHRS.SelectedIndex = 0;
            ddlKMHRS.Enabled = false;
            ucTarifasLinea.Inicializar(PlazoAnio, null, ETipoEquipo.Unidad, new List<TarifaFSLBO>(), EquipoID, false);
            ucCargosEquiposAliados.LimpiarSesion();
            ucCargosEquiposAliados.InicializarControl(0,null, new List<EquipoAliadoBO>(), 0, 0);
        }
        /// <summary>
        /// Establece la Moneda de Compra Seleccionada previamente
        /// </summary>
        /// <param name="moneda"></param>
        public void EstablecerMonedaCompra(MonedaBO moneda)
        {
            if (moneda != null && moneda.Codigo != null && moneda.Codigo.Trim().Length > 0)
                ddlMonedas.SelectedValue = moneda.Codigo;
            else
            ddlMonedas.SelectedIndex = 0;
        }
        /// <summary>
        /// Establece el Tipo de Codizacion Seleccionado previamente
        /// </summary>
        /// <param name="tipo"></param>
        public void EstablecerTipoCotizacion(ETipoCotizacion? tipo)
        {
            if (tipo != null)
                ddlTipoCotizacion.SelectedValue = Convert.ToInt32(tipo).ToString(CultureInfo.InvariantCulture);
            else
                ddlTipoCotizacion.SelectedIndex = 0;
        }
        /// <summary>
        /// Establece el valor de seleccion de cobro por km o por horas
        /// </summary>
        /// <param name="valorIndice">Indice del dropdown</param>
        public void EstablecerKmsHrs(Int32? valorIndice)
        {
            if (valorIndice != null)
                this.ddlKMHRS.SelectedValue = Convert.ToInt32(valorIndice).ToString(CultureInfo.InvariantCulture);
            else
                this.ddlKMHRS.SelectedIndex = 0;

            if (this.ddlTipoCotizacion.SelectedIndex != 0 && this.ddlTipoCotizacion.Enabled)
            {
                this.ddlKMHRS.Enabled = true;
            }
        }
        /// <summary>
        /// Establece las tarifas de renta de la unidad
        /// </summary>
        /// <param name="tarifas"></param>
        public void EstablecerTarifas(List<TarifaFSLBO> tarifas, bool? cargoPorKm)
        {
            ucTarifasLinea.Inicializar(PlazoAnio,((CargosAdicionalesFSLBO)UltimoObjeto.Cobrable).TipoCotizacion, ETipoEquipo.Unidad, tarifas, EquipoID, false);
            if (cargoPorKm != null)
                ucTarifasLinea.CargoKm = cargoPorKm;
            if(!ucTarifasLinea.EsModoConsulta.Value)
                ucTarifasLinea.PermitirAnio(true);
        }
        /// <summary>
        /// Establece los cargos de Equipos Aliados de la Unidad
        /// </summary>
        /// <param name="cargos"></param>
        public void EstablecerCargosAdicionalesEquiposAliados(List<CargoAdicionalEquipoAliadoBO> cargos)
        {
            ucCargosEquiposAliados.InicializarControl(PlazoAnio, TipoCotizacionSeleccionada, cargos, UnidadID, UnidadOperativaID);
            ucCargosEquiposAliados.EstablecerCargosAdicionales(cargos);            
        }
        /// <summary>
        /// Configura la interfaz en modo Consultar
        /// </summary>
        public void ConfigurarModoConsultar()
        {
            cbOpcionCompra.Enabled = false;
            ddlMonedas.Enabled = false;
            ddlTipoCotizacion.Enabled = false;
            btnAgregar.Enabled = false;              
            lblTitulo.Text = "UNIDAD EN RENTA";

            txtAnio.Enabled = false;
            txtCargoFijoMes.Enabled = false;
            txtComisionApertura.Enabled = false;
            txtDepositoGarantia.Enabled = false;
            txtImporteCompra.Enabled = false;
            txtKmEstimadoAnual.Enabled = false;
            txtKmInicial.Enabled = false;
            txtModelo.Enabled = false;
            txtNumeroEconomico.Enabled = false;
            txtPBC.Enabled = false;
            txtPBV.Enabled = false;
            txtVIN.Enabled = false;
            this.txtClaveProductoServicio.Enabled = false;
            this.ibtnBuscarProductoServicio.Visible = false;
            ucCargosEquiposAliados.ConfigurarModoConsultar();
            //ucTarifasLinea.DesHabilitarCampos();
            ucTarifasLinea.EstablecerModoConsulta(true);

            txtNumeroPoliza.Enabled = false;
            txtNumeroPoliza.Visible = true;
            lblPolizaSeguro.Visible = true;
        }
        /// <summary>
        /// Configura la interfaz en modo Editar
        /// </summary>
        public void ConfigurarModoEditar()
        {
            cbOpcionCompra.Enabled = true;
            ddlMonedas.Enabled = false;
            ddlTipoCotizacion.Enabled = true;
            ddlKMHRS.Enabled = true;
            btnAgregar.Enabled = true;
            lblTitulo.Text = "UNIDAD A RENTAR";

            txtAnio.Enabled = false;
            txtCargoFijoMes.Enabled = true;
            txtComisionApertura.Enabled = true;
            txtDepositoGarantia.Enabled = true;
            txtImporteCompra.Enabled = false;
            txtKmEstimadoAnual.Enabled = true;
            txtKmInicial.Enabled = false;
            txtModelo.Enabled = false;
            txtNumeroEconomico.Enabled = false;
            txtPBC.Enabled = false;
            txtPBV.Enabled = false;
            txtVIN.Enabled = false;
            this.txtClaveProductoServicio.Enabled = true;
            this.ibtnBuscarProductoServicio.Visible = true;
            ucTarifasLinea.EstablecerModoEditar(ETipoEquipo.Unidad);
            ucCargosEquiposAliados.ConfigurarModoEditar();

            txtNumeroPoliza.Enabled = false;
            txtNumeroPoliza.Visible = false;
            lblPolizaSeguro.Visible = false;
        }

        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ucLineaContratoFSLPRE(this);
                if (!Page.IsPostBack)
                {
                    Presentador.Inicializar();
                    RegistrarScript("BuscadorLineaContrato", 
                        "function "+ClientID+"_Buscar(guid, xml){ var width = ObtenerAnchoBuscador(xml); $.BuscadorWeb({ xml:xml,guid:guid,btnSender:$(\"#"+this.btnResult.ClientID+"\"),features:{dialogWidth:width,dialogHeight:'280px'}});}");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".Page_Load: " + ex.Message);
            }
        }
        protected void ddlTipoCotizacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //ucTarifasLinea.InicializarControl(PlazoAnio, TipoCotizacionSeleccionada, true);
                ucTarifasLinea.Inicializar(PlazoAnio, TipoCotizacionSeleccionada, ETipoEquipo.Unidad, new List<TarifaFSLBO>(), EquipoID, false);
                ucCargosEquiposAliados.LimpiarSesion();
                ucCargosEquiposAliados.InicializarControl(PlazoAnio, TipoCotizacionSeleccionada, ListadoEquiposAliados, UnidadID, UnidadOperativaID);
                
                ddlKMHRS.SelectedValue = "-1";
                ddlKMHRS.Enabled = ddlTipoCotizacion.SelectedValue != "-1";
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".ddlTipoCotizacion_SelectedIndexChanged: " + ex.Message);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                ucTarifasLinea.InicializarControl(0, null, true);
                this.LimpiarSesion(); 
                ucCargosEquiposAliados.InicializarControl(0, null, new List<EquipoAliadoBO>(), 0, 0);
                presentador.Inicializar();
                if (CancelarClick != null) CancelarClick.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".btnCancelar_Click: " + ex.Message);
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            string resultado = presentador.ValidarDatos();

            if (!string.IsNullOrEmpty(resultado))
            {
                MostrarMensaje(resultado, ETipoMensajeIU.INFORMACION);
                return;
            }

            try
            {
                if (AgregarClick != null) AgregarClick.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".btnAgregar_Click: " + ex.Message);
            }
        }
        protected void cbOpcionCompra_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                HabilitarCompra(cbOpcionCompra.Checked);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al seleccionar la opción de compra.", ETipoMensajeIU.ERROR, NombreClase + ".cbOpcionCompra_CheckedChanged:" + ex.Message);
            }
        }
        protected void ddlKMHRS_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ddlKMHRS.SelectedValue != "-1")
                {
                    ucTarifasLinea.Inicializar(PlazoAnio, TipoCotizacionSeleccionada, ETipoEquipo.Unidad, new List<TarifaFSLBO>(), EquipoID, false);
                    ucTarifasLinea.CargoKm = this.ddlKMHRS.SelectedValue == "0";
                    ucTarifasLinea.PermitirAnio(true);
                }
                else
                {
                    ucTarifasLinea.Inicializar(PlazoAnio, TipoCotizacionSeleccionada, ETipoEquipo.Unidad, new List<TarifaFSLBO>(), EquipoID, false);
                    ucTarifasLinea.CargoKm = null;
                    ucTarifasLinea.PermitirAnio(false);
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".ddlKMHRS_OnSelectedIndexChanged: " + ex.Message);
            }
        }

        #region Buscador
        protected void txtClaveProductoServicio_TextChanged(object sender, EventArgs e) {
            try {
                string clvProducto = this.ClaveProductoServicio;
                Session_BOSelecto = null;
                DesplegarBOSelecto(ucLineaContratoFSLUI.ECatalogoBuscador.ProductoServicio);

                this.ClaveProductoServicio = clvProducto;
                if (clvProducto != null)
                    EjecutaBuscador("ProductoServicio", ucLineaContratoFSLUI.ECatalogoBuscador.ProductoServicio);

                this.ProductoServicioId = null;
                this.ClaveProductoServicio = null;
                this.DescripcionProductoServicio = null;
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar un Producto", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".txtClaveProductoServicio: " + ex.Message);
            }
        }

        protected void ibtnBuscarProductoServicio_Click(object sender, ImageClickEventArgs e) {
            try {
                EjecutaBuscador("ProductoServicio&hidden=0", ucLineaContratoFSLUI.ECatalogoBuscador.ProductoServicio);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar el producto", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".ibtnBuscarProductoServicio_Click: " + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e) {
            try {
                switch (ViewState_Catalogo) {
                    case ECatalogoBuscador.ProductoServicio:

                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".btnResult_Click: " + ex.Message);
            }
        }

        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="catalogoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda) {
            ViewState_Catalogo = catalogoBusqueda;
            Session_ObjetoBuscador = Presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            Session_BOSelecto = null;
            RegistrarScript("Events", ClientID + "_Buscar('" + ViewState_Guid + "','" + catalogo + "');");
        }
        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="catalogo">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo) {
            Presentador.DesplegarResultadoBuscador(catalogo.ToString(), Session_BOSelecto);
            Session_BOSelecto = null;
        }
        #endregion

        

        #endregion
    }
}
