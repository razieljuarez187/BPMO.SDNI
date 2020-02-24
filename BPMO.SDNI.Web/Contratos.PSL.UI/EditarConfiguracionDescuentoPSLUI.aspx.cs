using System;
using System.Collections.Generic;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class EditarConfiguracionDescuentoPSLUI : System.Web.UI.Page, IEditarConfiguracionDescuentoPSLVIS {
        #region Atributos
        /// <summary>
        /// nombre de la clase para los errores
        /// </summary>
        private string nombreClase = "EditarConfiguracionDescuentoPSLUI";
        /// <summary>
        /// Presentador de editar descuento
        /// </summary>
        private EditarConfiguracionDescuentoPSLPRE presentador;
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene o regresa el ID de la unidad operativa
        /// </summary>
        public int? UnidadOperativaId {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }
        /// <summary>
        /// Obtiene o regresa el usuario autenticado
        /// </summary>
        public int? UsuarioAutenticado {
            get {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id.Value;
                return null;
            }
        }
        /// <summary>
        /// Obtiene o regresa la lista que viene de detalle
        /// </summary>
        public List<ConfiguracionDescuentoBO> ListaDetalle {
            get {
                if (Session["listDetalleConfiguracionDescuentoBO"] == null)
                    return new List<ConfiguracionDescuentoBO>();
                else
                    return (List<ConfiguracionDescuentoBO>)Session["listDetalleConfiguracionDescuentoBO"];
            }
            set {
                Session["listDetalleConfiguracionDescuentoBO"] = value;
            }

        }
        /// <summary>
        /// Obtiene o regresa el ultimo objeto seleccionado
        /// </summary>
        public ConfiguracionDescuentoBO UltimoObjeto {
            get {
                if (Session["UltimoObjetoConfiguracionDescuento"] != null)
                    return (ConfiguracionDescuentoBO)Session["UltimoObjetoConfiguracionDescuento"];

                return null;
            }
            set {
                Session["UltimoObjetoConfiguracionDescuento"] = value;
            }
        }
        /// <summary>
        /// Obtiene o regresa la lista de descuentos actualizada
        /// </summary>
        public List<ConfiguracionDescuentoBO> ListaDescuentos {
            get {
                if (Session["ListaDescuentos"] == null)
                    return new List<ConfiguracionDescuentoBO>();
                else
                    return (List<ConfiguracionDescuentoBO>)Session["ListaDescuentos"];
            }
            set {
                Session["ListaDescuentos"] = value;
            }
        }
        /// <summary>
        /// Obtiene o regresa el ID del modelo
        /// </summary>
        public int? ModeloID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnModeloID.Value.Trim()))
                    id = int.Parse(hdnModeloID.Value.Trim());
                return id;
            }
            set { hdnModeloID.Value = value != null ? value.ToString() : String.Empty; }
        }
        /// <summary>
        /// Obtiene o regresa el ID del descuento
        /// </summary>
        public int? ConfiguracionDescuentoID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnConfiguracionDescuentoID.Value.Trim()))
                    id = int.Parse(hdnConfiguracionDescuentoID.Value.Trim());
                return id;
            }
            set { hdnConfiguracionDescuentoID.Value = value != null ? value.ToString() : String.Empty; }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new EditarConfiguracionDescuentoPSLPRE(this, this.ucConfiguracionDescuentoPSLUI);
                if (!IsPostBack) {
                    presentador.ValidarAcceso();
                    presentador.RealizarPrimeraCarga();
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al crear la página", ETipoMensajeIU.ERROR,
                                    nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método para establecer la variable de sesión
        /// </summary>
        /// <param name="key">Nombre de la variable de la sesión</param>
        /// <param name="value">Valor de la sesión</param>
        public void EstablecerPaqueteNavegacion(string key, object value) {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".EstablecerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            Session[key] = value;
        }
        /// <summary>
        /// Método para obtener paquete de navegación
        /// </summary>
        /// <param name="key">Nombre de la variable de sesión</param>
        /// <returns>Una sesión</returns>
        public object ObtenerPaqueteNavegacion(string key) {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".ObtenerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            return Session[key];
        }
        /// <summary>
        /// Método para limpiar sesión
        /// </summary>
        /// <param name="key">Nombre de la variable de sesión a limpiar</param>
        public void LimpiarPaqueteNavegacion(string key) {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".LimpiarPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            if (Session[key] != null)
                Session.Remove(key);
        }
        /// <summary>
        /// Método para redirigir a detalle
        /// </summary>
        public void RedirigirADetalle() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/DetalleConfiguracionDescuentoPSLUI.aspx"));
        }
        /// <summary>
        /// Método para redirigir a consulta
        /// </summary>
        public void RedirigirAConsulta() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/ConsultarConfiguracionDescuentoPSLUI.aspx"));
        }
        /// <summary>
        /// Método para redirigir a pagina sin acceso
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        /// <summary>
        /// Método para limpiar las sesiones utilizadas
        /// </summary>
        public void LimpiarSesiones() {

            if (Session["ListaDescuentos"] != null)
                Session.Remove("ListaDescuentos");
            if (Session["listDetalleConfiguracionDescuentoBO"] != null)
                Session.Remove("listDetalleConfiguracionDescuentoBO");
            if (Session["UltimoObjetoConfiguracionDescuento"] != null)
                Session.Remove("UltimoObjetoConfiguracionDescuento");
        }
        /// <summary>
        /// Despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los detalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            } else {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Método que se realizara al apretar el botón de cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">evento</param>
        protected void btnCancelar_Click(object sender, EventArgs e) {
            try {
                presentador.Cancelar();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cancelar la edición de la Configuración de descuentos", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click:" + ex.Message);
            }
        }
        /// <summary>
        /// Método que se realizara al apretar el botón de Guardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">evento</param>
        protected void btnGuardar_Click(object sender, EventArgs e) {
            try {
                presentador.GuardarEditar();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al guardar la Configuración de descuentos", ETipoMensajeIU.ERROR, nombreClase + ".btnGuardar_Click:" + ex.Message);
            }
        }

        #endregion
    }
}