// Satisface al Caso de uso CU003 - Consultar Contrato Renta Diaria
// Satisface al Caso de uso CU014 - Imprimir Contrato de Renta Diaria
// Satisface al CU012 - Imprimir Check List de Entrega Recepción de Unidad
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.RD.BOF;
using BPMO.SDNI.Contratos.RD.PRE;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.RD.UI
{
    public partial class ConsultarContratoRDUI : System.Web.UI.Page, IConsultarContratoRDVIS
    {
        #region Atributos

        /// <summary>
        /// Presentador de Consultar Contratos
        /// </summary>
        private ConsultarContratoRDPRE presentador;


        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "ConsultarContratosRDUI";

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
        /// Fecha de Inicio del Contrato
        /// </summary>
        public DateTime? FechaInicioContrato
        {
            get
            {
                return !string.IsNullOrEmpty(txtFechaInicioContrato.Text.Trim())
                           ? (DateTime?)DateTime.Parse(txtFechaInicioContrato.Text.Trim())
                           : null;
            }
            set
            {
                #region SC0020
                if (value != null) txtFechaInicioContrato.Text = value.Value.ToString("dd/MM/yyyy");
                #endregion SC0020
            }
        }

        /// <summary>
        /// Fecha de Fin del Contrato
        /// </summary>
        public DateTime? FechaFinContrato
        {
            get
            {
                return !string.IsNullOrEmpty(txtFechaFinContrato.Text.Trim())
                           ? (DateTime?)DateTime.Parse(txtFechaFinContrato.Text.Trim())
                           : null;
            }
            set
            {
                #region SC0020
                if (value != null) txtFechaFinContrato.Text = value.Value.ToString("dd/MM/yyyy");
                #endregion SC0020
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

            #region SC0020

            set
            {
                if (value != null)
                {
                    int valueIndex = (int)value.Value;
                    //ddlEstatus.SelectedIndex = valueIndex;
                    //ddlEstatus.Items.FindByValue(valueIndex.ToString()).Selected = true;
                    ddlEstatus.SelectedIndex = ddlEstatus.Items.IndexOf(ddlEstatus.Items.FindByValue(valueIndex.ToString())); 
                }
            }

            #endregion SC0020
        }

        /// <summary>
        /// Numero de serie
        /// </summary>
        public string NumeroSerie
        {
            get { return string.IsNullOrEmpty(txtNumeroSerie.Text.Trim()) ? null : txtNumeroSerie.Text.Trim(); }
            set { txtNumeroSerie.Text = value ?? string.Empty; }
        }
        /// <summary>
        /// Identificador de la unidad
        /// </summary>
        public int? UnidadID {
            get {
                return !string.IsNullOrEmpty(hdnUnidadId.Value.Trim())
                           ? (int?)int.Parse(hdnUnidadId.Value.Trim())
                           : null;
            }
            set {
                hdnUnidadId.Value = value != null ? value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            }
        }
        /// <summary>
        /// Numero economico
        /// </summary>
        public string NumeroEcononomico
        {
            get { return string.IsNullOrEmpty(txtNumeroEconomico.Text.Trim()) ? null : txtNumeroEconomico.Text.Trim(); }
            set { txtNumeroEconomico.Text = value ?? string.Empty; }
        }

        /// <summary>
        /// Contratos Encontrados en la Consulta
        /// </summary>
        public List<ContratoRDBOF> ContratosEncontrados
        {
            get { return Session["ContratosEncontrados"] as List<ContratoRDBOF>; }
        }

        /// <summary>
        /// Usuario del Sistema
        /// </summary>
        public UsuarioBO Usuario
        {
            get
            {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario;
                return null;
            }
        }

        /// <summary>
        /// Nombre de la Sucursal
        /// </summary>
        public String SucursalNombre
        {
            get {
                String sucursalNombre = null;
                if (ddlSucursales.SelectedValue != "0")
                    sucursalNombre = ddlSucursales.SelectedItem.Text;
                return sucursalNombre;
            }            
        }

        /// <summary>
        /// Identificador de la sucursal
        /// </summary>
        public int? SucursalID
        {
            get
            {
                int? sucursalID = null;
                if (ddlSucursales.SelectedValue != "0")
                    sucursalID = int.Parse(ddlSucursales.SelectedValue);
                return sucursalID;
            }
        }

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
        #region SC051
        /// <summary>
        /// Obtiene o estable la lista de sucursales a las que el usaurio autenticado tiene permiso de acceder
        /// </summary>
        public List<SucursalBO> SucursalesAutorizadas {
            get {
                if (Session["SucursalesAutorizadas"] != null)
                    return Session["SucursalesAutorizadas"] as List<SucursalBO>;

                return null;
            }
            set {
                if (value != null && value.Any()) {
                    Session["SucursalesAutorizadas"] = value;
                } else {
                    Session.Remove("SucursalesAutorizadas");
                    this.ddlSucursales.Enabled = false;
                }
            }
        }

        #endregion
   
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ucHerramientas.Presentador = new ucHerramientasRDPRE(ucHerramientas);
                presentador = new ConsultarContratoRDPRE(this, ucHerramientas.Presentador);
                if (!IsPostBack){
                    presentador.PrepararBusqueda();                   
                }
                //CU014
                ucHerramientas.ImprimirPlantilla = ImprimirPlantillaContrato_Click;
                //CU012
                ucHerramientas.ImprimirPlantillaCheckList = ImprimirPlantillaCheckList_Click;
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
        /// Limpia el grid de contratos encontrados
        /// </summary>
        public void LimpiarContratosEncontrados()
        {
            Session.Remove("ContratoRDBO");
            Session.Remove("ContratosEncontrados");
            grdContratos.DataSource = new List<ContratoRDBOF>();
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
        /// Carga y despliega en el grid los contratos encontrados
        /// </summary>
        /// <param name="contratos">Listado de Contratos encontrados</param>
        public void CargarContratosEncontrados(List<ContratoRDBOF> contratos)
        {
            List<ContratoRDBOF> Lista = contratos ?? new List<ContratoRDBOF>();

            Session["ContratosEncontrados"] = Lista;

            grdContratos.DataSource = Lista;
            grdContratos.DataBind();
        }

        /// <summary>
        /// Despliega el Detalle del Contrato Seleccionado
        /// </summary>
        public void IrADetalle()
        {
            const string Url = "DetalleContratoRDUI.aspx";
            Response.Redirect(Url);
        }

        #region SC0020

        /// <summary>
        /// Establece el Paquete de Navegacion para el Detalle del Contrato Seleccionado
        /// </summary>
        /// <param name="Clave">Clave del Paquete</param>
        /// <param name="ContratoID">Identificador del Contrato Seleccionado</param>
        public void EstablecerPaqueteNavegacion(string ClaveContrato, ContratoRDBOF contrato, Dictionary<string, object> elementosFiltro)
        {
            if (contrato != null) Session[ClaveContrato] = contrato;
            else
            {
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: El contrato proporcionado no pertence al listado de contratos encontrados.");
            }

            Session["FiltroConsultaContratoRD"] = elementosFiltro;
        }

        public object ObtenerPaqueteNavegacion()
        {
            if (Session["FiltroConsultaContratoRD"] != null)
                return Session["FiltroConsultaContratoRD"] as object;
            return null;
        }

        public void LimpiarPaqueteNavegacion()
        {
            Session.Remove("FiltroConsultaContratoRD");
        }

        public void EstablecerPagResultados(int indice)
        {
            this.grdContratos.PageIndex = indice;
            this.grdContratos.DataBind();
        }

        public int ObtenerPagResultados()
        {
            return this.grdContratos.PageIndex;
        }

        #endregion SC0020

        /// <summary>
        /// Establece el Paquete de Navegacion para las impresiones
        /// </summary>
        /// <param name="codigoNavegacion"></param>
        /// <param name="DatosReporte"></param>
        public void EstablecerPaqueteNavegacionImprimir(string codigoNavegacion, Dictionary<string, object> DatosReporte)
        {
            Session["NombreReporte"] = codigoNavegacion;
            Session["DatosReporte"] = DatosReporte;
        }
        /// <summary>
        /// Despliega el visor de los formatos a imprimir
        /// </summary>
        public void IrAImprimir()
        {
            const string Url = "../Buscador.UI/VisorReporteUI.aspx";
            Response.Redirect(Url, true);
        }

        /// <summary>
        /// Bloquea los campos de consulta
        /// </summary>
        public void BloquearConsulta()
        {
            
            this.ddlEstatus.Enabled = false;
            this.txtFechaInicioContrato.Enabled = false;
            this.txtFechaFinContrato.Enabled = false;
            this.txtNombreCuentaCliente.Enabled = false;
            this.txtNumeroContrato.Enabled = false;
            this.txtNumeroEconomico.Enabled = false;
            this.txtNumeroSerie.Enabled = false;
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

        public void PermitirRegistrar(bool status)
        {
            this.hlRegistroOrden.Enabled = status;
        }

        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        
        #region SC_0038
        public void CargarArchivos(List<object> resultado)
        {
            this.ucHerramientas.Presentador.CargarArchivos(resultado);
        }

        public List<object> ObtenerPlantillas(string key)
        {
            return (List<object>)Session[key];
        }
        #endregion

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
        #endregion

        #region Eventos

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try{
                bool consultar = false;
                RequeridoFin.Visible = false;
                RequeridoInicio.Visible = false;
                if (string.IsNullOrEmpty(this.NumeroContrato)) {
                    if (FechaInicioContrato == null)
                        RequeridoInicio.Visible = true;
                    if (FechaFinContrato == null)
                        RequeridoFin.Visible = true;
                    if (FechaInicioContrato != null && FechaFinContrato != null)
                        if (FechaFinContrato < FechaInicioContrato)
                            RequeridoFin.Visible = true;
                        else
                            consultar = true;
                } else
                    consultar = true;

                if (consultar) 
                    presentador.ConsultarContratos();
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
                    ContratoRDBOF contrato = (ContratoRDBOF)e.Row.DataItem;
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

                    Label lblFechaFinContrato = e.Row.FindControl("lblFechaCierreContrato") as Label;
                    if (lblFechaFinContrato != null)
                    {
                        string fechaFin = string.Empty;
                        if (contrato.FechaFin != null)
                        {
                            fechaFin = String.Format("{0:dd/MM/yyyy}", contrato.FechaFin);
                        }
                        lblFechaFinContrato.Text = fechaFin;
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
        protected void ImprimirPlantillaContrato_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.ImprimirPlantillaContratoRD();

            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al intentar imprimir el contrato", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Genera el reporte con la plantilla para el Check List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImprimirPlantillaCheckList_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.ImprimirPlantillaCheckListRD();

            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al intentar imprimir el contrato", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        #region Eventos para el Buscador
       

        protected void txtNumeroSerie_TextChanged(object sender, EventArgs e) {
            try {
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
            catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNumeroSerie_TextChanged: " + ex.Message);
            }
        }

        protected void btnBuscarVin_Click(object sender, ImageClickEventArgs e) {
            try {
                bool limpiar = false;
                if (hdnBuscador.Value.Equals("1") && string.IsNullOrEmpty(this.NumeroSerie))
                    limpiar = true;
                else
                    if (hdnBuscador.Value.Equals("1"))
                        this.EjecutaBuscador("UnidadIdealeaseSimple&hidden=0", ECatalogoBuscador.UnidadIdealease);
                if (hdnBuscador.Value.Equals("1") || limpiar) {
                    this.NumeroSerie = null;                   
                }
                hdnBuscador.Value = "";
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarVin_Click" + ex.Message);
            }
        }

        protected void txtNombreCuentaCliente_TextChanged(object sender, EventArgs e) {
            try {
                string nombreCuentaCliente = NombreCuentaCliente;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                NombreCuentaCliente = nombreCuentaCliente;
                if (NombreCuentaCliente != null)
                {
                    EjecutaBuscador("CuentaClienteIdealeaseSimple", ECatalogoBuscador.CuentaClienteIdealease);
                    NombreCuentaCliente = null;
                }
            }
            catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNombreCuentaCliente_TextChanged:" + ex.Message);
            }
        }
        protected void ibtnBuscarCliente_Click(object sender, EventArgs e) {
            try {
                bool limpiar = false;
                if (hdnBuscador.Value.Equals("1") && string.IsNullOrEmpty(this.NombreCuentaCliente))
                    limpiar = true;
                else
                    if (hdnBuscador.Value.Equals("1"))
                        EjecutaBuscador("CuentaClienteIdealeaseSimple&&hidden=0", ECatalogoBuscador.CuentaClienteIdealease);
                if (hdnBuscador.Value.Equals("1") || limpiar) {
                    this.NombreCuentaCliente = null;
                }
                hdnBuscador.Value = "";
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