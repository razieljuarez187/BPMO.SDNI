// Satisface al Caso de uso CU022 - Consultar Contrato Full Service Leasing
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.FSL.BOF;
using BPMO.SDNI.Contratos.FSL.PRE;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.FSL.UI
{
    public partial class ConsultarContratosFSLUI : Page, IConsultarContratosFSLVIS
    {
        #region Atributos

        /// <summary>
        /// Presentador de Consultar Contratos
        /// </summary>
        private ConsultarContratosFSLPRE presentador;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "ConsultarContratosFSLUI";

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

        /// <summary>
        /// Unidad Operativa de Configurada
        /// </summary>
        public UnidadOperativaBO UnidadOperativa
        {
            get
            {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    return new UnidadOperativaBO { Id = masterMsj.Adscripcion.UnidadOperativa.Id };
                return null;
            }
        }
        /// <summary>
        /// Numero del Contrato a Buscar
        /// </summary>
        public string NumeroContrato
        {
            get { return string.IsNullOrEmpty(txtNumeroContrato.Text.Trim()) ? null : txtNumeroContrato.Text.Trim(); }
            set { txtNumeroContrato.Text = value ?? string.Empty; }
        }
        /// <summary>
        /// Identificador de la Cuenta Cliente
        /// </summary>
        public int? CuentaClienteID
        {
            get
            {
                return !string.IsNullOrEmpty(hdnCuentaClienteID.Value.Trim())
                           ? (int?)int.Parse(hdnCuentaClienteID.Value.Trim())
                           : null;
            }
            set
            {
                hdnCuentaClienteID.Value = value != null ? value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            }
        }
        /// <summary>
        /// Identificador del Cliente
        /// </summary>
        public int? ClienteID
        {
            get
            {
                return !string.IsNullOrEmpty(hdnClientID.Value.Trim())
                           ? (int?)int.Parse(hdnClientID.Value.Trim())
                           : null;
            }
            set
            {
                hdnClientID.Value = value != null ? value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            }
        }
        /// <summary>
        /// Nombre del Cliente
        /// </summary>
        public string NombreCuentaCliente
        {
            get { return string.IsNullOrEmpty(txtNombreCuentaCliente.Text.Trim()) ? null : txtNombreCuentaCliente.Text.Trim().ToUpper(); }
            set { txtNombreCuentaCliente.Text = value ?? string.Empty; }
        }
        /// <summary>
        /// Plazo del  Contrato en Meses
        /// </summary>
        public int? PlazoMeses
        {
            get
            {
                return !string.IsNullOrEmpty(txtPlazo.Text.Trim())
                           ? (int?)int.Parse(txtPlazo.Text.Trim())
                           : null;
            }
            set
            {
                txtPlazo.Text = value != null ? value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            }
        }
        /// <summary>
        /// Fecha de Inicio del Contrato
        /// </summary>
        public DateTime? FechaInicioContrato
        {
            get
            {
                return !string.IsNullOrEmpty(txtFechaInicio.Text.Trim())
                           ? (DateTime?)DateTime.Parse(txtFechaInicio.Text.Trim())
                           : null;
            }
            set
            {
                txtFechaInicio.Text = value != null ? value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            }
        }
        /// <summary>
        /// Fecha de Terminacion del Contrato
        /// </summary>
        public DateTime? FechaTerminoContrato
        {
            get
            {
                return !string.IsNullOrEmpty(txtFechaTerminacion.Text.Trim())
                           ? (DateTime?)DateTime.Parse(txtFechaTerminacion.Text.Trim())
                           : null;
            }
            set
            {
                txtFechaTerminacion.Text = value != null ? value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            }
        }
        /// <summary>
        /// Estatus de Contrato Seleccionado
        /// </summary>
        public EEstatusContrato? Estatus
        {
            get
            {
                if (ddlEstatus.SelectedValue != "-1")
                    return (EEstatusContrato)int.Parse(ddlEstatus.SelectedValue);

                return null;
            }
        }
        /// <summary>
        /// Contratos Encontrados en la Consulta
        /// </summary>
        public List<ContratoFSLBOF> ContratosEncontrados
        {
            get { return Session["ContratosEncontrados"] as List<ContratoFSLBOF>; }
        }
        /// <summary>
        /// Usuario del Sistema
        /// </summary>
        public UsuarioBO Usuario
        {
            get
            {
                var masterMsj = (Site) Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario;
                return null;
            }
        }

        public String SucursalNombre
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

        #region SC_0008
        /// <summary>
        /// Devuelve el identificador del usuario que ha iniciado sesión en el sistema
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                var master = (Site)Page.Master;
                if (master != null)
                    if (master.Usuario != null && master.Usuario.Usuario != null)
                        if (master.Usuario.Id.HasValue)
                            return master.Usuario.Id.Value;
                return null;
            }
        }
        /// <summary>
        /// Devuelve el identificador de la unidad de adscripción desde la cual se ha iniciado sesión en el sistema
        /// </summary>
        public int? UnidadAdscripcionID
        {
            get
            {
                var master = (Site)Page.Master;
                if (master != null)
                    if (master.Adscripcion != null && master.Adscripcion.UnidadOperativa != null)
                        if (master.Adscripcion.UnidadOperativa.Id.HasValue)
                            return master.Adscripcion.UnidadOperativa.Id.Value;
                return null;
            }
        }
        #endregion

        #region SC_0051
        /// <summary>
        /// Obtiene o estable la lista de sucursales a las que el usaurio autenticado tiene permiso de acceder
        /// </summary>
        public List<object> SucursalesAutorizadas
        {
            get
            {
                if (Session["SucursalesAutFSL"] != null)
                    return Session["SucursalesAutFSL"] as List<object>;
                else
                {
                    var lstRet = new List<SucursalBO>();
                    return lstRet.ConvertAll(x => (object)x);
                }
            }
        
            set { Session["SucursalesAutFSL"] = value; }
        }

        #endregion
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ucHerramientas.Presentador = new ucHerramientasFSLPRE(ucHerramientas);
                presentador = new ConsultarContratosFSLPRE(this, ucHerramientas.Presentador);
                if (!IsPostBack)
                {
                    presentador.PrepararBusqueda();
                }

                ucHerramientas.ImprimirFormatoPersonaFisica = ImprimirFormatoPersonaFisica_Click;
                ucHerramientas.ImprimirFormatoPersonaMoral = ImprimirFormatoPersonaMoral;
                //SC0038
                presentador.CargarPlantillas();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Limpia el gid de contratos encontrados
        /// </summary>
        public void LimpiarContratosEncontrados()
        {
            Session.Remove("ContratosEncontrados");
            grdContratos.DataSource = new List<ContratoFSLBOF>();
            grdContratos.DataBind();
        }
        /// <summary>
        /// Carga y despliega el listado de Estatus
        /// </summary>
        /// <param name="listado"></param>
        public void CargarEstatus(List<EEstatusContrato> listado)
        {
            var Lista = new List<KeyValuePair<int, string>>();

            if (listado != null)
                Lista.AddRange(from estatus in listado let tipo = ((DescriptionAttribute)estatus.GetType().GetField(estatus.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description select new KeyValuePair<int, string>((int)estatus, tipo));

            // Agregar el Item de fachada
            Lista.Insert(0, new KeyValuePair<int, string>(-1, "TODOS"));
            //Limpiar el DropDownList Actual
            ddlEstatus.Items.Clear();
            // Asignar Lista al DropDownList
            ddlEstatus.DataTextField = "Value";
            ddlEstatus.DataValueField = "Key";
            ddlEstatus.DataSource = Lista;
            ddlEstatus.DataBind();
        }
        /// <summary>
        /// Carga y despliega en el grid los contratos encontrados
        /// </summary>
        /// <param name="contratos">Listado de Contratos encontrados</param>
        public void CargarContratosEncontrados(List<ContratoFSLBOF> contratos)
        {
            List<ContratoFSLBOF> Lista = contratos ?? new List<ContratoFSLBOF>();

            Session["ContratosEncontrados"] = Lista;

            grdContratos.DataSource = Lista;
            grdContratos.DataBind();
        }
        /// <summary>
        /// Establece el Paquete de Navegacion para el Detalle del Contrato Seleccionado
        /// </summary>
        /// <param name="Clave">Clave del Paquete</param>
        /// <param name="ContratoID">Identificador del Contrato Seleccionado</param>
        public void EstablecerPaqueteNavegacion(string Clave, int? ContratoID)
        {
            ContratoFSLBOF contrato = ContratosEncontrados.Find(cont => cont.ContratoID == ContratoID);

            if (contrato != null) Session[Clave] = contrato;
            else
            {
                throw new Exception(nombreClase +
                                    ".EstablecerPaqueteNavegacion: El contrato proporcionado no pertence al listado de contratos encontrados.");
            }
        }

        /// <summary>
        /// Establece el Paquete de Navegacion para imprimir los Formatos del Contrato
        /// </summary>
        /// <param name="Clave">Clave del Paquete de Navegacion</param>
        /// <param name="DatosReporte">Datos del Reporte</param>
        public void EstablecerPaqueteNavegacionImprimir(string Clave, Dictionary<string, object> DatosReporte)
        {
            Session["NombreReporte"] = Clave;
            Session["DatosReporte"] = DatosReporte;
        }

        /// <summary>
        /// Despliega el Detalle del Contrato Seleccionado
        /// </summary>
        public void IrADetalle()
        {
            const string Url = "DetalleContratoFSLUI.aspx";
            Response.Redirect(Url, false);
        }

        /// <summary>
        /// Despliega el visor de los formatos a imprimir
        /// </summary>
        public void IrAImprimir()
        {
            const string Url = "../Buscador.UI/VisorReporteUI.aspx";
            Response.Redirect(Url,true);
        }

        /// <summary>
        /// Bloquea los campos de consulta
        /// </summary>
        public void BloquearConsulta()
        {
            this.txtSucursal.Enabled = false;
            this.ibtnBuscarSucursal.Enabled = false;
            this.ddlEstatus.Enabled = false;
            this.txtFechaInicio.Enabled = false;
            this.txtFechaTerminacion.Enabled = false;
            this.txtNombreCuentaCliente.Enabled = false;
            this.txtNumeroContrato.Enabled = false;
            this.txtPlazo.Enabled = false;
            this.btnBuscar.Enabled = false;
            this.btnBuscar.Visible = false;
            this.ibtnBuscarCliente.Enabled = false;
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
        #endregion

        #region SC0008
        public void PermitirRegistrar(bool status)
        {
            this.hlRegistroOrden.Enabled = status;
        }
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        #endregion

        #region SC0038
        public void CargarArchivos(List<object> resultado)
        {
            this.ucHerramientas.Presentador.CargarArchivos(resultado);
        }

        public List<object> ObtenerPlantillas(string key)
        {
            return (List<object>)Session[key];
        }
        #endregion

        #region SC_0051

        public void LimpiarSession()
        {
            Session.Remove("SucursalesAutFSL");
            Session.Remove("ContratosEncontrados");
        }

        #endregion
        #endregion

        #region Eventos
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.ConsultarContratos();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconcistencia al consultar los contratos", ETipoMensajeIU.ERROR, nombreClase + ".btnBuscar_Click" + ex.Message);
            }
            
        }
        
        protected void grdContratos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdContratos.DataSource = Session["ContratosEncontrados"];
                grdContratos.PageIndex = e.NewPageIndex;
                grdContratos.DataBind();
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
                        {

                            int? ContratoID = (e.CommandArgument != null) ? (int?)Convert.ToInt32(e.CommandArgument) : null;
                            presentador.IrADetalle(ContratoID);
                            break;
                        }
                    case "Page":
                        break;
                    default:
                        {
                            MostrarMensaje("Comando no encontrado", ETipoMensajeIU.ERROR,
                                           "El comando no está especificado en el sistema");
                            break;
                        }
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
                    ContratoFSLBOF contrato = (ContratoFSLBOF) e.Row.DataItem;
                    Label lblFechaInicioContrato = e.Row.FindControl("lblFechaInicioContrato") as Label;
                    if (lblFechaInicioContrato != null)
                    {
                        string fecha = string.Empty;
                        if (contrato.FechaInicioContrato != null)
                            if (contrato.FechaInicioContrato != null)
                            {
                                fecha = String.Format("{0:dd/MM/yyyy}", contrato.FechaInicioContrato);
                            }
                        lblFechaInicioContrato.Text = fecha;
                    }

                    Label lblFechaTerminacionContrato = e.Row.FindControl("lblFechaTerminacionContrato") as Label;
                    if (lblFechaTerminacionContrato != null)
                    {

                        lblFechaTerminacionContrato.Text = string.Format("{0:dd/MM/yyyy}",
                                                                         contrato.CalcularFechaTerminacionContrato());
                    }

                    Label labelSucursalNombre = e.Row.FindControl("lblSucursal") as Label;
                    if (labelSucursalNombre != null)
                    {
                        string sucursalNombre = string.Empty;
                        if (contrato.Sucursal != null)
                            if (contrato.Sucursal.Nombre != null)
                            {
                                sucursalNombre = contrato.Sucursal.Nombre;
                            }
                        labelSucursalNombre.Text = sucursalNombre;
                    }

                    Label labelClienteNombre = e.Row.FindControl("lblCliente") as Label;
                    if (labelClienteNombre != null)
                    {
                        string clienteNombre = string.Empty;
                        if (contrato.Cliente != null)
                            if (contrato.Cliente.Nombre != null)
                            {
                                clienteNombre = contrato.Cliente.Nombre;
                            }
                        labelClienteNombre.Text = clienteNombre;
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".grdContratos_RowDataBound: " + ex.Message);
            }
        }

        private void ImprimirFormatoPersonaMoral(object sender, EventArgs eventArgs)
        {
            try
            {
                presentador.ImprimirFormatoContrato(false);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconcistencia al intentar imprimir el formato.", ETipoMensajeIU.ERROR, nombreClase + ".ImprimirFormatoPersonaMoral" + ex.Message);
            }

        }
        private void ImprimirFormatoPersonaFisica_Click(object sender, EventArgs eventArgs)
        {
            try
            {
                presentador.ImprimirFormatoContrato(true);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconcistencia al intentar imprimir el formato.", ETipoMensajeIU.ERROR, nombreClase +".ImprimirFormatoPersonaFisica_Click"+ex.Message);
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

        protected void txtNombreCuentaCliente_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreCuentaCliente = NombreCuentaCliente;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                NombreCuentaCliente = nombreCuentaCliente;
                if (NombreCuentaCliente != null)
                {
                    EjecutaBuscador("CuentaClienteIdealease", ECatalogoBuscador.CuentaClienteIdealease);
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