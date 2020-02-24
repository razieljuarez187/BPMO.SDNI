// Satisface al CU027 - Catálogo Contacto Cliente
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.MapaSitio.UI;
using System.Configuration;

namespace BPMO.SDNI.Mantenimiento.UI {

    /// <summary>
    /// Clase que contiene los métodos para la presentación de la información de Detalle Contacto Cliente Idealease seleccionado, al usuario.
    /// </summary>
    public partial class DetalleContactoClienteUI : System.Web.UI.Page, IDetalleContactoClienteVIS {

        #region Atributos

        /// <summary>
        /// Presentador que atiende las peticiones de la vista.
        /// </summary>
        public DetalleContactoClientePRE presentador;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private readonly string nombreClase = typeof(DetalleContactoClienteUI).Name;

        /// <summary>
        /// Obtiene o establece un valor que representa el Contacto Cliente Idealease seleccionado
        /// </summary>
        public ContactoClienteBO ContactoClienteSeleccionado {
            get { return Session["contactoClienteSeleccionado"] as ContactoClienteBO; }
            set { Session["contactoClienteSeleccionado"] = value; }
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
                return ucContactoCliente.LibroActivos;
            }
            set {
                ucContactoCliente.LibroActivos = value;
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
        public int? UnidadOperativaId {
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
                this.presentador = new DetalleContactoClientePRE(this);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + " .Page_Load: " + ex.Message);
            }
            if (!IsPostBack) {
                presentador.PrepararSeguridad();
                TipoUI = "Ver";
                Session["recargarAuditorias"] = false;
                ucContactoCliente.SetBloqueoCampos(true);
                if(ContactoClienteSeleccionado != null && ContactoClienteSeleccionado.ContactoClienteID != null){
                    ucContactoCliente.ContactoClienteToForm();
                    mContactoCliente.Items[1].Text = ContactoClienteSeleccionado.Activo.Value ? "Eliminar " : "Reactivar";
                    if (Session["contactoClienteGuardado"] != null) {
                        ucContactoCliente.MostrarMensaje("Guardado con exito", ETipoMensajeIU.EXITO);
                        Session["contactoClienteGuardado"] = null;
                    }
                } else {
                    RedirigirAConsultaContactoCliente();
                }
            }
        }

        #endregion

        #region Métodos

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
        /// Realiza la redirección al visor de Consulta Contacto Cliente Idealease.
        /// </summary>
        public void RedirigirAConsultaContactoCliente() {
            ContactoClienteSeleccionado = new ContactoClienteBO();
            Response.Redirect(ResolveUrl("~/Mantenimiento.UI/ConsultarContactoClienteUI.aspx"));
        }

        /// <summary>
        /// Realiza la redirección al visor de Eliminar Contacto Cliente Idealease.
        /// </summary>
        public void RedirigirAEliminarContactoCliente() {
            Response.Redirect(ResolveUrl("~/Mantenimiento.UI/EliminarContactoClienteUI.aspx"));
        }

        /// <summary>
        /// Realiza la redirección al visor de Editar Contacto Cliente Idealease.
        /// </summary>
        public void RedirigirAEditarContactoCliente() {
            Response.Redirect(("~/Mantenimiento.UI/EditarContactoClienteUI.aspx"));
        }

        /// <summary>
        /// Realiza la redirección a la página de advertencia por falta de permisos.
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Realiza la redirección al visor seleccionado.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void mContactoCliente_MenuItemClick(object sender, MenuEventArgs e) {
            switch (e.Item.Value) {
                case "EliminarContactoCliente":
                    RedirigirAEliminarContactoCliente();
                    break;
                case "EditarContactoCliente":
                    RedirigirAEditarContactoCliente();
                    break;
            }
        }

        /// <summary>
        /// Realiza la redirección al visor de Consulta Contacto Cliente Idealease.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnCancelar_Click(object sender, EventArgs e) {
            RedirigirAConsultaContactoCliente();
        }

        /// <summary>
        /// Realiza la redirección al visor de Editar Contacto Cliente Idealease.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnEditar_Click(object sender, EventArgs e) {
            RedirigirAEditarContactoCliente();
        }

        #endregion
    }
}