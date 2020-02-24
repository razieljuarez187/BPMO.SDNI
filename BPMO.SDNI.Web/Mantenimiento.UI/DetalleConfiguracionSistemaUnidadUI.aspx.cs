// Satisface al CU073 - Catálogo Configuración Sistemas Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.MapaSitio.UI;
using System.Configuration;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Mantenimiento.UI {

    /// <summary>
    /// Clase que contiene los métodos para la presentación de la información del Detalle Configuración Sistema de Unidad Idealease 
    /// seleccionado, al usuario.
    /// </summary
    public partial class DetalleConfiguracionSistemaUnidadUI : System.Web.UI.Page, IDetalleConfiguracionSistemaUnidadVIS {

        #region Atributos

        /// <summary>
        /// Presentador que atiende las peticiones de la vista Detalle Configuración Sistema de Unidad Idealease.
        /// </summary>
        private DetalleConfiguracionSistemaUnidadPRE presentador;

        /// <summary>
        /// Obtiene o establece un valor que representa la Configuración Sistema de Unidad Idealease seleccionada.
        /// </summary>
        public ConfiguracionSistemaUnidadBO ConfiguracionSeleccionada {
            get { return Session["configuracionSeleccionada"] as ConfiguracionSistemaUnidadBO; }
            set { Session["configuracionSeleccionada"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Tipo de UI a desplegar.
        /// </summary>
        public String TipoUI {
            get { return Session["tipoUI"].ToString(); }
            set { Session["tipoUI"] = value; }
        }

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
                if (ucConfSistemaUnidad.LibroActivos.Length > 0)
                    valor = ucConfSistemaUnidad.LibroActivos.ToUpper();
                return valor;

            }
            set {
                if (value != null)
                    this.ucConfSistemaUnidad.LibroActivos = value.ToString();
                else
                    this.ucConfSistemaUnidad.LibroActivos = string.Empty;
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

        #endregion

        #region Constructor

        /// <summary>
        /// Método delegado para el evento de carga de la página.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new DetalleConfiguracionSistemaUnidadPRE(this);
            } catch(Exception ex){ }
            if (!IsPostBack) {
                Session["recargarConfiguraciones"] = false;
                presentador.PrepararSeguridad();
                if(ConfiguracionSeleccionada != null && ConfiguracionSeleccionada.ConfiguracionSistemaUnidadId != null){
                    IniciarCampos();
                }else{
                    RedirigirAConsultaConfiguracionSistemaUnidad();
                }
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carga los detalles de la Configuración Sistema de Unidad Idealease, establece el Tipo de UI.
        /// </summary>
        private void IniciarCampos() {
            TipoUI = "Detalle";
            ucConfSistemaUnidad.BloquearCampos(true);
            ucConfSistemaUnidad.ConfiguracionSistemaUnidadToForm();
            mConfiguracion.Items[1].Text = ConfiguracionSeleccionada.Activo.Value ? "Eliminar " : "Reactivar";
        }

        /// <summary>
        /// Realiza la redirección a la página de advertencia por falta de permisos.
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        /// <summary>
        /// Realiza la redirección al visor de Eliminación Configuración Sistema de Unidad Idealease.
        /// </summary>
        private void RedirigirAEliminarConfiguracionSistemaUnidad() {
            Response.Redirect(ResolveUrl("~/Mantenimiento.UI/EliminarConfiguracionSistemaUnidadUI.aspx"));
        }

        /// <summary>
        /// Realiza la redirección al visor de Edición Configuración Sistema de Unidad Idealease.
        /// </summary>
        private void RedirigirAEditarConfiguracionSistemaUnidad() {
            Response.Redirect(ResolveUrl("~/Mantenimiento.UI/EditarConfiguracionSistemaUnidadUI.aspx"));
        }

        /// <summary>
        /// Realiza la redirección al visor de Consulta Configuraciones Sistemas de Unidad Idealease.
        /// </summary>
        private void RedirigirAConsultaConfiguracionSistemaUnidad() {
            Response.Redirect(ResolveUrl("~/Mantenimiento.UI/ConsultarConfiguracionSistemaUnidadUI.aspx"));
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Realiza la redirección al visor seleccionado.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void mConfiguracion_MenuItemClick(object sender, MenuEventArgs e) {
            switch (e.Item.Value) {
                case "EliminarConfiguracion":
                    RedirigirAEliminarConfiguracionSistemaUnidad();
                    break;
                case "EditarConfiguracion":
                    RedirigirAEditarConfiguracionSistemaUnidad();
                    break;
            }
        }

        /// <summary>
        /// Realiza la redirección al visor de Consulta Configuraciones Sistemas de Unidad Idealease.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnCancelar_Click(object sender, EventArgs e) {
            ConfiguracionSeleccionada = new ConfiguracionSistemaUnidadBO();
            RedirigirAConsultaConfiguracionSistemaUnidad();
        }

        /// <summary>
        /// Realiza la redirección al visor de Edición Configuración Sistema de Unidad Idealease.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnEditar_Click(object sender, EventArgs e) {
            RedirigirAEditarConfiguracionSistemaUnidad();
        }

        #endregion
    }
}