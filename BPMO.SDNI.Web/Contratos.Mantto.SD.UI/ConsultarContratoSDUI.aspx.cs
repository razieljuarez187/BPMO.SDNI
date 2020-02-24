//Satisface al CU029 - Consultar Contratos de Mantenimiento
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.Mantto.BOF;
using BPMO.SDNI.Contratos.Mantto.PRE;
using BPMO.SDNI.Contratos.Mantto.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.Mantto.SD.UI
{
    public partial class ConsultarContratoSDUI : System.Web.UI.Page, IConsultarContratoManttoVIS
    {
        #region Atributos

        /// <summary>
        /// Presentador de Consultar Contratos
        /// </summary>
        private ConsultarContratoManttoPRE presentador;


        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "ConsultarContratoSDUI";

        /// <summary>
        /// Enumerador de Catalogos para el Buscador
        /// </summary>
        public enum ECatalogoBuscador
        {
            CuentaClienteIdealease = 0,
            UnidadIdealease = 1,
            Sucursal = 2
        }
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

        public string NumeroContrato
        {
            get { return string.IsNullOrEmpty(txtNumeroContrato.Text.Trim()) ? null : txtNumeroContrato.Text.Trim(); }
            set { txtNumeroContrato.Text = value ?? string.Empty; }
        }
        public DateTime? FechaContratoInicial
        {
            get
            {
                return !string.IsNullOrEmpty(this.txtFechaContratoInicial.Text.Trim())
                           ? (DateTime?)DateTime.Parse(this.txtFechaContratoInicial.Text.Trim())
                           : null;
            }
            set
            {
                if (value != null) this.txtFechaContratoInicial.Text = value.Value.ToString("dd/MM/yyyy");
            }
        }
        public DateTime? FechaContratoFinal
        {
            get
            {
                return !string.IsNullOrEmpty(this.txtFechaContratoFinal.Text.Trim())
                           ? (DateTime?)DateTime.Parse(this.txtFechaContratoFinal.Text.Trim())
                           : null;
            }
            set
            {
                if (value != null) this.txtFechaContratoFinal.Text = value.Value.ToString("dd/MM/yyyy");
            }
        }
        public string CuentaClienteNombre
        {
            get { return string.IsNullOrEmpty(this.txtNombreCuentaCliente.Text.Trim()) ? null : this.txtNombreCuentaCliente.Text.Trim().ToUpper(); }
            set { this.txtNombreCuentaCliente.Text = value ?? string.Empty; }
        }
        public int? CuentaClienteID
        {
            get
            {
                return !string.IsNullOrEmpty(this.hdnCuentaClienteID.Value.Trim())
                           ? (int?)int.Parse(this.hdnCuentaClienteID.Value.Trim())
                           : null;
            }
            set { this.hdnCuentaClienteID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        public string SucursalNombre
        {
            get
            {
                String sucursalNombre = null;
                if (this.txtSucursal.Text.Trim().Length > 0)
                    sucursalNombre = this.txtSucursal.Text.Trim().ToUpper();
                return sucursalNombre;
            }
            set
            {
                if (value != null)
                    this.txtSucursal.Text = value.ToString();
                else
                    this.txtSucursal.Text = String.Empty;
            }
        }
        public int? SucursalID
        {
            get
            {
                int? sucursalID = null;
                if (!String.IsNullOrEmpty(this.hdnSucursalID.Value))
                    sucursalID = int.Parse(this.hdnSucursalID.Value.Trim());
                return sucursalID;
            }
            set
            {
                if (value != null)
                    this.hdnSucursalID.Value = value.ToString();
                else
                    this.hdnSucursalID.Value = string.Empty;
            }
        }
        public int? EstatusID
        {
            get
            {
                int? id = null;
                if (this.ddlEstatus.SelectedIndex > 0)
                    id = int.Parse(this.ddlEstatus.SelectedValue.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.ddlEstatus.SelectedValue = value.ToString();
                else
                    this.ddlEstatus.SelectedIndex = -1;
            }
        }
        public int? TipoContratoID
        {
            get { return (int)ETipoContrato.SD; }
            set { }
        }
        public string NumeroSerie
        {
            get { return string.IsNullOrEmpty(this.txtNumeroSerie.Text.Trim()) ? null : this.txtNumeroSerie.Text.Trim(); }
            set { this.txtNumeroSerie.Text = value ?? string.Empty; }
        }
        public string NumeroEconomico
        {
            get { return string.IsNullOrEmpty(this.txtNumeroEconomico.Text.Trim()) ? null : this.txtNumeroEconomico.Text.Trim(); }
            set { this.txtNumeroEconomico.Text = value ?? string.Empty; }
        }

        public List<ContratoManttoBOF> Resultado
        {
            get { return Session["ContratosEncontradosSD"] as List<ContratoManttoBOF>; }
        }

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
        #endregion Propiedades Buscador

        #region SC_0051
        /// <summary>
        /// Obtiene o estable la lista de sucursales a las que el usaurio autenticado tiene permiso de acceder
        /// </summary>
        public List<object> SucursalesAutorizadas
        {
            get
            {
                if (Session["SucursalesAutSD"] != null)
                    return Session["SucursalesAutSD"] as List<object>;
                else
                {
                    var lstRet = new List<SucursalBO>();
                    return lstRet.ConvertAll(x => (object)x);
                }
            }

            set { Session["SucursalesAutSD"] = value; }
        }

        #endregion
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.ucHerramientas.Presentador = new ucHerramientasManttoPRE(this.ucHerramientas);
                presentador = new ConsultarContratoManttoPRE(this, this.ucHerramientas);
                if (!IsPostBack)
                {
                    presentador.PrepararBusqueda();
                }

                this.presentador.CargarPlantillas();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void EstablecerResultado(List<ContratoManttoBOF> resultado)
        {
            Session["ContratosEncontradosSD"] = resultado;

            this.grdContratos.DataSource = resultado;
            this.grdContratos.DataBind();
        }
        public void EstablecerOpcionesEstatus(Dictionary<int, string> estatus)
        {
            if (ReferenceEquals(estatus, null))
                estatus = new Dictionary<int, string>();

            this.ddlEstatus.Items.Clear();
            this.ddlEstatus.Items.Add(new ListItem("Todos", "-1"));

            this.ddlEstatus.DataSource = estatus;
            this.ddlEstatus.DataValueField = "key";
            this.ddlEstatus.DataTextField = "value";
            this.ddlEstatus.DataBind();
        }

        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".EstablecerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            Session[key] = value;
        }
        public object ObtenerPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".ObtenerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            return Session[key];
        }
        public void LimpiarPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".LimpiarPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            if (Session[key] != null)
                Session.Remove(key);
        }

        public void CargarArchivos(List<object> resultado)
        {
            this.ucHerramientas.Presentador.CargarArchivos(resultado);
        }
        public List<object> ObtenerPlantillas(string key)
        {
            return (List<object>)Session[key];
        }

        public void PermitirRegistrar(bool permitir)
        {
            this.hlkRegistro.Enabled = permitir;
        }

        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        public void RedirigirADetalle()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.Mantto.SD.UI/DetalleContratoSDUI.aspx"));
        }

        public void LimpiarSesion()
        {
            if (Session["ContratosEncontradosSD"] != null)
                Session.Remove("ContratosEncontradosSD");
        }
        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
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

        #region Métodos para el Buscador
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="catalogoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            ViewState_Catalogo = catalogoBusqueda;
            Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            Session_BOSelecto = null;
            RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
        }
        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="catalogo">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), Session_BOSelecto);
            Session_BOSelecto = null;
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
        #endregion

        #region Eventos
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ConsultarContratos();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnBuscar_Click:" + ex.Message);
            }
        }

        protected void grdContratos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grdContratos.DataSource = this.Resultado;
                this.grdContratos.PageIndex = e.NewPageIndex;
                this.grdContratos.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdContratos_PageIndexChanging: " + ex.Message);
            }
        }
        protected void grdContratos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName.Trim())
                {
                    case "Detalles":
                        int? ContratoID = (e.CommandArgument != null) ? (int?)Convert.ToInt32(e.CommandArgument) : null;
                        this.presentador.IrADetalle(ContratoID);
                        break;
                    case "Page":
                        break;
                    default:
                        MostrarMensaje("Comando no encontrado", ETipoMensajeIU.ERROR, "El comando no está especificado en el sistema");
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al intentar realizar una acción sobre un contrato:", ETipoMensajeIU.ERROR, nombreClase + ".grdContratos_RowCommand: " + ex.Message);
            }
        }
        protected void grdContratos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ContratoManttoBOF contrato = (ContratoManttoBOF)e.Row.DataItem;
                    Label lblFechaContrato = e.Row.FindControl("lblFechaContrato") as Label;
                    if (lblFechaContrato != null)
                    {
                        string fecha = string.Empty;
                        if (contrato.FechaContrato != null)
                        {
                            fecha = String.Format("{0:dd/MM/yyyy}", contrato.FechaContrato);
                        }
                        lblFechaContrato.Text = fecha;
                    }

                    Label labelSucursalNombre = e.Row.FindControl("lblSucursal") as Label;
                    if (labelSucursalNombre != null)
                    {
                        string sucursalNombre = string.Empty;
                        if (contrato.Sucursal != null)
                            if (contrato.Sucursal.Nombre != null)
                                sucursalNombre = contrato.Sucursal.Nombre;
                        labelSucursalNombre.Text = sucursalNombre;
                    }

                    Label labelClienteNombre = e.Row.FindControl("lblCliente") as Label;
                    if (labelClienteNombre != null)
                    {
                        string clienteNombre = string.Empty;
                        if (contrato.Cliente != null)
                            if (contrato.Cliente.Nombre != null)
                                clienteNombre = contrato.Cliente.Nombre;
                        labelClienteNombre.Text = clienteNombre;
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".grdContratos_RowDataBound: " + ex.Message);
            }
        }

        #region Eventos para el Buscador
        /// <summary>
        /// TextChanged activa el llamado al Buscador para la busqueda de Sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtSucursal_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreSucursal = SucursalNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                SucursalNombre = nombreSucursal;
                if (SucursalNombre != null)
                {
                    this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                    SucursalNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtSucursal_TextChanged" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscarSucursal_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarSucursal_Click" + ex.Message);
            }
        }

        protected void txtNumeroSerie_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string numeroSerie = NumeroSerie;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.UnidadIdealease);

                NumeroSerie = numeroSerie;
                if (NumeroSerie != null)
                {
                    EjecutaBuscador("UnidadIdealease", ECatalogoBuscador.UnidadIdealease);
                    NumeroSerie = null;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNumeroSerie_TextChanged: " + ex.Message);
            }
        }

        protected void btnBuscarVin_Click(object sender, ImageClickEventArgs e)
        {
            if (this.txtNumeroSerie.Text.Length < 1)
            {
                this.MostrarMensaje("Es necesario un número de serie.", ETipoMensajeIU.ADVERTENCIA);
                return;
            }

            try
            {
                this.EjecutaBuscador("UnidadIdealease&hidden=0", ECatalogoBuscador.UnidadIdealease);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarVin_Click" + ex.Message);
            }
        }

        protected void txtNombreCuentaCliente_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreCuentaCliente = this.CuentaClienteNombre;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                this.CuentaClienteNombre = nombreCuentaCliente;
                if (this.CuentaClienteNombre != null)
                {
                    EjecutaBuscador("CuentaClienteIdealease", ECatalogoBuscador.CuentaClienteIdealease);
                    this.CuentaClienteNombre = null;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNombreCuentaCliente_TextChanged:" + ex.Message);
            }
        }
        protected void ibtnBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                EjecutaBuscador("CuentaClienteIdealease&&hidden=0", ECatalogoBuscador.CuentaClienteIdealease);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarCliente_Click:" + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.CuentaClienteIdealease:
                    case ECatalogoBuscador.UnidadIdealease:
                    case ECatalogoBuscador.Sucursal:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click: " + ex.Message);
            }
        }
        #endregion
        #endregion
    }
}