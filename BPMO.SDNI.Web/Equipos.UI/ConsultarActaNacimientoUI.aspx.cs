//Satisface al CU079 - Consultar Acta de Nacimiento de Unidad
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.Equipos.PRE;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.UI;
using Newtonsoft.Json;
using BPMO.SDNI.Comun.PRE;

namespace BPMO.SDNI.Equipos.UI
{
    public partial class ConsultarActaNacimientoUI : System.Web.UI.Page, IConsultarActaNacimientoVIS
    {
        #region Atributos
        private ConsultarActaNacimientoPRE presentador = null;
        private string nombreClase = "ConsultarActaNacimientoUI";

        public enum ECatalogoBuscador
        {
            Unidad,
            Sucursal,
            Cliente
        }
        #endregion

        #region Propiedades
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

        public int? UsuarioAutenticado
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }
        public int? UnidadOperativaId
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }
        /// <summary>
        /// Identificador del Modulo desde el cual se esta accesando
        /// </summary>
        public int? ModuloID
        {
            get
            {
                return ((Site)Page.Master).ModuloID;
            }
        }
        /// <summary>
        /// Configuración de la unidad operativa que indica a qué libro corresponden los activos
        /// </summary>
        public string LibroActivos
        {
            get
            {
                string valor = null;
                if (this.hdnLibroActivos.Value.Trim().Length > 0)
                    valor = this.hdnLibroActivos.Value.Trim().ToUpper();
                return valor;

            }
            set
            {
                if (value != null)
                    this.hdnLibroActivos.Value = value.ToString();
                else
                    this.hdnLibroActivos.Value = string.Empty;
            }
        }

        public String NumeroVIN
        {
            get
            {
                String numeroVIN = null;
                if (this.txtNumVin.Text.Trim().Length > 0)
                    numeroVIN = this.txtNumVin.Text.Trim().ToUpper();
                return numeroVIN;

            }
            set
            {
                if (value != null)
                    this.txtNumVin.Text = value.ToString();
                else
                    this.txtNumVin.Text = String.Empty;
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

        public String NumeroEconomico
        {
            get
            {
                String numeroEconomico = null;
                if (this.txtNumeroEconomico.Text.Trim().Length > 0)
                    numeroEconomico = this.txtNumeroEconomico.Text.Trim();
                return numeroEconomico;
            }
            set
            {
                if (value != null)
                    this.txtNumeroEconomico.Text = value.ToString();
                else
                    this.txtNumeroEconomico.Text = String.Empty;
            }
        }

        public String ClienteNombre
        {
            get
            {
                String clienteNombre = null;
                if (this.txtCliente.Text.Trim().Length > 0)
                    clienteNombre = this.txtCliente.Text.Trim().ToUpper();
                return clienteNombre;
            }
            set
            {
                if (value != null)
                    this.txtCliente.Text = value.ToString();
                else
                    this.txtCliente.Text = String.Empty;
            }
        }
        public int? ClienteID
        {
            get
            {
                int? clienteID = null;
                if (!String.IsNullOrEmpty(this.hdnClienteID.Value))
                    clienteID = int.Parse(this.hdnClienteID.Value.Trim());
                return clienteID;
            }
            set
            {
                if (value != null)
                    this.hdnClienteID.Value = value.ToString();
                else
                    this.hdnClienteID.Value = string.Empty;
            }
        }

        public DateTime? FechaCompra
        {
            get
            {
                DateTime fechaCompra;
                bool esDate = DateTime.TryParse(this.txtFechaCompra.Text.Trim(), out fechaCompra);
                if (esDate == false)
                {
                    return null;
                }
                return fechaCompra;
            }
            set
            {
                if (value != null)
                    this.txtFechaCompra.Text = value.ToString();
                else
                    this.txtFechaCompra.Text = String.Empty;
            }
        }

        public int? Area
        {
            get
            {
                int? area = null;
                if (this.ddlArea.SelectedIndex > 0)
                    area = Int32.Parse(this.ddlArea.SelectedValue);
                return area;
            }
            set
            {
                if (value != null)
                    this.ddlArea.SelectedValue = value.ToString();
                else
                    this.ddlArea.SelectedIndex = 0;
            }
        }

        public int? EstatusActa
        {
            get
            {
                int? estatusActa = null;
                if (this.ddlEstatus.SelectedIndex > 0)
                    estatusActa = Int32.Parse(this.ddlEstatus.SelectedValue);
                return estatusActa;
            }
            set
            {
                if (value != null)
                    this.ddlEstatus.SelectedValue = value.ToString();
                else
                    this.ddlEstatus.SelectedIndex = 0;
            }
        }

        public List<UnidadBO> Resultado
        {
            get { return Session["listUnidades"] != null ? Session["listUnidades"] as List<UnidadBO> : null; }
            set { Session["listUnidades"] = value; }
        }
        public int IndicePaginaResultado
        {
            get { return this.grvActasNacimiento.PageIndex; }
            set { this.grvActasNacimiento.PageIndex = value; }
        }

        /// <summary>
        /// Contiene la lista de acciones a las cuales tiene acceso el usuario.
        /// </summary>
        public List<CatalogoBaseBO> ListaAcciones { get; set; }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ConsultarActaNacimientoPRE(this);
                if (!IsPostBack)
                {
                    this.presentador.PrepararBusqueda();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararBusqueda()
        {
            this.txtNumVin.Text = "";
            this.txtSucursal.Text = "";
            this.txtNumeroEconomico.Text = "";
            this.txtCliente.Text = "";
            this.txtFechaCompra.Text = "";
            this.ddlArea.SelectedIndex = 0;
            this.ddlEstatus.SelectedIndex = 0;

            this.hdnClienteID.Value = "";
            this.hdnSucursalID.Value = "";
        }

        public void ActualizarResultado()
        {
            this.grvActasNacimiento.DataSource = this.Resultado;
            this.grvActasNacimiento.DataBind();
        }

        public void EstablecerPaqueteNavegacion(string nombre, object valor)
        {
            Session[nombre] = valor;
        }
        public void RedirigirADetalles()
        {
            this.Response.Redirect(this.ResolveUrl("~/Equipos.UI/DetalleActaNacimientoUI.aspx"));
        }
        public void LimpiarSesion()
        {
            if (Session["listUnidades"] != null)
                Session.Remove("listUnidades");
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

        #region SC0008
        public void PermitirRegistrar(bool habilitar)
        {
                this.hlkRegistroActaNacimiento.Enabled = habilitar;
        }
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        #endregion

        #region REQ 13285 Métodos relacionados con las acciones dependiendo de la unidad operativa.
        /// <summary>
        /// Prepara los controles (etiquetas y visualización) que serán válidos para la unidad operativa Generación.
        /// </summary>
        /// <param name="tipoEmpresa">Indica la unidad operativa, este valor determina el comportamiento de los controles.</param>
        public void EstablecerAcciones(ETipoEmpresa tipoEmpresa)
        {
            //Declaramos e inicializamos variables
            Type TipoEnum = typeof(EArea);
            string key = string.Empty;
            int valorNum = 0;
            //Obteniendo el nombre de las etiquetas del archivo resource correspondiente.
            string VIN = ObtenerEtiquetadelResource("RE01", tipoEmpresa);
            string Economico = ObtenerEtiquetadelResource("RE04", tipoEmpresa);
            string AreaDepartamento = ObtenerEtiquetadelResource("RE34", tipoEmpresa);

            //Se válida si la variable "VIN" está vacía, si es el caso se oculta el control, en caso contrario el valor será asignado a la etiqueta lblVIN
            if (string.IsNullOrEmpty(VIN))
            {
                this.rowVIN.Visible = false;
                this.grvActasNacimiento.Columns[0].Visible = false;
            }
            else
            {
                this.lblVIN.Text = VIN;
                this.grvActasNacimiento.Columns[0].HeaderText = VIN;
            }
            
            //Se válida si la variable "Economico" está vacía,  si es el caso se oculta el control, en caso contrario el valor será asignado a la etiqueta lblEconomico
            if (string.IsNullOrEmpty(Economico))
            {
                this.rowEconomico.Visible = false;
                this.grvActasNacimiento.Columns[2].Visible = false;
            }
            else
            {
                this.lblEconomico.Text = Economico;
                this.grvActasNacimiento.Columns[2].HeaderText = Economico;
            }

            //Se válida si la variable AreaDepartamento está vacía,  si es el caso se oculta el control, en caso contrario el valor será asignado a la etiqueta lblAreaDepartamento
            if (string.IsNullOrEmpty(AreaDepartamento))
            {
                this.rowAreaDepartamento.Visible = false;
                this.grvActasNacimiento.Columns[5].Visible = false;
            }
            else
            {
                this.lblAreaDepartamento.Text = AreaDepartamento;
                this.grvActasNacimiento.Columns[5].HeaderText = AreaDepartamento;
            }

            //Se llenan los valores del DropDownList de Área/Departamento o Tipo de renta (dependiendo de la unidad operativa)
            this.ddlArea.Items.Clear();
            this.ddlArea.Items.Add(new ListItem() { Value = string.Empty, Text = "Todos" });
            switch (this.UnidadOperativaId)
            {
                case (int)ETipoEmpresa.Construccion:
                    TipoEnum = typeof(EAreaConstruccion);
                    break;
                case (int)ETipoEmpresa.Generacion:
                    TipoEnum = typeof(EAreaGeneracion);
                    break;
                case (int)ETipoEmpresa.Equinova:
                    TipoEnum = typeof(EAreaEquinova);
                    break;
            }
            foreach (var valor in Enum.GetValues(TipoEnum))
            {
                valorNum = Convert.ToInt32(valor);
                key = Enum.GetName(TipoEnum, valor);
                this.ddlArea.Items.Add(new ListItem() { Text = key, Value = valorNum.ToString() });
            }
        }

        /// <summary>
        /// Método que obtiene el nombre de la etiqueta del archivo resource correspondiente a la unidad operativa a la cual accedió el usuario.
        /// </summary>
        /// <param name="etiquetaBuscar">Nombre de la etiqueta que será buscada en el archivo resource</param>
        /// <param name="tipoEmpresa">Nombre de la unidad operativa a la cual accedió el usuario.</param>
        /// <returns>Retorna el nombre de la etiqueta correspondiente al valor recibido en el parámetro etiquetaBuscar del archivo resource.</returns>
        private string ObtenerEtiquetadelResource(string etiquetaBuscar, ETipoEmpresa tipoEmpresa)
        {
            string Etiqueta = string.Empty;
            //Instanciamos la clase o webmethod que obtiene las etiquetas
            ObtenerEtiquetaEmpresas obtenerEtiqueta = new ObtenerEtiquetaEmpresas();
            string EtiquetaObtenida = string.Empty;
            EtiquetaObtenida request = null;

            EtiquetaObtenida = obtenerEtiqueta.ObtenerEtiquetadelResource(etiquetaBuscar, (int)tipoEmpresa);
            request = JsonConvert.DeserializeObject<EtiquetaObtenida>(EtiquetaObtenida);
            if (string.IsNullOrEmpty(request.cMensaje))
            {
                EtiquetaObtenida = request.cEtiqueta;
                if (Etiqueta != "NO APLICA")
                {
                    Etiqueta = EtiquetaObtenida;
                }
            }
            return Etiqueta;
        }

        #endregion
        #endregion

        #region Eventos
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Consultar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al consultar actas de nacimiento", ETipoMensajeIU.ERROR, this.nombreClase + ".btnBuscar_Click:" + ex.Message);
            }
        }

        protected void grvActasNacimiento_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index;

            if (e.CommandName.ToString().ToUpper() == "PAGE") return;
            
            try
            {
                index = Convert.ToInt32(e.CommandArgument.ToString()) + (((GridView)sender).PageIndex * ((GridView)sender).PageSize);
                switch (e.CommandName.Trim())
                {
                    case "Detalles":              
                        this.presentador.VerDetalles(index);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al ejecutar la acción sobre el acta de nacimiento", ETipoMensajeIU.ERROR, this.nombreClase + ".grvActasNacimiento_RowCommand:" + ex.Message);
            }
        }

        protected void grvActasNacimiento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                UnidadBO unidad = (UnidadBO)e.Row.DataItem;
                Label labelFechaCompra = e.Row.FindControl("lblFechaCompra") as Label;
                if (labelFechaCompra != null)
                {
                    string fecha = string.Empty;
                    if (unidad.ActivoFijo != null)
                        if (unidad.ActivoFijo.FechaFacturaCompra != null)
                        {
                            fecha = String.Format("{0:dd/MM/yyyy}", unidad.ActivoFijo.FechaFacturaCompra);
                        }
                    labelFechaCompra.Text = fecha;
                }

                Label labelSucursalNombre = e.Row.FindControl("lblSucursal") as Label;
                if (labelSucursalNombre != null)
                {
                    string sucursalNombre = string.Empty;
                    if (unidad.Sucursal != null)
                        if (unidad.Sucursal.Nombre != null)
                        {
                            sucursalNombre = unidad.Sucursal.Nombre;
                        }
                    labelSucursalNombre.Text = sucursalNombre;
                }

                Label labelClienteNombre = e.Row.FindControl("lblCliente") as Label;
                if (labelClienteNombre != null)
                {
                    string clienteNombre = string.Empty;
                    if (unidad.Cliente != null)
                        if (unidad.Cliente.Nombre != null)
                        {
                            clienteNombre = unidad.Cliente.Nombre;
                        }
                    labelClienteNombre.Text = clienteNombre;
                }
            }
        }

        protected void grvActasNacimiento_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.presentador.CambiarPaginaResultado(e.NewPageIndex);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".grvActasNacimiento_PageIndexChanging:" + ex.Message);
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
                string serieUnidad = NumeroVIN;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Unidad);

                NumeroVIN = serieUnidad;
                if (NumeroVIN != null)
                {
                    this.EjecutaBuscador("EquipoBepensa", ECatalogoBuscador.Unidad);
                    NumeroVIN = null;
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
            if (txtNumVin.Text.Length < 1)
            {
                this.MostrarMensaje("Es necesario un número de serie.", ETipoMensajeIU.ADVERTENCIA);
                return;
            }

            try
            {
                this.EjecutaBuscador("EquipoBepensa&hidden=0", ECatalogoBuscador.Unidad);
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
        /// TextChanged activa el llamado al Buscador para la busqueda de Cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCliente_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreCliente = ClienteNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Cliente);

                ClienteNombre = nombreCliente;
                if (ClienteNombre != null)
                {
                    this.EjecutaBuscador("Cliente", ECatalogoBuscador.Cliente);
                    ClienteNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtCliente_TextChanged" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarClientes_Click(object sender, ImageClickEventArgs e)
        {
            if (txtCliente.Text.Length <1)
            {
                this.MostrarMensaje("Es necesario un nombre de cliente.", ETipoMensajeIU.ADVERTENCIA);
                return;
            }

            try
            {
                this.EjecutaBuscador("Cliente&hidden=0", ECatalogoBuscador.Cliente);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnBuscarClientes_Click" + ex.Message);
            }
        }


        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Unidad:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                    case ECatalogoBuscador.Cliente:
                    case ECatalogoBuscador.Sucursal:
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
