// Satisface al caso de uso CU004 - Consulta de Pagos a Facturar
// Satisface a la solicitud de cambio SC0035
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.MapaSitio.UI;


namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI
{
    public partial class Pagos : MasterPage, IPagosMasterVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador de la UI
        /// </summary>
        private PagosMasterPRE Presentador;

        private const string NombreClase = "Pagos";
        #endregion

        #region Propiedades
        /// <summary>
        /// Acciona el Metodo de Consulta de las content page
        /// </summary>
        public DelegateAccionConsultar AccionConsultar { get; set; }
        /// <summary>
        /// Departamento Seleccionado
        /// </summary>
        public EDepartamento? DepartamentoSeleccionado
        {
            get
            {
                if (ddlAreaSeleccionada.SelectedValue != "-1")
                    return (EDepartamento)int.Parse(ddlAreaSeleccionada.SelectedValue);

                return null;
            }
        }
        /// <summary>
        /// Fecha Inicio de Vencimiento
        /// </summary>
        public DateTime? FechaVencimientoInicio
        {
            get {
                return string.IsNullOrEmpty(txtFechaInicio.Text.Trim())
                    ? (DateTime?) null
                    : Convert.ToDateTime(txtFechaInicio.Text.Trim());
            }
            set
            {
                txtFechaInicio.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty;
            }
        }
        /// <summary>
        /// Fecha Fin de Vencimiento
        /// </summary>
        public DateTime? FechaVencimientoFin
        {
            get
            {
                return string.IsNullOrEmpty(txtFechaFin.Text.Trim())
                    ? (DateTime?)null
                    : Convert.ToDateTime(txtFechaFin.Text.Trim());
            }
            set
            {
                txtFechaFin.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty;
            }
        }
        /// <summary>
        /// Pagos Consultados
        /// </summary>
        public List<PagoUnidadContratoBO> PagosConsultados
        {
            get
            {
                return (List<PagoUnidadContratoBO>) (Session["PagosConsultados"] ?? new List<PagoUnidadContratoBO>());
            }
            set { Session["PagosConsultados"] = value ?? new List<PagoUnidadContratoBO>(); }
        }
        /// <summary>
        /// Pagos Consultados
        /// </summary>
        public List<PagoContratoPSLBO> PagosConsultadosPSL
        {
            get
            {
                return (List<PagoContratoPSLBO>)(Session["PagosConsultados"] ?? new List<PagoContratoPSLBO>());
            }
            set { Session["PagosConsultados"] = value ?? new List<PagoContratoPSLBO>(); }
        }
        /// <summary>
        /// Identificador de la Sucursal Seleccionada
        /// </summary>
        public int? SucursalSeleccionadaID {
            get {
                int? _result = null;
                if (ddlSucursales.SelectedValue != "0")
                    _result = int.Parse(ddlSucursales.SelectedValue);

                return _result;
            }
        }
        /// <summary>
        /// Sucursales permitidas al usuario
        /// </summary>
        public List<SucursalBO> SucursalesUsuario {
            get {
                if (Session["SucursalesAutorizadas"] != null)
                    return Session["SucursalesAutorizadas"] as List<SucursalBO>;

                return null;
            }
            set {
                if (value != null && value.Count > 0) {
                    Session["SucursalesAutorizadas"] = value;
                } else {
                    Session.Remove("SucursalesAutorizadas");
                }
            }
        }
        /// <summary>
        /// Total de Pagos por Facturar
        /// </summary>
        public int TotalFacturar
        {
            get
            {
                if (string.IsNullOrEmpty(btnIrPagosFacturar.Text.Trim())) return 0;
                return Convert.ToInt32(btnIrPagosFacturar.Text.Trim());
            }
            set { btnIrPagosFacturar.Text = value.ToString(CultureInfo.InvariantCulture); }
        }
        /// <summary>
        /// Total de Pagos No Facturados
        /// </summary>
        public int TotalNoFacturado
        {
            get
            {
                if (string.IsNullOrEmpty(btnIrPagosNoFacturados.Text.Trim())) return 0;
                return Convert.ToInt32(btnIrPagosNoFacturados.Text.Trim());
            }
            set { btnIrPagosNoFacturados.Text = value.ToString(CultureInfo.InvariantCulture); }
        }
        /// <summary>
        /// Identificador de la Unidad Operativa
        /// </summary>
        public int? UnidadOperativaID
        {
            get
            {
                var masterMsj = (Site)Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    return masterMsj.Adscripcion.UnidadOperativa.Id;
                return null;
            }
        }
        /// <summary>
        /// Identificador del Usuario
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                var masterMsj = (Site)Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id;
                return null;
            }
        }
        /// <summary>
        /// Nombre de la Sucursal Seleccionada
        /// </summary>
        public string SucursalNombre {
            get {
                string _result = string.Empty;
                if (ddlSucursales.SelectedValue != "0")
                    _result = ddlSucursales.SelectedItem.Text.ToUpper();

                return _result;
            }
        }

        #region SC0035
        /// <summary>
        /// Número de contrato a buscar.
        /// </summary>
        public string NumeroContrato
        {
            get { return string.IsNullOrEmpty(txtNumeroContrato.Text.Trim()) ? null : txtNumeroContrato.Text.Trim().ToUpper(); }
            set { txtNumeroContrato.Text = value ?? string.Empty; }
        }
        /// <summary>
        /// Identificador de la Cuenta Cliente
        /// </summary>
        public int? CuentaClienteID
        {
            get
            {
                if (string.IsNullOrEmpty(hdnCuentaClienteID.Value.Trim()))
                    return null;

                return Convert.ToInt32(hdnCuentaClienteID.Value.Trim());
            }
            set
            {
                hdnCuentaClienteID.Value = value.ToString();
            }
        }
        /// <summary>
        /// Nombre del Cliente seleccionado.
        /// </summary>
        public string NombreCuentaCliente
        {
            get { return string.IsNullOrEmpty(txtNombreCuentaCliente.Text.Trim()) ? null : txtNombreCuentaCliente.Text.Trim().ToUpper(); }
            set { txtNombreCuentaCliente.Text = value ?? string.Empty; }
        }
        /// <summary>
        /// Numero económico o vin a consultar.
        /// </summary>
        public string VinNumeroEconomico
        {
            get { return string.IsNullOrEmpty(txtVinNumeroEconomico.Text.Trim()) ? null : txtVinNumeroEconomico.Text.Trim().ToUpper(); }
            set { txtVinNumeroEconomico.Text = value ?? string.Empty; }
        }
        #endregion

        #region Propiedades Buscador
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
                    objeto = (Session[nombreSession]);

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
                    objeto = (Session[ViewState_Guid]);

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

        public enum ECatalogoBuscador
        {
            Sucursal = 0,
            CuentaClienteIdealease = 1
        }
        #endregion Propiedades Buscador

        #region Modulo
        //RQM 14078, se agrega el valor intermedio para poder asignarle un valor fuera del valor del webConfig
        /// <summary>
        /// Identificador del Módulo desde el cual se esta accediendo
        /// </summary>
        public int? ModuloID
        {
            get
            {
                int? id = null;
                if (id != null)
                {
                    return id;
                }
                else
                {
                    if (this.Session["ModuloID"] != null)
                        return Convert.ToInt32(this.Session["ModuloID"]);
                    else
                    {
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                            return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);
                    }

                    return null;
                }
            }
            set
            {
                //RQM 14150, se agrega el set para que permita en la configuración del modulo asignarle valor
                if (value != null)
                {
                    //Se guarda en sesión el valor del módulo id
                    this.Session["ModuloID"] = value;
                }
            }
        }
        /// <summary>
        /// URL del Logotipo de la Unidad Operativa
        /// </summary>
        public string URLLogoEmpresa {
            get {
                string s = this.ResolveUrl("~/Contenido/Imagenes/LogoBepensaMotriz.png");

                if (Session["ConfiguracionModuloSDNI"] != null && Session["ConfiguracionModuloSDNI"] is ConfiguracionModuloBO) {
                    string config = ((ConfiguracionModuloBO)Session["ConfiguracionModuloSDNI"]).URLLogoEmpresa;
                    if (!string.IsNullOrEmpty(config) && !string.IsNullOrWhiteSpace(config))
                        return config;
                }

                return s;
            }
        }
        /// <summary>
        /// Identificador del Usuario
        /// </summary>
        public AdscripcionBO Adscripcion
        {
            get
            {
                var masterMsj = (Site)Master;

                if (masterMsj != null && masterMsj.Adscripcion != null)
                    return masterMsj.Adscripcion;
                return null;
            }
        }
        #endregion
        #endregion

        #region Métodos
        /// <summary>
        /// Disara el evento Init de la Master
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e) {
            try {
                base.OnInit(e);
                Presentador = new PagosMasterPRE(this);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR,
                    NombreClase + ".OnInit: " + ex.Message);
            }
        }
        /// <summary>
        /// Marca como seleccionado el Marcador de Pagos no Facturados
        /// </summary>
        public void MarcarPagosFacturar()
        {
            btnIrPagosFacturar.Enabled = false;
        }
        /// <summary>
        /// Marca como seleccionado el Marcador de Pagos no Facturados
        /// </summary>
        public void MarcarPagoNoFacturados()
        {
            btnIrPagosNoFacturados.Enabled = false;
        }
        /// <summary>
        /// Despliega mensajes al usuario
        /// </summary>
        /// <param name="mensaje">Mnesaje a desplegar</param>
        /// <param name="tipo">Tipo de Mensaje</param>
        /// <param name="detalle">Detalle del Mensaje</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)Master;
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
        /// Carga la lista de Departamentos en la Intefaz de Usuario
        /// </summary>
        /// <param name="listado">Listado de Departamentos</param>
        public void CargarDepartamentos(List<EDepartamento> listado)
        {
            var Lista = new List<KeyValuePair<int, string>>();

            if (listado != null)
                Lista.AddRange(from dep in listado
                    let tipo =
                        ((DescriptionAttribute)
                            dep.GetType()
                                .GetField(dep.ToString())
                                .GetCustomAttributes(typeof (DescriptionAttribute), false)[0]).Description
                    select new KeyValuePair<int, string>((int) dep, tipo));

            // Agregar el Item de fachada
            Lista.Insert(0, new KeyValuePair<int, string>(-1, "TODOS"));
            //Limpiar el DropDownList Actual
            ddlAreaSeleccionada.Items.Clear();
            // Asignar Lista al DropDownList
            ddlAreaSeleccionada.DataTextField = "Value";
            ddlAreaSeleccionada.DataValueField = "Key";
            ddlAreaSeleccionada.DataSource = Lista;
            ddlAreaSeleccionada.DataBind();
        }
        
        /// <summary>
        /// Carga las sucursales autorizadas en el combobox
        /// </summary>
        /// <param name="lstSucursales">Lista de sucursales autorizadas</param>
        public void CargarSucursales(List<SucursalBO> lstSucursales) {
            try {
                List<SucursalBO> lstSucCargar = new List<SucursalBO>();
                // Agregar el SucursalBO de fachada
                lstSucCargar.Insert(0, new SucursalBO { Id = 0, Nombre = "TODAS" });
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
                this.MostrarMensaje("Error al cargar las sucursales autorizadas.", ETipoMensajeIU.ERROR, "ConsultarContratoROUI.CargarSucursales: " + ex.Message);
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
        /// Redirecciona a la Consulta de Pagos a Facturar
        /// </summary>
        public void IrConsultarPagosFacturar()
        {
            Response.Redirect("../Facturacion.AplicacionesFacturacion.UI/ConsultarPagosFacturarUI.aspx", true);
        }
        /// <summary>
        /// Redirecciona a la Consulta de Pagos No Facturados
        /// </summary>
        public void IrConsultarPagosNoFacturados()
        {
            Response.Redirect("../Facturacion.AplicacionesFacturacion.UI/ConsultarPagosNoFacturadosUI.aspx", true);
        }
        /// <summary>
        /// Limpia la session del modulo.
        /// </summary>
        public void LimpiarSession()
        {
            PagosConsultados = null;
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

        #region Métodos para el Buscador
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="catalogoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            ViewState_Catalogo = catalogoBusqueda;
            Session_ObjetoBuscador = Presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            Session_BOSelecto = null;
            RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
        }
        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="catalogo">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            Presentador.DesplegarResultadoBuscador(catalogo.ToString(), Session_BOSelecto);
            Session_BOSelecto = null;
        }
        #endregion
        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Presentador = new PagosMasterPRE(this);
                if (!IsPostBack)
                    Presentador.PrimeraCarga();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".Page_Load: " + ex.Message);
            }
        }

        protected void btnActualizarMarcadores_Click(object sender, EventArgs e)
        {
            try
            {
                Presentador.ActualizarMarcadores();
            }
            catch (Exception ex)
            {

                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".btnActualizarMarcadores_Click: " + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e) {
            try {
                switch (ViewState_Catalogo) {
                    case ECatalogoBuscador.CuentaClienteIdealease:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".btnResult_Click: " + ex.Message);
            }
        }

        protected void txtNombreCuentaCliente_TextChanged(object sender, EventArgs e) {
            try {
                string nombreCuentaCliente = NombreCuentaCliente;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                NombreCuentaCliente = nombreCuentaCliente;
                if (NombreCuentaCliente != null) {
                    EjecutaBuscador("CuentaClienteIdealeaseSimple", ECatalogoBuscador.CuentaClienteIdealease);
                    NombreCuentaCliente = null;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".txtNombreCuentaCliente_TextChanged:" + ex.Message);
            }
        }

        protected void ibtnBuscarCliente_Click(object sender, EventArgs e) {
            try {
                if (!string.IsNullOrWhiteSpace(this.NombreCuentaCliente))
                    EjecutaBuscador("CuentaClienteIdealeaseSimple&hidden=0", ECatalogoBuscador.CuentaClienteIdealease);
                else
                    MostrarMensaje("Ingrese un parámetro de búsqueda.", ETipoMensajeIU.INFORMACION);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".ibtnBuscarCliente_Click:" + ex.Message);
            }
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                AccionConsultar.Invoke();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".btnConsultar_Click: " + ex.Message);
            }
        }

        protected void btnIrPagosFacturar_Click(object sender, EventArgs e)
        {
            IrConsultarPagosFacturar();
        }

        protected void btnIrPagosNoFacturados_Click(object sender, EventArgs e)
        {
            IrConsultarPagosNoFacturados();
        }
        #endregion
    }
}