﻿//Satisface al CU082 - Registrar Movimiento de Flota
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Flota.PRE;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Flota.VIS;

namespace BPMO.SDNI.Flota.UI
{
    public partial class CambiarAsignacionEquipoAliadoUI : System.Web.UI.Page, ICambiarAsignacionEquipoAliadoVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador de la página
        /// </summary>
        private CambiarAsignacionEquipoAliadoPRE presentador = null;
        /// <summary>
        /// Nombre de la clase que se usará para los mensajes de error
        /// </summary>
        private const string nombreClase = "CambiarAsignacionEquipoAliadoUI";
        /// <summary>
        /// Enumeración que se usa en el buscador
        /// </summary>
        public enum ECatalogoBuscador
        {
            EquipoAliado
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
                this.presentador = new CambiarAsignacionEquipoAliadoPRE(this);
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
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                    return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);

                return null;
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
        /// Obtiene o establece las observaciones realizadas
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
        /// <summary>
        /// Obtiene o establece el número de serie del equipo aliado
        /// </summary>
        public string EquipoAliadoNumeroSerie
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtEquipoAliado.Text)) ? null : this.txtEquipoAliado.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtEquipoAliado.Text = value;
                else
                    this.txtEquipoAliado.Text = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establec el identificador del equipo aliado
        /// </summary>
        public int? EquipoAliadoID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnEquipoAliadoID.Value))
                    id = int.Parse(this.hdnEquipoAliadoID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnEquipoAliadoID.Value = value.ToString();
                else
                    this.hdnEquipoAliadoID.Value = string.Empty;
            }
        }
        /// <summary>
        /// Equipos aliados asociados a la unidad
        /// </summary>
        public List<object> EquiposAliados
        {
            get
            {
                if ((List<object>)Session["ListaEquiposAliados"] == null)
                    return new List<EquipoAliadoBO>().ConvertAll(x => (object)x);
                else
                    return (List<object>)Session["ListaEquiposAliados"];
            }
            set
            {
                Session["ListaEquiposAliados"] = value;
            }
        }

        #region Propiedades para el buscador
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

        /// <summary>
        /// Obtiene o establece si la lista de equipos aliados ha sufrido algun cambio
        /// </summary>
        public bool? CambiosEquipos
        {
            get
            {
                bool val = false;
                if (bool.TryParse(this.hdnCambiosEquipoAliado.Value, out val))
                    return val;
                return null;
            }
            set
            {
                this.hdnCambiosEquipoAliado.Value = value.HasValue ? value.Value.ToString() : string.Empty;
            }              
        }
        #endregion
        
        #region Métodos
        public void LimpiarSesion()
        {
            Session.Remove("UltimoObjetoExpediente");
            Session.Remove("ObjetoExpedienteEditar");
            Session.Remove("ListaEquiposAliados");
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

        public void ActualizarEquiposAliados()
        {
            this.grvEquiposAliados.DataSource = this.EquiposAliados;
            this.grvEquiposAliados.DataBind();
        }

        public void PrepararNuevoEquipoAliado()
        {
            this.txtEquipoAliado.Text = "";
            this.hdnEquipoAliadoID.Value = "";
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
            this.btnGuardar.Enabled = true;
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
                this.MostrarMensaje("Inconsistencias al cancelar la baja de la unidad", ETipoMensajeIU.ERROR, string.Format("{0}.btnCancelar_Click:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ActualizarAsignacionEquiposAliados();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al guardar la baja de la unidad", ETipoMensajeIU.ERROR, string.Format("{0}.btnGuardar_Click:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AgregarEquipoAliado();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al agregar el equipo aliado a la unidad", ETipoMensajeIU.ERROR, nombreClase + ".btnAgregar_Click:" + ex.Message);
            }
        }

        protected void grvEquiposAliados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index;

            if (e.CommandName.ToString().ToUpper() == "PAGE") return;

            try
            {
                index = Convert.ToInt32(e.CommandArgument.ToString());
                switch (e.CommandName.ToString())
                {
                    case "Eliminar":
                        this.presentador.QuitarEquipoAliado(index);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al ejecutar la acción sobre el equipo aliado", ETipoMensajeIU.ERROR, nombreClase + ".grvEquiposAliados_RowCommand:" + ex.Message);
            }
        }
        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Equipo Aliado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtEquipoAliado_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string numeroSerie = this.EquipoAliadoNumeroSerie;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.EquipoAliado);

                this.EquipoAliadoNumeroSerie = numeroSerie;
                if (this.EquipoAliadoNumeroSerie != null)
                {
                    this.EjecutaBuscador("EquipoAliado", ECatalogoBuscador.EquipoAliado);
                    this.EquipoAliadoNumeroSerie = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Tipo de Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtEquipoAliado_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Tipo de Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaEquipoAliado_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("EquipoAliado&hidden=0", ECatalogoBuscador.EquipoAliado);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Equipo Aliado", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscaEquipoAliado_Click:" + ex.Message);
            }
        }

        #region Eventos Buscador
        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.EquipoAliado:
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
        #endregion
    }
}