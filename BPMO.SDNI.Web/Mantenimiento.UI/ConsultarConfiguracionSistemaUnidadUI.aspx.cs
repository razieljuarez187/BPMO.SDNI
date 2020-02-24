// Satisface al CU073 - Catálogo Configuración Sistemas Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using System.Configuration;

namespace BPMO.SDNI.Mantenimiento.UI {

    /// <summary>
    /// Clase que contiene los métodos para la presentación de la información de Consulta Configuración Sistema de Unidad Idealease al usuario.
    /// </summary>
    public partial class ConsultarConfiguracionSistemaUnidadUI : System.Web.UI.Page, IConsultarConfiguracionSistemaUnidadVIS {

        #region Atributos

        /// <summary>
        /// Presentador que atiende las peticiones de la vista de Consulta Configuración Sistemas de Unidad Idealease.
        /// </summary>
        private ConsultarConfiguracionSistemaUnidadPRE presentador;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private readonly string nombreClase = typeof(ConsultarConfiguracionSistemaUnidadUI).Name;

            #region Seguridad

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
        /// Obtiene un valor que representa el Identificador de la Unidad Operativa Actual de la sesión en curso.
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

            #endregion

            #region Form Búsqueda

        /// <summary>
        /// Obtiene o establece un valor que representa el Nombre de la Configuración Sistema de Unidad Idealease.
        /// </summary
        public string Nombre {
            get {
                String nombre = null;
                if (this.txtNombre.Text.Trim().Length > 0)
                    nombre = this.txtNombre.Text.Trim().ToUpper();
                return nombre;
            }
            set {
                if (value != null)
                    this.txtNombre.Text = value.ToString();
                else
                    this.txtNombre.Text = String.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la Clave de la Configuración Sistema de Unidad Idealease.
        /// </summary
        public string Clave {
            get {
                String clave = null;
                if (this.txtClave.Text.Trim().Length > 0)
                    clave = this.txtClave.Text.Trim().ToUpper();
                return clave;
            }
            set {
                if (value != null)
                    this.txtClave.Text = value.ToString();
                else
                    this.txtClave.Text = String.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Estado de la Configuración Sistema de Unidad Idealease.
        /// </summary>
        public bool? Activo {
            get {
                bool? activo = null;
                if (ddlActivo.SelectedIndex > 0)
                    activo = Convert.ToBoolean(this.ddlActivo.SelectedValue);
                return activo;
            }
        }

            #endregion

            #region Grid Resultado Búsqueda

        /// <summary>
        /// Obtiene o establece un valor que representa la Lista de Configuraciones de Sistemas de Unidad Idealease.
        /// </summary>
        public List<ConfiguracionSistemaUnidadBO> Configuraciones {
            get { return Session["configuracionesSistemaUnidad"] as List<ConfiguracionSistemaUnidadBO>; }
            set { Session["configuracionesSistemaUnidad"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la Configuración de Sistema de Unidad Idealease.
        /// </summary>
        public ConfiguracionSistemaUnidadBO ConfiguracionSeleccionada {
            get { return Session["configuracionSeleccionada"] as ConfiguracionSistemaUnidadBO; }
            set { Session["configuracionSeleccionada"] = value; }
        }

            #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Método delegado para el evento de carga de la página.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void Page_Load(object sender, EventArgs e) {
            try{
                presentador = new ConsultarConfiguracionSistemaUnidadPRE(this);
                if (!IsPostBack) {
                    presentador.PrepararSeguridad();
                    IniciarCamposSesion();
                }
            } catch(Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + " .Page_Load: " + ex.Message);
            }

            DesplegarListaConfiguraciones();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Crea una nueva instancia de la Configuración Sistema de Unidad Idealease seleccionada y la lista de Configuraciones Sistema de 
        /// Unidad Idealease encontradas.
        /// </summary>
        private void IniciarCamposSesion() {
            if(Session["recargarConfiguraciones"] == null){
                Configuraciones = new List<ConfiguracionSistemaUnidadBO>();
            }else{
                Session["recargarConfiguraciones"] = false;
            }
            ConfiguracionSeleccionada = null;
        }

        /// <summary>
        /// Realiza la redirección a la página de advertencia por falta de permisos.
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        #region Grid Resultado Búsqueda

        /// <summary>
        /// Realiza la vinculación de la lista de Configuraciones Sistema de Unidad Idealease encontradas con la UI.
        /// </summary>
        public void DesplegarListaConfiguraciones() {
            this.gvConfiguracionesSistemaUnidad.DataSource = Configuraciones;
            this.gvConfiguracionesSistemaUnidad.DataBind();
        }

        /// <summary>
        /// Realiza la redirección al visor de Detalle Configuración Sistema de Unidad Idealease.
        /// </summary>
        private void RedirigirADetallesConfiguracionSistemaUnidad() {
            Response.Redirect(ResolveUrl("~/Mantenimiento.UI/DetalleConfiguracionSistemaUnidadUI.aspx"));
        }

        #endregion

        /// <summary>
        /// Despliega un mensaje en pantalla.
        /// </summary>
        /// <param name="mensaje">Mensaje a desplega.r</param>
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

        #endregion

        #region Eventos

            #region Form Búsqueda

        /// <summary>
        /// Realiza la consulta de Configuraciones Sistema de Unidad Idealease. 
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void OnClickBuscarConfiguraciones(object sender, EventArgs e) {
            presentador.BuscarConfiguraciones();
        }

            #endregion

            #region Grid Resultado Búsqueda

        /// <summary>
        /// Despliega las Configuraciones Sistema de Unidad Idealease encontradas en su página correspondiente.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void gvConfiguracionesSistemaUnidad_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try {
                this.gvConfiguracionesSistemaUnidad.PageIndex = e.NewPageIndex;
                DesplegarListaConfiguraciones();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + " .gvConfiguracionesSistemaUnidad_PageIndexChanging: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene la Configuración Sistema de Unidad Idealease seleccionada, carga sus detalles completos y redirecciona al visor
        /// de Detalle Configuración Sistema de Unidad Idealease.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void gvConfiguracionesSistemaUnidad_RowCommand(object sender, GridViewCommandEventArgs e) { 
            int index;

            try {
                if (!string.IsNullOrEmpty(e.CommandArgument.ToString())) {
                    index = Convert.ToInt32(e.CommandArgument.ToString()) + (((GridView)sender).PageIndex * ((GridView)sender).PageSize);
                    ConfiguracionSeleccionada = this.Configuraciones[index];
                    switch(e.CommandName.Trim()) {
                        case "Ver":
                            RedirigirADetallesConfiguracionSistemaUnidad();
                        break;
                    }
                }
            }
            catch (Exception er) { }
        }

            #endregion

        #endregion

    }
}