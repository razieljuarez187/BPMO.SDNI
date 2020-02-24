// Satisface al CU064 - Enviar Correo Servicio Realizado
using System;
using System.Collections.Generic;
using System.Web.UI;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.SDNI.Mantenimiento.BO.BOF;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using System.Web.UI.WebControls;
using BPMO.Facade.SDNI.BO;
using BPMO.Servicio.Catalogos.BO;
using System.Configuration;

namespace BPMO.SDNI.Mantenimiento.UI {

    /// <summary>
    /// Clase que contiene los métodos para la presentación de la información de Envío de Correo de Servicio Realizado, al usuario.
    /// </summary>
    public partial class EnviarCorreoServicioRealizadoUI : System.Web.UI.Page, IEnviarCorreoServicioRealizadoVIS {

        #region Atributos

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

        /// <summary>
        /// Presentador que atiende las peticiones de la vista de Envío de Correo de Servicio Realizado.
        /// </summary>
        private EnviarCorreoServicioRealizadoPRE presentador;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private readonly string nombreClase = typeof(EnviarCorreoServicioRealizadoUI).Name;

        /// <summary>
        /// Obtiene o establece un valor que representa el Mantenimiento Seleccionado.
        /// </summary>
        public MantenimientoBOF Mantenimiento {
            get { return Session["mantenimientoBOF"] as MantenimientoBOF; }
            set { Session["mantenimientoBOF"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que reprenta el Diccionario de datos de la información a enviar.
        /// </summary>
        public Dictionary<string, string> Datos {
            get { return Session["datosCorreoServicioRealizado"] as Dictionary<string, string>; }
            set { Session["datosCorreoServicioRealizado"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la Lista de Contactos Clientes disponibles para recibir el Correo.
        /// </summary>
        public List<DetalleContactoClienteBO> ContactosCliente {
            get { return Session["contactosCliente"] as List<DetalleContactoClienteBO>; }
            set { Session["contactosCliente"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Contacto Cliente disponible para recibir Correos.
        /// </summary>
        public DetalleContactoClienteBO ContactoClienteSeleccionado {
            get { return Session["contactoClienteSeleccionado"] as DetalleContactoClienteBO; }
            set { Session["contactoClienteSeleccionado"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Taller del servicio.
        /// </summary>
        public TallerBO Taller {
            get { return Session["taller"] as TallerBO; }
            set { Session["taller"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Título del Mensaje a enviar.
        /// </summary>
        public string TituloMensaje {
            get {
                String tituloMensaje = null;
                if (this.txtMensajeInicio.Text.Trim().Length > 0)
                    tituloMensaje = this.txtMensajeInicio.Text.Trim().ToUpper();
                return tituloMensaje;
            }
            set {
                if (value != null)
                    this.txtMensajeInicio.Text = value.ToString();
                else
                    this.txtMensajeInicio.Text = String.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Cuerpo del Mensaje a enviar.
        /// </summary>
        public string CuerpoMensaje {
            get {
                String mensaje = null;
                if (this.txtMensaje.Text.Trim().Length > 0)
                    mensaje = this.txtMensaje.Text.Trim().ToUpper();
                return mensaje;
            }
            set {
                if (value != null)
                    this.txtMensaje.Text = value.ToString();
                else
                    this.txtMensaje.Text = String.Empty;
            }
        }

        /// <summary>
        /// Obtiene la ruta del sistema.
        /// </summary>
        public string RootPath
        {
            get
            {
                return this.Server.MapPath("~");
            }
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Nombre de reporte que se envía como parámetro en la URL
        /// (para no sobrescribir datos de sesión)
        /// </summary>
        public bool ImprimirPendientes {
            get {
                bool aux = false;
                if (Request.QueryString["p"] != null)
                    if (bool.TryParse(Request.QueryString["p"], out aux)) return aux;
                    else return false;
                else
                    return false;
            }
        }

        /// <summary>
        /// Lista de Tareas Pendientes de
        /// </summary>
        public List<TareaPendienteBO> ListaTareasPendientes {
            get {
                return (Session["lstTareasPendientes"] == null) ? null : (List<TareaPendienteBO>)Session["lstTareasPendientes"];
            }
            set {
                if (value != null) Session["lstTareasPendientes"] = value;
                else Session.Remove("lstTareasPendientes");
            }
        }
        #endregion /Propiedades

        #region Constructor

        /// <summary>
        /// Método delegado para el evento de carga de la página.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new EnviarCorreoServicioRealizadoPRE(this);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + " .Page_Load: " + ex.Message);
            }
            if (!IsPostBack) {
                IniciarCampos();
                presentador.PrepararSeguridad();
                try {
                    if (Mantenimiento != null && (Mantenimiento.MantenimientoUnidad != null || Mantenimiento.MantenimientoAliado != null)) {
                        presentador.CargarDatosMensaje();
                    } else {
                        RedireccionarARegistrarUnidad();
                    }
                } catch(Exception ex) {
                    throw new Exception(ex.Message);
                }
            }
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

        /// <summary>
        /// Crea una nueva instancia de la lista Contactos Cliente, el Contacto Cliente seleccionado y el diccionario de datos.
        /// </summary>
        private void IniciarCampos() {
            ContactosCliente = new List<DetalleContactoClienteBO>();
            ContactoClienteSeleccionado = new DetalleContactoClienteBO();
            Datos = new Dictionary<string, string>();
        }

        /// <summary>
        /// Realiza la redirección al visor de Registro de Mantenimientos Idealease.
        /// </summary>
        public void RedireccionarARegistrarUnidad(){
            Response.Redirect(ResolveUrl("~/Mantenimiento.UI/RegistrarUnidadUI.aspx"));
        }

        /// <summary>
        /// Despliega la información del Mantenimiento Unidad Idealease realizado así como su Mantenimiento 
        /// Programado.
        /// </summary>
        public void CargarDatosUnidad() {
            bool cargarDatos = false;
            if (ValidarLlave("unidad") && ValidarLlave("tipoProximoServicio") && ValidarLlave("fechaProximoServicio")) {
                txtUnidad.Text = Datos["unidad"];
                txtProximoServicio.Text = Datos["tipoProximoServicio"];
                txtFechaProximoServicio.Text = Datos["fechaProximoServicio"];
                txtKilometraje.Text = Datos["kilometraje"];
                txtHoras.Text = Datos["horas"];
                cargarDatos = true;
                ImpresionSaltoDeLinea(txtMensajeInicio, Datos.ContainsKey("mensajeInicio") && Datos["mensajeInicio"] != null ? Datos["mensajeInicio"] : "");
                ImpresionSaltoDeLinea(txtMensaje, Datos["mensaje"]);
            }
            txtMensaje.Enabled = cargarDatos;
            txtMensaje.ReadOnly = !cargarDatos;
            txtMensajeInicio.Enabled = cargarDatos;
            txtMensajeInicio.ReadOnly = !cargarDatos;
            if (!cargarDatos) {
                MostrarMensaje("No se encontraron datos para enviar el Correo de Servicio Realizado.", ETipoMensajeIU.ERROR);
                btnEnviar.Enabled = true;
            }
        }

        /// <summary>
        /// Verifica que exista la Llave en el Dicccionario de Datos.
        /// </summary>
        /// <param name="llave">La llave a buscar.</param>
        /// <returns>Retorna True si encontro la Llave, en caso contrario retorna False.</returns>
        private bool ValidarLlave(string llave) {
            return Datos.ContainsKey(llave) && Datos[llave] != null;
        }

        /// <summary>
        /// Realiza la impresión de los saltos de linea del Cuerpo del Mensaje a enviar.
        /// </summary>
        /// <param name="campo">El campo del Cuerpo de Mensaje.</param>
        /// <param name="texto">El texto a mostrar.</param>
        private void ImpresionSaltoDeLinea(TextBox campo, string texto) { 
            string[] lineas = texto.Split(new String[]{"\\n"}, StringSplitOptions.None);
            for(int i = 0; i < lineas.Length; i++){
                string linea = lineas[i];
                campo.Text += linea;
                if((i + 1) != lineas.Length){
                    campo.Text += Environment.NewLine;
                    campo.Text += Environment.NewLine;
                }
            }
        }

        /// <summary>
        /// Despliega la información del Contacto Cliente configurado para recibir correos.
        /// </summary>
        public void CargarDatosContactoCliente() {
            txtNombreContacto.Text = ContactoClienteSeleccionado.Nombre != null ? ContactoClienteSeleccionado.Nombre : "";
            txtTelefono.Text = ContactoClienteSeleccionado.Telefono != null ? ContactoClienteSeleccionado.Telefono : "";
            txtEmail.Text = ContactoClienteSeleccionado.Correo != null ? ContactoClienteSeleccionado.Correo : "";
        }
        
        /// <summary>
        /// Despliega un mensaje en pantalla.
        /// </summary>
        /// <param name="mensaje">Mensaje a desplega.r</param>
        /// <param name="tipo">Tipo de mensaje a desplegar.</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar.</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            Site masterMsj = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo, detalle);
            } else {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Realiza la redirección al visor de Registro de Mantenimientos Idealease.
        /// </summary>
        protected void btnCancelar_Click(object sender, EventArgs e) {
            RedireccionarARegistrarUnidad();
        }

        /// <summary>
        /// Realiza el envío del Correo a la lista de Contactos Cliente.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnFinalizar_Click(object sender, EventArgs e) {
            if(ValidarLlave("unidad") && ValidarLlave("tipoProximoServicio") && ValidarLlave("fechaProximoServicio")) {
                presentador.EnviarCorreo();
            } else {
                MostrarMensaje("Error al intentar enviar el Correo de Servicio Realizado", ETipoMensajeIU.ERROR, "No se encontró información para enviar el Correo de Servicio Realizado");  
            }
        }

        #endregion

    }
}