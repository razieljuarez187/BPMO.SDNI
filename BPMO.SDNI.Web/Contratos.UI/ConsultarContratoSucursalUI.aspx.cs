using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Contratos.VIS;
using BPMO.SDNI.Contratos.PRE;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using System.Collections;

namespace BPMO.SDNI.Contratos.UI
{
    public partial class ConsultarContratoSucursalUI : Page, IConsultarContratoSucursalVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador de Consultar Contratos
        /// </summary>
        private ConsultarContratoSucursalPRE presentador;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private static readonly string nombreClase = typeof(ConsultarContratoSucursalUI).Name;

        /// <summary>
        /// Enumerador de Catalogos para el Buscador
        /// </summary>
        public enum ECatalogoBuscador
        {
            CuentaClienteIdealease = 0,
            Sucursal = 1
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
        /// Obtiene el identificador del usuario que ha iniciado sesion
        /// </summary>
        public int? UsuarioID { 
            get
            {
                var masterMsj = (Site) Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null && masterMsj.Usuario.Id != null)
                    return masterMsj.Usuario.Id;
                return null;
            }
        }
        /// <summary>
        /// Obtiene el identificador de la unidad operativa correspondiente al usuario
        /// </summary>
        public int? UnidadOperativaID
        {
            get
            {
                var masterMsj = (Site)Page.Master;

                if(masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null && masterMsj.Adscripcion.UnidadOperativa.Id != null))
                    return masterMsj.Adscripcion.UnidadOperativa.Id;
                return null;
            }
        }
        /// <summary>
        /// Unidad Operativa
        /// </summary>
        public UnidadOperativaBO UnidadOperativa
        {
            get
            {
                var masterMsj = (Site)Page.Master;

                if(masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    return new UnidadOperativaBO { Id = masterMsj.Adscripcion.UnidadOperativa.Id };
                return null;
            }
        }
        /// <summary>
        /// Identificador de la Sucursal
        /// </summary>
        public Int32? SucursalId
        {
            get
            {
                int? sucursalID = null;
                if(!String.IsNullOrEmpty(this.hdnSucursalID.Value))
                    sucursalID = int.Parse(this.hdnSucursalID.Value.Trim());
                return sucursalID;
            }
            set
            {
                if(value != null)
                    this.hdnSucursalID.Value = value.ToString();
                else
                    this.hdnSucursalID.Value = string.Empty;
            }
        }
        /// <summary>
        /// Nombre de la Sucursal
        /// </summary>
        public String SucursalNombre
        {
            get
            {
                String sucursalNombre = null;
                if(this.txtSucursal.Text.Trim().Length > 0)
                    sucursalNombre = this.txtSucursal.Text.Trim().ToUpper();
                return sucursalNombre;
            }
            set
            {
                if(value != null)
                    this.txtSucursal.Text = value.ToString();
                else
                    this.txtSucursal.Text = String.Empty;
            }
        }
        /// <summary>
        /// Identificador del Cliente
        /// </summary>
        public Int32? ClienteId
        {
            get
            {
                int? clientId = null;
                if(!String.IsNullOrEmpty(this.hdnClientID.Value))
                    clientId = int.Parse(this.hdnClientID.Value.Trim());
                return clientId;
            }
            set
            {
                if(value != null)
                    this.hdnClientID.Value = value.ToString();
                else
                    this.hdnClientID.Value = string.Empty;
            }
        }
        /// <summary>
        /// Nombre del Cliente
        /// </summary>
        public String ClienteNombre
        {
            get
            {
                String clienteNombre = null;
                if(this.txtNombreCuentaCliente.Text.Trim().Length > 0)
                    clienteNombre = this.txtNombreCuentaCliente.Text.Trim().ToUpper();
                return clienteNombre;
            }
            set
            {
                if(value != null)
                    this.txtNombreCuentaCliente.Text = value.ToString();
                else
                    this.txtNombreCuentaCliente.Text = String.Empty;
            }
        }
        /// <summary>
        /// Identificador del Cotrato
        /// </summary>
        public Int32? ContratoId
        {
            get
            {
                int? contratoId = null;
                if(!String.IsNullOrEmpty(this.hdnContratoId.Value) && !String.IsNullOrWhiteSpace(this.hdnContratoId.Value))
                    contratoId = Int32.Parse(this.hdnContratoId.Value);
                return contratoId;
            }
            set
            {
                this.hdnContratoId.Value = value != null ? value.ToString() : String.Empty;
            }
        }
        /// <summary>
        /// Numero del Contrato
        /// </summary>
        public String NumeroContrato
        {
            get { return string.IsNullOrEmpty(txtNumeroContrato.Text.Trim()) ? null : txtNumeroContrato.Text.Trim(); }
            set { txtNumeroContrato.Text = value ?? string.Empty; }
        }
        /// <summary>
        /// Tipo del Contrato: RD/FSL/CM/SD
        /// </summary>
        public ETipoContrato? TipoContrato
        {
            get
            {
                ETipoContrato? tipoContrato = null;
                if(!String.IsNullOrEmpty(this.hdnTipoContrato.Value) && !String.IsNullOrWhiteSpace(this.hdnTipoContrato.Value))
                    tipoContrato = (ETipoContrato?)Int32.Parse(this.hdnTipoContrato.Value);
                return tipoContrato;
            }
            set
            {
                this.hdnTipoContrato.Value = value != null ? ((Int32)value).ToString() : String.Empty;
            }
        }
        /// <summary>
        /// Contratos Encontrados en la Consulta
        /// </summary>
        public List<ContratoBO> ContratosEncontrados
        {
            get
            {
                object contratos = Session["ContratosEncontrados"];
                if(contratos !=  null)
                    return (List<ContratoBO>)contratos;
                return null;
            }
            set
            {
                if(value != null) Session["ContratosEncontrados"] = value;
                else
                    Session.Remove("ContratosEncontrados");
            }
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje que es desplegado</param>
        /// <param name="tipo">Tipo del mensaje que es desplegao</param>
        /// <param name="detalle">Detalle del mensaje que es desplegado</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)Master;
            if(tipo == ETipoMensajeIU.ERROR)
            {
                if(master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            }
            else
            {
                if(master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        /// <summary>
        /// Limpia la sesion de la página
        /// </summary>
        public void LimpiarSesion()
        {
            this.Session.Remove("ContratosEncontrados");
        }
        /// <summary>
        /// Redirige a la Información a Detalle del Contrato
        /// </summary>
        public void RedirigirADetalle()
        {
            const string Url = "CambiarContratoSucursalUI.aspx";
            Response.Redirect(Url, true);
        }
        /// <summary>
        /// Redirige a la página de acceso denegado
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"), true);
        }
        /// <summary>
        /// Determina el Paquete que sera enviado a la interfaz de detalle
        /// </summary>
        /// <param name="nombre">Nombre del paquete</param>
        /// <param name="valor">Objeto que sera enviado</param>
        public void EstablecerPaqueteNavegacion(string nombre, object valor)
        {
            if(valor != null) Session[nombre] = valor;
            else
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: No se Proporciono el Contrato al cual se le cambiara la sucursal");
        }
        /// <summary>
        /// Presenta en la interfaz los contratos Consultados
        /// </summary>
        /// <param name="contratos">Lista de Cotratos Consultados</param>
        public void PresentarResultadoConsulta(List<ContratoBO> contratos)
        {
           ContratosEncontrados = contratos;
            this.grdContratos.DataSource = contratos;
            this.grdContratos.DataBind();
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
        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ConsultarContratoSucursalPRE(this);
                if(!IsPostBack)
                {
                    presentador.PrepararBusqueda();
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion
        #region Eventos
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.Consultar();
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("Inconcistencia al consultar los contratos", ETipoMensajeIU.ERROR, nombreClase + ".btnBuscar_Click" + ex.Message);
            }
        }
        protected void grdContratos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdContratos.DataSource = ContratosEncontrados;
                grdContratos.PageIndex = e.NewPageIndex;
                grdContratos.DataBind();
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdContratos_PageIndexChanging: " + ex.Message);
            }
        }
        protected void grdContratos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch(e.CommandName.Trim())
                {
                    case "Detalles":
                        {

                            int? ContratoID = (e.CommandArgument != null) ? (int?)Convert.ToInt32(e.CommandArgument) : null;
                            this.ContratoId = ContratoID;

                            ContratoBO contrato = ContratosEncontrados.FirstOrDefault(x => x.ContratoID == ContratoID);
                            if(contrato != null)
                                this.TipoContrato = contrato.Tipo;

                            presentador.RedirigirADetalle();
                            break;
                        }
                    case "Page":
                        break;
                    default:
                        {
                            MostrarMensaje("Comando no encontrado", ETipoMensajeIU.ERROR, "El comando no está especificado en el sistema");
                            break;
                        }
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al intentar realizar una acción sobre un contrato:", ETipoMensajeIU.ERROR, nombreClase + ".grdContratos_RowCommand: " + ex.Message);
            }
        }
        protected void grdContratos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if(e.Row.RowType == DataControlRowType.DataRow)
                {
                    ContratoBO contrato = (ContratoBO)e.Row.DataItem;

                    Label labelSucursalNombre = e.Row.FindControl("lblSucursal") as Label;
                    if(labelSucursalNombre != null)
                    {
                        string sucursalNombre = string.Empty;
                        if(contrato.Sucursal != null)
                            if(contrato.Sucursal.Nombre != null)
                            {
                                sucursalNombre = contrato.Sucursal.Nombre;
                            }
                        labelSucursalNombre.Text = sucursalNombre;
                    }

                    Label labelClienteNombre = e.Row.FindControl("lblCliente") as Label;
                    if(labelClienteNombre != null)
                    {
                        string clienteNombre = string.Empty;
                        if(contrato.Cliente != null)
                            if(contrato.Cliente.Nombre != null)
                            {
                                clienteNombre = contrato.Cliente.Nombre;
                            }
                        labelClienteNombre.Text = clienteNombre;
                    }
                }
            }
            catch(Exception ex)
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
                if(SucursalNombre != null)
                {
                    this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                    SucursalNombre = null;
                }
            }
            catch(Exception ex)
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
            catch(Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarSucursal_Click" + ex.Message);
            }
        }
        protected void txtNombreCuentaCliente_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreCuentaCliente = ClienteNombre;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                ClienteNombre = nombreCuentaCliente;
                if(ClienteNombre != null)
                {
                    EjecutaBuscador("CuentaClienteIdealease", ECatalogoBuscador.CuentaClienteIdealease);
                }
            }
            catch(Exception ex)
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
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarCliente_Click:" + ex.Message);
            }
        }
        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch(ViewState_Catalogo)
                {
                    case ECatalogoBuscador.CuentaClienteIdealease:
                    case ECatalogoBuscador.Sucursal:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click: " + ex.Message);
            }
        }
        #endregion
        #endregion
    }
}