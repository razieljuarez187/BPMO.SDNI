//Satiface al caso de uso CU068 - Catáloglo de Clientes
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
    public partial class DetalleCuentaClienteUI : System.Web.UI.Page, IDetalleCuentaClienteVIS {
        #region Atributos

        private string nombreClase = "DetalleCuentaClienteUI";
        private DetalleCuentaClientePRE presentador;
        private int? id = 0;
        public List<CatalogoBaseBO> lstAcciones;
        public List<TelefonoClienteBO> lstTelefonos;
        public int sector;
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
            get { return (String.IsNullOrEmpty(this.txtObservaciones.Text.Trim())) ? null : this.txtObservaciones.Text.Trim().ToUpper(); }
            set { this.txtObservaciones.Text = value != null ? value : string.Empty; }
        }

        public int? ClienteID {
            get {
                TextBox txtClienteID = mCuentaCliente.Controls[0].FindControl("txtValue") as TextBox;
                if (String.IsNullOrEmpty(txtClienteID.Text.Trim()) == false)
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
            get { return (String.IsNullOrEmpty(this.txtCURP.Text)) ? null : this.txtCURP.Text; }
            set { this.txtCURP.Text = value != null ? value : string.Empty; }
        }

        /// <summary>
        /// Numero de cuenta de Oracle - CuentaClienteBO.Numero
        /// </summary>
        public string NumeroCuentaOracle {
            get { return String.IsNullOrEmpty(this.txtNumeroCuentaOracle.Text.Trim()) ? null : this.txtNumeroCuentaOracle.Text.Trim(); }
            set { this.txtNumeroCuentaOracle.Text = value ?? String.Empty; }
        }

        public DateTime? FechaEscritura {
            get {
                DateTime? fechaEscritura = null;
                if (!String.IsNullOrEmpty(this.txtFechaEscritura.Text.Trim()))
                    fechaEscritura = DateTime.Parse(this.txtFechaEscritura.Text.Trim());

                return fechaEscritura;
            }
            set { this.txtFechaEscritura.Text = value != null ? value.Value.ToString("dd/MM/yyyy") : string.Empty; }
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

        public DateTime? FechaRPPC {
            get {
                DateTime? fechaRPPC = null;
                if (!String.IsNullOrEmpty(this.txtFechaRPPC.Text.Trim()))
                    fechaRPPC = DateTime.Parse(this.txtFechaRPPC.Text.Trim());

                return fechaRPPC;
            }
            set { this.txtFechaRPPC.Text = value != null ? value.Value.ToString("dd/MM/yyyy") : string.Empty; }
        }

        public bool? Fisica {
            get {
                if (!String.IsNullOrEmpty(this.txtTipoContribuyente.Text)) {
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

        /// <summary>
        /// Sector al que pertenece al cliente
        /// </summary>
        public Enum SectorCliente {
            get {
                if (this.UnidadOperativaId == (int)ETipoEmpresa.Generacion) {
                    if (this.ddlSector.SelectedIndex > 0)
                        return (ESectorGeneracion)Enum.Parse(typeof(ESectorGeneracion), this.ddlSector.SelectedValue);
                }
                if (this.UnidadOperativaId == (int)ETipoEmpresa.Construccion) {
                    if (this.ddlSector.SelectedIndex > 0)
                        return (ESectorConstruccion)Enum.Parse(typeof(ESectorConstruccion), this.ddlSector.SelectedValue);
                }
                if (this.UnidadOperativaId == (int)ETipoEmpresa.Equinova) {
                    if (this.ddlSector.SelectedIndex > 0)
                        return (ESectorEquinova)Enum.Parse(typeof(ESectorEquinova), this.ddlSector.SelectedValue);
                }
                return null;
            }
            set {
                if (this.UnidadOperativaId == (int)ETipoEmpresa.Generacion) {
                    if (value == null)
                        this.ddlSector.SelectedIndex = 0;
                    else
                        this.ddlSector.SelectedValue = ((int)(ESectorGeneracion)value).ToString();
                }
                if (this.UnidadOperativaId == (int)ETipoEmpresa.Construccion) {
                    if (value == null)
                        this.ddlSector.SelectedIndex = 0;
                    else
                        this.ddlSector.SelectedValue = ((int)(ESectorConstruccion)value).ToString();
                }
                if (this.UnidadOperativaId == (int)ETipoEmpresa.Equinova) {
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
                Session["ListaTelefonos"] = value;
            }
        }

        public string GiroEmpresa {
            get { return (String.IsNullOrEmpty(this.txtGiroEmpresa.Text)) ? null : this.txtGiroEmpresa.Text; }
            set { this.txtGiroEmpresa.Text = value != null ? value : string.Empty; }
        }

        public string LocalidadNotaria {
            get { return (String.IsNullOrEmpty(this.txtLocalidadNotaria.Text)) ? null : this.txtLocalidadNotaria.Text; }
            set { this.txtLocalidadNotaria.Text = value != null ? value : string.Empty; }
        }

        public string LocalidadRPPC {
            get { return (String.IsNullOrEmpty(this.txtLocalidadRPPC.Text)) ? null : this.txtLocalidadRPPC.Text; }
            set { this.txtLocalidadRPPC.Text = value != null ? value : string.Empty; }
        }

        public string Nombre {
            get { return (String.IsNullOrEmpty(this.txtNombre.Text)) ? null : this.txtNombre.Text; }
            set { this.txtNombre.Text = value != null ? value : string.Empty; }
        }

        public string NombreNotario {
            get { return (String.IsNullOrEmpty(this.txtNombreNotario.Text)) ? null : this.txtNombreNotario.Text; }
            set { this.txtNombreNotario.Text = value != null ? value : string.Empty; }
        }

        public string NumeroEscritura {
            get { return (String.IsNullOrEmpty(this.txtNumeroEscritura.Text)) ? null : this.txtNumeroEscritura.Text; }
            set { this.txtNumeroEscritura.Text = value != null ? value : string.Empty; }
        }

        public string NumeroFolio {
            get { return (String.IsNullOrEmpty(this.txtNumeroFolio.Text)) ? null : this.txtNumeroFolio.Text; }
            set { this.txtNumeroFolio.Text = value != null ? value : string.Empty; }
        }

        public string NumeroNotaria {
            get { return (String.IsNullOrEmpty(this.txtNumeroNotaria.Text)) ? null : this.txtNumeroNotaria.Text; }
            set { this.txtNumeroNotaria.Text = value != null ? value : string.Empty; }
        }

        public void EstablecerPaquete(object bo) {
            Session["DatosCuentaClienteIdealeaseBO"] = bo;
        }


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

        public DateTime? FC {
            get { return DateTime.Today; }
        }

        public DateTime? FUA {
            get { return this.FC; }
        }

        public List<ObligadoSolidarioBO> Obligados {
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

        public List<RepresentanteLegalBO> Representantes {
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

        public List<RepresentanteLegalBO> RepresentantesObligados {
            get {
                if (Session["RepresentantesObligadosSol"] == null) return new List<RepresentanteLegalBO>();
                return Session["RepresentantesObligadosSol"] as List<RepresentanteLegalBO>;
            }
            set { Session["RepresentantesObligadosSol"] = value; }
        }

        public string RFC {
            get { return (String.IsNullOrEmpty(this.txtRFC.Text)) ? null : this.txtRFC.Text; }
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

        #region SC_0008
        public int? UnidadOperativaId {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }
        public int? UsuarioId {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }
        #endregion

        #region SC0001

        public int? DiasUsoUnidad {
            get {
                int? dias = null;
                if (txtDiasUsoUnidad.Text != string.Empty)
                    dias = int.Parse(this.txtDiasUsoUnidad.Text);
                return dias;
            }
            set {
                if (value != null)
                    this.txtDiasUsoUnidad.Text = value.ToString();
                else
                    this.txtDiasUsoUnidad.Text = string.Empty;

            }
        }

        public int? HorasUsoUnidad {
            get {
                int? horas = null;
                if (txtHorasUsoUnidad.Text != string.Empty)
                    horas = int.Parse(this.txtHorasUsoUnidad.Text);
                return horas;

            }
            set {
                if (value != null)
                    this.txtHorasUsoUnidad.Text = value.ToString();
                else
                    this.txtHorasUsoUnidad.Text = string.Empty;

            }
        }

        public string Correo {
            get { return this.txtCorreo.Text; }
            set { this.txtCorreo.Text = value; }
        }
        #endregion

        #endregion Propiedades

        #region Constructores

        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new DetalleCuentaClientePRE(this, this.ucDatosRepresentanteLegal);

                if (!IsPostBack) {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso(); //SC_0008

                    this.MostrarTipoCuenta();
                    this.presentador.Inicializar();
                    this.AgregarAtributoObservaciones();
                    this.OcultarEscrituraRepresentante();
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, this.nombreClase + ex.Message);
            }
        }

        #endregion Constructores

        #region Métodos

        /// <summary>
        /// Metodo sobrecargado para mostrar los campos segun sea el caso para el tipo de unidad Operativa.
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
            if (empresa != ETipoEmpresa.Idealease) {
                txtDiasUsoUnidad.Visible = false;
                grdvTelefonos.Visible = true;
                if (!habilitar) txtDiasUsoUnidad.Enabled = false;
                else txtDiasUsoUnidad.Enabled = true;
            }

            if (empresa == ETipoEmpresa.Construccion) {
                this.divObservaciones.Visible = true;
            } else if (empresa == ETipoEmpresa.Generacion) {
                this.divObservaciones.Visible = true;
            } else if (empresa == ETipoEmpresa.Idealease) {
                this.divObservaciones.Visible = false;
            } else if (empresa == ETipoEmpresa.Equinova) {
                this.divObservaciones.Visible = true;
            }

            if (empresa != ETipoEmpresa.Idealease) {
                this.grdvTelefonos.Visible = true;
                ddlSector.Visible = true;
                if (empresa == ETipoEmpresa.Construccion) {
                    if (ddlSector.Items.Count <= 0) {
                        Type type = typeof(ESectorConstruccion);
                        Array values = Enum.GetValues(typeof(ESectorConstruccion));
                        ListItem item = new ListItem { Enabled = true, Selected = true, Text = "SELECCIONE UNA OPCIÓN", Value = "0" };
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
                    this.grdvTelefonos.Visible = false;
                    if (ddlSector.Items.Count <= 0) {
                        Type type = typeof(ESectorGeneracion);
                        Array values = Enum.GetValues(typeof(ESectorGeneracion));
                        ListItem item = new ListItem { Enabled = true, Selected = true, Text = "SELECCIONE UNA OPCIÓN", Value = "0" };
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
                    this.grdvTelefonos.Visible = false;
                    if (ddlSector.Items.Count <= 0) {
                        Type type = typeof(ESectorEquinova);
                        Array values = Enum.GetValues(typeof(ESectorEquinova));
                        ListItem item = new ListItem { Enabled = true, Selected = true, Text = "SELECCIONE UNA OPCIÓN", Value = "0" };
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
                ddlSector.Visible = false;
                if (!habilitar) ddlSector.Enabled = false;
                else ddlSector.Enabled = true;
            }

            if (empresa == ETipoEmpresa.Idealease)
                txtHorasUsoUnidad.Visible = true;
            else
                txtHorasUsoUnidad.Visible = false;

            if (empresa == ETipoEmpresa.Idealease) {
                this.trDiasUso.Visible = true;
                this.trSector.Visible = false;
                txtTelefonos.Visible = false;
                btnMas.Visible = false;
                grdvTelefonos.Visible = false;

            } else {
                this.trDiasUso.Visible = false;
                this.trSector.Visible = true;

                txtTelefonos.Visible = true;
                btnMas.Visible = true;
                btnMas.Enabled = false;
                grdvTelefonos.Visible = true;
                grdvTelefonos.Enabled = false;
                grdvTelefonos.Columns[1].Visible = false;
            }

            if (empresa != ETipoEmpresa.Idealease) {
                this.lblSector.InnerText = etiqueta;
                this.lblTipoCuenta.InnerText = etiqueta2;
                this.txtHorasUsoUnidad.Text = etiqueta3;
            }

        }

        /// <summary>
        /// Método EstablecerAcciones mediante la lista de acciones, permitiendo el acceso o no a los campos de la ventana.
        /// </summary>
        public void EstablecerAcciones() {
            bool finaliza = false;
            ucDatosRepresentanteLegal.UnidadOperatiaId = (int)UnidadOperativaId;
            ucDatosRepresentanteLegal.ListaAcciones = this.ListaAcciones;
            ucDatosRepresentanteLegal.HabilitarCampos(false);
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativaId;
            if (lstAcciones != null) {
                foreach (CatalogoBaseBO acciones in lstAcciones) {
                    if (finaliza == true) break;
                    switch (acciones.Nombre.ToUpper()) {
                        case "ACTUALIZARCOMPLETO":
                            EstablecerAcciones(empresa, false);
                            finaliza = true;
                            break;
                    }
                }
            } else EstablecerAcciones(empresa, false);
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
        /// Método encargado de mostrar el panel con el campo de observaciones de la solución.
        /// </summary>
        public void MostrarObservaciones() {
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativaId;

            if (empresa != ETipoEmpresa.Idealease) {
                divObservaciones.Visible = true;
            } else {
                divObservaciones.Visible = false;
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

        public void DeshabilitarCampos() {
            this.txtFechaEscritura.Enabled = false;
            this.txtFechaRegistro.Enabled = false;
            this.txtFechaRPPC.Enabled = false;
            this.txtGiroEmpresa.Enabled = false;
            this.txtLocalidadNotaria.Enabled = false;
            this.txtLocalidadRPPC.Enabled = false;
            this.txtNombreNotario.Enabled = false;
            this.txtNumeroEscritura.Enabled = false;
            this.txtNumeroFolio.Enabled = false;
            this.txtNumeroNotaria.Enabled = false;
            this.txtCURP.Enabled = false;
            #region SC0001
            this.txtCorreo.Enabled = false;
            this.txtDiasUsoUnidad.Enabled = false;
            this.txtHorasUsoUnidad.Enabled = false;
            #endregion
            this.txtNumeroCuentaOracle.Enabled = false;
            this.ddlTipoCuenta.Enabled = false;
        }

        public void OcultarEscrituraRepresentante() {
            ucDatosRepresentanteLegal.ValidarEscrituraRepresentante(this.UnidadOperativaId);
        }

        public void EstablecerDatos(CuentaClienteIdealeaseBO bo) {
            Session["DatosCuentaClienteIdealeaseBO"] = bo;
        }

        public void EstablecerDatosNavegacion(string nombre, object bo) {
            Session[nombre] = bo;
        }

        public void LimpiarSesion() {
            if (Session["ListaRepresentantesLegales"] != null)
                Session.Remove("ListaRepresentantesLegales");
            if (Session["ListaObligadosSolidarios"] != null)
                Session.Remove("ListaObligadosSolidarios");
            if (Session["DatosCuentaClienteIdealeaseBO"] != null)
                Session.Remove("DatosCuentaClienteIdealeaseBO");
        }

        public void MostrarActaConstitutiva() {
            this.pnlActaConstitutiva.Visible = true;
            divRepresentantesLegales.Visible = true;
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativaId;
            switch (empresa) {
                case ETipoEmpresa.Generacion:
                    txtFechaEscritura.Enabled = false;
                    txtNumeroEscritura.Enabled = false;

                    txtFechaRPPC.Visible = false;
                    txtLocalidadNotaria.Visible = false;
                    txtLocalidadRPPC.Visible = false;
                    txtNombreNotario.Visible = false;
                    txtNumeroFolio.Visible = false;
                    txtNumeroNotaria.Visible = false;
                    this.trSeccion1.Visible = false;
                    this.trSeccion2.Visible = false;
                    this.trSeccion3.Visible = false;
                    break;
                case ETipoEmpresa.Equinova:
                    txtFechaEscritura.Enabled = false;
                    txtNumeroEscritura.Enabled = false;

                    txtFechaRPPC.Visible = false;
                    txtLocalidadNotaria.Visible = false;
                    txtLocalidadRPPC.Visible = false;
                    txtNombreNotario.Visible = false;
                    txtNumeroFolio.Visible = false;
                    txtNumeroNotaria.Visible = false;
                    this.trSeccion1.Visible = false;
                    this.trSeccion2.Visible = false;
                    this.trSeccion3.Visible = false;
                    break;
                case ETipoEmpresa.Construccion:
                    txtFechaEscritura.Enabled = false;
                    txtNumeroEscritura.Enabled = false;

                    txtFechaRPPC.Visible = false;
                    txtLocalidadNotaria.Visible = false;
                    txtLocalidadRPPC.Visible = false;
                    txtNombreNotario.Visible = false;
                    txtNumeroFolio.Visible = false;
                    txtNumeroNotaria.Visible = false;
                    this.trSeccion1.Visible = false;
                    this.trSeccion2.Visible = false;
                    this.trSeccion3.Visible = false;
                    break;
                case ETipoEmpresa.Idealease:
                    txtFechaEscritura.Enabled = false;
                    txtNumeroEscritura.Enabled = false;
                    txtFechaRPPC.Enabled = false;
                    txtLocalidadNotaria.Enabled = false;
                    txtLocalidadRPPC.Enabled = false;
                    txtNumeroFolio.Enabled = false;
                    txtNumeroNotaria.Enabled = false;
                    break;
            }
        }

        public void MostrarDatos() {
            this.grdObligadosSolidarios.DataSource = this.Obligados;
            this.grdObligadosSolidarios.DataBind();
            this.grdRepresentantesLegales.DataSource = this.Representantes;
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativaId;

            if (empresa != ETipoEmpresa.Idealease) {
                this.grdRepresentantesLegales.Columns[3].Visible = false;
                this.grdvTelefonos.DataSource = this.ListaTelefonos;
                this.grdvTelefonos.DataBind();
            }
            this.grdRepresentantesLegales.DataBind();
        }

        public void MostrarDetalleObligado(List<RepresentanteLegalBO> representantes) {
            grdRepresentantesObligados.DataSource = representantes;
            if (this.UnidadOperativaId != (int)ETipoEmpresa.Idealease)
                grdRepresentantesObligados.Columns[2].Visible = false;
            grdRepresentantesObligados.DataBind();
        }

        public void MostrarHacienda() {
            this.pnlRegistroHacienda.Visible = true;
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
        /// <summary>
        /// Muestra en la Interfaz la lista de Creditos Disponibles del Cliente
        /// </summary>
        /// <param name="creditos">Lista de Creditos del cliente</param>
        public void MostrarCreditoCliente(List<CreditoClienteBO> creditos) {
            if (creditos == null) creditos = new List<CreditoClienteBO>();

            this.rptCreditoCliente.DataSource = creditos;
            this.rptCreditoCliente.DataBind();

            this.lblNoHayCredito.Visible = !creditos.Any();
        }

        public object ObtenerDatos() {
            return (CuentaClienteIdealeaseBO)Session["DatosCuentaClienteIdealeaseBO"];
        }

        public void OcultarActaConstitutiva() {
            this.pnlActaConstitutiva.Visible = false;
            divRepresentantesLegales.Visible = false;
        }

        public void OcultarHacienda() {
            this.pnlRegistroHacienda.Visible = false;
        }

        public void RedirigirAEdicion() {
            this.Response.Redirect(this.ResolveUrl("~/Comun.UI/EditarCuentaClienteUI.aspx"));
        }

        #region SC0008
        public void PermitirEditar(bool permitir) {
            this.mCuentaCliente.Items[1].Enabled = permitir;
            this.btnEditar.Enabled = permitir;
        }
        public void PermitirRegistrar(bool permitir) {
            this.hlRegistroOrden.Enabled = permitir;
        }
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        #endregion

        #endregion Métodos

        #region Eventos

        protected void btnEditar_Click(object sender, EventArgs e) {
            try {
                this.presentador.Editar();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cambiar a edición", ETipoMensajeIU.ERROR, this.nombreClase + ".btnEditar_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// Dispara el cambio de paginacion del grid de obligados solidarios del cliente
        /// </summary>
        /// <param name="sender">objeto que dispara el evento</param>
        /// <param name="e">parametros del evento</param>
        protected void grdObligadosSolidarios_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                EstablecerAcciones((ETipoEmpresa)this.UnidadOperativaId, true);
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
                int index = Convert.ToInt32(e.CommandArgument);
                ObligadoSolidarioBO obligado = this.Obligados[index];
                presentador.MostrarDetalleObligado(obligado);
                RepresentantesObligados = ((ObligadoSolidarioMoralBO)obligado).Representantes;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), null, "DialogoDetalleObligado();", true);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".gvRepresentantesLegales_RowCommand: " + ex.Message);
            }
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
                this.MostrarMensaje("Inconsistencia al desplegar los Obligados Solidarios", ETipoMensajeIU.ERROR, this.nombreClase + ".grdObligadosSolidarios_RowDataBound" + ex.Message);
            }
        }

        /// <summary>
        /// Dispara el cambio de paginacion del grid de representantes legales del cliente
        /// </summary>
        /// <param name="sender">objeto que dispara el evento</param>
        /// <param name="e">parametros del evento</param>
        protected void grdRepresentantesLegales_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                grdRepresentantesLegales.DataSource = this.Representantes;
                grdRepresentantesLegales.PageIndex = e.NewPageIndex;
                grdRepresentantesLegales.DataBind();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdRepresentantesLegales_PageIndexChanging: " + ex.Message);
            }
        }

        /// <summary>
        /// Determina ver detalle de un representente legal
        /// </summary>
        /// <param name="sender">objeto que dispara el evento</param>
        /// <param name="e">parametros del evento</param>
        protected void grdRepresentantesLegales_RowCommand(object sender, GridViewCommandEventArgs e) {
            try {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                int index = Convert.ToInt32(e.CommandArgument);
                RepresentanteLegalBO representante = this.Representantes[index];
                presentador.MostrarDetalleRepresentante(representante);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "grdRepresentantesLegales_RowCommand_" + DateTime.Now.Ticks.ToString(), "Dialog();", true);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".gvRepresentantesLegales_RowCommand: " + ex.Message);
            }
        }

        protected void grdRepresentantesObligados_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            grdRepresentantesObligados.DataSource = RepresentantesObligados;
            grdRepresentantesObligados.PageIndex = e.NewPageIndex;
            grdRepresentantesObligados.DataBind();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), null, "DialogoDetalleObligado();", true);
        }

        protected void mCuentaCliente_MenuItemClick(object sender, MenuEventArgs e) {
            switch (e.Item.Value) {
                case "Editar":
                    this.presentador.Editar();
                    break;
            }
        }

        protected void rptCreditoCliente_ItemDataBound(object sender, RepeaterItemEventArgs e) {
            try {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) {
                    var creditoCliente = (CreditoClienteBO)e.Item.DataItem;

                    var lblHeader = e.Item.FindControl("lblNombreMoneda") as Label;
                    var txtCodigoMoneda = e.Item.FindControl("txtMoneda") as TextBox;
                    var txtDiasFactura = e.Item.FindControl("txtDiasFactura") as TextBox;
                    var txtDiasCredito = e.Item.FindControl("txtDiasCredito") as TextBox;
                    var txtLimiteCredito = e.Item.FindControl("txtLimiteCredito") as TextBox;
                    var txtCreditoDisponible = e.Item.FindControl("txtCreditoDisponible") as TextBox;

                    lblHeader.Text = lblHeader.Text.Replace("[MONEDA]", creditoCliente.Moneda != null ? !String.IsNullOrEmpty(creditoCliente.Moneda.Nombre) ?
                        creditoCliente.Moneda.Nombre : String.Empty : String.Empty);
                    txtCodigoMoneda.Text = creditoCliente.Moneda != null ? !String.IsNullOrEmpty(creditoCliente.Moneda.Codigo) ?
                        creditoCliente.Moneda.Codigo : String.Empty : String.Empty;
                    txtDiasFactura.Text = creditoCliente.DiasFactura != null ? creditoCliente.DiasFactura.ToString() : String.Empty;
                    txtDiasCredito.Text = creditoCliente.DiasCredito != null ? creditoCliente.DiasCredito.ToString() : String.Empty;
                    txtLimiteCredito.Text = creditoCliente.LimiteCredito != null ? creditoCliente.LimiteCredito.ToString() : String.Empty;
                    txtCreditoDisponible.Text = creditoCliente.LimiteCredito != null && creditoCliente.Saldo != null ? (creditoCliente.LimiteCredito - creditoCliente.Saldo).ToString() : creditoCliente.LimiteCredito != null ? creditoCliente.LimiteCredito.ToString() : "0";

                    txtCodigoMoneda.Enabled = txtDiasFactura.Enabled = txtDiasCredito.Enabled = txtLimiteCredito.Enabled = txtCreditoDisponible.Enabled = false;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al desplegar El Crédito del Cliente", ETipoMensajeIU.ERROR, this.nombreClase + ".rptCreditoCliente_ItemDataBound" + ex.Message);
            }
        }

        #endregion Eventos

        protected void btnMas_Click(object sender, EventArgs e) {
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativaId;
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
        /// Evento de Grid de teléfonos para eliminar un Row que sea seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdvTelefonos_OnRowDeleting(object sender, GridViewDeleteEventArgs e) {
            int index = Convert.ToInt32(e.RowIndex);
            DataTable TableTemp = DtTelefonos;
            List<TelefonoClienteBO> listaTelefonos = ListaTelefonos;
            datarow = TableTemp.NewRow();
            TableTemp.Rows.Add(datarow);
            grdvTelefonos.DataSource = TableTemp;
            grdvTelefonos.DataBind();
            DtTelefonos = TableTemp;
        }

        /// <summary>
        /// Evento que controla la selección del DropDownList de Sector.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSector_SelectedIndexChanged(object sender, EventArgs e) {
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativaId;
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

        protected void btnActualizar_Click(object sender, EventArgs e) {
            if (this.hdnBtnActualizar.Value == "false") {
                this.hdnBtnActualizar.Value = "true";
                int? ert = 0;
                TextBox txtClienteID = mCuentaCliente.Controls[0].FindControl("txtValue") as TextBox;
                if (String.IsNullOrEmpty(txtClienteID.Text.Trim()))
                    ert = Convert.ToInt32(txtClienteID.Text.Trim());
                this.presentador.ActualizarClienteOracle();
                this.hdnBtnActualizar.Value = "false";
            }
        }

        /// <summary>
        /// Aplica atributo al campo Observaciones
        /// </summary>
        public void AgregarAtributoObservaciones() {
            txtObservaciones.Attributes.Add("maxlength", txtObservaciones.MaxLength.ToString());
        }

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