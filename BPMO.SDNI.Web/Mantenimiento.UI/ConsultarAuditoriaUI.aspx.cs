//Satisface al CU072 - Obtener Auditoría
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Servicio.Catalogos.BO;
using BPMO.Basicos.BO;
using System.Configuration;

namespace BPMO.SDNI.Mantenimiento.UI {

    /// <summary>
    /// Clase que contiene los métodos para la presentación de la información al usuario.
    /// </summary>
    public partial class ConsultarAuditoriaUI : System.Web.UI.Page, IConsultarAuditoriaVIS {

        #region Propiedades

        /// <summary>
        /// Presentador que atiende las peticiones de la vista.
        /// </summary>
        private ConsultarAuditoriaPRE presentador;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private readonly string nombreClase = typeof(ConsultarAuditoriaUI).Name;

        /// <summary>
        /// Obtiene o establece un valor que representa la Auditoría de Mantenimiento Idealease a buscar.
        /// </summary>
        public AuditoriaMantenimientoBO AuditoriaSeleccionada {
            get { return (AuditoriaMantenimientoBO)Session["auditoriaSeleccionada"]; }
            set { Session["auditoriaSeleccionada"] = value; }
        }

            #region Form Búsqueda

                #region Seguridad

        /// <summary>
        /// Obtiene un valor que representa el identificador del Usuario Actual de la sesión en curso.
        /// </summary>
        public int? UsuarioAutenticado {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el identificador de la Unidad Operativa Actual de la sesión en curso.
        /// </summary>
        public int? UnidadOperativaID {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la Configuración de la Unidad Operativa que indica a que libro corresponden los activos.
        /// </summary>
        public string LibroActivos {
            get {
                string valor = null;
                if (this.hdnLibroActivos.Value.Trim().Length > 0)
                    valor = this.hdnLibroActivos.Value.Trim().ToUpper();
                return valor;
            }
            set {
                if (value != null)
                    this.hdnLibroActivos.Value = value.ToString();
                else
                    this.hdnLibroActivos.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el Identificador del Módulo desde el cual se está accesando.
        /// </summary>
        public int? ModuloID {
            get {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                    return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);

                return null;
            }
        }
                #endregion

                #region Buscador

        /// <summary>
        /// Enumerador de Catálogos para el Buscador.
        /// </summary>
        public enum ECatalogoBuscador {
            Sucursal,
            Tecnico
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Enumerador que contiene los buscadores existentes en la UI.
        /// </summary>
        public ECatalogoBuscador ViewState_Catalogo {
            get { return (ECatalogoBuscador)ViewState["BUSQUEDA"]; }
            set { ViewState["BUSQUEDA"] = value; }
        }

        /// <summary>
        /// Obtiene un valor que representa un identificador único para la UI.
        /// </summary>
        public string ViewState_Guid {
            get {
                if (ViewState["GuidSession"] == null) {
                    Guid guid = Guid.NewGuid();
                    ViewState["GuidSession"] = guid.ToString();
                }
                return ViewState["GuidSession"].ToString();
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Objeto con la información de filtrado para el buscador.
        /// </summary>
        protected object Session_ObjetoBuscador {
            get {
                object objeto = null;
                if (Session[ViewState_Guid] != null)
                    objeto = (Session[ViewState_Guid] as object);

                return objeto;
            }
            set {
                if (value != null)
                    Session[ViewState_Guid] = value;
                else
                    Session.Remove(ViewState_Guid);
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Objeto que fue seleccionado del buscador.
        /// </summary>
        protected object Session_BOSelecto {
            get {
                object objeto = null;
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (Session[nombreSession] != null)
                    objeto = (Session[nombreSession] as object);

                return objeto;
            }
            set {
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (value != null)
                    Session[nombreSession] = value;
                else
                    Session.Remove(nombreSession);
            }
        }

                #endregion

                #region Filtro Sucursal

        /// <summary>
        /// Obtiene o establece un valor que representa la Sucursal seleccionada.
        /// </summary>
        public SucursalBO SucursalSeleccionada {
            get { return (SucursalBO)Session["sucursalSeleccionado"]; }
            set { Session["sucursalSeleccionado"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de la Sucursal seleccionada.
        /// </summary>
        public int? SucursalID {
            get {
                int? sucursalID = null;
                if (!String.IsNullOrEmpty(this.hdnSucursalID.Value))
                    sucursalID = int.Parse(this.hdnSucursalID.Value.Trim());
                return sucursalID;
            }
            set {
                if (value != null)
                    this.hdnSucursalID.Value = value.ToString();
                else
                    this.hdnSucursalID.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o estable el nombre de la Sucursal a buscar.
        /// </summary>
        public string NombreSucursal {
            get {
                String sucursalNombre = null;
                if (this.txtSucursal.Text.Trim().Length > 0)
                    sucursalNombre = this.txtSucursal.Text.Trim().ToUpper();
                return sucursalNombre;
            }
            set {
                if (value != null)
                    this.txtSucursal.Text = value.ToString();
                else
                    this.txtSucursal.Text = String.Empty;
            }
        }

                #endregion

                #region Filtro Técnico

        /// <summary>
        /// Obtiene o establece un valor que representa un Técnico seleccionado.
        /// </summary>
        public TecnicoBO TecnicoSeleccionado {
            get { return (TecnicoBO) Session["tecnicoSeleccionado"]; }
            set { Session["tecnicoSeleccionado"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre del Técnico a buscar.
        /// </summary>
        public String NombreTecnico { 
            get {
                String nombreTecnico = null;
                if (this.txtNombreTecnico.Text.Trim().Length > 0)
                    nombreTecnico = this.txtNombreTecnico.Text.Trim().ToUpper();
                return nombreTecnico;
            }
            set {
                if (value != null)
                    this.txtNombreTecnico.Text = value.ToString();
                else
                    this.txtNombreTecnico.Text = String.Empty;
            }
        }

                #endregion

        /// <summary>
        /// Obtiene o establece un valor que representa el tipo de Paquete de Mantenimiento a buscar.
        /// </summary>
        public ETipoMantenimiento? TipoMantenimiento { 
            get{
                int valorSeleccionado = Int32.Parse(ddTipoMantenimiento.SelectedItem.Value.ToString());
                if(valorSeleccionado > 0){
                    switch (valorSeleccionado) { 
                        case 1:
                            return ETipoMantenimiento.PMA;
                        case 2:
                            return ETipoMantenimiento.PMB;
                        case 3:
                            return ETipoMantenimiento.PMC;
                    }
                }
                return null;
            }
            set {
                if(value != null && Int32.Parse(ddTipoMantenimiento.SelectedValue) > 0){
                    int index = Int32.Parse(value.Value.ToString());
                    ddTipoMantenimiento.SelectedIndex = index;
                } else {
                    ddTipoMantenimiento.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de la Orden de Servicio a buscar.
        /// </summary>
        public int? OrdenServicioID { 
            get {
                String uiOrdenServicioId = this.txtNumeroOrdenServicio.Text;
                if (!String.IsNullOrEmpty(uiOrdenServicioId.Trim())) {
                    return int.Parse(uiOrdenServicioId);
                }

                return null;
            }
            set {
                if (value != null) {
                    this.txtNumeroOrdenServicio.Text = value.ToString();
                } else {
                    this.txtNumeroOrdenServicio.Text = String.Empty;
                }
            } 
        }

            #endregion

            #region Grid Resultado Búsqueda

        /// <summary>
        /// Obtiene o establece un valor que representa una lista de Auditorías de Mantenimientos Idealease encontradas.
        /// </summary>
        public List<AuditoriaMantenimientoBO> Auditorias {
            get { return Session["listaAuditorias"] as List<AuditoriaMantenimientoBO>; }
            set { Session["listaAuditorias"] = value; }
        }

            #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Método delegado para el evento de carga de la página.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento</param>
        /// <param name="e">Argumento asociado al evento</param>
        protected void Page_Load(object sender, EventArgs e) {
            try {
                this.presentador = new ConsultarAuditoriaPRE(this);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }

            if (!IsPostBack) {
                presentador.PrepararSeguridad();
                IniciarCampos();
                Session["recargarAuditorias"] = null;
            }
            DesplegarListaAuditorias();
        }

        #endregion
        
        #region Métodos

            #region Seguridad

        /// <summary>
        /// Realiza la redirección a la página de advertencia por falta de permisos.
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

            #endregion

            #region Form Búsqueda

                #region Buscador

        /// <summary>
        /// Ejecuta el buscador.
        /// </summary>
        /// <param name="catalogo">Nombre del xml.</param>
        /// <param name="catalogoBusqueda">Nombre del catálogo.</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda) {
            try {
                ViewState_Catalogo = catalogoBusqueda;
                this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
                this.Session_BOSelecto = null;
                this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
            } catch (Exception ex) {
                this.MostrarMensaje("Error al consultar unidades", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        /// <summary>
        /// Despliega la información del Objeto seleccionado.
        /// </summary>
        /// <param name="catalogo">Nombre del catálogo.</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo) {
            this.presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;
        }

                #endregion

                #region Filtro Sucursal

        /// <summary>
        /// Activa el llamado al Buscador de Sucursales.
        /// </summary>
        /// <param name="campo">Nombre del campo que desencadenó el evento.</param>
        private void DesplegarFormBusquedaSucursal(string campo){
            try {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + campo + ex.Message);
            }
        }

                #endregion

                #region Filtro Técnico

        /// <summary>
        /// Activa el llamado al Buscador de Técnicos.
        /// </summary>
        /// <param name="campo">Nombre del campo que desencadenó el evento.</param>
        private void DesplegarFormBusquedaTecnico(string campo) { 
            try {
                this.EjecutaBuscador("Tecnico&hidden=0", ECatalogoBuscador.Tecnico);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar un Técnico", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + campo + ex.Message);
            }
        }

                #endregion

            #endregion

        /// <summary>
        /// Crea una nueva instancia de la lista de Auditorías de Mantenimientos Idealease encontradas y la Auditoría de Mantenimiento Idealease seleccionada.
        /// </summary>
        private void IniciarCampos() {
            if(Session["recargarAuditorias"] == null){
                Auditorias = new List<AuditoriaMantenimientoBO>();
            }
            AuditoriaSeleccionada = new AuditoriaMantenimientoBO();
        }

        /// <summary>
        /// Despliega un mensaje en pantalla.
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar.</param>
        /// <param name="tipo">Tipo de mensaje a desplegar.</param>
        /// <param name="msjDetalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar.</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null) {
            Site masterMsj = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
            } else {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// Registra un Script en el cliente.
        /// </summary>
        /// <param name="key">Llave del Script.</param>
        /// <param name="script">Script a registrar.</param>
        private void RegistrarScript(string key, string script) {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        /// <summary>
        /// Método que verifica la selección de una Sucursal o Técnico. Devuelve true si encontró alguna inconsistencia, 
        /// retorna false en caso contrario.
        /// </summary>
        /// <returns>Objeto de tipo boolean</returns>
        private bool ValidarBusqueda() { 
            if (NombreSucursal != null && NombreSucursal.Trim() != null) {
                if (SucursalSeleccionada == null || SucursalSeleccionada.Id == null) {
                    MostrarMensaje("Debes seleccionar una Sucursal", ETipoMensajeIU.ADVERTENCIA);
                    return true;
                } else if(!NombreSucursal.Equals(SucursalSeleccionada.Nombre)){
                    MostrarMensaje("Debes seleccionar una Sucursal", ETipoMensajeIU.ADVERTENCIA);
                    return true;
                }
            } else if(SucursalID != null && SucursalID > 0){
                SucursalID = null;
            }
            if (NombreTecnico != null && NombreTecnico.Trim() != null) {
                if (TecnicoSeleccionado == null || TecnicoSeleccionado.Id == null) {
                    MostrarMensaje("Debes seleccionar un Técnico", ETipoMensajeIU.ADVERTENCIA);
                    return true;
                }else if(!NombreTecnico.Equals(TecnicoSeleccionado.Empleado.NombreCompleto)){
                    MostrarMensaje("Debes seleccionar un Técnico", ETipoMensajeIU.ADVERTENCIA);
                    return true;
                }
            } else if(TecnicoSeleccionado != null){
                TecnicoSeleccionado = null;
            }
            
            return false;
        }

            #region Grid Resultado Búsqueda

        /// <summary>
        /// Realiza la vinculación de la lista de Auditorías de Mantenimientos Idealease encontrados con la UI.
        /// </summary>
        public void DesplegarListaAuditorias() {
            this.gvAuditorias.DataSource = this.Auditorias;
            this.gvAuditorias.DataBind();
        }
        
        /// <summary>
        /// Realiza la redirección al visor de Detalle de Auditoría de Mantenimiento Idealease.
        /// </summary>
        private void RedirigirAVerAuditoria() { 
            this.Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/DetalleAuditoria.aspx"));
        }

            #endregion
        
        #endregion

        #region Eventos

            #region Form Búsqueda

                #region Buscador

        /// <summary>
        /// Establece el Objeto Seleccionado del Buscador.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnResult_Click(object sender, EventArgs e) {
            try {
                switch (ViewState_Catalogo) {
                    case ECatalogoBuscador.Sucursal:
                    case ECatalogoBuscador.Tecnico:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click" + ex.Message);
            }
        }

                #endregion

                #region Filtro Sucursal

        /// <summary>
        /// Evento que activa el llamado al Buscador de Sucursales.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnBuscarSucursal_Click(object sender, ImageClickEventArgs e) {
            DesplegarFormBusquedaSucursal(" .btnBuscarSucursal_Click");
        }

        /// <summary>
        /// Evento que activa el llamado al Buscador de Sucursales.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void txtSucursal_TextChanged(object sender, EventArgs e) { 
            DesplegarFormBusquedaSucursal(" .txtSucursal_TextChanged ");
        }

                #endregion

                #region Filtro Técnicos

        /// <summary>
        /// Evento que activa el llamado al Buscador de Técnicos.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnBuscarTecnicos_Click(object sender, ImageClickEventArgs e) {
            DesplegarFormBusquedaTecnico(" .btnBuscarTecnicos_Click ");
        }

        /// <summary>
        /// Evento que activa el llamado al Buscador de Técnicos.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void txtNombreTecnico_TextChanged(object sender, EventArgs e) {
            DesplegarFormBusquedaTecnico(" .txtNombreTecnico_TextChanged ");
        }

                #endregion

        /// <summary>
        /// Realiza la consulta de Auditorías de Mantenimientos Idealease.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void BuscarAuditorias_Click(object sender, EventArgs e) {
            if(ValidarBusqueda()){
                return;
            }
            Auditorias = new List<AuditoriaMantenimientoBO>();
            presentador.MostrarDatosInterfaz();
        }

            #endregion

            #region Grid Resultado Búsqueda

        /// <summary>
        /// Despliega las Auditorías de Mantenimientos Idealease encontradas en su página correspondiente.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void gvAuditorias_PageIndexChanging(Object sender, GridViewPageEventArgs e) {
            try {
                gvAuditorias.PageIndex = e.NewPageIndex;
                DesplegarListaAuditorias();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".gvAuditorias_PageIndexChanging:" + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene la Auditoría Mantenimiento Idealease seleccionada, carga sus detalles completos y redirecciona al visor de Detalle 
        /// Auditoría Mantenimiento Idealease.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void gvAuditorias_RowCommand(Object sender, GridViewCommandEventArgs e) {
            int index;

            try {
                if (!string.IsNullOrEmpty(e.CommandArgument.ToString())) {
                    index = Convert.ToInt32(e.CommandArgument.ToString()) + (((GridView)sender).PageIndex * ((GridView)sender).PageSize);
                    if (e.CommandName.Trim().Equals("Ver")) {
                       bool encontrada = presentador.VerAuditoriaCompleta(Auditorias[index]);
                        if(encontrada) {
                            RedirigirAVerAuditoria();
                        }
                    }
                }
            } catch (Exception er) { }
        }

            #endregion

        #endregion

    }
}