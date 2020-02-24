//Satisface al CU027 - Registrar Contrato de Mantenimiento
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.Mantto.BO;
using BPMO.SDNI.Contratos.Mantto.PRE;
using BPMO.SDNI.Contratos.Mantto.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.Mantto.UI
{
    public partial class ucContratoManttoUI : System.Web.UI.UserControl, IucContratoManttoVIS, IucLineaContratoManttoVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase en la cual estamos trabajando
        /// </summary>
        private string nombreClase = "ucContratoManttoUI";
        /// <summary>
        /// Enumeración para controlar los objetos del buscador
        /// </summary>
        public enum ECatalogoBuscador
        {
            Sucursal,
            CuentaClienteIdealease,
            DireccionCliente,
            Unidad,
            ProductoServicio
        }
        /// <summary>
        /// presentador del UC de información general del contrato de renta diaria
        /// </summary>
        private ucContratoManttoPRE presentador;
        #endregion

        #region Propiedades
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

        #region Propiedades de IucContratoManttoVIS
        bool IucContratoManttoVIS.ModoEdicion
        {
            get
            {
                bool id = false;
                if (!String.IsNullOrEmpty(this.hdnModoEdicionContrato.Value))
                    id = bool.Parse(this.hdnModoEdicionContrato.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnModoEdicionContrato.Value = value.ToString();
                else
                    this.hdnModoEdicionContrato.Value = false.ToString();
            }
        }

        /// <summary>
        /// Identificador del contrato
        /// </summary>
        public int? ContratoID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnContratoID.Value))
                    id = int.Parse(this.hdnContratoID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnContratoID.Value = value.ToString();
                else
                    this.hdnContratoID.Value = string.Empty;
            }
        }
        public string NumeroContrato
        {
            get
            {
                return (String.IsNullOrEmpty(this.hdnNumeroContrato.Value)) ? null : this.hdnNumeroContrato.Value.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.hdnNumeroContrato.Value = value;
                else
                    this.hdnNumeroContrato.Value = string.Empty;
            }
        }
        public int? TipoContratoID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnTipoContratoID.Value))
                    id = int.Parse(this.hdnTipoContratoID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnTipoContratoID.Value = value.ToString();
                else
                    this.hdnTipoContratoID.Value = string.Empty;
            }
        }
        public int? EstatusID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnEstatusContratoID.Value))
                    id = int.Parse(this.hdnEstatusContratoID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnEstatusContratoID.Value = value.ToString();
                else
                    this.hdnEstatusContratoID.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el nombre de la empresa arrendadora
        /// </summary>
        public string NombreEmpresa
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtEmpresa.Text)) ? null : this.txtEmpresa.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtEmpresa.Text = value;
                else
                    this.txtEmpresa.Text = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la dirección de la empresa arrendadora
        /// </summary>
        public string DomicilioEmpresa
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtDireccionEmpresa.Text)) ? null : this.txtDireccionEmpresa.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtDireccionEmpresa.Text = value;
                else
                    this.txtDireccionEmpresa.Text = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el representante de la empresa arrendadora
        /// </summary>
        public string RepresentanteEmpresa
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtRepresentante.Text)) ? null : this.txtRepresentante.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtRepresentante.Text = value;
                else
                    this.txtRepresentante.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el identificador de la sucursal de la cual se desean filtrar las unidades
        /// </summary>
        public int? SucursalID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnSucursalID.Value))
                    id = int.Parse(this.hdnSucursalID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnSucursalID.Value = value.ToString();
                else
                    this.hdnSucursalID.Value = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal seleccionada para el filtro de consulta
        /// </summary>
        string IucContratoManttoVIS.SucursalNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtSucursal.Text)) ? null : this.txtSucursal.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtSucursal.Text = value;
                else
                    this.txtSucursal.Text = string.Empty;
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
        public DateTime? FechaInicioContrato
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtFechaInicioContrato.Text) && !string.IsNullOrWhiteSpace(this.txtFechaInicioContrato.Text))
                {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.txtFechaInicioContrato.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtFechaInicioContrato.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        public DateTime? FechaTerminacionContrato
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtFechaFinalizacion.Text) && !string.IsNullOrWhiteSpace(this.txtFechaFinalizacion.Text))
                {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.txtFechaFinalizacion.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtFechaFinalizacion.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        public int? Plazo
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtPlazo.Text))
                    id = int.Parse(this.txtPlazo.Text.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.txtPlazo.Text = value.ToString();
                else
                    this.txtPlazo.Text = string.Empty;
            }
        }

        /// <summary>
        /// Codigo de la moneda selecionada
        /// </summary>
        public string CodigoMoneda
        {
            get
            {
                if (this.ddlMonedas.SelectedIndex > 0)
                    return this.ddlMonedas.SelectedValue.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.ddlMonedas.SelectedValue = value;
                else
                    this.ddlMonedas.SelectedIndex = 0;
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
        /// Numero de Cuenta de Oracle CuentaClienteBO.Numero
        /// </summary>
        public String ClienteNumeroCuenta
        {
            get { return String.IsNullOrEmpty(this.txtNumeroCuentaOracle.Text) ? null : this.txtNumeroCuentaOracle.Text; }
            set { this.txtNumeroCuentaOracle.Text = value ?? String.Empty; }
        }
        /// <summary>
        /// Obtiene o establece si la cuenta del cliente es física o Moral [true = fisica | false=moral]
        /// </summary>
        public bool? ClienteEsFisica
        {
            get
            {
                bool val;
                return Boolean.TryParse(this.rbtEsFisico.SelectedValue, out val) ? (bool?)val : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.rbtEsFisico.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece el número de cuenta del cliente
        /// </summary>
        public int? CuentaClienteID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnCuentaClienteID.Value) && !string.IsNullOrWhiteSpace(this.hdnCuentaClienteID.Value))
                {
                    int val = 0;
                    return Int32.TryParse(this.hdnCuentaClienteID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnCuentaClienteID.Value = value.ToString();
                else
                    this.hdnCuentaClienteID.Value = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre del cliente
        /// </summary>
        public string CuentaClienteNombre
        {
            get
            {
                return (String.IsNullOrEmpty(txtNombreCliente.Text)) ? null : this.txtNombreCliente.Text.Trim().ToUpper();
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
        /// Dirección completa del cliente
        /// </summary>
        public string ClienteDireccionCompleta
        {
            get
            {
                return (String.IsNullOrEmpty(txtDomicilioCliente.Text)) ? null : this.txtDomicilioCliente.Text.Trim().ToUpper();
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
                return (String.IsNullOrEmpty(txtCodigoPostal.Text)) ? null : this.txtCodigoPostal.Text.Trim().ToUpper();
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
                if (Session["ListadoRepresentantesLegalesCM"] != null)
                    return (List<RepresentanteLegalBO>)Session["ListadoRepresentantesLegalesCM"];

                return new List<RepresentanteLegalBO>();
            }
            set
            {
                List<RepresentanteLegalBO> lst = value ?? new List<RepresentanteLegalBO>();

                //Se sube a la sesión
                Session["ListadoRepresentantesLegalesCM"] = new List<RepresentanteLegalBO>(lst);
                //Se asigna en el dropdownlist
                this.ddlRepresentantesLegales.Items.Clear();
                this.ddlRepresentantesLegales.Items.Add(new ListItem("Seleccione una opción", "0"));
                this.ddlRepresentantesLegales.DataTextField = "Nombre";
                this.ddlRepresentantesLegales.DataValueField = "Id";
                this.ddlRepresentantesLegales.DataSource = lst;
                this.ddlRepresentantesLegales.DataBind();
            }
        }
        /// <summary>
        /// Obtiene o establece los representantes legales del cliente que han sido seleccionados
        /// </summary>
        public List<RepresentanteLegalBO> RepresentantesSeleccionados
        {
            get
            {
                List<RepresentanteLegalBO> lst = new List<RepresentanteLegalBO>();
                if (Session["RepresentantesLegalesContratoCM"] != null)
                    lst = (List<RepresentanteLegalBO>)Session["RepresentantesLegalesContratoCM"];

                return lst;
            }
            set
            {
                List<RepresentanteLegalBO> lst = value ?? new List<RepresentanteLegalBO>();
                Session["RepresentantesLegalesContratoCM"] = lst;
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
        /// Obtiene o establece el listado de obligados solidarios del cliente seleccionado
        /// </summary>
        public List<ObligadoSolidarioBO> ObligadosSolidariosTotales
        {
            get
            {
                if (Session["ListadoObligadosSolidariosCM"] != null)
                    return (List<ObligadoSolidarioBO>)Session["ListadoObligadosSolidariosCM"];

                return new List<ObligadoSolidarioBO>();
            }
            set
            {
                List<ObligadoSolidarioBO> lst = value ?? new List<ObligadoSolidarioBO>();

                //Se sube a la sesión
                Session["ListadoObligadosSolidariosCM"] = new List<ObligadoSolidarioBO>(lst);
                //Se asigna en el dropdownlist
                this.ddlObligadosSolidarios.Items.Clear();
                this.ddlObligadosSolidarios.Items.Add(new ListItem("Seleccione una opción", "0"));
                this.ddlObligadosSolidarios.DataTextField = "Nombre";
                this.ddlObligadosSolidarios.DataValueField = "Id";
                this.ddlObligadosSolidarios.DataSource = lst;
                this.ddlObligadosSolidarios.DataBind();
            }
        }
        /// <summary>
        /// Obtiene o establece los obligados solidarios del cliente que han sido seleccionados
        /// </summary>
        public List<ObligadoSolidarioBO> ObligadosSolidariosSeleccionados
        {
            get
            {
                List<ObligadoSolidarioBO> lst = new List<ObligadoSolidarioBO>();
                if (Session["ObligadosSolidariosContratoCM"] != null)
                    lst = (List<ObligadoSolidarioBO>)Session["ObligadosSolidariosContratoCM"];

                return lst;
            }
            set
            {
                List<ObligadoSolidarioBO> lst = value ?? new List<ObligadoSolidarioBO>();
                Session["ObligadosSolidariosContratoCM"] = lst;
            }
        }
        /// <summary>
        /// Obtiene el identificador del obligado solidario seleccionado
        /// </summary>
        public int? ObligadoSolidarioSeleccionadoID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ddlObligadosSolidarios.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlObligadosSolidarios.SelectedValue))
                {
                    if (System.String.Compare(this.ddlObligadosSolidarios.SelectedValue, "0", System.StringComparison.Ordinal) != 0)
                    {
                        int val = 0;
                        return Int32.TryParse(this.ddlObligadosSolidarios.SelectedValue, out val) ? (int?)val : null;
                    }
                }
                return null;
            }
        }
        public List<RepresentanteLegalBO> RepresentantesObligadoTotales
        {
            get
            {
                if (Session["ListaRepresentantesObligadosSolidarioCM"] == null)
                    return new List<RepresentanteLegalBO>();
                return (List<RepresentanteLegalBO>)Session["ListaRepresentantesObligadosSolidarioCM"];
            }
            set
            {
                List<RepresentanteLegalBO> lst = value ?? new List<RepresentanteLegalBO>();

                Session["ListaRepresentantesObligadosSolidarioCM"] = lst;
                this.grdRepresentantesObligadoSolidario.DataSource = lst;
                this.grdRepresentantesObligadoSolidario.DataBind();
            }
        }
        public List<RepresentanteLegalBO> RepresentantesObligadoSeleccionados
        {
            get { return (List<RepresentanteLegalBO>)Session["RepresentantesObligSolidarioCM"]; }
            set
            {
                List<RepresentanteLegalBO> lst = value ?? new List<RepresentanteLegalBO>();

                Session["RepresentantesObligSolidarioCM"] = lst;
            }
        }
        public int? RepresentanteObligadoSeleccionadoID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnRepresentanteObligadoSeleccionadoID.Value) && !string.IsNullOrWhiteSpace(this.hdnRepresentanteObligadoSeleccionadoID.Value))
                {
                    int val = 0;
                    return Int32.TryParse(this.hdnRepresentanteObligadoSeleccionadoID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
        }
        public bool? ObligadosComoAvales
        {
            get
            {
                return this.cbObligadosComoAvales.Checked;
            }
            set
            {
                if (value != null)
                    this.cbObligadosComoAvales.Checked = value.Value;
                else
                    this.cbObligadosComoAvales.Checked = false;
            }
        }

        /// <summary>
        /// Obtiene o establece el listado de avales del cliente seleccionado
        /// </summary>
        public List<AvalBO> AvalesTotales
        {
            get
            {
                if (Session["ListadoAvalesCM"] != null)
                    return (List<AvalBO>)Session["ListadoAvalesCM"];

                return new List<AvalBO>();
            }
            set
            {
                List<AvalBO> lst = value ?? new List<AvalBO>();

                //Se sube a la sesión
                Session["ListadoAvalesCM"] = new List<AvalBO>(lst);
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
                if (Session["AvalesContratoCM"] != null)
                    lst = (List<AvalBO>)Session["AvalesContratoCM"];

                return lst;
            }
            set
            {
                List<AvalBO> lst = value ?? new List<AvalBO>();
                Session["AvalesContratoCM"] = lst;
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
                if (Session["ListaRepresentantesAvalCM"] == null)
                    return new List<RepresentanteLegalBO>();
                return (List<RepresentanteLegalBO>)Session["ListaRepresentantesAvalCM"];
            }
            set
            {
                List<RepresentanteLegalBO> lst = value ?? new List<RepresentanteLegalBO>();

                Session["ListaRepresentantesAvalCM"] = lst;
                this.grdRepresentantesAval.DataSource = lst;
                this.grdRepresentantesAval.DataBind();
            }
        }
        public List<RepresentanteLegalBO> RepresentantesAvalSeleccionados
        {
            get { return (List<RepresentanteLegalBO>)Session["RepresentantesAvalCM"]; }
            set
            {
                List<RepresentanteLegalBO> lst = value ?? new List<RepresentanteLegalBO>();

                Session["RepresentantesAvalCM"] = lst;
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

        public List<LineaContratoManttoBO> LineasContrato
        {
            get
            {
                if (Session["ListadoLineasContratoCM"] != null)
                    return (List<LineaContratoManttoBO>)Session["ListadoLineasContratoCM"];

                return new List<LineaContratoManttoBO>();
            }
            set
            {
                List<LineaContratoManttoBO> lst = value ?? new List<LineaContratoManttoBO>();

                Session["ListadoLineasContratoCM"] = new List<LineaContratoManttoBO>(lst);
            }
        }
        public int? LineaContratoSeleccionadaIndex
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnLineaContratoIndex.Value))
                    id = int.Parse(this.hdnLineaContratoIndex.Value.Trim());
                return id;
            }
        }

        public string UbicacionTaller
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUbicacionTallerServicio.Text)) ? null : this.txtUbicacionTallerServicio.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtUbicacionTallerServicio.Text = value;
                else
                    this.txtUbicacionTallerServicio.Text = string.Empty;
            }
        }
        public decimal? DepositoGarantia
        {
            get
            {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtDepositoGarantia.Text))
                    temp = Decimal.Parse(this.txtDepositoGarantia.Text.Trim().Replace(",", ""));
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtDepositoGarantia.Text = string.Format("{0:#,##0.00##}", value);
                else
                    this.txtDepositoGarantia.Text = string.Empty;
            }
        }
        public decimal? ComisionApertura
        {
            get
            {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtComisionApertura.Text))
                    temp = Decimal.Parse(this.txtComisionApertura.Text.Trim().Replace(",", ""));
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtComisionApertura.Text = string.Format("{0:#,##0.00##}", value);
                else
                    this.txtComisionApertura.Text = string.Empty;
            }
        }
        /// <summary>
        /// Tipo de Inclusion para el Seguro Seleccionado
        /// </summary>
        public int? IncluyeSeguroID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ddlIncluyeSeguro.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlIncluyeSeguro.SelectedValue))
                {
                    if (this.ddlIncluyeSeguro.SelectedIndex > 0)
                    {
                        int val = 0;
                        return Int32.TryParse(this.ddlIncluyeSeguro.SelectedValue, out val) ? (int?)val : null;
                    }
                }
                return null;
            }
            set
            {
                if (value != null)
                    this.ddlIncluyeSeguro.SelectedValue = value.ToString();
                else
                    this.ddlIncluyeSeguro.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Tipo de Inclusion seleccionado para lavado
        /// </summary>
        public int? IncluyeLavadoID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ddlIncluyeLavado.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlIncluyeLavado.SelectedValue))
                {
                    if (this.ddlIncluyeLavado.SelectedIndex > 0)
                    {
                        int val = 0;
                        return Int32.TryParse(this.ddlIncluyeLavado.SelectedValue, out val) ? (int?)val : null;
                    }
                }
                return null;
            }
            set
            {
                if (value != null)
                    this.ddlIncluyeLavado.SelectedValue = value.ToString();
                else
                    this.ddlIncluyeLavado.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Tipo de Inclusion seleccionado para pintura y rotulación
        /// </summary>
        public int? IncluyePinturaRotulacionID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ddlIncluyeRotulacionPintura.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlIncluyeRotulacionPintura.SelectedValue))
                {
                    if (this.ddlIncluyeRotulacionPintura.SelectedIndex > 0)
                    {
                        int val = 0;
                        return Int32.TryParse(this.ddlIncluyeRotulacionPintura.SelectedValue, out val) ? (int?)val : null;
                    }
                }
                return null;
            }
            set
            {
                if (value != null)
                    this.ddlIncluyeRotulacionPintura.SelectedValue = value.ToString();
                else
                    this.ddlIncluyeRotulacionPintura.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Tipo de Inclusion seleccionado para llantas
        /// </summary>
        public int? IncluyeLlantasID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ddlIncluyeLlantas.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlIncluyeLlantas.SelectedValue))
                {
                    if (this.ddlIncluyeLlantas.SelectedIndex > 0)
                    {
                        int val = 0;
                        return Int32.TryParse(this.ddlIncluyeLlantas.SelectedValue, out val) ? (int?)val : null;
                    }
                }
                return null;
            }
            set
            {
                if (value != null)
                    this.ddlIncluyeLlantas.SelectedValue = value.ToString();
                else
                    this.ddlIncluyeLlantas.SelectedIndex = 0;
            }
        }
        public string DireccionAlmacenaje
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtDireccionAlmacenaje.Text)) ? null : this.txtDireccionAlmacenaje.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtDireccionAlmacenaje.Text = value;
                else
                    this.txtDireccionAlmacenaje.Text = string.Empty;
            }
        }

        /// <summary>
        /// Titulo del Dato Adicional
        /// </summary>
        public string DatoAdicionalTitulo
        {
            get
            {
                if (this.txtTitulo.Text != null && !string.IsNullOrEmpty(this.txtTitulo.Text.Trim()))
                    return this.txtTitulo.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                this.txtTitulo.Text = (value != null && !string.IsNullOrEmpty(value.Trim())) ? value.Trim() : string.Empty;
            }
        }
        /// <summary>
        /// Descripcion del Dato Adicional
        /// </summary>
        public string DatoAdicionalDescripcion
        {
            get
            {
                if (this.txtDescripcion.Text != null && !string.IsNullOrEmpty(this.txtDescripcion.Text.Trim()))
                    return this.txtDescripcion.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                this.txtDescripcion.Text = (value != null && !string.IsNullOrEmpty(value.Trim())) ? value.Trim() : string.Empty;
            }
        }
        /// <summary>
        /// Indica si el Dato Adicional es una Observacion
        /// </summary>
        public bool? DatoAdicionalEsObservacion
        {
            get
            {
                return this.cbEsObservacion.Checked;
            }
            set
            {
                if (value != null)
                    this.cbEsObservacion.Checked = value.Value;
                else
                    this.cbEsObservacion.Checked = false;
            }
        }
        /// <summary>
        /// Listado de Datos Adicionales
        /// </summary>
        public List<DatoAdicionalAnexoBO> DatosAdicionales
        {
            get
            {
                if (Session["DatosAdicionalesContratoCM"] == null)
                    Session["DatosAdicionalesContratoCM"] = new List<DatoAdicionalAnexoBO>();

                return (List<DatoAdicionalAnexoBO>)Session["DatosAdicionalesContratoCM"];
            }
            set
            {
                Session["DatosAdicionalesContratoCM"] = value ?? new List<DatoAdicionalAnexoBO>();
                this.grdDatosAdicionales.DataSource = Session["DatosAdicionalesContratoCM"];
                this.grdDatosAdicionales.DataBind();
            }
        }

        public string Observaciones
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtObservaciones.Text)) ? null : this.txtObservaciones.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtObservaciones.Text = value;
                else
                    this.txtObservaciones.Text = string.Empty;
            }
        }

        public DateTime? FechaFinalizacion
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.hdnFechaFinalizacion.Value))
                    temp = DateTime.Parse(this.hdnFechaFinalizacion.Value);
                return temp;
            }
            set
            {
                if (value != null)
                    this.hdnFechaFinalizacion.Value = value.Value.ToString();
                else
                    this.hdnFechaFinalizacion.Value = string.Empty;
            }
        }
        public int? UsuarioFinalizacionID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUsuarioFinalizacionID.Value))
                    id = int.Parse(this.hdnUsuarioFinalizacionID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUsuarioFinalizacionID.Value = value.ToString();
                else
                    this.hdnUsuarioFinalizacionID.Value = string.Empty;
            }
        }
        public string ObservacionesFinalizacion
        {
            get
            {
                return (String.IsNullOrEmpty(this.hdnObservacionesFinalizacion.Value)) ? null : this.hdnObservacionesFinalizacion.Value.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.hdnObservacionesFinalizacion.Value = value;
                else
                    this.hdnObservacionesFinalizacion.Value = string.Empty;
            }
        }
        public string MotivoFinalizacion
        {
            get
            {
                return (String.IsNullOrEmpty(this.hdnMotivoFinalizacion.Value)) ? null : this.hdnMotivoFinalizacion.Value.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.hdnMotivoFinalizacion.Value = value;
                else
                    this.hdnMotivoFinalizacion.Value = string.Empty;
            }
        }
        //Identificador de Dirección de Cliente
        public int? DireccionClienteID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnDireccionClienteID.Value))
                    id = int.Parse(this.hdnDireccionClienteID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnDireccionClienteID.Value = value.ToString();
                else
                    this.hdnDireccionClienteID.Value = string.Empty;
            }
        }
        #endregion

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
        /// Obtiene o establece el número económico de la unidad que será rentada
        /// </summary>
        public string NumeroSerie
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadVIN.Text)) ? null : this.txtUnidadVIN.Text.Trim().ToUpper();
            }
            set
            {
                this.txtUnidadVIN.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }

        #region Propiedades de IucLineaContratoManttoVIS
        bool IucLineaContratoManttoVIS.ModoEdicion
        {
            get
            {
                bool id = false;
                if (!String.IsNullOrEmpty(this.hdnModoEdicionLineaContrato.Value))
                    id = bool.Parse(this.hdnModoEdicionLineaContrato.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnModoEdicionLineaContrato.Value = value.ToString();
                else
                    this.hdnModoEdicionLineaContrato.Value = false.ToString();
            }
        }

        public int? LineaContratoID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnLineaContratoID.Value))
                    id = int.Parse(this.hdnLineaContratoID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnLineaContratoID.Value = value.ToString();
                else
                    this.hdnLineaContratoID.Value = string.Empty;
            }
        }
        public string VIN
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadVIN.Text)) ? null : this.txtUnidadVIN.Text.Trim().ToUpper();
            }
            set
            {
                this.txtUnidadVIN.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public int? Anio
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtUnidadAnio.Text))
                    id = int.Parse(this.txtUnidadAnio.Text.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.txtUnidadAnio.Text = value.ToString();
                else
                    this.txtUnidadAnio.Text = string.Empty;
            }
        }
        string IucLineaContratoManttoVIS.SucursalNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadSucursalNombre.Text)) ? null : this.txtUnidadSucursalNombre.Text.Trim().ToUpper();
            }
            set
            {
                this.txtUnidadSucursalNombre.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public string ModeloNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadModeloNombre.Text)) ? null : this.txtUnidadModeloNombre.Text.Trim().ToUpper();
            }
            set
            {
                this.txtUnidadModeloNombre.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public string MarcaNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadMarcaNombre.Text)) ? null : this.txtUnidadMarcaNombre.Text.Trim().ToUpper();
            }
            set
            {
                this.txtUnidadMarcaNombre.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public string PlacaEstatal
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadPlacaEstatal.Text)) ? null : this.txtUnidadPlacaEstatal.Text.Trim().ToUpper();
            }
            set
            {
                this.txtUnidadPlacaEstatal.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public string PlacaFederal
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadPlacaFederal.Text)) ? null : this.txtUnidadPlacaFederal.Text.Trim().ToUpper();
            }
            set
            {
                this.txtUnidadPlacaFederal.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public decimal? CapacidadCarga
        {
            get
            {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtUnidadCapacidadCarga.Text))
                    temp = Decimal.Parse(this.txtUnidadCapacidadCarga.Text.Trim().Replace(",", ""));
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtUnidadCapacidadCarga.Text = string.Format("{0:#,##0.00##}", value);
                else
                    this.txtUnidadCapacidadCarga.Text = string.Empty;
            }
        }
        public decimal? CapacidadTanque
        {
            get
            {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtUnidadCapacidadTanque.Text))
                    temp = Decimal.Parse(this.txtUnidadCapacidadTanque.Text.Trim().Replace(",", ""));
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtUnidadCapacidadTanque.Text = string.Format("{0:#,##0.00##}", value);
                else
                    this.txtUnidadCapacidadTanque.Text = string.Empty;
            }
        }
        public decimal? RendimientoTanque
        {
            get
            {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtUnidadRendimientoTanque.Text))
                    temp = Decimal.Parse(this.txtUnidadRendimientoTanque.Text.Trim().Replace(",", ""));
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtUnidadRendimientoTanque.Text = string.Format("{0:#,##0.00##}", value);
                else
                    this.txtUnidadRendimientoTanque.Text = string.Empty;
            }
        }
        public int? KmEstimadoAnual
        {
            get
            {
                int? capacidad = null;
                if (!String.IsNullOrEmpty(this.txtUnidadKmEstimadoAnual.Text.Trim()))
                    capacidad = int.Parse(this.txtUnidadKmEstimadoAnual.Text.Trim().Replace(",", ""));
                return capacidad;
            }
            set { this.txtUnidadKmEstimadoAnual.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }
        public int? CobrableID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnCobrableID.Value))
                    id = int.Parse(this.hdnCobrableID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnCobrableID.Value = value.ToString();
                else
                    this.hdnCobrableID.Value = string.Empty;
            }
        }
        public decimal? CargoFijoMensual
        {
            get
            {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtUnidadCargoFijoMensual.Text))
                    temp = Decimal.Parse(this.txtUnidadCargoFijoMensual.Text.Trim().Replace(",", ""));
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtUnidadCargoFijoMensual.Text = string.Format("{0:#,##0.00##}", value);
                else
                    this.txtUnidadCargoFijoMensual.Text = string.Empty;
            }
        }

        public int? KilometrosLibres
        {
            get
            {
                int val = 0;

                if (Int32.TryParse(this.txtKilometrosLibres.Text, out val))
                    return val;
                return null;
            }
            set { this.txtKilometrosLibres.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        public decimal? CostoKmRecorrido
        {
            get
            {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtUnidadCostoKmRecorrido.Text))
                    temp = Decimal.Parse(this.txtUnidadCostoKmRecorrido.Text.Trim().Replace(",", ""));
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtUnidadCostoKmRecorrido.Text = string.Format("{0:#,##0.00##}", value);
                else
                    this.txtUnidadCostoKmRecorrido.Text = string.Empty;
            }
        }

        public int? HorasLibres
        {
            get
            {
                int val = 0;
                if (int.TryParse(this.txtHorasLibres.Text, out val))
                    return val;
                return null;
            }
            set { this.txtHorasLibres.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        public decimal? CostoHorasRefrigeradas
        {
            get
            {
                decimal val = new decimal(0.00);

                if (decimal.TryParse(this.txtCostoHoraRefrigerada.Text.Trim().Replace(",", ""), out val))
                    return val;

                return null;
            }
            set
            {
                this.txtCostoHoraRefrigerada.Text = value.HasValue ? string.Format("{0:#,##0.00##}", value.Value)  : string.Empty;
            }
        }

        public string NumeroEconomico
        {
            get
            {
                return !string.IsNullOrEmpty(this.txtUnidadNumeroEconomico.Text) && !string.IsNullOrWhiteSpace(this.txtUnidadNumeroEconomico.Text)
                           ? this.txtUnidadNumeroEconomico.Text.Trim().ToUpper()
                           : null;
            }
            set
            {
                this.txtUnidadNumeroEconomico.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                         ? value.Trim().ToUpper()
                                                         : string.Empty;
            }
        }
        public List<SubLineaContratoManttoBO> SubLineasContrato
        {
            get
            {
                if (Session["SubLineasContratoCM"] == null)
                    Session["SubLineasContratoCM"] = new List<SubLineaContratoManttoBO>();

                return (List<SubLineaContratoManttoBO>)Session["SubLineasContratoCM"];
            }
            set
            {
                Session["SubLineasContratoCM"] = value ?? new List<SubLineaContratoManttoBO>();
                this.grdUnidadEquiposAliados.DataSource = value;
                this.grdUnidadEquiposAliados.DataBind();
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
        #endregion

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

        #region SC_0051

        public decimal? CostoFijoMensualEA
        {
            get
            {
                decimal val;

                if (decimal.TryParse(this.txtCargoFijoMensualEA.Text.Replace(",", ""), out val))
                    return val;

                return null;
            }
            set { this.txtCargoFijoMensualEA.Text = value.HasValue ? string.Format("{0:#,##0.0000}", value) : string.Empty; }
        }

        public int? KilometrosLibresEA
        {
            get
            {
                int val = 0;

                if (int.TryParse(this.txtKilometrosLibresEA.Text, out val))
                    return val;

                return null;
            }
            set { this.txtKilometrosLibresEA.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        /// <summary>
        /// Obtiene o establece el costo por cada kilometro recorrido por el equipo aliado
        /// </summary>
        public decimal? CostoKilometroEA
        {
            get
            {
                decimal val;

                if (decimal.TryParse(this.txtCostoKilometroEA.Text.Replace(",",""), out val))
                    return val;

                return null;
            }
            set { this.txtCostoKilometroEA.Text = value.HasValue ? string.Format("{0:#,##0.0000}", value) : string.Empty; }
        }
        public int? HorasLibresEA
        {
            get 
            { 
                int val = 0;

                if (int.TryParse(this.txtHorasLibresEA.Text, out val))
                    return val;

                return null;
            }
            set { this.txtHorasLibresEA.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o estable el costo por cada hora de refrigeración del equipo aliado
        /// </summary>
        public decimal? CostoHoraRefrigeradaEA
        {
            get
            {
                decimal val;

                if (decimal.TryParse(this.txtCostoHoraRefrigeradaEA.Text.Replace(",",""), out val))
                    return val;

                return null;
            }
            set { this.txtCostoHoraRefrigeradaEA.Text = value.HasValue ? string.Format("{0:#,##0.0000}", value) : string.Empty; }
        }

        public int? EquipoAliadoID
        {
            get
            {
                int val;

                if (Int32.TryParse(this.hdnEquipoAliadoID.Value, out val))
                    return val;

                return null;
            }
            set { this.hdnEquipoAliadoID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        public bool? MantenimientoEA
        {
            get { return this.chkMantenimientoEA.Checked; }
            set
            {
                this.chkMantenimientoEA.Checked = value.HasValue && (value.Value ? true : false);
            }
        }

        public int? PeriodoTarifaKM
        {
            get
            {
                int val = 0;

                if (Int32.TryParse(this.ddlPeriodoTarifaKM.SelectedValue, out val))
                    return val;

                return null;
            }
            set { this.ddlPeriodoTarifaKM.SelectedValue = value.HasValue ? value.Value.ToString() : "-1";
            }
        }
        public int? PeriodoTarifaHRS
        {
            get
            {
                int val = 0;

                if (Int32.TryParse(this.ddlPeriodoTarifaHRS.SelectedValue, out val))
                    return val;

                return null;
            }
            set
            {
                this.ddlPeriodoTarifaHRS.SelectedValue = value.HasValue ? value.Value.ToString() : "-1";
            }
        }
        public int? PeriodoTarifaKMEA
        {
            get
            {
                int val = 0;

                if (Int32.TryParse(this.ddlPeriodoTarifaKMEA.SelectedValue, out val))
                    return val < 0 ? null : (int?)val;

                return null;
            }
            set
            {
                this.ddlPeriodoTarifaKMEA.SelectedValue = value.HasValue ? value.Value.ToString() : "-1";
            }
        }
        public int? PeriodoTarifaHRSEA
        {
            get
            {
                int val = 0;

                if (Int32.TryParse(this.ddlPeriodoTarifaHRSEA.SelectedValue, out val))
                    return val < 0 ? null : (int?)val;

                return null;
            }
            set
            {
                this.ddlPeriodoTarifaHRSEA.SelectedValue = value.HasValue ? value.Value.ToString() : "-1";
            }
        }
        #endregion
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new ucContratoManttoPRE(this, this);

                this.txtDireccionAlmacenaje.Attributes.Add("onkeyup", "checkText(this,500);");
                this.txtObservaciones.Attributes.Add("onkeyup", "checkText(this,300);");
                this.txtDescripcion.Attributes.Add("onkeyup", "checkText(this,300);");
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear de página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.txtCodigoPostal.Text = "";
            this.txtComisionApertura.Text = "";
            this.txtDepositoGarantia.Text = "";
            this.txtDescripcion.Text = "";
            this.txtDireccionAlmacenaje.Text = "";
            this.txtDireccionEmpresa.Text = "";
            this.txtDomicilioCliente.Text = "";
            this.txtEmpresa.Text = "";
            this.txtFechaContrato.Text = "";
            this.txtFechaFinalizacion.Text = "";
            this.txtFechaInicioContrato.Text = "";
            this.txtNombreCliente.Text = "";
            this.txtNumeroCuentaOracle.Text = "";
            this.txtObservaciones.Text = "";
            this.txtPlazo.Text = "";
            this.txtRepresentante.Text = "";
            this.txtSucursal.Text = "";
            this.txtTitulo.Text = "";
            this.txtUbicacionTallerServicio.Text = "";
            this.txtUnidadAnio.Text = "";
            this.txtUnidadCapacidadCarga.Text = "";
            this.txtUnidadCapacidadTanque.Text = "";
            this.txtUnidadCargoFijoMensual.Text = "";
            this.txtUnidadCostoKmRecorrido.Text = "";
            this.txtUnidadKmEstimadoAnual.Text = "";
            this.txtUnidadMarcaNombre.Text = "";
            this.txtUnidadModeloNombre.Text = "";
            this.txtUnidadNumeroEconomico.Text = "";
            this.txtUnidadPlacaEstatal.Text = "";
            this.txtUnidadPlacaFederal.Text = "";
            this.txtUnidadRendimientoTanque.Text = "";
            this.txtUnidadSucursalNombre.Text = "";
            this.txtUnidadVIN.Text = "";
            this.txtClaveProductoServicio.Text = "";

            this.ddlAvales.SelectedIndex = -1;
            this.ddlIncluyeLavado.SelectedIndex = -1;
            this.ddlIncluyeLlantas.SelectedIndex = -1;
            this.ddlIncluyeRotulacionPintura.SelectedIndex = -1;
            this.ddlIncluyeSeguro.SelectedIndex = -1;
            this.ddlMonedas.SelectedIndex = -1;
            this.ddlObligadosSolidarios.SelectedIndex = -1;
            this.ddlRepresentantesLegales.SelectedIndex = -1;

            this.rbtEsFisico.SelectedIndex = -1;

            this.hdnCalle.Value = "";
            this.hdnCiudad.Value = "";
            this.hdnClienteID.Value = "";
            this.hdnColonia.Value = "";
            this.hdnContratoID.Value = "";
            this.hdnCuentaClienteID.Value = "";
            this.hdnEmpresaID.Value = "";
            this.hdnEquipoID.Value = "";
            this.hdnEstado.Value = "";
            this.hdnEstatusContratoID.Value = "";
            this.hdnFechaFinalizacion.Value = "";
            this.hdnLineaContratoIndex.Value = "";
            this.hdnMotivoFinalizacion.Value = "";
            this.hdnMunicipio.Value = "";
            this.hdnNumeroContrato.Value = "";
            this.hdnObservacionesFinalizacion.Value = "";
            this.hdnPais.Value = "";
            this.hdnRepresentanteObligadoSeleccionadoID.Value = "";
            this.hdnSucursalID.Value = "";
            this.hdnTipoContratoID.Value = "";
            this.hdnTipoCuentaRegion.Value = "";
            this.hdnUnidadID.Value = "";
            this.hdnUsuarioFinalizacionID.Value = "";
            this.hdnDireccionClienteID.Value = "";
            this.hdnProductoServicioId.Value = "";

            this.grdDatosAdicionales.DataSource = null;
            this.grdDatosAdicionales.DataBind();
            this.grdLineasContrato.DataSource = null;
            this.grdLineasContrato.DataBind();
            this.grdObligadosSolidarios.DataSource = null;
            this.grdObligadosSolidarios.DataBind();
            this.grdAvales.DataSource = null;
            this.grdAvales.DataBind();
            this.grdRepresentantesAval.DataSource = null;
            this.grdRepresentantesAval.DataBind();
            this.grdRepresentantesLegales.DataSource = null;
            this.grdRepresentantesLegales.DataBind();
            this.grdRepresentantesDialog.DataSource = null;
            this.grdRepresentantesDialog.DataBind();
            this.grdRepresentantesObligadoSolidario.DataSource = null;
            this.grdRepresentantesObligadoSolidario.DataBind();
            this.grdUnidadEquiposAliados.DataSource = null;
            this.grdUnidadEquiposAliados.DataBind();

            ((IucContratoManttoVIS)this).LimpiarSesion();
            ((IucLineaContratoManttoVIS)this).LimpiarSesion();
        }
        void IucContratoManttoVIS.PrepararEdicion()
        {
            ((IucContratoManttoVIS)this).LimpiarSesion();
            ((IucLineaContratoManttoVIS)this).LimpiarSesion();
        }
        void IucContratoManttoVIS.PrepararVisualizacion()
        {
            this.txtCodigoPostal.Enabled = false;
            this.txtComisionApertura.Enabled = false;
            this.txtDepositoGarantia.Enabled = false;
            this.txtDescripcion.Enabled = false;
            this.txtDireccionAlmacenaje.Enabled = false;
            this.txtDireccionEmpresa.Enabled = false;
            this.txtDomicilioCliente.Enabled = false;
            this.txtEmpresa.Enabled = false;
            this.txtFechaContrato.Enabled = false;
            this.txtFechaFinalizacion.Enabled = false;
            this.txtFechaInicioContrato.Enabled = false;
            this.txtNombreCliente.Enabled = false;
            this.txtObservaciones.Enabled = false;
            this.txtPlazo.Enabled = false;
            this.txtRepresentante.Enabled = false;
            this.txtSucursal.Enabled = false;
            this.txtTitulo.Enabled = false;
            this.txtUbicacionTallerServicio.Enabled = false;
            this.txtUnidadAnio.Enabled = false;
            this.txtUnidadCapacidadCarga.Enabled = false;
            this.txtUnidadCapacidadTanque.Enabled = false;
            this.txtUnidadCargoFijoMensual.Enabled = false;
            this.txtUnidadCostoKmRecorrido.Enabled = false;
            this.txtUnidadKmEstimadoAnual.Enabled = false;
            this.txtUnidadMarcaNombre.Enabled = false;
            this.txtUnidadModeloNombre.Enabled = false;
            this.txtUnidadNumeroEconomico.Enabled = false;
            this.txtUnidadPlacaEstatal.Enabled = false;
            this.txtUnidadPlacaFederal.Enabled = false;
            this.txtUnidadRendimientoTanque.Enabled = false;
            this.txtUnidadSucursalNombre.Enabled = false;
            this.txtUnidadVIN.Enabled = false;

            this.ddlAvales.Enabled = false;
            this.ddlIncluyeLavado.Enabled = false;
            this.ddlIncluyeLlantas.Enabled = false;
            this.ddlIncluyeRotulacionPintura.Enabled = false;
            this.ddlIncluyeSeguro.Enabled = false;
            this.ddlMonedas.Enabled = false;
            this.ddlObligadosSolidarios.Enabled = false;
            this.ddlRepresentantesLegales.Enabled = false;

            this.rbtEsFisico.Enabled = false;
            this.cbObligadosComoAvales.Enabled = false;
            this.cbSoloRepresentantes.Enabled = false;

            this.ibtnBuscarCliente.Visible = false;
            this.ibtnBuscarDirieccionCliente.Visible = false;
            this.ibtnBuscarSucursal.Visible = false;
            this.ibtnBuscarUnidad.Visible = false;
            this.ibtnBuscarCliente.Visible = false;

            this.tbDatoAdicional.Visible = false;
            this.tbAval.Visible = false;
            this.tbObligadoSolidario.Visible = false;
            this.tbRepresentanteLegal.Visible = false;

            this.grdDatosAdicionales.Columns[3].Visible = false;
            this.grdLineasContrato.Columns[5].Visible = false;
            this.grdLineasContrato.Columns[6].Visible = false;
            this.grdRepresentantesLegales.Columns[1].Visible = false;
            this.grdObligadosSolidarios.Columns[3].Visible = false;
            this.grdAvales.Columns[3].Visible = false;

            ((IucContratoManttoVIS)this).LimpiarSesion();
            ((IucLineaContratoManttoVIS)this).LimpiarSesion();
        }
        void IucLineaContratoManttoVIS.PrepararEdicion()
        {
            this.txtUnidadCargoFijoMensual.Enabled = true;
            this.txtKilometrosLibres.Enabled = true;//SC_0051
            this.txtUnidadCostoKmRecorrido.Enabled = true;
            this.txtUnidadKmEstimadoAnual.Enabled = true;
            this.txtHorasLibres.Enabled = true;//SC_0051
            this.txtCostoHoraRefrigerada.Enabled = true;//SC_0051
            this.txtClaveProductoServicio.Enabled = true;
            this.ddlPeriodoTarifaHRS.Enabled = true;
            this.ddlPeriodoTarifaHRSEA.Enabled = true;
            this.ddlPeriodoTarifaKM.Enabled = true;
            this.ddlPeriodoTarifaKMEA.Enabled = true;
            this.grdUnidadEquiposAliados.Enabled = true;

            this.btnCancelarLineaContrato.Text = "Cancelar";
            this.ibtnBuscarProductoServicio.Visible = true;

            ((IucLineaContratoManttoVIS)this).LimpiarSesion();
        }
        void IucLineaContratoManttoVIS.PrepararVisualizacion()
        {
            this.txtUnidadCargoFijoMensual.Enabled = false;
            this.txtKilometrosLibres.Enabled = false;//SC_0051
            this.txtUnidadCostoKmRecorrido.Enabled = false;
            this.txtUnidadKmEstimadoAnual.Enabled = false;
            this.txtHorasLibres.Enabled = false;//SC_0051
            this.txtCostoHoraRefrigerada.Enabled = false;//SC_0051
            this.txtClaveProductoServicio.Enabled = false;
            this.ddlPeriodoTarifaHRS.Enabled = false;
            this.ddlPeriodoTarifaHRSEA.Enabled = false;
            this.ddlPeriodoTarifaKM.Enabled = false;
            this.ddlPeriodoTarifaKMEA.Enabled = false;

            this.btnCancelarLineaContrato.Text = "Cerrar";
            this.ibtnBuscarProductoServicio.Visible = false;

            ((IucLineaContratoManttoVIS)this).LimpiarSesion();
        }

        public void PermitirSeleccionarDireccionCliente(bool permitir)
        {
            this.ibtnBuscarDirieccionCliente.Enabled = permitir;
            this.ibtnBuscarDirieccionCliente.Visible = permitir;
        }
        public void PermitirSeleccionarUnidad(bool permitir)
        {
            this.txtUnidadVIN.Enabled = permitir;
            this.ibtnBuscarUnidad.Enabled = permitir;
            this.ibtnBuscarUnidad.Visible = permitir;
        }
        public void PermitirSeleccionarRepresentantes(bool permitir)
        {
            this.ddlRepresentantesLegales.Enabled = permitir;
            this.pnlRepresentantesLegales.Visible = permitir;
        }
        public void PermitirAgregarRepresentantes(bool permitir)
        {
            this.btnAgregarRepresentante.Enabled = permitir;
            this.btnAgregarRepresentante.Visible = permitir;
        }
        public void PermitirSeleccionarObligadosSolidarios(bool permitir)
        {
            this.ddlObligadosSolidarios.Enabled = permitir;
        }
        public void PermitirAgregarObligadosSolidarios(bool permitir)
        {
            this.btnAgregarObligadoSolidario.Enabled = permitir;
            this.btnAgregarObligadoSolidario.Visible = permitir;
        }
        public void PermitirSeleccionarAvales(bool permitir)
        {
            this.ddlAvales.Enabled = permitir;
        }
        public void PermitirAgregarAvales(bool permitir)
        {
            this.btnAgregarAval.Enabled = permitir;
            this.btnAgregarAval.Visible = permitir;
        }

        public void PermitirAsignarTarifasEA(bool permitir)
        {
            this.txtCargoFijoMensualEA.Enabled = permitir;
            this.txtKilometrosLibresEA.Enabled = permitir;
            this.txtCostoKilometroEA.Enabled = permitir;
            this.txtHorasLibresEA.Enabled = permitir;
            this.txtCostoHoraRefrigeradaEA.Enabled = permitir;            
            this.chkMantenimientoEA.Enabled = permitir;
            this.cmdAceptarTarifaEA.Enabled = permitir;
            this.ddlPeriodoTarifaHRSEA.Enabled = permitir;
            this.ddlPeriodoTarifaKMEA.Enabled = permitir;
            this.cmdAceptarTarifaEA.Visible = permitir;            
        }

        public void MostrarPersonasCliente(bool mostrar)
        {
            this.pnlPersonasCliente.Visible = mostrar;
        }
        public void MostrarObligadosSolidarios(bool mostrar)
        {
            this.pnlObligadosSolidarios.Visible = mostrar;
        }
        public void MostrarAvales(bool mostrar)
        {
            this.pnlAvales.Visible = mostrar;
        }
        public void MostrarRepresentantesObligado(bool mostrar)
        {
            this.trRepresentantesObligado.Visible = mostrar;
        }
        public void MostrarRepresentantesAval(bool mostrar)
        {
            this.trRepresentantesAval.Visible = mostrar;
        }
        public void MostrarNumeroEconomico(bool mostrar)
        {
            this.tbSeleccionarUnidad.Visible = mostrar;
        }
        public void MostrarLineaContrato(bool mostrar)
        {
            this.pnlLineaContrato.Visible = mostrar;
        }
        public void MostrarDetalleRepresentantesObligado(List<RepresentanteLegalBO> representantes)
        {
            this.grdRepresentantesDialog.DataSource = representantes;
            this.grdRepresentantesDialog.DataBind();

            this.RegistrarScript("DetalleObligado", "abrirDetalleRepresentantes('OBLIGADO SOLIDARIO');");
        }
        public void MostrarDetalleRepresentantesAval(List<RepresentanteLegalBO> representantes)
        {
            this.grdRepresentantesDialog.DataSource = representantes;
            this.grdRepresentantesDialog.DataBind();

            this.RegistrarScript("DetalleObligado", "abrirDetalleRepresentantes('AVAL');");
        }
        public void MostrarDatosAdicionales(bool mostrar)
        {
            this.fsDatosAdicionales.Visible = mostrar;
        }

        public void EstablecerOpcionesMoneda(Dictionary<string, string> monedas)
        {
            if (ReferenceEquals(monedas, null))
                monedas = new Dictionary<string, string>();

            this.ddlMonedas.Items.Clear();
            this.ddlMonedas.Items.Add(new ListItem("Seleccione una opción", "0"));

            this.ddlMonedas.DataSource = monedas;
            this.ddlMonedas.DataValueField = "key";
            this.ddlMonedas.DataTextField = "value";
            this.ddlMonedas.DataBind();
        }
        public void EstablecerOpcionesIncluyeSeguro(Dictionary<int, string> tiposInclucion)
        {
            if (ReferenceEquals(tiposInclucion, null))
                tiposInclucion = new Dictionary<int, string>();

            this.ddlIncluyeSeguro.Items.Clear();
            this.ddlIncluyeSeguro.Items.Add(new ListItem("Seleccione una opción", "-1"));

            this.ddlIncluyeSeguro.DataSource = tiposInclucion;
            this.ddlIncluyeSeguro.DataValueField = "key";
            this.ddlIncluyeSeguro.DataTextField = "value";
            this.ddlIncluyeSeguro.DataBind();
        }
        public void EstablecerOpcionesIncluyeLavado(Dictionary<int, string> tiposInclucion)
        {
            if (ReferenceEquals(tiposInclucion, null))
                tiposInclucion = new Dictionary<int, string>();

            this.ddlIncluyeLavado.Items.Clear();
            this.ddlIncluyeLavado.Items.Add(new ListItem("Seleccione una opción", "-1"));

            this.ddlIncluyeLavado.DataSource = tiposInclucion;
            this.ddlIncluyeLavado.DataValueField = "key";
            this.ddlIncluyeLavado.DataTextField = "value";
            this.ddlIncluyeLavado.DataBind();
        }
        public void EstablecerOpcionesIncluyePinturaRotulacion(Dictionary<int, string> tiposInclucion)
        {
            if (ReferenceEquals(tiposInclucion, null))
                tiposInclucion = new Dictionary<int, string>();

            this.ddlIncluyeRotulacionPintura.Items.Clear();
            this.ddlIncluyeRotulacionPintura.Items.Add(new ListItem("Seleccione una opción", "-1"));

            this.ddlIncluyeRotulacionPintura.DataSource = tiposInclucion;
            this.ddlIncluyeRotulacionPintura.DataValueField = "key";
            this.ddlIncluyeRotulacionPintura.DataTextField = "value";
            this.ddlIncluyeRotulacionPintura.DataBind();
        }
        public void EstablecerOpcionesIncluyeLlantas(Dictionary<int, string> tiposInclucion)
        {
            if (ReferenceEquals(tiposInclucion, null))
                tiposInclucion = new Dictionary<int, string>();

            this.ddlIncluyeLlantas.Items.Clear();
            this.ddlIncluyeLlantas.Items.Add(new ListItem("Seleccione una opción", "-1"));

            this.ddlIncluyeLlantas.DataSource = tiposInclucion;
            this.ddlIncluyeLlantas.DataValueField = "key";
            this.ddlIncluyeLlantas.DataTextField = "value";
            this.ddlIncluyeLlantas.DataBind();
        }
        /// <summary>
        /// Establece las opciones para la lista de frecuencias
        /// </summary>
        /// <param name="frecuencias">Diccionario con las opciones para las frecuencias de las tarifas del contrato</param>
        public void EstablecerOpcionesFrecuencia(Dictionary<int, string> frecuencias)//SC_0051
        {
            if (ReferenceEquals(frecuencias, null))
                frecuencias = new Dictionary<int, string>();

            //Periodo para las horas
            this.ddlPeriodoTarifaHRS.Items.Clear();
            this.ddlPeriodoTarifaHRS.DataSource = frecuencias;
            this.ddlPeriodoTarifaHRS.DataValueField = "key";
            this.ddlPeriodoTarifaHRS.DataTextField = "value";
            this.ddlPeriodoTarifaHRS.DataBind();

            //Período para los kilometros
            this.ddlPeriodoTarifaKM.Items.Clear();
            this.ddlPeriodoTarifaKM.DataSource = frecuencias;
            this.ddlPeriodoTarifaKM.DataValueField = "key";
            this.ddlPeriodoTarifaKM.DataTextField = "value";
            this.ddlPeriodoTarifaKM.DataBind();

            //Período para las horas de EA
            this.ddlPeriodoTarifaHRSEA.Items.Clear();
            this.ddlPeriodoTarifaHRSEA.DataSource = frecuencias;
            this.ddlPeriodoTarifaHRSEA.DataValueField = "key";
            this.ddlPeriodoTarifaHRSEA.DataTextField = "value";
            this.ddlPeriodoTarifaHRSEA.DataBind();

            //período par los kilometros de los EA
            this.ddlPeriodoTarifaKMEA.Items.Clear();
            this.ddlPeriodoTarifaKMEA.DataSource = frecuencias;
            this.ddlPeriodoTarifaKMEA.DataValueField = "key";
            this.ddlPeriodoTarifaKMEA.DataTextField = "value";
            this.ddlPeriodoTarifaKMEA.DataBind();
        }

        public void ActualizarRepresentantesLegales()
        {
            this.grdRepresentantesLegales.DataSource = this.RepresentantesSeleccionados;
            this.grdRepresentantesLegales.DataBind();
        }
        public void ActualizarObligadosSolidarios()
        {
            this.grdObligadosSolidarios.DataSource = this.ObligadosSolidariosSeleccionados;
            this.grdObligadosSolidarios.DataBind();
        }
        public void ActualizarAvales()
        {
            this.grdAvales.DataSource = this.AvalesSeleccionados;
            this.grdAvales.DataBind();
        }
        public void ActualizarLineasContrato()
        {
            this.grdLineasContrato.DataSource = this.LineasContrato;
            this.grdLineasContrato.DataBind();
        }
        public void ActualizarDatosAdicionales()
        {
            this.grdDatosAdicionales.DataSource = this.DatosAdicionales;
            this.grdDatosAdicionales.DataBind();
        }

        void IucContratoManttoVIS.LimpiarSesion()
        {
            if (Session["ListadoRepresentantesLegalesCM"] != null)
                Session.Remove("ListadoRepresentantesLegalesCM");
            if (Session["RepresentantesLegalesContratoCM"] != null)
                Session.Remove("RepresentantesLegalesContratoCM");
            if (Session["ListadoObligadosSolidariosCM"] != null)
                Session.Remove("ListadoObligadosSolidariosCM");
            if (Session["ObligadosSolidariosContratoCM"] != null)
                Session.Remove("ObligadosSolidariosContratoCM");
            if (Session["ListaRepresentantesObligadosSolidarioCM"] != null)
                Session.Remove("ListaRepresentantesObligadosSolidarioCM");
            if (Session["RepresentantesObligSolidarioCM"] != null)
                Session.Remove("RepresentantesObligSolidarioCM");
            if (Session["ListadoAvalesCM"] != null)
                Session.Remove("ListadoAvalesCM");
            if (Session["AvalesContratoCM"] != null)
                Session.Remove("AvalesContratoCM");
            if (Session["ListaRepresentantesAvalCM"] != null)
                Session.Remove("ListaRepresentantesAvalCM");
            if (Session["RepresentantesAvalCM"] != null)
                Session.Remove("RepresentantesAvalCM");
            if (Session["ListadoLineasContratoCM"] != null)
                Session.Remove("ListadoLineasContratoCM");
            if (Session["DatosAdicionalesContratoCM"] != null)
                Session.Remove("DatosAdicionalesContratoCM");
        }
        void IucLineaContratoManttoVIS.LimpiarSesion()
        {
            if (Session["SubLineasContratoCM"] != null)
                Session.Remove("SubLineasContratoCM");
        }

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

        #region SC_0051
        /// <summary>
        /// Despliega en pantalla la tabla para la captura de las tarifas del equipo aliado
        /// </summary>
        public void MostrarTarifasEquipoAliado()
        {
            this.RegistrarScript("TARIFAEQUIPOALIADO", "abrirTarifasEquipoAliado();");
        }
        /// <summary>
        /// Actualiza la inforamción del grid de equipos aliados para la unidad seleccionada
        /// </summary>
        private void ActualizarEquiposAliados()
        {
            this.grdUnidadEquiposAliados.DataSource = this.SubLineasContrato;
            this.grdUnidadEquiposAliados.DataBind();
        }
        #endregion
        #endregion

        #region Eventos
        protected void btnAgregarRepresentante_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AgregarRepresentanteLegal();
                this.ddlRepresentantesLegales.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al agregar un Representante Legal al contrato", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnAgregarRepresentante_Click: " + ex.Message);
            }
        }
        protected void grdRepresentantesLegales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grdRepresentantesLegales.DataSource = this.RepresentantesSeleccionados;
                this.grdRepresentantesLegales.PageIndex = e.NewPageIndex;
                this.grdRepresentantesLegales.DataBind();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al cambiar de página en los representantes legales", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".grdRepresentantesLegales_PageIndexChanging: " + ex.Message);
            }
        }
        protected void grdRepresentantesLegales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "PAGE" || e.CommandName.ToUpper() == "SORT") return;
                int index = Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName.Trim())
                {
                    case "CMDELIMINAR":
                        this.presentador.QuitarRepresentanteLegal(index);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, this.nombreClase + ".grdRepresentantesLegales_RowCommand: " + ex.Message);
            }
        }

        protected void cbSoloRepresentantes_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ConfigurarOpcionesPersonas();

                this.ddlObligadosSolidarios.SelectedIndex = -1;
                this.ddlAvales.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, this.nombreClase + ".cbSoloRepresentantes_CheckedChanged: " + ex.Message);
            }
        }

        #region Eventos de Obligados Solidarios
        protected void btnAgregarObligadoSolidario_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AgregarObligadoSolidario();

                this.ddlObligadosSolidarios.SelectedIndex = -1;
                this.RepresentantesObligadoSeleccionados = null;
                this.RepresentantesObligadoTotales = null;
                this.MostrarRepresentantesObligado(false);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al agregar un Obligado Solidario al contrato", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnAgregarObligadoSolidario_Click: " + ex.Message);
            }
        }

        protected void ddlObligadosSolidarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.presentador.SeleccionarObligadoSolidario();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al seleccionar el obligado solidario", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ddlObligadosSolidarios_SelectedIndexChanged: " + ex.Message);
            }
        }
        protected void grdObligadosSolidarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grdObligadosSolidarios.DataSource = this.ObligadosSolidariosSeleccionados;
                this.grdObligadosSolidarios.PageIndex = e.NewPageIndex;
                this.grdObligadosSolidarios.DataBind();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al cambiar la página de los obligados solidarios", ETipoMensajeIU.ERROR, this.nombreClase + ".grdObligadosSolidarios_PageIndexChanging: " + ex.Message);
            }
        }
        protected void grdObligadosSolidarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "PAGE" || e.CommandName.ToUpper() == "SORT") return;
                int index = Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName.ToUpper().Trim())
                {
                    case "CMDELIMINAR":
                        this.presentador.QuitarObligadoSolidario(index);
                        break;
                    case "CMDDETALLE":
                        this.presentador.PrepararVisualizacionRepresentantesObligado(index);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al ejecutar la acción sobre el obligado solidario", ETipoMensajeIU.ERROR, this.nombreClase + ".grdObligadosSolidarios_RowCommand: " + ex.Message);
            }
        }
        protected void grdObligadosSolidarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType != DataControlRowType.DataRow) return;
                ObligadoSolidarioBO bo = e.Row.DataItem != null ? (ObligadoSolidarioBO)e.Row.DataItem : new ObligadoSolidarioProxyBO();
                if (!bo.TipoObligado.HasValue)
                    e.Row.FindControl("ibtDetalle").Visible = false;
                else if (bo.TipoObligado == ETipoObligadoSolidario.Fisico)
                    e.Row.FindControl("ibtDetalle").Visible = false;
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al desplegar los obligados solidarios", ETipoMensajeIU.ERROR, this.nombreClase + ".grdObligadosSolidarios_RowDataBound: " + ex.Message);
            }
        }

        protected void grdRepresentantesObligadoSolidario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    RepresentanteLegalBO persona = (RepresentanteLegalBO)e.Row.DataItem;
                    var chk = e.Row.FindControl("chkRepOS") as CheckBox;

                    if (chk != null)
                        chk.Checked = this.RepresentantesObligadoSeleccionados != null && this.RepresentantesObligadoSeleccionados.Exists(p => p.Id == persona.Id);
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias listar los representantes legales del obligado solidario", ETipoMensajeIU.ERROR, this.nombreClase + ".grdRepresentantesObligadoSolidario_RowDataBound: " + ex.Message);
            }
        }
        protected void grdRepresentantesObligadoSolidario_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grdRepresentantesObligadoSolidario.DataSource = this.RepresentantesObligadoTotales;
                this.grdRepresentantesObligadoSolidario.PageIndex = e.NewPageIndex;
                this.grdRepresentantesObligadoSolidario.DataBind();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al cambiar la página de los representantes legales del obligado solidario", ETipoMensajeIU.ERROR, this.nombreClase + ".grdRepresentantesObligadoSolidario_PageIndexChanging: " + ex.Message);
            }
        }
        protected void chkRepresentanteOS_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //Se obtiene el id de los controles
                CheckBox chk = (CheckBox)sender;
                GridViewRow row = (GridViewRow)chk.Parent.Parent;
                Label lbl = (Label)row.FindControl("lblRepresentanteOSID");

                int id;
                if (Int32.TryParse(lbl.Text, out id))
                {
                    this.hdnRepresentanteObligadoSeleccionadoID.Value = id.ToString();

                    if (chk.Checked)
                        this.presentador.AgregarRepresentanteObligado();
                    else
                        this.presentador.QuitarRepresentanteObligado();

                    this.hdnRepresentanteObligadoSeleccionadoID.Value = string.Empty;
                }
                else
                    throw new Exception("No se encontró el ID del representante legal del obligado solidario o tiene un dato inválido.");
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al seleccionar el representante legal para el obligado solidario", ETipoMensajeIU.ERROR, this.nombreClase + ".chkRepresentanteOS_CheckedChanged: " + ex.Message);
            }
        }
        #endregion

        protected void cbObligadosComoAvales_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ConfigurarOpcionesPersonas();

                this.ddlAvales.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, this.nombreClase + ".cbObligadosComoAvales_CheckedChanged: " + ex.Message);
            }
        }

        #region Eventos de Avales
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
                this.MostrarMensaje("Inconsistencias al agregar un Aval al contrato", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnAgregarAval_Click: " + ex.Message);
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
                this.MostrarMensaje("Inconsistencias al seleccionar el aval", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ddlAvales_SelectedIndexChanged: " + ex.Message);
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
                this.MostrarMensaje("Inconsistencias al cambiar la página de los avales", ETipoMensajeIU.ERROR, this.nombreClase + ".grdAvaless_PageIndexChanging: " + ex.Message);
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
                this.MostrarMensaje("Inconsistencias al ejecutar la acción sobre el aval", ETipoMensajeIU.ERROR, this.nombreClase + ".grdAvales_RowCommand: " + ex.Message);
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
                this.MostrarMensaje("Inconsistencias al desplegar los avales", ETipoMensajeIU.ERROR, this.nombreClase + ".grdAvales_RowDataBound: " + ex.Message);
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
                this.MostrarMensaje("Inconsistencias listar los representantes legales del aval", ETipoMensajeIU.ERROR, this.nombreClase + ".grdRepresentantesAval_RowDataBound: " + ex.Message);
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
                this.MostrarMensaje("Inconsistencias al cambiar la página de los obligados solidarios", ETipoMensajeIU.ERROR, this.nombreClase + ".grdObligadosSolidarios_PageIndexChanging: " + ex.Message);
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
                this.MostrarMensaje("Inconsistencias al seleccionar el representante legal para el aval", ETipoMensajeIU.ERROR, this.nombreClase + ".chkRepresentanteAval_CheckedChanged: " + ex.Message);
            }
        }
        #endregion

        protected void txtPlazo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CalcularFechaTerminacionContrato();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al calcular la fecha de terminación del contrato", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtPlazo_TextChanged: " + ex.Message);
            }
        }
        protected void txtFechaInicioContrato_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CalcularFechaTerminacionContrato();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al calcular la fecha de terminación del contrato", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtFechaInicioContrato_TextChanged: " + ex.Message);
            }
        }

        protected void chkMantenimiento_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //Se obtiene el id de los controles
                CheckBox chk = (CheckBox)sender;
                GridViewRow row = (GridViewRow)chk.Parent.Parent;
                Label lbl = (Label)row.FindControl("lblEquipoAliadoID");

                int id;
                if (Int32.TryParse(lbl.Text, out id))
                {
                    if (this.SubLineasContrato == null)
                        this.SubLineasContrato = new List<SubLineaContratoManttoBO>();

                    int index = this.SubLineasContrato.FindIndex(p => p.EquipoAliado.EquipoAliadoID == id);
                    this.SubLineasContrato[index].Mantenimiento = chk.Checked;
                    if (chk.Checked)
                    {
                        this.hdnEquipoAliadoID.Value = id.ToString();
                        this.txtCostoHoraRefrigeradaEA.Text =
                            this.SubLineasContrato[index].CargoHorasRefrigeradas.HasValue
                                ? this.SubLineasContrato[index].CargoHorasRefrigeradas.Value.ToString()
                                : string.Empty;
                        this.txtCostoKilometroEA.Text = this.SubLineasContrato[index].CargoKilometros.HasValue ? this.SubLineasContrato[index].CargoKilometros.Value.ToString() : string.Empty;
                        this.RegistrarScript("TarifasEquipoAliado", "abrirTarifasEquipoAliado();");
                    }
                    else
                    {
                        this.PermitirAsignarTarifasEA(true);
                        this.hdnEquipoAliadoID.Value = string.Empty;
                        this.txtCostoHoraRefrigeradaEA.Text = string.Empty;
                        this.txtCostoKilometroEA.Text = string.Empty;
                        this.SubLineasContrato[index].CargoHorasRefrigeradas = null;
                        this.SubLineasContrato[index].CargoKilometros = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al marcar para mantenimiento el equipo aliado", ETipoMensajeIU.ERROR, this.nombreClase + ".chkMantenimiento_CheckedChanged" + ex.Message);
            }
        }

        #region Eventos de Líneas de Contrato
        protected void btnAgregarLineaContrato_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AplicarLineaContrato();
                this.grdLineasContrato.Enabled = true;
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al agregar una unidad al contrato", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnAgregarLineaContrato_Click: " + ex.Message);
            }
        }
        protected void btnCancelarLineaContrato_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CancelarLinea();
                this.grdLineasContrato.Enabled = true;
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al cancelar la edición o vista de una unidad al contrato", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnCancelarLineaContrato_Click: " + ex.Message);
            }
        }
        protected void btnActualizarLineaContrato_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AplicarLineaContrato();
                this.grdLineasContrato.Enabled = true;
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al actualizar una unidad al contrato", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnActualizarLineaContrato_Click: " + ex.Message);
            }
        }
        protected void grdLineasContrato_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "PAGE" || e.CommandName.ToUpper() == "SORT") return;
                this.hdnLineaContratoIndex.Value = Convert.ToInt32(e.CommandArgument).ToString();

                switch (e.CommandName.ToUpper().Trim())
                {
                    case "CMDELIMINAR":
                        this.presentador.QuitarLineaContrato();
                        break;
                    case "CMDDETALLES":
                        this.presentador.PrepararVisualizacionLinea();

                        this.btnActualizarLineaContrato.Visible = false;
                        this.btnCancelarLineaContrato.Visible = true;
                        this.btnAgregarLineaContrato.Visible = false;
                        this.grdLineasContrato.Enabled = false;
                        break;
                    case "CMDEDITAR":
                        this.presentador.PrepararEdicionLinea();

                        this.btnActualizarLineaContrato.Visible = true;
                        this.btnCancelarLineaContrato.Visible = true;
                        this.btnAgregarLineaContrato.Visible = false;
                        this.grdLineasContrato.Enabled = false;
                        break;
                }

                this.hdnLineaContratoIndex.Value = "";
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al ejecutar la acción sobre la unidad del contrato", ETipoMensajeIU.ERROR, this.nombreClase + ".grdLineasContrato_RowCommand: " + ex.Message);
            }
        }
        protected void grdLineasContrato_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grdLineasContrato.DataSource = this.LineasContrato;
                this.grdLineasContrato.PageIndex = e.NewPageIndex;
                this.grdLineasContrato.DataBind();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al cambiar la página de las unidades del contrato", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".grdLineasContrato_PageIndexChanging: " + ex.Message);
            }
        }
        protected void grdLineasContrato_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType != DataControlRowType.DataRow) return;

                LineaContratoManttoBO bo = e.Row.DataItem != null ? (LineaContratoManttoBO)e.Row.DataItem : new LineaContratoManttoBO();
                if (bo.Cobrable != null && ((TarifaManttoBO)bo.Cobrable).CargoFijoMensual != null)
                    ((Label)e.Row.FindControl("lblCargoFijoMensual")).Text = string.Format("{0:#,##0.00##}", ((TarifaManttoBO)bo.Cobrable).CargoFijoMensual);
                else
                    ((Label)e.Row.FindControl("lblCargoFijoMensual")).Text = string.Empty;
                if (bo.Cobrable != null && ((TarifaManttoBO)bo.Cobrable).CargoKmRecorrido != null)
                    ((Label)e.Row.FindControl("lblCostoKmRecorrido")).Text = string.Format("{0:#,##0.00##}", ((TarifaManttoBO)bo.Cobrable).CargoKmRecorrido);
                else
                    ((Label)e.Row.FindControl("lblCostoKmRecorrido")).Text = string.Empty;
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al desplegar las unidades del contrato", ETipoMensajeIU.ERROR, this.nombreClase + ".grdLineasContrato_RowDataBound: " + ex.Message);
            }
        }
        #endregion

        protected void grdDatosAdicionales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grdDatosAdicionales.DataSource = DatosAdicionales;
                this.grdDatosAdicionales.PageIndex = e.NewPageIndex;
                this.grdDatosAdicionales.DataBind();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al cambiar la página de los datos Adicionales", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".grdDatosAdicionales_PageIndexChanging: " + ex.Message);
            }
        }
        protected void grdDatosAdicionales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                if (e.CommandName.ToUpper() == "PAGE" || e.CommandName.ToUpper() == "SORT") return;

                switch (e.CommandName.ToUpper())
                {
                    case "CMDELIMINAR":
                        this.presentador.QuitarDatoAdicional(index);
                        break;
                    default:
                        throw new Exception("La operación solicitada no está permitida en este módulo.");
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, this.nombreClase + ".grdDatosAdicionales_RowCommand: " + ex.Message);
            }
        }
        protected void btnAgregarDatoAdicional_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AgregarDatoAdicional();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al Agregar el Dato Adicional.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnAgregarDatoAdicional_Click: " + ex.Message);
            }
        }

        #region Eventos del Buscador
        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtSucursal_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.SucursalID = null;

                string sucursalNombre = ((IucContratoManttoVIS)this).SucursalNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                ((IucContratoManttoVIS)this).SucursalNombre = sucursalNombre;
                if (this.UnidadOperativaID.HasValue && (!string.IsNullOrEmpty(((IucContratoManttoVIS)this).SucursalNombre) && !string.IsNullOrWhiteSpace(((IucContratoManttoVIS)this).SucursalNombre)))
                {
                    this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                    ((IucContratoManttoVIS)this).SucursalNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtSucursal_TextChanged: " + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaSucursal_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscaSucursal_Click: " + ex.Message);
            }
        }

        protected void txtNombreCliente_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.CuentaClienteID = null;

                string nombreCuentaCliente = this.CuentaClienteNombre;
                Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                this.CuentaClienteNombre = nombreCuentaCliente;
                if (!string.IsNullOrEmpty(this.CuentaClienteNombre) && !string.IsNullOrWhiteSpace(this.CuentaClienteNombre))
                    this.EjecutaBuscador("CuentaClienteIdealease", ECatalogoBuscador.CuentaClienteIdealease);

                this.CuentaClienteNombre = null;
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtNombreCliente_TextChanged: " + ex.Message);
            }
        }
        protected void ibtnBuscarCliente_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.CuentaClienteID = null;

                string nombreCuentaCliente = this.CuentaClienteNombre;
                Session_BOSelecto = null;

                this.EjecutaBuscador("CuentaClienteIdealease&&hidden=0", ECatalogoBuscador.CuentaClienteIdealease);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscarCliente_Click: " + ex.Message);
            }
        }

        protected void ibtnBuscarDirieccionCliente_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.hdnClienteID.Value) && !string.IsNullOrWhiteSpace(this.hdnClienteID.Value))
                    this.EjecutaBuscador("DireccionCuentaClienteIdealease", ECatalogoBuscador.DireccionCliente);
                else
                    this.MostrarMensaje("Por favor seleccione un cliente previamente a consultar sus direcciones.", ETipoMensajeIU.ADVERTENCIA, null);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar las direcciones del Cliente", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscarDirieccionCliente_Click: " + ex.Message);
            }
        }

        protected void txtUnidadNumeroEconomico_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.UnidadID = null;
                string numeco = this.NumeroSerie;
                Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Unidad);

                this.NumeroSerie = numeco;
                if (!string.IsNullOrEmpty(this.NumeroSerie))
                    this.EjecutaBuscador("UnidadIdealease", ECatalogoBuscador.Unidad);

                this.NumeroSerie = null;
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al buscar las unidades para la renta", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtUnidadVIN_TextChanged: " + ex.Message);
            }
        }
        protected void ibtnBuscarUnidad_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.UnidadID = null;

                string numeco = this.NumeroSerie;
                Session_BOSelecto = null;

                this.EjecutaBuscador("UnidadIdealease&&hidden=0", ECatalogoBuscador.Unidad);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al buscar las unidades para la renta", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscarUnidad_Click: " + ex.Message);
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
                EjecutaBuscador("ProductoServicio&hidden=0", ECatalogoBuscador.ProductoServicio);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar el producto", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarProductoServicio_Click: " + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Unidad:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        if (this.UnidadID != null)
                        {
                            this.presentador.PrepararNuevaLinea();

                            this.btnActualizarLineaContrato.Visible = false;
                            this.btnCancelarLineaContrato.Visible = false;
                            this.btnAgregarLineaContrato.Visible = true;
                            this.grdLineasContrato.Enabled = false;
                        }
                        else
                            this.grdLineasContrato.Enabled = true;
                        break;
                    case ECatalogoBuscador.Sucursal:
                    case ECatalogoBuscador.CuentaClienteIdealease:
                    case ECatalogoBuscador.DireccionCliente:
                    case ECatalogoBuscador.ProductoServicio:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click: " + ex.Message);
            }
        }
        #endregion


        #region SC_0051
        protected void grdUnidadEquiposAliados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "PAGE" || e.CommandName.ToUpper() == "SORT") return;

                switch (e.CommandName.ToUpper().Trim())
                {
                    case "CMDDETALLES":
                        int eaID = 0;

                        if (Int32.TryParse(e.CommandArgument.ToString(), out eaID))
                        {
                            this.presentador.PrepararVisualizacionTarifaEquipoAliado(eaID);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al ejecutar la acción sobre la unidad del contrato", ETipoMensajeIU.ERROR, this.nombreClase + ".grdLineasContrato_RowCommand: " + ex.Message);
            }
        }
        protected void cmdAceptarTarifaEA_Click(object sender, EventArgs e)
        {
            try
            {
                int eaID = 0;

                if (Int32.TryParse(this.hdnEquipoAliadoID.Value, out eaID))
                {
                    this.presentador.AsignarTarifaEA(eaID);

                    this.ActualizarEquiposAliados();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconcistencia al capturar las tarifas", ETipoMensajeIU.ERROR, nombreClase + ".cmdAceptarTarifaEA_Click :" + ex.Message);
            }
        }

        
        #endregion
        #endregion
    }
}