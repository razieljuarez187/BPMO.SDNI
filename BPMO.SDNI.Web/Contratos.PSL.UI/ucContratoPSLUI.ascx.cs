using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ucContratoPSLUI : System.Web.UI.UserControl, IucContratoPSLVIS {
        #region Atributos
        /// <summary>
        /// Nombre de la clase en la cual estamos trabajando
        /// </summary>
        private const string nombreClase = "ucContratoPSLUI";
        /// <summary>
        /// Enumeración para controlar los objetos del buscador
        /// </summary>
        public enum ECatalogoBuscador {
            CuentaClienteIdealease,
            DireccionCliente,
            Unidad,
            ProductoServicio
        }
        /// <summary>
        /// presentador del UC de información general del contrato de renta PSL
        /// </summary>
        private ucContratoPSLPRE presentador;
        #endregion

        #region Propiedades
        /// <summary>
        /// Manejador de Evento para ver el Detalle de la Línea de Contrato
        /// </summary>
        internal CommandEventHandler VerDetalleLineaContrato { get; set; }
        /// <summary>
        /// Manejador de Evento para remover una línea de contrato
        /// </summary>
        internal EventHandler RemoverLineaContrato { get; set; }
        /// <summary>
        /// Manejador de Evento para agregar una nueva unidad al contrato
        /// </summary>
        internal EventHandler AgregarUnidadClick { get; set; }
        /// <summary>
        /// Manejador de Evento para eliminar las tarifas de las líneas
        /// </summary>
        internal EventHandler EliminarTarifaLineasClick { get; set; }
        /// <summary>
        /// Manejador de Evento para imprimir el chek list de entrega o recepción
        /// </summary>
        internal CommandEventHandler ImprimirChkEntregaRecepcion { get; set; }

        /// <summary>
        /// Identificador del contrato
        /// </summary>
        public int? ContratoID {
            get {
                if (!string.IsNullOrEmpty(this.hdnContratoID.Value) && !string.IsNullOrWhiteSpace(this.hdnContratoID.Value)) {
                    int val = 0;

                    return Int32.TryParse(this.hdnContratoID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnContratoID.Value = value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture).Trim().ToUpper() : string.Empty; }
        }
        /// <summary>
        /// Usuario que se ha identificado en el Sistema
        /// </summary>
        public int? UsuarioID {
            get {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id.Value;
                return null;
            }
        }
        /// <summary>
        /// Obtiene o establece el estatus del contrato
        /// </summary>
        public int? EstatusID {
            get {
                if (!string.IsNullOrEmpty(this.hdnEstatusContratoID.Value) && !string.IsNullOrWhiteSpace(this.hdnEstatusContratoID.Value)) {
                    int val;
                    return Int32.TryParse(this.hdnEstatusContratoID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnEstatusContratoID.Value = value.HasValue ? value.Value.ToString().Trim() : string.Empty; }
        }
        /// <summary>
        /// Identificador del Modulo desde el cual se esta accediendo
        /// </summary>
        public int? ModuloID {
            get {
                return ((Site)Page.Master).ModuloID;
            }
        }
        /// <summary>
        /// Fecha en la que se ejecuta el contrato
        /// </summary>
        public DateTime? FechaContrato {
            get {
                if (!string.IsNullOrEmpty(this.txtFechaContrato.Text) && !string.IsNullOrWhiteSpace(this.txtFechaContrato.Text)) {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.txtFechaContrato.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtFechaContrato.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el número del contrato
        /// </summary>
        public string NumeroContrato {
            get {
                return !string.IsNullOrEmpty(this.hdnNumeroContrato.Value) && !string.IsNullOrWhiteSpace(this.hdnNumeroContrato.Value)
                    ? this.hdnNumeroContrato.Value.Trim().ToUpper()
                    : string.Empty;
            }
            set { this.hdnNumeroContrato.Value = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value) ? value.Trim().ToUpper() : string.Empty; }
        }
        /// <summary>
        /// Obtiene la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary> 
        public int? UnidadOperativaID {
            get {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
            }
        }

        public UnidadOperativaBO UnidadOperativa {
            get {
                Site master = (Site)Page.Master;
                return master != null && master.Adscripcion != null && master.Adscripcion.UnidadOperativa != null
                        ? master.Adscripcion.UnidadOperativa : null;
            }
        }

        public string Session_DomicilioEmpresa {
            get {
                return !string.IsNullOrWhiteSpace((string)this.Session["DirEmpresaRO"]) ? (string)this.Session["DirEmpresaRO"] : string.Empty;
            }
            set {
                if (string.IsNullOrWhiteSpace(value))
                    this.Session.Remove("DirEmpresaRO");
                else
                    this.Session["DirEmpresaRO"] = value.Trim().ToUpper();
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
                        Id = int.Parse(ddlSucursales.SelectedValue), Nombre = ddlSucursales.SelectedItem.Text,
                        UnidadOperativa = new UnidadOperativaBO() { Id = this.UnidadOperativaID.Value }
                    };
                }

                return sucursalSeleccionada;
            }
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
                }
            }
        }
        /// Obtiene o estable la lista de Monedas de la Interfaz
        /// </summary>
        public List<MonedaBO> ListaMonedas {
            get {
                if (Session["ListaMonedasRO"] != null)
                    return Session["ListaMonedasRO"] as List<MonedaBO>;

                return null;
            }
            set {
                if (value != null && value.Any()) {
                    Session["ListaMonedasRO"] = value;
                } else {
                    Session.Remove("ListaMonedasRO");
                }
            }
        }
        /// <summary>
        /// Porcentaje de Interés
        /// </summary>
        public decimal? TasaInteres {
            get {
                decimal val;
                return !string.IsNullOrEmpty(this.txtTasaInteres.Text) 
                    ?(decimal.TryParse(this.txtTasaInteres.Text, out val) ? (decimal?)val : null) : null;
            }
            set {
                if (value != null)
                    this.txtTasaInteres.Text = value.ToString();
                else
                    this.txtTasaInteres.Text = "";

            }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la empresa arrendadora
        /// </summary>
        public string NombreEmpresa {
            get {
                return (String.IsNullOrEmpty(txtEmpresa.Text)) ? null : this.txtEmpresa.Text.Trim().ToUpper(); //RI0061
            }
            set {
                this.txtEmpresa.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                           ? value.Trim().ToUpper()
                                           : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la dirección de la empresa arrendadora
        /// </summary>
        public string DomicilioEmpresa {
            get {
                return (String.IsNullOrEmpty(this.txtDireccionEmpresa.Text)) ? null : this.txtDireccionEmpresa.Text.Trim().ToUpper(); //RI0061
            }
            set {
                this.txtDireccionEmpresa.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                    ? value.Trim().ToUpper()
                                                    : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el representante de la empresa arrendadora
        /// </summary>
        public string RepresentanteEmpresa {
            get {
                return (String.IsNullOrEmpty(this.txtRepresentante.Text)) ? null : this.txtRepresentante.Text.Trim().ToUpper(); //RI0061
            }
            set {
                this.txtRepresentante.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
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
        /// Obtiene o establece si la cuenta del cliente es física o Moral [true = fisica | false=moral]
        /// </summary>
        public bool? ClienteEsFisica {
            get {
                if (!string.IsNullOrEmpty(this.hdnClienteEsFisico.Value) && !string.IsNullOrWhiteSpace(this.hdnClienteEsFisico.Value)) {
                    bool val = false;
                    return Boolean.TryParse(this.hdnClienteEsFisico.Value, out val) ? (bool?)val : null;
                }
                return null;
            }
            set { this.hdnClienteEsFisico.Value = value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture).Trim().ToLower() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el número de cuenta del cliente
        /// </summary>
        public int? CuentaClienteID {
            get {
                if (!string.IsNullOrEmpty(this.txtNumeroCuentaCliente.Text) && !string.IsNullOrWhiteSpace(this.txtNumeroCuentaCliente.Text)) {
                    int val = 0;
                    return Int32.TryParse(this.txtNumeroCuentaCliente.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set {
                this.txtNumeroCuentaCliente.Text = value.HasValue
                                                       ? value.Value.ToString(CultureInfo.InvariantCulture).Trim().ToUpper()
                                                       : string.Empty;
            }
        }
        /// <summary>
        /// Número de Cuenta de Oracle CuentaClienteBO.Numero
        /// </summary>
        public String CuentaClienteNumeroCuenta {
            get { return String.IsNullOrEmpty(this.txtNumeroCuentaOracle.Text) ? null : this.txtNumeroCuentaOracle.Text; }
            set { this.txtNumeroCuentaOracle.Text = value ?? String.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el nombre del cliente
        /// </summary>
        public string CuentaClienteNombre {
            get {
                return (String.IsNullOrEmpty(txtNombreCliente.Text)) ? null : this.txtNombreCliente.Text.Trim().ToUpper(); //RI0061
            }
            set {
                this.txtNombreCliente.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el tipo de cuenta del cliente
        /// </summary>
        public int? CuentaClienteTipoID {
            get {
                if (!string.IsNullOrEmpty(this.hdnTipoCuentaRegion.Value) && !string.IsNullOrWhiteSpace(this.hdnTipoCuentaRegion.Value)) {
                    int val = 0;
                    return Int32.TryParse(this.hdnTipoCuentaRegion.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnTipoCuentaRegion.Value = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty; }
        }
        /// <summary>
        /// Obtiene  o establece el RFC del cliente
        /// </summary>
        public string ClienteRFC {
            get {
                return (String.IsNullOrEmpty(txtRFCCliente.Text)) ? null : this.txtRFCCliente.Text.Trim().ToUpper(); //RI0061
            }
            set {
                this.txtRFCCliente.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                              ? value.Trim().ToUpper()
                                              : string.Empty;
            }
        }
        /// <summary>
        /// Id de la Dirección del Cliente
        /// </summary>
        public int? ClienteDireccionClienteID {
            get {
                if (!string.IsNullOrEmpty(this.hdnDireccionId.Value) && !string.IsNullOrWhiteSpace(this.hdnDireccionId.Value)) {
                    int val = 0;
                    return Int32.TryParse(this.hdnDireccionId.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnDireccionId.Value = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty; }
        }
        /// <summary>
        /// Dirección completa del cliente
        /// </summary>
        public string ClienteDireccionCompleta {
            get {
                return (String.IsNullOrEmpty(txtDomicilioCliente.Text)) ? null : this.txtDomicilioCliente.Text.Trim().ToUpper(); //RI0061
            }
            set {
                this.txtDomicilioCliente.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                    ? value.Trim().ToUpper()
                                                    : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la calle del domicilio del cliente
        /// </summary>
        public string ClienteDireccionCalle {
            get { return this.hdnCalle.Value.Trim().ToUpper(); }
            set {
                this.hdnCalle.Value = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                          ? value.Trim().ToUpper()
                                          : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el código postal del domicilio del cliente
        /// </summary>
        public string ClienteDireccionCodigoPostal {
            get {
                return (String.IsNullOrEmpty(txtCodigoPostal.Text)) ? null : this.txtCodigoPostal.Text.Trim().ToUpper(); //RI0061
            }
            set {
                this.txtCodigoPostal.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la ciudad del domicilio del cliente
        /// </summary>
        public string ClienteDireccionCiudad {
            get { return this.hdnCiudad.Value.Trim().ToUpper(); }
            set {
                this.hdnCiudad.Value = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el estado de la dirección del cliente
        /// </summary>
        public string ClienteDireccionEstado {
            get { return this.hdnEstado.Value.Trim().ToUpper(); }
            set {
                this.hdnEstado.Value = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el municipio de la dirección del cliente
        /// </summary>
        public string ClienteDireccionMunicipio {
            get { return this.hdnMunicipio.Value.Trim().ToUpper(); }
            set {
                this.hdnMunicipio.Value = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el país de la dirección del cliente
        /// </summary>
        public string ClienteDireccionPais {
            get { return this.hdnPais.Value.Trim().ToUpper(); }
            set {
                this.hdnPais.Value = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la colonia de la dirección del cliente
        /// </summary>
        public string ClienteDireccionColonia {
            get { return this.hdnColonia.Value.Trim().ToUpper(); }
            set {
                this.hdnColonia.Value = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el listado de representantes legales del cliente seleccionado
        /// </summary>
        public List<RepresentanteLegalBO> RepresentantesTotales {
            get {
                if (Session["ListadoRepresentantesLegales"] != null)
                    return (List<RepresentanteLegalBO>)Session["ListadoRepresentantesLegales"];

                return new List<RepresentanteLegalBO>();
            }
            set {
                List<RepresentanteLegalBO> listValue = value ?? new List<RepresentanteLegalBO>();

                // Clonar la Lista para no afectar la lista original
                var lista = new List<RepresentanteLegalBO>(listValue);
                Session["ListadoRepresentantesLegales"] = new List<RepresentanteLegalBO>(listValue);
                // Agregar el SucursalBO de fachada
                lista.Insert(0, new RepresentanteLegalBO { Id = 0, Nombre = "Seleccione una opción" });
                //Limpiar el DropDownList Actual
                this.ddlRepresentantesLegales.Items.Clear();
                // Asignar Lista al DropDownList
                ddlRepresentantesLegales.DataTextField = "Nombre";
                ddlRepresentantesLegales.DataValueField = "Id";
                ddlRepresentantesLegales.DataSource = lista;
                ddlRepresentantesLegales.DataBind();
            }
        }
        /// <summary>
        /// Obtiene o establece los representantes legales del cliente que han sido seleccionados
        /// </summary>
        public List<RepresentanteLegalBO> RepresentantesSeleccionados {
            get {
                List<RepresentanteLegalBO> listValue;
                listValue = Session["RepresentantesLegalesContrato"] != null
                                ? (List<RepresentanteLegalBO>)Session["RepresentantesLegalesContrato"]
                                : new List<RepresentanteLegalBO>();

                return listValue;
            }
            set {
                List<RepresentanteLegalBO> listValue = value ?? new List<RepresentanteLegalBO>();
                Session["RepresentantesLegalesContrato"] = listValue;
            }
        }
        /// <summary>
        /// Obtiene el identificador del representante legal seleccionado
        /// </summary>
        public int? RepresentanteLegalSeleccionadoID {
            get {
                if (!string.IsNullOrEmpty(this.ddlRepresentantesLegales.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlRepresentantesLegales.SelectedValue)) {
                    if (System.String.Compare(this.ddlRepresentantesLegales.SelectedValue, "0", System.StringComparison.Ordinal) != 0) {
                        int val = 0;
                        return Int32.TryParse(this.ddlRepresentantesLegales.SelectedValue, out val) ? (int?)val : null;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// Código de la moneda seleccionada
        /// </summary>
        public string CodigoMoneda {
            get {
                if (this.ddlMonedas.SelectedValue.CompareTo("0") != 0)
                    return this.ddlMonedas.SelectedValue.Trim().ToUpper();
                return null;
            }
            set {
                ddlMonedas.SelectedValue = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                               ? value.Trim().ToUpper()
                                               : "0";
            }
        }
        /// <summary>
        /// Obtiene o establece el tipo de autorización para el pago a crédito
        /// </summary>
        public int? TipoConfirmacionID {
            get {
                if (!string.IsNullOrEmpty(this.ddlTipoPagoCredito.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlTipoPagoCredito.SelectedValue)) {
                    int val = 0;
                    return Int32.TryParse(this.ddlTipoPagoCredito.SelectedValue, out val) ? (int?)val : null;
                }
                return null;
            }
            set {
                if (value.HasValue)
                    this.ddlTipoPagoCredito.SelectedValue = value.Value.ToString().Trim();
                else
                    this.ddlTipoPagoCredito.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Obtiene o establece la forma de pago del contrato
        /// </summary>
        public int? FormaPagoID {
            get {
                if (!string.IsNullOrEmpty(this.ddlFormaPago.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlFormaPago.SelectedValue)) {
                    int val = 0;
                    return Int32.TryParse(this.ddlFormaPago.SelectedValue, out val) ? (int?)val : null;
                }
                return null;
            }
            set {
                if (value.HasValue)
                    this.ddlFormaPago.SelectedValue = value.Value.ToString().Trim();
            }
        }
        /// <summary>
        /// Obtiene o establece la frecuencia de facturación para el contrato
        /// </summary>
        public int? FrecuenciaFacturacionID {
            get {
                return (int)EFrecuencia.DIARIA;
            }
        }
        /// <summary>
        /// Nombre de la persona que autoriza el pago a crédito
        /// </summary>
        public string AutorizadorTipoConfirmacion {
            get {
                return (String.IsNullOrEmpty(this.txtPersonaAutorizaCredito.Text)) ? null : this.txtPersonaAutorizaCredito.Text.Trim().ToUpper(); //RI0061
            }
            set {
                this.txtPersonaAutorizaCredito.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                          ? value.Trim().ToUpper()
                                                          : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la persona que autoriza la orden de compra
        /// </summary>
        public string AutorizadorOrdenCompra {
            get {
                return (String.IsNullOrEmpty(txtPersonaAutorizaOrdenCompra.Text)) ? null : this.txtPersonaAutorizaOrdenCompra.Text.Trim().ToUpper(); //RI0061
            }
            set {
                this.txtPersonaAutorizaOrdenCompra.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                          ? value.Trim().ToUpper()
                                                          : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la mercancía que se va a transportar en la unidad
        /// </summary>
        public string MercanciaTransportar {
            get {
                return (String.IsNullOrEmpty(this.txtCondicionesMercancia.Text)) ? null : this.txtCondicionesMercancia.Text.Trim().ToUpper(); //RI0061
            }
            set {
                this.txtCondicionesMercancia.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el destino o área de operación para la unidad
        /// </summary>
        public string DestinoAreaOperacion {
            get {
                return (String.IsNullOrEmpty(this.txtCondicionesAreaOperacion.Text)) ? null : this.txtCondicionesAreaOperacion.Text.Trim().ToUpper(); //RI0061
            }
            set {
                this.txtCondicionesAreaOperacion.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la fecha de inicio del arrendamiento
        /// </summary>
        public DateTime? FechaInicioArrendamiento {
            get {
                if (!string.IsNullOrEmpty(this.hdnCondicionesFechaInicioArrendamiento.Value) && !string.IsNullOrWhiteSpace(this.hdnCondicionesFechaInicioArrendamiento.Value)) {
                    DateTime date;
                    return DateTime.TryParse(this.hdnCondicionesFechaInicioArrendamiento.Value, out date)
                               ? (DateTime?)date
                               : null;
                }
                return null;
            }
            set { this.hdnCondicionesFechaInicioArrendamiento.Value = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece la fecha de promesa de devolución de la unidad
        /// </summary>
        public DateTime? FechaPromesaDevolucion {
            get {
                if (!string.IsNullOrEmpty(this.hdnCondicionesFechaPromesaDevolucion.Value) && !string.IsNullOrWhiteSpace(this.hdnCondicionesFechaPromesaDevolucion.Value)) {
                    DateTime date;
                    return DateTime.TryParse(this.hdnCondicionesFechaPromesaDevolucion.Value, out date)
                               ? (DateTime?)date
                               : null;
                }
                return null;
            }
            set { this.hdnCondicionesFechaPromesaDevolucion.Value = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece la fecha de inicio del arrendamiento
        /// </summary>
        public DateTime? FechaInicioActual {
            get {
                if (!string.IsNullOrEmpty(this.txtCondicionesFechaInicioActual.Text) && !string.IsNullOrWhiteSpace(this.txtCondicionesFechaInicioActual.Text)) {
                    DateTime date;
                    return DateTime.TryParse(this.txtCondicionesFechaInicioActual.Text, out date)
                               ? (DateTime?)date
                               : null;
                }
                return null;
            }
            set { this.txtCondicionesFechaInicioActual.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece la fecha de promesa de devolución de la unidad
        /// </summary>
        public DateTime? FechaPromesaActual {
            get {
                if (!string.IsNullOrEmpty(this.txtCondicionesFechaPromesaActual.Text) && !string.IsNullOrWhiteSpace(this.txtCondicionesFechaPromesaActual.Text)) {
                    DateTime date;
                    return DateTime.TryParse(this.txtCondicionesFechaPromesaActual.Text, out date)
                               ? (DateTime?)date
                               : null;
                }
                return null;
            }
            set { this.txtCondicionesFechaPromesaActual.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece las observaciones realizadas al contrato
        /// </summary>
        public string Observaciones {
            get { return (String.IsNullOrEmpty(this.txtObservaciones.Text)) ? null : this.txtObservaciones.Text.Trim().ToUpper(); } //RI0061
            set {
                this.txtObservaciones.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece los días de renta
        /// </summary>
        public int? DiasRenta {
            get {
                if (!string.IsNullOrEmpty(this.txtCondicionesDiasRenta.Text) && !string.IsNullOrWhiteSpace(this.txtCondicionesDiasRenta.Text)) {
                    int val;
                    return Int32.TryParse(this.txtCondicionesDiasRenta.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.txtCondicionesDiasRenta.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        /// <summary>
        /// Cantidad de días para la primera Factura
        /// </summary>
        public Int32? DiasFacturar {
            get {
                return 1;
            }
        }

        /// <summary>
        /// Numero de Serie (VIN) de la Unidad a Agregar
        /// </summary>
        public string NumeroSerie {
            get {
                return txtNumeroSerie.Text.Trim().ToUpper();
            }
            set {
                txtNumeroSerie.Text = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el identificador de la unidad
        /// </summary>
        public int? UnidadID {
            get {
                if (!string.IsNullOrEmpty(this.hdnUnidadID.Value) && !string.IsNullOrWhiteSpace(this.hdnUnidadID.Value)) {
                    int val;
                    return Int32.TryParse(this.hdnUnidadID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnUnidadID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        /// <summary>
        /// obtiene o establece el identificador del equipo
        /// </summary>
        public int? EquipoID {
            get {
                if (!string.IsNullOrEmpty(this.hdnEquipoID.Value) && !string.IsNullOrWhiteSpace(this.hdnEquipoID.Value)) {
                    int val;
                    return Int32.TryParse(this.hdnEquipoID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnEquipoID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
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
        #region Campos de Renta con Opcion a Compra
        /// <summary>
        /// Obtiene o establece el Monto Total del Arrendamiento
        /// </summary>
        public decimal? MontoTotalArrendamiento {
            get {
                if (!String.IsNullOrEmpty(txtMontoArrendamiento.Text.Trim()))
                    return decimal.Parse(txtMontoArrendamiento.Text.Trim(), NumberStyles.Currency);
                return null;
            }
            set {
                this.txtMontoArrendamiento.Text = value != null ? string.Format("{0:c2}", value) : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la fecha de promesa de devolución de la unidad
        /// </summary>
        public DateTime? FechaPagoRenta {
            get {
                if (!string.IsNullOrEmpty(this.txtROCFechaPagoRenta.Text) && !string.IsNullOrWhiteSpace(this.txtROCFechaPagoRenta.Text)) {
                    DateTime date;
                    return DateTime.TryParse(this.txtROCFechaPagoRenta.Text, out date)
                               ? (DateTime?)date
                               : null;
                }
                return null;
            }
            set { this.txtROCFechaPagoRenta.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        /// <summary>
        /// Plazo
        /// </summary>
        public int? Plazo {
            get { return (string.IsNullOrEmpty(this.txtPlazo.Text)) ? null : (int?)int.Parse(this.txtPlazo.Text); }
            set { this.txtPlazo.Text = (value != null) ? value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece la Inversion Inicial
        /// </summary>
        public decimal? InversionInicial {
            get {
                if (!String.IsNullOrEmpty(txtInversionInicial.Text.Trim()))
                    return decimal.Parse(txtInversionInicial.Text.Trim(), NumberStyles.Currency);
                return null;
            }
            set {
                this.txtInversionInicial.Text = value != null ? string.Format("{0:c2}", value) : string.Empty;
            }
        }
        #endregion

        /// <summary>
        /// Líneas del Contrato
        /// </summary>
        public List<LineaContratoPSLBO> LineasContrato {
            get {
                List<LineaContratoPSLBO> listValue;
                if (Session["LineasContratoSession"] != null) {
                    listValue = (List<LineaContratoPSLBO>)Session["LineasContratoSession"];
                } else
                    listValue = new List<LineaContratoPSLBO>();

                return listValue;
            }
            set {
                var listValue = new List<LineaContratoPSLBO>();
                if (value != null) {
                    listValue = value;
                }
                Session["LineasContratoSession"] = listValue;


                grdLineasContrato.DataSource = listValue;
                grdLineasContrato.DataBind();
            }
        }

        ///// <summary>
        ///// Gastos de Maniobra
        ///// </summary>
        public decimal? Maniobra {
            get {
                if (!String.IsNullOrEmpty(txtManiobra.Text.Trim()))
                    return decimal.Parse(txtManiobra.Text.Trim(), NumberStyles.Currency);
                return null;
            }
            set {
                this.txtManiobra.Text = value != null ? string.Format("{0:c2}", value) : string.Empty;
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

        /// <summary>
        /// Obtiene o establece el listado de avales del cliente seleccionado
        /// </summary>
        public List<AvalBO> AvalesTotales {
            get {
                if (Session["ListadoAvalesPSL"] != null)
                    return (List<AvalBO>)Session["ListadoAvalesPSL"];

                return new List<AvalBO>();
            }
            set {
                List<AvalBO> lst = value ?? new List<AvalBO>();

                //Se sube a la sesión
                Session["ListadoAvalesPSL"] = new List<AvalBO>(lst);
                //Se asigna en el dropdownlist
                this.ddlAvales.Items.Clear();
                this.ddlAvales.Items.Add(new ListItem("Seleccione una opción", "0"));
                this.ddlAvales.DataTextField = "Nombre";
                this.ddlAvales.DataValueField = "Id";
                this.ddlAvales.DataSource = lst;
                this.ddlAvales.DataBind();
            }
        }
        /// <summary>
        /// Obtiene o establece los avales del cliente que han sido seleccionados
        /// </summary>
        public List<AvalBO> AvalesSeleccionados {
            get {
                List<AvalBO> lst = new List<AvalBO>();
                if (Session["AvalesContratoPSL"] != null)
                    lst = (List<AvalBO>)Session["AvalesContratoPSL"];

                return lst;
            }
            set {
                List<AvalBO> lst = value ?? new List<AvalBO>();
                Session["AvalesContratoPSL"] = lst;
            }
        }
        /// <summary>
        /// Obtiene el identificador del aval seleccionado
        /// </summary>
        public int? AvalSeleccionadoID {
            get {
                if (!string.IsNullOrEmpty(this.ddlAvales.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlAvales.SelectedValue)) {
                    if (System.String.Compare(this.ddlAvales.SelectedValue, "0", System.StringComparison.Ordinal) != 0) {
                        int val = 0;
                        return Int32.TryParse(this.ddlAvales.SelectedValue, out val) ? (int?)val : null;
                    }
                }
                return null;
            }
        }
        public List<RepresentanteLegalBO> RepresentantesAvalTotales {
            get {
                if (Session["ListaRepresentantesAvalPSL"] == null)
                    return new List<RepresentanteLegalBO>();
                return (List<RepresentanteLegalBO>)Session["ListaRepresentantesAvalPSL"];
            }
            set {
                List<RepresentanteLegalBO> lst = value ?? new List<RepresentanteLegalBO>();

                Session["ListaRepresentantesAvalPSL"] = lst;
                this.grdRepresentantesAval.DataSource = lst;
                this.grdRepresentantesAval.DataBind();
            }
        }
        public List<RepresentanteLegalBO> RepresentantesAvalSeleccionados {
            get { return (List<RepresentanteLegalBO>)Session["RepresentantesAvalPSL"]; }
            set {
                List<RepresentanteLegalBO> lst = value ?? new List<RepresentanteLegalBO>();

                Session["RepresentantesAvalPSL"] = lst;
            }
        }
        public int? RepresentanteAvalSeleccionadoID {
            get {
                if (!string.IsNullOrEmpty(this.hdnRepresentanteAvalSeleccionadoID.Value) && !string.IsNullOrWhiteSpace(this.hdnRepresentanteAvalSeleccionadoID.Value)) {
                    int val = 0;
                    return Int32.TryParse(this.hdnRepresentanteAvalSeleccionadoID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
        }
        public bool? SoloRepresentantes {
            get {
                return this.cbSoloRepresentantes.Checked;
            }
            set {
                if (value != null)
                    this.cbSoloRepresentantes.Checked = value.Value;
                else
                    this.cbSoloRepresentantes.Checked = false;
            }
        }
        /// <summary>
        /// Indica el tipo de contrato, pudiendo ser RO, RE, ROC
        /// </summary>
        public ETipoContrato? TipoContrato {
            get {
                ETipoContrato? tipo = null;
                if (!string.IsNullOrEmpty(this.hdnTipoContrato.Value))
                    tipo = (ETipoContrato)Enum.Parse(typeof(ETipoContrato), this.hdnTipoContrato.Value.Trim());
                return tipo;
            }
            set { this.hdnTipoContrato.Value = value != null ? ((int)value).ToString() : string.Empty; }
        }
        public string ModoRegistro {
            get {
                return this.hdnModoRegistro.Value != null ? this.hdnModoRegistro.Value : string.Empty;
            }
            set {
                this.hdnModoRegistro.Value = value != null ? value : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la fecha de inicio del arrendamiento seleccionado anterior
        /// </summary>
        public DateTime? FechaInicioElegidoPrevio {
            get {
                if (!string.IsNullOrEmpty(this.hdnCondicionesFechaInicioActual.Value) && !string.IsNullOrWhiteSpace(this.hdnCondicionesFechaInicioActual.Value)) {
                    DateTime date;
                    return DateTime.TryParse(this.hdnCondicionesFechaInicioActual.Value, out date)
                               ? (DateTime?)date
                               : null;
                }
                return null;
            }
            set { this.hdnCondicionesFechaInicioActual.Value = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece la fecha de promesa de devolución seleccionado anterior
        /// </summary>
        public DateTime? FechaDevolucionElegidoPrevio {
            get {
                if (!string.IsNullOrEmpty(this.hdnCondicionesFechaPromesaActual.Value) && !string.IsNullOrWhiteSpace(this.hdnCondicionesFechaPromesaActual.Value)) {
                    DateTime date;
                    return DateTime.TryParse(this.hdnCondicionesFechaPromesaActual.Value, out date)
                               ? (DateTime?)date
                               : null;
                }
                return null;
            }
            set { this.hdnCondicionesFechaPromesaActual.Value = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece la configuración de días a cobrar
        /// </summary>
        public bool? IncluyeSD {
            get {
                if (!string.IsNullOrEmpty(this.hdnIncluyeSD.Value) && !string.IsNullOrWhiteSpace(this.hdnIncluyeSD.Value)) {
                    bool val;
                    return Boolean.TryParse(this.hdnIncluyeSD.Value, out val) ? (bool?)val : null;
                }
                return null;
            }
            set { this.hdnIncluyeSD.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Código de la moneda seleccionada anterior
        /// </summary>
        public string CodigoMonedaElegidoPrevio {
            get {
                if (!string.IsNullOrEmpty(this.hdnCondicionesMoneda.Value) && !string.IsNullOrWhiteSpace(this.hdnCondicionesMoneda.Value))
                    return this.hdnCondicionesMoneda.Value.Trim().ToUpper();
                return null;
            }
            set {
                this.hdnCondicionesMoneda.Value = value != null ? value : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la el tipo de unidad a buscar
        /// </summary>
        public bool? EsROC {
            get {
                if (!string.IsNullOrEmpty(this.hdnEsROC.Value) && !string.IsNullOrWhiteSpace(this.hdnEsROC.Value)) {
                    bool val;
                    return Boolean.TryParse(this.hdnEsROC.Value, out val) ? (bool?)val : null;
                }
                return null;
            }
            set { this.hdnEsROC.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        #region Cálculo de Subtotales        
        /// <summary>
        /// Determina el Monto del Contrato
        /// </summary>
        public decimal? MontoTotal {
            get {
                if (String.IsNullOrEmpty(txtSumaTotal.Text)) return null;
                return Int32.Parse(txtSumaTotal.Text.Trim());
            }
            set {
                txtSumaTotal.Text = value != null ? string.Format("{0:c2}", value) : String.Empty;
            }
        }
        /// <summary>
        /// Obtiene o Establece el porcentaje del seguro de la UO
        /// </summary>
        public decimal? PorcentajeSeguro {
            get {
                if (!String.IsNullOrEmpty(hdnPorcentajeSeguro.Value))
                    return Decimal.Parse(hdnPorcentajeSeguro.Value);
                return null;
            }
            set {
                this.hdnPorcentajeSeguro.Value = value != null ? String.Format("{0:##0.00}", value) : string.Empty;
            }
        }
        #endregion

        /// <summary>
        /// Obtiene o establece el número de días anteriores a hoy en que se puede crear un contrato
        /// </summary>
        public short DiasAnterioridadContrato {
            get {
                return Session["DiasAnterioridadContrato"] != null ? (short)Session["DiasAnterioridadContrato"] : (short)0;
            }
            set {
                if (value != null)
                    Session["DiasAnterioridadContrato"] = value;
                else
                    Session.Remove("DiasAnterioridadContrato");
            }
        }
        #endregion

        #region Constructor
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new ucContratoPSLPRE(this);

                this.txtObservaciones.Attributes.Add("onkeyup", "checkText(this,300);");
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear de página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los detalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            } else {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// Habilita la pantalla para el registro de un nuevo contrato
        /// </summary>
        public void PrepararNuevo() {
            #region Limpiamos todos los hiddden fields
            this.hdnCalle.Value = string.Empty;
            this.hdnCiudad.Value = string.Empty;
            this.hdnClienteEsFisico.Value = string.Empty;
            this.hdnClienteID.Value = string.Empty;
            this.hdnColonia.Value = string.Empty;
            this.hdnContratoID.Value = string.Empty;
            this.hdnDireccionId.Value = string.Empty;
            this.hdnEmpresaID.Value = string.Empty;
            this.hdnEquipoID.Value = string.Empty;
            this.hdnEstado.Value = string.Empty;
            this.hdnEstatusContratoID.Value = string.Empty;
            this.hdnMunicipio.Value = string.Empty;
            this.hdnNumeroContrato.Value = string.Empty;
            this.hdnPais.Value = string.Empty;
            this.hdnTipoCuentaRegion.Value = string.Empty;
            this.hdnUnidadID.Value = string.Empty;
            this.hdnProductoServicioId.Value = string.Empty;
            this.hdnCondicionesMoneda.Value = string.Empty;
            this.hdnCondicionesFechaInicioArrendamiento.Value = string.Empty;
            this.hdnCondicionesFechaPromesaDevolucion.Value = string.Empty;
            this.hdnModoRegistro.Value = string.Empty;
            this.hdnCondicionesFechaInicioActual.Value = string.Empty;
            this.hdnCondicionesFechaPromesaActual.Value = string.Empty;
            this.hdnCondicionesFechaPagoRenta.Value = string.Empty;
            #endregion

            #region Limpiamos todos los textbox
            this.txtCodigoPostal.Text = string.Empty;
            this.txtCondicionesAreaOperacion.Text = string.Empty;
            this.txtCondicionesDiasRenta.Text = string.Empty;
            this.txtCondicionesFechaPromesaActual.Text = string.Empty;
            this.txtCondicionesMercancia.Text = string.Empty;
            this.txtDireccionEmpresa.Text = string.Empty;
            this.txtDomicilioCliente.Text = string.Empty;
            this.txtEmpresa.Text = string.Empty;
            this.txtFechaContrato.Text = string.Empty;
            this.txtNombreCliente.Text = string.Empty;
            this.txtNumeroCuentaCliente.Text = string.Empty;
            this.txtNumeroCuentaOracle.Text = string.Empty;
            this.txtObservaciones.Text = string.Empty;
            this.txtPersonaAutorizaCredito.Text = string.Empty;
            this.txtRFCCliente.Text = string.Empty;
            this.txtRepresentante.Text = string.Empty;
            this.ddlSucursales.SelectedIndex = 0;
            this.txtClaveProductoServicio.Text = string.Empty;
            this.txtDescripcionProductoServicio.Text = string.Empty;
            this.txtManiobra.Text = string.Empty;
            this.txtCondicionesFechaInicioActual.Text = string.Empty;
            this.txtCondicionesFechaPromesaActual.Text = string.Empty;
            #endregion

            #region Deshabilitados los controles que solo son de lectura

            this.txtCodigoPostal.Enabled = false;
            this.txtCondicionesDiasRenta.Enabled = false;

            this.txtDireccionEmpresa.Enabled = false;
            this.txtDomicilioCliente.Enabled = false;
            this.txtEmpresa.Enabled = false;
            this.txtNumeroCuentaCliente.Enabled = false;
            this.txtNumeroCuentaOracle.Enabled = false;
            this.txtRFCCliente.Enabled = false;
            this.txtRepresentante.Enabled = false;
            this.txtClaveProductoServicio.Enabled = false;
            this.txtManiobra.Enabled = false;
            #endregion

            #region Inicializamos los valores de fecha y otros controles
            DateTime fechaActual = DateTime.Now;
            this.txtFechaContrato.Text = fechaActual.ToShortDateString();
            this.txtCondicionesFechaInicioActual.Text = fechaActual.ToShortDateString();
            this.txtCondicionesFechaPromesaActual.Text = fechaActual.ToShortDateString();
            this.hdnCondicionesFechaInicioArrendamiento.Value = this.txtCondicionesFechaPromesaActual.Text;
            this.hdnCondicionesFechaPromesaDevolucion.Value = this.txtCondicionesFechaPromesaActual.Text;
            this.hdnCondicionesFechaPagoRenta.Value = this.txtROCFechaPagoRenta.Text;
            this.hdnCondicionesMoneda.Value = "0";
            this.txtCondicionesDiasRenta.Text = "1";
            this.grdLineasContrato.Columns[9].Visible = false;
            #endregion
        }

        public void PrepararEdicion() {
            #region Deshabilitados los controles que solo son de lectura
            this.txtCodigoPostal.Enabled = false;
            this.txtCondicionesDiasRenta.Enabled = false;
            this.txtDireccionEmpresa.Enabled = false;
            this.txtDomicilioCliente.Enabled = false;
            this.txtEmpresa.Enabled = false;
            this.txtNumeroCuentaCliente.Enabled = false;
            this.txtNumeroCuentaOracle.Enabled = false;
            this.txtRFCCliente.Enabled = false;
            this.txtRepresentante.Enabled = false;
            this.txtClaveProductoServicio.Enabled = false;
            this.txtManiobra.Enabled = false;
            #endregion
            #region Bloquear los controles cuando se edita cuando un contrato está en Curso
            if (this.ModoRegistro == "EDEC") {
                this.txtFechaContrato.Enabled = false;
                this.ddlSucursales.Enabled = false;
                this.txtNombreCliente.Enabled = false;
                this.ibtnBuscarCliente.Visible = false;
                this.txtCondicionesAreaOperacion.Enabled = false;
                this.txtCondicionesMercancia.Enabled = false;
                this.txtCondicionesFechaInicioActual.Enabled = false;
                this.txtCondicionesFechaPromesaActual.Enabled = false;
                this.ddlMonedas.Enabled = false;
                this.ddlFormaPago.Enabled = false;
                this.ddlTipoPagoCredito.Enabled = false;
                this.txtPersonaAutorizaCredito.Enabled = false;
                this.txtMontoArrendamiento.Enabled = false;
                this.txtROCFechaPagoRenta.Enabled = false;
                this.txtPlazo.Enabled = false;
                this.txtInversionInicial.Enabled = false;
                this.txtObservaciones.Enabled = false;
                this.txtNumeroSerie.Enabled = false;
                this.ibtnBuscarDirieccionCliente.Visible = false;
                this.trSoloRepresentantes.Visible = false;
                this.trRepresentantes1.Visible = false;
                this.trAgregarUnidades.Visible = false;
                this.trAvales.Visible = false;
                this.trPagoCredito.Visible = false;
                this.trAutorizadorCredito.Visible = false;
                this.grdLineasContrato.Columns[7].Visible = false;
                this.grdLineasContrato.Columns[9].Visible = false;
                this.grdRepresentantesLegales.Columns[2].Visible = false;
                this.grdAvales.Columns[3].Visible = false;
                this.grdAvales.Columns[4].Visible = true;
            }
            this.lblTitulo.Text = "UNIDADES AGREGADAS A RENTA";
            #endregion
        }

        /// <summary>
        /// Carga las sucursales autorizadas en el combobox
        /// </summary>
        /// <param name="lstSucursales">Lista de sucursales autorizadas</param>
        public void CargarSucursales(List<SucursalBO> lstSucursales) {
            try {
                List<SucursalBO> lstSucCargar = new List<SucursalBO>();
                // Agregar el SucursalBO de fachada
                lstSucCargar.Insert(0, new SucursalBO { Id = 0, Nombre = "SELECCIONAR" });
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
                this.MostrarMensaje("Error al cargar las sucursales autorizadas.", ETipoMensajeIU.ERROR, "RegistrarContratoROUI.CargarSucursales: " + ex.Message);
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
        /// Habilita los controles que serán editables para la renovación
        /// </summary>
        public void PrepararRenovacion() {
            #region Deshabilitados los controles que solo son de lectura
            this.txtCodigoPostal.Enabled = false;
            this.txtCondicionesDiasRenta.Enabled = false;

            this.txtDireccionEmpresa.Enabled = false;
            this.txtDomicilioCliente.Enabled = false;
            this.txtEmpresa.Enabled = false;
            this.txtNumeroCuentaCliente.Enabled = false;
            this.txtNumeroCuentaOracle.Enabled = false;
            this.txtRFCCliente.Enabled = false;
            this.txtRepresentante.Enabled = false;
            this.txtManiobra.Enabled = false;

            this.txtFechaContrato.Enabled = false;
            this.txtCondicionesFechaInicioActual.Enabled = false;
            this.txtEmpresa.Enabled = false;
            this.ddlSucursales.Enabled = false;
            this.txtRepresentante.Enabled = false;
            this.txtDireccionEmpresa.Enabled = false;
            this.txtNombreCliente.Enabled = false;
            this.ibtnBuscarCliente.Visible = false;
            this.txtRFCCliente.Enabled = false;
            this.txtNumeroCuentaCliente.Enabled = false;
            this.txtNumeroCuentaOracle.Enabled = false;
            this.txtDomicilioCliente.Enabled = false;
            this.txtCodigoPostal.Enabled = false;
            this.txtCondicionesAreaOperacion.Enabled = false;
            this.txtCondicionesMercancia.Enabled = false;
            this.ddlMonedas.Enabled = false;
            this.txtCondicionesDiasRenta.Enabled = false;
            this.ddlFormaPago.Enabled = false;
            this.ddlTipoPagoCredito.Enabled = false;
            this.txtPersonaAutorizaCredito.Enabled = false;
            this.txtClaveProductoServicio.Enabled = false;
            this.txtMontoArrendamiento.Enabled = false;
            this.txtROCFechaPagoRenta.Enabled = false;
            this.txtPlazo.Enabled = false;
            this.txtInversionInicial.Enabled = false;
            this.txtManiobra.Enabled = false;
            this.txtObservaciones.Enabled = false;
            this.txtNumeroSerie.Enabled = false;
            this.ibtnBuscarDirieccionCliente.Visible = false;
            this.trSoloRepresentantes.Visible = false;
            this.trRepresentantes1.Visible = false;
            this.trAgregarUnidades.Visible = false;
            this.trAvales.Visible = false;
            this.trPagoCredito.Visible = false;
            this.trAutorizadorCredito.Visible = false;
            this.grdLineasContrato.Columns[7].Visible = false;
            this.grdRepresentantesLegales.Columns[2].Visible = false;
            this.lblTitulo.Text = "UNIDADES AGREGADAS A RENTA";
            this.ibtnBuscarDirieccionCliente.Enabled = false;
            this.grdAvales.Columns[3].Visible = false;
            this.grdAvales.Columns[4].Visible = false;
            #endregion
        }

        /// <summary>
        /// Muestra u oculta los controles relacionados con la sección ROC
        /// </summary>
        /// <param name="mostrar"></param>
        public void MostrarControlesROC(bool mostrar) {
            this.opcionesROC.Visible = mostrar;
        }

        /// <summary>
        /// Habilita o deshabilita la opción de seleccionar direcciones para el cliente
        /// </summary>
        /// <param name="permitir">Parámetro que controla el estatus del control</param>
        public void PermitirSeleccionarDireccionCliente(bool permitir) {
            this.ibtnBuscarDirieccionCliente.Enabled = permitir;
            this.ibtnBuscarDirieccionCliente.Visible = permitir;
        }

        /// <summary>
        /// Habilita o deshabilita la opción de seleccionar unidades
        /// </summary>
        /// <param name="permitir">Parámetro que controla el estatus del control</param>
        public void PermitirSeleccionarUnidad(bool permitir) {
            this.ibtnBuscarUnidad.Enabled = permitir;
            this.ibtnBuscarUnidad.Visible = permitir;
            this.txtNumeroSerie.Enabled = permitir;
        }

        /// <summary>
        /// Habilita o deshabilita la opciónn de seleccionar Moneda
        /// </summary>
        /// <param name="permitir">Parámetro que controla el estatus del control</param>
        public void PermitirSeleccionarMoneda(bool permitir) {
            this.ddlMonedas.Enabled = permitir;
        }

        /// <summary>
        /// Habilita o deshabilita la opción de seleccionar representantes legales de acuerdo al tipo de cliente
        /// </summary>
        /// <param name="permitir">Parámetro que controla el estatus del control</param>
        public void PermitirSeleccionarRepresentantes(bool permitir) {
            this.ddlRepresentantesLegales.Enabled = permitir;
        }

        /// <summary>
        /// Habilita o deshabilita la opción de agregar representantes legales
        /// </summary>
        /// <param name="permitir">Parámetro que controla el estatus del control</param>
        public void PermitirAgregarRepresentantes(bool permitir) {
            this.btnAgregarRepresentante.Enabled = permitir;
            this.btnAgregarRepresentante.Visible = permitir;
        }

        /// <summary>
        /// Habilita o deshabilita la opciones de pago a crédito
        /// </summary>
        /// <param name="permitir">Parámetro que controla el estatus del control</param>
        public void PermitirSeleccionarTipoConfirmacion(bool permitir) {
            this.tblPagoCredito.Visible = permitir;

            this.txtPersonaAutorizaCredito.Enabled = permitir;
            this.txtPersonaAutorizaCredito.Visible = permitir;

            this.ddlTipoPagoCredito.Enabled = permitir;
            this.ddlTipoPagoCredito.Visible = permitir;
        }

        /// <summary>
        /// Habilita o deshabilita la opción de autorizador de orden de compra para pago a crédito
        /// </summary>
        /// <param name="permitir">Parámetro que controla el estatus del control</param>
        public void PermitirAsignarAutorizadorOrdenCompra(bool permitir) {
            this.trAutorizaOrdenCompra.Visible = permitir;
        }

        /// <summary>
        /// Habilita o deshabilita la opción de búsqueda de productos servicio.
        /// </summary>
        /// <param name="permitir"></param>
        public void PermitirAgregarProductoServicio(bool permitir) {
            this.txtClaveProductoServicio.ReadOnly = !permitir;
            this.txtClaveProductoServicio.Enabled = permitir;
            this.ibtnBuscarProductoServicio.Visible = permitir;
        }

        /// <summary>
        /// Provee las monedas que pueden aplicar a los contratos
        /// </summary>
        /// <param name="monedas">Listado de monedas que puede seleccionar el usuario</param>
        public void EstablecerOpcionesMoneda(Dictionary<string, string> monedas) {
            if (ReferenceEquals(monedas, null))
                monedas = new Dictionary<string, string>();

            this.ddlMonedas.DataSource = monedas;
            this.ddlMonedas.DataValueField = "key";
            this.ddlMonedas.DataTextField = "value";
            ListItem item = new ListItem { Enabled = true, Selected = true, Text = "Seleccione una opción", Value = "0" };
            this.ddlMonedas.Items.Add(item);
            this.ddlMonedas.DataBind();
        }

        /// <summary>
        /// Actualiza el grid de representantes legales con los representantes seleccionados
        /// </summary>
        public void ActualizarRepresentantesLegales() {
            grdRepresentantesLegales.DataSource = this.RepresentantesSeleccionados;
            grdRepresentantesLegales.DataBind();
        }

        /// <summary>
        /// Limpia de sesión las variables que se usan para registrar un contrato
        /// </summary>
        public void LimpiarSesion() {
            Session.Remove("RepresentantesLegalesContrato");
            Session.Remove("ContratoPSLBO");
            if (Session["LineasContratoSession"] != null)
                Session.Remove("LineasContratoSession");
        }
        /// <summary>
        /// Habilita o deshabilita el agregar una unidad
        /// </summary>
        /// <param name="habilitar">Indica si habilita o deshabilita el botón</param>
        public void HabilitarAgregarUnidad(bool habilitar) {
            this.btnAgregarUnidad.Enabled = habilitar;
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

        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script) {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        /// <summary>
        /// ejecuta el buscador para mostrar la direccion de cliente
        /// </summary>
        public void DesplegarDireccionCliente() {
            EjecutaBuscador("DireccionCuentaClienteIdealeaseSimple", ECatalogoBuscador.DireccionCliente);
        }
        #endregion

        public void ActualizarAvales() {
            this.grdAvales.DataSource = this.AvalesSeleccionados;
            this.grdAvales.DataBind();
        }

        public void MostrarAvales(bool mostrar) {
            this.pnlAvales.Visible = mostrar;
        }

        public void MostrarRepresentantesAval(bool mostrar) {
            this.trRepresentantesAval.Visible = mostrar;
        }
        public void MostrarDetalleRepresentantesAval(List<RepresentanteLegalBO> representantes) {
            this.grdRepresentantesDialog.DataSource = representantes;
            this.grdRepresentantesDialog.DataBind();

            this.RegistrarScript("DetalleAval", "abrirDetalleRepresentantes('AVAL');");
        }

        public void MostrarPersonasCliente(bool mostrar) {
            pnlPersonasCliente.Visible = mostrar;
        }
        public void PermitirAgregarAvales(bool permitir) {
            this.btnAgregarAval.Enabled = permitir;
            this.btnAgregarAval.Visible = permitir;
            this.MostrarRepresentantesAval(permitir);
        }
        public void PermitirSeleccionarAvales(bool permitir) {
            this.ddlAvales.Enabled = permitir;
        }

        public void MostrarRepresentantesLegales(bool mostrar) {
            pnlRepresentantes.Visible = mostrar;
        }

        /// <summary>
        /// Valida si será necesario la eliminación de las tarifas de la línea de contrato por cambio en condiciones
        /// </summary>
        /// <param name="ctrl">Indica el control a validar</param>
        /// <param name="value">Valor anterior del control</param>
        /// <param name="diasRenta">Días de renta anterior</param>
        private void ValidarEliminacionTarifasLinea(string ctrl, string value, string diasRenta) {
            try {
                if (!this.presentador.ExistenLineasNoValidas())
                    this.RegistrarScript("EliminarTarifa", "confirmarEliminarTarifas('" + ctrl + "','" + value + "','" + diasRenta + "','" + this.ModoRegistro + "');");
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }

        /// <summary>
        /// Dibuja de nuevo el grid de líneas de contrato
        /// </summary>
        public void RenderizarGridLineas() {
            try {
                grdLineasContrato.DataSource = LineasContrato;
                grdLineasContrato.DataBind();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias en el renderizado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".RenderizarGridLineas: " + ex.Message);
            }
        }

        /// <summary>
        /// Permite habilitar/deshabilitar los controles para el detalle del contrato
        /// </summary>
        public void PrepararVistaDetalle() {
            this.txtFechaContrato.Enabled = false;
            this.txtTasaInteres.Enabled = false;
            this.txtEmpresa.Enabled = false;
            this.ddlSucursales.Enabled = false;
            this.txtRepresentante.Enabled = false;
            this.txtDireccionEmpresa.Enabled = false;
            this.txtNombreCliente.Enabled = false;
            this.ibtnBuscarCliente.Visible = false;
            this.txtRFCCliente.Enabled = false;
            this.txtNumeroCuentaCliente.Enabled = false;
            this.txtNumeroCuentaOracle.Enabled = false;
            this.txtDomicilioCliente.Enabled = false;
            this.txtCodigoPostal.Enabled = false;
            this.txtCondicionesAreaOperacion.Enabled = false;
            this.txtCondicionesMercancia.Enabled = false;
            this.txtCondicionesFechaInicioActual.Enabled = false;
            this.txtCondicionesFechaPromesaActual.Enabled = false;
            this.ddlMonedas.Enabled = false;
            this.txtCondicionesDiasRenta.Enabled = false;
            this.ddlFormaPago.Enabled = false;
            this.ddlTipoPagoCredito.Enabled = false;
            this.txtPersonaAutorizaCredito.Enabled = false;
            this.txtClaveProductoServicio.Enabled = false;
            this.txtMontoArrendamiento.Enabled = false;
            this.txtROCFechaPagoRenta.Enabled = false;
            this.txtPlazo.Enabled = false;
            this.txtInversionInicial.Enabled = false;
            this.txtManiobra.Enabled = false;
            this.txtObservaciones.Enabled = false;
            this.txtNumeroSerie.Enabled = false;
            this.ibtnBuscarDirieccionCliente.Visible = false;
            this.trSoloRepresentantes.Visible = false;
            this.trRepresentantes1.Visible = false;
            this.trAgregarUnidades.Visible = false;
            this.trAvales.Visible = false;
            this.trPagoCredito.Visible = false;
            this.trAutorizadorCredito.Visible = false;
            this.grdLineasContrato.Columns[7].Visible = false;
            this.grdRepresentantesLegales.Columns[2].Visible = false;
            this.grdAvales.Columns[3].Visible = false;
            this.grdAvales.Columns[4].Visible = true;
            this.lblTitulo.Text = "UNIDADES AGREGADAS A RENTA";
        }

        public string ValidarCamposContratoROC(bool? validarCamposContratoROC) {
            return presentador.ValidarCamposContratoROC(validarCamposContratoROC);
        }
        #endregion

        #region Eventos
        protected void ddlSucursales_SelectedIndexChanged(object sender, EventArgs e) {
            this.presentador.SeleccionarSucursal();
        }

        protected void txtNombreCliente_TextChanged(object sender, EventArgs e) {
            try {
                this.CuentaClienteID = null;

                string nombreCuentaCliente = this.CuentaClienteNombre;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                this.CuentaClienteNombre = nombreCuentaCliente;
                if (!string.IsNullOrWhiteSpace(this.CuentaClienteNombre))
                    EjecutaBuscador("CuentaClienteIdealeaseSimple", ECatalogoBuscador.CuentaClienteIdealease);

                this.CuentaClienteNombre = null;
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNombreCliente_TextChanged:" + ex.Message);
            }
        }
        protected void ibtnBuscarCliente_Click(object sender, ImageClickEventArgs e) {
            try {
                this.CuentaClienteID = null;

                string nombreCuentaCliente = this.CuentaClienteNombre;
                Session_BOSelecto = null;
                DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                this.CuentaClienteNombre = nombreCuentaCliente;
                if (!string.IsNullOrWhiteSpace(this.CuentaClienteNombre))
                    EjecutaBuscador("CuentaClienteIdealeaseSimple&hidden=0", ECatalogoBuscador.CuentaClienteIdealease);
                else
                    MostrarMensaje("Ingrese un parámetro de búsqueda.", ETipoMensajeIU.INFORMACION);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarCliente_Click:" + ex.Message);
            }
        }

        protected void ibtnBuscarDirieccionCliente_Click(object sender, ImageClickEventArgs e) {
            try {
                if (!string.IsNullOrWhiteSpace(this.hdnClienteID.Value))
                    EjecutaBuscador("DireccionCuentaClienteIdealeaseSimple", ECatalogoBuscador.DireccionCliente);
                else
                    this.MostrarMensaje("Por favor seleccione un cliente previamente a consultar sus direcciones.", ETipoMensajeIU.ADVERTENCIA, null);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar las direcciones del Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarDirieccionCliente_Click:" + ex.Message);
            }
        }

        protected void txtClaveProductoServicio_TextChanged(object sender, EventArgs e) {
            try {
                string clvProducto = this.ClaveProductoServicio;
                Session_BOSelecto = null;
                DesplegarBOSelecto(ECatalogoBuscador.ProductoServicio);

                this.ClaveProductoServicio = clvProducto;
                if (!string.IsNullOrWhiteSpace(clvProducto))
                    EjecutaBuscador("ProductoServicio", ECatalogoBuscador.ProductoServicio);
                    
                this.ProductoServicioId = null;
                this.ClaveProductoServicio = null;
                this.DescripcionProductoServicio = null;
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar un Producto", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtClaveProductoServicio: " + ex.Message);
            }
        }
        protected void ibtnBuscarProductoServicio_Click(object sender, ImageClickEventArgs e) {
            try {
                string clvProducto = this.ClaveProductoServicio;
                Session_BOSelecto = null;
                DesplegarBOSelecto(ECatalogoBuscador.ProductoServicio);

                this.ClaveProductoServicio = clvProducto;
                if (!string.IsNullOrWhiteSpace(clvProducto))
                    EjecutaBuscador("ProductoServicio&hidden=0", ECatalogoBuscador.ProductoServicio);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar el producto", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarProductoServicio_Click: " + ex.Message);
            }
        }

        protected void btnAgregarRepresentante_Click(object sender, EventArgs e) {
            try {
                this.presentador.AgregarRepresentanteLegal();
                ddlRepresentantesLegales.SelectedValue = "0";
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al agregar un Representante Legal al contrato", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnAgregarRepresentante_Click: " + ex.Message);
            }
        }

        protected void grdRepresentantesLegales_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                grdRepresentantesLegales.DataSource = this.RepresentantesSeleccionados;
                grdRepresentantesLegales.PageIndex = e.NewPageIndex;
                grdRepresentantesLegales.DataBind();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al cambiar de pagina en los datos del representante legal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".grdRepresentantesLegales_PageIndexChanging: " + ex.Message);
            }
        }
        protected void grdRepresentantesLegales_RowCommand(object sender, GridViewCommandEventArgs e) {
            try {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                int index = Convert.ToInt32(e.CommandArgument);
                this.presentador.QuitarRepresentanteLegal(index);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".grdRepresentantesLegales_RowCommand: " + ex.Message);
            }
        }
        protected void grdRepresentantesLegales_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {
                if (e.Row.RowType == DataControlRowType.DataRow) {
                    var label = e.Row.FindControl("lblTipoPersona") as Label;
                    if (label != null) {
                        var persona = (PersonaBO)e.Row.DataItem;
                        string tipo = string.Empty;
                        if (persona.TipoPersona.HasValue)
                            tipo =
                                ((DescriptionAttribute)
                                 persona.TipoPersona.Value.GetType()
                                        .GetField(persona.TipoPersona.Value.ToString())
                                        .GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description;
                        label.Text = tipo;
                    }
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".grdRepresentantesLegales_RowDataBound: " + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e) {
            try {
                switch (ViewState_Catalogo) {
                    case ECatalogoBuscador.Unidad:
                    case ECatalogoBuscador.CuentaClienteIdealease:
                    case ECatalogoBuscador.DireccionCliente:
                    case ECatalogoBuscador.ProductoServicio:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click: " + ex.Message);
            }
        }

        protected void ddlFormaPago_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if (!string.IsNullOrEmpty(this.ddlFormaPago.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlFormaPago.SelectedValue))
                    this.presentador.SeleccionarFormaPago(int.Parse(this.ddlFormaPago.SelectedValue));
                else
                    this.presentador.SeleccionarFormaPago(null);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al seleccionar la forma de pago", ETipoMensajeIU.ERROR, nombreClase + ".ddlFormaPago_SelectedIndexChanged: " + ex.Message);
            }
        }
        protected void ddlTipoPagoCredito_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if (this.TipoConfirmacionID != null)
                    this.presentador.SeleccionarTipoConfirmacion(this.TipoConfirmacionID);
                else
                    this.presentador.SeleccionarTipoConfirmacion(null);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al seleccionar el tipo de crédito", ETipoMensajeIU.ERROR, nombreClase + ".ddlTipoPagoCredito_SelectedIndexChanged: " + ex.Message);
            }
        }
        protected void txtCondicionesFechaInicioActual_TextChanged(object sender, EventArgs e) {
            try {
                string diasRenta = string.IsNullOrEmpty(this.txtCondicionesDiasRenta.Text) ? string.Empty : this.txtCondicionesDiasRenta.Text;
                this.presentador.SeleccionarFechasContrato();
                this.ValidarEliminacionTarifasLinea("fechainicio", string.IsNullOrEmpty(this.hdnCondicionesFechaInicioActual.Value) ? string.Empty : this.hdnCondicionesFechaInicioActual.Value, diasRenta);
                this.hdnCondicionesFechaInicioActual.Value = this.txtCondicionesFechaInicioActual.Text;
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al momento de validar la información.", ETipoMensajeIU.ERROR, nombreClase + ".txtCondicionesFechaDevolucion_TextChanged: " + ex.Message);
            }
        }
        protected void txtCondicionesFechaPromesaActual_TextChanged(object sender, EventArgs e) {
            try {
                string diasRenta = string.IsNullOrEmpty(this.txtCondicionesDiasRenta.Text) ? string.Empty : this.txtCondicionesDiasRenta.Text;
                this.presentador.SeleccionarFechasContrato();
                this.ValidarEliminacionTarifasLinea("fechadevolucion", string.IsNullOrEmpty(this.hdnCondicionesFechaPromesaActual.Value) ? string.Empty : this.hdnCondicionesFechaPromesaActual.Value, diasRenta);
                this.hdnCondicionesFechaPromesaActual.Value = this.txtCondicionesFechaPromesaActual.Text;
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al momento de validar la información.", ETipoMensajeIU.ERROR, nombreClase + ".txtCondicionesFechaDevolucion_TextChanged: " + ex.Message);
            }
        }
        protected void txtFechaContrato_TextChanged(object sender, EventArgs e) {
            try {
                this.presentador.SeleccionarFechasContrato();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al momento de validar la información.", ETipoMensajeIU.ERROR, nombreClase + ".txtFechaContrato_TextChanged: " + ex.Message);
            }
        }
        protected void txtROCFechaPagoRenta_TextChanged(object sender, EventArgs e) {
            try {
                this.hdnCondicionesFechaPagoRenta.Value = this.txtROCFechaPagoRenta.Text;
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al momento de validar la información.", ETipoMensajeIU.ERROR, nombreClase + ".txtFechaContrato_TextChanged: " + ex.Message);
            }
        }
        protected void ddlMonedas_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                string diasRenta = string.IsNullOrEmpty(this.txtCondicionesDiasRenta.Text) ? string.Empty : this.txtCondicionesDiasRenta.Text;
                this.presentador.SeleccionarMoneda(this.ddlMonedas.SelectedValue);
                this.ValidarEliminacionTarifasLinea("moneda", this.hdnCondicionesMoneda.Value, diasRenta);
                this.hdnCondicionesMoneda.Value = this.ddlMonedas.SelectedValue;
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al seleccionar la moneda", ETipoMensajeIU.ERROR, nombreClase + ".ddlMonedas_SelectedIndexChanged" + ex.Message);
            }
        }

        protected void cbSoloRepresentantes_CheckedChanged(object sender, EventArgs e) {
            try {
                this.presentador.ConfigurarOpcionesPersonas();
                this.ddlAvales.SelectedIndex = -1;
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".cbSoloRepresentantes_CheckedChanged: " + ex.Message);
            }
        }
        protected void ddlAvales_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                this.presentador.SeleccionarAval();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al seleccionar el aval", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ddlAvales_SelectedIndexChanged: " + ex.Message);
            }
        }
        protected void btnAgregarAval_Click(object sender, EventArgs e) {
            try {
                this.presentador.AgregarAval();

                this.ddlAvales.SelectedIndex = -1;
                this.RepresentantesAvalSeleccionados = null;
                this.RepresentantesAvalTotales = null;
                this.MostrarRepresentantesAval(false);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al agregar un Aval al contrato", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnAgregarAval_Click: " + ex.Message);
            }
        }
        protected void grdRepresentantesAval_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                this.grdRepresentantesAval.DataSource = this.RepresentantesAvalTotales;
                this.grdRepresentantesAval.PageIndex = e.NewPageIndex;
                this.grdRepresentantesAval.DataBind();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al cambiar la página de los obligados solidarios", ETipoMensajeIU.ERROR, nombreClase + ".grdObligadosSolidarios_PageIndexChanging: " + ex.Message);
            }
        }
        protected void chkRepresentanteAval_CheckedChanged(object sender, EventArgs e) {
            try {
                //Se obtiene el id de los controles
                CheckBox chk = (CheckBox)sender;
                GridViewRow row = (GridViewRow)chk.Parent.Parent;
                Label lbl = (Label)row.FindControl("lblRepresentanteAvalID");

                int id;
                if (Int32.TryParse(lbl.Text, out id)) {
                    this.hdnRepresentanteAvalSeleccionadoID.Value = id.ToString();

                    if (chk.Checked)
                        this.presentador.AgregarRepresentanteAval();
                    else
                        this.presentador.QuitarRepresentanteAval();

                    this.hdnRepresentanteAvalSeleccionadoID.Value = string.Empty;
                } else
                    throw new Exception("No se encontró el ID del representante legal del aval o tiene un dato inválido.");
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al seleccionar el representante legal para el aval", ETipoMensajeIU.ERROR, nombreClase + ".chkRepresentanteAval_CheckedChanged: " + ex.Message);
            }
        }

        protected void grdRepresentantesAval_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {
                if (e.Row.RowType == DataControlRowType.DataRow) {
                    RepresentanteLegalBO persona = (RepresentanteLegalBO)e.Row.DataItem;
                    var chk = e.Row.FindControl("chkRepAval") as CheckBox;

                    if (chk != null)
                        chk.Checked = this.RepresentantesAvalSeleccionados != null && this.RepresentantesAvalSeleccionados.Exists(p => p.Id == persona.Id);
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias listar los representantes legales del aval", ETipoMensajeIU.ERROR, nombreClase + ".grdRepresentantesAval_RowDataBound: " + ex.Message);
            }
        }

        protected void grdAvales_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                this.grdAvales.DataSource = this.AvalesSeleccionados;
                this.grdAvales.PageIndex = e.NewPageIndex;
                this.grdAvales.DataBind();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al cambiar la página de los avales", ETipoMensajeIU.ERROR, nombreClase + ".grdAvaless_PageIndexChanging: " + ex.Message);
            }
        }

        protected void grdAvales_RowCommand(object sender, GridViewCommandEventArgs e) {
            try {
                if (e.CommandName.ToUpper() == "PAGE" || e.CommandName.ToUpper() == "SORT") return;
                int index = Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName.ToUpper().Trim()) {
                    case "CMDELIMINAR":
                        this.presentador.QuitarAval(index);
                        break;
                    case "CMDDETALLE":
                        this.presentador.PrepararVisualizacionRepresentantesAval(index);
                        break;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al ejecutar la acción sobre el aval", ETipoMensajeIU.ERROR, nombreClase + ".grdAvales_RowCommand: " + ex.Message);
            }
        }

        protected void grdAvales_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {
                if (e.Row.RowType != DataControlRowType.DataRow) return;
                AvalBO bo = e.Row.DataItem != null ? (AvalBO)e.Row.DataItem : new AvalProxyBO();
                if (!bo.TipoAval.HasValue)
                    e.Row.FindControl("ibtDetalle").Visible = false;
                else if (bo.TipoAval == ETipoAval.Fisico)
                    e.Row.FindControl("ibtDetalle").Visible = false;
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al desplegar los avales", ETipoMensajeIU.ERROR, nombreClase + ".grdAvales_RowDataBound: " + ex.Message);
            }
        }

        protected void btnAgregarUnidad_Click(object sender, EventArgs e) {
            try {
                if (AgregarUnidadClick != null) AgregarUnidadClick.Invoke(sender, e);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al agregar la Unidad.", ETipoMensajeIU.ERROR, nombreClase + ".btnAgregarUnidad_Click:" + ex.Message);
            }
        }

        protected void ibtnBuscarUnidad_Click(object sender, ImageClickEventArgs e) {
            try {
                string numeroSerie = NumeroSerie;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.Unidad);

                NumeroSerie = numeroSerie;
                if (!string.IsNullOrWhiteSpace(this.NumeroSerie))
                    EjecutaBuscador("UnidadIdealeaseSimple&hidden=0", ECatalogoBuscador.Unidad);
                else
                    MostrarMensaje("Ingrese un parámetro de búsqueda.", ETipoMensajeIU.INFORMACION);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarUnidad_Click: " + ex.Message);
            }
        }

        protected void txtNumeroSerie_TextChanged(object sender, EventArgs e) {
            try {
                string numeroSerie = NumeroSerie;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.Unidad);

                NumeroSerie = numeroSerie;
                if (!string.IsNullOrWhiteSpace(this.NumeroSerie))
                    EjecutaBuscador("UnidadIdealeaseSimple", ECatalogoBuscador.Unidad);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNumeroSerie_TextChanged: " + ex.Message);
            }
        }
        protected void grdLineasContrato_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                grdLineasContrato.DataSource = LineasContrato;
                grdLineasContrato.PageIndex = e.NewPageIndex;
                grdLineasContrato.DataBind();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdLineasContrato_PageIndexChanging: " + ex.Message);
            }
        }

        protected void grdLineasContrato_RowCommand(object sender, GridViewCommandEventArgs e) {
            try {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                if (e.CommandArgument == null) return;

                int index = 0;

                if (!Int32.TryParse(e.CommandArgument.ToString(), out index))
                    return;

                LineaContratoPSLBO linea = LineasContrato[index];

                switch (eCommandNameUpper) {
                    case "CMDELIMINAR": {
                            this.presentador.RemoverLineaContrato(linea);
                            if (RemoverLineaContrato != null) RemoverLineaContrato.Invoke(sender, EventArgs.Empty);
                        }
                        break;
                    case "CMDDETALLES": {
                            if (VerDetalleLineaContrato != null) {
                                var c = new CommandEventArgs("LineaContrato", linea);

                                VerDetalleLineaContrato.Invoke(sender, c);
                            }
                        }
                        break;
                    case "CMDCHKLIST": {
                            var c = new CommandEventArgs("LineaContrato", linea);
                            ImprimirChkEntregaRecepcion.Invoke(sender, c);
                        }
                        break;
                }

            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al eliminar la unidad del contrato.", ETipoMensajeIU.ERROR, nombreClase + ".grdLineasContrato_RowCommand: " + ex.Message);
            }
        }

        protected void grdLineasContrato_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {
                if (e.Row.RowType == DataControlRowType.DataRow) {
                    LineaContratoPSLBO linea = ((LineaContratoPSLBO)e.Row.DataItem);
                    var label = e.Row.FindControl("lblVIN") as Label;

                    var button = e.Row.FindControl("ibtnDetalles") as ImageButton;
                    button.Visible = true;
                    #region CheckList
                    var buttonChkList = e.Row.FindControl("ibtnChkList") as ImageButton;
                    buttonChkList.Visible = true;

                    if (linea.ListadosVerificacion == null)
                        buttonChkList.Enabled = false;
                    else {
                        if (linea.ListadosVerificacion.Count == 0) {
                            buttonChkList.Enabled = false;
                        } else
                            buttonChkList.Enabled = true;
                    }
                    #endregion
                    // Numero de Serie
                    if (label != null) {
                        if (linea.Equipo != null && linea.Equipo.NumeroSerie != null) label.Text = linea.Equipo.NumeroSerie;
                        else label.Text = string.Empty;
                    }

                    // Modelo
                    label = e.Row.FindControl("lblModelo") as Label;
                    if (label != null) {
                        if (linea.Equipo != null && linea.Equipo.Modelo != null) label.Text = linea.Equipo.Modelo.Nombre;
                        else label.Text = string.Empty;
                    }

                    //Anio
                    label = e.Row.FindControl("lblAnio") as Label;
                    if (label != null) {
                        label.Text = linea.Equipo.Anio != null ? linea.Equipo.Anio.ToString() : string.Empty;
                    }

                    //Tipo Tarifa
                    label = e.Row.FindControl("lblTipoTarifa") as Label;
                    if (label != null) {
                        label.Text = linea.TipoTarifa != null ? linea.TipoTarifa.ToString() : string.Empty;
                    }

                    //Turno
                    label = e.Row.FindControl("lblTurno") as Label;
                    if (label != null) {
                        label.Text = string.Empty;

                        if (linea.Cobrable != null && ((TarifaContratoPSLBO)linea.Cobrable).TarifaTurno != null) {
                            Type type = ((TarifaContratoPSLBO)linea.Cobrable).obtenerETarifaTurno(this.UnidadOperativaID.Value);
                            var memInfo = type.GetMember(type.GetEnumName(((TarifaContratoPSLBO)linea.Cobrable).TarifaTurno));
                            var display = memInfo[0]
                                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                .FirstOrDefault() as DescriptionAttribute;
                            label.Text = display.Description;
                        }
                    }

                    //Maniobra
                    label = e.Row.FindControl("lblManiobra") as Label;
                    if (label != null) {
                        label.Text = string.Empty;
                        if (linea.Cobrable != null && ((TarifaContratoPSLBO)linea.Cobrable).Maniobra != null)
                            label.Text = string.Format("{0:c2}", ((TarifaContratoPSLBO)linea.Cobrable).Maniobra.Value);
                    }

                    //Activo
                    label = e.Row.FindControl("lblActiva") as Label;
                    if (label != null) {
                        label.Text = linea.Activo != null ? linea.Activo.Value ? "SI" : "NO" : string.Empty;
                        label.ForeColor = linea.Activo != null && linea.Activo == false ? Color.Red : label.ForeColor;
                    }

                    //Cambiar color cuando no se hala configurado la tarifa
                    if (linea.Cobrable == null || (linea.Cobrable != null && (((TarifaContratoPSLBO)linea.Cobrable).Tarifa == null || ((TarifaContratoPSLBO)linea.Cobrable).TarifaHrAdicional == null))) {
                        e.Row.Attributes["style"] = "background-color: #DA5554";
                    }
                }
            } catch (Exception ex) {
                MostrarMensaje("Se han encontrado Inconsistencias al presentar el detalle del contrato.",
                               ETipoMensajeIU.ERROR, nombreClase + ".grdLineasContrato_RowDataBound: " + ex.Message);
            }
        }
        protected void btnEliminarTarifas_Click(object sender, EventArgs e) {
            try {
                if (EliminarTarifaLineasClick != null) EliminarTarifaLineasClick.Invoke(sender, e);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al eliminar las tarifas de la línea.", ETipoMensajeIU.ERROR, nombreClase + ".btnEliminarTarifas_Click:" + ex.Message);
            }
        }
        protected void btnCancelarEliminarTarifas_Click(object sender, EventArgs e) {
            string parameter = this.hdnValoresCambiar.Value;
            string[] array = parameter.Split('|');
            string ctrl = array[0];
            string valAnterior = array[1];
            string diasAnterior = array[2];
            switch (ctrl) {
                case "moneda":
                    this.ddlMonedas.SelectedValue = valAnterior;
                    hdnCondicionesMoneda.Value = valAnterior;
                    break;
                case "fechainicio":
                    this.txtCondicionesFechaInicioActual.Text = valAnterior;
                    this.hdnCondicionesFechaInicioActual.Value = valAnterior;
                    this.txtCondicionesDiasRenta.Text = diasAnterior;
                    break;
                case "fechadevolucion":
                    this.txtCondicionesFechaPromesaActual.Text = valAnterior;
                    this.hdnCondicionesFechaPromesaActual.Value = valAnterior;
                    this.txtCondicionesDiasRenta.Text = diasAnterior;
                    break;
                case "fechapagorenta":
                    this.txtROCFechaPagoRenta.Text = valAnterior;
                    this.hdnCondicionesFechaPagoRenta.Value = valAnterior;
                    break;
            }
        }

        protected void btnVerSubtotales_Click(object sender, EventArgs e) {
            if (!presentador.ExistenLineasNoValidas()) {
                if (this.PorcentajeSeguro == null) {
                    this.PorcentajeSeguro = 0;
                }
                this.RegistrarScript("Subtotales", "abrirSubtotales();");
                this.grdMontoUnidades.DataSource = this.presentador.ObtenerMontoContrato().Tables[0];
                this.grdMontoUnidades.DataBind();

            } else
                this.MostrarMensaje("No existen unidades en el contrato o no se ha configurado tarifa a alguna de ellas.", ETipoMensajeIU.ADVERTENCIA);
            
        }
        protected void grdMontoUnidades_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {
                if (e.Row.RowType != DataControlRowType.DataRow) return;
                System.Data.DataRowView bo = e.Row.DataItem != null ? (System.Data.DataRowView)e.Row.DataItem : null;
                if (bo != null) {
                    var label = e.Row.FindControl("lblSerie") as Label;
                    label.Text = bo["Serie"].ToString();
                    label = e.Row.FindControl("lblTarifaBase") as Label;
                    label.Text = string.Format("{0:c2}", bo["TarifaBase"]);
                    label = e.Row.FindControl("lblTarifa") as Label;
                    label.Text = string.Format("{0:c2}", bo["Tarifa"]);
                    label = e.Row.FindControl("lblManiobra") as Label;
                    label.Text = string.Format("{0:c2}", bo["Maniobra"]); 
                    label = e.Row.FindControl("lblSeguro") as Label;
                    label.Text = string.Format("{0:c2}", bo["Seguro"]); 
                    label = e.Row.FindControl("lblMonto") as Label;
                    label.Text = string.Format("{0:c2}", bo["Monto"]);
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al desplegar los montos", ETipoMensajeIU.ERROR, nombreClase + ".grdMontoUnidades_RowDataBound: " + ex.Message);
            }
        }
        protected void txtTarifaInteres_TextChanged(object sender, EventArgs e) {

        }
        #endregion
    }
}