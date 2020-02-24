//Satisface CU009 – Consultar Tablero de Seguimiento Unidades Renta Diaria
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Flota.BOF;
using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.Flota.PRE;
using BPMO.SDNI.Flota.BO;
using BPMO.SDNI.Contratos.RD.BOF;
using System.Web;

namespace BPMO.SDNI.Flota.UI{

    public partial class ConsultarTableroRDUI : System.Web.UI.Page, IConsultarTableroRDVIS{

        #region Atributos
        private ConsultarTableroRDPRE presentador = null;
        private const string nombreClase = "ConsultarTableroRDUI";
        private bool activarDetallesUnidad = true;
        private bool activarDetallesContrato = true;

        public enum ECatalogoBuscador{
            Marca = 0,
            Modelo = 1,
            Sucursal = 2,
            CuentaClienteIdealease = 3
        }
        #endregion

        #region Propiedades
        #region Propiedades para el Buscador
        
        public string ViewState_Guid{
            get{
                if (ViewState["GuidSession"] == null){
                    Guid guid = Guid.NewGuid();
                    ViewState["GuidSession"] = guid.ToString();
                }
                return ViewState["GuidSession"].ToString();
            }
        }
        
        protected object Session_BOSelecto{
            get{
                object objeto = null;
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (Session[nombreSession] != null)
                    objeto = (Session[nombreSession] as object);

                return objeto;
            }
            set{
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (value != null)
                    Session[nombreSession] = value;
                else
                    Session.Remove(nombreSession);
            }
        }
        
        protected object Session_ObjetoBuscador{
            get{
                object objeto = null;
                if (Session[ViewState_Guid] != null)
                    objeto = (Session[ViewState_Guid] as object);

                return objeto;
            }
            set{
                if (value != null)
                    Session[ViewState_Guid] = value;
                else
                    Session.Remove(ViewState_Guid);
            }
        }
        
        public ECatalogoBuscador ViewState_Catalogo{
            get{
                return (ECatalogoBuscador)ViewState["BUSQUEDA"];
            }
            set{
                ViewState["BUSQUEDA"] = value;
            }
        }
        #endregion

        public int? UnidadOperativaID{
            get{
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    return masterMsj.Adscripcion.UnidadOperativa.Id.Value;
                return null;
            }            
        }

        public string NumeroEconomico {
            get {
                if (!string.IsNullOrEmpty(this.txtNumeroEconomico.Text) && !string.IsNullOrWhiteSpace(this.txtNumeroEconomico.Text))
                    return this.txtNumeroEconomico.Text.Trim().ToUpper();
                return null;
            }
            set {
                if (value != null)
                    this.txtNumeroEconomico.Text = value;
                else
                    this.txtNumeroEconomico.Text = string.Empty;
            }
        }

        public string MarcaNombre {
            get {
                if (!string.IsNullOrEmpty(this.txtMarca.Text) && !string.IsNullOrWhiteSpace(this.txtMarca.Text))
                    return this.txtMarca.Text.Trim().ToUpper();
                return null;
            }
            set {
                if (value != null)
                    this.txtMarca.Text = value;
                else
                    this.txtMarca.Text = string.Empty;
            }
        }

        public int? MarcaID {
            get {
                int val;
                if (!string.IsNullOrEmpty(this.hdnMarcaID.Value) && !string.IsNullOrWhiteSpace(this.hdnMarcaID.Value))
                    if (Int32.TryParse(this.hdnMarcaID.Value, out val))
                        return val;
                return null;
            }
            set {
                if (value != null)
                    this.hdnMarcaID.Value = value.Value.ToString();
                else
                    this.hdnMarcaID.Value = string.Empty;
            }
        }

        public string ModeloNombre {
            get {
                if (!string.IsNullOrEmpty(this.txtModelo.Text) && !string.IsNullOrWhiteSpace(this.txtModelo.Text))
                    return this.txtModelo.Text.Trim().ToUpper();
                return null;
            }
            set {
                if (value != null)
                    this.txtModelo.Text = value;
                else
                    this.txtModelo.Text = string.Empty;
            }
        }

        public int? ModeloID {
            get {
                int val;
                if (!string.IsNullOrEmpty(this.hdnModeloID.Value) && !string.IsNullOrWhiteSpace(this.hdnModeloID.Value))
                    if (Int32.TryParse(this.hdnModeloID.Value, out val))
                        return val;
                return null;
            }
            set {
                if (value != null)
                    this.hdnModeloID.Value = value.Value.ToString();
                else
                    this.hdnModeloID.Value = string.Empty;
            }
        }

        public string SucursalNombre{
            get {
                if (!string.IsNullOrEmpty(this.txtSucursal.Text) && !string.IsNullOrWhiteSpace(this.txtSucursal.Text))
                    return this.txtSucursal.Text.Trim().ToUpper();
                return null;
            }
            set {
                if (value != null)
                    this.txtSucursal.Text = value;
                else
                    this.txtSucursal.Text = string.Empty;
            }
        }

        public int? SucursalID {
            get {
                int val;
                if (!string.IsNullOrEmpty(this.hdnSucursalID.Value) && !string.IsNullOrWhiteSpace(this.hdnSucursalID.Value))
                    if (Int32.TryParse(this.hdnSucursalID.Value, out val))
                        return val;
                return null;
            }
            set {
                if (value != null)
                    this.hdnSucursalID.Value = value.Value.ToString();
                else
                    this.hdnSucursalID.Value = string.Empty;
            }
        }

        public string CuentaClienteNombre {
            get {
                if (!string.IsNullOrEmpty(this.txtCuentaCliente.Text) && !string.IsNullOrWhiteSpace(this.txtCuentaCliente.Text))
                    return this.txtCuentaCliente.Text.Trim().ToUpper();
                return null;
            }
            set {
                if (value != null)
                    this.txtCuentaCliente.Text = value;
                else
                    this.txtCuentaCliente.Text = string.Empty;
            }
        }

        public int? CuentaClienteID {
            get {
                int val;
                if (!string.IsNullOrEmpty(this.hdnCuentaClienteID.Value) && !string.IsNullOrWhiteSpace(this.hdnCuentaClienteID.Value))
                    if (Int32.TryParse(this.hdnCuentaClienteID.Value, out val))
                        return val;
                return null;
            }
            set {
                if (value != null)
                    this.hdnCuentaClienteID.Value = value.Value.ToString();
                else
                    this.hdnCuentaClienteID.Value = string.Empty;
            }
        }

        public int? EstatusUnidad {
            get {
                int val;
                if (!string.IsNullOrEmpty(this.ddlEstatusUnidad.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlEstatusUnidad.SelectedValue))
                    if (Int32.TryParse(this.ddlEstatusUnidad.SelectedValue, out val))
                        if (val >= 0)
                            return val;
                return null;
            }
            set {
                if (value != null)
                    this.ddlEstatusUnidad.SelectedValue = value.Value.ToString();
                else
                    this.ddlEstatusUnidad.SelectedValue = "-1";
            }
        }

        public bool? EstaEnTaller{
            get{
                int val;
                if (!string.IsNullOrEmpty(this.ddlEstaEnTaller.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlEstaEnTaller.SelectedValue)){
                    if (Int32.TryParse(this.ddlEstaEnTaller.SelectedValue, out val))
                    {
                        switch(val){
                            case 1:
                                return true;
                            case 2:
                                return false;
                            default:
                                return null;
                        }
                    }      
                }
                return null;
            }
            set{
                if (value != null){
                    if (value.Value)
                        this.ddlEstaEnTaller.SelectedValue = "1";
                    else
                        this.ddlEstaEnTaller.SelectedValue = "2";
                }
                else
                    this.ddlEstaEnTaller.SelectedValue = "0";
            }
        }

        public bool? EstaReservada {
            get {
                int val;
                if (!string.IsNullOrEmpty(this.ddlEstaReservada.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlEstaReservada.SelectedValue)) {
                    if (Int32.TryParse(this.ddlEstaReservada.SelectedValue, out val)) {
                        switch (val) {
                            case 1:
                                return true;
                            case 2:
                                return false;
                            default:
                                return null;
                        }
                    }
                }
                return null;
            }
            set {
                if (value != null) {
                    if (value.Value)
                        this.ddlEstaReservada.SelectedValue = "1";
                    else
                        this.ddlEstaReservada.SelectedValue = "2";
                } else
                    this.ddlEstaReservada.SelectedValue = "0";
            }
        }

        public string NumeroContrato {
            get {
                if (!string.IsNullOrEmpty(this.txtNumeroContrato.Text) && !string.IsNullOrWhiteSpace(this.txtNumeroContrato.Text))
                    return this.txtNumeroContrato.Text.Trim().ToUpper();
                return null;
            }
            set {
                if (value != null)
                    this.txtNumeroContrato.Text = value;
                else
                    this.txtNumeroContrato.Text = string.Empty;
            }
        }

        public DateTime? FechaContratoInicial {
            get {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.txtFechaInicial.Text))
                    temp = DateTime.Parse(this.txtFechaInicial.Text.Trim());
                return temp;
            }
            set {
                if (value != null)
                    this.txtFechaInicial.Text = value.ToString();
                else
                    this.txtFechaInicial.Text = string.Empty;
            }
        }

        public DateTime? FechaContratoFinal {
            get {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.txtFechaFinal.Text))
                    temp = DateTime.Parse(this.txtFechaFinal.Text.Trim());
                return temp;
            }
            set {
                if (value != null)
                    this.txtFechaFinal.Text = value.ToString();
                else
                    this.txtFechaFinal.Text = string.Empty;
            }
        }

        public int? UsuarioAutenticado{
            get{
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }

        public List<FlotaRDBOF> UnidadesRD {
            get { 
                return Session["ListUnidadesRD"] != null ? Session["ListUnidadesRD"] as List<FlotaRDBOF> : null; 
            }
            set { 
                Session["ListUnidadesRD"] = value; 
            }
        }

        public int IndicePaginaResultado{
            get { 
                return this.grdTableroRD.PageIndex; 
            }
            set { 
                this.grdTableroRD.PageIndex = value; 
            }
        }
        
        public bool ActivarDetallesUnidad {
            get {
                return activarDetallesUnidad;
            }
            set {
                activarDetallesUnidad = value;
            }
        }
        
        public bool ActivarDetallesContrato {
            get {
                return activarDetallesContrato;
            }
            set {
                activarDetallesContrato = value;
            }
        }
        #endregion        

        #region Constructores
        protected void Page_Load(object sender, EventArgs e){
            try{
                presentador = new ConsultarTableroRDPRE(this);
                if (!IsPostBack)
                    this.presentador.PrepararBusqueda();
            } catch (Exception ex){
                this.MostrarMensaje("Inconsistencia al crear de página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararBusqueda(){
            this.hdnUnidadOperativaID.Value = string.Empty;
            this.txtNumeroEconomico.Text = string.Empty;
            this.txtMarca.Text = string.Empty;
            this.hdnMarcaID.Value = string.Empty;
            this.txtModelo.Text = string.Empty;
            this.hdnModeloID.Value = string.Empty;
            this.txtSucursal.Text = string.Empty;
            this.hdnSucursalID.Value = string.Empty;
            this.txtCuentaCliente.Text = string.Empty;
            this.hdnCuentaClienteID.Value = string.Empty;
            this.ddlEstatusUnidad.SelectedValue = "-1";
            this.ddlEstaEnTaller.SelectedValue = "0";
            this.ddlEstaReservada.SelectedValue = "0";
            this.txtNumeroContrato.Text = string.Empty;
            this.txtFechaInicial.Text = string.Empty;
            this.txtFechaFinal.Text = string.Empty;
        }

        public void LimpiarSesion(){
            if (Session["ListUnidadesRD"] != null)
                Session.Remove("ListUnidadesRD");
        }

        public void ActualizarResultado(){
            this.grdTableroRD.DataSource = this.UnidadesRD;
            this.grdTableroRD.DataBind();
        }

        private void PrepararNuevaBusqueda(){
            this.hdnUnidadOperativaID.Value = string.Empty;
            this.txtNumeroEconomico.Text = string.Empty;
            this.txtMarca.Text = string.Empty;
            this.hdnMarcaID.Value = string.Empty;
            this.txtModelo.Text = string.Empty;
            this.hdnModeloID.Value = string.Empty;
            this.txtSucursal.Text = string.Empty;
            this.hdnSucursalID.Value = string.Empty;
            this.txtCuentaCliente.Text = string.Empty;
            this.hdnCuentaClienteID.Value = string.Empty;
            this.ddlEstatusUnidad.SelectedValue = "-1";
            this.ddlEstaEnTaller.SelectedValue = "0";
            this.ddlEstaReservada.SelectedValue = "0";
            this.txtNumeroContrato.Text = string.Empty;
            this.txtFechaInicial.Text = string.Empty;
            this.txtFechaFinal.Text = string.Empty;
        }

        private static void Redirect(string url, string target, string windowFeatures) {
            HttpContext context = HttpContext.Current;

            if ((String.IsNullOrEmpty(target) ||
                target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
                String.IsNullOrEmpty(windowFeatures)) {

                context.Response.Redirect(url);
            } else {
                Page page = (Page)context.Handler;
                if (page == null) {
                    throw new InvalidOperationException(
                        "La página que desea abrir no se encuentra dentro del contexto. No se puede abrir uan neuva ventana.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures)) {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                } else {
                    script = @"window.open(""{0}"", ""{1}"");";
                }

                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page,
                    typeof(Page),
                    "Redirect",
                    script,
                    true);
            }
        }

        public void RedirigirAReservaciones(){
            Redirect("~/contratos.RD.UI/registrarReservacionRDUI.aspx", "_blank", null);
        }

        public void RedirigirADetallesUnidad() {
            Redirect("~/Flota.UI/DetalleFlotaUI.aspx", "_blank", null);
        }

        public void RedirigirADetallesContrato() {
            Redirect("~/Contratos.RD.UI/DetalleContratoRDUI.aspx", "_blank", null);
        }

        public void EstablecerPaqueteNavegacion(string Nombre, int? UnidadID) {
            FlotaRDBOF elementoBO = this.UnidadesRD.Find(p => p.Unidad.UnidadID.HasValue && p.Unidad.UnidadID.Value == UnidadID);
            if (elementoBO != null)
            switch (Nombre){
               case "RegistrarReservacionUI":
                    Session[Nombre] = elementoBO.Unidad;
                   break;
               case "DetalleFlotaUI":
                    Session[Nombre] = new ElementoFlotaBO{ Unidad = elementoBO.Unidad };
                   break;
               case "ContratoRDBO":
                   Session[Nombre] = new ContratoRDBOF { ContratoID = elementoBO.ContratoID };
                   break;
           } else
                throw new Exception(this.ToString() + ".EstablecerPaqueteNavegacion: La unidad seleccionada no se encuentra dentro del listado de unidades encintradas.");            

        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null){
            Site masterMsj = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
                masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
            else
                masterMsj.MostrarMensaje(mensaje, tipo);
        }

        #region Métodos para el Buscador
        
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="tipoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda){
            ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
        }
       
        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo){
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;
        }

        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script){
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }
        #endregion

        public void RedirigirSinPermisoAcceso(){
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        public void DesactivarReservaciones() {
            this.mnTableroRD.Items[0].Enabled = false;
        }

        /// <summary>
        /// Habilita o deshabilita el campo del modelo segun sea requerido
        /// </summary>
        /// <param name="estado"></param>
        public void HabilitarModelo(bool estado) {
            this.txtModelo.Enabled = estado;
            this.ibtnBuscaModelo.Enabled = estado;
        }
        #endregion

        #region Eventos
        protected void btnBuscar_Click(object sender, EventArgs e){
            try{
                this.presentador.Consultar();
            } catch (Exception ex){
                this.MostrarMensaje("Inconsistencia al consultar Unidades de renta diaria", ETipoMensajeIU.ERROR, nombreClase + ".btnBuscar_Click:" + ex.Message);
            }
        }

        protected void grdTableroRD_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                this.presentador.CambiarPaginaResultado(e.NewPageIndex);
            } catch (Exception ex){
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".grdTableroRD_PageIndexChanging:" + ex.Message);
            }
        }

        protected void grdTableroRD_RowCommand(object sender, GridViewCommandEventArgs e){
            if (e.CommandName.ToString().Trim().ToUpper() == "PAGE") return;
            try{
                int? Identificador = Convert.ToInt32(e.CommandArgument.ToString());
                switch (e.CommandName.ToString().Trim().ToUpper()){
                    case "DETALLESUNIDAD":
                          this.presentador.VerDetallesUnidad(Identificador);
                        break;
                    case "DETALLESCONTRATO":
                        this.presentador.VerDetallesContrato(Identificador);
                        break;
                }
            } catch (Exception ex){
                this.MostrarMensaje("Inconsistencia al ejecutar la acción sobre la unidad", ETipoMensajeIU.ERROR, nombreClase + ".grdTableroRD_RowCommand:" + ex.Message);
            }
        }

        protected void grdTableroRD_RowDataBound(object sender, GridViewRowEventArgs e){
            try{
                if (e.Row.RowType == DataControlRowType.DataRow){
                    FlotaRDBOF bo = (FlotaRDBOF)e.Row.DataItem;


                    Label labelEstatus = e.Row.FindControl("lblEstatus") as Label;
                    if (labelEstatus != null)
                    {
                        string Estatus = string.Empty;
                        if (bo.EstaReservada == true && bo.ContratoID == null)
                            Estatus = "PEDIDA";
                        if (bo.ContratoID != null)
                            Estatus = "RENTADA";
                        if (bo.ContratoID == null && bo.EstaReservada == false)
                            Estatus = "DISPONIBLE";
                        labelEstatus.Text = Estatus;
                    }

                    Label labelNumeroEconomico = e.Row.FindControl("lblNumeroEconomico") as Label;
                    if (labelNumeroEconomico != null) {
                        string numeroEconomico = string.Empty;
                        if (bo.Unidad.NumeroEconomico != null)
                            numeroEconomico = bo.Unidad.NumeroEconomico;
                        labelNumeroEconomico.Text = numeroEconomico;
                    }
                    
                    Label labelModelo = e.Row.FindControl("lblModelo") as Label;
                    if (labelModelo != null) {
                        string modelo = string.Empty;
                        if (bo.Unidad.Modelo != null)
                            modelo = bo.Unidad.Modelo.Nombre;
                        labelModelo.Text = modelo;
                    }

                    Label labelSucursal = e.Row.FindControl("lblSucursal") as Label;
                    if (labelSucursal != null){
                        string sucursal = string.Empty;
                        if (bo.Unidad.Sucursal != null)
                                sucursal = bo.Unidad.Sucursal.Nombre;
                        labelSucursal.Text = sucursal;
                    }

                    Label labelCliente = e.Row.FindControl("lblCliente") as Label;
                    if (labelCliente != null) {
                        string cliente = string.Empty;
                        if (bo.Cliente != null)
                                cliente = bo.Cliente.Nombre;
                        labelCliente.Text = cliente;
                    }
                   
                    Label labelEstaEnTaller = e.Row.FindControl("lblEstaEnTaller") as Label;
                    if (labelEstaEnTaller != null){
                        string estaEnTaller = string.Empty;

                        if (bo.Unidad != null && bo.Unidad.EstatusActual != null)
                        {
                            if (bo.Unidad.EstatusActual == EEstatusUnidad.EnTaller)
                                estaEnTaller = "SI";
                            else
                                estaEnTaller = "NO";
                        } 

                        labelEstaEnTaller.Text = estaEnTaller;
                    }

                    Label labelEstaReservada = e.Row.FindControl("lblEstaReservada") as Label;
                    if (labelEstaReservada != null) {
                        string estaReservada = string.Empty;
                        if (bo.EstaReservada.HasValue)
                            if (bo.EstaReservada.Value)
                                estaReservada = "SI";
                            else
                                estaReservada = "NO";
                        labelEstaReservada.Text = estaReservada;
                    }
                }
            } catch (Exception ex){
                this.MostrarMensaje("Inconsistencia al ejecutar la acción sobre la unidad de renta diaria", ETipoMensajeIU.ERROR, nombreClase + ".grdTableroRD_RowDataBound:" + ex.Message);
            }
        }

        #region Eventos para el Buscador

        protected void txtMarca_TextChanged(object sender, EventArgs e) {
            try {
                this.MarcaID = null;
                string marcaNombre = this.MarcaNombre;
                this.Session_BOSelecto = null;

                if (!string.IsNullOrEmpty(this.txtMarca.Text) && !string.IsNullOrWhiteSpace(this.txtMarca.Text)) {
                    this.DesplegarBOSelecto(ECatalogoBuscador.Marca);
                    
                    this.MarcaNombre = marcaNombre;
                    if (this.MarcaNombre != null) {
                        this.EjecutaBuscador("Marca", ECatalogoBuscador.Marca);
                        this.MarcaNombre = null;
                    }
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una marca", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtMarca_TextChanged" + ex.Message);
            }
        }

        protected void ibtnBuscaMarca_Click(object sender, ImageClickEventArgs e) {
            try {
                this.EjecutaBuscador("Marca&hidden=0", ECatalogoBuscador.Marca);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una Marca", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscaMarca_Click:" + ex.Message);
            }
        }

        protected void txtModelo_TextChanged(object sender, EventArgs e) {
            try {
                this.ModeloID = null;
                int? marcaId = this.MarcaID;
                string modeloNombre = this.ModeloNombre;
                this.Session_BOSelecto = null;

                if (!string.IsNullOrEmpty(this.txtModelo.Text) && !string.IsNullOrWhiteSpace(this.txtModelo.Text)) {
                    this.DesplegarBOSelecto(ECatalogoBuscador.Modelo);

                    this.MarcaID = marcaId;
                    this.ModeloNombre = modeloNombre;
                    if (this.ModeloNombre != null) {
                        this.EjecutaBuscador("Modelo", ECatalogoBuscador.Modelo);
                        this.ModeloNombre = null;
                    }
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar un Modelo", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtModelo_TextChanged:" + ex.Message);
            }
        }
        
        protected void ibtnBuscaModelo_Click(object sender, ImageClickEventArgs e) {
            try {
                this.EjecutaBuscador("Modelo&hidden=0", ECatalogoBuscador.Modelo);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar un Modelo", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscaModelo_Click:" + ex.Message);
            }
        }

        protected void txtSucursal_TextChanged(object sender, EventArgs e){
            try {
                this.Session_BOSelecto = null;
                this.SucursalID = null;

                if (!string.IsNullOrEmpty(this.txtSucursal.Text) && !string.IsNullOrWhiteSpace(this.txtSucursal.Text)) {
                    string sucursalNombre = this.SucursalNombre;

                    this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                    this.SucursalNombre = sucursalNombre;
                    if (this.UnidadOperativaID != null && this.SucursalNombre != null) {
                        this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                        this.SucursalNombre = null;
                    }
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtSucursal_TextChanged:" + ex.Message);
            }
        }

        protected void btnBuscarSucursal_Click(object sender, ImageClickEventArgs e){
            try{
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            } catch (Exception ex){
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarSucursal_Click" + ex.Message);
            }
        }

        protected void txtCuentaCliente_TextChanged(object sender, EventArgs e) {
            try {

                this.Session_BOSelecto = null;
                this.CuentaClienteID = null;

                if (!string.IsNullOrEmpty(this.txtCuentaCliente.Text) && !string.IsNullOrWhiteSpace(this.txtCuentaCliente.Text)) {
                    string cuentaClienteNombre = this.CuentaClienteNombre;

                    DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                    this.CuentaClienteNombre = cuentaClienteNombre;
                    if (this.CuentaClienteNombre != null) {
                        EjecutaBuscador("CuentaClienteIdealease", ECatalogoBuscador.CuentaClienteIdealease);
                        this.CuentaClienteNombre = null;
                    }
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNombreCuentaCliente_TextChanged:" + ex.Message);
            }
        }

        protected void ibtnBuscarCuentaCliente_Click(object sender, ImageClickEventArgs e) {
            try {
                EjecutaBuscador("CuentaClienteIdealease&&hidden=0", ECatalogoBuscador.CuentaClienteIdealease);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarCuentaCliente_Click:" + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e){
            try{
                switch (ViewState_Catalogo){
                    case ECatalogoBuscador.Marca:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                    case ECatalogoBuscador.Modelo:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                    case ECatalogoBuscador.Sucursal:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                    case ECatalogoBuscador.CuentaClienteIdealease:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            } catch (Exception ex){
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click" + ex.Message);
            }
        }
        #endregion     

        protected void mnTableroRD_MenuItemClick(object sender, MenuEventArgs e) {
            switch (e.Item.Value) {
                case "HacerReservacion":
                    RedirigirAReservaciones();
                    break;
            }
        }
        #endregion
    }
}