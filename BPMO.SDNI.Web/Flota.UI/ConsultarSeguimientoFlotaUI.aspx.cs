//Satisface al CU081 - Consultar Seguimiento Flota
//Satisface la solicitud de cambio SC0006

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Flota.PRE;
using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Flota.UI
{
    public partial class ConsultarSeguimientoFlotaUI : System.Web.UI.Page, IConsultarSeguimientoFlotaVIS
    {
        #region Atributos
        private ConsultarSeguimientoFlotaPRE presentador = null;
        private string nombreClase = "ConsultarSeguimientoFlotaUI";

        private ETipoEmpresa ETipoEmpresa;

        public enum ECatalogoBuscador
        {
            UnidadIdealease,
            Sucursal,
            TipoUnidad,
            Modelo,
            Marca
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

        /// <summary>
        /// Obtiene o establece el número económico de la unidad que será rentada
        /// </summary>
        public string VIN
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtNumVin.Text)) ? null : this.txtNumVin.Text.Trim().ToUpper();
            }
            set
            {
                this.txtNumVin.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public string NumeroEconomico
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtNumeroEconomico.Text)) ? null : this.txtNumeroEconomico.Text.Trim().ToUpper();
            }
            set
            {
                this.txtNumeroEconomico.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
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
        public int? MarcaID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnMarcaID.Value))
                    id = int.Parse(this.hdnMarcaID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnMarcaID.Value = value.ToString();
                else
                    this.hdnMarcaID.Value = string.Empty;
            }
        }
        public string MarcaNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtMarca.Text)) ? null : this.txtMarca.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtMarca.Text = value;
                else
                    this.txtMarca.Text = string.Empty;
            }
        }
        public int? ModeloID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnModeloID.Value))
                    id = int.Parse(this.hdnModeloID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnModeloID.Value = value.ToString();
                else
                    this.hdnModeloID.Value = string.Empty;
            }
        }
        public string ModeloNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtModelo.Text)) ? null : this.txtModelo.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtModelo.Text = value;
                else
                    this.txtModelo.Text = string.Empty;
            }
        }
        public int? TipoUnidadID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnTipoUnidadID.Value))
                    id = int.Parse(this.hdnTipoUnidadID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnTipoUnidadID.Value = value.ToString();
                else
                    this.hdnTipoUnidadID.Value = string.Empty;
            }
        }
        public string TipoUnidadNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtTipoUnidad.Text)) ? null : this.txtTipoUnidad.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtTipoUnidad.Text = value;
                else
                    this.txtTipoUnidad.Text = string.Empty;
            }
        }
        public int? AreaID
        {
            get
            {
                int? id = null;
                if (this.ddlArea.SelectedIndex > 0)
                    id = int.Parse(this.ddlArea.SelectedValue);
                return id;
            }
            set
            {
                if (value != null)
                    this.ddlArea.SelectedValue = value.ToString();
                else
                    this.ddlArea.SelectedIndex = 0;
            }
        }

        public EAreaConstruccion? ETipoRentaConstruccion
        {
            get
            {
                EAreaConstruccion? area = null;
                if (this.ddlArea.SelectedIndex > 0)
                    area = (EAreaConstruccion)Enum.Parse(typeof(EAreaConstruccion), this.ddlArea.SelectedValue);
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

        public EAreaGeneracion? ETipoRentaGeneracion
        {
            get
            {
                EAreaGeneracion? area = null;
                if (this.ddlArea.SelectedIndex > 0)
                    area = (EAreaGeneracion)Enum.Parse(typeof(EAreaGeneracion), this.ddlArea.SelectedValue);
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

        public EAreaEquinova? ETipoRentaEquinova {
            get {
                EAreaEquinova? area = null;
                if (this.ddlArea.SelectedIndex > 0)
                    area = (EAreaEquinova)Enum.Parse(typeof(EAreaEquinova), this.ddlArea.SelectedValue);
                return area;
            }
            set {
                if (value == null)
                    this.ddlArea.SelectedIndex = 0;
                else
                    this.ddlArea.SelectedValue = ((int)value).ToString();
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
        public int? EstatusID
        {
            get
            {
                int? id = null;
                if (this.ddlEstatus.SelectedIndex > 0)
                    id = int.Parse(this.ddlEstatus.SelectedValue);
                return id;
            }
            set
            {
                if (value != null)
                    this.ddlEstatus.SelectedValue = value.ToString();
                else
                    this.ddlEstatus.SelectedIndex = 0;
            }
        }
        public DateTime? FechaAltaInicial
        {
            get
            {
                DateTime? date = null;

                if (!string.IsNullOrEmpty(this.txtFechaAltaInicial.Text.Trim()))
                    date = DateTime.Parse(this.txtFechaAltaInicial.Text.Trim());

                return date;
            }
            set
            {
                if (value != null) this.txtFechaAltaInicial.Text = value.Value.ToString("dd/MM/yyyy");
            }
        }
        public DateTime? FechaAltaFinal
        {
            get
            {
                DateTime? date = null;

                if (!string.IsNullOrEmpty(this.txtFechaAltaFinal.Text.Trim()))
                {
                    date = DateTime.Parse(this.txtFechaAltaFinal.Text.Trim());
                    date = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 23, 59, 59);
                }

                return date;
            }
            set
            {
                if (value != null) this.txtFechaAltaFinal.Text = value.Value.ToString("dd/MM/yyyy");
            }
        }
        public DateTime? FechaBajaInicial
        {
            get
            {
                DateTime? date = null;

                if (!string.IsNullOrEmpty(this.txtFechaBajaInicial.Text.Trim()))
                    date = DateTime.Parse(this.txtFechaBajaInicial.Text.Trim());

                return date;
            }
            set
            {
                if (value != null) this.txtFechaBajaInicial.Text = value.Value.ToString("dd/MM/yyyy");
            }
        }
        public DateTime? FechaBajaFinal
        {
            get
            {
                DateTime? date = null;

                if (!string.IsNullOrEmpty(this.txtFechaBajaFinal.Text.Trim()))
                {
                    date = DateTime.Parse(this.txtFechaBajaFinal.Text.Trim());
                    date = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 23, 59, 59);
                }

                return date;
            }
            set
            {
                if (value != null) this.txtFechaBajaFinal.Text = value.Value.ToString("dd/MM/yyyy");
            }
        }

        public object Resultado
        {
            get { return Session["ResultadoSeguimientoFlota"]; }
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
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //SC0006 - Los cambios de la solicitud de cambios se reflejan desde el presenter
                presentador = new ConsultarSeguimientoFlotaPRE(this);
                if (!IsPostBack)
                {
                    presentador.PrepararBusqueda();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void EstablecerResultado(object resultado)
        {
            Session["ResultadoSeguimientoFlota"] = resultado;

            this.grdResultado.DataSource = resultado;
            this.grdResultado.DataBind();
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

        public void PermitirRealizarCambioSucursalEquipoAliado(bool permitir)
        {
            this.hlkMovimiento.Enabled = permitir;
        }

        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        public void RedirigirAExpediente()
        {
            this.Response.Redirect(this.ResolveUrl("~/Flota.UI/DetalleExpedienteUnidadUI.aspx"));
        }

        public void LimpiarSesion()
        {
            if (Session["ResultadoSeguimientoFlota"] != null)
                Session.Remove("ResultadoSeguimientoFlota");
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            Site masterMsj = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
            }
            else
            {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        #region [RQM 13285- Integración Construcción y Generacción]

        public void CargarAreaIdealease(Dictionary<string, object> tipoAreaIdealease)
        {
            this.ddlArea.Items.Clear();
            this.ddlArea.Items.Add(new ListItem() { Value = "x", Text = "Todos" });

            foreach (KeyValuePair<string, object> tipoRenta in tipoAreaIdealease)
                this.ddlArea.Items.Add(new ListItem() { Text = tipoRenta.Key, Value = tipoRenta.Value.ToString() });
        }
        public void CargarAreaConstruccion(Dictionary<string, object> tipoAreaCostruccion)
        {
            this.ddlArea.Items.Clear();
            this.ddlArea.Items.Add(new ListItem() { Value = "x", Text = "Todos" });

            foreach (KeyValuePair<string, object> tipoRenta in tipoAreaCostruccion)
                this.ddlArea.Items.Add(new ListItem() { Text = tipoRenta.Key, Value = tipoRenta.Value.ToString() });
        }
        public void CargarAreaGeneracion(Dictionary<string, object> tipoAreaGeneracion)
        {
            this.ddlArea.Items.Clear();
            this.ddlArea.Items.Add(new ListItem() { Value = "x", Text = "Todos" });

            foreach (KeyValuePair<string, object> tipoRenta in tipoAreaGeneracion)
                this.ddlArea.Items.Add(new ListItem() { Text = tipoRenta.Key, Value = tipoRenta.Value.ToString() });
        }
        public void CargarAreaEquinova(Dictionary<string, object> tipoAreaEquinova) {
            this.ddlArea.Items.Clear();
            this.ddlArea.Items.Add(new ListItem() { Value = "x", Text = "Todos" });

            foreach (KeyValuePair<string, object> tipoRenta in tipoAreaEquinova)
                this.ddlArea.Items.Add(new ListItem() { Text = tipoRenta.Key, Value = tipoRenta.Value.ToString() });
        }

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
        #endregion

        #region Eventos
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ConsultarFlota();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al consultar actas de nacimiento", ETipoMensajeIU.ERROR, this.nombreClase + ".btnBuscar_Click:" + ex.Message);
            }
        }

        protected void grdResultado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grdResultado.DataSource = this.Resultado;
                this.grdResultado.PageIndex = e.NewPageIndex;
                this.grdResultado.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdResultado_PageIndexChanging: " + ex.Message);
            }
        }
        protected void grdResultado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName.Trim())
                {
                    case "Detalles":
                        int? unidadID = (e.CommandArgument != null) ? (int?)Convert.ToInt32(e.CommandArgument) : null;
                        this.presentador.IrAExpediente(unidadID);
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
                MostrarMensaje("Inconsistencias al intentar realizar una acción sobre un contrato:", ETipoMensajeIU.ERROR, nombreClase + ".grdResultado_RowCommand: " + ex.Message);
            }
        }

        #region Eventos para el buscador
        /// <summary>
        /// TextChanged activa el llamado al Buscador para la busqueda de Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtNumVin_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string serieUnidad = VIN;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.UnidadIdealease);

                VIN = serieUnidad;
                if (VIN != null)
                {
                    this.EjecutaBuscador("UnidadIdealease", ECatalogoBuscador.UnidadIdealease);
                    VIN = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtNumVin_TextChanged" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarVin_Click(object sender, ImageClickEventArgs e)
        {
            if (this.txtNumVin.Text.Length < 1)
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
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnBuscarVin_Click" + ex.Message);
            }
        }

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
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtSucursal_TextChanged" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarSucursal_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnBuscarSucursal_Click" + ex.Message);
            }
        }

        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Tipo de Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtTipoUnidad_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string tipoUnidadNombre = this.TipoUnidadNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.TipoUnidad);

                this.TipoUnidadNombre = tipoUnidadNombre;
                if (this.TipoUnidadNombre != null)
                {
                    this.EjecutaBuscador("TipoUnidad", ECatalogoBuscador.TipoUnidad);
                    this.TipoUnidadNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Tipo de Unidad", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtTipoUnidad_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Tipo de Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaTipoUnidad_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("TipoUnidad&hidden=0", ECatalogoBuscador.TipoUnidad);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Tipo de Unidad", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscaTipoUnidad_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Marca
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtMarca_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string marcaNombre = this.MarcaNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Marca);

                this.MarcaNombre = marcaNombre;
                if (this.MarcaNombre != null)
                {
                    this.EjecutaBuscador("Marca", ECatalogoBuscador.Marca);
                    this.MarcaNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Marca", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtMarca_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Marca
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaMarca_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("Marca&hidden=0", ECatalogoBuscador.Marca);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Marca", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscaMarca_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Modelo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtModelo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string modeloNombre = this.ModeloNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Modelo);

                this.ModeloNombre = modeloNombre;
                if (this.ModeloNombre != null)
                {
                    this.EjecutaBuscador("Modelo", ECatalogoBuscador.Modelo);
                    this.ModeloNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Modelo", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtModelo_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Modelo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaModelo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("Modelo&hidden=0", ECatalogoBuscador.Modelo);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Modelo", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscaModelo_Click:" + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.UnidadIdealease:
                    case ECatalogoBuscador.Modelo:
                    case ECatalogoBuscador.TipoUnidad:
                    case ECatalogoBuscador.Sucursal:
                    case ECatalogoBuscador.Marca:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click" + ex.Message);
            }
        }
        #endregion
        #endregion
    }
}