//Esta clase satisface los requerimientos especificados en el caso de uso CU082 – REGISTRAR MOVIMIENTO DE FLOTA
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Flota.PRE;
using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Flota.UI
{
    public partial class CambiarDepartamentoUnidadUI : System.Web.UI.Page, ICambiarDepartamentoUnidadVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador de la página
        /// </summary>
        private CambiarDepartamentoUnidadPRE presentador = null;
        /// <summary>
        /// Nombre de la clase que se usará para los mensajes de error
        /// </summary>
        private const string nombreClase = "CambiarDepartamentoUnidadUI";
        private ETipoEmpresa ETipoEmpresa;

        public enum ECatalogoBuscador
        {
            Propietario,
            Cliente
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Load de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new CambiarDepartamentoUnidadPRE(this);
                this.txtObservaciones.Attributes.Add("onkeyup", "checkText(this,350);");
                if (!Page.IsPostBack)
                {
                    //Se valida el acceso a la página
                    this.presentador.ValidarAcceso();
                    //Se prepara la página para la edición del contrato
                    this.presentador.PrepararEdicion();
                }
            }
            catch (Exception ex)
            {
                this.ReestablecerControles();
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, string.Format("{0}.Page_Load:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        #endregion        

        #region Propiedades
        /// <summary>
        /// Obtiene el identificador del modulo en el que se encuentra trabajando con el fin de obtener las configuraciones necesarias para editar el contrato
        /// </summary>
        public int? ModuloID
        {
            get
            {
                return ((Site)Page.Master).ModuloID;
            }
        }
        /// <summary>
        /// Obtiene el usuario autenticado en el sistema
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                Site masterMsj = (Site)Page.Master;

                return masterMsj != null && masterMsj.Usuario != null ? masterMsj.Usuario.Id : null;
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
                           ? masterMsj.Adscripcion.UnidadOperativa.Id
                           : null;
            }
        }
        /// <summary>
        /// Obtiene o establece el número de serie de la unidad
        /// </summary>
        public string NumeroSerie
        {
            get { return this.txtEstaticoNumSerie.Text.Trim().ToUpper(); }
            set
            {
                this.txtEstaticoNumSerie.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                    ? value.Trim().ToUpper()
                                                    : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la clave dela ctivo de oracle
        /// </summary>
        public string ClaveActivoOracle
        {
            get { return this.txtEstaticoClaveOracle.Text.Trim().ToUpper(); }
            set
            {
                this.txtEstaticoClaveOracle.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                       ? value.Trim().ToUpper()
                                                       : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador de líder
        /// </summary>
        public int? LiderID
        {
            get
            {
                int val = 0;
                if (Int32.TryParse(this.txtEstaticoIDLeader.Text, out val))
                    return val;
                return null;
            }
            set { this.txtEstaticoIDLeader.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtieen eo establece el identificador de la unidad
        /// </summary>
        public int? UnidadID
        {
            get
            {
                int val = 0;
                if (Int32.TryParse(this.hdnUnidadID.Value, out val))
                    return val;
                return null;
            }
            set
            {
                this.hdnUnidadID.Value = value.HasValue ? value.Value.ToString() : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el número económico de la unidad
        /// </summary>
        public string NumeroEconomico
        {
            get { return this.txtEstaticoNumEconomico.Text.Trim().ToUpper(); }
            set
            {
                this.txtEstaticoNumEconomico.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                        ? value.Trim().ToUpper()
                                                        : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el tipo de la unidad
        /// </summary>
        public string TipoUnidadNombre
        {
            get { return this.txtEstaticoTipoUnidad.Text.Trim().ToUpper(); }
            set
            {
                this.txtEstaticoTipoUnidad.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                      ? value.ToUpper().Trim()
                                                      : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el modelo de la unidad
        /// </summary>
        public string ModeloNombre
        {
            get { return this.txtEstaticoModelo.Text.Trim().ToUpper(); }
            set
            {
                this.txtEstaticoModelo.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                  ? value.ToUpper().Trim()
                                                  : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el año de la unidad
        /// </summary>
        public int? Anio
        {
            get
            {
                int val = 0;

                if (Int32.TryParse(this.txtEstaticoAnio.Text.Trim().ToUpper(), out val))
                    return val;
                return null;
            }
            set { this.txtEstaticoAnio.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece la fecha de compra de la unidad
        /// </summary>
        public DateTime? FechaCompra
        {
            get
            {
                DateTime val = new DateTime();
                if (DateTime.TryParse(this.txtEstaticoFechaCompra.Text, out val))
                    return val;
                return null;
            }
            set { this.txtEstaticoFechaCompra.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el monto de la factura de la unidad
        /// </summary>
        public decimal? MontoFactura
        {
            get
            {
                decimal val = 0;
                if (decimal.TryParse(this.txtEstaticoMontoFactura.Text, out val))
                    return val;
                return null;
            }
            set { this.txtEstaticoMontoFactura.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el folio de la factura de compra de la unidad
        /// </summary>
        public string FolioFactura
        {
            get { return this.txtFolioFacturaCompra.Text.Trim().ToUpper(); }
            set { this.txtFolioFacturaCompra.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value) ? value.Trim().ToUpper() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece si la unidad esta disponible
        /// </summary>
        public bool? EstaDisponible
        {
            get
            {
                bool? id = null;
                if (!String.IsNullOrEmpty(this.hdnEstaDisponible.Value))
                    id = bool.Parse(this.hdnEstaDisponible.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnEstaDisponible.Value = value.ToString();
                else
                    this.hdnEstaDisponible.Value = string.Empty;

                Image img = this.mFlota.Controls[3].FindControl("imgEstatus") as Image;
                if (value != null && value == true)
                    img.ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ESTATUS-SI-ICO.png");
                else
                    img.ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ESTATUS-NO-ICO.png");
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad se encuentra en un contrato
        /// </summary>
        public bool? EstaEnContrato
        {
            get
            {
                bool? id = null;
                if (!String.IsNullOrEmpty(this.hdnEstaEnContrato.Value))
                    id = bool.Parse(this.hdnEstaEnContrato.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnEstaEnContrato.Value = value.ToString();
                else
                    this.hdnEstaEnContrato.Value = string.Empty;

                Image img = this.mFlota.Controls[4].FindControl("imgEstatus") as Image;
                if (value != null && value == true)
                    img.ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ESTATUS-SI-ICO.png");
                else
                    img.ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ESTATUS-NO-ICO.png");
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad cuenta con equipos aliados
        /// </summary>
        public bool? TieneEquipoAliado
        {
            get
            {
                bool? id = null;
                if (!String.IsNullOrEmpty(this.hdnTieneEquipoAliado.Value))
                    id = bool.Parse(this.hdnTieneEquipoAliado.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnTieneEquipoAliado.Value = value.ToString();
                else
                    this.hdnTieneEquipoAliado.Value = string.Empty;

                Image img = this.mFlota.Controls[5].FindControl("imgEstatus") as Image;
                if (value != null && value == true)
                    img.ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ESTATUS-SI-ICO.png");
                else
                    img.ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ESTATUS-NO-ICO.png");
            }
        }
        /// <summary>
        /// Obtiene o establece el número de las placas de la unidad
        /// </summary>
        public string NumeroPlaca
        {
            get
            {
                var txt = this.mFlota.Controls[6].FindControl("txtValue") as TextBox;
                return (String.IsNullOrEmpty(txt.Text)) ? null : txt.Text.Trim().ToUpper();
            }
            set
            {
                var txt = this.mFlota.Controls[6].FindControl("txtValue") as TextBox;
                txt.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la Sucursal actual
        /// </summary>
        public int? SucursalActualID
        {
            get
            {
                int val = 0;
                if (Int32.TryParse(this.hdnSucursalID.Value, out val))
                    return val;
                return null;
            }
            set { this.hdnSucursalID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal actual
        /// </summary>
        public string SucursalActualNombre
        {
            get { return this.txtSucursal.Text.Trim().ToUpper(); }
            set
            {
                this.txtSucursal.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                  ? value.Trim().ToUpper()
                                                  : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el domicilio de la sucursal actual
        /// </summary>
        public string DomicilioSucursalActual
        {
            get { return this.txtDireccionSucursal.Text.Trim().ToUpper(); }
            set
            {
                this.txtDireccionSucursal.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value) ? value.Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la empresa actual
        /// </summary>
        public int? EmpresaActualID
        {
            get
            {
                var val = 0;
                if (Int32.TryParse(this.hdnEmpresaID.Value, out val))
                    return val;
                return null;
            }
            set { this.hdnEmpresaID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la empresa actual
        /// </summary>
        public string EmpresaActualNombre
        {
            get { return this.txtEmpresa.Text.Trim().ToUpper(); }
            set
            {
                this.txtEmpresa.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.ToUpper().Trim()
                                                 : string.Empty;
            }
        }        
        /// <summary>
        /// Obtiene o establece el identificador del departamento actual
        /// </summary>
        public int? DepartamentoActualID
        {
            get
            {
                if (this.ddlDepartamentoActual.SelectedValue.CompareTo("-1") != 0)
                {
                    int val = 0;
                    if (Int32.TryParse(this.ddlDepartamentoActual.SelectedValue, out val))
                        return val;
                }
                return null;
            }
            set { this.ddlDepartamentoActual.SelectedValue = value.HasValue ? value.ToString() : "-1"; }
        }
        /// <summary>
        /// Obtiene o establece el identificador del departamento destino
        /// </summary>
        public int? DepartamentoDestinoID
        {
            get
            {
                if (this.ddlDepartamentoDestino.SelectedValue.CompareTo("-1") != 0)
                {
                    int val = 0;
                    if (Int32.TryParse(this.ddlDepartamentoDestino.SelectedValue, out val))
                        return val;
                }
                return null;
            }
            set { this.ddlDepartamentoDestino.SelectedValue = value.HasValue ? value.ToString() : "-1"; }
        }
        /// <summary>
        /// Obtiene o establece el propietario de la unidad
        /// </summary>
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
        /// <summary>
        /// Obtiene o establece el identificador del propietario de la unidad
        /// </summary>
        public int? PropietarioID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnPropietarioID.Value))
                    id = int.Parse(this.hdnPropietarioID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnPropietarioID.Value = value.ToString();
                else
                    this.hdnPropietarioID.Value = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre del clietne de la unidad
        /// </summary>
        public string Cliente
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtCliente.Text)) ? null : this.txtCliente.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtCliente.Text = value;
                else
                    this.txtCliente.Text = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador del cliente de la unidad
        /// </summary>
        public int? ClienteID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnClienteID.Value))
                    id = int.Parse(this.hdnClienteID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnClienteID.Value = value.ToString();
                else
                    this.hdnClienteID.Value = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la unidad oeprativa del cliente
        /// </summary>
        public string NombreClienteUnidadOperativa
        {
            get
            {
                return (String.IsNullOrEmpty(this.hdnNombreClienteUnidadOperativa.Value)) ? null : this.hdnNombreClienteUnidadOperativa.Value.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.hdnNombreClienteUnidadOperativa.Value = value;
                else
                    this.hdnNombreClienteUnidadOperativa.Value = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece las observaciones d ela unidad
        /// </summary>
        public string Observaciones
        {
            get { return this.txtObservaciones.Text.Trim().ToUpper(); }
            set
            {
                this.txtObservaciones.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la referencía al objeto que se esta editando
        /// </summary>
        public object ObjetoEdicion
        {
            get
            {
                if (Session["ObjetoExpedienteEditar"] != null)
                    return Session["ObjetoExpedienteEditar"];

                return null;
            }
            set
            {
                Session["ObjetoExpedienteEditar"] = value;
            }
        }
        /// <summary>
        /// Obtiene o establece la referencía al objeto inicial que se esta editando
        /// </summary>
        public object UltimoObjeto
        {
            get
            {
                if (Session["UltimoObjetoExpediente"] != null)
                    return Session["UltimoObjetoExpediente"];

                return null;
            }
            set
            {
                Session["UltimoObjetoExpediente"] = value;
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

        #region Métodos
        public void LimpiarSesion()
        {
            Session.Remove("UltimoObjetoExpediente");
        }
        /// <summary>
        /// Elimina de la variable de session un paquete
        /// </summary>
        /// <param name="key">Clave del paquete que se desea eliminar</param>
        public void LimpiarPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) && string.IsNullOrWhiteSpace(key))
                throw new Exception(string.Format("{0}.LimpiarPaqueteNavegacion:{1}{2}", nombreClase, Environment.NewLine, "Es necesario especificar la clave del paquete que se desea eliminar"));

            Session.Remove(key);
        }
        /// <summary>
        /// Guarda en la variable de session un paquete para su uso en otra página
        /// </summary>
        /// <param name="key">Clave del paquete de navegación</param>
        /// <param name="value">Paquete que se desea subir a la variable de session</param>
        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: El paquete de navegación no puede tener una llave nula o vacía.");
            if (value == null)
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: El paquete de navegación no puede ser nulo.");

            Session[key] = value;
        }
        /// <summary>
        /// Recupera un paquete de navegación de la variable de session
        /// </summary>
        /// <param name="key">Clave del paquete de navegación que de desea obtener</param>
        /// <returns>Paquete de navegación recueprado</returns>
        public object ObtenerPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) && string.IsNullOrWhiteSpace(key))
                throw new Exception(string.Format("{0}.ObtenerPaqueteNavegacion:{1}{2}", nombreClase, Environment.NewLine, "Es necesario especificar la clave del paquete que se desea recuperar"));

            return Session[key];
        }
        /// <summary>
        /// Redirige a la página de detalle de la unidad
        /// </summary>
        public void RedirigirADetalles()
        {
            this.Response.Redirect(this.ResolveUrl("~/Flota.UI/DetalleExpedienteUnidadUI.aspx"), false);
        }
        /// <summary>
        /// Redirige a la página de permiso denegado
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.LimpiarSesion();
            this.LimpiarPaqueteNavegacion("UnidadExpedienteBO");
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"), false);
        }
        /// <summary>
        /// Habilita o deshabilita los controles de consulta
        /// </summary>
        /// <param name="status">Estatus que se desea proporcionar a los controles</param>
        public void PermitirConsultar(bool status)
        {
            this.hlkConsultar.Enabled = status;
        }
        /// <summary>
        /// Habilita o deshabilita los controles de registro
        /// </summary>
        /// <param name="status">Estatus que se desea aplicar a los controles</param>
        public void PermitirRegistrar(bool status)
        {
            this.btnGuardar.Enabled = status;
        }
        /// <summary>
        /// Despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)Page.Master;
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
        /// Reestablece los controles a la configuración inicial
        /// </summary>
        private void ReestablecerControles()
        {
            this.btnCancelar.Enabled = true;
            this.btnGuardar.Enabled = false;
        }
        /// <summary>
        /// Carga la lista de opciones configuradas para los departamentos
        /// </summary>
        /// <param name="items">Departamentos en los que puede estar una unidad</param>
        public void CargarDepartamentos(Dictionary<int, string> items)
        {
            this.ddlDepartamentoActual.Items.Clear();
            this.ddlDepartamentoActual.DataSource = items;
            this.ddlDepartamentoActual.DataTextField = "Value";
            this.ddlDepartamentoActual.DataValueField = "Key";
            this.ddlDepartamentoActual.DataBind();
            this.ddlDepartamentoActual.SelectedValue = "-1";

            this.ddlDepartamentoDestino.Items.Clear();
            this.ddlDepartamentoDestino.DataSource = items;
            this.ddlDepartamentoDestino.DataTextField = "Value";
            this.ddlDepartamentoDestino.DataValueField = "Key";
            this.ddlDepartamentoDestino.DataBind();
            this.ddlDepartamentoDestino.SelectedValue = "-1";
        }
        public void HabilitarPropietario(bool habilitar)
        {
            this.txtPropietario.Enabled = habilitar;
            this.ibtnBuscaPropietario.Enabled = habilitar;
        }
        public void HabilitarCliente(bool habilitar)
        {
            this.txtCliente.Enabled = habilitar;
            this.ibtnBuscaCliente.Enabled = habilitar;
        }
        /// <summary>
        /// Prepara la página para el registro de
        /// </summary>
        public void  PrepararEdicion()
        {
            this.ddlDepartamentoActual.Enabled = true;

            #region Establecer la opción para cambiar de departamento en Generación y Construcción
            //1. RE puede cambiar a RO y ROC   2. RO solo a ROC   3. ROC solo a RO
            switch (this.UnidadOperativaID)
            {
                case (int)ETipoEmpresa.Generacion:
                    if (this.ddlDepartamentoActual.SelectedValue == "10") {
                        this.ddlDepartamentoDestino.SelectedValue = "11";
                        this.ddlDepartamentoDestino.Enabled = false;
                    } else if (this.ddlDepartamentoActual.SelectedValue == "11") {
                        this.ddlDepartamentoDestino.SelectedValue = "10";
                        this.ddlDepartamentoDestino.Enabled = false;
                    } else {
                        this.ddlDepartamentoDestino.Enabled = true;
                    }
                    break;
                case (int)ETipoEmpresa.Construccion:
                    if (this.ddlDepartamentoActual.SelectedValue == "20") {
                        this.ddlDepartamentoDestino.SelectedValue = "21";
                        this.ddlDepartamentoDestino.Enabled = false;
                    } else if (this.ddlDepartamentoActual.SelectedValue == "21") {
                        this.ddlDepartamentoDestino.SelectedValue = "20";
                        this.ddlDepartamentoDestino.Enabled = false;
                    } else {
                        this.ddlDepartamentoDestino.Enabled = true;
                    }
                    break;
                case (int)ETipoEmpresa.Equinova:
                    if (this.ddlDepartamentoActual.SelectedValue == "30") {
                        this.ddlDepartamentoDestino.SelectedValue = "31";
                        this.ddlDepartamentoDestino.Enabled = false;
                    } else if (this.ddlDepartamentoActual.SelectedValue == "31") {
                        this.ddlDepartamentoDestino.SelectedValue = "30";
                        this.ddlDepartamentoDestino.Enabled = false;
                    } else {
                        this.ddlDepartamentoDestino.Enabled = true;
                    }
                    break;
                case (int)ETipoEmpresa.Idealease:
                default:
                    this.ddlDepartamentoDestino.SelectedValue = "-1";
                    this.ddlDepartamentoDestino.Enabled = true;
                    break;
            }
            #endregion
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
            this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "', '" + this.btnResult.ClientID + "');");
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
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Cancelar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al cancelar el movimiento de la sucursal", ETipoMensajeIU.ERROR, string.Format("{0}.btnCancelar_Click:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ActualizarUnidadDepartamento();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al guardar el movimiento de la sucursal", ETipoMensajeIU.ERROR, string.Format("{0}.btnGuardar_Click:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }

        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Propietario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtPropietario_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string propietario = this.Propietario;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Propietario);

                this.Propietario = propietario;
                if (this.Propietario != null)
                {
                    this.EjecutaBuscador("Cliente", ECatalogoBuscador.Propietario);
                    this.Propietario = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Propietario", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtPropietario_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Propietario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaPropietario_Click(object sender, ImageClickEventArgs e)
        {
            string s;
            if ((s = this.presentador.ValidarCamposConsultaPropietario()) != null)
            {
                this.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try
            {
                this.EjecutaBuscador("Cliente&hidden=0", ECatalogoBuscador.Propietario);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscaPropietario_Click:" + ex.Message);
            }
        }
        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCliente_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string cliente = this.Cliente;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Cliente);

                this.Cliente = cliente;
                if (this.Cliente != null)
                {
                    this.EjecutaBuscador("Cliente", ECatalogoBuscador.Cliente);
                    this.Cliente = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtCliente_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaCliente_Click(object sender, ImageClickEventArgs e)
        {
            string s;
            if ((s = this.presentador.ValidarCamposConsultaCliente()) != null)
            {
                this.MostrarMensaje(s, ETipoMensajeIU.INFORMACION, null);
                return;
            }

            try
            {
                this.EjecutaBuscador("Cliente&hidden=0", ECatalogoBuscador.Cliente);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscaCliente_Click:" + ex.Message);
            }
        }

        protected void ddlDepartamentoDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int? valor = null;

                if (this.ddlDepartamentoDestino.SelectedIndex > 0)
                    valor = int.Parse(this.ddlDepartamentoDestino.SelectedValue);

                this.presentador.SeleccionarArea(valor);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al seleccionar el área o departamento", ETipoMensajeIU.ERROR, nombreClase + ".ddlDepartamentoDestino_SelectedIndexChanged:" + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Propietario:
                    case ECatalogoBuscador.Cliente:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click:" + ex.Message);
            }
        }
        #endregion
    }
}