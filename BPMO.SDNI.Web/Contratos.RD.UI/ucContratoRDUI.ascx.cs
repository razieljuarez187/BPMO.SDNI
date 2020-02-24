//Satisface al CU001 - Registrar Contrato de Renta Diaria
//Satisface al CU002 - Editar Contrato Renta Diaria
// BEP1401 Satisface a la SC0026
// BEP1401 Satisface a la SC0034

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.RD.PRE;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Contratos.RD.UI
{
    public partial class ucContratoRDUI : System.Web.UI.UserControl, IucContratoRDVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase en la cual estamos trabajando
        /// </summary>
        private const string nombreClase = "ucContratoRDUI";
        /// <summary>
        /// Enumeración para controlar los objetos del buscador
        /// </summary>
        public enum ECatalogoBuscador
        {
            Sucursal,
            CuentaClienteIdealease,
            DireccionCliente,
            Tarifa,
            Unidad,
            Operador,
            ProductoServicio
        }
        /// <summary>
        /// presentador del UC de información general del contrato de renta diaria
        /// </summary>
        private ucContratoRDPRE presentador;
        #endregion

        #region Propiedades
        /// <summary>
        /// Identificador del contrato
        /// </summary>
        public int? ContratoID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnContratoID.Value) && !string.IsNullOrWhiteSpace(this.hdnContratoID.Value))
                {
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
        public int? UsuarioID
        {
            get
            {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id.Value;
                return null;
            }
        }
        /// <summary>
        /// Obtiene o estabece el estatus del contrato
        /// </summary>
        public int? EstatusID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnEstatusContratoID.Value) && !string.IsNullOrWhiteSpace(this.hdnEstatusContratoID.Value))
                {
                    int val;
                    return Int32.TryParse(this.hdnEstatusContratoID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnEstatusContratoID.Value = value.HasValue ? value.Value.ToString().Trim() : string.Empty; }
        }
        /// <summary>
        /// Identificador del Modulo desde el cual se esta accesando
        /// </summary>
        public int? ModuloID
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                    return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);

                return null;
            }
        }
        /// <summary>
        /// Fecha en la que se ejecuta el contrato
        /// </summary>
        public DateTime? FechaContrato
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtFechaContrato.Text) && !string.IsNullOrWhiteSpace(this.txtFechaContrato.Text))
                {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.txtFechaContrato.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtFechaContrato.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece la hora del contrato de renta diaria
        /// </summary>
        public TimeSpan? HoraContrato
        {
            get
            {
                // SC0034, Se envia por default el tiempo a las cero horas
                return DateTime.MinValue.Date.TimeOfDay;
            }
            set
            {
                
            }
        }
        /// <summary>
        /// Obtiene o establece el número del contrato
        /// </summary>
        public string NumeroContrato
        {
            get
            {
                return !string.IsNullOrEmpty(this.hdnNumeroContrato.Value) && !string.IsNullOrWhiteSpace(this.hdnNumeroContrato.Value)
                    ? this.hdnNumeroContrato.Value.Trim().ToUpper()
                    : string.Empty;
            }
            set { this.hdnNumeroContrato.Value = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value) ? value.Trim().ToUpper() : string.Empty; }
        }
        /// <summary>
        /// Obtiene la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary> 
        public int? UnidadOperativaID
        {
            get
            {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la sucursal de la cual se desean filtrar las unidades
        /// </summary>
        public int? SucursalID
        {
            get {
                int? sucursalID = null;
                if (ddlSucursales.SelectedValue != "0")
                    sucursalID = int.Parse(ddlSucursales.SelectedValue);
                return sucursalID;
            }
            set {
                if (value != null)
                    ddlSucursales.SelectedValue = value.Value.ToString(CultureInfo.InvariantCulture);
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal seleccionada para el filtro de consulta
        /// </summary>
        public string SucursalNombre
        {
            get {
                String sucursalNombre = null;
                if (ddlSucursales.SelectedValue != "0")
                    sucursalNombre = ddlSucursales.SelectedItem.Text;
                return sucursalNombre;
            }
            set {
                ddlSucursales.SelectedItem.Text = !string.IsNullOrEmpty(value) ? value.ToUpper() : "0";
            }
        }
        /// <summary>
        /// Obtiene o establece la dirección de la sucursal a donde será devuelta la unidad
        /// </summary>
        public string DireccionSucursal
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtCondicionesDireccionRegreso.Text)) ? null : this.txtCondicionesDireccionRegreso.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtCondicionesDireccionRegreso.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                    ? String.Format("{0}, {1}", this.NombreEmpresa, value.Trim().ToUpper())
                                                    : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la empresa arrendadora
        /// </summary>
        public string NombreEmpresa
        {
            get
            {
                return (String.IsNullOrEmpty(txtEmpresa.Text)) ? null : this.txtEmpresa.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtEmpresa.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                           ? value.Trim().ToUpper()
                                           : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la dirección de la empresa arrendadora
        /// </summary>
        public string DomicilioEmpresa
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtDireccionEmpresa.Text)) ? null : this.txtDireccionEmpresa.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtDireccionEmpresa.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                    ? value.Trim().ToUpper()
                                                    : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el representante de la empresa arrendadora
        /// </summary>
        public string RepresentanteEmpresa
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtRepresentante.Text)) ? null : this.txtRepresentante.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtRepresentante.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador del cliente
        /// </summary>
        public int? ClienteID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnClienteID.Value) && !string.IsNullOrWhiteSpace(this.hdnClienteID.Value))
                {
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
        public bool? ClienteEsFisica
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnClienteEsFisico.Value) && !string.IsNullOrWhiteSpace(this.hdnClienteEsFisico.Value))
                {
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
        public int? CuentaClienteID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtNumeroCuentaCliente.Text) && !string.IsNullOrWhiteSpace(this.txtNumeroCuentaCliente.Text))
                {
                    int val = 0;
                    return Int32.TryParse(this.txtNumeroCuentaCliente.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.txtNumeroCuentaCliente.Text = value.HasValue
                                                       ? value.Value.ToString(CultureInfo.InvariantCulture).Trim().ToUpper()
                                                       : string.Empty;
            }
        }
        /// <summary>
        /// Número de Cuenta de Oracle CuentaClienteBO.Numero
        /// </summary>
        public String CuentaClienteNumeroCuenta
        {
            get { return String.IsNullOrEmpty(this.txtNumeroCuentaOracle.Text) ? null : this.txtNumeroCuentaOracle.Text; }
            set { this.txtNumeroCuentaOracle.Text = value ?? String.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el nombre del cliente
        /// </summary>
        public string CuentaClienteNombre
        {
            get
            {
                return (String.IsNullOrEmpty(txtNombreCliente.Text)) ? null : this.txtNombreCliente.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtNombreCliente.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el tipo de cuenta del cliente
        /// </summary>
        public int? CuentaClienteTipoID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnTipoCuentaRegion.Value) && !string.IsNullOrWhiteSpace(this.hdnTipoCuentaRegion.Value))
                {
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
        public string ClienteRFC
        {
            get
            {
                return (String.IsNullOrEmpty(txtRFCCliente.Text)) ? null : this.txtRFCCliente.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtRFCCliente.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                              ? value.Trim().ToUpper()
                                              : string.Empty;
            }
        }
        /// <summary>
        /// Id de la Direccion del Cliente
        /// </summary>
        public int? ClienteDireccionClienteID 
        {
            get
            {
                if(!string.IsNullOrEmpty(this.hdnDireccionId.Value) && !string.IsNullOrWhiteSpace(this.hdnDireccionId.Value))
                {
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
        public string ClienteDireccionCompleta
        {
            get
            {
                return (String.IsNullOrEmpty(txtDomicilioCliente.Text)) ? null : this.txtDomicilioCliente.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtDomicilioCliente.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                    ? value.Trim().ToUpper()
                                                    : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la calle del domicilio del cliente
        /// </summary>
        public string ClienteDireccionCalle
        {
            get { return this.hdnCalle.Value.Trim().ToUpper(); }
            set
            {
                this.hdnCalle.Value = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                          ? value.Trim().ToUpper()
                                          : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el codigo postal del domiciio del cliente
        /// </summary>
        public string ClienteDireccionCodigoPostal
        {
            get
            {
                return (String.IsNullOrEmpty(txtCodigoPostal.Text)) ? null : this.txtCodigoPostal.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtCodigoPostal.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la ciudad del domicilio del cliente
        /// </summary>
        public string ClienteDireccionCiudad
        {
            get { return this.hdnCiudad.Value.Trim().ToUpper(); }
            set
            {
                this.hdnCiudad.Value = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el estado de la dirección del cliente
        /// </summary>
        public string ClienteDireccionEstado
        {
            get { return this.hdnEstado.Value.Trim().ToUpper(); }
            set
            {
                this.hdnEstado.Value = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el municipio de la dirección del cliente
        /// </summary>
        public string ClienteDireccionMunicipio
        {
            get { return this.hdnMunicipio.Value.Trim().ToUpper(); }
            set
            {
                this.hdnMunicipio.Value = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el pais de la dirección del cleinte
        /// </summary>
        public string ClienteDireccionPais
        {
            get { return this.hdnPais.Value.Trim().ToUpper(); }
            set
            {
                this.hdnPais.Value = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la colonia de la dirección del clietne
        /// </summary>
        public string ClienteDireccionColonia
        {
            get { return this.hdnColonia.Value.Trim().ToUpper(); }
            set
            {
                this.hdnColonia.Value = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el listado de representantes legales del cliente seleccionado
        /// </summary>
        public List<RepresentanteLegalBO> RepresentantesTotales
        {
            get
            {
                if (Session["ListadoRepresentantesLegales"] != null)
                    return (List<RepresentanteLegalBO>)Session["ListadoRepresentantesLegales"];

                return new List<RepresentanteLegalBO>();
            }
            set
            {
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
        public List<RepresentanteLegalBO> RepresentantesSeleccionados
        {
            get
            {
                List<RepresentanteLegalBO> listValue;
                listValue = Session["RepresentantesLegalesContrato"] != null
                                ? (List<RepresentanteLegalBO>)Session["RepresentantesLegalesContrato"]
                                : new List<RepresentanteLegalBO>();

                return listValue;
            }
            set
            {
                List<RepresentanteLegalBO> listValue = value ?? new List<RepresentanteLegalBO>();
                Session["RepresentantesLegalesContrato"] = listValue;
            }
        }
        /// <summary>
        /// Obtiene el identificador del representante legal seleccionado
        /// </summary>
        public int? RepresentanteLegalSeleccionadoID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ddlRepresentantesLegales.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlRepresentantesLegales.SelectedValue))
                {
                    if (System.String.Compare(this.ddlRepresentantesLegales.SelectedValue, "0", System.StringComparison.Ordinal) != 0)
                    {
                        int val = 0;
                        return Int32.TryParse(this.ddlRepresentantesLegales.SelectedValue, out val) ? (int?)val : null;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// Codigo de la moneda selecionada
        /// </summary>
        public string CodigoMoneda
        {
            get
            {
                if (this.ddlMonedas.SelectedValue.CompareTo("0") != 0)
                    return this.ddlMonedas.SelectedValue.Trim().ToUpper();
                return null;
            }
            set
            {
                ddlMonedas.SelectedValue = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                               ? value.Trim().ToUpper()
                                               : "0";
            }
        }
        /// <summary>
        /// Obtiene o establece el tipo de autorización para el pago a crédito
        /// </summary>
        public int? TipoConfirmacionID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ddlTipoPagoCredito.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlTipoPagoCredito.SelectedValue))
                {
                    int val = 0;
                    return Int32.TryParse(this.ddlTipoPagoCredito.SelectedValue, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.ddlTipoPagoCredito.SelectedValue = value.Value.ToString().Trim();
                else
                    this.ddlTipoPagoCredito.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Obtiene o establece el tipo de lector de kilometraje de al unidad
        /// </summary>
        public int? LectorKilometrajeID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ddlLectorKilometraje.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlLectorKilometraje.SelectedValue))
                {
                    int val = 0;
                    return Int32.TryParse(this.ddlLectorKilometraje.SelectedValue, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.ddlLectorKilometraje.SelectedValue = value.Value.ToString().Trim();
            }
        }
        /// <summary>
        /// Obtiene o establece la forma de pago del contrato
        /// </summary>
        public int? FormaPagoID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ddlFormaPago.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlFormaPago.SelectedValue))
                {
                    int val = 0;
                    return Int32.TryParse(this.ddlFormaPago.SelectedValue, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.ddlFormaPago.SelectedValue = value.Value.ToString().Trim();
            }
        }
        /// <summary>
        /// Obtiene o establece el motivo de la renta de la unidad
        /// </summary>
        public int? MotivoRentaID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ddlCondicionesMotivoRenta.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlCondicionesMotivoRenta.SelectedValue))
                {
                    int val = 0;
                    return Int32.TryParse(this.ddlCondicionesMotivoRenta.SelectedValue, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.ddlCondicionesMotivoRenta.SelectedValue = value.Value.ToString().Trim();
                else
                    this.ddlCondicionesMotivoRenta.SelectedIndex = -1;
            }
        }
        /// <summary>
        /// Obtiene o establece la frecuencia de facturación para el contrato
        /// </summary>
        public int? FrecuenciaFacturacionID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ddlFrecuenciaFacturacion.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlFrecuenciaFacturacion.SelectedValue))
                {
                    int val = 0;
                    return Int32.TryParse(this.ddlFrecuenciaFacturacion.SelectedValue, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                    this.ddlFrecuenciaFacturacion.SelectedValue = value.Value.ToString().Trim();
            }
        }
        /// <summary>
        /// Nombre de la persona que autoriza el pago a crédito
        /// </summary>
        public string AutorizadorTipoConfirmacion
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtPersonaAutorizaCredito.Text)) ? null : this.txtPersonaAutorizaCredito.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtPersonaAutorizaCredito.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                          ? value.Trim().ToUpper()
                                                          : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la persona que autoriza la orden de compra
        /// </summary>
        public string AutorizadorOrdenCompra
        {
            get
            {
                return (String.IsNullOrEmpty(txtPersonaAutorizaOrdenCompra.Text)) ? null : this.txtPersonaAutorizaOrdenCompra.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtPersonaAutorizaOrdenCompra.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                          ? value.Trim().ToUpper()
                                                          : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la mercancia que se va a transportar en la unidad
        /// </summary>
        public string MercanciaTransportar
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtCondicionesMercancia.Text)) ? null : this.txtCondicionesMercancia.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtCondicionesMercancia.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el destino o área de operación para la unidad
        /// </summary>
        public string DestinoAreaOperacion
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtCondicionesAreaOperacion.Text)) ? null : this.txtCondicionesAreaOperacion.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtCondicionesAreaOperacion.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Porcentaje Deducible para el seguro de la unidad
        /// </summary>
        public decimal? PorcentajeDeducible
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnUnidadPorcentajeDeducible.Value) && !string.IsNullOrWhiteSpace(this.hdnUnidadPorcentajeDeducible.Value))
                {
                    decimal val = 0;
                    return decimal.TryParse(this.hdnUnidadPorcentajeDeducible.Value, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.hdnUnidadPorcentajeDeducible.Value = value.HasValue ? value.Value.ToString().Trim() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece si tiene habilitada la bitacora de viaje para el conductor
        /// </summary>
        public bool? BitacoraViajeConductor
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ddlBitacoraViaje.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlBitacoraViaje.SelectedValue))
                {
                    switch (this.ddlBitacoraViaje.SelectedValue)
                    {
                        case "0":
                            return false;
                        case "1":
                            return true;
                        default:
                            return null;
                    }
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.ddlBitacoraViaje.SelectedValue = value.Value ? "1" : "0";
                }
            }
        }
        /// <summary>
        /// Obtiene o establece la fecha de promesa de devolución de la unidad
        /// </summary>
        public DateTime? FechaPromesaDevolucion
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtCondicionesFechaDevolucion.Text) && !string.IsNullOrWhiteSpace(this.txtCondicionesFechaDevolucion.Text))
                {
                    DateTime date;
                    return DateTime.TryParse(this.txtCondicionesFechaDevolucion.Text, out date)
                               ? (DateTime?)date
                               : null;
                }
                return null;
            }
            set { this.txtCondicionesFechaDevolucion.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece la hora de promesa de devolución
        /// </summary>
        public TimeSpan? HoraPromesaDevolucion
        {
            get
            {
                // SC0034, Se envia por default el tiempo a las cero horas
                return DateTime.MinValue.Date.TimeOfDay;
            }
            set
            {
                
            }
        }
        /// <summary>
        /// Obtiene o establece las observaciones realizadas al contrato
        /// </summary>
        public string Observaciones
        {
            get { return (String.IsNullOrEmpty(this.txtObservaciones.Text)) ? null : this.txtObservaciones.Text.Trim().ToUpper(); } //RI0061
            set
            {
                this.txtObservaciones.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece los días de renta
        /// </summary>
        public int? DiasRenta
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtCondicionesDiasRenta.Text) && !string.IsNullOrWhiteSpace(this.txtCondicionesDiasRenta.Text))
                {
                    int val;
                    return Int32.TryParse(this.txtCondicionesDiasRenta.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.txtCondicionesDiasRenta.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        #region SC_0013
        /// <summary>
        /// Obtiene o establece el identificador del operador
        /// </summary>
        public int? OperadorID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnOperadorID.Value) && !string.IsNullOrWhiteSpace(this.hdnOperadorID.Value))
                {
                    int val;
                    return Int32.TryParse(this.hdnOperadorID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.hdnOperadorID.Value = value.HasValue ? value.Value.ToString() : string.Empty;
            }
        }
        ///<summary>
        ///Obtiene o establece la cuenta del cliente del operador
        /// </summary>
        public int? OperadorCuentaClienteID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnOperadorCuentaClienteID.Value) && !string.IsNullOrWhiteSpace(this.hdnOperadorCuentaClienteID.Value))
                {
                    int val;
                    return Int32.TryParse(this.hdnOperadorCuentaClienteID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnOperadorCuentaClienteID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        #endregion
        #region BEP1401.SC_0026
        /// <summary>
        /// Cantidad de días para la primera Factura
        /// </summary>
        public Int32? DiasFacturar 
        {
            get 
            {
                if(!String.IsNullOrEmpty(this.txtDiasFacturar.Text) && !String.IsNullOrWhiteSpace(this.txtDiasFacturar.Text))
                {
                    int val;
                    return Int32.TryParse(this.txtDiasFacturar.Text, out val) ? (Int32?)val : null;
                }
                return null;
            } 
            set { this.txtDiasFacturar.Text = value.HasValue ? value.ToString() : String.Empty; }
        }
        #endregion

        /// <summary>
        /// Obtiene o establece el nombre del operador
        /// </summary>
        public string OperadorNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtOperadorNombre.Text)) ? null : this.txtOperadorNombre.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtOperadorNombre.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la fecha de nacimiento del operador
        /// </summary>
        public DateTime? OperadorFechaNacimiento
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtOperadorFechaNacimiento.Text) && !string.IsNullOrWhiteSpace(this.txtOperadorFechaNacimiento.Text))
                {
                    DateTime date;
                    return DateTime.TryParse(this.txtOperadorFechaNacimiento.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtOperadorFechaNacimiento.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece los años de experiencia del operador
        /// </summary>
        public int? OperadorAniosExperiencia
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtOperadorAniosExperiencia.Text) && !string.IsNullOrWhiteSpace(this.txtOperadorAniosExperiencia.Text))
                {
                    int val = 0;
                    return Int32.TryParse(this.txtOperadorAniosExperiencia.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.txtOperadorAniosExperiencia.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el la calle de la dirección del operador
        /// </summary>
        public string OperadorDireccionCalle
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtOperadorCalle.Text)) ? null : this.txtOperadorCalle.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtOperadorCalle.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la ciudad de la dirección del operador
        /// </summary>
        public string OperadorDireccionCiudad
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtOperadorCiudad.Text)) ? null : this.txtOperadorCiudad.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtOperadorCiudad.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el estado de la dirección del operador
        /// </summary>
        public string OperadorDireccionEstado
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtOperadorEstado.Text)) ? null : this.txtOperadorEstado.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtOperadorEstado.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el codigo postal de la dirección del operador
        /// </summary>
        public string OperadorDireccionCP
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtOperadorCodigoPostal.Text)) ? null : this.txtOperadorCodigoPostal.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtOperadorCodigoPostal.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el tipo de la licencia del operador
        /// </summary>
        public int? OperadorLicenciaTipoID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ddlTipoLicencia.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlTipoLicencia.SelectedValue))
                {
                    int val = 0;
                    return Int32.TryParse(this.ddlTipoLicencia.SelectedValue, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.ddlTipoLicencia.SelectedValue = value.Value.ToString().Trim();
                }
            }
        }
        /// <summary>
        /// Obtiene o establece el número de la licencia del operador
        /// </summary>
        public string OperadorLicenciaNumero
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtLicenciaNumero.Text)) ? null : this.txtLicenciaNumero.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtLicenciaNumero.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Fecha de expiración de la licencia
        /// </summary>
        public DateTime? OperadorLicenciaFechaExpiracion
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtLicenciaFechaExpiracion.Text) && !string.IsNullOrWhiteSpace(this.txtLicenciaFechaExpiracion.Text))
                {
                    DateTime date;
                    return DateTime.TryParse(this.txtLicenciaFechaExpiracion.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtLicenciaFechaExpiracion.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el estado de expedición de la licencia
        /// </summary>
        public string OperadorLicenciaEstado
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtLicenciaEstadoExpedicion.Text)) ? null : this.txtLicenciaEstadoExpedicion.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtLicenciaEstadoExpedicion.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la unidad
        /// </summary>
        public int? UnidadID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnUnidadID.Value) && !string.IsNullOrWhiteSpace(this.hdnUnidadID.Value))
                {
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
        public int? EquipoID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnEquipoID.Value) && !string.IsNullOrWhiteSpace(this.hdnEquipoID.Value))
                {
                    int val;
                    return Int32.TryParse(this.hdnEquipoID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnEquipoID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el número de serie de la unidad que será rentada
        /// </summary>
        public string VIN
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadNumeroSerie.Text)) ? null : this.txtUnidadNumeroSerie.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtUnidadNumeroSerie.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el número económico de la unidad que será rentada
        /// </summary>
        public string NumeroEconomico
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadNumeroEconomico.Text)) ? null : this.txtUnidadNumeroEconomico.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtUnidadNumeroEconomico.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador del modelo
        /// </summary>
        public int? ModeloID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnModeloID.Value) && !string.IsNullOrWhiteSpace(this.hdnModeloID.Value))
                {
                    int val;
                    return Int32.TryParse(this.hdnModeloID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnModeloID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el nombre del modelo de la unidad
        /// </summary>
        public string ModeloNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadModelo.Text)) ? null : this.txtUnidadModelo.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtUnidadModelo.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la marca de la unidad
        /// </summary>
        public string MarcaNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadMarca.Text)) ? null : this.txtUnidadMarca.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtUnidadMarca.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el año de la unidad
        /// </summary>
        public int? UnidadAnio
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtUnidadAnio.Text) && !string.IsNullOrWhiteSpace(this.txtUnidadAnio.Text))
                {
                    int val;
                    return Int32.TryParse(this.txtUnidadAnio.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.txtUnidadAnio.Text = value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal de la unidad
        /// </summary>
        public string UnidadSucursalNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadSucursal.Text)) ? null : this.txtUnidadSucursal.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtUnidadSucursal.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el número de las placas estatales de la unidad
        /// </summary>
        public string UnidadPlacaEstatal
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadPlacasEstales.Text)) ? null : this.txtUnidadPlacasEstales.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtUnidadPlacasEstales.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el número de las placas federales de la unidad
        /// </summary>
        public string UnidadPlacaFederal
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadPlacasFederales.Text)) ? null : this.txtUnidadPlacasFederales.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtUnidadPlacasFederales.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Capacidad de carga máxima de la unidad
        /// </summary>
        public decimal? UnidadPBC
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtUnidadCapacidadCarga.Text) && !string.IsNullOrWhiteSpace(this.txtUnidadCapacidadCarga.Text))
                {
                    decimal val;
                    return decimal.TryParse(this.txtUnidadCapacidadCarga.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtUnidadCapacidadCarga.Text = value.HasValue ? string.Format("{0:#,##0.00##}", value.Value) : string.Empty; }
        }
        /// <summary>
        /// Capacidad del tanque de combustible de la unidad
        /// </summary>
        public decimal? UnidadCapacidadTanque
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtUnidadCapacidadTanque.Text) && !string.IsNullOrWhiteSpace(this.txtUnidadCapacidadTanque.Text))
                {
                    decimal val;
                    return decimal.TryParse(this.txtUnidadCapacidadTanque.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtUnidadCapacidadTanque.Text = value.HasValue ? string.Format("{0:#,##0.00##}", value.Value) : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el rendimiento del tanque de combustible de la unidad
        /// </summary>
        public decimal? UnidadRendimientoTanque
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtUnidadRendimientoTanque.Text) && !string.IsNullOrWhiteSpace(this.txtUnidadRendimientoTanque.Text))
                {
                    decimal val;
                    return decimal.TryParse(this.txtUnidadRendimientoTanque.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtUnidadRendimientoTanque.Text = value.HasValue ? string.Format("{0:#,##0.00##}", value.Value) : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el número de poliza del seguro de la unidad
        /// </summary>
        public string UnidadNumeroPoliza
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadNumeroPoliza.Text)) ? null : this.txtUnidadNumeroPoliza.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtUnidadNumeroPoliza.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la compañia de seguros de la unidad
        /// </summary>
        public string UnidadAseguradora
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadCompaniaSeguro.Text)) ? null : this.txtUnidadCompaniaSeguro.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtUnidadCompaniaSeguro.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el importe del deducible del seguro
        /// </summary>
        public decimal? UnidadMontoDeducible
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtUnidadImporteDeducible.Text) && !string.IsNullOrWhiteSpace(this.txtUnidadImporteDeducible.Text))
                {
                    decimal val;
                    return decimal.TryParse(this.txtUnidadImporteDeducible.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtUnidadImporteDeducible.Text = value.HasValue ? string.Format("{0:#,##0.00##}", value.Value) : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el importe del deposito que debe dejar el cliente por la renta de la unidad
        /// </summary>
        public decimal? UnidadMontoDeposito
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtCondicionesImporteDeposito.Text) && !string.IsNullOrWhiteSpace(this.txtCondicionesImporteDeposito.Text))
                {
                    decimal val;
                    return decimal.TryParse(this.txtCondicionesImporteDeposito.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtCondicionesImporteDeposito.Text = value.HasValue ? string.Format("{0:#,##0.00##}", value.Value) : string.Empty; }
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
        /// Obtiene o establece el identificador de la tarifa que aplicará a la unidad
        /// </summary>
        public int? TarifaID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnTarifaID.Value) && !string.IsNullOrWhiteSpace(this.hdnTarifaID.Value))
                {
                    int val;
                    return Int32.TryParse(this.hdnTarifaID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnTarifaID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece la descripción de la tarifa seleccionada
        /// </summary>
        public string TarifaDescripcion
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtTarifaDescripcion.Text)) ? null : this.txtTarifaDescripcion.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtTarifaDescripcion.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el tipo de tarifa que apicará al contrato
        /// </summary>
        public int? TipoTarifaID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ddlTipoTarifa.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlTipoTarifa.SelectedValue))
                {
                    int val;
                    return Int32.TryParse(this.ddlTipoTarifa.SelectedValue, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.ddlTipoTarifa.SelectedValue = value.Value.ToString(CultureInfo.InvariantCulture);
                }
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la sucursal de la tarifa
        /// </summary>
        public int? TarifaSucursalID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnTarifaSucursalID.Value) && !string.IsNullOrWhiteSpace(this.hdnTarifaSucursalID.Value))
                {
                    int val;
                    return Int32.TryParse(this.hdnTarifaSucursalID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnTarifaSucursalID.Value = value.HasValue ? value.Value.ToString() : null; }
        }
        /// <summary>
        /// Obtiene o establece el identificador del modelo de la tarifa
        /// </summary>
        public int? TarifaModeloID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnTarifaModeloID.Value) && !string.IsNullOrWhiteSpace(this.hdnTarifaModeloID.Value))
                {
                    int val;
                    return Int32.TryParse(this.hdnTarifaModeloID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnTarifaModeloID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece la unidad operativa de la tarifa
        /// </summary>
        public int? TarifaUnidadOperativaID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnTarifaUnidadOperativaID.Value) && !string.IsNullOrWhiteSpace(this.hdnTarifaUnidadOperativaID.Value))
                {
                    int val;
                    return Int32.TryParse(this.hdnTarifaUnidadOperativaID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnTarifaUnidadOperativaID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece la cuenta del cliente de la tarifa
        /// </summary>
        public int? TarifaCuentaClienteID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnTarifaCuentaClienteID.Value) && !string.IsNullOrWhiteSpace(this.hdnTarifaCuentaClienteID.Value))
                {
                    int val;
                    return Int32.TryParse(this.hdnTarifaCuentaClienteID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnTarifaCuentaClienteID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el codigo de la moneda para la tarifa
        /// </summary>
        public string TarifaCodigoMoneda
        {
            get { return hdnTarifaCodigoMoneda.Value.Trim().ToUpper(); }
            set
            {
                this.hdnTarifaCodigoMoneda.Value = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la capacidad de carga para la tarifa seleccionada
        /// </summary>
        public int? CapacidadCarga
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtTarifaCarga.Text) && !string.IsNullOrWhiteSpace(this.txtTarifaCarga.Text))
                {
                    int val;
                    return Int32.TryParse(this.txtTarifaCarga.Text.Replace(",", ""), out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.txtTarifaCarga.Text = value.HasValue ? string.Format("{0:#,##0}", value.Value) : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el importe de la tarifa diaria
        /// </summary>
        public decimal? TarifaDiaria
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtTarifaDiaria.Text) && !string.IsNullOrWhiteSpace(this.txtTarifaDiaria.Text))
                {
                    decimal val;
                    return Decimal.TryParse(this.txtTarifaDiaria.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtTarifaDiaria.Text = value.HasValue ? string.Format("{0:#,##0.00##}", value.Value) : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece los kilometros libres para la tarifa seleccionada
        /// </summary>
        public int? KmsLibres
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtTarifaKMLibres.Text) && !string.IsNullOrEmpty(this.txtTarifaKMLibres.Text))
                {
                    int val;
                    return Int32.TryParse(this.txtTarifaKMLibres.Text.Replace(",", ""), out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.txtTarifaKMLibres.Text = value.HasValue ? string.Format("{0:#,##0}", value.Value) : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el importe por kilometro adicional para la tarifa seleccionada
        /// </summary>
        public decimal? TarifaKmAdicional
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtTarifaKMAdicional.Text) && !string.IsNullOrEmpty(this.txtTarifaKMAdicional.Text))
                {
                    decimal val;
                    return decimal.TryParse(this.txtTarifaKMAdicional.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtTarifaKMAdicional.Text = value.HasValue ? string.Format("{0:#,##0.00##}", value.Value) : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece las horas libres para la tarifa seleccionada
        /// </summary>
        public int? HrsLibres
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtTarifaHorasLibres.Text) && !string.IsNullOrEmpty(this.txtTarifaHorasLibres.Text))
                {
                    int val;
                    return Int32.TryParse(this.txtTarifaHorasLibres.Text.Replace(",", ""), out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.txtTarifaHorasLibres.Text = value.HasValue ? string.Format("{0:#,##0}", value.Value) : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el importe por hora adicional en la tarifa seleccionada
        /// </summary>
        public decimal? TarifaHrAdicional
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtTarifaHoraAdicional.Text) && !string.IsNullOrEmpty(this.txtTarifaHoraAdicional.Text))
                {
                    decimal val;
                    return decimal.TryParse(this.txtTarifaHoraAdicional.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set
            {
                this.txtTarifaHoraAdicional.Text = value.HasValue ? string.Format("{0:#,##0.00##}", value.Value) : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el porcentaje de pago por cargo de posfactura
        /// </summary>
        public decimal? PorcentajePagoPostFactura
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtCargosPorcentajeFacturacion.Text) && !string.IsNullOrEmpty(this.txtCargosPorcentajeFacturacion.Text))
                {
                    decimal val;
                    return decimal.TryParse(this.txtCargosPorcentajeFacturacion.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtCargosPorcentajeFacturacion.Text = value.HasValue ? string.Format("{0:#,##0.00##}", value.Value) : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece los días de pago  post factura
        /// </summary>
        public int? DiasPagoPostFactura
        {
            get
            {
                if (!string.IsNullOrEmpty(this.lblDiasPago.Text) && !string.IsNullOrEmpty(this.lblDiasPago.Text))
                {
                    int val;
                    return Int32.TryParse(this.lblDiasPago.Text.Replace(",", ""), out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.lblDiasPago.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el cargo por hora adicional de posecion de la unidad
        /// </summary>
        public decimal? CargoHoraPosecion
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtCargosHoraPosesion.Text) && !string.IsNullOrEmpty(this.txtCargosHoraPosesion.Text))
                {
                    decimal val;
                    return decimal.TryParse(this.txtCargosHoraPosesion.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtCargosHoraPosesion.Text = value.HasValue ? string.Format("{0:#,##0.00##}", value.Value) : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el cargo por daño al medidor de kilometraje
        /// </summary>
        public decimal? CargoAlteracionMedidorKm
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtCargosAlteracionMedidor.Text) && !string.IsNullOrEmpty(this.txtCargosAlteracionMedidor.Text))
                {
                    decimal val;
                    return decimal.TryParse(this.txtCargosAlteracionMedidor.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtCargosAlteracionMedidor.Text = value.HasValue ? string.Format("{0:#,##0.00##}", value.Value) : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el cargo por entrega inpuntual de la unidad
        /// </summary>
        public decimal? CargoEntregaInpuntual
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtCargosEntregaInpuntual.Text) && !string.IsNullOrEmpty(this.txtCargosEntregaInpuntual.Text))
                {
                    decimal val;
                    return decimal.TryParse(this.txtCargosEntregaInpuntual.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtCargosEntregaInpuntual.Text = value.HasValue ? string.Format("{0:#,##0.00##}", value.Value) : string.Empty; }
        }

        /// <summary>
        /// Obtiene o Establece el código de autorización
        /// </summary>
        public string TarifaPersonalizadaCodigoAutorizacion
        {
            get
            {
                return string.IsNullOrEmpty(hdnCodigoAutorizacion.Value.Trim()) ? null : hdnCodigoAutorizacion.Value;
            }
            set
            {
                hdnCodigoAutorizacion.Value = value ?? "";
                txtTarifaPersonalizadaCodigoAutorizacion.Text = "";
            }
        }
        /// <summary>
        /// Obtiene o establece el tipo de tarifa personalizada
        /// </summary>
        public int? TarifaPersonalizadaTipoTarifaID
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlTarifaPersonalizadaTipoTarifa.SelectedValue.Trim()))
                {
                    int val;
                    return Int32.TryParse(ddlTarifaPersonalizadaTipoTarifa.SelectedValue.Trim(), out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    ddlTarifaPersonalizadaTipoTarifa.SelectedValue = value.Value.ToString(CultureInfo.InvariantCulture);
                }
            }
        }
        /// <summary>
        /// Obtiene o establece la capacidad de carga de la tarifa personalizada
        /// </summary>
        public int? TarifaPersonalizadaCapacidadCarga
        {
            get
            {
                if (!string.IsNullOrEmpty(txtTarifaPersonalizadaCapacidadCarga.Text.Trim()))
                {
                    int val;
                    return Int32.TryParse(this.txtTarifaPersonalizadaCapacidadCarga.Text.Replace(",", ""), out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                txtTarifaPersonalizadaCapacidadCarga.Text = value.HasValue ? string.Format("{0:#,##0}", value.Value) : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la tarifa diaria para la tarifa personalizada
        /// </summary>
        public decimal? TarifaPersonalizadaTarifaDiaria
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtTarifaPersonalizadaTarifaDiaria.Text.Trim()))
                {
                    decimal val;
                    return decimal.TryParse(this.txtTarifaPersonalizadaTarifaDiaria.Text.Trim(), out val) ? (decimal?)val : null;
                }
                return null;
            }
            set
            {
                txtTarifaPersonalizadaTarifaDiaria.Text = value.HasValue ? string.Format("{0:#,##0.00##}", value.Value) : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece los km libres de la tarifa personalizada
        /// </summary>
        public int? TarifaPersonalizadaKmsLibres
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtTarifaPersonalizadaKmsLibres.Text.Trim()))
                {
                    int val;
                    return Int32.TryParse(this.txtTarifaPersonalizadaKmsLibres.Text.Trim().Replace(",", ""), out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                txtTarifaPersonalizadaKmsLibres.Text = value.HasValue ? string.Format("{0:#,##0}", value.Value) : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la tarifa por km adicional de la tarifa personalizada
        /// </summary>
        public decimal? TarifaPersonalizadaTarifaKmAdicional
        {
            get
            {
                if (!string.IsNullOrEmpty(txtTarifaPersonalizadaTarifaKmAdicional.Text.Trim()))
                {
                    decimal val;
                    return decimal.TryParse(this.txtTarifaPersonalizadaTarifaKmAdicional.Text.Trim(), out val) ? (decimal?)val : null;
                }
                return null;
            }
            set
            {
                txtTarifaPersonalizadaTarifaKmAdicional.Text = value.HasValue ? string.Format("{0:#,##0.00##}", value.Value) : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece las hrs libres de la tarifa personalizada
        /// </summary>
        public int? TarifaPersonalizadaHrsLibres
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtTarifaPersonalizadaHrsLibres.Text.Trim()))
                {
                    int val;
                    return Int32.TryParse(this.txtTarifaPersonalizadaHrsLibres.Text.Trim().Replace(",", ""), out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                txtTarifaPersonalizadaHrsLibres.Text = value.HasValue ? string.Format("{0:#,##0}", value.Value) : string.Empty;
            }
        }
        /// <summary>
        /// obtiene o establece la tarifa por hr adicional de la tarifa personalizada
        /// </summary>
        public decimal? TarifaPersonalizadaTarifaHrAdicional
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtTarifaPersonalizadaTarifaHrAdicional.Text.Trim()))
                {
                    decimal val;
                    return decimal.TryParse(this.txtTarifaPersonalizadaTarifaHrAdicional.Text.Trim(), out val) ? (decimal?)val : null;
                }
                return null;
            }
            set
            {
                txtTarifaPersonalizadaTarifaHrAdicional.Text = value.HasValue ? string.Format("{0:#,##0.00##}", value.Value) : string.Empty;

            }
        }

        #region Propiedades para el Buscador
        public string ViewState_Guid
        {
            get
            {
                if (ViewState["GuidSession"] == null)
                {
                    Guid guid = Guid.NewGuid();
                    ViewState["GuidSession"] = guid.ToString();
                }
                return ViewState["GuidSession"].ToString();
            }
        }
        protected object Session_BOSelecto
        {
            get
            {
                object objeto = null;
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (Session[nombreSession] != null)
                    objeto = (Session[nombreSession] as object);

                return objeto;
            }
            set
            {
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (value != null)
                    Session[nombreSession] = value;
                else
                    Session.Remove(nombreSession);
            }
        }
        protected object Session_ObjetoBuscador
        {
            get
            {
                object objeto = null;
                if (Session[ViewState_Guid] != null)
                    objeto = (Session[ViewState_Guid] as object);

                return objeto;
            }
            set
            {
                if (value != null)
                    Session[ViewState_Guid] = value;
                else
                    Session.Remove(ViewState_Guid);
            }
        }
        public ECatalogoBuscador ViewState_Catalogo
        {
            get
            {
                return (ECatalogoBuscador)ViewState["BUSQUEDA"];
            }
            set
            {
                ViewState["BUSQUEDA"] = value;
            }
        }
        #endregion

		/// <summary>
		/// Obtiene o establece el listado de avales del cliente seleccionado
		/// </summary>
		public List<AvalBO> AvalesTotales
		{
			get
			{
				if (Session["ListadoAvalesRD"] != null)
					return (List<AvalBO>)Session["ListadoAvalesRD"];

				return new List<AvalBO>();
			}
			set
			{
				List<AvalBO> lst = value ?? new List<AvalBO>();

				//Se sube a la sesión
				Session["ListadoAvalesRD"] = new List<AvalBO>(lst);
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
		public List<AvalBO> AvalesSeleccionados
		{
			get
			{
				List<AvalBO> lst = new List<AvalBO>();
				if (Session["AvalesContratoRD"] != null)
					lst = (List<AvalBO>)Session["AvalesContratoRD"];

				return lst;
			}
			set
			{
				List<AvalBO> lst = value ?? new List<AvalBO>();
				Session["AvalesContratoRD"] = lst;
			}
		}
		/// <summary>
		/// Obtiene el identificador del aval seleccionado
		/// </summary>
		public int? AvalSeleccionadoID
		{
			get
			{
				if (!string.IsNullOrEmpty(this.ddlAvales.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlAvales.SelectedValue))
				{
					if (System.String.Compare(this.ddlAvales.SelectedValue, "0", System.StringComparison.Ordinal) != 0)
					{
						int val = 0;
						return Int32.TryParse(this.ddlAvales.SelectedValue, out val) ? (int?)val : null;
					}
				}
				return null;
			}
		}
		public List<RepresentanteLegalBO> RepresentantesAvalTotales
		{
			get
			{
				if (Session["ListaRepresentantesAvalRD"] == null)
					return new List<RepresentanteLegalBO>();
				return (List<RepresentanteLegalBO>)Session["ListaRepresentantesAvalRD"];
			}
			set
			{
				List<RepresentanteLegalBO> lst = value ?? new List<RepresentanteLegalBO>();

				Session["ListaRepresentantesAvalRD"] = lst;
				this.grdRepresentantesAval.DataSource = lst;
				this.grdRepresentantesAval.DataBind();
			}
		}
		public List<RepresentanteLegalBO> RepresentantesAvalSeleccionados
		{
			get { return (List<RepresentanteLegalBO>)Session["RepresentantesAvalRD"]; }
			set
			{
				List<RepresentanteLegalBO> lst = value ?? new List<RepresentanteLegalBO>();

				Session["RepresentantesAvalRD"] = lst;
			}
		}
		public int? RepresentanteAvalSeleccionadoID
		{
			get
			{
				if (!string.IsNullOrEmpty(this.hdnRepresentanteAvalSeleccionadoID.Value) && !string.IsNullOrWhiteSpace(this.hdnRepresentanteAvalSeleccionadoID.Value))
				{
					int val = 0;
					return Int32.TryParse(this.hdnRepresentanteAvalSeleccionadoID.Value, out val) ? (int?)val : null;
				}
				return null;
			}
		}
		public bool? SoloRepresentantes
		{
			get
			{
				return this.cbSoloRepresentantes.Checked;
			}
			set
			{
				if (value != null)
					this.cbSoloRepresentantes.Checked = value.Value;
				else
					this.cbSoloRepresentantes.Checked = false;
			}
		}
        /// <summary>
        /// Obtiene o estable la lista de sucursales autorizadas al usuario r
        /// </summary>
        public List<SucursalBO> SucursalesAutorizadas {
            get {
                return (List<SucursalBO>)ddlSucursales.DataSource;
            }
            set {
                List<SucursalBO> lista =  value != null ? new List<SucursalBO>(value) : new List<SucursalBO>();
               
                if (lista!=null && lista.Count > 1 )
                    lista.Insert(0, new SucursalBO { Id = 0, Nombre = "SELECCIONE SUCURSAL" });

                ddlSucursales.Items.Clear();
                ddlSucursales.DataTextField = "Nombre";
                ddlSucursales.DataValueField = "Id";
                ddlSucursales.DataSource = lista;
                ddlSucursales.DataBind();
            }
        }
        /// <summary>
        /// Obtiene la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary> 
        public UnidadOperativaBO UnidadOperativa {
            get {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa : null;
            }
            set {
                Site masterMsj = (Site)Page.Master;
                if (masterMsj == null && masterMsj.Adscripcion == null &&  masterMsj.Adscripcion.UnidadOperativa == null)
                       if( masterMsj.Adscripcion == null)
                            masterMsj.Adscripcion = new AdscripcionBO();
                        masterMsj.Adscripcion.UnidadOperativa = new UnidadOperativaBO();

                masterMsj.Adscripcion.UnidadOperativa =  value ?? value;
            }
        }
        #endregion

        #region Constructor
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ucContratoRDPRE(this);
                if (!IsPostBack){
                    if (SucursalesAutorizadas == null)
                    SucursalesAutorizadas = (List<SucursalBO>)Session["SucursalesAutorizadas"]; 
                }
                this.txtObservaciones.Attributes.Add("onkeyup", "checkText(this,300);");
            }
            catch (Exception ex)
            {
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
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            }
            else
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// Habilita la pantalla para el registro de un nuevo contrato
        /// </summary>
        public void PrepararNuevo()
        {
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
            this.hdnModeloID.Value = string.Empty;
            this.hdnMunicipio.Value = string.Empty;
            this.hdnNumeroContrato.Value = string.Empty;
            this.hdnPais.Value = string.Empty;
            this.hdnTarifaCodigoMoneda.Value = string.Empty;
            this.hdnTarifaCuentaClienteID.Value = string.Empty;
            this.hdnTarifaID.Value = string.Empty;
            this.hdnTarifaModeloID.Value = string.Empty;
            this.hdnTarifaSucursalID.Value = string.Empty;
            this.hdnTarifaUnidadOperativaID.Value = string.Empty;
            this.hdnTipoCuentaRegion.Value = string.Empty;
            this.hdnUnidadID.Value = string.Empty;
            this.hdnUnidadPorcentajeDeducible.Value = string.Empty;
            #region SC_0013
            this.hdnOperadorID.Value = string.Empty;
            this.hdnOperadorCuentaClienteID.Value = string.Empty;
            #endregion
            this.hdnProductoServicioId.Value = string.Empty;
            #endregion

            #region Limpiamos todos los textbox
            this.txtCargosAlteracionMedidor.Text = string.Empty;
            this.txtCargosEntregaInpuntual.Text = string.Empty;
            this.txtCargosHoraPosesion.Text = string.Empty;
            this.txtCargosPorcentajeFacturacion.Text = string.Empty;
            this.txtCodigoPostal.Text = string.Empty;
            this.txtCondicionesAreaOperacion.Text = string.Empty;
            this.txtCondicionesDiasRenta.Text = string.Empty;
            this.txtCondicionesDireccionRegreso.Text = string.Empty;
            this.txtCondicionesFechaDevolucion.Text = string.Empty;
            this.txtCondicionesImporteDeposito.Text = string.Empty;
            this.txtCondicionesMercancia.Text = string.Empty;
            this.txtDiasFacturar.Text = String.Empty; //BEP1401.SC0026
            this.txtDireccionEmpresa.Text = string.Empty;
            this.txtDomicilioCliente.Text = string.Empty;
            this.txtEmpresa.Text = string.Empty;
            this.txtFechaContrato.Text = string.Empty;
            this.txtHoraContrato.Text = string.Empty;
            this.txtLicenciaEstadoExpedicion.Text = string.Empty;
            this.txtLicenciaFechaExpiracion.Text = string.Empty;
            this.txtLicenciaNumero.Text = string.Empty;
            this.txtNombreCliente.Text = string.Empty;
            this.txtNumeroCuentaCliente.Text = string.Empty;
            this.txtNumeroCuentaOracle.Text = string.Empty;
            this.txtObservaciones.Text = string.Empty;
            this.txtOperadorAniosExperiencia.Text = string.Empty;
            this.txtOperadorCalle.Text = string.Empty;
            this.txtOperadorCiudad.Text = string.Empty;
            this.txtOperadorCodigoPostal.Text = string.Empty;
            this.txtOperadorEstado.Text = string.Empty;
            this.txtOperadorFechaNacimiento.Text = string.Empty;
            this.txtOperadorNombre.Text = string.Empty;
            this.txtPersonaAutorizaCredito.Text = string.Empty;
            this.txtRFCCliente.Text = string.Empty;
            this.txtRepresentante.Text = string.Empty;
            this.txtTarifaCarga.Text = string.Empty;
            this.txtTarifaDescripcion.Text = string.Empty;
            this.txtTarifaDiaria.Text = string.Empty;
            this.txtTarifaHoraAdicional.Text = string.Empty;
            this.txtTarifaHorasLibres.Text = string.Empty;
            this.txtTarifaKMAdicional.Text = string.Empty;
            this.txtTarifaKMLibres.Text = string.Empty;
            this.txtUnidadAnio.Text = string.Empty;
            this.txtUnidadCapacidadCarga.Text = string.Empty;
            this.txtUnidadCapacidadTanque.Text = string.Empty;
            this.txtUnidadCompaniaSeguro.Text = string.Empty;
            this.txtUnidadImporteDeducible.Text = string.Empty;
            this.txtUnidadMarca.Text = string.Empty;
            this.txtUnidadModelo.Text = string.Empty;
            this.txtUnidadNumeroEconomico.Text = string.Empty;
            this.txtUnidadNumeroPoliza.Text = string.Empty;
            this.txtUnidadNumeroSerie.Text = string.Empty;
            this.txtUnidadPlacasEstales.Text = string.Empty;
            this.txtUnidadPlacasFederales.Text = string.Empty;
            this.txtUnidadRendimientoTanque.Text = string.Empty;
            this.txtUnidadSucursal.Text = string.Empty;
            this.txtClaveProductoServicio.Text = string.Empty;
            this.txtDescripcionProductoServicio.Text = string.Empty;
            #endregion

            #region Deshabilitados los controles que solo son de lectura
            this.ddlTipoTarifa.Enabled = false;
            this.txtCargosAlteracionMedidor.Enabled = false;
            this.txtCargosEntregaInpuntual.Enabled = false;
            this.txtCargosHoraPosesion.Enabled = false;
            this.txtCargosPorcentajeFacturacion.Enabled = false;
            this.txtCodigoPostal.Enabled = false;
            this.txtCondicionesDiasRenta.Enabled = false;
            this.txtCondicionesDireccionRegreso.Enabled = false;
            this.txtCondicionesImporteDeposito.Enabled = false;
            this.txtDiasFacturar.Enabled = false; //BEP1401.SC0026
            this.txtDireccionEmpresa.Enabled = false;
            this.txtDomicilioCliente.Enabled = false;
            this.txtEmpresa.Enabled = false;
            this.txtNumeroCuentaCliente.Enabled = false;
            this.txtNumeroCuentaOracle.Enabled = false;
            this.txtRFCCliente.Enabled = false;
            this.txtRepresentante.Enabled = false;
            this.txtTarifaCarga.Enabled = false;
            this.txtTarifaDiaria.Enabled = false;
            this.txtTarifaHoraAdicional.Enabled = false;
            this.txtTarifaHorasLibres.Enabled = false;
            this.txtTarifaKMAdicional.Enabled = false;
            this.txtTarifaKMLibres.Enabled = false;
            this.txtUnidadAnio.Enabled = false;
            this.txtUnidadCapacidadCarga.Enabled = false;
            this.txtUnidadCapacidadTanque.Enabled = false;
            this.txtUnidadCompaniaSeguro.Enabled = false;
            this.txtUnidadImporteDeducible.Enabled = false;
            this.txtUnidadMarca.Enabled = false;
            this.txtUnidadModelo.Enabled = false;
            this.txtUnidadSucursal.Enabled = false;
            this.txtUnidadNumeroPoliza.Enabled = false;
            this.txtUnidadNumeroSerie.Enabled = false;
            this.txtUnidadPlacasEstales.Enabled = false;
            this.txtUnidadPlacasFederales.Enabled = false;
            this.txtUnidadRendimientoTanque.Enabled = false;
            this.txtClaveProductoServicio.Enabled = false;
            #region SC_0013
            this.txtOperadorAniosExperiencia.Enabled = false;
            this.txtOperadorFechaNacimiento.Enabled = false;
            this.txtOperadorCalle.Enabled = false;
            this.txtOperadorCiudad.Enabled = false;
            this.txtOperadorEstado.Enabled = false;
            this.txtOperadorCodigoPostal.Enabled = false;
            this.txtLicenciaNumero.Enabled = false;
            this.ddlTipoLicencia.Enabled = false;
            this.txtLicenciaFechaExpiracion.Enabled = false;
            this.txtLicenciaEstadoExpedicion.Enabled = false;
            #endregion
            #endregion
        }

        public void PrepararEdicion()
        {
            #region Deshabilitados los controles que solo son de lectura
            this.ddlSucursales.Enabled = false;
            this.ddlTipoTarifa.Enabled = false;
            this.txtCargosAlteracionMedidor.Enabled = false;
            this.txtCargosEntregaInpuntual.Enabled = false;
            this.txtCargosHoraPosesion.Enabled = false;
            this.txtCargosPorcentajeFacturacion.Enabled = false;
            this.txtCodigoPostal.Enabled = false;
            this.txtCondicionesDiasRenta.Enabled = false;
            this.txtCondicionesDireccionRegreso.Enabled = false;
            this.txtCondicionesImporteDeposito.Enabled = false;
            this.txtDiasFacturar.Enabled = false; //BEP1401.SC0026
            this.txtDireccionEmpresa.Enabled = false;
            this.txtDomicilioCliente.Enabled = false;
            this.txtEmpresa.Enabled = false;
            this.txtNumeroCuentaCliente.Enabled = false;
            this.txtNumeroCuentaOracle.Enabled = false;
            this.txtRFCCliente.Enabled = false;
            this.txtRepresentante.Enabled = false;
            this.txtTarifaCarga.Enabled = false;
            this.txtTarifaDiaria.Enabled = false;
            this.txtTarifaHoraAdicional.Enabled = false;
            this.txtTarifaHorasLibres.Enabled = false;
            this.txtTarifaKMAdicional.Enabled = false;
            this.txtTarifaKMLibres.Enabled = false;
            this.txtUnidadAnio.Enabled = false;
            this.txtUnidadCapacidadCarga.Enabled = false;
            this.txtUnidadCapacidadTanque.Enabled = false;
            this.txtUnidadCompaniaSeguro.Enabled = false;
            this.txtUnidadImporteDeducible.Enabled = false;
            this.txtUnidadMarca.Enabled = false;
            this.txtUnidadModelo.Enabled = false;
            this.txtUnidadSucursal.Enabled = false;
            this.txtUnidadNumeroPoliza.Enabled = false;
            this.txtUnidadNumeroSerie.Enabled = false;
            this.txtUnidadPlacasEstales.Enabled = false;
            this.txtUnidadPlacasFederales.Enabled = false;
            this.txtUnidadRendimientoTanque.Enabled = false;
            #region SC_0013
            this.txtOperadorAniosExperiencia.Enabled = false;
            this.txtOperadorFechaNacimiento.Enabled = false;
            this.txtOperadorCalle.Enabled = false;
            this.txtOperadorCiudad.Enabled = false;
            this.txtOperadorEstado.Enabled = false;
            this.txtOperadorCodigoPostal.Enabled = false;
            this.txtLicenciaNumero.Enabled = false;
            this.ddlTipoLicencia.Enabled = false;
            this.txtLicenciaFechaExpiracion.Enabled = false;
            this.txtLicenciaEstadoExpedicion.Enabled = false;
            #endregion

            PermitirAgregarProductoServicio(this.EquipoID != null && this.UnidadID != null);
            #endregion
        }

        /// <summary>
        /// Habilita o deshabilita la opción de seleccionar direcciones para el cliente
        /// </summary>
        /// <param name="permitir">Parametro que controla el estatus del control</param>
        public void PermitirSeleccionarDireccionCliente(bool permitir)
        {
            this.ibtnBuscarDirieccionCliente.Enabled = permitir;
            this.ibtnBuscarDirieccionCliente.Visible = permitir;
        }

        /// <summary>
        /// Habilita o deshabilita la opción de seleccionar tarifas para la unidad seleccionada
        /// </summary>
        /// <param name="permitir">Parametro que controla el estatus del control</param>
        public void PermitirSeleccionarTarifas(bool permitir)
        {
            this.ibtnBuscarTarifa.Enabled = permitir;
            this.ibtnBuscarTarifa.Visible = permitir;
            this.txtTarifaDescripcion.Enabled = permitir;
            btnPersonalizarTarifa.Enabled = permitir;
        }

        /// <summary>
        /// Habilita o deshabilita la opción de seleccionar uniades
        /// </summary>
        /// <param name="permitir">Parametro que controla el estatus del control</param>
        public void PermitirSeleccionarUnidad(bool permitir)
        {
            this.ibtnBuscarUnidad.Enabled = permitir;
            this.ibtnBuscarUnidad.Visible = permitir;
            this.txtUnidadNumeroEconomico.Enabled = permitir;
        }

        /// <summary>
        /// Habilita o deshabilita la opción de seleccionar representantes legales de acuerdo al tipo de cliente
        /// </summary>
        /// <param name="permitir">Parametro que controla el estatus del control</param>
        public void PermitirSeleccionarRepresentantes(bool permitir)
        {
            this.ddlRepresentantesLegales.Enabled = permitir;
        }

        /// <summary>
        /// Habilita o deshabilita la opción de agregar representantes legales
        /// </summary>
        /// <param name="permitir">Parametro que controla el estatus del control</param>
        public void PermitirAgregarRepresentantes(bool permitir)
        {
            this.btnAgregarRepresentante.Enabled = permitir;
            this.btnAgregarRepresentante.Visible = permitir;
        }

        /// <summary>
        /// Habilita o deshabilita la opciones de pago a crédito
        /// </summary>
        /// <param name="permitir">Parametro que controla el estatus del control</param>
        public void PermitirSeleccionarTipoConfirmacion(bool permitir)
        {
            this.tblPagoCredito.Visible = permitir;

            this.txtPersonaAutorizaCredito.Enabled = permitir;
            this.txtPersonaAutorizaCredito.Visible = permitir;

            this.ddlTipoPagoCredito.Enabled = permitir;
            this.ddlTipoPagoCredito.Visible = permitir;
        }

        /// <summary>
        /// Habilita o deshabilita la opción de autorizador de orden de compra para pago a crédito
        /// </summary>
        /// <param name="permitir">Parametro que controla el estatus del control</param>
        public void PermitirAsignarAutorizadorOrdenCompra(bool permitir)
        {
            this.trAutorizaOrdenCompra.Visible = permitir;
        }

        /// <summary>
        /// Habilita o deshabilita la opción de seleccionar operadores para el cliente seleccionado
        /// </summary>
        /// <param name="permitir">Parametro que controla el estatus del control</param>
        public void PermitirSeleccionarOperador(bool permitir)
        {
            this.ibtnBuscarOperador.Enabled = permitir;
            this.ibtnBuscarOperador.Visible = permitir;
            this.txtOperadorNombre.Enabled = permitir;
        }

        /// <summary>
        /// Habilita o deshabilita la opción de seleccionar la cantidad de dias a Facturar.
        /// </summary>
        /// <param name="permitir">Parametro que controla el estatus del control</param>
        public void PemitirSeleccionarDiasFacturar(Boolean permitir)//BEP1401.SC0026
        {
            this.txtDiasFacturar.Enabled = permitir;
        }

        /// <summary>
        /// Provee las monedas que pueden aplicar a los contratos
        /// </summary>
        /// <param name="monedas">Listado de monedas que puede seleccionar el usaurio</param>
        public void EstablecerOpcionesMoneda(Dictionary<string, string> monedas)
        {
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
        /// Establece las opciones de Facturacion de acuerdo a la duracion del contrato. 
        /// </summary>
        /// <param name="frecuenciaFacturacion">Diccionario con las frecuencias a presentar</param>
        public void EstablecerOpcionesFrecuenciaFacturacion(Dictionary<string, string> frecuenciaFacturacion)//BEP1401.SC0026
        {
            if(ReferenceEquals(frecuenciaFacturacion, null))
                frecuenciaFacturacion = new Dictionary<string, string>();

            this.ddlFrecuenciaFacturacion.DataSource = frecuenciaFacturacion;
            this.ddlFrecuenciaFacturacion.DataValueField = "key";
            this.ddlFrecuenciaFacturacion.DataTextField = "value";
            this.ddlFrecuenciaFacturacion.DataBind();
        }

        /// <summary>
        /// Despliega en pantalla los equipos aliados de la unidad seleccionada
        /// </summary>
        /// <param name="dt">Tabla con la información de los equipos aliados</param>
        public void EstablecerEquiposAliadoUnidad(System.Data.DataTable dt)
        {
            this.grdEquiposAliados.DataSource = dt;
            this.grdEquiposAliados.DataBind();
        }

        /// <summary>
        /// Actualiza el grid de representantes legales con los representantes seleccionados
        /// </summary>
        public void ActualizarRepresentantesLegales()
        {
            grdRepresentantesLegales.DataSource = this.RepresentantesSeleccionados;
            grdRepresentantesLegales.DataBind();
        }

        /// <summary>
        /// Limpia de session las variables que se usan para registrar un contrato
        /// </summary>
        public void LimpiarSesion()
        {
            Session.Remove("RepresentantesLegalesContrato");
            Session.Remove("ContratoRDBO");
        }

        public string ObtenerClaveDeducible()//SC0018
        {
            string clave = ConfigurationManager.AppSettings["ConceptoDeducible"];
            if (string.IsNullOrEmpty(clave) || string.IsNullOrWhiteSpace(clave))
                throw new Exception("Aun no se configura el concepto del deducible para DAÑOS MATERIALES de la unidad. Es necesario configurarlo antes de continuar con el registro del contrato.");
            return !string.IsNullOrEmpty(clave) && !string.IsNullOrWhiteSpace(clave) ? clave : string.Empty;
        }

        /// <summary>
        /// Habilita o deshabilita la opción de Validar Codigo de Autorizacion
        /// </summary>
        /// <param name="permitir">Parametro que controla el estatus del control</param>
        public void PermitirValidarCodigoAutorizacion(bool permitir)
        {
            btnValidarAutorizacion.Enabled = permitir;

            txtTarifaPersonalizadaCodigoAutorizacion.Enabled = permitir;
            txtTarifaPersonalizadaCodigoAutorizacion.ReadOnly = !permitir;

        }

        /// <summary>
        /// Habilita o deshabilita la opción de Solicitar Codigo de Autorizacion
        /// </summary>
        /// <param name="permitir">Parametro que controla el estatus del control</param>
        public void PermitirSolicitarCodigoAutorizacion(bool permitir)
        {

            btnSolicitarAutorizacion.Enabled = permitir;

            txtTarifaPersonalizadaCapacidadCarga.Enabled = permitir;
            txtTarifaPersonalizadaCapacidadCarga.ReadOnly = !permitir;
            txtTarifaPersonalizadaHrsLibres.Enabled = permitir;
            txtTarifaPersonalizadaHrsLibres.ReadOnly = !permitir;
            txtTarifaPersonalizadaKmsLibres.Enabled = permitir;
            txtTarifaPersonalizadaKmsLibres.ReadOnly = !permitir;
            txtTarifaPersonalizadaTarifaDiaria.Enabled = permitir;
            txtTarifaPersonalizadaTarifaDiaria.ReadOnly = !permitir;
            txtTarifaPersonalizadaTarifaHrAdicional.Enabled = permitir;
            txtTarifaPersonalizadaTarifaHrAdicional.ReadOnly = !permitir;
            txtTarifaPersonalizadaTarifaKmAdicional.Enabled = permitir;
            txtTarifaPersonalizadaTarifaKmAdicional.ReadOnly = !permitir;
        }

        #region Métodos para el Buscador
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="catalogoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
        }
        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
             this.Session_BOSelecto = null;    
       
        }

        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }
        #endregion

		public void ActualizarAvales()
		{
			this.grdAvales.DataSource = this.AvalesSeleccionados;
			this.grdAvales.DataBind();
		}

		public void MostrarAvales(bool mostrar)
		{
			this.pnlAvales.Visible = mostrar;
		}

		public void MostrarRepresentantesAval(bool mostrar)
		{
			this.trRepresentantesAval.Visible = mostrar;
		}
		public void MostrarDetalleRepresentantesAval(List<RepresentanteLegalBO> representantes)
		{
			this.grdRepresentantesDialog.DataSource = representantes;
			this.grdRepresentantesDialog.DataBind();

			this.RegistrarScript("DetalleAval", "abrirDetalleRepresentantes('AVAL');");
		}

		public void MostrarPersonasCliente(bool mostrar)
		{
			pnlPersonasCliente.Visible = mostrar;
		}
		public void PermitirAgregarAvales(bool permitir)
		{
			this.btnAgregarAval.Enabled = permitir;
			this.btnAgregarAval.Visible = permitir;
		}
		public void PermitirSeleccionarAvales(bool permitir)
		{
			this.ddlAvales.Enabled = permitir;
		}

		public void MostrarRepresentantesLegales(bool mostrar)
		{
			pnlRepresentantes.Visible = mostrar;
		}

        public void PermitirAgregarProductoServicio(bool permitir) {
            this.txtClaveProductoServicio.ReadOnly = !permitir;
            this.txtClaveProductoServicio.Enabled = permitir;
            this.ibtnBuscarProductoServicio.Visible = permitir;
        }
        #endregion

        #region Eventos
        protected void txtNombreCliente_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.CuentaClienteID = null;
                string nombreCuentaCliente = this.CuentaClienteNombre;
                Session_BOSelecto = null;
                DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                this.CuentaClienteNombre = nombreCuentaCliente;
                if (!string.IsNullOrEmpty(this.CuentaClienteNombre) && !string.IsNullOrWhiteSpace(this.CuentaClienteNombre)) {
                    EjecutaBuscador("CuentaClienteIdealeaseSimple", ECatalogoBuscador.CuentaClienteIdealease);
                }

                this.CuentaClienteNombre = null;
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNombreCliente_TextChanged:" + ex.Message);
            }
        }
        protected void ibtnBuscarCliente_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.CuentaClienteID = null;
                string nombreCuentaCliente = this.CuentaClienteNombre;
                Session_BOSelecto = null;
                DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                this.CuentaClienteNombre = nombreCuentaCliente;
                if(!string.IsNullOrEmpty(this.CuentaClienteNombre) )
                    EjecutaBuscador("CuentaClienteIdealeaseSimple&&hidden=0", ECatalogoBuscador.CuentaClienteIdealease);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarCliente_Click:" + ex.Message);
            }
        }

        protected void ibtnBuscarDirieccionCliente_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.hdnClienteID.Value) && !string.IsNullOrWhiteSpace(this.CuentaClienteNombre))
                    EjecutaBuscador("DireccionCuentaClienteIdealeaseSimple", ECatalogoBuscador.DireccionCliente);
                else
                    this.MostrarMensaje("Por favor seleccione un cliente previamente a consultar sus direcciones.", ETipoMensajeIU.ADVERTENCIA, null);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar las direcciones del Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarDirieccionCliente_Click:" + ex.Message);
            }
        }
        /// <summary>
        /// ejecuta el buscador para mostrar la direccion de cliente
        /// </summary>
        public void DesplegarDireccionCliente() {
            EjecutaBuscador("DireccionCuentaClienteIdealeaseSimple", ECatalogoBuscador.DireccionCliente);
        }
        protected void txtUnidadNumeroEconomico_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.UnidadID = null;
                string numeco = this.NumeroEconomico;
                Session_BOSelecto = null;
                this.DesplegarBOSelecto(ECatalogoBuscador.Unidad);

                this.NumeroEconomico = numeco;
                if (!string.IsNullOrEmpty(this.NumeroEconomico) && SucursalID.HasValue)
                    this.EjecutaBuscador("UnidadIdealease", ECatalogoBuscador.Unidad);

                this.NumeroEconomico = null;
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al buscar las unidades para la renta", ETipoMensajeIU.ADVERTENCIA, nombreClase + "txtUnidadNumeroEconomico_TextChanged" + ex.Message);
            }
        }
        protected void ibtnBuscarUnidad_Click(object sender, ImageClickEventArgs e)
        {
            try  {
                this.UnidadID = null;
                string numeco = this.NumeroEconomico;
                Session_BOSelecto = null;
                if (!string.IsNullOrEmpty(numeco))
                    this.EjecutaBuscador("UnidadIdealeaseSimple", ECatalogoBuscador.Unidad);

            }
            catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al buscar las unidades para la renta", ETipoMensajeIU.ADVERTENCIA, nombreClase + "ibtnBuscarUnidad_Click" + ex.Message);
            }
        }

        protected void txtClaveProductoServicio_TextChanged(object sender, EventArgs e) {
            try {
                string clvProducto = this.ClaveProductoServicio;
                Session_BOSelecto = null;
                DesplegarBOSelecto(ECatalogoBuscador.ProductoServicio);

                this.ClaveProductoServicio = clvProducto;
                if (clvProducto != null)
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
                if (!string.IsNullOrEmpty( clvProducto))
                    EjecutaBuscador("ProductoServicio&hidden=0", ECatalogoBuscador.ProductoServicio);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar el producto", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarProductoServicio_Click: " + ex.Message);
            }
        }

        protected void txtTarifaDescripcion_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.TarifaID = null;

                string descripcion = this.TarifaDescripcion;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Tarifa);

                this.TarifaDescripcion = descripcion;
                if (!string.IsNullOrEmpty(this.TarifaDescripcion) && !string.IsNullOrWhiteSpace(this.TarifaDescripcion)) {
                    this.EjecutaBuscador("Tarifas", ECatalogoBuscador.Tarifa);
                    this.TarifaDescripcion = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, this.ToString() + ".txtSucursal_TextChanged:" + ex.Message);
            }
        }
        protected void ibtnBuscarTarifa_Click(object sender, ImageClickEventArgs e)
        {
            try {
                string descripcion = this.TarifaDescripcion;
                this.Session_BOSelecto = null;
                if (!string.IsNullOrEmpty(descripcion))
                    this.EjecutaBuscador("Tarifas", ECatalogoBuscador.Tarifa);
            }
            catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al buscar las tarifas para la renta", ETipoMensajeIU.ADVERTENCIA, nombreClase + "ibtnBuscarTarifa_Click" + ex.Message);
            }
        }

        protected void btnAgregarRepresentante_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AgregarRepresentanteLegal();
                ddlRepresentantesLegales.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al agregar un Representante Legal al contrato", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnAgregarRepresentante_Click: " + ex.Message);
            }
        }

        protected void grdRepresentantesLegales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdRepresentantesLegales.DataSource = this.RepresentantesSeleccionados;
                grdRepresentantesLegales.PageIndex = e.NewPageIndex;
                grdRepresentantesLegales.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al cambiar de pagina en los datos del representante legal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".grdRepresentantesLegales_PageIndexChanging: " + ex.Message);
            }
        }
        protected void grdRepresentantesLegales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                int index = Convert.ToInt32(e.CommandArgument);
                this.presentador.QuitarRepresentanteLegal(index);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".grdRepresentantesLegales_RowCommand: " + ex.Message);
            }
        }
        protected void grdRepresentantesLegales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var label = e.Row.FindControl("lblTipoPersona") as Label;
                    if (label != null)
                    {
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
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".grdRepresentantesLegales_RowDataBound: " + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Unidad:
                    case ECatalogoBuscador.Sucursal:
                    case ECatalogoBuscador.CuentaClienteIdealease:
                    case ECatalogoBuscador.DireccionCliente:
                    case ECatalogoBuscador.Tarifa:
                    case ECatalogoBuscador.Operador:
                    case ECatalogoBuscador.ProductoServicio:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click: " + ex.Message);
            }
        }

        protected void ddlFormaPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.ddlFormaPago.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlFormaPago.SelectedValue))
                    this.presentador.SeleccionarFormaPago(int.Parse(this.ddlFormaPago.SelectedValue));
                else
                    this.presentador.SeleccionarFormaPago(null);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al seleccionar la forma de pago", ETipoMensajeIU.ERROR, nombreClase + ".ddlFormaPago_SelectedIndexChanged: " + ex.Message);
            }
        }
        protected void ddlTipoPagoCredito_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.TipoConfirmacionID != null)
                    this.presentador.SeleccionarTipoConfirmacion(this.TipoConfirmacionID);
                else
                    this.presentador.SeleccionarTipoConfirmacion(null);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al seleccionar el tipo de crédito", ETipoMensajeIU.ERROR, nombreClase + ".ddlTipoPagoCredito_SelectedIndexChanged: " + ex.Message);
            }
        }

        protected void txtCondicionesFechaDevolucion_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.presentador.SeleccionarFechasContrato();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al momento de validar la información.", ETipoMensajeIU.ERROR, nombreClase + ".txtCondicionesFechaDevolucion_TextChanged: " + ex.Message);
            }
        }
        protected void txtFechaContrato_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.presentador.SeleccionarFechasContrato();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al momento de validar la información.", ETipoMensajeIU.ERROR, nombreClase + ".txtFechaContrato_TextChanged: " + ex.Message);
            }
        }
        protected void txtLicenciaFechaExpiracion_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string s;
                if ((s = this.presentador.ValidarFechaExpiracionLicenciaOperador()) != null)
                {
                    this.txtLicenciaFechaExpiracion.Text = string.Empty;
                    this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al seleccionar la fecha", ETipoMensajeIU.ERROR, nombreClase + ".txtLicenciaFechaExpiracion_TextChanged:" + ex.Message);
            }
        }
        protected void txtOperadorFechaNacimiento_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string s;
                if ((s = this.presentador.ValidarFechaNacimientoOperador()) != null)
                {
                    this.txtOperadorFechaNacimiento.Text = string.Empty;
                    this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al seleccioanr la fecha", ETipoMensajeIU.ERROR, nombreClase + ".txtOperadorFechaNacimiento_TextChanged:" + ex.Message);
            }
        }
        protected void ddlMonedas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.presentador.SeleccionarMoneda(this.ddlMonedas.SelectedValue);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al seleccionar la moneda", ETipoMensajeIU.ERROR, nombreClase + ".ddlMonedas_SelectedIndexChanged" + ex.Message);
            }
        }

        protected void btnValidarAutorizacion_Click(object sender, EventArgs e)
        {
            try
            {
                if (presentador.ValidarCodigoAutorizacion(txtTarifaPersonalizadaCodigoAutorizacion.Text.Trim()))
                {
                    presentador.ActualizarTarifa();
                    presentador.PrepararDialogoTarifaPersonalizada();
                    RegistrarScript(null, "MostrarDialogo(false);");
                }
                else
                    MostrarMensaje("El c&oacute;digo de autorizaci&oacute;n es inv&aacute;lido, por favor verifique",
                                   ETipoMensajeIU.ADVERTENCIA);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un Error al validar el código de autorización", ETipoMensajeIU.INFORMACION);
            }
        }

        protected void btnPersonalizarTarifa_Click(object sender, EventArgs e)
        {
            presentador.PrepararDialogoTarifaPersonalizada();
            RegistrarScript(null, "MostrarDialogo(true);");
        }

        protected void btnSolicitarAutorizacion_Click(object sender, EventArgs e)
        {
            try {
                PermitirSolicitarCodigoAutorizacion(false);
                presentador.SolicitarAutorizacionTarifaPersonalizada();
            }
            catch (Exception ex) {
                MostrarMensaje("Ocurrió un error al solicitar el código de autorización", ETipoMensajeIU.ERROR, ex.Message);
                presentador.PrepararDialogoTarifaPersonalizada();
                hdnMostrarDialogoTarifa.Value = "0";
            }
        }

        protected void btnCancelarCambioTarifa_Click(object sender, EventArgs e)
        {
            presentador.PrepararDialogoTarifaPersonalizada();
            hdnMostrarDialogoTarifa.Value = "0";

        }

        #region SC_0013
        protected void ibtnBuscarOperador_Click(object sender, ImageClickEventArgs e)
        {
            try {
                string nombreOperador=this.OperadorNombre;
                Session_BOSelecto = null;
                if (!string.IsNullOrEmpty(this.OperadorNombre))
                    this.EjecutaBuscador("Operadores", ECatalogoBuscador.Operador);
                nombreOperador = null;
            }
            catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar un Operador", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarOperador_Click:" + ex.Message);
            }
        }
        protected void txtOperadorNombre_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.OperadorID = null;

                string operadorNombre = this.OperadorNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Operador);
                this.OperadorNombre = operadorNombre;
                if (!string.IsNullOrEmpty(this.OperadorNombre) && !string.IsNullOrWhiteSpace(this.OperadorNombre))
                {
                    this.EjecutaBuscador("Operadores", ECatalogoBuscador.Operador);
                    this.OperadorNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Operador", ETipoMensajeIU.ADVERTENCIA, this.ToString() + ".txtOperadorNombre_TextChanged:" + ex.Message);
            }
        }
        #endregion

		protected void cbSoloRepresentantes_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				this.presentador.ConfigurarOpcionesPersonas();
				this.ddlAvales.SelectedIndex = -1;
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".cbSoloRepresentantes_CheckedChanged: " + ex.Message);
			}
		}
		protected void ddlAvales_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				this.presentador.SeleccionarAval();
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias al seleccionar el aval", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ddlAvales_SelectedIndexChanged: " + ex.Message);
			}
		}
		protected void btnAgregarAval_Click(object sender, EventArgs e)
		{
			try
			{
				this.presentador.AgregarAval();

				this.ddlAvales.SelectedIndex = -1;
				this.RepresentantesAvalSeleccionados = null;
				this.RepresentantesAvalTotales = null;
				this.MostrarRepresentantesAval(false);
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias al agregar un Aval al contrato", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnAgregarAval_Click: " + ex.Message);
			}
		}
		protected void grdRepresentantesAval_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			try
			{
				this.grdRepresentantesAval.DataSource = this.RepresentantesAvalTotales;
				this.grdRepresentantesAval.PageIndex = e.NewPageIndex;
				this.grdRepresentantesAval.DataBind();
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias al cambiar la página de los obligados solidarios", ETipoMensajeIU.ERROR, nombreClase + ".grdObligadosSolidarios_PageIndexChanging: " + ex.Message);
			}
		}
		protected void chkRepresentanteAval_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				//Se obtiene el id de los controles
				CheckBox chk = (CheckBox)sender;
				GridViewRow row = (GridViewRow)chk.Parent.Parent;
				Label lbl = (Label)row.FindControl("lblRepresentanteAvalID");

				int id;
				if (Int32.TryParse(lbl.Text, out id))
				{
					this.hdnRepresentanteAvalSeleccionadoID.Value = id.ToString();

					if (chk.Checked)
						this.presentador.AgregarRepresentanteAval();
					else
						this.presentador.QuitarRepresentanteAval();

					this.hdnRepresentanteAvalSeleccionadoID.Value = string.Empty;
				}
				else
					throw new Exception("No se encontró el ID del representante legal del aval o tiene un dato inválido.");
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias al seleccionar el representante legal para el aval", ETipoMensajeIU.ERROR, nombreClase + ".chkRepresentanteAval_CheckedChanged: " + ex.Message);
			}
		}

		protected void grdRepresentantesAval_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			try
			{
				if (e.Row.RowType == DataControlRowType.DataRow)
				{
					RepresentanteLegalBO persona = (RepresentanteLegalBO)e.Row.DataItem;
					var chk = e.Row.FindControl("chkRepAval") as CheckBox;

					if (chk != null)
						chk.Checked = this.RepresentantesAvalSeleccionados != null && this.RepresentantesAvalSeleccionados.Exists(p => p.Id == persona.Id);
				}
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias listar los representantes legales del aval", ETipoMensajeIU.ERROR, nombreClase + ".grdRepresentantesAval_RowDataBound: " + ex.Message);
			}
		}

		protected void grdAvales_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			try
			{
				this.grdAvales.DataSource = this.AvalesSeleccionados;
				this.grdAvales.PageIndex = e.NewPageIndex;
				this.grdAvales.DataBind();
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias al cambiar la página de los avales", ETipoMensajeIU.ERROR, nombreClase + ".grdAvaless_PageIndexChanging: " + ex.Message);
			}
		}

		protected void grdAvales_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			try
			{
				if (e.CommandName.ToUpper() == "PAGE" || e.CommandName.ToUpper() == "SORT") return;
				int index = Convert.ToInt32(e.CommandArgument);

				switch (e.CommandName.ToUpper().Trim())
				{
					case "CMDELIMINAR":
						this.presentador.QuitarAval(index);
						break;
					case "CMDDETALLE":
						this.presentador.PrepararVisualizacionRepresentantesAval(index);
						break;
				}
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias al ejecutar la acción sobre el aval", ETipoMensajeIU.ERROR, nombreClase + ".grdAvales_RowCommand: " + ex.Message);
			}
		}

		protected void grdAvales_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			try
			{
				if (e.Row.RowType != DataControlRowType.DataRow) return;
				AvalBO bo = e.Row.DataItem != null ? (AvalBO)e.Row.DataItem : new AvalProxyBO();
				if (!bo.TipoAval.HasValue)
					e.Row.FindControl("ibtDetalle").Visible = false;
				else if (bo.TipoAval == ETipoAval.Fisico)
					e.Row.FindControl("ibtDetalle").Visible = false;
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias al desplegar los avales", ETipoMensajeIU.ERROR, nombreClase + ".grdAvales_RowDataBound: " + ex.Message);
			}
		}

        protected void txtDiasFacturar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var validarDias = this.presentador.ValidarDiasFacturar();
                if(validarDias != "")
                    MostrarMensaje(validarDias, ETipoMensajeIU.ADVERTENCIA);
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al validar la cantidad de Dias a Facturar", ETipoMensajeIU.ERROR, this.ToString() + ".txtDiasFacturar_TextChanged:" + ex.Message);
            }
        }
        #endregion        

    }
}
