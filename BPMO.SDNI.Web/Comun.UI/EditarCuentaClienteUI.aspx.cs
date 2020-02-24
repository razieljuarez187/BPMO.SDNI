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
    public partial class EditarCuentaClienteUI : System.Web.UI.Page, IEditarCuentaClienteVIS {
        #region Atributos

        private string nombreClase = "EditarClienteUI";
        private EditarCuentaClientePRE presentador;
        private List<CatalogoBaseBO> lstAcciones;
        public List<TelefonoClienteBO> lstTelefonos;

        public DataRow datarow;
        #endregion Atributos

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
            get { return (String.IsNullOrEmpty(this.txtObservaciones.Text.Trim())) ? null : this.txtObservaciones.Text.ToUpper(); }
            set { this.txtObservaciones.Text = value != null ? value : string.Empty; }
        }

        /// <summary>
        /// Atributo para almacenar los elementos del Grid de Teléfonos del cliente.
        /// </summary>
        public DataTable DtTelefonos {
            get {
                DataTable DTTemp = new DataTable();
                DTTemp.TableName = "Telefono";
                DTTemp.Columns.Add("TelefonoId", typeof(System.Int32));
                DTTemp.Columns.Add("Telefono", typeof(System.String));
                if (Session["ListaTelefonos"] != null) {
                    foreach (TelefonoClienteBO telefonoBO in ListaTelefonos) {
                        if (telefonoBO.Activo) {
                            DataRow dr = DTTemp.NewRow();
                            dr[0] = telefonoBO.TelefonoId.GetValueOrDefault();
                            dr[1] = telefonoBO.Telefono.ToString();
                            DTTemp.Rows.Add(dr);
                        }
                    }
                }
                return DTTemp;
            }
            set {
                Session["ListaTelefonos"] = value;
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


        /// <summary>
        /// Atributo que contiene la lista de teléfonos ingresados por el cliente.
        /// </summary>
        public List<TelefonoClienteBO> ListaTelefonos {
            get {
                if (Session["ListaTelefonos"] == null) {
                    lstTelefonos = new List<TelefonoClienteBO>();
                    return lstTelefonos;
                } else
                    return (List<TelefonoClienteBO>)Session["ListaTelefonos"];
            }
            set {
                if (value != null)
                    this.Session["ListaTelefonos"] = value;
                else
                    this.Session.Remove("ListaTelefonos");

                this.grdvTelefonos.DataSource = value != null ? value.Where(x => x.Activo).ToList() : value;
                this.grdvTelefonos.DataBind();
            }
        }

        public CuentaClienteIdealeaseBO Cliente {
            get {
                if (Session["Cliente"] == null)
                    return new CuentaClienteIdealeaseBO();
                return Session["Cliente"] as CuentaClienteIdealeaseBO;
            }
            set {
                Session["Cliente"] = value;
            }
        }

        public CuentaClienteIdealeaseBO ClienteAnterior {
            get {
                if (Session["LastCliente"] == null)
                    return new CuentaClienteIdealeaseBO();
                return Session["LastCliente"] as CuentaClienteIdealeaseBO;
            }
            set {
                Session["LastCliente"] = value;
            }
        }

        public int? ClienteID {
            get {
                TextBox txtClienteID = mCuentaCliente.Controls[0].FindControl("txtValue") as TextBox;
                if (String.IsNullOrEmpty(txtClienteID.Text.Trim()))
                    return Convert.ToInt32(txtClienteID.Text.Trim());

                return null;
            }
            set {
                TextBox txtClienteID = mCuentaCliente.Controls[0].FindControl("txtValue") as TextBox;
                if (txtClienteID != null) {
                    if (value != null)
                        txtClienteID.Text = value.ToString();
                    else txtClienteID.Text = string.Empty;
                }
            }
        }

        public string CURP {
            get { return (String.IsNullOrEmpty(this.txtCURP.Text.Trim())) ? null : this.txtCURP.Text.Trim().ToUpper(); }
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
            get {
                return DateTime.Today;
            }
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

        public string Nombre {
            get { return (String.IsNullOrEmpty(this.txtNombreCliente.Text.Trim())) ? null : this.txtNombreCliente.Text.ToUpper(); }
            set {
                if (value != null)
                    this.txtNombreCliente.Text = value;
                else
                    this.txtNombreCliente.Text = string.Empty;
            }
        }

        public List<ObligadoSolidarioBO> Obligados {
            get {
                if (Session["ListaObligados"] == null)
                    return new List<ObligadoSolidarioBO>();
                return Session["ListaObligados"] as List<ObligadoSolidarioBO>;
            }
            set {
                Session["ListaObligados"] = value;
            }
        }

        public List<ObligadoSolidarioBO> ObligadosInactivos {
            get {
                if (Session["ListaObligadosInactivos"] == null)
                    return new List<ObligadoSolidarioBO>();
                return Session["ListaObligadosInactivos"] as List<ObligadoSolidarioBO>;
            }
            set {
                Session["ListaObligadosInactivos"] = value;
            }
        }

        public List<RepresentanteLegalBO> Representantes {
            get {
                if (Session["ListaRepresentantes"] == null)
                    return new List<RepresentanteLegalBO>();
                return Session["ListaRepresentantes"] as List<RepresentanteLegalBO>;
            }
            set {
                Session["ListaRepresentantes"] = value;
            }
        }

        public List<RepresentanteLegalBO> RepresentantesInactivos {
            get {
                if (Session["ListaRepresentantesInactivos"] == null)
                    return new List<RepresentanteLegalBO>();
                return Session["ListaRepresentantesInactivos"] as List<RepresentanteLegalBO>;
            }
            set {
                Session["ListaRepresentantesInactivos"] = value;
            }
        }

        public List<RepresentanteLegalBO> RepresentantesObligados {
            get {
                if (Session["RepresentantesObligadosSol"] == null) return new List<RepresentanteLegalBO>();
                return Session["RepresentantesObligadosSol"] as List<RepresentanteLegalBO>;
            }
            set { Session["RepresentantesObligadosSol"] = value; }
        }

        public string RFC {
            get { return (String.IsNullOrEmpty(this.txtRFC.Text.Trim())) ? null : this.txtRFC.Text.Trim().ToUpper(); }
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
                Site master = (Site)Page.Master;
                if (master.Usuario.Id != null)
                    id = master.Usuario.Id;

                return id;
            }
        }

        public int? UUA {
            get { return this.UC; }
        }

        #region SC0008
        public UnidadOperativaBO UnidadOperativa {
            get {
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    return new UnidadOperativaBO { Id = masterMsj.Adscripcion.UnidadOperativa.Id };
                return null;
            }
        }
        #endregion SC0008
        #endregion Propiedades

        #region Constructores

        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new EditarCuentaClientePRE(this, ucDatosObligadoSolidario, ucDatosRepresentanteLegal, ucDatosRepresentantesObligados);

                if (!Page.IsPostBack) {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso(); //SC_0008

                    this.MostrarTipoCuenta();
                    presentador.Inicializar();
                    mvwEditarCuentaCliente.SetActiveView(viewEdicion);
                } else {
                    this.EstablerEtiqueta();
                }
                ucDatosObligadoSolidario.hdlAgregarRepresentante = btnMostrarVistaRepresentantesObligados_Click;
                ucDatosRepresentantesObligados.MostrarDepositario(false);
                this.AgregarAtributoObservaciones();
                this.OcultarEscrituraRepresentante();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        #endregion Constructores

        #region Métodos
        /// <summary>
        /// Método sobrecargado para mostrar los campos según sea el caso para el tipo de unidad Operativa.
        /// </summary>
        /// <param name="empresa">variable tipo enumerador que define el tipo de unidad operativa que esta ingresando</param>
        /// <param name="habilitar">variable booleana que validara si los campos nuevos se muestran habilitados o no</param>
        public void EstablecerAcciones(ETipoEmpresa empresa, Boolean habilitar) {
            MostrarControles();

            if (empresa != ETipoEmpresa.Idealease) {
                this.txtDiasUso.Visible = false;
                this.txtHorasUso.Visible = false;
                this.ddlSector.Visible = true;
                this.grdvTelefonos.Visible = true;
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
                ddlSector.Visible = true;
                if (!habilitar) ddlSector.Enabled = false;
                else ddlSector.Enabled = true;
            }

            if (empresa == ETipoEmpresa.Idealease)
                txtHorasUso.Visible = true;
            else
                txtHorasUso.Visible = false;

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
                //Se oculta la columna notario no es requerida para generación ni construcción
                this.grvActasConstitutivas.Columns[2].Visible = false;
            } else {
                ddlSector.Visible = false;
                txtTelefonos.Visible = false;
                btnMas.Visible = false;
                this.grdvTelefonos.Visible = true;
                if (!habilitar) {
                    txtTelefonos.Enabled = false;
                    btnMas.Enabled = false;
                } else {
                    txtTelefonos.Enabled = true;
                    btnMas.Enabled = true;
                }
            }
            EstablerEtiqueta();
        }

        /// <summary>
        /// Asignación de etiquetas de interfaz
        /// </summary>
        public void EstablerEtiqueta() {
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            string etiqueta = ObtenerEtiquetadelResource("CLI01", empresa);
            string etiqueta2 = ObtenerEtiquetadelResource("CLI02", empresa);
            string etiqueta3 = ObtenerEtiquetadelResource("CLI03", empresa);

            var json = "{\"CLI01\": \"" + etiqueta + "\", \"CLI02\": \"" + etiqueta2 + "\", \"CLI03\": \"" + etiqueta3 + "\"}";
            this.RegistrarScript("EventsEditarCliente", "InicializarControlesEmpresas('" + json + "');");
        }

        /// <summary>
        /// Método EstablecerAcciones mediante la lista de acciones, permitiendo el acceso o no a los campos de la ventana.
        /// </summary>
        public void EstablecerAcciones() {
            bool finaliza = false;
            ucDatosRepresentanteLegal.UnidadOperatiaId = (int)UnidadOperativa.Id;
            ucDatosRepresentanteLegal.ListaAcciones = this.ListaAcciones;
            ucDatosRepresentanteLegal.HabilitarCampos(true);
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            foreach (CatalogoBaseBO acciones in lstAcciones) {
                if (finaliza == true) break;
                switch (acciones.Nombre.ToUpper()) {
                    case "ACTUALIZARCOMPLETO":
                        EstablecerAcciones(empresa, true);
                        finaliza = true;
                        break;
                }
            }
        }

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
        /// Método que muestra el control de Observaciones y grid de teléfono.
        /// </summary>
        public void MostrarControles() {
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            if (empresa == ETipoEmpresa.Construccion || empresa == ETipoEmpresa.Generacion || empresa == ETipoEmpresa.Equinova) {
                this.divObservaciones.Visible = true;
                this.grdvTelefonos.Visible = true;
            }
            if (empresa == ETipoEmpresa.Idealease) {
                this.divObservaciones.Visible = false;
                this.grdvTelefonos.Visible = false;
            }
        }
        #region SC0005

        public void MostrarDetalleObligado(List<RepresentanteLegalBO> representantes) {
            RepresentantesObligados = representantes;
            grdRepresentantesObligados.DataSource = RepresentantesObligados;
            grdRepresentantesObligados.DataBind();
        }

        public void MostrarEdicion() {
            mvwEditarCuentaCliente.SetActiveView(viewEdicion);
        }

        public void MostrarRepresentanteObligado() {
            mvwEditarCuentaCliente.SetActiveView(viewRepresentantesObligados);
        }

        #endregion SC0005

        public void ActualizarObligadosSolidarios() {
            this.grdObligadosSolidarios.DataSource = this.Obligados;
            this.grdObligadosSolidarios.DataBind();
        }

        public void ActualizarRepresentantesLegales() {
            this.grdRepresentantesLegales.DataSource = this.Representantes;
            this.grdRepresentantesLegales.DataBind();
        }

        public void DeshabilitarCampos() {
            this.txtFechaRegistro.Enabled = false;
            this.txtGiroEmpresa.Enabled = false;
            this.txtCURP.Enabled = false;

            #region SC0001
            this.txtCorreo.Enabled = false;
            #endregion
            this.btnAgregarObligadoSolidario.Enabled = false;
            this.btnAgregarRepresentante.Enabled = false;
            this.ddlTipoCuenta.Enabled = false;
            this.btnGuardar.Enabled = false;
            this.btnCancelar.Enabled = false;
            ucDatosActaConstitutiva.HabilitarCampos(false);
            this.btnAgregarActa.Enabled = false;
            this.btnLimpiarActa.Enabled = false;
            this.btnActualizarObligado.Enabled = false;
            this.btnActualizarRepresentante.Enabled = false;
            this.txtNumeroCuentaOracle.Enabled = false;
        }

        public void EstablecerPaquete(object bo) {
            Session["DatosCuentaClienteIdealeaseBO"] = bo;
        }

        public void HabilitarCampos() {
            txtFechaRegistro.Enabled = true;
            txtGiroEmpresa.Enabled = true;
            txtCURP.Enabled = true;
            #region SC0001
            this.txtCorreo.Enabled = true;
            #endregion
            btnAgregarObligadoSolidario.Enabled = true;
            btnAgregarRepresentante.Enabled = true;
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;
            ucDatosObligadoSolidario.HabilitarCampos(true);
            ucDatosActaConstitutiva.HabilitarCampos(true);
            this.btnAgregarActa.Enabled = true;
            this.btnLimpiarActa.Enabled = true;
        }

        public void LimpiarSesion() {
            if (Session["ListaObligadosInactivos"] != null)
                Session.Remove("ListaObligadosInactivos");
            if (Session["ListaRepresentantesInactivos"] != null)
                Session.Remove("ListaRepresentantesInactivos");
            if (Session["ListaObligados"] != null)
                Session.Remove("ListaObligados");
            if (Session["ListaRepresentantes"] != null)
                Session.Remove("ListaRepresentantes");
            if (Session["Cliente"] != null)
                Session.Remove("Cliente");
            if (Session["LastCliente"] != null)
                Session.Remove("LastCliente");
        }

        public void ModoAgregarObligado() {
            this.grdRepresentantesLegales.Enabled = true;
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            if (empresa != ETipoEmpresa.Idealease)
                this.grdRepresentantesLegales.Columns[3].Visible = false;
            this.grdObligadosSolidarios.Enabled = true;
            this.btnAgregarObligadoSolidario.Visible = true;
            this.btnAgregarObligadoSolidario.Enabled = true;

            this.btnAgregarRepresentante.Visible = true;
            this.btnAgregarRepresentante.Enabled = true;

            this.btnActualizarObligado.Visible = false;
            this.btnActualizarRepresentante.Visible = false;
            this.btnCancelarObligado.Visible = false;
            this.btnCancelarRepresentante.Visible = false;
        }

        public void ModoAgregarRepresentante() {
            this.grdRepresentantesLegales.Enabled = true;
            this.grdObligadosSolidarios.Enabled = true;
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            if (empresa != ETipoEmpresa.Idealease)
                this.grdRepresentantesLegales.Columns[3].Visible = false;
            this.btnAgregarObligadoSolidario.Visible = true;
            this.btnAgregarObligadoSolidario.Enabled = true;

            this.btnAgregarRepresentante.Visible = true;
            this.btnAgregarRepresentante.Enabled = true;

            this.btnCancelarRepresentante.Visible = false;
            this.btnCancelarObligado.Visible = false;
            this.btnActualizarObligado.Visible = false;
            this.btnActualizarRepresentante.Visible = false;
        }

        public void ModoEdicionObligado() {
            this.grdObligadosSolidarios.Enabled = false;
            this.grdRepresentantesLegales.Enabled = false;
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            if (empresa != ETipoEmpresa.Idealease) {
                this.grdRepresentantesLegales.Columns[3].Visible = false;
            }

            this.btnActualizarObligado.Visible = true;
            this.btnActualizarObligado.Enabled = true;
            this.btnCancelarObligado.Visible = true;
            this.btnCancelarObligado.Enabled = true;

            this.btnAgregarObligadoSolidario.Visible = false;
            this.btnAgregarRepresentante.Visible = true;
            this.btnAgregarRepresentante.Enabled = true;
            this.btnActualizarRepresentante.Visible = false;
            this.btnCancelarRepresentante.Visible = false;
        }

        public void ModoEdicionRepresentante() {
            this.grdObligadosSolidarios.Enabled = false;
            this.grdRepresentantesLegales.Enabled = false;
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            if (empresa != ETipoEmpresa.Idealease)
                this.grdRepresentantesLegales.Columns[3].Visible = false;

            this.btnActualizarRepresentante.Visible = true;
            this.btnActualizarRepresentante.Enabled = true;
            this.btnCancelarRepresentante.Visible = true;
            this.btnCancelarRepresentante.Enabled = true;

            this.btnAgregarRepresentante.Visible = false;
            this.btnAgregarObligadoSolidario.Visible = true;
            this.btnAgregarObligadoSolidario.Enabled = true;
            this.btnActualizarObligado.Visible = false;
            this.btnCancelarObligado.Visible = false;
        }

        public void ModoPresentarInformacion() {
            grdRepresentantesLegales.Enabled = false;
            grdObligadosSolidarios.Enabled = false;
            pnlObligadosSolidarios.Visible = false;
            pnlRepresentantesLegales.Visible = false;
        }

        public void MostrarActaConstitutiva() {
            this.pnlActaConstitutiva.Visible = true;
            divRepresentantesLegales.Visible = true;
            ucDatosActaConstitutiva.HabilitarCampos(true);
        }

        public void MostrarHacienda() {
            this.txtCURP.Enabled = true;
            this.pnlRegistroHacienda.Visible = true;
        }

        public void MostrarTelefonos() {
            this.grdvTelefonos.Visible = false;
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
            foreach (var tipo in Enum.GetValues(typeof(ETipoCuenta))) {
                this.ddlTipoCuenta.Items.Add(new ListItem(Enum.GetName(typeof(ETipoCuenta), tipo), ((int)tipo).ToString()));
            }
        }

        public object ObtenerDatos() {
            return Session["DatosCuentaClienteIdealeaseBO"];
        }

        public void OcultarActaConstitutiva() {
            this.pnlActaConstitutiva.Visible = false;
            this.txtCURP.Enabled = true;
            divRepresentantesLegales.Visible = false;
            ucDatosActaConstitutiva.HabilitarCampos(true);
        }

        public void OcultarHacienda() {
            this.txtCURP.Enabled = false;
            this.pnlRegistroHacienda.Visible = false;
        }

        public void OcultarEscrituraRepresentante() {
            ucDatosRepresentanteLegal.ValidarEscrituraRepresentante(this.UnidadOperativa.Id);
        }

        public void RedirigirAConsulta() {
            this.Response.Redirect(this.ResolveUrl("~/Comun.UI/ConsultarCuentaClienteUI.aspx"));
        }

        public void RedirigirADetalle() {
            this.Response.Redirect(this.ResolveUrl("~/Comun.UI/DetalleCuentaClienteUI.aspx"));
        }

        #region SC0008

        public void PermitirRegistrar(bool permitir) {
            this.hlRegistroOrden.Enabled = permitir;
        }

        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        #endregion

        #endregion Métodos

        #region Eventos


        #region SC0001

        protected void txtHorasUso_TextChanged(object sender, EventArgs e) {
            if (HorasUsoUnidad.Value > 24) {
                this.txtHorasUso.Text = string.Empty;
                this.MostrarMensaje("LAS HORAS AL DIA NO PUEDEN SER MAYORES A 24", ETipoMensajeIU.ADVERTENCIA);
            }
        }

        #endregion

        #region SC0005

        protected void btnAgregarRepresentanteObligado_Click(object sender, EventArgs e) {
            presentador.AgregarRepresentanteLegalObligado();
        }

        protected void btnCancelarRepresentanteObligado_Click(object sender, EventArgs e) {
            presentador.LimpiarRepresentanteObligado();
            presentador.MostrarEdicion();
        }

        protected void btnMostrarVistaRepresentantesObligados_Click(object sender, EventArgs e) {
            presentador.LimpiarRepresentanteObligado();
            presentador.MostrarRepresentanteObligado();
        }

        protected void grdRepresentantesObligados_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            grdRepresentantesObligados.DataSource = RepresentantesObligados;
            grdRepresentantesObligados.PageIndex = e.NewPageIndex;
            grdRepresentantesObligados.DataBind();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), null, "DialogoDetalleObligado();", true);
        }

        #endregion SC0005

        protected void btnActualizarObligado_Click(object sender, EventArgs e) {
            presentador.ActualizarObligadoSolidario();
        }

        protected void btnActualizarRepresentante_Click(object sender, EventArgs e) {
            this.presentador.ActualizarRepresentanteLegal();
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            if (empresa != ETipoEmpresa.Idealease)
                this.grdRepresentantesLegales.Columns[3].Visible = false;
        }

        protected void btnAgregarObligadoSolidario_Click(object sender, EventArgs e) {
            presentador.AgregarObligadosolidario();
        }

        protected void btnAgregarRepresentante_Click(object sender, EventArgs e) {
            this.presentador.AgregarRepresentanteLegal();
        }

        protected void btnCancelar_Click(object sender, EventArgs e) {
            this.presentador.Cancelar();
        }

        protected void btnCancelarObligado_Click(object sender, EventArgs e) {
            this.presentador.CancelarObligadoSolidario();
        }

        protected void btnCancelarRepresentante_Click(object sender, EventArgs e) {
            this.presentador.CancelarRepresentante();
        }

        protected void btnGuardar_Click(object sender, EventArgs e) {
            this.presentador.ActualizarCliente();
        }

        protected void grdObligadosSolidarios_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                EstablecerAcciones((ETipoEmpresa)this.UnidadOperativa.Id, true);
                grdObligadosSolidarios.DataSource = this.Obligados;
                grdObligadosSolidarios.PageIndex = e.NewPageIndex;
                grdObligadosSolidarios.DataBind();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdObligadosSolidarios_PageIndexChanging: " + ex.Message);
            }
        }

        protected void grdObligadosSolidarios_RowCommand(object sender, GridViewCommandEventArgs e) {
            try {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                int index;
                ObligadoSolidarioBO obligado;
                switch (eCommandNameUpper) {
                    case "CMDELIMINAR":
                        index = Convert.ToInt32(e.CommandArgument);
                        obligado = this.Obligados[index];

                        presentador.QuitarObligadoSolidario(obligado);
                        break;

                    case "CMDEDITAR":
                        int indexEdit = Convert.ToInt32(e.CommandArgument);
                        ObligadoSolidarioBO obligadoEdit = this.Obligados[indexEdit];
                        presentador.EditarObligadoSolidario(obligadoEdit);
                        break;

                    case "CMDDETALLE":

                        index = Convert.ToInt32(e.CommandArgument);
                        obligado = this.Obligados[index];
                        presentador.MostrarDetalleObligado(obligado);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), null, "DialogoDetalleObligado();", true);

                        break;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".grdObligadosSolidarios_RowCommand: " + ex.Message);
            }
        }

        protected void grdObligadosSolidarios_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {
                if (e.Row.RowType == DataControlRowType.DataRow) {
                    ObligadoSolidarioBO bo = e.Row.DataItem != null ? (ObligadoSolidarioBO)e.Row.DataItem : new ObligadoSolidarioProxyBO();

                    if (bo.TipoObligado == ETipoObligadoSolidario.Fisico)//SC0005
                    {
                        e.Row.FindControl("ibtDetalle").Visible = false;
                    }

                    if (bo.Id != null) {
                        ((ImageButton)e.Row.FindControl("ibtEliminar")).ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/BORRAR-ICO.png");
                        ((ImageButton)e.Row.FindControl("ibtEditar")).ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/EDITAR-ICO.png");
                    } else {
                        ((ImageButton)e.Row.FindControl("ibtEliminar")).ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ELIMINAR-ICO.png");
                        ((ImageButton)e.Row.FindControl("ibtEditar")).Visible = false;
                    }
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al desplegar los Obligados Solidarios", ETipoMensajeIU.ERROR, this.nombreClase + ".grdObligadosSolidarios_RowDataBound" + ex.Message);
            }
        }

        protected void grdRepresentantesLegales_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                grdRepresentantesLegales.DataSource = Representantes;
                grdRepresentantesLegales.PageIndex = e.NewPageIndex;
                grdRepresentantesLegales.DataBind();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".gvRepresentantesLegales_PageIndexChanging: " + ex.Message);
            }
        }

        protected void grdRepresentantesLegales_RowCommand(object sender, GridViewCommandEventArgs e) {
            try {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                switch (eCommandNameUpper) {
                    case "CMDEDITAR":
                        int index = Convert.ToInt32(e.CommandArgument);
                        RepresentanteLegalBO representanteEdit = this.Representantes[index];

                        presentador.EditarRepresentanteLegal(representanteEdit);
                        break;

                    case "CMDELIMINAR":
                        int indexEdit = Convert.ToInt32(e.CommandArgument);
                        RepresentanteLegalBO representante = this.Representantes[indexEdit];

                        presentador.QuitarRepresentanteLegal(representante);
                        break;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".grdRepresentantesLegales_RowCommand: " + ex.Message);
            }
        }

        protected void grdRepresentantesLegales_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {
                if (e.Row.RowType == DataControlRowType.DataRow) {
                    RepresentanteLegalBO bo = e.Row.DataItem != null ? (RepresentanteLegalBO)e.Row.DataItem : new RepresentanteLegalBO();
                    if (bo.Id != null) {
                        ((ImageButton)e.Row.FindControl("ibtEliminar")).ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/BORRAR-ICO.png");
                        ((ImageButton)e.Row.FindControl("ibtEditar")).ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/EDITAR-ICO.png");
                    } else {
                        ((ImageButton)e.Row.FindControl("ibtEliminar")).ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ELIMINAR-ICO.png");
                        ((ImageButton)e.Row.FindControl("ibtEditar")).Visible = false;
                    }
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al desplegar los Obligados Solidarios", ETipoMensajeIU.ERROR, this.nombreClase + ".grdObligadosSolidarios_RowDataBound" + ex.Message);
            }
        }

        #endregion Eventos
        public string ValidarActaConstitutiva() {
            return ucDatosActaConstitutiva.ValidarCampos();
        }
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
                    if (actaId > -1) {
                        this.presentador.MostrarActaConstitutiva(actaId);
                        this.btnAgregarActa.Text = "Actualizar";
                        this.btnAgregarActa.CssClass = "btnWizardGuardar";
                        this.btnLimpiarActa.Text = "Cancelar";
                    }
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
                if (this.presentador.AgregarActaConstitutiva()) {
                    this.ucDatosActaConstitutiva.LimpiarCampos();
                    this.btnAgregarActa.Text = "Agregar acta";
                    this.btnAgregarActa.CssClass = "btnAgregarATabla";
                    this.btnLimpiarActa.Text = "Limpiar";
                }
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
                this.btnAgregarActa.Text = "Agregar acta";
                this.btnAgregarActa.CssClass = "btnAgregarATabla";
                this.btnLimpiarActa.Text = "Limpiar";
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al ejecutar la operación.", ETipoMensajeIU.ERROR, nombreClase + ".btnLimpiarActa_Click: " + ex.Message);
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
        /// Evento del botón para agregar teléfonos del TextBox de Teléfonos al Grid correspondiente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMas_Click(object sender, EventArgs e) {
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            TelefonoClienteBO telefono = new TelefonoClienteBO();
            if (txtTelefonos.Text != String.Empty) {
                DataTable TableTemp = DtTelefonos;
                List<TelefonoClienteBO> listaTelefonos = ListaTelefonos;
                telefono.Telefono = txtTelefonos.Text;
                telefono.Activo = true;
                txtTelefonos.Text = "";
                if (listaTelefonos.Contains(telefono) == false) listaTelefonos.Add(telefono);
                this.ListaTelefonos = listaTelefonos;
            }
            EstablecerAcciones(empresa, true);
        }
        /// <summary>
        /// Evento de Grid de teléfonos para eliminar un Row que sea seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdvTelefonos_OnRowDeleting(object sender, GridViewDeleteEventArgs e) {
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativa.Id;
            List<TelefonoClienteBO> listaTelefonos = ListaTelefonos;
            int index = (grdvTelefonos.PageIndex * grdvTelefonos.PageSize);
            index += Convert.ToInt32(e.RowIndex);

            DataTable dt = DtTelefonos;
            DataRow row = dt.Rows[index];
            int? TelefonoId = (int)row["TelefonoId"];
            string Telefono = (string)row["Telefono"];

            dt.Rows.Remove(row);
            ViewState["Telefono"] = dt;
            this.grdvTelefonos.DataSource = dt;
            this.grdvTelefonos.DataBind();
            if (TelefonoId != null) {
                TelefonoClienteBO telefonoBO = ListaTelefonos.Find(x => ((x.TelefonoId == null && TelefonoId == 0) || (x.TelefonoId == TelefonoId)) && x.Telefono == Telefono);
                if (telefonoBO != null) {
                    if (telefonoBO.TelefonoId != null && telefonoBO.TelefonoId > 0)
                        telefonoBO.Activo = false;
                    else
                        ListaTelefonos.Remove(telefonoBO);
                }

            }
            EstablecerAcciones(empresa, true);
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
        /// <summary>
        /// Evento para la paginación del grid de teléfonos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdvTelefonos_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                DataTable dt = DtTelefonos;
                grdvTelefonos.DataSource = dt;
                grdvTelefonos.PageIndex = e.NewPageIndex;
                grdvTelefonos.DataBind();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdvTelefonos_PageIndexChanging: " + ex.Message);
            }
        }
    }
}