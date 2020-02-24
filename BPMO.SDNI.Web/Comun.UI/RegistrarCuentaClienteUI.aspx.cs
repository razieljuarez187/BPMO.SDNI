//Satisface al caso de uso CU068 - Catálogo de Clientes
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.MapaSitio.UI;
using Newtonsoft.Json;

namespace BPMO.SDNI.Comun.UI {
    public partial class RegistrarCuentaClienteUI : System.Web.UI.Page, IRegistrarCuentaClienteVIS {
        #region Atributos

        private const string nombreClase = "RegistrarClienteUI";
        private RegistrarCuentaClientePRE presentador;
        public List<CatalogoBaseBO> lstAcciones;
        public List<TelefonoClienteBO> lstTelefonos;

        public DataRow datarow;
        public enum ECatalogoBuscador {
            Cliente
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Lista de acciones para establecer permisos para la empresa de tipo Construcción, Generación o Idealease.
        /// </summary>
        public List<CatalogoBaseBO> ListaAcciones {
            get { return this.lstAcciones; }
            set { this.lstAcciones = value; }
        }

        /// <summary>
        /// Atributo para almacenar las observaciones del cliente.
        /// </summary>
        public string Observaciones {
            get { return (String.IsNullOrEmpty(this.txtObservaciones.Text.Trim())) ? null : this.txtObservaciones.Text.Trim().ToUpper(); }
            set { this.txtObservaciones.Text = value != null ? value : string.Empty; }
        }

        public CuentaClienteIdealeaseBO Cliente {
            get {
                if (Session["Cliente"] == null)
                    return null;
                else
                    return (CuentaClienteIdealeaseBO)Session["Cliente"];
            }
            set {
                Session["Cliente"] = value;
            }
        }

        public string CURP {
            get { return (String.IsNullOrEmpty(this.txtCURP.Text.Trim())) ? null : this.txtCURP.Text.ToUpper(); }
            set { this.txtCURP.Text = value != null ? value : string.Empty; }
        }

        /// <summary>
        /// Numero de cuenta de Oracle - CuentaClienteBO.Numero
        /// </summary>
        public string NumeroCuentaOracle {
            get { return String.IsNullOrEmpty(this.txtNumeroCuentaOracle.Text.Trim()) ? null : this.txtNumeroCuentaOracle.Text.Trim(); }
            set { this.txtNumeroCuentaOracle.Text = value ?? String.Empty; }
        }

        public DateTime? FC {
            get { return DateTime.Today; }
        }

        public DateTime? FechaRegistro {
            get {
                DateTime? fechaRegistro = null;
                if (!String.IsNullOrEmpty(this.txtFechaRegistro.Text.Trim()))
                    fechaRegistro = DateTime.Parse(this.txtFechaRegistro.Text.Trim());

                return fechaRegistro;
            }
            set { this.txtFechaRegistro.Text = value != null ? value.Value.ToString("dd/MM/yyyy") : string.Empty; }
        }

        /// <summary>
        /// Atributo para almacenar los elementos del Grid de Teléfonos del cliente.
        /// </summary>
        public DataTable DtTelefonos {
            get {
                if (Session["Telefonos"] == null) {
                    DataTable DTTemp = new DataTable();
                    DTTemp.TableName = "Telefonos";
                    DTTemp.Columns.Add("Telefono", typeof(System.String));
                    return DTTemp;
                } else
                    return (DataTable)Session["Telefonos"];
            }
            set {
                Session["Telefonos"] = value;
            }
        }

        public bool? Fisica {
            get {
                if (!String.IsNullOrEmpty(this.txtTipoContribuyente.Text.Trim())) {
                    if (this.txtTipoContribuyente.Text.ToUpper() == "FÍSICO") {
                        return true;
                    } else if (this.txtTipoContribuyente.Text.ToUpper() == "MORAL") {
                        return false;
                    }
                }
                return null;
            }
            set {
                if (value != null) {
                    if (value == true)
                        this.txtTipoContribuyente.Text = "FÍSICO";
                    if (value == false)
                        this.txtTipoContribuyente.Text = "MORAL";
                } else
                    this.txtTipoContribuyente.Text = string.Empty;
            }
        }

        public DateTime? FUA {
            get { return this.FC; }
        }

        public string GiroEmpresa {
            get { return (String.IsNullOrEmpty(this.txtGiroEmpresa.Text.Trim())) ? null : this.txtGiroEmpresa.Text.ToUpper(); }
            set { this.txtGiroEmpresa.Text = value != null ? value : string.Empty; }
        }

        public string NombreCliente {
            get { return (String.IsNullOrEmpty(this.txtCliente.Text.Trim())) ? null : this.txtCliente.Text.ToUpper(); }
            set {
                if (value != null)
                    this.txtCliente.Text = value;
                else
                    this.txtCliente.Text = string.Empty;
            }
        }

        public List<ObligadoSolidarioBO> ObligadosSolidarios {
            get {
                if (Session["ListaObligadosSolidarios"] == null)
                    return new List<ObligadoSolidarioBO>();
                else
                    return (List<ObligadoSolidarioBO>)Session["ListaObligadosSolidarios"];
            }
            set {
                Session["ListaObligadosSolidarios"] = value;
            }
        }

        public List<RepresentanteLegalBO> RepresentantesLegales {
            get {
                if (Session["ListaRepresentantesLegales"] == null)
                    return new List<RepresentanteLegalBO>();
                else
                    return (List<RepresentanteLegalBO>)Session["ListaRepresentantesLegales"];
            }
            set {
                Session["ListaRepresentantesLegales"] = value;
            }
        }

        public List<RepresentanteLegalBO> RepresentantesObligados {
            get {
                if (Session["RepresentantesObligadosSol"] == null) return new List<RepresentanteLegalBO>();
                return Session["RepresentantesObligadosSol"] as List<RepresentanteLegalBO>;
            }
            set { Session["RepresentantesObligadosSol"] = value; }
        }

        /// <summary>
        /// Atributo para RFC de obligados solidarios.
        /// </summary>
        public string RFC {
            get { return (String.IsNullOrEmpty(this.txtRFC.Text.Trim())) ? null : this.txtRFC.Text.ToUpper(); }
            set {
                if (value != null)
                    this.txtRFC.Text = value;
                else
                    this.txtRFC.Text = string.Empty;
            }
        }

        public ETipoCuenta? TipoCuenta {
            get {
                if (this.ddlTipoCuenta.SelectedValue.Trim() == "0")
                    return ETipoCuenta.Local;
                if (this.ddlTipoCuenta.SelectedValue.Trim() == "1")
                    return ETipoCuenta.Nacional;

                return null;
            }
            set {
                if (value != null)
                    this.ddlTipoCuenta.SelectedValue = ((int)value).ToString();
                else
                    this.ddlTipoCuenta.SelectedValue = "";
            }
        }


        #region SC0001


        public int? DiasUsoUnidad {
            get {
                int? dias = null;
                if (txtDiasUso.Text != string.Empty)
                    dias = int.Parse(this.txtDiasUso.Text);
                return dias;
            }
            set {
                if (value != null)
                    this.txtDiasUso.Text = value.ToString();
                else
                    this.txtDiasUso.Text = string.Empty;

            }
        }

        public int? HorasUsoUnidad {
            get {
                int? horas = null;
                if (txtHorasUso.Text != string.Empty)
                    horas = int.Parse(this.txtHorasUso.Text);
                return horas;
            }
            set {
                if (value != null)
                    this.txtHorasUso.Text = value.ToString();
                else
                    this.txtHorasUso.Text = string.Empty;

            }
        }

        public string Correo {
            get { return this.txtCorreo.Text; }
            set { this.txtCorreo.Text = value; }
        }


        #endregion

        public int? UC {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj.Usuario.Id != null)
                    id = masterMsj.Usuario.Id;

                return id;
            }
        }

        public UnidadOperativaBO UnidadOperativa {
            get {
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    return new UnidadOperativaBO { Id = masterMsj.Adscripcion.UnidadOperativa.Id };
                return null;
            }
        }

        /// <summary>
        /// Sector al que pertenece al cliente
        /// </summary>
        public Enum SectorCliente {
            get {
                Enum sector = null;
                if (this.UnidadOperativa.Id == (int)ETipoEmpresa.Generacion) {
                    if (this.ddlSector.SelectedIndex > 0)
                        sector = (ESectorGeneracion)Enum.Parse(typeof(ESectorGeneracion), this.ddlSector.SelectedValue);
                } 
                if (this.UnidadOperativa.Id == (int)ETipoEmpresa.Construccion) {
                    if (this.ddlSector.SelectedIndex > 0)
                        sector = (ESectorConstruccion)Enum.Parse(typeof(ESectorConstruccion), this.ddlSector.SelectedValue);
                }
                if (this.UnidadOperativa.Id == (int)ETipoEmpresa.Equinova) {
                    if (this.ddlSector.SelectedIndex > 0)
                        sector = (ESectorEquinova)Enum.Parse(typeof(ESectorEquinova), this.ddlSector.SelectedValue);
                }
                return sector;
            }
            set {
                if (this.UnidadOperativa.Id == (int)ETipoEmpresa.Generacion) {
                    if (value == null)
                        this.ddlSector.SelectedIndex = 0;
                    else
                        this.ddlSector.SelectedValue = ((int)(ESectorGeneracion)value).ToString();
                } 
                if (this.UnidadOperativa.Id == (int)ETipoEmpresa.Construccion) {
                    if (value == null)
                        this.ddlSector.SelectedIndex = 0;
                    else
                        this.ddlSector.SelectedValue = ((int)(ESectorConstruccion)value).ToString();
                }
                if (this.UnidadOperativa.Id == (int)ETipoEmpresa.Equinova) {
                    if (value == null)
                        this.ddlSector.SelectedIndex = 0;
                    else
                        this.ddlSector.SelectedValue = ((int)(ESectorEquinova)value).ToString();
                }
            }
        }

        public int? UUA {
            get { return this.UC; }
        }

        #region Propiedades Buscador

        public ECatalogoBuscador ViewState_Catalogo {
            get {
                return (ECatalogoBuscador)ViewState["BUSQUEDA"];
            }
            set {
                ViewState["BUSQUEDA"] = value;
            }
        }

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

        #endregion

        public ActaConstitutivaBO ActaConstitutivaSeleccionada {
            get { return ucDatosActaConstitutiva.ObtenerActaConstitutiva(); }
            set { ucDatosActaConstitutiva.MostrarDatosActaConstitutiva(value); }
        }

        public List<ActaConstitutivaBO> ActasConstitutivas {
            get {
                return this.Session["ActasConstitutivas01"] != null ? (List<ActaConstitutivaBO>)this.Session["ActasConstitutivas01"] : null;
            }
            set {
                if (value != null)
                    this.Session["ActasConstitutivas01"] = value;
                else
                    this.Session.Remove("ActasConstitutivas01");

                this.grvActasConstitutivas.DataSource = value;
                this.grvActasConstitutivas.DataBind();
            }
        }

        /// <summary>
        /// Atributo que contiene la lista de teléfonos ingresados por el cliente.
        /// </summary>
        public List<TelefonoClienteBO> ListaTelefonos {
            get {
                if (Session["ListaTelefonos"] == null)
                    return new List<TelefonoClienteBO>();
                else
                    return (List<TelefonoClienteBO>)Session["ListaTelefonos"];
            }
            set {
                Session["ListaTelefonos"] = value;
            }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new RegistrarCuentaClientePRE(this, this.ucDatosObligadoSolidario, this.ucDatosRepresentanteLegalUI1, ucDatosRepresentantesObligados);
                if (!Page.IsPostBack) {
                    presentador.Inicializar(false);
                    mvwRegistrarCuentaCliente.SetActiveView(viewRegistro);

                }
                ucDatosObligadoSolidario.hdlAgregarRepresentante = btnMostrarVistaRepresentantesObligados_Click;
                ucDatosRepresentantesObligados.MostrarDepositario(true);
                this.AgregarAtributoObservaciones();
                this.OcultarEscrituraRepresentante();

            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Método sobrecargado para mostrar los campos según sea el caso para el tipo de unidad Operativa.
        /// </summary>
        /// <param name="empresa">variable tipo enumerador que define el tipo de unidad operativa que esta ingresando</param>
        /// <param name="habilitar">variable booleana que validara si los campos nuevos se muestran habilitados o no</param>
        public void EstablecerAcciones(ETipoEmpresa empresa, Boolean habilitar) {
            string etiqueta = ObtenerEtiquetadelResource("CLI01", empresa);
            string etiqueta2 = ObtenerEtiquetadelResource("CLI02", empresa);
            string etiqueta3 = ObtenerEtiquetadelResource("CLI03", empresa);
            var json = "{\"CLI01\": \"" + etiqueta + "\", \"CLI02\": \"" + etiqueta2 + "\", \"CLI03\": \"" + etiqueta3 + "\"}";
            MostrarObservaciones();
            txtObservaciones.Enabled = habilitar;

            if (empresa == ETipoEmpresa.Construccion) {
                this.tblObservaciones.Visible = true;
            } else if (empresa == ETipoEmpresa.Generacion) {
                this.tblObservaciones.Visible = true;
            } else if (empresa == ETipoEmpresa.Idealease) {
                this.tblObservaciones.Visible = false;
                this.divLabelObserva.Visible = false;
            } else if (empresa == ETipoEmpresa.Equinova) {
                this.tblObservaciones.Visible = true;
            }

            if (empresa != ETipoEmpresa.Idealease) {
                trDiasUso.Visible = false;
                ddlSector.Visible = true;
                if (empresa == ETipoEmpresa.Construccion) {
                    if (ddlSector.Items.Count <= 0) {
                        Type type = typeof(ESectorConstruccion);
                        Array values = Enum.GetValues(typeof(ESectorConstruccion));
                        ListItem item = new ListItem { Enabled = true, Selected = true, Text = "SELECCIONE UNA OPCIÓN", Value = "-1" };
                        ddlSector.Items.Add(item);
                        foreach (int value in values) {
                            var memInfo = type.GetMember(type.GetEnumName(value));
                            var display = memInfo[0]
                                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                .FirstOrDefault() as DescriptionAttribute;

                            if (display != null) {
                                ListItem itemenum = new ListItem(display.Description, value.ToString());
                                this.ddlSector.Items.Add(itemenum);
                            }
                        }
                    }
                } else if (empresa == ETipoEmpresa.Generacion) {
                    if (ddlSector.Items.Count <= 0) {
                        Type type = typeof(ESectorGeneracion);
                        Array values = Enum.GetValues(typeof(ESectorGeneracion));
                        ListItem item = new ListItem { Enabled = true, Selected = true, Text = "SELECCIONE UNA OPCIÓN", Value = "-1" };
                        ddlSector.Items.Add(item);
                        foreach (int value in values) {
                            var memInfo = type.GetMember(type.GetEnumName(value));
                            var display = memInfo[0]
                                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                .FirstOrDefault() as DescriptionAttribute;

                            if (display != null) {
                                ListItem itemenum = new ListItem(display.Description, value.ToString());
                                this.ddlSector.Items.Add(itemenum);
                            }
                        }
                    }
                } else if (empresa == ETipoEmpresa.Equinova) {
                    if (ddlSector.Items.Count <= 0) {
                        Type type = typeof(ESectorEquinova);
                        Array values = Enum.GetValues(typeof(ESectorEquinova));
                        ListItem item = new ListItem { Enabled = true, Selected = true, Text = "SELECCIONE UNA OPCIÓN", Value = "-1" };
                        ddlSector.Items.Add(item);
                        foreach (int value in values) {
                            var memInfo = type.GetMember(type.GetEnumName(value));
                            var display = memInfo[0]
                                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                .FirstOrDefault() as DescriptionAttribute;

                            if (display != null) {
                                ListItem itemenum = new ListItem(display.Description, value.ToString());
                                this.ddlSector.Items.Add(itemenum);
                            }
                        }
                    }
                }
                if (!habilitar) ddlSector.Enabled = false;
                else ddlSector.Enabled = true;
            } else {
                trDiasUso.Visible = true;
                this.trTelefono.Visible = false;
                ddlSector.Visible = false;
                if (!habilitar) ddlSector.Enabled = false;
                else ddlSector.Enabled = true;
            }

            if (empresa != ETipoEmpresa.Idealease) {
                txtTelefonos.Visible = true;
                btnMas.Visible = true;
                grdvTelefonos.Visible = true;
                if (!habilitar) {
                    txtTelefonos.Enabled = false;
                    btnMas.Enabled = false;
                } else {
                    txtTelefonos.Enabled = true;
                    btnMas.Enabled = true;
                }
            } else {
                this.trTelefono.Visible = false;
                txtTelefonos.Visible = false;
                btnMas.Visible = false;
                grdvTelefonos.Visible = false;
                if (!habilitar) {
                    txtTelefonos.Enabled = false;
                    btnMas.Enabled = false;
                } else {
                    txtTelefonos.Enabled = true;
                    btnMas.Enabled = true;
                }
            }

            if (empresa != ETipoEmpresa.Idealease) {
                this.lblSector.InnerText = etiqueta;
                this.lblTipoCuenta.InnerText = etiqueta2;
                this.lblHorasUso.InnerText = etiqueta3;

                //Se oculta la columna notario no es requerida para generación ni construcción
                this.grvActasConstitutivas.Columns[2].Visible = false;
            }
        }


        /// <summary>
        /// Método EstablecerAcciones mediante la lista de acciones, permitiendo el acceso o no a los campos de la ventana.
        /// </summary>
        public void EstablecerAcciones() {
            bool finaliza = false;
            ucDatosRepresentanteLegalUI1.UnidadOperatiaId = (int)UnidadOperativa.Id;
            ucDatosRepresentanteLegalUI1.ListaAcciones = this.ListaAcciones;
            ucDatosRepresentanteLegalUI1.HabilitarCampos(false);
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            foreach (CatalogoBaseBO acciones in lstAcciones) {
                if (finaliza == true) break;
                switch (acciones.Nombre.ToUpper()) {
                    case "ACTUALIZARCOMPLETO":
                        EstablecerAcciones(empresa, false);
                        finaliza = true;
                        break;
                }
            }
        }
        #region SC0005

        /// <summary>
        /// Método que obtiene el nombre de la etiqueta del archivo resource correspondiente a la unidad operativa a la cual accedió el usuario.
        /// </summary>
        /// <param name="cEtiquetaBuscar">Nombre de la etiqueta que será buscada en el archivo resource</param>
        /// <param name="tipoEmpresa">Nombre de la unidad operativa a la cual accedió el usuario.</param>
        /// <returns>Retorna el nombre de la etiqueta correspondiente al valor recibido en el parámetro cEtiquetaBuscar del archivo resource.</returns>
        public string ObtenerEtiquetadelResource(string cEtiquetaBuscar, ETipoEmpresa tipoEmpresa) {
            string cEtiqueta = string.Empty;
            //Instanciamos la clase o webmethod que obtiene las etiquetas
            ObtenerEtiquetaEmpresas obtenerEtiqueta = new ObtenerEtiquetaEmpresas();
            string cEtiquetaObtenida = string.Empty;
            EtiquetaObtenida request = null;

            cEtiquetaObtenida = obtenerEtiqueta.ObtenerEtiquetadelResource(cEtiquetaBuscar, (int)tipoEmpresa);
            request = JsonConvert.DeserializeObject<EtiquetaObtenida>(cEtiquetaObtenida);
            if (string.IsNullOrEmpty(request.cMensaje)) {
                cEtiquetaObtenida = request.cEtiqueta;
                if (cEtiqueta != "NO APLICA") {
                    cEtiqueta = cEtiquetaObtenida;
                }
            }
            return cEtiqueta;
        }

        /// <summary>
        /// Método encargado de mostrar el panel con el campo de observaciones de la solución.
        /// </summary>
        public void MostrarObservaciones() {
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            if (empresa != ETipoEmpresa.Idealease) {
                divObservaciones.Visible = true;
            } else {
                divObservaciones.Visible = false;
            }
        }

        public void MostrarDetalleObligado(List<RepresentanteLegalBO> representantes) {
            RepresentantesObligados = representantes;
            grdRepresentantesObligados.DataSource = RepresentantesObligados;
            grdRepresentantesObligados.DataBind();
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            if (empresa != ETipoEmpresa.Idealease)
                grdRepresentantesObligados.Columns[2].Visible = false;
        }
        public void MostrarRegistro() {
            mvwRegistrarCuentaCliente.SetActiveView(viewRegistro);
        }
        public void MostrarRepresentanteObligado() {
            mvwRegistrarCuentaCliente.SetActiveView(viewRepresentantesObligados);
        }

        #endregion

        public void ActualizarObligadosSolidarios() {
            this.grdObligadosSolidarios.DataSource = this.ObligadosSolidarios;
            this.grdObligadosSolidarios.DataBind();
        }
        public void ActualizarRepresentantesLegales() {
            this.grdRepresentantesLegales.DataSource = this.RepresentantesLegales;
            this.grdRepresentantesLegales.DataBind();
        }

        public void DeshabilitarCampos() {
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            this.txtFechaRegistro.Enabled = false;
            this.txtGiroEmpresa.Enabled = false;
            this.txtCURP.Enabled = false;
            #region SC0001
            this.txtCorreo.Enabled = false;
            this.txtDiasUso.Enabled = false;
            this.txtHorasUso.Enabled = false;
            #endregion
            this.btnAgregarObligadoSolidario.Enabled = false;
            this.btnAgregarRepresentante.Enabled = false;
            this.ddlTipoCuenta.Enabled = false;
            this.btnGuardar.Enabled = false;
            this.btnCancelar.Enabled = false;
            ucDatosActaConstitutiva.HabilitarCampos(false);
            this.btnAgregarActa.Enabled = false;
            this.btnLimpiarActa.Enabled = false;
        }

        public void EstablecerDatosNavegacion(string nombre, object objeto) {
            Session[nombre] = objeto;
        }

        public void HabilitarCampos() {
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            txtFechaRegistro.Enabled = true;
            txtGiroEmpresa.Enabled = true;
            txtCURP.Enabled = false;
            #region SC0001
            this.txtCorreo.Enabled = true;
            this.txtDiasUso.Enabled = true;
            this.txtHorasUso.Enabled = true;
            #endregion
            btnAgregarObligadoSolidario.Enabled = true;
            btnAgregarRepresentante.Enabled = true;
            ddlTipoCuenta.Enabled = false;
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;
            ucDatosActaConstitutiva.HabilitarCampos(true);
            this.btnAgregarActa.Enabled = true;
            this.btnLimpiarActa.Enabled = true;
        }

        public void LimpiarSesion() {
            if (Session["ListaObligadosSolidarios"] != null)
                Session.Remove("ListaObligadosSolidarios");
            if (Session["ListaRepresentantesLegales"] != null)
                Session.Remove("ListaRepresentantesLegales");
            if (Session["Cliente"] != null)
                Session.Remove("Cliente");
        }

        public void MostrarActaConstitutiva() {
            this.pnlActaConstitutiva.Visible = true;
            this.txtCURP.Enabled = false;
            divRepresentantesLegales.Visible = true;
        }
        public void MostrarHacienda() {
            this.pnlRegistroHacienda.Visible = true;
            this.txtCURP.Enabled = true;
        }
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            Site master = (Site)Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            } else {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        public void MostrarTipoCuenta() {
            ddlTipoCuenta.Items.Clear();
            foreach (var tipo in Enum.GetValues(typeof(ETipoCuenta))) {
                this.ddlTipoCuenta.Items.Add(new ListItem(Enum.GetName(typeof(ETipoCuenta), tipo), ((int)tipo).ToString()));
            }
        }

        public void OcultarActaConstitutiva() {
            this.pnlActaConstitutiva.Visible = false;
            this.txtCURP.Enabled = true;
            divRepresentantesLegales.Visible = false;
        }
        public void OcultarHacienda() {
            this.pnlRegistroHacienda.Visible = false;
            this.txtCURP.Enabled = false;
        }

        public void OcultarEscrituraRepresentante() {
            ucDatosRepresentanteLegalUI1.ValidarEscrituraRepresentante(this.UnidadOperativa.Id);
        }

        public void RedirigirADetalle() {
            this.Response.Redirect(this.ResolveUrl("~/Comun.UI/DetalleCuentaClienteUI.aspx"));
        }

        public string ValidarActaConstitutiva(bool? validarrfc = true) {
            return ucDatosActaConstitutiva.ValidarCampos(validarrfc);
        }

        public void ReiniciarCampos() {
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            if (empresa != ETipoEmpresa.Idealease) {
                this.txtCorreo.Text = "";
                this.txtTelefonos.Text = "";
                this.txtObservaciones.Text = "";
                this.ListaTelefonos = null;
                this.grdvTelefonos.DataSource = null;
                this.grdvTelefonos.DataBind();
                this.ddlSector.SelectedIndex = -1;
                if (Session["Telefonos"] != null)
                    Session.Remove("Telefonos");
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

        #region SC0008
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        #endregion

        #region Métodos para el Buscador

        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo) {
            if (Session_BOSelecto == null) {
                presentador.Inicializar(false);
                mvwRegistrarCuentaCliente.SetActiveView(viewRegistro);
            }
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            Session_BOSelecto = null;
        }

        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="tipoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda) {
            ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
        }

        #endregion

        #endregion

        #region Eventos

        #region SC0005

        protected void btnAgregarRepresentanteObligado_Click(object sender, EventArgs e) {
            presentador.AgregarRepresentanteLegalObligado(false);
        }

        protected void btnCancelarRepresentanteObligado_Click(object sender, EventArgs e) {
            presentador.MostrarRegistro();
            presentador.LimpiarRepresentanteObligado();
        }

        protected void btnMostrarVistaRepresentantesObligados_Click(object sender, EventArgs e) {
            presentador.LimpiarRepresentanteObligado();
            presentador.MostrarRepresentanteObligado();
        }

        protected void grdObligadosSolidarios_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {
                if (e.Row.RowType != DataControlRowType.DataRow) return;
                ObligadoSolidarioBO bo = e.Row.DataItem != null ? (ObligadoSolidarioBO)e.Row.DataItem : new ObligadoSolidarioProxyBO();//SC0005
                if (bo.TipoObligado == ETipoObligadoSolidario.Fisico)//SC0005
                {
                    e.Row.FindControl("ibtDetalle").Visible = false;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al desplegar los Obligados Solidarios", ETipoMensajeIU.ERROR, "RegistrarCuentaClienteUI" + ".grdObligadosSolidarios_RowDataBound" + ex.Message);
            }
        }

        protected void grdRepresentantesObligados_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            grdRepresentantesObligados.DataSource = RepresentantesObligados;
            grdRepresentantesObligados.PageIndex = e.NewPageIndex;
            grdRepresentantesObligados.DataBind();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), null, "DialogoDetalleObligado();", true);
        }

        #endregion

        protected void btnAgregarObligadoSolidario_Click(object sender, EventArgs e) {
            presentador.AgregarObligadoSolidario(false);
        }

        protected void btnAgregarRepresentante_Click(object sender, EventArgs e) {
            presentador.AgregarRepresentanteLegal(false, false);
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            if (empresa != ETipoEmpresa.Idealease)
                this.grdRepresentantesLegales.Columns[3].Visible = false;
        }

        protected void btnCancelar_Click(object sender, EventArgs e) {
            this.presentador.Inicializar(false);
        }

        protected void btnGuardar_Click(object sender, EventArgs e) {
            this.presentador.RegistrarCliente();
        }

        /// <summary>
        /// Dispara el cambio de paginacion del grid de obligados solidarios del cliente asignados al contrato
        /// </summary>
        /// <param name="sender">objeto que dispara el evento</param>
        /// <param name="e">parametros del evento</param>
        protected void grdObligadosSolidarios_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                EstablecerAcciones((ETipoEmpresa)this.UnidadOperativa.Id, true);
                grdObligadosSolidarios.DataSource = this.ObligadosSolidarios;
                grdObligadosSolidarios.PageIndex = e.NewPageIndex;
                grdObligadosSolidarios.DataBind();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdObligadosSolidarios_PageIndexChanging: " + ex.Message);
            }
        }

        /// <summary>
        /// Determina la eliminacion de la asignacion de un obligado solidario del contrato
        /// </summary>
        /// <param name="sender">objeto que dispara el evento</param>
        /// <param name="e">parametros del evento</param>
        protected void grdObligadosSolidarios_RowCommand(object sender, GridViewCommandEventArgs e) {
            try {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                int index = Convert.ToInt32(e.CommandArgument);
                ObligadoSolidarioBO obligado = this.ObligadosSolidarios[index];

                switch (eCommandNameUpper) {
                    case "CMDELIMINAR":

                        presentador.QuitarObligadoSolidario(obligado);
                        break;

                    case "CMDDETALLE":

                        index = Convert.ToInt32(e.CommandArgument);
                        presentador.MostrarDetalleObligado(obligado);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), null, "DialogoDetalleObligado();", true);

                        break;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".grdObligadosSolidarios_RowCommand: " + ex.Message);
            }
        }

        /// <summary>
        /// Dispara el cambio de paginacion del grid de representantes legales del cliente asignados al contrato
        /// </summary>
        /// <param name="sender">objeto que dispara el evento</param>
        /// <param name="e">parametros del evento</param>
        protected void grdRepresentantesLegales_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                grdRepresentantesLegales.DataSource = this.RepresentantesLegales;
                grdRepresentantesLegales.PageIndex = e.NewPageIndex;
                grdRepresentantesLegales.DataBind();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdRepresentantesLegales_PageIndexChanging: " + ex.Message);
            }
        }

        /// <summary>
        /// Determina la eliminacion de la asignacion de un representente legal del contrato
        /// </summary>
        /// <param name="sender">objeto que dispara el evento</param>
        /// <param name="e">parametros del evento</param>
        protected void grdRepresentantesLegales_RowCommand(object sender, GridViewCommandEventArgs e) {
            try {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                int index = Convert.ToInt32(e.CommandArgument);
                RepresentanteLegalBO representante = this.RepresentantesLegales[index];

                presentador.QuitarRepresentanteLegal(representante);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".grdRepresentantesLegales_RowCommand: " + ex.Message);
            }
        }

        #region Eventos para el Buscador

        protected void btnResult_Click(object sender, EventArgs e) {
            try {
                switch (ViewState_Catalogo) {
                    case ECatalogoBuscador.Cliente:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        EstablecerAcciones((ETipoEmpresa)this.UnidadOperativa.Id, true);
                        break;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click: " + ex.Message);
            }
        }

        protected void ibtnBuscarCliente_Click(object sender, EventArgs e) {
            try {
                this.EjecutaBuscador("Cuenta&hidden=0", ECatalogoBuscador.Cliente);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al buscar un cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarCliente_Click:" + ex.Message);
            }
        }

        #region SC0001
        protected void txtHorasUso_TextChanged(object sender, EventArgs e) {
            if (HorasUsoUnidad.Value > 24) {
                this.txtHorasUso.Text = string.Empty;
                this.MostrarMensaje("LAS HORAS AL DIA NO PUEDEN SER MAYORES A 24", ETipoMensajeIU.ADVERTENCIA);
            }

        }
        #endregion

        protected void txtCliente_TextChanged(object sender, EventArgs e) {
            try {
                string nombreCuentaCliente = this.NombreCliente;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.Cliente);

                this.NombreCliente = nombreCuentaCliente;
                if (this.NombreCliente != null) {
                    EjecutaBuscador("Cuenta&hidden=0", ECatalogoBuscador.Cliente);
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtCliente_TextChanged:" + ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// Gestiona la paginación del GridView
        /// </summary>
        /// <param name="sender">Objeto que genera el evento</param>
        /// <param name="e">Contiene información del evento</param>
        protected void grvActasConstitutivas_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                this.grvActasConstitutivas.DataSource = this.ActasConstitutivas;
                this.grvActasConstitutivas.PageIndex = e.NewPageIndex;
                this.grvActasConstitutivas.DataBind();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grvActasConstitutivas_PageIndexChanging: " + ex.Message);
            }
        }
        /// <summary>
        /// Gestiona el comando ejecutado en el GridView
        /// </summary>
        /// <param name="sender">Objeto que genera el evento</param>
        /// <param name="e">Contiene información del evento</param>
        protected void grvActasConstitutivas_RowCommand(object sender, GridViewCommandEventArgs e) {
            try {
                string eCommandNameUpper = e.CommandName.ToString().ToUpper();
                if (eCommandNameUpper == "CMDDEDITAR") {
                    int actaId = int.Parse(e.CommandArgument.ToString());
                    if (actaId > -1)
                        this.presentador.MostrarActaConstitutiva(actaId);
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".grvActasConstitutivas_RowCommand: " + ex.Message);
            }
        }
        /// <summary>
        /// Agrego o actualiza información de acta constitutiva
        /// </summary>
        /// <param name="sender">Objeto que genera el evento</param>
        /// <param name="e">Contiene información del evento</param>
        protected void btnAgregarActa_Click(object sender, EventArgs e) {
            try {
                if (this.presentador.AgregarActaConstitutiva(true))
                    this.ucDatosActaConstitutiva.LimpiarCampos();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al ejecutar la operación.", ETipoMensajeIU.ERROR, nombreClase + ".btnAgregar_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Limpia la captura de los datos
        /// </summary>
        /// <param name="sender">Objeto que genera el evento</param>
        /// <param name="e">Contiene información del evento</param>
        protected void btnLimpiarActa_Click(object sender, EventArgs e) {
            try {
                this.ucDatosActaConstitutiva.LimpiarCampos();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al ejecutar la operación.", ETipoMensajeIU.ERROR, nombreClase + ".btnLimpiarActa_Click: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento del botón para agregar teléfonos del TextBox de Teléfonos al Grid correspondiente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMas_Click(object sender, EventArgs e) {
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            TelefonoClienteBO telefono = new TelefonoClienteBO();
            if (txtTelefonos.Text != String.Empty) {
                telefono.Telefono = txtTelefonos.Text;
                DataTable TableTemp = DtTelefonos;
                List<TelefonoClienteBO> listaTelefonos = ListaTelefonos;
                datarow = TableTemp.NewRow();
                telefono.Telefono = txtTelefonos.Text;
                datarow["Telefono"] = txtTelefonos.Text;
                TableTemp.Rows.Add(datarow);
                grdvTelefonos.DataSource = TableTemp;
                grdvTelefonos.DataBind();
                txtTelefonos.Text = "";
                DtTelefonos = TableTemp;
                if (listaTelefonos.Contains(telefono) == false)
                    listaTelefonos.Add(telefono);

                this.ListaTelefonos = listaTelefonos;
            }
            EstablecerAcciones(empresa, true);
        }

        /// <summary>
        /// Evento que controla la paginación de los números de teléfonos del cliente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdvTelefonos_OnPageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
                this.grdvTelefonos.DataSource = this.ListaTelefonos;
                this.grdvTelefonos.PageIndex = e.NewPageIndex;
                this.grdvTelefonos.DataBind();
                EstablecerAcciones(empresa, true);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grvActasConstitutivas_PageIndexChanging: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento de Grid de teléfonos para eliminar un Row que sea seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdvTelefonos_OnRowDeleting(object sender, GridViewDeleteEventArgs e) {
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            List<TelefonoClienteBO> listaTelefonos = ListaTelefonos;
            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt = DtTelefonos;
            dt.Rows[index].Delete();
            ViewState["Telefono"] = dt;
            grdvTelefonos.DataSource = dt;
            grdvTelefonos.DataBind();
            listaTelefonos.RemoveAt(index);
            EstablecerAcciones(empresa, true);
            ListaTelefonos = listaTelefonos;
        }

        /// <summary>
        /// Evento que controla la selección del DropDownList de Sector.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSector_SelectedIndexChanged(object sender, EventArgs e) {
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            switch (empresa) {
                case ETipoEmpresa.Construccion:
                    this.SectorCliente = (ESectorConstruccion)System.Enum.Parse(typeof(ESectorConstruccion), ddlSector.SelectedValue);
                    break;
                case ETipoEmpresa.Generacion:
                    this.SectorCliente = (ESectorGeneracion)System.Enum.Parse(typeof(ESectorGeneracion), ddlSector.SelectedValue);
                    break;
                case ETipoEmpresa.Equinova:
                    this.SectorCliente = (ESectorEquinova)System.Enum.Parse(typeof(ESectorEquinova), ddlSector.SelectedValue);
                    break;
            }
        }

        /// <summary>
        /// Aplica atributo al campo Observaciones
        /// </summary>
        public void AgregarAtributoObservaciones() {
            txtObservaciones.Attributes.Add("maxlength", txtObservaciones.MaxLength.ToString());
        }
        #endregion

        protected void grdvTelefonos_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                grdvTelefonos.DataSource = this.ListaTelefonos;
                grdvTelefonos.PageIndex = e.NewPageIndex;
                grdvTelefonos.DataBind();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdvTelefonos_PageIndexChanging: " + ex.Message);
            }
        }
    }
}