using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ucConfiguracionDescuentoPSLUI : System.Web.UI.UserControl, IucConfiguracionDescuentoPSLVIS {
        #region Atributos

        private string nombreClase = "ucConfiguracionDescuentoPSLUI";

        private ucConfiguracionDescuentoPSLPRE presentador;

        public enum ECatalogoBuscador {
            Propietario,
            CuentaClienteIdealease,
            Sucursal
        }

        public DataRow datarow;

        #endregion

        #region Propiedades
        #region Descuento
        /// <summary>
        /// Obtiene o Agrega el ID de la unidad operativa
        /// </summary>
        public int? UnidadOperativaId {
            get {
                int? id = null;
                Site masterMsj = (Site)this.Parent.Page.Master;

                if (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }
        /// <summary>
        /// Obtiene o Agrega el usuario autenticado
        /// </summary>
        public int? UsuarioAutenticado {
            get {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id.Value;
                return null;
            }
        }
        /// <summary>
        /// Obtiene o Agrega el nombre del cliente
        /// </summary>
        public string Cliente {
            get {
                return (String.IsNullOrEmpty(this.txtCliente.Text)) ? null : this.txtCliente.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtCliente.Text = value;
                else
                    this.txtCliente.Text = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o Agrega el ID del cliente
        /// </summary>
        public int? ClienteId {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnClienteID.Value))
                    id = int.Parse(this.hdnClienteID.Value.Trim());
                return id;
            }
            set {
                if (value != null)
                    this.hdnClienteID.Value = value.ToString();
                else
                    this.hdnClienteID.Value = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o Agrega el  nombre de la sucursal
        /// </summary>
        public string SucursalNombre {
            get {
                return (String.IsNullOrEmpty(this.txtSucursal.Text)) ? null : this.txtSucursal.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtSucursal.Text = value;
                else
                    this.txtSucursal.Text = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o Agrega el ID de la sucursal
        /// </summary>
        public int? SucursalId {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnSucursalID.Value))
                    id = int.Parse(this.hdnSucursalID.Value.Trim());
                return id;
            }
            set {
                if (value != null)
                    this.hdnSucursalID.Value = value.ToString();
                else
                    this.hdnSucursalID.Value = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o Agrega el nombre del contacto comercial
        /// </summary>
        public string ContactoComercial {
            get {
                return (String.IsNullOrEmpty(this.txtContactoComercial.Text)) ? null : this.txtContactoComercial.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtContactoComercial.Text = value;
                else
                    this.txtContactoComercial.Text = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o Agrega la fecha de inicio
        /// </summary>
        public DateTime? FechaInicio {
            get {
                if (!string.IsNullOrEmpty(this.txtFechaInicio.Text) && !string.IsNullOrWhiteSpace(this.txtFechaInicio.Text)) {
                    DateTime date;
                    return DateTime.TryParse(this.txtFechaInicio.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtFechaInicio.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }

        }
        /// <summary>
        /// Obtiene o Agrega la fecha de fin
        /// </summary>
        public DateTime? FechaFin {
            get {
                if (!string.IsNullOrEmpty(this.txtFechaFin.Text) && !string.IsNullOrWhiteSpace(this.txtFechaFin.Text)) {
                    DateTime date;
                    return DateTime.TryParse(this.txtFechaFin.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtFechaFin.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }

        }
        /// <summary>
        /// Obtiene o Agrega el descuento máximos
        /// </summary>
        public decimal? DescuentoMaximo {
            get {
                decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtMaximoDescuento.Text))
                    temp = decimal.Parse(this.txtMaximoDescuento.Text.Trim().Replace(",", ""));
                return temp;
            }
            set {
                if (value != null)
                    this.txtMaximoDescuento.Text = string.Format("{0:#,##0.00}", value);
                else
                    this.txtMaximoDescuento.Text = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o Agrega el estado del chbx
        /// </summary>
        public bool? Estado {
            get {
                bool val;
                return Boolean.TryParse(this.chbxActivo.Checked.ToString(), out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxActivo.Checked = true;
                    else
                        chbxActivo.Checked = false;

                }
            }
        }
        /// <summary>
        /// Usuario del Sistema
        /// </summary>
        public UsuarioBO Usuario {
            get {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario;
                return null;
            }
        }
        /// <summary>
        /// Unidad Operativa de Configurada
        /// </summary>
        public UnidadOperativaBO UnidadOperativa {
            get {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    return new UnidadOperativaBO { Id = masterMsj.Adscripcion.UnidadOperativa.Id };
                return null;
            }
        }
        #endregion
        /// <summary>
        /// Obtiene o Agrega el ultimo objeto seleccionado por el usuario
        /// </summary>
        public ConfiguracionDescuentoBO UltimoObjeto {
            get {
                if (Session["UltimoObjetoConfiguracionDescuento"] != null)
                    return (ConfiguracionDescuentoBO)Session["UltimoObjetoConfiguracionDescuento"];

                return null;
            }
            set {
                Session["UltimoObjetoConfiguracionDescuento"] = value;
            }
        }
        /// <summary>
        /// Tabla de descuentos
        /// </summary>
        public DataTable DtDescuentos {
            get {
                if (Session["Descuentos"] == null) {
                    DataTable DTTemp = new DataTable();
                    DTTemp.TableName = "Descuentos";
                    DTTemp.Columns.Add("Cliente", typeof(System.String));
                    DTTemp.Columns.Add("ContactoComercial", typeof(System.String));
                    DTTemp.Columns.Add("Sucursal", typeof(System.String));
                    DTTemp.Columns.Add("DescuentoMaximo", typeof(System.String));
                    DTTemp.Columns.Add("FechaInicio", typeof(System.String));
                    DTTemp.Columns.Add("FechaFin", typeof(System.String));
                    DTTemp.Columns.Add("Estado", typeof(System.String));

                    return DTTemp;
                } else
                    return (DataTable)Session["Descuentos"];
            }
            set {
                Session["Descuentos"] = value;
            }
        }
        /// <summary>
        /// Obtiene o Agrega la lista de descuentos
        /// </summary>
        public List<ConfiguracionDescuentoBO> ListaDescuentos {
            get {
                if (Session["ListaDescuentos"] == null)
                    return new List<ConfiguracionDescuentoBO>();
                else
                    return (List<ConfiguracionDescuentoBO>)Session["ListaDescuentos"];
            }
            set {
                Session["ListaDescuentos"] = value;
            }
        }
        /// <summary>
        /// Obtiene o Agrega la lista de detalle
        /// </summary>
        public List<ConfiguracionDescuentoBO> ListaDetalle {
            get {
                if (Session["listDetalleConfiguracionDescuentoBO"] == null)
                    return new List<ConfiguracionDescuentoBO>();
                else
                    return (List<ConfiguracionDescuentoBO>)Session["listDetalleConfiguracionDescuentoBO"];
            }
            set {
                Session["listDetalleConfiguracionDescuentoBO"] = value;
            }

        }
        /// <summary>
        /// Obtiene o Agrega el índice de la pagina del resultado
        /// </summary>
        public int IndicePaginaResultado {
            get { return this.grvDescuentos.PageIndex; }
            set { this.grvDescuentos.PageIndex = value; }
        }
        /// <summary>
        /// Obtiene o Agrega la lista de sucursales
        /// </summary>
        public List<SucursalBO> ListaSucursales {

            get {
                if (Session["ListaSucursales"] == null)
                    return new List<SucursalBO>();
                else
                    return (List<SucursalBO>)Session["ListaSucursales"];
            }
            set {
                Session["ListaSucursales"] = value;
            }

        }
        /// <summary>
        /// Obtiene o estable la lista de sucursales a las que el usuario autenticado tiene permiso de acceder
        /// </summary>
        public List<SucursalBO> SucursalesAutorizadas {
            get {
                if (Session["SucursalesAutRO"] != null)
                    return Session["SucursalesAutRO"] as List<SucursalBO>;
                else {
                    var lstRet = new List<SucursalBO>();
                    return lstRet.ConvertAll(x => (SucursalBO)x);
                }
            }

            set { Session["SucursalesAutRO"] = value; }
        }

        #region Propiedades buscador
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

        #region Constructor
        protected void Page_Load(object sender, EventArgs e) {

            if (this.hdnAccion.Value != null) {
                this.presentador = new ucConfiguracionDescuentoPSLPRE(this, this.hdnAccion.Value);
            } else {
                this.presentador = new ucConfiguracionDescuentoPSLPRE(this);
            }
            if (this.hdnAccion.Value.ToUpper().Equals("EDITAR"))
                this.DeshabilitarBotonesNoUtilizadosParaEditar();
            if (this.hdnAccion.Value.ToUpper().Equals("AGREGAR"))
                this.btnCancelar.Visible = true;
            if (!IsPostBack) {
                this.Inicializarfechas();
            }


        }
        #endregion

        #region Métodos

        public void PrepararNuevo() {
            this.txtCliente.Text = "";
            this.chbxTodasSucursales.Checked = false;
            this.txtContactoComercial.Text = "";

            this.txtSucursal.Text = "";
            this.txtMaximoDescuento.Text = "";
            this.chbxActivo.Checked = true;
        }

        /// <summary>
        /// habilita o deshabilita el botón para editar.
        /// </summary>
        /// <param name="habilitar">true para habilitar/False para deshabilitar</param>
        public void HabilitarModoEdicion(bool habilitar) {
            this.txtCliente.Enabled = habilitar;

            this.chbxTodasSucursales.Enabled = habilitar;

            this.txtContactoComercial.Enabled = habilitar;

            this.txtSucursal.Enabled = habilitar;

            //this.txtFechaVigencia.Enabled = habilitar;

            this.txtMaximoDescuento.Enabled = habilitar;

            this.chbxActivo.Enabled = habilitar;
        }

        /// <summary>
        /// Habilita el check "Todas las sucursales".
        /// </summary>
        /// <param name="habilitar">true para habilitar/False para deshabilitar</param>
        public void HabilitaCheckSucursales(bool habilitar) {
            this.chbxTodasSucursales.Enabled = habilitar;
        }

        /// <summary>
        /// Desplegar mensaje de Error con detalle
        /// </summary>
        /// <param name="mensaje">Descripción del mensaje</param>
        /// <param name="tipo">EMensaje tipo de mensaje a desplegar</param>
        /// <param name="detalle">Detalle del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null) {
            if (tipo.Equals(ETipoMensajeIU.CONFIRMACION)) {
                ((HiddenField)this.Parent.FindControl("hdnTipoMensaje")).Value = ((int)tipo).ToString();
                ((HiddenField)this.Parent.FindControl("hdnMensaje")).Value = mensaje;
            } else {
                Site masterMsj = (Site)this.Parent.Page.Master;
                if (tipo == ETipoMensajeIU.ERROR)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
                else
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// Habilita el botón para agregar.
        /// </summary>
        /// <param name="visible"></param>
        public void MostrarBotonAgregar(bool visible) {
            this.btnAgregar.Visible = visible;
        }

        /// <summary>
        /// Habilita el botón para actualizar
        /// </summary>
        /// <param name="habilitar">True en caso de habilitarlo ó false si no.</param>
        public void HabilitaBotonActualizar(bool habilitar) {
            this.btnActualizar.Enabled = habilitar;
        }

        /// <summary>
        /// Muestra el botón para cancelar
        /// </summary>
        /// <param name="visible"> true para habilitar/False para deshabilitar</param>
        public void MostrarBotonCancelar(bool visible) {
            this.btnCancelar.Visible = visible;
        }

        /// <summary>
        /// Habilita el campo cliente.
        /// </summary>
        /// <param name="habilitar">true para habilitar/False para deshabilitar</param>
        public void HabilitarCliente(bool habilitar) {
            this.txtCliente.Enabled = habilitar;
        }

        /// <summary>
        /// Habilita el campo Contacto Comercial.
        /// </summary>
        /// <param name="habilitar">true para habilitar/False para deshabilitar</param>
        public void HabilitarContactoComercial(bool habilitar) {
            this.txtContactoComercial.Enabled = habilitar;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="Estado"></param>
        public void DesHabilitarSucursal(bool Estado) {


            if (Estado) {


            } else {



            }


        }

        #region Métodos para el buscador
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


        public void Inicializarfechas() {
            DateTime FechaActual = DateTime.Today;
            this.txtFechaInicio.Text = string.Format(FechaActual.ToShortDateString(), "dd/mm/aaaa");
            this.txtFechaFin.Text = string.Format(FechaActual.ToShortDateString(), "dd/mm/aaaa");
        }
        #endregion

        #region Eventos

        protected void btnResult_Click(object sender, EventArgs e) {
            try {
                switch (ViewState_Catalogo) {
                    case ECatalogoBuscador.CuentaClienteIdealease:
                    case ECatalogoBuscador.Sucursal:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click:" + ex.Message);
            }
        }

        protected void ibtnBuscaCliente_Click(object sender, ImageClickEventArgs e) {
            string s;
            if ((s = this.presentador.ValidarCampoConsultarCliente()) != null) {
                this.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try {
                this.EjecutaBuscador("CuentaClienteIdealease&hidden=0", ECatalogoBuscador.CuentaClienteIdealease);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscaCliente_Click:" + ex.Message);
            }
        }

        protected void txtCliente_TextChanged(object sender, EventArgs e) {
            try {
                string cliente = this.Cliente;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                this.Cliente = cliente;
                if (this.Cliente != null) {
                    this.EjecutaBuscador("CuentaClienteIdealease", ECatalogoBuscador.CuentaClienteIdealease);
                    this.Cliente = null;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtCliente_TextChanged:" + ex.Message);
            }
        }

        protected void txtSucursal_TextChanged(object sender, EventArgs e) {
            try {
                string sucursalNombre = this.SucursalNombre;
                int? unidadOperativaId = this.UnidadOperativaId;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                this.SucursalNombre = sucursalNombre;
                if (this.UnidadOperativaId != null && this.SucursalNombre != null) {
                    this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                    this.SucursalNombre = null;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtSucursal_TextChanged:" + ex.Message);
            }
        }

        protected void ibtnBuscaSucursal_Click(object sender, ImageClickEventArgs e) {
            try {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscaSucursal_Click:" + ex.Message);
            }
        }

        #endregion

        #region Eventos de botones y checkbox
        /// <summary>
        /// Evento que controla el estado del checkBox todas las sucursales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chbxTodasSucursales_CheckedChanged1(object sender, EventArgs e) {

            if (this.chbxTodasSucursales.Checked) {
                this.txtSucursal.Enabled = false;
                this.txtSucursal.Text = null;
                this.ibtnBuscaSucursal.Enabled = false;
                this.MostrarGridSucursalesPorEventoChecked();
                this.grvDescuentos.Columns[5].Visible = false;

            } else {
                switch (this.hdnAccion.Value.ToUpper()) {

                    case "EDITAR":

                        this.grvDescuentos.Columns[5].Visible = true;
                        this.presentador.limpiarCampos(this.hdnAccion.Value);

                        break;

                    case "AGREGAR":
                        this.txtSucursal.Enabled = true;
                        this.txtSucursal.Text = null;
                        this.ibtnBuscaSucursal.Enabled = true;
                        this.grvDescuentos.Columns[5].Visible = true;
                        this.HabilitarBotonesParaRegistrar(true);

                        break;

                    default:

                        break;

                }
                //this.presentador.limpiarCampos(this.hdnAccion.Value);
                this.HabilitarBotonesParaEditar(false);
                this.chbxTodasSucursales.Checked = false;
            }
        }

        /// <summary>
        /// Evento para generar el grid en el apartado de información del descuento.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregar_Click(object sender, EventArgs e) {
            try {
                string s = null;

                //Verifica si se encuentra activado el check de todas las sucursales
                if (this.chbxTodasSucursales.Checked) {
                    if ((s = this.presentador.ValidarCampos(true)) != null) {
                        this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                        return;
                    }

                    if ((s = this.presentador.ValidarFechas()) != null) {
                        this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                        return;
                    }
                } else {
                    if ((s = this.presentador.ValidarCampos(false)) != null) {
                        this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                        return;
                    }
                    if ((s = this.presentador.ValidarFechas()) != null) {
                        this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                        return;
                    }
                }

                VeriricarDato();

            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al preparar los datos de la unidad a agregar al contrato.", ETipoMensajeIU.ERROR, nombreClase + ".AgregarUnidad_Click: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento para el botón de aceptar correspondiente al modal. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarModal_Click(object sender, EventArgs e) {
            List<ConfiguracionDescuentoBO> lstTemporal = this.presentador.ObtenerListaClonada();

            this.chbxTodasSucursales.Checked = true;
            switch (this.hdnAccion.Value.ToUpper()) {

                case "EDITAR":
                    this.grvDescuentos.Columns[5].Visible = false;
                    this.HabilitarBotonesParaEditar(true);
                    this.presentador.limpiarCampos(this.hdnAccion.Value);
                    break;

                case "AGREGAR":
                    Session.Remove("ListaDescuentos");
                    Session.Remove("Descuentos");
                    Session.Remove("ListaSucursales");
                    this.txtSucursal.Enabled = false;
                    this.grvDescuentos.Columns[5].Visible = false;
                    this.HabilitarBotonesParaEditar(false);
                    this.HabilitarBotonesParaRegistrar(true);

                    break;


                default:

                    break;

            }
            this.CrearTabla(lstTemporal);

        }

        /// <summary>
        /// Evento del botón actualizar el cual actualiza los datos del grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActualizar_Click(object sender, EventArgs e) {

            int index = this.hdnIndex.Value.Trim() != "" ? int.Parse(this.hdnIndex.Value) : -1;
            this.presentador.Actualizar(this.chbxTodasSucursales.Checked, index, this.hdnAccion.Value);

        }

        /// <summary>
        /// Evento cancelar para limpiar Campos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e) {

            //Limpiar Campos para editar

            #region LimpiarCampos

            if (this.hdnAccion.Value.Equals("EDITAR")) {
                this.presentador.limpiarCampos(this.hdnAccion.Value);
            }

            if (this.hdnAccion.Value.Equals("AGREGAR")) {
                if (this.EsRegistro() && this.EsCancelar()) {
                    this.txtCliente.Text = null;
                    this.txtSucursal.Text = null;
                    this.txtContactoComercial.Text = null;
                    this.txtMaximoDescuento.Text = null;
                    Inicializarfechas();
                } else {
                    this.txtSucursal.Text = null;
                    this.txtMaximoDescuento.Text = null;
                    this.Inicializarfechas();
                }
            }
            #endregion

            #region ValidarMostrarBotones
            // Si el check se encuentra activado y el botón agregar no se encuentra visible, se mostrará el botón Editar
            if (this.chbxTodasSucursales.Checked) {
                if (this.hdnAccion.Value.Equals("AGREGAR")) {
                    if (this.btnAgregar.Visible == false) {
                        this.HabilitarBotonesParaEditar(true);
                    }
                } else {
                    this.HabilitarBotonesParaEditar(true);
                }
            } else {
                this.HabilitarBotonesParaEditar(false);

                if (this.hdnAccion.Value.Equals("AGREGAR")) {
                    this.btnAgregar.Visible = true;
                }
            }

            #endregion


        }

        /// <summary>
        /// Cierra el modal y desactiva el checkbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarModal_Click(object sender, EventArgs e) {
            List<ConfiguracionDescuentoBO> lstTemporal = this.presentador.ObtenerListaClonada();
            switch (this.hdnAccion.Value.ToUpper()) {

                case "EDITAR":

                    this.grvDescuentos.Columns[5].Visible = true;

                    break;

                case "AGREGAR":
                    this.txtSucursal.Enabled = true;
                    this.txtSucursal.Text = null;
                    this.grvDescuentos.Columns[5].Visible = true;
                    this.HabilitarBotonesParaRegistrar(true);

                    break;


                default:

                    break;

            }
            this.presentador.limpiarCampos(this.hdnAccion.Value);
            this.HabilitarBotonesParaEditar(false);
            this.chbxTodasSucursales.Checked = false;
            this.CrearTabla(lstTemporal);
        }
        #endregion

        #region Eventos Tabla
        /// <summary>
        /// Evento del grid cuando se le asigna un nuevo valor de índice.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvDescuentos_SelectedIndexChanged(object sender, EventArgs e) {

        }

        /// <summary>
        /// Evento del grid que controla la información que recibe el grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvDescuentos_RowCommand(object sender, GridViewCommandEventArgs e) {
            int index;

            if (e.CommandName.ToString().ToUpper() == "PAGE") return;

            try {
                index = Convert.ToInt32(e.CommandArgument.ToString()) + (((GridView)sender).PageIndex * ((GridView)sender).PageSize);
                switch (e.CommandName.Trim()) {
                    case "Editar":
                        this.muestraInformacion(index);
                        break;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al ejecutar la acción sobre el acta de nacimiento", ETipoMensajeIU.ERROR, this.nombreClase + ".grvActasNacimiento_RowCommand:" + ex.Message);
            }
        }


        protected void grvDescuentos_RowDataBound(object sender, GridViewRowEventArgs e) {

        }
        protected void grvDescuentos_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                this.grvDescuentos.DataSource = this.DtDescuentos;
                grvDescuentos.PageIndex = e.NewPageIndex;
                grvDescuentos.DataBind();
                this.InsertaValoresCheckboxTabla(ListaDescuentos);
                this.chbxActivo.Checked = true;
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grvDescuentos_PageIndexChanging: " + ex.Message);
            }
        }
        #endregion


        #region Métodos para Editar
        /// <summary>
        /// Método para la creación de la tabla en la acción de editar.
        /// </summary>
        /// <param name="lstDescuentos">Lista de descuentos</param>
        public void CrearTabla(List<ConfiguracionDescuentoBO> lstDescuentos) {
            //Si se actualiza una fila en el módulo de registrar, se elimina los datos del grid para agregarle los datos con la nueva fila.
            if (this.hdnAccion.Value.Equals("AGREGAR")) {
                Session.Remove("Descuentos");
                Session.Remove("ListaDescuentos");
            }
            if (this.hdnAccion.Value.Equals("EDITAR")) {
                Session.Remove("Descuentos");
            }

            //this.ListaDescuentos.Clear();
            DataTable TableTemp = DtDescuentos;
            List<ConfiguracionDescuentoBO> lstTemporal = new List<ConfiguracionDescuentoBO>();

            // Clonamos la lista para que no se registren los cambios en la variable de sesión
            foreach (var descuento in lstDescuentos) {

                lstTemporal.Add(descuento.Clone());

            }

            foreach (ConfiguracionDescuentoBO descuento in lstTemporal) {
                datarow = TableTemp.NewRow();
                datarow["Cliente"] = descuento.Cliente.Nombre;
                datarow["ContactoComercial"] = descuento.ContactoComercial;
                datarow["Sucursal"] = descuento.Sucursal.Nombre;

                var desc = Math.Round((decimal)descuento.DescuentoMaximo, 2);

                datarow["DescuentoMaximo"] = desc;

                DateTime FechaInicio = Convert.ToDateTime(descuento.FechaInicio);
                DateTime FechaFin = Convert.ToDateTime(descuento.FechaFin);

                datarow["FechaInicio"] = string.Format(FechaInicio.ToShortDateString(), "dd/mm/aaaa");
                datarow["FechaFin"] = string.Format(FechaFin.ToShortDateString(), "dd/mm/aaaa");
                datarow["Estado"] = descuento.Estado;

                TableTemp.Rows.Add(datarow);
            }


            this.grvDescuentos.DataSource = TableTemp;
            this.grvDescuentos.DataBind();
            this.InsertaValoresCheckboxTabla(lstTemporal);
            this.ListaDescuentos = lstDescuentos;

            if (this.hdnAccion.Value.Equals("AGREGAR")) {
                DtDescuentos = TableTemp;
            }

        }
        /// <summary>
        /// Inserta los valores(true o false) en los checkbox de la tabla
        /// </summary>
        /// <param name="lstDescuentos">Lista de descuentos</param>
        public void InsertaValoresCheckboxTabla(List<ConfiguracionDescuentoBO> lstDescuentos) {
            int aux = 0;
            foreach (GridViewRow row in this.grvDescuentos.Rows) {
                CheckBox CheckBoxActivo = row.FindControl("ChkActivo") as CheckBox;
                if (CheckBoxActivo != null) {
                    if (lstDescuentos[aux].Estado != false) {
                        CheckBoxActivo.Checked = true;
                    } else {
                        CheckBoxActivo.Checked = false;
                    }

                }

                aux++;
            }

        }
        /// <summary>
        /// Habilita o Deshabilita Campos
        /// </summary>
        /// <param name="habilitar">booleano para habilitar o deshabilitar</param>
        public void HabilitarCamposEditar(bool habilitar) {
            this.txtCliente.Enabled = habilitar;

        }
        /// <summary>
        /// Habilita o deshabilita los botones para editar dependiendo del parámetro
        /// </summary>
        /// <param name="habilitar">booleano que habilita o deshabilita los botones</param>
        public void HabilitarBotonesParaEditar(bool habilitar) {
            if (this.hdnAccion.Value.Equals("EDITAR")) {
                this.btnCancelar.Visible = habilitar;
            }

            this.btnActualizar.Visible = habilitar;

            if (this.hdnAccion.Value.Equals("AGREGAR")) {
                this.btnAgregar.Visible = true;
            }
        }
        /// <summary>
        /// Deshabilita todos los botones o componentes que no son utilizados por la vista
        /// </summary>
        public void DeshabilitarBotonesNoUtilizadosParaEditar() {
            this.txtSucursal.Enabled = false;
            this.btnCancelar.Visible = false;
            this.btnAgregar.Visible = false;

            ibtnBuscaCliente.Visible = false;
            ibtnBuscaSucursal.Visible = false;

        }
        /// <summary>
        /// Agarra información de un registro del grid de la tabla y la muestra en la vista
        /// </summary>
        /// <param name="index">valor que será recibido del evento de la tabla</param>
        public void muestraInformacion(int index) {
            if (index < 0)
                throw new Exception("No se encontró el la configuración de descuentos.");

            ConfiguracionDescuentoBO bo = this.ListaDescuentos[index];

            if (hdnAccion.Value.ToUpper().Equals("EDITAR")) {
                this.SucursalNombre = bo.Sucursal.Nombre;
                this.FechaInicio = bo.FechaInicio;
                this.FechaFin = bo.FechaFin;
                this.DescuentoMaximo = bo.DescuentoMaximo;
                this.Estado = bo.Estado;
                this.UltimoObjeto = bo;
                this.hdnIndex.Value = index.ToString();
                this.HabilitarBotonesParaEditar(true);
            } else {
                this.SucursalNombre = bo.Sucursal.Nombre;
                this.FechaInicio = bo.FechaInicio;
                this.FechaFin = bo.FechaFin;
                this.DescuentoMaximo = bo.DescuentoMaximo;
                this.Estado = bo.Estado;
                this.UltimoObjeto = bo;
                this.hdnIndex.Value = index.ToString();
                this.HabilitarBotonesParaEditar(true);
                this.HabilitarBotonesParaRegistrar(false);

                if (hdnAccion.Value.ToUpper().Equals("REGISTRAR")) {
                    this.HabilitarBotonesParaRegistrar(true);
                }

            }

        }

        public void EstablecerDatosNavegacion(object configuracionDescuento) {
            Session["listConfiguracionPSLBO"] = configuracionDescuento;
        }


        #endregion

        #region Metodos Registrar

        /// <summary>
        /// Desplegar mensaje de Error con detalle
        /// </summary>
        /// <param name="mensaje">Descripción del mensaje</param>
        /// <param name="tipo">EMensaje tipo de mensaje a desplegar</param>
        /// <param name="detalle">Detalle del mensaje a desplegar</param>
        public void MostrarMensajeRegistro(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null) {
            if (tipo.Equals(ETipoMensajeIU.CONFIRMACION)) {
                this.hdnTipoMensaje.Value = ((int)tipo).ToString();
                this.hdnMensaje.Value = mensaje;

                #region SC_0027
                string botonID = this.GuardarEnGrid.UniqueID;

                this.RegistrarScript("Confirm", "abrirConfirmacion('" + mensaje + "');");

                #endregion
            } else {
                Site masterMsj = (Site)Page.Master;
                if (tipo == ETipoMensajeIU.ERROR)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
                else
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// Método para pintar le grid con base a los datos capturados en el sistema
        /// </summary>
        public void LlenarGrid() {

            bool check = false;

            ConfiguracionDescuentoBO Descuento = new ConfiguracionDescuentoBO();


            Descuento.Cliente = new CuentaClienteIdealeaseBO();

            Descuento.Sucursal = new SucursalBO();

            Descuento.UnidadOperativa = new UnidadOperativaBO();


            #region llenado del grid

            DataTable TableTemp = DtDescuentos;

            List<ConfiguracionDescuentoBO> listaDescuentos = ListaDescuentos;

            datarow = TableTemp.NewRow();

            if (this.chbxActivo.Checked) {
                check = true;
            }

            Descuento.Cliente.Nombre = txtCliente.Text;
            Descuento.ContactoComercial = txtContactoComercial.Text;
            Descuento.sucursal.Nombre = txtSucursal.Text;
            Descuento.DescuentoMaximo = Convert.ToDecimal(txtMaximoDescuento.Text);
            Descuento.FechaInicio = Convert.ToDateTime(txtFechaInicio.Text);
            Descuento.FechaFin = Convert.ToDateTime(txtFechaFin.Text);
            Descuento.Sucursal.Id = this.SucursalId;
            Descuento.Cliente.Id = this.ClienteId;
            Descuento.Estado = check;
            Descuento.UnidadOperativa.Id = this.UnidadOperativaId;

            var existe = listaDescuentos.FindAll(x => x.Cliente.Id == Descuento.Cliente.Id && x.sucursal.Id == Descuento.sucursal.Id);

            if (existe == null || existe.Count <= 0) {

                datarow["Cliente"] = txtCliente.Text;
                datarow["ContactoComercial"] = txtContactoComercial.Text;
                datarow["Sucursal"] = txtSucursal.Text;
                datarow["DescuentoMaximo"] = string.Format("{0:#,##0.00}", txtMaximoDescuento.Text);
                datarow["FechaInicio"] = txtFechaInicio.Text;
                datarow["FechaFin"] = txtFechaFin.Text;

                if (chbxTodasSucursales.Checked) {
                    check = true;
                }

                datarow["Estado"] = check;

                TableTemp.Rows.Add(datarow);
                this.grvDescuentos.DataSource = TableTemp;
                this.grvDescuentos.DataBind();

                //this.txtCliente.Text = "";
                //this.txtContactoComercial.Text = "";
                this.txtSucursal.Text = "";
                this.txtMaximoDescuento.Text = "";
                Inicializarfechas();

                DtDescuentos = TableTemp;


                listaDescuentos.Add(Descuento);

                this.ListaDescuentos = listaDescuentos;

                this.InsertaValoresCheckboxTabla(ListaDescuentos);

                this.chbxActivo.Checked = true;

            } else {
                this.MostrarMensajeRegistro("Ya existe configurado un descuento para este cliente", ETipoMensajeIU.INFORMACION, null);

                //this.txtCliente.Text = "";
                //this.txtContactoComercial.Text = "";
                this.txtSucursal.Text = "";
                this.txtMaximoDescuento.Text = "";
                Inicializarfechas();
                this.chbxActivo.Checked = true;
            }
            #endregion

        }

        /// <summary>
        /// Verifica que los datos del descuento no se encuentren registrados en la BD.
        /// </summary>
        public void VeriricarDato() {
            if (this.chbxTodasSucursales.Checked) {
                this.presentador.ValidarDescuentosSucursales();
            } else {
                if (this.presentador.ConsultarDescuentoUsuario().Count > 0) {
                    this.MostrarMensajeRegistro("Ya existe configurado un descuento para este cliente", ETipoMensajeIU.INFORMACION, null);
                } else {
                    LlenarGrid();
                }
            }
        }

        /// <summary>
        /// Llena las filas del grid con base a todas los sucursales que el usuario tiene acceso.
        /// </summary>
        /// <param name="lstSucursales"></param>
        public void llenaGridPorSucursal(List<SucursalBO> lstSucursales) {

            bool check = false;


            DataTable TableTemp = DtDescuentos;

            List<ConfiguracionDescuentoBO> listaSucursales = ListaDescuentos;

            if (this.chbxActivo.Checked) {
                check = true;
            }

            #region ForEach

            foreach (SucursalBO Sucursal in lstSucursales) {

                ConfiguracionDescuentoBO Descuento = new ConfiguracionDescuentoBO();

                Descuento.Cliente = new CuentaClienteIdealeaseBO();

                Descuento.Sucursal = new SucursalBO();

                Descuento.UnidadOperativa = new UnidadOperativaBO();

                Descuento.Cliente.Nombre = txtCliente.Text;
                Descuento.ContactoComercial = txtContactoComercial.Text;
                Descuento.sucursal.Nombre = Sucursal.Nombre;
                Descuento.DescuentoMaximo = Convert.ToDecimal(txtMaximoDescuento.Text);
                Descuento.FechaInicio = Convert.ToDateTime(txtFechaInicio.Text);
                Descuento.FechaFin = Convert.ToDateTime(txtFechaFin.Text);
                Descuento.Sucursal.Id = Sucursal.Id;
                Descuento.Cliente.Id = this.ClienteId;
                Descuento.Estado = check;
                Descuento.UnidadOperativa.Id = this.UnidadOperativaId;

                var existe = listaSucursales.FindAll(x => x.Cliente.Id == Descuento.Cliente.Id && x.sucursal.Id == Descuento.sucursal.Id);

                if (existe == null || existe.Count <= 0) {
                    listaSucursales.Remove(Descuento);
                }

                datarow = TableTemp.NewRow();
                datarow["Cliente"] = txtCliente.Text;
                datarow["ContactoComercial"] = txtContactoComercial.Text;
                datarow["Sucursal"] = Sucursal.Nombre;
                datarow["DescuentoMaximo"] = string.Format("{0:#,##0.00}", txtMaximoDescuento.Text);
                datarow["FechaInicio"] = txtFechaInicio.Text;
                datarow["FechaFin"] = txtFechaFin.Text;

                if (this.chbxActivo.Checked) {
                    datarow["Estado"] = true;
                } else {
                    datarow["Estado"] = false;
                }

                listaSucursales.Add(Descuento);

                this.ListaDescuentos = listaSucursales;


                TableTemp.Rows.Add(datarow);

                this.grvDescuentos.DataSource = TableTemp;
                this.grvDescuentos.DataBind();

                this.InsertaValoresCheckboxTabla(ListaDescuentos);

                DtDescuentos = TableTemp;

            }

            #endregion

            Inicializarfechas();
            this.txtMaximoDescuento.Text = null;
            this.chbxActivo.Checked = true;
        }

        /// <summary>
        /// Muestra un modal cuando el grid contenga campos y se active el check de todas las sucursales.
        /// </summary>
        public void MostrarGridSucursalesPorEventoChecked() {
            if (this.grvDescuentos.Rows.Count > 0) {
                string msg = "Al activar ésta casilla todos los registros de la matriz de descuento serán actualizados";
                this.MostrarMensajeRegistro(msg, ETipoMensajeIU.CONFIRMACION, null);

            }
        }

        /// <summary>
        /// Valida que todos los campos obligatorios no se encuentren vacíos.
        /// </summary>
        /// <returns></returns>
        public string validarCampos() {
            string s = "";

            if (!(this.txtCliente.Text != null || this.txtCliente.Text != ""))
                s += "Cliente, ";

            if (!(this.txtContactoComercial.Text != null || this.txtContactoComercial.Text != ""))
                s += "ContactoComercial, ";

            if (!(this.txtSucursal.Text != null || this.txtSucursal.Text != ""))
                s += "Sucursal, ";

            if (!(this.txtMaximoDescuento.Text != null || this.txtMaximoDescuento.Text != ""))
                s += "Descuento, ";

            if (!(this.txtFechaInicio != null || this.txtFechaInicio.Text != ""))
                s += "Fecha Inicio, ";

            if (!(this.txtFechaFin.Text != null || this.txtFechaFin.Text != ""))
                s += "Fecha Fin, ";


            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        /// <summary>
        /// Habilita el boton registrar.
        /// </summary>
        /// <param name="habilitar"></param>
        public void HabilitarBotonesParaRegistrar(bool habilitar) {
            this.btnAgregar.Visible = habilitar;

        }

        /// <summary>
        /// Verifica si el grid contiene datos en sus filas.
        /// </summary>
        /// <returns></returns>
        public bool ExisteDatosEnGrid() {
            if (this.grvDescuentos.Rows.Count != 0) {
                return true;
            } else {
                return false;
            }
        }
        #endregion

        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        /// <summary>
        /// Método para saber si el grid va a Editar o Agregar
        /// </summary>
        /// <param name="Accion">Recibe EDITAR o AGREGAR </param>
        public void IngresarAccion(String Accion) {
            this.hdnAccion.Value = Accion;
        }
        /// <summary>
        /// Limpia las sesiones utilizadas
        /// </summary>
        public void LimpiarSesiones() {
            if (Session["ListaDescuentos"] != null)
                Session.Remove("ListaDescuentos");
            if (Session["listDetalleConfiguracionDescuentoBO"] != null)
                Session.Remove("listDetalleConfiguracionDescuentoBO");
            if (Session["UltimoObjetoConfiguracionDescuento"] != null)
                Session.Remove("UltimoObjetoConfiguracionDescuento");
            if (Session["FilasSeleccionadas"] != null)
                Session.Remove("FilasSeleccionadas");
        }

        /// <summary>
        /// Indica si se encuentra activo el botón para registrar
        /// </summary>
        /// <returns></returns>
        public bool EsRegistro() {
            bool esRegistro = false;

            if (this.btnAgregar.Visible) {
                esRegistro = true;
            }

            return esRegistro;

        }

        /// <summary>
        /// Verifica si el botón para actualizar se encuentra activo
        /// </summary>
        /// <returns></returns>
        public bool EsActualizarEnRegistro() {
            bool esActualizarRegistro = false;

            if (this.btnActualizar.Visible) {
                esActualizarRegistro = true;
            }

            return esActualizarRegistro;
        }

        public bool EsCancelar() {
            bool esCancelar = false;

            if (this.btnCancelar.Visible) {
                esCancelar = true;
            }

            return esCancelar;
        }

        /// <summary>
        /// Verifica el estado del checkBox todas las sucursales.
        /// </summary>
        /// <returns></returns>
        public bool TodasSucursales() {
            if (this.chbxTodasSucursales.Checked) {
                return true;
            }
            return false;
        }
    }
}