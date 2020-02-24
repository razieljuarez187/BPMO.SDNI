// Satisface al CU027 - Catálogo Contacto Cliente
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Comun.BO;
using BPMO.Basicos.BO;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Mantenimiento.VIS;
using System.Configuration;

namespace BPMO.SDNI.Mantenimiento.UI {

    /// <summary>
    /// Clase que contiene los métodos para la presentación de la información del Registro Contacto Cliente Idealease al usuario.
    /// </summary>
    public partial class RegistrarContactoClienteUI : System.Web.UI.Page, IRegistrarContactoClienteVIS {

        #region Atributos

        /// <summary>
        /// Presentador que atiende las peticiones de la vista.
        /// </summary>
        private RegistrarContactoClientePRE presentador;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private readonly string nombreClase = typeof(RegistrarContactoClienteUI).Name;

        /// <summary>
        /// Obtiene o establece un valor que representa el Tipo de UI a desplegar.
        /// </summary>
        public string TipoUI {
            get { return Session["tipoUI"].ToString(); }
            set { Session["tipoUI"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Contacto Cliente Idealease seleccionado.
        /// </summary>
        public ContactoClienteBO ContactoClienteSeleccionado {
            get { return Session["contactoClienteSeleccionado"] as ContactoClienteBO; }
            set { Session["contactoClienteSeleccionado"] = value; }
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

        /// <summary>
        /// Obtiene o establece un valor que representa la Configuración de la Unidad Operativa que indica a que libro corresponden los activos.
        /// </summary>
        public string LibroActivos {
             get {
                string valor = null;
                if (ucContactoCliente.LibroActivos.Length > 0)
                    valor = ucContactoCliente.LibroActivos.ToUpper();
                return valor;

            }
            set {
                if (value != null)
                    this.ucContactoCliente.LibroActivos = value.ToString();
                else
                    this.ucContactoCliente.LibroActivos = string.Empty;
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
                presentador = new RegistrarContactoClientePRE(this);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
            if (!IsPostBack) {
                TipoUI = "Agregar";
                Session["recargarAuditorias"] = false;
                presentador.PrepararSeguridad();
            }
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Reestablece la variable TipoUI.
        /// </summary>
        public void LimpiarDatosSesion() {
            Session["tipoUI"] = null;
        }

        /// <summary>
        /// Realiza la redirección a la página de advertencia por falta de permisos.
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        /// <summary>
        /// Realiza la redirección al visor de Consulta Contacto Cliente Idealease. Crea una nueva instancia del Contacto Cliente 
        /// Idealease seleccionado.
        /// </summary>
        public void RedirigirAConsultaContactoCliente() {
            ContactoClienteSeleccionado = new ContactoClienteBO();
            Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/ConsultarContactoClienteUI.aspx"));
        }

        /// <summary>
        /// Realiza la redirección al visor de Eliminar Contacto Cliente Idealease.
        /// </summary>
        public void RedirigirAEliminarContactoCliente() {
            Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/EliminarContactoClienteUI.aspx"));
        }

        /// <summary>
        /// Realiza la redirección al visor de Edición Contacto Cliente Idealease.
        /// </summary>
        public void RedirigirAEditarContactoCliente() {
            Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/EditarContactoClienteUI.aspx"));
        }

        /// <summary>
        /// Realiza la redirección al visor de Detalle Contacto Cliente Idealease.
        /// </summary>
        public void RedirigirADetalleContactoCliente() {
            Response.Redirect(ResolveUrl("~/Mantenimiento.UI/DetalleContactoClienteUI.aspx"));
        }

        /// <summary>
        /// Estable la variable de Sesión ContactoClienteGuardado como True si se registro con éxito.
        /// </summary>
        public void setGuardadoExitoso() {
            Session["contactoClienteGuardado"] = true;
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

        #endregion

        #region Eventos

        /// <summary>
        /// Realiza la redirección al visor seleccionado.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void mContactoCliente_MenuItemClick(object sender, MenuEventArgs e) {
            presentador.Redirigir(e.Item.Value);
        }

        /// <summary>
        /// Realiza el registro del Contacto Cliente Idealease. Valida los campos obligatorios.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnFinalizar_Click(object sender, EventArgs e) {
            ucContactoCliente.FormToContactoCliente();
            if(ucContactoCliente.ValidarForm()){
                return;
            }
            presentador.Guardar();
        }

        /// <summary>
        /// Realiza la redirección al visor de Consulta Contacto Cliente Idealease.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnCancelar_Click(object sender, EventArgs e) {
            RedirigirAConsultaContactoCliente();
        }

        #endregion

    }
}