//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.UI;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.PRE;
using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.MapaSitio.UI;
using Newtonsoft.Json;

namespace BPMO.SDNI.Equipos.UI
{
    public partial class ucDatosGeneralesUI : UserControl, IucDatosGeneralesVIS
    {
        #region Atributos
        private ucDatosGeneralesPRE presentador;
        private string nombreClase = "ucDatosGeneralesUI";

        private ETipoEmpresa ETipoEmpresa;

        public enum ECatalogoBuscador
        {
            Propietario,
            Cliente,
            Sucursal
        }
        #endregion

        #region Propiedades
        

        /// <summary>
        /// Contiene la lista de acciones a las cuales tiene acceso el usuario.
        /// </summary>
        public List<CatalogoBaseBO> ListaAcciones { get; set; }

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

        public int? UsuarioAutenticado
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUsuarioAutenticado.Value))
                    id = int.Parse(this.hdnUsuarioAutenticado.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUsuarioAutenticado.Value = value.ToString();
                else
                    this.hdnUsuarioAutenticado.Value = string.Empty;
            }
        }
        public string Propietario
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtPropietario.Text)) ? null : this.txtPropietario.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtPropietario.Text = value;
                else
                    this.txtPropietario.Text = string.Empty;
            }
        }
        public int? PropietarioId
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnPropietarioID.Value))
                    id = int.Parse(this.hdnPropietarioID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnPropietarioID.Value = value.ToString();
                else
                    this.hdnPropietarioID.Value = string.Empty;
            }
        }
        public string Cliente
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtCliente.Text)) ? null : this.txtCliente.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtCliente.Text = value;
                else
                    this.txtCliente.Text = string.Empty;
            }
        }
        public int? ClienteId
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnClienteID.Value))
                    id = int.Parse(this.hdnClienteID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnClienteID.Value = value.ToString();
                else
                    this.hdnClienteID.Value = string.Empty;
            }
        }
        public int? UnidadOperativaId
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)this.Parent.Page.Master;

                if (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }
        public string SucursalNombre
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
        public int? SucursalId
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
        public EArea? Area
        {
            get 
            { 
                EArea? area = null;
                if (this.ddlArea.SelectedIndex > 0)
                    area = (EArea)Enum.Parse(typeof(EArea), this.ddlArea.SelectedValue);
                return area;
            }
            set 
            {
                if (value == null)
                    this.ddlArea.SelectedIndex = 0;
                else
                    this.ddlArea.SelectedValue = ((int)value).ToString();
            }
        }       
        public string Fabricante
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtFabricante.Text)) ? null : this.txtFabricante.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtFabricante.Text = value;
                else
                    this.txtFabricante.Text = string.Empty;
            }
        }
        public string NombreClienteUnidadOperativa
        {
            get
            {
                return (String.IsNullOrEmpty(this.hdnNombreClienteUnidadOperativa.Value)) ? null : this.hdnNombreClienteUnidadOperativa.Value.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.hdnNombreClienteUnidadOperativa.Value = value;
                else
                    this.hdnNombreClienteUnidadOperativa.Value = string.Empty;
            }
        }

        /// <summary>
        /// Determina si la unidad se encuentra o no bloqueda en el sistema Lider
        /// </summary>
        public bool? UnidadBloqueada
        {
            get { return cbBloqueoUnidad.Checked; }
            set { cbBloqueoUnidad.Checked = value != null && value.Value; }
        }

        #region SC0002
        public bool? EntraMantenimiento
        {
            get
            {
                bool? entra = null;
                if (chbxEntraMantenimiento.Checked == true)
                    entra = true;
                else
                    entra = false;
                return entra;
            }
            set
            {
                if (value != null)
                {
                    if (value == true)
                        chbxEntraMantenimiento.Checked = true;
                    else
                        chbxEntraMantenimiento.Checked = false;
                }

            }
        }
        #endregion


        #region [RQM 13285- Integración Construcción y Generacción]

        /// <summary>
        /// Enumerador Tipo de Renta para Construcción
        /// </summary>
        public EAreaConstruccion? ETipoRentaConstruccion
        {
            get
            {
                EAreaConstruccion? eTipoRenta = null;
                if (this.ddlTipoRentaEmpresas.SelectedIndex > 0)
                    eTipoRenta = (EAreaConstruccion)Enum.Parse(typeof(EAreaConstruccion), this.ddlTipoRentaEmpresas.SelectedValue);
                return eTipoRenta;
            }
            set
            {
                if (value == null)
                    this.ddlTipoRentaEmpresas.SelectedIndex = 0;
                else
                    this.ddlTipoRentaEmpresas.SelectedValue = ((int)value).ToString();
            }
        }
       
        /// <summary>
        /// Enumerador Tipo de Renta para Generación
        /// </summary>
        public EAreaGeneracion? ETipoRentaGeneracion
        {
            get
            {
                EAreaGeneracion? eTipoRenta = null;
                if (this.ddlTipoRentaEmpresas.SelectedIndex > 0)
                    eTipoRenta = (EAreaGeneracion)Enum.Parse(typeof(EAreaGeneracion), this.ddlTipoRentaEmpresas.SelectedValue);
                return eTipoRenta;
            }
            set
            {
                if (value == null)
                    this.ddlTipoRentaEmpresas.SelectedIndex = 0;
                else
                    this.ddlTipoRentaEmpresas.SelectedValue = ((int)value).ToString();
            }
        }

        /// <summary>
        /// Enumerador Tipo de Renta para Generación
        /// </summary>
        public EAreaEquinova? ETipoRentaEquinova {
            get {
                EAreaEquinova? eTipoRenta = null;
                if (this.ddlTipoRentaEmpresas.SelectedIndex > 0)
                    eTipoRenta = (EAreaEquinova)Enum.Parse(typeof(EAreaEquinova), this.ddlTipoRentaEmpresas.SelectedValue);
                return eTipoRenta;
            }
            set {
                if (value == null)
                    this.ddlTipoRentaEmpresas.SelectedIndex = 0;
                else
                    this.ddlTipoRentaEmpresas.SelectedValue = ((int)value).ToString();
            }
        }

        public ETipoEmpresa EnumTipoEmpresa
        {
            get
            {
                return this.ETipoEmpresa;
            }
            set
            {
                this.ETipoEmpresa = value;
            }
        }

        public int? ProveedorID
        {
            get
            {
                return this.PropietarioId;
            }
            set
            {
                this.PropietarioId = value;
            }
        }

        public DateTime? FC
        {
            get { return DateTime.Now; }
            set { this.FC = value; }
        }

        public int? UC
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
            set { this.UC = value; }
        }

        /// <summary>
        /// Número de orden de compra
        /// </summary>
        public string OrdenCompraProveedor
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtNumeroOrdenCompra.Text)) ? null : this.txtNumeroOrdenCompra.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtNumeroOrdenCompra.Text = value;
                else
                    this.txtNumeroOrdenCompra.Text = string.Empty;
            }
        }

        /// <summary>
        /// Monto del arrendamiento
        /// </summary>
        public decimal? MontoArrendamiento
        {
            get
            {
                decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtMontoArrendamiento.Text))
                    temp = decimal.Parse(this.txtMontoArrendamiento.Text.Trim().Replace(",", ""));
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtMontoArrendamiento.Text = string.Format("{0:#,##0.00}", value);
                else
                    this.txtMontoArrendamiento.Text = string.Empty;
            }
        }

        /// <summary>
        /// Código moneda
        /// </summary>
        public string CodigoMoneda
        {
            get
            {
                string codigo = null;
                if (ddlMonedas.SelectedValue != "-1")
                    codigo = ddlMonedas.SelectedValue;
                return codigo;
            }
            set
            {
                this.ddlMonedas.SelectedValue = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                   ? value.Trim().ToUpper()
                                                   : "-1";
            }
        }

        /// <summary>
        /// Fecha inicio del arrendamiento
        /// </summary>
        public DateTime? FechaInicioArrendamiento
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtFechaInicioArrendamiento.Text) && !string.IsNullOrWhiteSpace(this.txtFechaInicioArrendamiento.Text))
                {
                    DateTime date;
                    return DateTime.TryParse(this.txtFechaInicioArrendamiento.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtFechaInicioArrendamiento.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }

        /// <summary>
        /// Fecha final del arrendamiento
        /// </summary>
        public DateTime? FechaFinArrendamiento
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtFechaFinArrendamiento.Text) && !string.IsNullOrWhiteSpace(this.txtFechaFinArrendamiento.Text))
                {
                    DateTime date;
                    return DateTime.TryParse(this.txtFechaFinArrendamiento.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtFechaFinArrendamiento.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }

        /// <summary>
        /// Fecha inicio del arrendamiento nuevo
        /// </summary>
        public DateTime? FechaInicioArrendamientoNuevo
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtNuevaFechaInicio.Text) && !string.IsNullOrWhiteSpace(this.txtNuevaFechaInicio.Text))
                {
                    DateTime date;
                    return DateTime.TryParse(this.txtNuevaFechaInicio.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtNuevaFechaInicio.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }

        /// <summary>
        /// Fecha final del arrendamiento nuevo
        /// </summary>
        public DateTime? FechaFinArrendamientoNuevo
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtNuevaFechaFinal.Text) && !string.IsNullOrWhiteSpace(this.txtNuevaFechaFinal.Text))
                {
                    DateTime date;
                    return DateTime.TryParse(this.txtNuevaFechaFinal.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtNuevaFechaFinal.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }

        /// <summary>
        /// Indica si será un arrendamiento nuevo
        /// </summary>
        public bool ArrendamientoNuevo
        {
            get
            {
                return (String.IsNullOrEmpty(this.hdnArrendamientoNuevo.Value)) ? false : Convert.ToBoolean(this.hdnArrendamientoNuevo.Value);
            }
            set
            {
                if (value != null)
                    this.hdnArrendamientoNuevo.Value = value.ToString();
                else
                    this.hdnArrendamientoNuevo.Value = string.Empty;
            }
        }

        public List<ArchivoBO> ArchivosOC
        {
            get
            {
                if (ucCatalogoDocumentosOC.ObtenerArchivos() != null)
                    return ucCatalogoDocumentosOC.ObtenerArchivos();
                else
                    return new List<ArchivoBO>();
            }
            set { ucCatalogoDocumentosOC.EstablecerListasArchivos(value, null); }
        }

        public List<TipoArchivoBO> TiposArchivo
        {
            get { return ucCatalogoDocumentosOC.TiposArchivo; }
            set { ucCatalogoDocumentosOC.TiposArchivo = value; }
        }
        public IucCatalogoDocumentosVIS VistaDocumentos
        {
            get { return this.ucCatalogoDocumentosOC; }
        }

        public void EstablecerIdentificadorListaArchivos(string identificador)
        {
            this.ucCatalogoDocumentosOC.Identificador = identificador;
        }

        public AdscripcionBO Adscripcion
        {
            get
            {
                return this.Session["Adscripcion"] != null ? (AdscripcionBO)this.Session["Adscripcion"] : null;
            }
            set
            {
                this.Session["Adscripcion"] = value;
            }
        }

        public void EstablecerTipoAdjunto(ETipoAdjunto tipo)
        {
            ucCatalogoDocumentosOC.TipoAdjunto = tipo;
        }
        #endregion


        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            ucCatalogoDocumentosOC.presentador = new ucCatalogoDocumentosPRE(ucCatalogoDocumentosOC);
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            
            this.presentador = new ucDatosGeneralesPRE(this, ucCatalogoDocumentosOC.presentador);

        }
        #endregion

        #region Métodos

        #region [RQM 13285- Modificación -Integración Construcción y Generacción]

        public void PrepararNuevo()
        {
            this.txtCliente.Text = "";
            this.txtPropietario.Text = "";
            this.txtSucursal.Text = "";
            this.txtFabricante.Text = "";
            cbBloqueoUnidad.Checked = false;
            chbxEntraMantenimiento.Checked = false;
            this.ddlArea.SelectedIndex = 0;
           
            //Se inicializan los controles para Construcción y Generación
            this.txtNumeroOrdenCompra.Text = "";
            this.txtMontoArrendamiento.Text = "";
            this.ddlTipoRentaEmpresas.SelectedIndex = 0;
            this.ddlMonedas.SelectedValue = "MXN";
            this.txtFechaInicioArrendamiento.Text = "";
            this.txtFechaFinArrendamiento.Text = "";
            this.txtNuevaFechaInicio.Text = "";
            this.txtNuevaFechaFinal.Text = "";           
        }
        #endregion

        public void CargarAreas(Dictionary<string, object> areas)
        {
            this.ddlArea.Items.Clear();
            this.ddlArea.Items.Add(new ListItem() { Value = "x", Text = "Seleccionar" });

            foreach (KeyValuePair<string, object> area in areas)
                this.ddlArea.Items.Add(new ListItem() { Text = area.Key, Value = area.Value.ToString() });
        }

        #region [RQM 13285- Integración Construcción y Generacción]
        /// <summary>
        /// Cargar lista Tipo de Renta para Construcción
        /// </summary>
        /// <param name="tipoRentaC"></param>
        public void CargarTipoRentaConstruccion(Dictionary<string, object> tipoRentaC)
        {
            this.ddlTipoRentaEmpresas.Items.Clear();
            this.ddlTipoRentaEmpresas.Items.Add(new ListItem() { Value = "x", Text = "Seleccionar" });

            foreach (KeyValuePair<string, object> tipoRenta in tipoRentaC)
                this.ddlTipoRentaEmpresas.Items.Add(new ListItem() { Text = tipoRenta.Key, Value = tipoRenta.Value.ToString() });
        }

        /// <summary>
        /// Cargar lista Tipo de Renta para Generación
        /// </summary>
        /// <param name="tipoRentaG"></param>
        public void CargarTipoRentaGeneracion(Dictionary<string, object> tipoRentaG)
        {
            this.ddlTipoRentaEmpresas.Items.Clear();
            this.ddlTipoRentaEmpresas.Items.Add(new ListItem() { Value = "x", Text = "Seleccionar" });

            foreach (KeyValuePair<string, object> tipoRenta in tipoRentaG)
                this.ddlTipoRentaEmpresas.Items.Add(new ListItem() { Text = tipoRenta.Key, Value = tipoRenta.Value.ToString() });
        }

        /// <summary>
        /// Cargar lista Tipo de Renta para Equinova
        /// </summary>
        /// <param name="tipoRentaEqi"></param>
        public void CargarTipoRentaEquinova(Dictionary<string, object> tipoRentaEqi) {
            this.ddlTipoRentaEmpresas.Items.Clear();
            this.ddlTipoRentaEmpresas.Items.Add(new ListItem() { Value = "x", Text = "Seleccionar" });

            foreach (KeyValuePair<string, object> tipoRenta in tipoRentaEqi)
                this.ddlTipoRentaEmpresas.Items.Add(new ListItem() { Text = tipoRenta.Key, Value = tipoRenta.Value.ToString() });
        }
        #endregion

        public void EstablecerEntraMantenimeinto()
        {
            Session["ActivarCheckAliado"] = this.EntraMantenimiento;
        }

        #region [RQM 13285- Modificación - Integración Construcción y Generacción]
        public void HabilitarModoEdicion(bool habilitar)
        {
            this.EnumTipoEmpresa = (ETipoEmpresa)this.UnidadOperativaId;
            if (this.EnumTipoEmpresa == ETipoEmpresa.Idealease)
            {
                this.txtCliente.Enabled = habilitar;
                this.txtPropietario.Enabled = habilitar;
                this.txtSucursal.Enabled = habilitar;
                this.ibtnBuscaCliente.Enabled = habilitar;
                this.ibtnBuscaPropietario.Enabled = habilitar;
                this.ibtnBuscaSucursal.Enabled = habilitar;
                this.ddlArea.Enabled = habilitar;
                this.cbBloqueoUnidad.Enabled = habilitar;
                this.chbxEntraMantenimiento.Enabled = habilitar;
            }

            if (this.EnumTipoEmpresa == ETipoEmpresa.Construccion || this.EnumTipoEmpresa == ETipoEmpresa.Generacion
                || this.EnumTipoEmpresa == ETipoEmpresa.Equinova)
            {
                this.txtCliente.Enabled = habilitar;
                this.txtPropietario.Enabled = habilitar;
                this.txtSucursal.Enabled = habilitar;
                this.ibtnBuscaCliente.Enabled = habilitar;
                this.ibtnBuscaPropietario.Enabled = habilitar;
                this.ibtnBuscaSucursal.Enabled = habilitar;
                this.ddlArea.Enabled = habilitar;
                this.ddlTipoRentaEmpresas.Enabled = habilitar;
                this.cbBloqueoUnidad.Enabled = habilitar;
                this.chbxEntraMantenimiento.Enabled = habilitar;
                this.ucCatalogoDocumentosOC.HabilitarControles(habilitar);
                this.txtMontoArrendamiento.Enabled = habilitar;
                this.txtNumeroOrdenCompra.Enabled = habilitar;
                this.txtFechaInicioArrendamiento.Enabled = habilitar;
                this.txtFechaFinArrendamiento.Enabled = habilitar;                
            }
        }
        #endregion

        public void HabilitarPropietario(bool habilitar)
        {
            this.txtPropietario.Enabled = habilitar;
            this.ibtnBuscaPropietario.Enabled = habilitar;
        }
        public void HabilitarCliente(bool habilitar)
        {
            this.txtCliente.Enabled = habilitar;
            this.ibtnBuscaCliente.Enabled = habilitar;
        }
        public void HabilitarArea(bool habilitar)
        {
            this.ddlArea.Enabled = habilitar;
        }
        public void HabilitarSucursal(bool habilitar)
        {
            this.txtSucursal.Enabled = habilitar;
            this.ibtnBuscaSucursal.Enabled = habilitar;
        }

        public void MostrarFabricante(bool mostrar)
        {
            this.trFabricante.Visible = mostrar;
        }

        /// <summary>
        /// Se usa para activar o desactivar el campo de Bloqueo de la Unidad
        /// </summary>
        /// <param name="habilitar">Determina si se habilita o no el campo</param>
        public void HabilitarUnidadBloqueada(bool habilitar)
        {
            cbBloqueoUnidad.Enabled = habilitar;
        }

        #region SC0002
        /// <summary>
        /// Se usa para activar o desactivar el campo de Entra Mantenimiento
        /// </summary>
        /// <param name="habilitar">Determina si se habilita o no el campo</param>
        public void HabilitarEntraMantenimiento(bool habilitar)
        {
            chbxEntraMantenimiento.Enabled = habilitar;
        }
        #endregion


        #region [RQM 13285- Integración Construcción y Generacción]

        public void HabilitaMontoArrendamiento(bool habilitar)
        {
            txtMontoArrendamiento.Enabled = habilitar;
            
        }
        public void HabilitaFechasArrendamiento(bool mostrar)
        {
            trFechaInicioArrendamiento.Visible = mostrar;
            trFechaFinArrendamiento.Visible = mostrar;
            if (mostrar && hdnModo.Value == "E")
            {
                trdNuevasFechasInicio.Visible = mostrar;
                trdNuevasFechasFin.Visible = mostrar;
            }
        }
        public void ActivaCheckMantenimiento(bool activar)
        {
            chbxEntraMantenimiento.Checked = activar;
        }
        public void ModificaEtiquetaPropietario(bool modifica)
        {
            if (modifica)
            {
                this.EnumTipoEmpresa = (ETipoEmpresa)this.UnidadOperativaId;
                string EtiquetaPropietario = ObtenerConfiguracionResource("RE44", this.EnumTipoEmpresa, true);
                lblPropietario.Text = EtiquetaPropietario;
            }
            else
            {
                lblPropietario.Text = "PROPIETARIO";
            }
        }
        public void HabilitaTipoMonedas(bool habilitar)
        {
            this.ddlMonedas.Enabled = habilitar;
            this.ddlMonedas.SelectedValue = "MXN";
        }       
        public void HabilitaArea(bool habilitar)
        {
            this.ddlArea.Visible = habilitar;
        }
        public void HabilitaTipoRentaEmpresas(bool habilitar)
        {
            this.ddlTipoRentaEmpresas.Enabled = habilitar;
        }
        public void MostrarOrdenCompra(bool mostrar)
        {
            this.trNumOrdenCompra.Visible = mostrar;
        }
        public void MostrarTipoMoneda(bool mostrar)
        {
            this.trTipoMoneda.Visible = mostrar; 
        }
        public void MostrarTipoRentaEmpresas(bool mostrar)
        {
            this.ddlTipoRentaEmpresas.Visible = mostrar;
        }

        public void MostrarMontoArrendamiento(bool mostrar)
        {
            this.trMontoArrendamiento.Visible = mostrar;
        }

        public void MostrarAreas(bool mostrar)
        {
            this.ddlArea.Visible = mostrar;
        }
        public void ReiniciaMontoArrendamiento()
        {
            this.txtMontoArrendamiento.Text = "";
        }
        public void CargarTiposArchivos(List<TipoArchivoBO> tipos)
        {
            ucCatalogoDocumentosOC.TiposArchivo = tipos;
        }

        public void HabilitarCargaArchivoOC(bool habilitar)
        {
            divArchivos.Visible = habilitar;
            this.ucCatalogoDocumentosOC.Visible = habilitar;
            this.ucCatalogoDocumentosOC.MostrarObservaciones(false);
        }

        public void HabilitaControlesOC(bool habilitar)
        {
            this.ucCatalogoDocumentosOC.HabilitarControles(habilitar);
        }

        public void MostrarBotonAgregarFechas(bool habilitar)
        {   
            this.btnAgregarFechas.Visible = habilitar;
            this.btnAgregarFechas.Enabled = habilitar;
        }

        public void ReiniciarGridOC()
        {
            this.ucCatalogoDocumentosOC.ReiniciarGrid();
        }
        #endregion

        public void LimpiarSesion()
        {
            //No se si se debe limpiar estas variables de sesión
            //if (Session[String.Format("BOSELECTO_{0}", ViewState_Guid)] != null)
            //    Session.Remove(String.Format("BOSELECTO_{0}", ViewState_Guid));
            //if (Session[ViewState_Guid] != null)
            //    Session.Remove(ViewState_Guid);
        }

        /// <summary>
        /// Desplegar mensaje de Error con detalle
        /// </summary>
        /// <param name="mensaje">Descripción del mensaje</param>
        /// <param name="tipo">EMensaje tipo de mensaje a desplegar</param>
        /// <param name="detalle">Detalle del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            if (tipo.Equals(ETipoMensajeIU.CONFIRMACION))
            {
                ((HiddenField)this.Parent.FindControl("hdnTipoMensaje")).Value = ((int)tipo).ToString();
                ((HiddenField)this.Parent.FindControl("hdnMensaje")).Value = mensaje;
            }
            else
            {
                Site masterMsj = (Site)this.Parent.Page.Master;
                if (tipo == ETipoMensajeIU.ERROR)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
                else
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }
        /// <summary>
        /// Indica el modo del uc
        /// </summary>
        /// <param name="habilitar"></param>
        public void ModoEdicion(bool habilitar)
        {
            this.ucCatalogoDocumentosOC.ModoEdicion = habilitar;
        }
        #region

        #endregion

        #region Métodos para el Buscador
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="tipoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "', '" + this.btnResult.ClientID + "');");
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


        #region [RQM 13285- Integración Construcción y Generacción]
        /// <summary>
        /// Método de validación inicial para el tipo de empresa autenticada 
        /// </summary>
        public void EstablecerAcciones(string modo)
        {
            this.EnumTipoEmpresa = (ETipoEmpresa)this.UnidadOperativaId;
            switch (this.EnumTipoEmpresa)
            {
                case ETipoEmpresa.Construccion:
                    HabilitarControles(true, modo, this.EnumTipoEmpresa);
                    break;
                case ETipoEmpresa.Generacion:
                    HabilitarControles(true, modo, this.EnumTipoEmpresa);
                    break;
                case ETipoEmpresa.Equinova:
                    HabilitarControles(true, modo, this.EnumTipoEmpresa);
                    break;
                case ETipoEmpresa.Idealease:
                    HabilitarControles(false, modo, this.EnumTipoEmpresa);
                    break;
            }
            this.hdnModo.Value = modo;
        }

        /// <summary>
        /// Establece el comportamiento inicial de controles nuevos para Construcción y Generación
        /// </summary>
        /// <param name="mostrar">Condición para el comportamiento de los controles</param>
        /// <param name="empresa">Empresa autenticada</param>
        public void HabilitarControles(bool mostrar, string modo, ETipoEmpresa empresa = 0)
        {            
            if (!mostrar)
            {
                this.ddlArea.Visible = true;
            }
            else
            {
                string TipoRenta = ObtenerConfiguracionResource("RE34", empresa, true);
                this.lblAreaDepto.Text = TipoRenta;
                this.trFabricante.Visible = false;
                this.ddlArea.Visible = false;               
            }
           
            if (modo == "R")
            {
                this.ddlMonedas.SelectedValue = "MXN";
                DateTime fechaActual = DateTime.Now;
                string Fecha = fechaActual.ToShortDateString();
                txtFechaInicioArrendamiento.Text = Fecha;
                txtFechaFinArrendamiento.Text = Fecha;
                this.HabilitaTipoMonedas(false);
                this.trdNuevasFechasInicio.Visible = false;
                this.trdNuevasFechasFin.Visible = false;
                ucCatalogoDocumentosOC.ReiniciarGrid();
                this.btnAgregarFechas.Enabled = false;
                this.txtFechaInicioArrendamiento.CssClass = "CampoFecha";
                this.txtFechaFinArrendamiento.CssClass = "CampoFecha";
            }

            if(modo == "D")
            {
                this.txtFechaInicioArrendamiento.Enabled = false;
                this.txtFechaFinArrendamiento.Enabled = false;
                this.trdNuevasFechasInicio.Visible = false;
                this.trdNuevasFechasFin.Visible = false;
                this.ddlMonedas.Enabled = false;
                this.btnAgregarFechas.Enabled = false;
                this.btnAgregarFechas.Visible = false;
                this.txtFechaInicioArrendamiento.CssClass = "CampoFechas";
                this.txtFechaFinArrendamiento.CssClass = "CampoFechas";
            }
            if(modo == "E")
            {
                if (((empresa == ETipoEmpresa.Generacion) && this.ddlTipoRentaEmpresas.SelectedValue == ((int)EAreaGeneracion.RE).ToString()) ||
                    ((empresa == ETipoEmpresa.Construccion) && this.ddlTipoRentaEmpresas.SelectedValue == ((int)EAreaConstruccion.RE).ToString()) ||
                    ((empresa == ETipoEmpresa.Equinova) && this.ddlTipoRentaEmpresas.SelectedValue == ((int)EAreaEquinova.RE).ToString()))
                {
                    //Se habilitan los campos para editar si el Tipo de Renta es RE
                    this.txtNumeroOrdenCompra.Enabled = true;
                    this.txtMontoArrendamiento.Enabled = true;
                    this.ddlMonedas.Enabled = true;
                    this.txtFechaInicioArrendamiento.Enabled = true;
                    this.txtFechaFinArrendamiento.Enabled = true;
                    this.ModificaEtiquetaPropietario(true);
                    this.txtNumeroOrdenCompra.Enabled = true;
                    this.trdNuevasFechasInicio.Visible = false;
                    this.trdNuevasFechasFin.Visible = false;
                    this.txtFechaInicioArrendamiento.CssClass = "CampoFechas";
                    this.txtFechaFinArrendamiento.CssClass = "CampoFechas";
                    this.ddlTipoRentaEmpresas.Enabled = false;
                    this.txtPropietario.Enabled = false;
                    this.ibtnBuscaPropietario.Enabled = false;
                }
            }
            //Control de filas y campos                        
            this.trTipoMoneda.Visible = mostrar;
            this.ddlTipoRentaEmpresas.Visible = mostrar;
            this.trMontoArrendamiento.Visible = mostrar;
            if ((empresa == ETipoEmpresa.Generacion && this.ddlTipoRentaEmpresas.SelectedValue != ((int)EAreaGeneracion.RE).ToString()) ||
                (empresa == ETipoEmpresa.Construccion && this.ddlTipoRentaEmpresas.SelectedValue != ((int)EAreaConstruccion.RE).ToString()) ||
                (empresa == ETipoEmpresa.Equinova && this.ddlTipoRentaEmpresas.SelectedValue != ((int)EAreaEquinova.RE).ToString()) ||
                (empresa == ETipoEmpresa.Idealease))
            {
                OcultarControlesRE(false);
            }
        }

        /// <summary>
        /// Método para ocultar los controles de Renta Extraordinaria
        /// </summary>
        /// <param name="monedas">Lista de monedas</param>
        public void OcultarControlesRE(bool habilitar){

            this.trNumOrdenCompra.Visible = habilitar;
            this.trMontoArrendamiento.Visible = habilitar;
            this.trTipoMoneda.Visible = habilitar;
            this.trFechaInicioArrendamiento.Visible = habilitar;
            this.trFechaFinArrendamiento.Visible = habilitar;
            this.trdNuevasFechasInicio.Visible = habilitar;
            this.trdNuevasFechasFin.Visible = habilitar;
            this.btnAgregarFechas.Visible = habilitar;
            this.divArchivos.Visible = habilitar;
        }

        /// <summary>
        /// Método para cargar los Tipos de Moneda al combo ddlMonedas
        /// </summary>
        /// <param name="monedas">Lista de monedas</param>
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
        /// Método que obtiene la configuración de una etiqueta desde el archivo resource correspondiente a la unidad operativa a la cual accedió el usuario.
        /// </summary>
        /// <param name="etiquetaBuscar">Nombre de la etiqueta que será buscada en el archivo resource</param>
        /// <param name="tipoEmpresa">Nombre de la unidad operativa a la cual accedió el usuario.</param>
        /// <param name="esEtiqueta">Indica sí el valor a obtener es una etiqueta, en caso contrario se considera un TAB o CHECKBOX.</param>
        /// <returns>Retorna la configuración correspondiente al valor recibido en el parámetro etiquetaBuscar del archivo resource.</returns>
        private string ObtenerConfiguracionResource(string etiquetaBuscar, ETipoEmpresa tipoEmpresa, bool esEtiqueta)
        {
            string Configuracion = string.Empty;
            //Instanciamos la clase o webmethod que obtiene las etiquetas
            ObtenerEtiquetaEmpresas obtenerEtiqueta = new ObtenerEtiquetaEmpresas();
            string ConfiguracionObtenida = string.Empty;
            EtiquetaObtenida request = null;

            ConfiguracionObtenida = obtenerEtiqueta.ObtenerEtiquetadelResource(etiquetaBuscar, (int)tipoEmpresa);
            request = JsonConvert.DeserializeObject<EtiquetaObtenida>(ConfiguracionObtenida);
            if (string.IsNullOrEmpty(request.cMensaje))
            {
                ConfiguracionObtenida = request.cEtiqueta;
                if (esEtiqueta)
                {
                    if (ConfiguracionObtenida != "NO APLICA")
                    {
                        Configuracion = ConfiguracionObtenida;
                    }
                }
                else
                {
                    Configuracion = ConfiguracionObtenida;
                }
            }
            return Configuracion;
        }


        #endregion

        #endregion
        #endregion

        #region Eventos
        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int? valor = null;

                if (this.ddlArea.SelectedIndex > 0)
                    valor = int.Parse(this.ddlArea.SelectedValue);

                this.presentador.SeleccionarArea(valor);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al seleccionar el área o departamento", ETipoMensajeIU.ERROR, this.nombreClase + ".ddlArea_SelectedIndexChanged:" + ex.Message);
            }
        }

        #region [RQM 13285- Integración Construcción y Generacción]
        /// <summary>
        /// Evento para el combo ddlTipoRentaEmpresas
        /// </summary>
        protected void ddlTipoRentaEmpresas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int? Evalor = null;
                if (this.ddlTipoRentaEmpresas.SelectedIndex > 0)
                    Evalor = int.Parse(this.ddlTipoRentaEmpresas.SelectedValue);


                this.presentador.SeleccionarTipoRentaEmpresas(Evalor);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al seleccionar el tipo de renta ", ETipoMensajeIU.ERROR, this.nombreClase + ".ddlEmpresas_SelectedIndexChanged:" + ex.Message);
            }
        }

        #endregion

        #region Eventos para el Buscador
        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Propietario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtPropietario_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string propietario = this.Propietario;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Propietario);

                this.Propietario = propietario;
                if (this.Propietario != null)
                {
                    this.EjecutaBuscador("Cliente", ECatalogoBuscador.Propietario);
                    this.Propietario = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Propietario", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtPropietario_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Propietario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaPropietario_Click(object sender, ImageClickEventArgs e)
        {
            string s;
            if ((s = this.presentador.ValidarCamposConsultaPropietario()) != null)
            {
                this.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try
            {
                this.EjecutaBuscador("Cliente&hidden=0", ECatalogoBuscador.Propietario);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscaPropietario_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCliente_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string cliente = this.Cliente;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Cliente);

                this.Cliente = cliente;
                if (this.Cliente != null)
                {
                    this.EjecutaBuscador("Cliente", ECatalogoBuscador.Cliente);
                    this.Cliente = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtCliente_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaCliente_Click(object sender, ImageClickEventArgs e)
        {
            string s;
            if ((s = this.presentador.ValidarCamposConsultaCliente()) != null)
            {
                this.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try
            {
                this.EjecutaBuscador("Cliente&hidden=0", ECatalogoBuscador.Cliente);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscaCliente_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtSucursal_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string sucursalNombre = this.SucursalNombre;
                int? unidadOperativaId = this.UnidadOperativaId;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                this.SucursalNombre = sucursalNombre;
                if (this.UnidadOperativaId != null && this.SucursalNombre != null)
                {
                    this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                    this.SucursalNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtSucursal_TextChanged:" + ex.Message);
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
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscaSucursal_Click:" + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Propietario:
                    case ECatalogoBuscador.Cliente:
                    case ECatalogoBuscador.Sucursal:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click:" + ex.Message);
            }
        }
        #endregion

        protected void chbxEntraMantenimiento_CheckedChanged(object sender, EventArgs e)
        {
            Session["ActivarCheckAliado"] = this.EntraMantenimiento;
        }
        

        protected void btnAgregarFechas_Click(object sender, EventArgs e)
        {
            this.ArrendamientoNuevo = true;
            this.txtFechaInicioArrendamiento.Enabled = false;
            this.txtFechaFinArrendamiento.Enabled = false;
            this.trdNuevasFechasInicio.Visible = true;
            this.trdNuevasFechasFin.Visible = true;
        }
        #endregion
    }
}
